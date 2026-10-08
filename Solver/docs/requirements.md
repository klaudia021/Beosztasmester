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

