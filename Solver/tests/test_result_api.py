
from time import monotonic, sleep

import pytest
from fastapi.testclient import TestClient

from app import main as main_module
from app.main import app
from app.solver.engines.dummy import DummySolverEngine
from app.solver.runner import SolverRunner


def test_result_unknown_run_returns_404() -> None:
    """An unknown run ID must produce HTTP 404.

    The API must not invent or implicitly create a missing run.
    """
    with TestClient(app) as client:
        response = client.get("/solver-runs/unknown/result")

    assert response.status_code == 404
    assert response.json()["detail"] == "Solver run not found"


def test_result_accepted_run_has_no_result() -> None:
    """An admitted but not yet scheduled run has no result.

    Its lifecycle is ACCEPTED, with no start/finish timestamps.
    Result retrieval must not trigger execution.
    """

    with TestClient(app) as client:
        runner: SolverRunner = app.state.runner

        run_id = runner.submit(
            horizon_id="H-001",
            engine_type="dummy",
            instance_id="M-001",
        )

        response = client.get(f"/solver-runs/{run_id}/result")

    assert response.status_code == 200

    body = response.json()

    assert body["runId"] == run_id
    assert body["horizonId"] == "H-001"
    assert body["instanceId"] == "M-001"
    assert body["engineType"] == "dummy"
    assert body["status"] == "accepted"
    assert body["result"] is None
    assert body["startedAt"] is None
    assert body["finishedAt"] is None


def test_completed_run_returns_engine_result() -> None:
    """A completed run exposes its stored Engine result.
    The result endpoint returns assignments and lifecycle metadata.
    The status endpoint must not include assignments.
    """

    assignments = [
        {
            "employeeId": "E001",
            "date": "2026-03-02",
            "shiftType": "D",
        }
    ]

    with TestClient(app) as client:
        runner: SolverRunner = app.state.runner

        run_id = runner.submit(
            horizon_id="H-002",
            engine_type="dummy",
            instance_id="M-002",
        )

        runner.schedule(run_id, lambda: assignments)
        runner.close()

        response = client.get(f"/solver-runs/{run_id}/result")
        status_response = client.get(f"/solver-runs/{run_id}")

    assert response.status_code == 200

    body = response.json()

    assert body["runId"] == run_id
    assert body["status"] == "completed"
    assert body["result"] == assignments
    assert body["startedAt"] is not None
    assert body["finishedAt"] is not None

    assert status_response.status_code == 200
    assert status_response.json()["status"] == "completed"
    assert "result" not in status_response.json()


def test_http_solve_to_completed_result(
    monkeypatch: pytest.MonkeyPatch,
) -> None:
    """Verify the asynchronous HTTP execution path end to end.
    POST /solve -> Runner -> Dummy Engine -> COMPLETED -> GET result.
    A short injected delay keeps the test fast and independent
    of Dani's unfinished roster provider.
    """

    assignments = [
        {
            "employeeId": "E001",
            "date": "2026-03-02",
            "shiftType": "D",
        }
    ]

    engine = DummySolverEngine(
        roster_provider=lambda: assignments,
        delay_seconds=0.05,
    )

    monkeypatch.setattr(
        main_module,
        "select_engine",
        lambda engine_type: engine.run,
    )

    payload = {
        "horizonId": "H-INTEGRATION-001",
        "engineType": "dummy",
        "instance": {
            "instanceId": "M-INTEGRATION-001",
            "horizon": {
                "start": "2026-03-02",
                "days": 7,
            },
            "shiftTypes": [],
            "skills": [],
            "employees": [],
            "demand": [],
            "requests": [],
            "rules": [],
        },
    }

    with TestClient(app) as client:
        response = client.post("/solve", json=payload)

        assert response.status_code == 202
        run_id = response.json()["runId"]

        deadline = monotonic() + 2.0

        while True:
            status_response = client.get(
                f"/solver-runs/{run_id}"
            )

            assert status_response.status_code == 200
            status = status_response.json()["status"]

            if status == "completed":
                break

            assert status in ("accepted", "running")
            assert monotonic() < deadline
            sleep(0.01)

        result_response = client.get(
            f"/solver-runs/{run_id}/result"
        )

    assert result_response.status_code == 200

    body = result_response.json()

    assert body["runId"] == run_id
    assert body["status"] == "completed"
    assert body["result"] == assignments
    assert body["startedAt"] is not None
    assert body["finishedAt"] is not None
    assert "result" not in status_response.json()
