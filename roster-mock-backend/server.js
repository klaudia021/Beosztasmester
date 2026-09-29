const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors());
app.use(express.json());

const runs = {};
let counter = 0;

app.post('/solve', (req, res) => {
  const runId = 'run-' + (++counter);
  console.log(`[${runId}] Request received — starting fake solve`);

  runs[runId] = {
    status: 'RUNNING',
    roster: [
      { employeeId: 'E001', date: '2026-03-01', shiftType: 'D' },
      { employeeId: 'E002', date: '2026-03-01', shiftType: 'N' }
    ]
  };

  res.json({ runId });

  setTimeout(() => {
    runs[runId].status = 'DONE';
    console.log(`[${runId}] Solver done — answer ready`);
  }, 5000);
});

app.get('/status/:id', (req, res) => {
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