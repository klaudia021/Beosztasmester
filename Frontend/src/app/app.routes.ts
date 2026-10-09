import { Routes } from '@angular/router';
import { RosterPage } from './roster/roster-page/roster-page';
import { EntityList } from './crud/entity-list/entity-list';
import { EntityForm } from './crud/entity-form/entity-form';
import { EntityDelete } from './crud/entity-delete/entity-delete';

export const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'roster' },
    { path: 'roster', component: RosterPage },

    // CRUD pages, shared by every entity (:entity = employees, competencies, ...)
    { path: ':entity', component: EntityList },                 // Read
    { path: ':entity/new', component: EntityForm },             // Create
    { path: ':entity/:id/edit', component: EntityForm },        // Update
    { path: ':entity/:id/delete', component: EntityDelete },    // Delete

    { path: '**', redirectTo: 'roster' }
];