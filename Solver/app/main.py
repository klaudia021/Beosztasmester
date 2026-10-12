from collections.abc import AsyncIterator
from contextlib import asynccontextmanager

from fastapi import FastAPI, HTTPException, Request

from app.api.models import SolveRequest
from app.solver.engines.dispatch import (
    UnsupportedEngineTypeError,
    select_engine,
)
from app.solver.runner import DuplicateActiveRunError, SolverRunner


@asynccontextmanager
async def lifespan(app: FastAPI) -> AsyncIterator[None]:
    runner = SolverRunner()
    app.state.runner = runner

    try:
        yield
    finally:
        runner.close()


app = FastAPI(lifespan=lifespan)


@app.get("/health")
def health() -> dict[str, str]:
    return {"status": "ok"}


@app.post("/solve", status_code=202)
def solve(payload: SolveRequest, request: Request) -> dict[str, str]:
    runner: SolverRunner = request.app.state.runner

    try:
        engine = select_engine(payload.engineType)
    except UnsupportedEngineTypeError as exc:
        raise HTTPException(
            status_code=422,
            detail=str(exc),
        ) from exc

    try:
        run_id = runner.submit(
            horizon_id=payload.horizonId,
            engine_type=payload.engineType,
            instance_id=payload.instance.instanceId,
        )
    except DuplicateActiveRunError as exc:
        raise HTTPException(
            status_code=409,
            detail="Active Solver run already exists",
        ) from exc

    runner.schedule(run_id, engine)

    return {"runId": run_id}
