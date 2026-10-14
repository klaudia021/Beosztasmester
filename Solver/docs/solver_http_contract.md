# Solver HTTP Contract

**Status:** DRAFT – for Backend/Solver integration review  
**Primary source:** `beosztasmester-specifikacio.md`, Chapters 6–7  
**Derived requirements:** `Solver/docs/requirements.md`  
**Date:** 2026-10-12

**Integration change notice (2026-10-12):** Section 4 changes the previously shared GET response contract. `GET /solver-runs/{runId}` no longer includes `result`;  
a separate `GET /solver-runs/{runId}/result` is proposed. Backend implementers must review this change before relying on either GET response shape.  
Both GET endpoints are implemented.

## 1. Scope

This document proposes the internal HTTP/JSON contract between the .NET Backend and the Python Solver service.

The **Beosztásmester System Specification remains authoritative**. Solver requirements are derived from it, supplemented by implementation assumptions and proposed features where necessary. They are not additional obligations imposed on other team members.

The purpose of this draft is to support implementation and integration. Open details are subject to team review.

The Angular ↔ .NET API and database schema are outside this contract.

## 2. Data and identifiers

**Requirements:** `FR.SOLV.0001`, `FR.SOLV.0002`, `D06.SOLV.0005`, `D06.SOLV.0006`

The interface uses the system specification's common JSON formats:

- **Ch 7.1 – Instance:** self-contained scheduling problem, supplied as Solver input.
- **Ch 7.2 – Solution:** algorithm result, including assignments and quality metrics.

**Proposed:** The Backend assembles and sends the complete Ch 7.1 instance. The Python Solver does not directly access the application database.

| Identifier | Purpose |
|---|---|
| `horizonId` | Backend planning horizon; run admission/correlation |
| `instanceId` | Ch 7.1 problem instance; reproducibility and comparison |
| `runId` | Unique execution; status and result polling |

`runId` is the only HTTP run-lookup key. One instance may have multiple runs, including different Solver Engines.

The three identifiers are conceptually distinct; no additional lookup API is proposed for `instanceId`.

## 3. POST /solve

**Requirements:** `F04.SOLV.0001`, `F04.SOLV.0007`, `F04.SOLV.0008`, `D06.SOLV.0003`, `FR.SOLV.0001`

Starts one asynchronous Solver execution.

### Proposed request

```json
{
  "horizonId": "73d9882a-5b2e-4c4e-ae0b-65b1c6bdbaf8",
  "engineType": "dummy",
  "instance": {
    "instanceId": "M-01",
    "horizon": {
      "start": "2026-03-01",
      "days": 28
    },
    "shiftTypes": [],
    "skills": [],
    "employees": [],
    "demand": [],
    "requests": [],
    "rules": []
  }
}
```

The `instance` is abbreviated for illustration; its complete content follows Ch 7.1. Empty arrays do not imply that an empty scheduling problem is valid.

The outer envelope supplies execution metadata; it does not modify the Ch 7.1 format.

### Successful response

**HTTP 202 Accepted**

```json
{
  "runId": "0fbd20fb-4b16-40ef-936e-f81a7f07f5aa"
}
```

The run is registered and immediately queryable. HTTP 202 does not indicate a completed or feasible solution.

**Proposed admission errors:**

| Condition | HTTP |
|---|---|
| Invalid request | 422 |
| Duplicate active `(horizonId, engineType)` | 409 |
| Internal service failure | 500 / 503 |

Error bodies and failure recovery remain open.

## 4. Run status and result retrieval

**Requirements:** `F04.SOLV.0002`, `F04.SOLV.0007`, `F04.SOLV.0008`, `D06.SOLV.0001`, `D06.SOLV.0008`, `FR.SOLV.0002`

### GET /solver-runs/{runId} — status polling

**HTTP 200 OK.** Returns lifecycle status and metadata; **no `result` field**.

```json
{
  "runId": "0fbd20fb-4b16-40ef-936e-f81a7f07f5aa",
  "horizonId": "73d9882a-5b2e-4c4e-ae0b-65b1c6bdbaf8",
  "instanceId": "M-01",
  "engineType": "dummy",
  "status": "running",
  "acceptedAt": "2026-10-12T06:30:00Z",
  "startedAt": "2026-10-12T06:30:01Z",
  "finishedAt": null
}
```

### GET /solver-runs/{runId}/result — result retrieval

Returns lifecycle metadata together with the available result, read atomically
from the Runner. The result is **not limited to `completed`**: cancelled and
timed-out runs may retain a best-available solution.

**Target response (Ch 7.2 solution; not yet implemented):**

