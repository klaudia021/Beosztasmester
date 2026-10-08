from datetime import datetime, timezone

from app.solver.state_machine import SolverRunStateMachine
from app.solver.states import SolverRunState


class SolverRun:
    def __init__(self, run_id: str) -> None:
        self._run_id = run_id
        self._state_machine = SolverRunStateMachine()
        self._accepted_at = datetime.now(timezone.utc)
        self._started_at: datetime | None = None
        self._finished_at: datetime | None = None

    @property
    def run_id(self) -> str:
        return self._run_id

    @property
    def state(self) -> SolverRunState:
        return self._state_machine.state

    @property
    def accepted_at(self) -> datetime:
        return self._accepted_at

    @property
    def started_at(self) -> datetime | None:
        return self._started_at

    @property
    def finished_at(self) -> datetime | None:
        return self._finished_at

    def start(self) -> None:
        self._state_machine.start()
        self._started_at = datetime.now(timezone.utc)

    def complete(self) -> None:
        self._state_machine.complete()
        self._finished_at = datetime.now(timezone.utc)
