# Return a dummy roster – megvalósítás és tesztelés

## Hatókör

Csak a tickethez tartozó determinisztikus dummy roster provider készült el.
Interfész: `app/solver/roster/dummy.py`,
`get_dummy_assignments() -> list[dict[str, int | str]]`.
Minden sor pontosan `employeeId` (egész szám), `date` és `shiftType` (szöveg)
mezőket tartalmaz.
A meglévő DummySolverEngine alapértelmezésben már ezt a függvényt hívja;
az engine, Runner, HTTP API, frontend és backend működését nem módosítottuk.

## Eltérés a csoporttársi javaslattól

Klaudia hivatkozott `dummyroster.txt` fájlja és a teljes 7.2 specifikáció nem
szerepel a kapott projektben. Emiatt nem állítjuk, hogy ezt a fájlt reprodukáljuk.
A projekt `roster-mock-backend/dummy-data.js` mintájából vettük át a dolgozókat
(1–5), dátumokat (2026-03-01–07) és műszakkódokat (D, A, N, OFF).
A véletlen választást determinisztikus forgás váltja fel:
`shift_types[(employee_index + day - 1) % 4]`.
Az eredmény 35 sor, dolgozónként és dátumonként egy bejegyzéssel.
Minden hívás új listát és új szótárakat ad vissza.

Ez fix demonstrációs adat: a kérésben kapott időszakot és dolgozókat nem követi,
és nem garantál szabályoknak megfelelő beosztást. Csak az assignments-listát
adja, nem a teljes 7.2 Solution objektumot: ez megfelel a ticketnek és a meglévő
Runner listaalapú eredménykezelésének. A HTTP szerződés dokumentumában szereplő
korábbi „default Dummy Engine fails” megjegyzés ezzel már elavult; a dokumentum
többi nyitott szerződéses kérdését ez a ticket nem rendezi.

## Automatizált tesztelés

A `Solver` könyvtárból, telepített Python mellett, külső csomagok nélkül:

```sh
python -m unittest discover -s tests -p test_dummy_roster.py -v
```

Három teszt ellenőrzi a JSON-formátumot, a teljes 5×7 lefedettséget, a dátumokat,
műszakkódokat, konkrét mintaértékeket, ismételhetőséget és módosítás elleni
függetlenséget. Ezeket a teszteket a fejlesztés során sikeresen futtattuk.

A teljes Solver tesztcsomaghoz a projekt szerinti Python 3.14 és dev függőségek:

```sh
python -m pip install -e ".[dev]"
python -m pytest
```

A `test_default_dummy_provider_completes_run` az eredeti providerrel ellenőrzi,
hogy a DummySolverEngine a Runneren át `completed` állapotot és 35 soros
eredményt ad. A teljes suite és ez az engine-integrációs teszt itt nem futott:
a rendelkezésre álló környezet Python 3.12-es, pytest nélkül. A projekt
Python-verzióját és függőségeit emiatt nem módosítottuk.

## Kézi API-teszt – csak a Solver szükséges

A projekt gyökeréből Dockerrel:

```sh
docker compose up --build solver
```

1. Nyisd meg a `http://localhost:8000/docs` címet.
2. A `POST /solve` alatt a Try it out gombbal küldd ezt:

```json
{
  "horizonId": 1,
  "engineType": "dummy",
  "instance": {"instanceId": 101}
}
```

3. Várt válasz: HTTP 202 és `runId`. Másold ki az azonosítót.
4. A `GET /solver-runs/{run_id}` hívásban használd ezt az azonosítót.
   Kb. 5 másodperc után `status: "completed"` várható; előtte accepted/running.
5. A `GET /solver-runs/{run_id}/result` eredményében `result` egy 35 elemű lista.
   Első sora: `{"employeeId":1,"date":"2026-03-01","shiftType":"D"}`.
   Utolsó sora: `{"employeeId":5,"date":"2026-03-07","shiftType":"N"}`.
6. Ismételd meg a POST-ot az első futás befejezése után új, backend által
   kiosztott `instanceId`-val (ez a runId is):
   pontosan azonos result listát kell kapnod. A status endpoint továbbra sem
   tartalmaz result mezőt.

## Módosított fájlok

- `app/solver/roster/dummy.py`
- `tests/test_dummy_roster.py` (új)
- `tests/test_solver_execution.py` (egy célzott integrációs teszt)
- `docs/dummy_roster_ticket.md` (ez a dokumentáció)
