import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { interval, switchMap, takeWhile } from 'rxjs';
import { environment } from '../environments/environment';

interface StatusResponse {
  status: 'QUEUED' | 'RUNNING' | 'DONE';
  roster?: any[];
}

@Injectable({ providedIn: 'root' })
export class SolverService {
  private http = inject(HttpClient);

  status = signal<string>('idle');
  roster = signal<any[] | null>(null);

    startPolling(runId: string) {
        console.log(`Starting to poll for run ${runId}`);

        interval(1500)
            .pipe(
                switchMap(() => this.http.get<StatusResponse>(`${environment.apiUrl}/api/status/${runId}`)),
                takeWhile(res => res.status !== 'DONE', true)
            )
            .subscribe(res => {
                console.log(`Poll response:`, res.status);

                this.status.set(res.status);
                if (res.status === 'DONE' && res.roster) {
                    console.log('Roster received:', res.roster);
                    this.roster.set(res.roster);
                }
            });
    }

    createHorizon(orgUnitId: number, start: string, end: string) {
        return this.http.post<{ id: number }>(`${ environment.apiUrl }/api/horizons`, {
            orgUnitId, start, end
        });
    }
}