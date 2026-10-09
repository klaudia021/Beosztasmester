import { Component, inject } from '@angular/core';
import { JobTrigger } from '../job-trigger/job-trigger';
import { RosterGrid } from '../roster-grid/roster-grid';
import { RosterService } from '../roster.service';

@Component({
  selector: 'app-roster-page',
  imports: [JobTrigger, RosterGrid],
  templateUrl: './roster-page.html'
})
export class RosterPage {
  roster = inject(RosterService);
}