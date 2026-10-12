from collections.abc import AsyncIterator
from contextlib import asynccontextmanager

from fastapi import FastAPI

from app.solver.runner import SolverRunner


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


@app.post("/solve")
def solve() -> dict[str, str]:
    return {"status": "accepted"}
