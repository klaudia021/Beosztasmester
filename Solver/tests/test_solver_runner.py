from uuid import UUID

import pytest

from app.solver.runner import SolverRunner
from app.solver.states import SolverRunState


def test_new_runner_has_empty_registry() -> None:
    runner = SolverRunner()

    assert runner._runs == {}


def test_register_run_creates_accepted_run() -> None:
    runner = SolverRunner()

    run_id = runner._register_run()

    assert run_id in runner._runs
    assert runner._runs[run_id].run_id == run_id
    assert runner._runs[run_id].state is SolverRunState.ACCEPTED



def test_register_run_rejects_duplicate_id(
    monkeypatch: pytest.MonkeyPatch,
) -> None:
    runner = SolverRunner()

    fixed_uuid = UUID("12345678-1234-4234-8234-123456789abc")

    monkeypatch.setattr(
        "app.solver.runner.uuid4",
        lambda: fixed_uuid,
    )

    run_id = runner._register_run()
    original_run = runner._runs[run_id]

    with pytest.raises(RuntimeError, match="Duplicate run ID"):
        runner._register_run()

    assert len(runner._runs) == 1
    assert runner._runs[run_id] is original_run
