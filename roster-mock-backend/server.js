const express = require('express');
const cors = require('cors');
const app = express();
const { generateDummyRoster } = require('./dummy-data');
const { registerCrud } = require('./crud-routes');
const seed = require('./seed-data');

app.use(cors());
app.use(express.json());

const runs = {};
let counter = 0;

let horizonCounter = 0;

app.post('/api/horizons', (req, res) => {
  const id = ++horizonCounter;

  console.log(`Horizon ${id} created`);
  
  res.status(201).json({
    id,
    orgUnitId: req.body.orgUnitId,
    start: req.body.start,
    end: req.body.end,
    status: 'DRAFT'
  });
});

app.post('/api/horizons/:horizonId/solve', (req, res) => {
  const horizonId = req.params.horizonId;
  const runId = 'run-' + (++counter);
  console.log(`[${runId}] Request received — starting fake solve`);

  runs[runId] = {
    status: 'RUNNING',
    horizonId,
    roster: seed.dummyRoster
  };

  res.json({ runId });

  setTimeout(() => {
    runs[runId].status = 'DONE';
    console.log(`[${runId}] Solver done — answer ready`);
  }, 5000);
});

app.get('/api/status/:id', (req, res) => {
  const run = runs[req.params.id];
  if (!run) {
    return res.status(404).json({ error: 'not found' });
  }

  if (run.status === 'RUNNING') {
    console.log(`[${req.params.id}] Status check — still running`);
  } else {
    console.log(`[${req.params.id}] Status check — sending finished answer`);
  }

  res.json(run);
});

registerCrud(app, 'org-units', seed.orgUnits);
registerCrud(app, 'competencies', seed.competencies);
registerCrud(app, 'shift-types', seed.shiftTypes);
registerCrud(app, 'employees', seed.employees, 'E');
registerCrud(app, 'unavailabilities', seed.unavailabilities);

app.listen(3000, () => console.log('Fake backend running on http://localhost:3000'));