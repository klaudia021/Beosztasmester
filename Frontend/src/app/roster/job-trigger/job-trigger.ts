import { Component, inject } from '@angular/core';
import { RosterService } from '../roster.service';

@Component({
  selector: 'app-job-trigger',
  imports: [],
  templateUrl: './job-trigger.html'
})
export class JobTrigger {
  private rosterService = inject(RosterService);

  status = this.rosterService.status;

  start() {
    this.rosterService.createHorizon(5, '2026-03-01', '2026-03-28').subscribe({
      next: horizon => {
        console.log('Horizon created:', horizon.id);

        this.rosterService.solve(horizon.id).subscribe({
          next: response => this.rosterService.startPolling(response.runId),
          error: err => console.error('Solve failed:', err)
        });
      },
      error: err => console.error('Horizon creation failed:', err)
    });
  }
}