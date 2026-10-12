import pytest
from pydantic import ValidationError

from app.api.models import SolveRequest, SolverInstance


def test_solve_request_valid() -> None:
    payload = {
        "horizonId": "H-01",
        "engineType": "dummy",
        "instance": {
            "instanceId": "M-01",
            "employees": [],
            "demand": [],
        },
    }

    request = SolveRequest.model_validate(payload)

    assert isinstance(request, SolveRequest)
    assert isinstance(request.instance, SolverInstance)

    assert request.horizonId == "H-01"
    assert request.engineType == "dummy"
    assert request.instance.instanceId == "M-01"

    assert request.instance.model_extra == {
        "employees": [],
        "demand": [],
    }


@pytest.mark.parametrize(
    "payload",
    [
        {
            "horizonId": "",
            "engineType": "dummy",
            "instance": {"instanceId": "M-01"},
        },
        {
            "horizonId": "H-01",
            "engineType": "dummy",
            "instance": {},
        },
        {
            "horizonId": "H-01",
            "engineType": "dummy",
            "instance": "invalid",
        },
    ],
)
def test_solve_request_invalid(payload: dict[str, object]) -> None:
    with pytest.raises(ValidationError):
        SolveRequest.model_validate(payload)
