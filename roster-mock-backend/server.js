const express = require('express');
const cors = require('cors');
const app = express();
const { generateDummyRoster } = require('./dummy-data');

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

  console.log(`[${runId}] Solving for horizon ${horizonId}`);

  runs[runId] = {
    status: 'RUNNING',
    horizonId,
    roster: generateDummyRoster()
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

app.listen(3000, () => console.log('Fake backend running on http://localhost:3000'));