# Solver Run Lifecycle

## Requirements view

This diagram shows the externally observable Solver-run lifecycle implied by
the system requirements.

It does not yet define the implementation state machine.

```mermaid
stateDiagram-v2

    classDef badBadEvent fill:#f00,color:white,font-weight:bold,stroke-width:2px,stroke:yellow

    [*] --> Accepted : run requested [F04.SOLV.0001]

    Accepted --> Running : execution starts

    note right of Running
        Status/progress queryable
        [F04.SOLV.0002]
    end note

    Running --> Completed : solution finished
    Running --> Cancelled : cancel requested [F04.SOLV.0003]
    Running --> TimedOut : time limit reached [F04.SOLV.0004]
    Running --> Infeasible : infeasibility proven [F06.SOLV.0001]
    Running --> Failed : technical failure [NFR.SOLV.0001]

    Completed --> [*]
    Cancelled --> [*]
    TimedOut --> [*]
    Infeasible --> [*]
    Failed --> [*]

    class Failed badBadEvent
```
