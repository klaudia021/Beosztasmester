from datetime import datetime, timedelta, timezone

import pytest

from app.solver.state_machine import (
    InvalidStateTransitionError,
    SolverRun,
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


def test_new_run_is_accepted() -> None:
    run = SolverRun("run-001")

    assert run.run_id == "run-001"
    assert run.state is SolverRunState.ACCEPTED


def test_run_can_start() -> None:
    run = SolverRun("run-001")
    before = datetime.now(timezone.utc)

    run.start()

    after = datetime.now(timezone.utc)

    assert run.state is SolverRunState.RUNNING
    assert run.started_at is not None
    assert before <= run.started_at <= after


def test_new_run_has_accepted_timestamp() -> None:
    run = SolverRun("run-001")

    assert isinstance(run.accepted_at, datetime)
    assert run.accepted_at.tzinfo is not None
    assert run.accepted_at.utcoffset() == timedelta(0)


def test_accepted_timestamp_is_set_at_creation() -> None:
    before = datetime.now(timezone.utc)

    run = SolverRun("run-001")

    after = datetime.now(timezone.utc)

    assert before <= run.accepted_at <= after


def test_new_run_has_no_start_timestamp() -> None:
    run = SolverRun("run-001")

    assert run.started_at is None


def test_solver_run_cannot_start_twice() -> None:
    run = SolverRun("run-001")
    run.start()

    original_started_at = run.started_at

    with pytest.raises(InvalidStateTransitionError):
        run.start()

    assert run.state is SolverRunState.RUNNING
    assert run.started_at == original_started_at
