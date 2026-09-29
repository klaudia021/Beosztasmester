import { Component, signal } from '@angular/core';
import { JobTrigger } from './job-trigger/job-trigger';

@Component({
  imports: [JobTrigger],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('Frontend');

  
}
