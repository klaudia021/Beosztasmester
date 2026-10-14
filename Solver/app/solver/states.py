from enum import StrEnum


class SolverRunState(StrEnum):
    ACCEPTED = "accepted"
    RUNNING = "running"
    COMPLETED = "completed"
    CANCELLED = "cancelled"
    TIMED_OUT = "timed_out"
    INFEASIBLE = "infeasible"
    FAILED = "failed"
