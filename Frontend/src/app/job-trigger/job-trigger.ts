import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SolverService } from '../solver.service';

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
    this.http.post<{ runId: string }>('http://localhost:3000/solve', {})
      .subscribe(response => {
        this.solver.startPolling(response.runId);
      });
  }
}