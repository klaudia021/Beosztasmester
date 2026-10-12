
from threading import Event
from unittest.mock import patch

from fastapi.testclient import TestClient

from app.main import app
from app.solver.runner import SolverRunner
from app.solver.states import SolverRunState


def test_health() -> None:
    with TestClient(app) as client:
        response = client.get("/health")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}


def test_solve() -> None:
    payload = {
        "horizonId": "H-01",
        "engineType": "dummy",
        "instance": {"instanceId": "M-01"},
    }

    engine_started = Event()
    release_engine = Event()

    def controlled_engine() -> list[dict[str, str]]:
        engine_started.set()
        if not release_engine.wait(timeout=5):
            raise TimeoutError("Test engine was not released")
        return []

    with patch(
        "app.main.select_engine",
        return_value=controlled_engine,
    ):
        with TestClient(app) as client:
            try:
                response = client.post("/solve", json=payload)

                assert response.status_code == 202
                assert engine_started.wait(timeout=5)

                run_id = response.json()["runId"]
                runner: SolverRunner = app.state.runner
                snapshot = runner.get_run(run_id)

                assert snapshot is not None
                assert snapshot.horizon_id == "H-01"
                assert snapshot.engine_type == "dummy"
                assert snapshot.instance_id == "M-01"
                assert snapshot.state is SolverRunState.RUNNING
            finally:
                release_engine.set()

        completed = runner.get_run(run_id)
        assert completed is not None
        assert completed.state is SolverRunState.COMPLETED
        assert runner.get_result(run_id) == []


def test_solve_invalid_request() -> None:
    payload = {
        "horizonId": "",
        "engineType": "dummy",
        "instance": {"instanceId": "M-01"},
    }

    with TestClient(app) as client:
        response = client.post("/solve", json=payload)

    assert response.status_code == 422


def test_solve_unsupported_engine_type() -> None:
    payload = {
        "horizonId": "H-01",
        "engineType": "cpsat",
        "instance": {"instanceId": "M-01"},
    }

    with patch.object(SolverRunner, "submit", autospec=True) as submit_mock:
        with TestClient(app) as client:
            response = client.post("/solve", json=payload)

        assert response.status_code == 422
        assert response.json() == {
            "detail": "Unsupported engine type: cpsat",
        }
        submit_mock.assert_not_called()


def test_solve_duplicate_active_run() -> None:
    payload = {
        "horizonId": "H-01",
        "engineType": "dummy",
        "instance": {"instanceId": "M-01"},
    }

    engine_started = Event()
    release_engine = Event()

    def controlled_engine() -> list[dict[str, str]]:
        engine_started.set()
        if not release_engine.wait(timeout=5):
            raise TimeoutError("Test engine was not released")
        return []

    with patch(
        "app.main.select_engine",
        return_value=controlled_engine,
    ):
        with TestClient(app) as client:
            try:
                first = client.post("/solve", json=payload)

                assert first.status_code == 202
                assert engine_started.wait(timeout=5)

                second = client.post("/solve", json=payload)

                assert second.status_code == 409
                assert second.json() == {
                    "detail": "Active Solver run already exists",
                }

                runner: SolverRunner = app.state.runner
                snapshot = runner.get_run(first.json()["runId"])

                assert snapshot is not None
                assert snapshot.state is SolverRunState.RUNNING
            finally:
                release_engine.set()


def test_runner_lifecycle() -> None:
    original_close = SolverRunner.close
    with patch.object(
        SolverRunner,
        "close",
        autospec=True,
        side_effect=original_close,
    ) as close_mock:
        with TestClient(app):
            runner = app.state.runner
            assert isinstance(runner, SolverRunner)
            close_mock.assert_not_called()

        close_mock.assert_called_once_with(runner)
