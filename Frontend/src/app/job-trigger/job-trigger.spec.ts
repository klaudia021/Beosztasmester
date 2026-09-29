import { ComponentFixture, TestBed } from '@angular/core/testing';
import { JobTrigger } from './job-trigger';

describe('JobTrigger', () => {
  let component: JobTrigger;
  let fixture: ComponentFixture<JobTrigger>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobTrigger],
    }).compileComponents();

    fixture = TestBed.createComponent(JobTrigger);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
