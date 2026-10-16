from concurrent.futures import ThreadPoolExecutor
from dataclasses import FrozenInstanceError
from threading import Event
from unittest.mock import patch

import pytest

from app.solver.runner import DuplicateActiveRunError, SolverRunner
from app.solver.states import SolverRunState


def test_new_runner_has_empty_registry() -> None:
    runner = SolverRunner()

    assert runner._runs == {}
    assert runner._run_context == {}


def test_submit_registers_immediately_visible_run() -> None:
    runner = SolverRunner()

    run_id = runner.submit(1, "greedy", 101)
    snapshot = runner.get_run(run_id)

    assert snapshot is not None
    assert snapshot.run_id == run_id
    assert snapshot.horizon_id == 1
    assert snapshot.engine_type == "greedy"
    assert snapshot.state is SolverRunState.ACCEPTED
    assert snapshot.accepted_at is not None
    assert snapshot.started_at is None
    assert snapshot.finished_at is None


def test_get_run_returns_none_for_unknown_id() -> None:
    runner = SolverRunner()

    assert runner.get_run(999) is None


def test_snapshot_is_immutable() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "greedy", 101)
    snapshot = runner.get_run(run_id)

    assert snapshot is not None

    with pytest.raises(FrozenInstanceError):
        setattr(snapshot, "state", SolverRunState.COMPLETED)

    assert runner.get_run(run_id) == snapshot


def test_submit_rejects_duplicate_active_run() -> None:
    runner = SolverRunner()
    first_id = runner.submit(1, "greedy", 101)

    with pytest.raises(DuplicateActiveRunError):
        runner.submit(1, "greedy", 102)

    assert len(runner._runs) == 1
    assert len(runner._run_context) == 1
    assert runner.get_run(first_id) is not None


def test_different_engines_are_independent() -> None:
    runner = SolverRunner()

    greedy_id = runner.submit(1, "greedy", 101)
    cpsat_id = runner.submit(1, "cpsat", 102)

    assert greedy_id != cpsat_id
    assert len(runner._runs) == 2
    assert runner.get_run(greedy_id) is not None
    assert runner.get_run(cpsat_id) is not None


def test_different_horizons_are_independent() -> None:
    runner = SolverRunner()

    first_id = runner.submit(1, "greedy", 101)
    second_id = runner.submit(2, "dummy", 102)

    assert first_id != second_id
    assert len(runner._runs) == 2


def test_terminal_run_allows_resubmission() -> None:
    runner = SolverRunner()

    first_id = runner.submit(1, "greedy", 101)

    # Temporary test setup until Runner owns execution transitions.
    runner._runs[first_id].start()
    runner._runs[first_id].complete()

    second_id = runner.submit(1, "greedy", 102)

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
        runner.submit(0, "greedy", 0)

    with pytest.raises(ValueError):
        runner.submit(1, "", 0)

    with pytest.raises(ValueError):
        runner.submit(1, "greedy", 0)

    assert runner._runs == {}
    assert runner._run_context == {}


def test_submit_rejects_duplicate_backend_run_id() -> None:
    runner = SolverRunner()

    first_id = runner.submit(1, "greedy", 101)
    original_run = runner._runs[first_id]

    with pytest.raises(RuntimeError, match="Duplicate backend run ID"):
        runner.submit(2, "greedy", 101)

    assert len(runner._runs) == 1
    assert len(runner._run_context) == 1
    assert runner._runs[first_id] is original_run
    assert runner._run_context[first_id] == (1, "greedy", 101)


def test_submit_preserves_instance_id() -> None:
    runner = SolverRunner()

    try:
        run_id = runner.submit(1, "dummy", 101)
        snapshot = runner.get_run(run_id)

        assert snapshot is not None
        assert snapshot.instance_id == 101
        assert snapshot.horizon_id == 1
        assert snapshot.engine_type == "dummy"
    finally:
        runner.close()


def test_submit_rejects_empty_instance_id() -> None:
    runner = SolverRunner()

    try:
        with pytest.raises(ValueError, match="instance_id"):
            runner.submit(1, "dummy", 0)
    finally:
        runner.close()


def test_different_instance_does_not_bypass_admission() -> None:
    runner = SolverRunner()

    try:
        first_id = runner.submit(1, "dummy", 101)

        with pytest.raises(DuplicateActiveRunError):
            runner.submit(1, "dummy", 102)

        assert runner.get_run(first_id) is not None
    finally:
        runner.close()


def test_get_run_with_result_unknown_id() -> None:
    runner = SolverRunner()

    assert runner.get_run_with_result(999) is None


def test_get_run_with_result_before_execution() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.run_id == run_id
    assert snapshot.state is SolverRunState.ACCEPTED
    assert result is None


def test_get_run_with_result_empty_result() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    runner.schedule(run_id, lambda: [])
    runner.close()

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.COMPLETED
    assert result == []


def test_get_run_with_result_returns_independent_copy() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    expected = [{"employeeId": 1, "shiftType": "D"}]

    runner.schedule(run_id, lambda: expected)
    runner.close()

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.COMPLETED
    assert result == expected
    assert result is not None

    result[0]["shiftType"] = "N"
    result.append({"employeeId": 2, "shiftType": "D"})

    second_retrieval = runner.get_run_with_result(run_id)

    assert second_retrieval is not None
    _, stored_result = second_retrieval
    assert stored_result == expected


def test_get_run_with_result_is_atomic_during_completion() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    engine_started = Event()
    release_engine = Event()
    snapshot_created = Event()
    release_snapshot = Event()

    expected = [{"employeeId": 1}]

    def controlled_engine() -> list[dict[str, int | str]]:
        engine_started.set()
        if not release_engine.wait(timeout=5):
            raise TimeoutError("Engine was not released")
        return expected

    original_make_snapshot = runner._make_snapshot

    def controlled_snapshot(run_id: int):  # type: ignore[no-untyped-def]
        snapshot = original_make_snapshot(run_id)
        snapshot_created.set()
        if not release_snapshot.wait(timeout=5):
            raise TimeoutError("Snapshot was not released")
        return snapshot

    runner.schedule(run_id, controlled_engine)

    try:
        assert engine_started.wait(timeout=5)

        with patch.object(
            runner,
            "_make_snapshot",
            side_effect=controlled_snapshot,
        ):
            with ThreadPoolExecutor(max_workers=1) as reader:
                try:
                    future = reader.submit(
                        runner.get_run_with_result, run_id
                    )

                    assert snapshot_created.wait(timeout=5)

                    release_engine.set()
                finally:
                    release_snapshot.set()

                retrieved = future.result(timeout=5)
    finally:
        release_engine.set()
        release_snapshot.set()
        runner.close()

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.RUNNING
    assert result is None

    completed = runner.get_run_with_result(run_id)
    assert completed is not None

    final_snapshot, final_result = completed
    assert final_snapshot.state is SolverRunState.COMPLETED
    assert final_result == expected
