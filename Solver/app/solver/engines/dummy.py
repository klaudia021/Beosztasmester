
from collections.abc import Callable
from time import sleep

type Assignments = list[dict[str, str]]
type RosterProvider = Callable[[], Assignments]


class DummySolverEngine:
    def __init__(
        self,
        roster_provider: RosterProvider,
        delay_seconds: float = 5.0,
    ) -> None:
        if delay_seconds < 0:
            raise ValueError("delay_seconds must not be negative")

        self._roster_provider = roster_provider
        self._delay_seconds = delay_seconds

    def run(self) -> Assignments:
        sleep(self._delay_seconds)
        return self._roster_provider()
