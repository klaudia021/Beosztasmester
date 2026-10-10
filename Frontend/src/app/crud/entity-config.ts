export type FieldType =
    | 'text'
    | 'number'
    | 'date'
    | 'time'
    | 'checkbox'
    | 'select'
    | 'ref'
    | 'multiref';

export interface Option {
    value: string | number;
    label: string;
}

export interface FieldConfig {
    key: string;          // JSON property name sent to / received from the API
    label: string;        // text shown in forms and table headers
    type: FieldType;
    required?: boolean;
    min?: number;
    options?: Option[];   // only for 'select': fixed list of choices
    ref?: string;         // only for 'ref' / 'multiref': entity key whose records are the choices
    hideInList?: boolean; // true = not shown as a table column
}

export interface EntityConfig {
    key: string;          // used in the URL (/employees) AND the API path (/api/employees)
    title: string;        // plural name, e.g. "Employees"
    singular: string;     // e.g. "Employee"
    labelField: string;   // which field names a record in dropdowns and the delete page
    fields: FieldConfig[];
}

export const ENTITIES: Record<string, EntityConfig> = {
    employees: {
        key: 'employees',
        title: 'Employees',
        singular: 'Employee',
        labelField: 'name',
        fields: [
            { key: 'name', label: 'Name', type: 'text', required: true },
            { key: 'orgUnitId', label: 'Org unit', type: 'ref', ref: 'org-units', required: true },
            {
                key: 'status',
                label: 'Status',
                type: 'select',
                required: true,
                options: [
                    { value: 'ACTIVE', label: 'Active' },
                    { value: 'INACTIVE', label: 'Inactive' }
                ]
            },
            { key: 'contractHoursPerWeek', label: 'Contract hours / week', type: 'number', required: true, min: 1 },
            { key: 'startDate', label: 'Start date', type: 'date', required: true },
            { key: 'endDate', label: 'End date', type: 'date' },
            { key: 'competencyIds', label: 'Competencies', type: 'multiref', ref: 'competencies' }
        ]
    },

    competencies: {
        key: 'competencies',
        title: 'Competencies',
        singular: 'Competency',
        labelField: 'name',
        fields: [
            { key: 'code', label: 'Code', type: 'text', required: true },
            { key: 'name', label: 'Name', type: 'text', required: true }
        ]
    },

    'shift-types': {
        key: 'shift-types',
        title: 'Shift types',
        singular: 'Shift type',
        labelField: 'code',
        fields: [
            { key: 'orgUnitId', label: 'Org unit', type: 'ref', ref: 'org-units', required: true },
            { key: 'code', label: 'Code', type: 'text', required: true },
            { key: 'start', label: 'Start time', type: 'time', required: true },
            { key: 'end', label: 'End time', type: 'time', required: true },
            { key: 'durationMin', label: 'Duration (minutes)', type: 'number', required: true, min: 1 },
            { key: 'night', label: 'Night shift', type: 'checkbox' }
        ]
    },

    'org-units': {
        key: 'org-units',
        title: 'Org units',
        singular: 'Org unit',
        labelField: 'name',
        fields: [
            { key: 'name', label: 'Name', type: 'text', required: true },
            { key: 'parentId', label: 'Parent unit', type: 'ref', ref: 'org-units' }
        ]
    },

    unavailabilities: {
        key: 'unavailabilities',
        title: 'Unavailability',
        singular: 'Unavailability',
        labelField: 'type',
        fields: [
            { key: 'employeeId', label: 'Employee', type: 'ref', ref: 'employees', required: true },
            {
                key: 'type',
                label: 'Type',
                type: 'select',
                required: true,
                options: [
                    { value: 'VACATION', label: 'Vacation' },
                    { value: 'SICK', label: 'Sick leave' },
                    { value: 'TRAINING', label: 'Training' }
                ]
            },
            { key: 'start', label: 'From', type: 'date', required: true },
            { key: 'end', label: 'To', type: 'date', required: true },
            { key: 'reason', label: 'Reason', type: 'text' }
        ]
    }
};