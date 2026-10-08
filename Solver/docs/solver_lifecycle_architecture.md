# Solver lifecycle architecture


## Requirements view

The important ownership relations:

- The SolverRun owns its state machine and time data
- The SolverRunner coordinates the running instances and enforces resource limits
- The Engine (OP Tools) does the optimizations
- The SolverRunStateMachine / SolverRun does not measure time periods and does not initiate calculations


```mermaid
flowchart TD
    API["FastAPI / HTTP API"]
    R["SolverRunner<br/>Futások koordinálása"]
    A["SolverRun A<br/>run_id, accepted_at, started_at"]
    B["SolverRun B<br/>run_id, accepted_at, started_at"]
    SMA["SolverRunStateMachine A"]
    SMB["SolverRunStateMachine B"]
    EA["SolverEngine A<br/>CP-SAT"]
    EB["SolverEngine B<br/>CP-SAT"]

    API --> R
    R --> A
    R --> B
    A --> SMA
    B --> SMB
    R --> EA
    R --> EB
    EA -.-> R
    EB -.-> R

    classDef control fill:#dbeafe,stroke:#2563eb,color:#1e3a8a
    classDef model fill:#dcfce7,stroke:#16a34a,color:#14532d
    classDef engine fill:#fef3c7,stroke:#d97706,color:#78350f
    class API,R control
    class A,B,SMA,SMB model
    class EA,EB engine
```
