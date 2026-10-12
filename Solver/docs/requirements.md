# Solver Requirements

## Purpose

Solver-specific requirements derived from the Beosztásmester system specification.

Primary source: `beosztasmester-specifikacio.md`

Traceability target:

system requirement → Solver requirement → design → implementation → verification

## Requirement IDs

Format:

`<SOURCE>.SOLV.<LOCAL-ID>`

Examples:
- `F04.SOLV.0001`
- `F05.SOLV.0001`
- `H01.SOLV.0001`

Requirement IDs shall never be reused.

## Lifecycle

Allowed statuses:

- `draft`
- `active`
- `superseded`
- `obsolete`
- `rejected`

Inactive requirements shall remain in the document for traceability.

Predecessor/successor links shall be recorded where applicable.

## Metadata

Each requirement records:

- `Status`
- `Parent`
- `Predecessor`
- `Successor`
- `Source`
- `Verification`

Use `N/A` where a metadata field is not applicable.


---


## F04 — Solver execution

### F04.SOLV.0001 — Asynchronous run creation

**Status:** draft  
**Parent:** F04  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F4  
**Verification:** API test

WHEN a Solver run is requested,
THE Solver service SHALL create the run asynchronously and return a run identifier
without waiting for execution to finish.


### F04.SOLV.0002 — Run status and progress

**Status:** draft  
**Parent:** F04  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F4  
**Verification:** API test

WHILE a Solver run is executing,
THE Solver service SHALL make the current run status and available progress
information queryable.


### F04.SOLV.0003 — Run cancellation

**Status:** draft  
**Parent:** F04  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F4  
**Verification:** API / integration test

WHILE a Solver run is executing,
THE Solver service SHALL accept a request to stop the run and preserve the best
available result found before termination.


### F04.SOLV.0004 — Time-limit termination

**Status:** draft  
**Parent:** F04  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F4  
**Verification:** Solver / integration test

WHEN the configured Solver time limit is reached,
THE Solver service SHALL terminate the run and preserve the best available result
together with available solution-quality information.


### F04.SOLV.0005 — Run lifecycle ownership

**Status:** draft
**Parent:** F04
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, F4
**Verification:** Solver / integration test

THE SolverRun object SHALL own its lifecycle state machine and expose the current run state through a read-only interface.


### F04.SOLV.0006 — Run timestamps

**Status:** draft
**Parent:** F04
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, F4
**Verification:** Solver / integration test

THE SolverRun object SHALL record the acceptance timestamp at creation
 and the execution start timestamp upon a successful transition to RUNNING.


### F04.SOLV.0007 — Run registration and immediate visibility

**Status:** draft  
**Parent:** F04  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F4  
**Verification:** Solver / integration test

WHEN the Solver service successfully accepts a run request,
THE Solver service SHALL register the run before returning its identifier
and SHALL make the run immediately queryable by that identifier.


### F04.SOLV.0008 — Asynchronous HTTP execution contract

**Status:** draft
**Parent:** F04
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, Ch 6 / F4
**Verification:** API test

WHEN the Backend submits a valid Solver request,
THE Solver service SHALL return HTTP 202 and a runId
without waiting for calculation to complete.

THE Backend SHALL be able to query the status
and available result of the run using that runId.


### F04.SOLV.0009 — Engine selection and background scheduling

**Status:** draft
**Parent:** F04
**Predecessor:** N/A
**Successor:** N/A
**Source:** Solver service architecture decision; F04.SOLV.0001, D06.SOLV.0001
**Verification:** API / integration test

WHEN the Solver service receives a valid execution request,
THE Solver service SHALL select the Solver Engine according to
the requested engineType and schedule the admitted run for
asynchronous background execution.

IF the requested engineType is unsupported,
THE Solver service SHALL reject the request before run registration.

The SolverRunner SHALL remain independent of concrete
Solver Engine implementations.


---


## F06 — Infeasibility handling

### F06.SOLV.0001 — Infeasibility detection

**Status:** draft  
**Parent:** F06  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F6  
**Verification:** Solver / integration test

WHEN the Solver determines that the hard constraints are jointly infeasible,
THE Solver service SHALL terminate the run with an infeasible result state.


### F06.SOLV.0002 — Infeasibility explanation

**Status:** draft  
**Parent:** F06  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, F6  
**Verification:** Solver / integration test

WHEN a Solver run terminates as infeasible,
THE Solver service SHALL provide information identifying a minimal or otherwise
narrow conflicting set of hard constraints suitable for human-readable explanation.


---


## D06 — Concurrent Solver execution

### D06.SOLV.0001 — Exclusive lifecycle ownership

