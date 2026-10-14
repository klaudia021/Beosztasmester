from pydantic import BaseModel, ConfigDict, Field


class SolverInstance(BaseModel):
    model_config = ConfigDict(extra="allow")

    instanceId: str = Field(min_length=1)


class SolveRequest(BaseModel):
    horizonId: str = Field(min_length=1)
    engineType: str = Field(min_length=1)
    instance: SolverInstance
