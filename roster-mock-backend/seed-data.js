const orgUnits = [
    { id: 1, name: 'Internal Medicine', parentId: null },
    { id: 2, name: 'Ward A', parentId: 1 },
    { id: 3, name: 'Ward B', parentId: 1 }
];

const competencies = [
    { id: 1, code: 'RN', name: 'Registered Nurse' },
    { id: 2, code: 'SENIOR', name: 'Senior Nurse' }
];

const shiftTypes = [
    { id: 1, orgUnitId: 1, code: 'D', start: '06:00', end: '14:00', durationMin: 480, night: false },
    { id: 2, orgUnitId: 1, code: 'A', start: '14:00', end: '22:00', durationMin: 480, night: false },
    { id: 3, orgUnitId: 1, code: 'N', start: '22:00', end: '06:00', durationMin: 480, night: true }
];

const employees = [
    { id: 'E001', name: 'Kovács Anna', orgUnitId: 2, status: 'ACTIVE', contractHoursPerWeek: 40, startDate: '2022-03-01', endDate: null, competencyIds: [1, 2] },
    { id: 'E002', name: 'Nagy Péter', orgUnitId: 2, status: 'ACTIVE', contractHoursPerWeek: 40, startDate: '2023-01-09', endDate: null, competencyIds: [1] },
    { id: 'E003', name: 'Szabó Eszter', orgUnitId: 2, status: 'ACTIVE', contractHoursPerWeek: 30, startDate: '2021-09-15', endDate: null, competencyIds: [1, 2] },
    { id: 'E004', name: 'Tóth Gábor', orgUnitId: 3, status: 'ACTIVE', contractHoursPerWeek: 40, startDate: '2024-02-01', endDate: null, competencyIds: [1] },
    { id: 'E005', name: 'Varga Zsófia', orgUnitId: 3, status: 'ACTIVE', contractHoursPerWeek: 40, startDate: '2020-06-01', endDate: null, competencyIds: [1, 2] },
    { id: 'E006', name: 'Horváth Bence', orgUnitId: 3, status: 'INACTIVE', contractHoursPerWeek: 20, startDate: '2025-05-12', endDate: '2026-06-30', competencyIds: [1] }
];

const unavailabilities = [
    { id: 1, employeeId: 'E002', type: 'VACATION', start: '2026-03-05', end: '2026-03-09', reason: null }
];

const dummyRoster = [
    { "employeeId": "E001", "date": "2026-03-02", "shiftType": "D" },
    { "employeeId": "E001", "date": "2026-03-03", "shiftType": "D" },
    { "employeeId": "E001", "date": "2026-03-04", "shiftType": "OFF" },
    { "employeeId": "E001", "date": "2026-03-05", "shiftType": "N" },
    { "employeeId": "E001", "date": "2026-03-06", "shiftType": "N" },
    { "employeeId": "E001", "date": "2026-03-07", "shiftType": "OFF" },
    { "employeeId": "E001", "date": "2026-03-08", "shiftType": "A" },

    { "employeeId": "E002", "date": "2026-03-02", "shiftType": "N" },
    { "employeeId": "E002", "date": "2026-03-03", "shiftType": "OFF" },
    { "employeeId": "E002", "date": "2026-03-04", "shiftType": "D" },
    { "employeeId": "E002", "date": "2026-03-05", "shiftType": "D" },
    { "employeeId": "E002", "date": "2026-03-06", "shiftType": "A" },
    { "employeeId": "E002", "date": "2026-03-07", "shiftType": "A" },
    { "employeeId": "E002", "date": "2026-03-08", "shiftType": "OFF" },

    { "employeeId": "E003", "date": "2026-03-02", "shiftType": "A" },
    { "employeeId": "E003", "date": "2026-03-03", "shiftType": "A" },
    { "employeeId": "E003", "date": "2026-03-04", "shiftType": "N" },
    { "employeeId": "E003", "date": "2026-03-05", "shiftType": "OFF" },
    { "employeeId": "E003", "date": "2026-03-06", "shiftType": "D" },
    { "employeeId": "E003", "date": "2026-03-07", "shiftType": "D" },
    { "employeeId": "E003", "date": "2026-03-08", "shiftType": "D" },

    { "employeeId": "E004", "date": "2026-03-02", "shiftType": "OFF" },
    { "employeeId": "E004", "date": "2026-03-03", "shiftType": "N" },
    { "employeeId": "E004", "date": "2026-03-04", "shiftType": "N" },
    { "employeeId": "E004", "date": "2026-03-05", "shiftType": "A" },
    { "employeeId": "E004", "date": "2026-03-06", "shiftType": "OFF" },
    { "employeeId": "E004", "date": "2026-03-07", "shiftType": "D" },
    { "employeeId": "E004", "date": "2026-03-08", "shiftType": "A" },

    { "employeeId": "E005", "date": "2026-03-02", "shiftType": "D" },
    { "employeeId": "E005", "date": "2026-03-03", "shiftType": "OFF" },
    { "employeeId": "E005", "date": "2026-03-04", "shiftType": "A" },
    { "employeeId": "E005", "date": "2026-03-05", "shiftType": "N" },
    { "employeeId": "E005", "date": "2026-03-06", "shiftType": "N" },
    { "employeeId": "E005", "date": "2026-03-07", "shiftType": "OFF" },
    { "employeeId": "E005", "date": "2026-03-08", "shiftType": "D" },

    { "employeeId": "E006", "date": "2026-03-02", "shiftType": "A" },
    { "employeeId": "E006", "date": "2026-03-03", "shiftType": "D" },
    { "employeeId": "E006", "date": "2026-03-04", "shiftType": "OFF" },
    { "employeeId": "E006", "date": "2026-03-05", "shiftType": "D" },
    { "employeeId": "E006", "date": "2026-03-06", "shiftType": "A" },
    { "employeeId": "E006", "date": "2026-03-07", "shiftType": "N" },
    { "employeeId": "E006", "date": "2026-03-08", "shiftType": "OFF" },

    { "employeeId": "E007", "date": "2026-03-02", "shiftType": "OFF" },
    { "employeeId": "E007", "date": "2026-03-03", "shiftType": "D" },
    { "employeeId": "E007", "date": "2026-03-04", "shiftType": "OFF" },
    { "employeeId": "E007", "date": "2026-03-05", "shiftType": "A" },
    { "employeeId": "E007", "date": "2026-03-06", "shiftType": "OFF" },
    { "employeeId": "E007", "date": "2026-03-07", "shiftType": "D" },
    { "employeeId": "E007", "date": "2026-03-08", "shiftType": "N" },

    { "employeeId": "E008", "date": "2026-03-02", "shiftType": "D" },
    { "employeeId": "E008", "date": "2026-03-03", "shiftType": "D" },
    { "employeeId": "E008", "date": "2026-03-04", "shiftType": "A" },
    { "employeeId": "E008", "date": "2026-03-05", "shiftType": "A" },
    { "employeeId": "E008", "date": "2026-03-06", "shiftType": "OFF" },
    { "employeeId": "E008", "date": "2026-03-07", "shiftType": "N" },
    { "employeeId": "E008", "date": "2026-03-08", "shiftType": "A" }
];

module.exports = { orgUnits, competencies, shiftTypes, employees, unavailabilities, dummyRoster };