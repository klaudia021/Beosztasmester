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
    const horizonId = 1;
    console.log('Sending solve request...');

    this.rosterService.solve(horizonId).subscribe({
      next: response => this.rosterService.startPolling(response.runId),
      error: err => {
        console.error('Solve failed:', err);
        this.rosterService.status.set('FAILED');
      }
    });
  }
}