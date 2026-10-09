import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { ENTITIES } from './entity-config';
import type { Option } from './entity-config';

@Injectable({ providedIn: 'root' })
export class CrudService {
    private http = inject(HttpClient);

    private url(entity: string): string {
        return `${environment.apiUrl}/api/${entity}`;
    }

    list(entity: string): Observable<any[]> {
        return this.http.get<any[]>(this.url(entity));
    }

    get(entity: string, id: string): Observable<any> {
        return this.http.get<any>(`${this.url(entity)}/${id}`);
    }

    create(entity: string, body: unknown): Observable<any> {
        return this.http.post<any>(this.url(entity), body);
    }

    update(entity: string, id: string, body: unknown): Observable<any> {
        return this.http.put<any>(`${this.url(entity)}/${id}`, body);
    }

    remove(entity: string, id: string): Observable<void> {
        return this.http.delete<void>(`${this.url(entity)}/${id}`);
    }

    // Turns a list of records into dropdown choices: { value: id, label: name }
    options(entity: string): Observable<Option[]> {
        const labelField = ENTITIES[entity]?.labelField ?? 'id';
        return this.list(entity).pipe(
            map(rows => rows.map(row => ({ value: row.id, label: String(row[labelField] ?? row.id) })))
        );
    }
}