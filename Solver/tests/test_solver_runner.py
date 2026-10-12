
from dataclasses import FrozenInstanceError
from uuid import UUID

import pytest

from app.solver.runner import DuplicateActiveRunError, SolverRunner
from app.solver.states import SolverRunState


def test_new_runner_has_empty_registry() -> None:
    runner = SolverRunner()

    assert runner._runs == {}
    assert runner._run_context == {}


def test_submit_registers_immediately_visible_run() -> None:
    runner = SolverRunner()

    run_id = runner.submit("horizon-1", "greedy")
    snapshot = runner.get_run(run_id)

    assert snapshot is not None
    assert snapshot.run_id == run_id
    assert snapshot.horizon_id == "horizon-1"
    assert snapshot.engine_type == "greedy"
    assert snapshot.state is SolverRunState.ACCEPTED
    assert snapshot.accepted_at is not None
    assert snapshot.started_at is None
    assert snapshot.finished_at is None


def test_get_run_returns_none_for_unknown_id() -> None:
    runner = SolverRunner()

    assert runner.get_run("unknown") is None


def test_snapshot_is_immutable() -> None:
    runner = SolverRunner()
    run_id = runner.submit("horizon-1", "greedy")
    snapshot = runner.get_run(run_id)

    assert snapshot is not None

    with pytest.raises(FrozenInstanceError):
        setattr(snapshot, "state", SolverRunState.COMPLETED)

    assert runner.get_run(run_id) == snapshot


def test_submit_rejects_duplicate_active_run() -> None:
    runner = SolverRunner()
    first_id = runner.submit("horizon-1", "greedy")

    with pytest.raises(DuplicateActiveRunError):
        runner.submit("horizon-1", "greedy")

    assert len(runner._runs) == 1
    assert len(runner._run_context) == 1
    assert runner.get_run(first_id) is not None


def test_different_engines_are_independent() -> None:
    runner = SolverRunner()

    greedy_id = runner.submit("horizon-1", "greedy")
    cpsat_id = runner.submit("horizon-1", "cpsat")

    assert greedy_id != cpsat_id
    assert len(runner._runs) == 2
    assert runner.get_run(greedy_id) is not None
    assert runner.get_run(cpsat_id) is not None


def test_different_horizons_are_independent() -> None:
    runner = SolverRunner()

    first_id = runner.submit("horizon-1", "greedy")
    second_id = runner.submit("horizon-2", "greedy")

    assert first_id != second_id
    assert len(runner._runs) == 2


def test_terminal_run_allows_resubmission() -> None:
    runner = SolverRunner()

    first_id = runner.submit("horizon-1", "greedy")

    # Temporary test setup until Runner owns execution transitions.
    runner._runs[first_id].start()
    runner._runs[first_id].complete()

    second_id = runner.submit("horizon-1", "greedy")

    assert first_id != second_id
    assert len(runner._runs) == 2

    first_snapshot = runner.get_run(first_id)
    second_snapshot = runner.get_run(second_id)

    assert first_snapshot is not None
    assert second_snapshot is not None
    assert first_snapshot.state is SolverRunState.COMPLETED
    assert second_snapshot.state is SolverRunState.ACCEPTED


def test_invalid_submission_leaves_registry_unchanged() -> None:
    runner = SolverRunner()

    with pytest.raises(ValueError):
        runner.submit("", "greedy")

    with pytest.raises(ValueError):
        runner.submit("horizon-1", "")

    assert runner._runs == {}
    assert runner._run_context == {}


def test_submit_rejects_duplicate_uuid(
    monkeypatch: pytest.MonkeyPatch,
) -> None:
    runner = SolverRunner()

    fixed_uuid = UUID("12345678-1234-4234-8234-123456789abc")

    monkeypatch.setattr(
        "app.solver.runner.uuid4",
        lambda: fixed_uuid,
    )

    first_id = runner.submit("horizon-1", "greedy")
    original_run = runner._runs[first_id]

    with pytest.raises(RuntimeError, match="Duplicate run ID"):
        runner.submit("horizon-2", "greedy")

    assert len(runner._runs) == 1
    assert len(runner._run_context) == 1
    assert runner._runs[first_id] is original_run
    assert runner._run_context[first_id] == ("horizon-1", "greedy")
