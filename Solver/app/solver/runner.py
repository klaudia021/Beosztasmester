from collections.abc import Callable
from concurrent.futures import ThreadPoolExecutor
from copy import deepcopy
from threading import RLock

from app.solver.run import SolverRun
from app.solver.snapshot import SolverRunSnapshot
from app.solver.states import SolverRunState

TERMINAL_STATES = frozenset(
    {
        SolverRunState.COMPLETED,
        SolverRunState.CANCELLED,
        SolverRunState.TIMED_OUT,
        SolverRunState.INFEASIBLE,
        SolverRunState.FAILED,
    }
)


class DuplicateActiveRunError(RuntimeError):
    pass


class SolverRunner:
    def __init__(self) -> None:
        self._lock = RLock()
        self._runs: dict[int, SolverRun] = {}
        self._run_context: dict[int, tuple[int, str, int]] = {}
        self._executor = ThreadPoolExecutor(max_workers=1)
        self._scheduled: set[int] = set()
        self._results: dict[int, list[dict[str, int | str]]] = {}


    def submit(
        self, horizon_id: int, engine_type: str, instance_id: int,
    ) -> int:

        if horizon_id <= 0:
            raise ValueError("horizon_id must be positive")

        if not engine_type:
            raise ValueError("engine_type must not be empty")

        if instance_id <= 0:
            raise ValueError("instance_id must be positive")

        with self._lock:
            for run_id, (registered_horizon, registered_engine, _) in (
                self._run_context.items()
            ):
                if (
                    registered_horizon == horizon_id
                    and registered_engine == engine_type
                    and self._runs[run_id].state not in TERMINAL_STATES
                ):
                    raise DuplicateActiveRunError(
                        f"Active run already exists: {run_id}"
                    )

            run_id = instance_id

            if run_id in self._runs:
                raise RuntimeError(f"Duplicate backend run ID: {run_id}")

            run = SolverRun(run_id)

            self._runs[run_id] = run
            self._run_context[run_id] = (
                horizon_id,
                engine_type,
                instance_id,
            )

            return run_id

    def _make_snapshot(self, run_id: int) -> SolverRunSnapshot:
        run = self._runs[run_id]
        horizon_id, engine_type, instance_id = self._run_context[run_id]

        return SolverRunSnapshot(
            run_id=run.run_id,
            horizon_id=horizon_id,
            instance_id=instance_id,
            engine_type=engine_type,
            state=run.state,
            accepted_at=run.accepted_at,
            started_at=run.started_at,
            finished_at=run.finished_at,
        )

    def get_run(self, run_id: int) -> SolverRunSnapshot | None:
        with self._lock:
            if run_id not in self._runs:
                return None

            return self._make_snapshot(run_id)

    def get_run_with_result(
        self,
        run_id: int,
    ) -> tuple[SolverRunSnapshot, list[dict[str, int | str]] | None] | None:
        with self._lock:
            if run_id not in self._runs:
                return None

            snapshot = self._make_snapshot(run_id)
            result = self._results.get(run_id)

            return (
                snapshot,
                deepcopy(result) if result is not None else None,
            )

    def schedule(
        self,
        run_id: int,
        engine: Callable[[], list[dict[str, int | str]]],
    ) -> None:
        with self._lock:
            if run_id not in self._runs:
                raise KeyError(f"Unknown run ID: {run_id}")

            if self._runs[run_id].state is not SolverRunState.ACCEPTED:
                raise RuntimeError(f"Run cannot be scheduled: {run_id}")

            if run_id in self._scheduled:
                raise RuntimeError(f"Run already scheduled: {run_id}")

            self._scheduled.add(run_id)

            try:
                self._executor.submit(self._execute, run_id, engine)
            except RuntimeError:
                self._scheduled.remove(run_id)
                raise

    def _execute(
        self,
        run_id: int,
        engine: Callable[[], list[dict[str, int | str]]],
    ) -> None:
        with self._lock:
            self._runs[run_id].start()

        try:
            result = engine()
        except Exception:
            with self._lock:
                self._runs[run_id].fail()
            return

        with self._lock:
            self._results[run_id] = deepcopy(result)
            self._runs[run_id].complete()

    def close(self) -> None:
        self._executor.shutdown(wait=True)
