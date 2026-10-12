from unittest.mock import patch

from fastapi.testclient import TestClient

from app.main import app
from app.solver.runner import SolverRunner


def test_health() -> None:
    with TestClient(app) as client:
        response = client.get("/health")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}


def test_solve() -> None:
    with TestClient(app) as client:
        response = client.post("/solve")

    assert response.status_code == 200
    assert response.json() == {"status": "accepted"}


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
