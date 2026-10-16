from pydantic import BaseModel, ConfigDict, Field


class SolverInstance(BaseModel):
    model_config = ConfigDict(extra="allow")

    instanceId: int = Field(gt=0)


class SolveRequest(BaseModel):
    horizonId: int = Field(gt=0)
    engineType: str = Field(min_length=1)
    instance: SolverInstance
