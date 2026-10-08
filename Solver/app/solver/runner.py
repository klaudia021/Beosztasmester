from uuid import uuid4

from app.solver.run import SolverRun


class SolverRunner:
    def __init__(self) -> None:
        self._runs: dict[str, SolverRun] = {}

    def _register_run(self) -> str:
        run_id = str(uuid4())

        if run_id in self._runs:
            raise RuntimeError(f"Duplicate run ID: {run_id}")

        self._runs[run_id] = SolverRun(run_id)
        return run_id
