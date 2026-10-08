from app.solver.states import SolverRunState


class InvalidStateTransitionError(RuntimeError):
    pass


class SolverRunStateMachine:
    def __init__(self) -> None:
        self._state = SolverRunState.ACCEPTED

    @property
    def state(self) -> SolverRunState:
        return self._state

    def start(self) -> None:
        if self._state is not SolverRunState.ACCEPTED:
            raise InvalidStateTransitionError(
                f"Cannot start Solver run from state {self._state}"
            )

        self._state = SolverRunState.RUNNING

    def cancel(self) -> None:
        if self._state is not SolverRunState.RUNNING:
            raise InvalidStateTransitionError(
                f"Cannot cancel Solver run from state {self._state}"
            )

        self._state = SolverRunState.CANCELLED

    def complete(self) -> None:
        if self._state is not SolverRunState.RUNNING:
            raise InvalidStateTransitionError(
                f"Cannot complete Solver run from state {self._state}"
            )

        self._state = SolverRunState.COMPLETED

    def time_limit_reached(self) -> None:
        if self._state is not SolverRunState.RUNNING:
            raise InvalidStateTransitionError(
                f"Cannot time out Solver run from state {self._state}"
            )

        self._state = SolverRunState.TIMED_OUT

    def infeasibility_proven(self) -> None:
        if self._state is not SolverRunState.RUNNING:
            raise InvalidStateTransitionError(
                f"Cannot mark Solver run infeasible from state {self._state}"
            )

        self._state = SolverRunState.INFEASIBLE

    def fail(self) -> None:
        if self._state is not SolverRunState.RUNNING:
            raise InvalidStateTransitionError(
                f"Cannot fail Solver run from state {self._state}"
            )

        self._state = SolverRunState.FAILED
