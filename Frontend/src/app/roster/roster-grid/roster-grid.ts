import { Component, Input, computed, Signal } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { themeQuartz } from 'ag-grid-community';
import type { ColDef } from 'ag-grid-community';

interface Assignment {
  employeeId: string;
  date: string;
  shiftType: string;
}

@Component({
  selector: 'app-roster-grid',
  standalone: true,
  imports: [AgGridAngular],
  templateUrl: './roster-grid.html',
  styleUrl: './roster-grid.css'
})

export class RosterGrid {
  theme = themeQuartz;

  @Input({ required: true }) roster!: Signal<Assignment[] | null>;

  columnDefs = computed<ColDef[]>(() => {
    const data = this.roster();
    if (!data) return [];

    const dates = [...new Set(data.map(a => a.date))].sort();

    return [
      { field: 'employeeId', headerName: 'Employee', pinned: 'left' },
      ...dates.map(date => ({
        field: date,
        headerName: date,
        cellClassRules: {
          'shift-day': (params: any) => params.value === 'D',
          'shift-afternoon': (params: any) => params.value === 'A',
          'shift-night': (params: any) => params.value === 'N',
          'shift-off': (params: any) => params.value === 'OFF'
        }
      }))
    ];
  });

  rowData = computed(() => {
    const data = this.roster();
    if (!data) return [];

    const rowsByEmployee: Record<string, any> = {};

    for (const assignment of data) {
      if (!rowsByEmployee[assignment.employeeId]) {
        rowsByEmployee[assignment.employeeId] = { employeeId: assignment.employeeId };
      }
      rowsByEmployee[assignment.employeeId][assignment.date] = assignment.shiftType;
    }

    return Object.values(rowsByEmployee);
  });
}