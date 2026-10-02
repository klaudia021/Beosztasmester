import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SolverService } from '../solver.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-job-trigger',
  imports: [],
  templateUrl: './job-trigger.html',
  styleUrl: './job-trigger.css'
})
export class JobTrigger {
  private http = inject(HttpClient);
  private solver = inject(SolverService);

  status = this.solver.status;
  roster = this.solver.roster;

  start() {
    this.solver.createHorizon(5, '2026-03-01', '2026-03-28').subscribe({
      next: horizon => {
        console.log('Horizon created:', horizon.id);
        
        this.http.post<{ runId: string }>(`${ environment.apiUrl }/api/solve`, {})
          .subscribe({
            next: response => this.solver.startPolling(response.runId),
            error: err => console.error('Solve failed:', err)
          });
      },
      error: err => console.error('Horizon creation failed:', err)
    });
  }
}