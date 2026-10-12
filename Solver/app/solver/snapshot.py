from dataclasses import dataclass
from datetime import datetime

from app.solver.states import SolverRunState


@dataclass(frozen=True)
class SolverRunSnapshot:
    run_id: str
    horizon_id: str
    engine_type: str
    state: SolverRunState
    accepted_at: datetime
    started_at: datetime | None
    finished_at: datetime | None
