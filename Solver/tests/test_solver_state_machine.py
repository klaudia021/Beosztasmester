import pytest

from app.solver.state_machine import (
    InvalidStateTransitionError,
    SolverRunStateMachine,
)
from app.solver.states import SolverRunState


def test_accepted_run_can_start() -> None:
    state_machine = SolverRunStateMachine()

    state_machine.start()

    assert state_machine.state is SolverRunState.RUNNING


def test_state_machine_running_run_cannot_start_again() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.start()


def test_running_run_can_be_cancelled() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    state_machine.cancel()

    assert state_machine.state is SolverRunState.CANCELLED


def test_accepted_run_cannot_be_cancelled() -> None:
    state_machine = SolverRunStateMachine()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.cancel()


def test_running_run_can_complete() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    state_machine.complete()

    assert state_machine.state is SolverRunState.COMPLETED


def test_accepted_run_cannot_complete() -> None:
    state_machine = SolverRunStateMachine()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.complete()


def test_running_run_can_time_out() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    state_machine.time_limit_reached()

    assert state_machine.state is SolverRunState.TIMED_OUT


def test_accepted_run_cannot_time_out() -> None:
    state_machine = SolverRunStateMachine()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.time_limit_reached()


def test_running_run_can_become_infeasible() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    state_machine.infeasibility_proven()

    assert state_machine.state is SolverRunState.INFEASIBLE


def test_accepted_run_cannot_become_infeasible() -> None:
    state_machine = SolverRunStateMachine()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.infeasibility_proven()


def test_running_run_can_fail() -> None:
    state_machine = SolverRunStateMachine()
    state_machine.start()

    state_machine.fail()

    assert state_machine.state is SolverRunState.FAILED


def test_accepted_run_cannot_fail() -> None:
    state_machine = SolverRunStateMachine()

    with pytest.raises(InvalidStateTransitionError):
        state_machine.fail()