```json
{
  "runId": "0fbd20fb-4b16-40ef-936e-f81a7f07f5aa",
  "horizonId": "73d9882a-5b2e-4c4e-ae0b-65b1c6bdbaf8",
  "instanceId": "M-01",
  "engineType": "cpsat",
  "status": "completed",
  "acceptedAt": "2026-10-12T06:30:00Z",
  "startedAt": "2026-10-12T06:30:01Z",
  "finishedAt": "2026-10-12T06:30:44Z",
  "result": {
    "instanceId": "M-01",
    "solverId": "cpsat-v3",
    "status": "FEASIBLE",
    "runtimeSec": 42.7,
    "objective": 318,
    "lowerBound": 290,
    "hardViolations": 0,
    "softBreakdown": {
      "S1": 120,
      "S2": 48,
      "S3": 90,
      "S4": 40,
      "S5": 20,
      "S6": 0
    },
    "assignments": [
      {
        "employeeId": "E001",
        "date": "2026-03-01",
        "shiftType": "D"
      }
    ]
  }
}
```

The nested `result` SHALL conform to the common Solution JSON format in Ch 7.2.
**Current implementation limitation:** The Runner currently stores only
`list[dict[str, str]]`, not the Ch 7.2 solution object.

The result endpoint and its final response/error details remain subject to
Backend/Solver agreement. In particular, behavior when no result is available
is not yet specified.

Lifecycle status and solution status are distinct (e.g. `completed` versus
`FEASIBLE`). Unknown `runId` returns HTTP 404 (response body TBD).

**Compatibility note:** The previously shared draft placed `result` inside
`GET /solver-runs/{runId}`. Splitting these calls is a breaking response-contract
change and requires Backend coordination before integration.

## 5. Execution semantics

**Requirements:** `F04.SOLV.0005`, `F04.SOLV.0006`, `D06.SOLV.0001`–`0003`

The SolverRunner owns run lifecycles, scheduling and result retention. Engines perform calculations without directly changing run states.

Supported lifecycle states:

`accepted`, `running`, `completed`, `cancelled`, `timed_out`, `infeasible`, `failed`.

At most one non-terminal run per `(horizonId, engineType)` is admitted. Completed or otherwise terminal runs remain queryable and permit resubmission.

The initial implementation uses an in-memory Registry and one execution worker. A single service process is assumed; restart persistence and multi-process coordination are not yet supported.

Ch 7.2 `runtimeSec` represents algorithm execution time, not necessarily the complete HTTP-to-result duration. Exact benchmark timing semantics remain open.

## 6. Current implementation boundary

**Implemented:**

- FastAPI lifespan ownership of a single in-memory SolverRunner
- Run registration, duplicate-active-run protection, UUIDs and immutable snapshots
- Serialized background execution, lifecycle timestamps and result retention
- POST `/solve`: partial request-envelope validation, engine selection, run admission and scheduling
- HTTP 202 for accepted runs; HTTP 409 for duplicate active runs; HTTP 422 for invalid requests or unsupported engines
- GET `/solver-runs/{runId}`: status and lifecycle metadata without result data; HTTP 404 for unknown runId.
- GET `/solver-runs/{runId}/result`: lifecycle metadata and stored result; HTTP 404 for unknown runId.

**Current limitations:**

- Only `engineType: "dummy"` is supported. The production roster provider is not implemented; the default Dummy Engine currently fails without it.
- Complete Ch 7.1 validation, Ch 7.2 solution generation, Greedy/CP-SAT engines, cancellation and timeout APIs are pending.
- Run storage is process-local and non-persistent; scheduling failure after successful admission has no recovery mechanism yet.

HTTP 202 confirms admission and scheduling, not execution completion. A run may remain
ACCEPTED while waiting for the single worker. Engine selection is separate from
SolverRunner lifecycle management.

## 7. Open items for team review

The principal decisions requiring Backend/Solver agreement are:

1. **Input:** Complete Ch 7.1 instance supplied by the Backend inside the proposed POST envelope.
2. **Output:** Separate status polling and result retrieval GET calls (breaking change from the previously shared draft); final Ch 7.2 payload and no-result HTTP behavior require agreement.
3. **Selection:** `engineType` values, defaults and execution options.
4. **Errors:** HTTP error responses, cancellation and timeout semantics.
5. **Integration:** Backend acknowledgment of the changed GET contract, endpoint naming, authentication and deployment assumptions.
6. **Post-demo:** Finalize no-result HTTP semantics and F6 infeasibility diagnostics representation, aligned with the Ch 7.2 Solution JSON.

Development may proceed against this draft. Changes agreed during review will be reflected first in the corresponding Solver requirements, retaining their history and traceability, and then in this contract, implementation and tests.

**Verification:** API contract tests and a later requirements → implementation → test traceability review.
