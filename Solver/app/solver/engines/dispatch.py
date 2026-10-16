from collections.abc import Callable

from app.solver.engines.dummy import DummySolverEngine

type EngineCallable = Callable[[], list[dict[str, int | str]]]


class UnsupportedEngineTypeError(ValueError):
    pass


def select_engine(engine_type: str) -> EngineCallable:
    if engine_type == "dummy":
        return DummySolverEngine().run

    raise UnsupportedEngineTypeError(
        f"Unsupported engine type: {engine_type}"
    )
