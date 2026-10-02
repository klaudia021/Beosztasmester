import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RosterGrid } from './roster-grid';

describe('RosterGrid', () => {
  let component: RosterGrid;
  let fixture: ComponentFixture<RosterGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RosterGrid],
    }).compileComponents();

    fixture = TestBed.createComponent(RosterGrid);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