**Status:** draft  
**Parent:** D6  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, D6 (F4 / Decision Point 2)  
**Verification:** Architecture review / integration test

THE SolverRunner SHALL exclusively own the SolverRun instances
and SHALL NOT expose mutable SolverRun references to external components.


### D06.SOLV.0002 — Concurrent lifecycle consistency

**Status:** draft  
**Parent:** D6  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, D6 (F4 / Decision Point 2)  
**Verification:** Solver / concurrency test

WHEN concurrent events affect the same Solver run,
THE Solver service SHALL serialize lifecycle modifications
and preserve valid state transitions and consistent timestamps.


### D06.SOLV.0003 – Duplicate active run admission

**Status:** draft  
**Parent:** D6  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, D6 (F4 / Decision Point 2)  
**Verification:** Solver / concurrency test

THE SolverRunner SHALL prevent the admission of more than one
non-terminal SolverRun for the same horizon identifier and engine type.

The admission check and run registration SHALL be atomic.

WHEN an existing run reaches a terminal state,
THE SolverRunner SHALL permit a new run for the same horizon
identifier and engine type.

Rejected admission SHALL NOT modify any existing run
or leave a partially registered run.


### D06.SOLV.0004 — Independent Solver service boundary

**Status:** draft
**Parent:** D6
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, Ch 6; team architecture decision  
**Verification:** Architecture review / API integration test

THE Solver service SHALL expose an HTTP/JSON interface
through which the Backend can initiate Solver runs and
retrieve their execution status and results.

The Solver service SHALL NOT require direct access to
the application database.


### D06.SOLV.0005 — Backend-provided problem instance

**Status:** draft
**Parent:** D6
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, Ch 6 and Ch 7.1; proposed Backend/Solver integration assumption  
**Verification:** API contract test

WHEN a Solver run is requested,
THE Backend SHALL provide the problem instance
using the common JSON representation defined in Ch 7.1.

THE Solver SHALL consume that instance independently
of the Backend's persistence model.


### D06.SOLV.0006 — Separate execution identifiers

**Status:** draft
**Parent:** D6
**Predecessor:** N/A
**Successor:** N/A
**Source:** Beosztásmester specification, Ch 4, Ch 6 and Ch 7.1; proposed identifier model  
**Verification:** API contract / integration test

THE Solver service SHALL distinguish between:

- horizonId: application planning horizon;
- instanceId: concrete Solver problem instance;
- runId: one Solver execution.

THE service SHALL NOT assume that horizonId
and instanceId are interchangeable.



### D06.SOLV.0007 — Application-scoped Runner lifecycle

**Status:** draft  
**Parent:** D6  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Solver service architecture decision; D06.SOLV.0001–0003  
**Verification:** FastAPI lifespan integration test (`test_runner_lifecycle`)

THE Solver service SHALL create one application-owned SolverRunner
during FastAPI lifespan startup and SHALL make that instance available
to HTTP request handlers.

HTTP request handlers SHALL NOT create independent SolverRunner
instances for processing Solver requests.

During normal FastAPI lifespan shutdown, the application SHALL invoke
the owned SolverRunner's close operation.

The initial deployment SHALL use one Uvicorn worker process.
The in-memory Registry and admission protection SHALL be considered
process-local; multi-process coordination is not supported.


---


## NFR — Solver fault handling

### NFR.SOLV.0001 — Failed run state

**Status:** draft  
**Parent:** Non-Functional-Requirements (NFR) / Fault tolerance  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, 5.3 Hibatűrés  
**Verification:** Integration test

WHEN a technical failure prevents a Solver run from continuing,
THE Solver service SHALL leave the run in an explicitly identifiable failed state
or make the run restartable.


---


## FR — Solver interface requirements



### FR.SOLV.0001 — Self-contained Solver instance input

**Status:** draft  
**Parent:** N/A  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, Ch 7.1 and Ch 7.3; team decision: separate Solver service  
**Verification:** API contract / integration / reproducibility test

WHEN the Backend requests a Solver execution,
THE Backend SHALL provide a self-contained problem instance
conforming to the common JSON format defined in Ch 7.1.

THE Solver SHALL use the supplied instance data as its
problem input without requiring direct access to the
application database.

THE same instance SHALL be usable as input for different
Solver engines and repeated benchmark executions.


### FR.SOLV.0002 — Common solution JSON

**Status:** draft  
**Parent:** N/A  
**Predecessor:** N/A  
**Successor:** N/A  
**Source:** Beosztásmester specification, Chapter 7.2  
**Verification:** JSON schema / contract test

THE Solver service SHALL provide Solver results conforming
to the common JSON solution format defined in Chapter 7.2
of the Beosztásmester system specification.
