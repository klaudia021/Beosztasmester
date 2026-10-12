import pytest

from app.solver.engines.dispatch import (
    UnsupportedEngineTypeError,
    select_engine,
)


def test_select_dummy_engine() -> None:
    engine = select_engine("dummy")

    assert callable(engine)


def test_reject_unsupported_engine_type() -> None:
    with pytest.raises(
        UnsupportedEngineTypeError,
        match="Unsupported engine type: cpsat",
    ):
        select_engine("cpsat")
