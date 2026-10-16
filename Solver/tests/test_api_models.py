import pytest
from pydantic import ValidationError

from app.api.models import SolveRequest, SolverInstance


def test_solve_request_valid() -> None:
    payload = {
        "horizonId": 1,
        "engineType": "dummy",
        "instance": {
            "instanceId": 101,
            "employees": [],
            "demand": [],
        },
    }

    request = SolveRequest.model_validate(payload)

    assert isinstance(request, SolveRequest)
    assert isinstance(request.instance, SolverInstance)

    assert request.horizonId == 1
    assert request.engineType == "dummy"
    assert request.instance.instanceId == 101

    assert request.instance.model_extra == {
        "employees": [],
        "demand": [],
    }


@pytest.mark.parametrize(
    "payload",
    [
        {
            "horizonId": 0,
            "engineType": "dummy",
            "instance": {"instanceId": 101},
        },
        {
            "horizonId": 1,
            "engineType": "dummy",
            "instance": {},
        },
        {
            "horizonId": 1,
            "engineType": "dummy",
            "instance": "invalid",
        },
        {
            "horizonId": 1,
            "engineType": "dummy",
            "instance": {"instanceId": 0},
        },
    ],
)
def test_solve_request_invalid(payload: dict[str, object]) -> None:
    with pytest.raises(ValidationError):
        SolveRequest.model_validate(payload)
