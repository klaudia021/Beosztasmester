from datetime import datetime, timedelta, timezone

import pytest

from app.solver.run import SolverRun
from app.solver.state_machine import InvalidStateTransitionError
from app.solver.states import SolverRunState


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
