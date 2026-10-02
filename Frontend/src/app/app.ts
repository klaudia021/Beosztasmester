import { Component, signal, inject } from '@angular/core';
import { JobTrigger } from './job-trigger/job-trigger';
import { RosterGrid } from './roster-grid/roster-grid';
import { SolverService } from './solver.service';

@Component({
  imports: [JobTrigger, RosterGrid],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('Frontend');

  solver = inject(SolverService);
}
