
from threading import Event

from app.solver.engines.dummy import Assignments, DummySolverEngine
from app.solver.roster.dummy import get_dummy_assignments
from app.solver.runner import SolverRunner
from app.solver.states import SolverRunState


def test_default_dummy_provider_completes_run() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)
    engine = DummySolverEngine(delay_seconds=0)

    try:
        runner.schedule(run_id, engine.run)
    finally:
        runner.close()

    retrieved = runner.get_run_with_result(run_id)
    assert retrieved is not None
    snapshot, result = retrieved
    assert snapshot.state is SolverRunState.COMPLETED
    assert result == get_dummy_assignments()


def test_dummy_engine_returns_provider_result() -> None:
    assignments: Assignments = [
        {
            "employeeId": 1,
            "date": "2026-10-12",
            "shiftType": "D",
        }
    ]

    engine = DummySolverEngine(
        lambda: assignments,
        delay_seconds=0,
    )

    assert engine.run() == assignments


def test_scheduled_run_completes_and_stores_result() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)
    assignments: Assignments = [{"employeeId": 1}]

    try:
        runner.schedule(run_id, lambda: assignments)
    finally:
        runner.close()

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.COMPLETED
    assert snapshot.started_at is not None
    assert snapshot.finished_at is not None
    assert result == assignments


def test_engine_failure_marks_run_failed() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    def failing_engine() -> list[dict[str, int | str]]:
        raise ValueError("Simulated engine failure")

    try:
        runner.schedule(run_id, failing_engine)
    finally:
        runner.close()

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.FAILED
    assert result is None


def test_second_run_waits_for_available_worker() -> None:
    runner = SolverRunner()
    first_started = Event()
    release_first = Event()

    def blocking_engine() -> list[dict[str, int | str]]:
        first_started.set()
        if not release_first.wait(timeout=5):
            raise TimeoutError("Test worker was not released")
        return []

    first_id = runner.submit(1, "dummy", 101)
    second_id = runner.submit(2, "dummy", 102)

    try:
        runner.schedule(first_id, blocking_engine)

        assert first_started.wait(timeout=5)

        runner.schedule(second_id, lambda: [])

        first_snapshot = runner.get_run(first_id)
        second_snapshot = runner.get_run(second_id)

        assert first_snapshot is not None
        assert second_snapshot is not None
        assert first_snapshot.state is SolverRunState.RUNNING
        assert second_snapshot.state is SolverRunState.ACCEPTED
    finally:
        release_first.set()
        runner.close()

    first_snapshot = runner.get_run(first_id)
    second_snapshot = runner.get_run(second_id)

    assert first_snapshot is not None
    assert second_snapshot is not None
    assert first_snapshot.state is SolverRunState.COMPLETED
    assert second_snapshot.state is SolverRunState.COMPLETED


def test_get_run_with_result_returns_independent_copy() -> None:
    runner = SolverRunner()
    run_id = runner.submit(1, "dummy", 101)

    try:
        runner.schedule(
            run_id,
            lambda: [{"employeeId": 1}],
        )
    finally:
        runner.close()

    retrieved = runner.get_run_with_result(run_id)

    assert retrieved is not None
    snapshot, result = retrieved

    assert snapshot.state is SolverRunState.COMPLETED
    assert result is not None

    result[0]["employeeId"] = -1

    second_retrieval = runner.get_run_with_result(run_id)

    assert second_retrieval is not None
    _, stored_result = second_retrieval

    assert stored_result == [{"employeeId": 1}]


def test_dummy_engine_uses_injected_provider() -> None:
    calls = 0

    def fake_provider() -> list[dict[str, int | str]]:
        nonlocal calls
        calls += 1
        return [{"employeeId": 42}]

    engine = DummySolverEngine(
        roster_provider=fake_provider,
        delay_seconds=0,
    )

    assert engine.run() == [{"employeeId": 42}]
    assert calls == 1
