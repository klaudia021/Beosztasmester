import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CrudService } from '../crud.service';
import { ENTITIES } from '../entity-config';
import type { EntityConfig, FieldConfig, Option } from '../entity-config';

@Component({
  selector: 'app-entity-list',
  imports: [RouterLink],
  templateUrl: './entity-list.html'
})
export class EntityList {
  private route = inject(ActivatedRoute);
  private crud = inject(CrudService);

  config = signal<EntityConfig | null>(null);
  rows = signal<any[]>([]);
  options = signal<Record<string, Option[]>>({});
  loading = signal(false);
  error = signal<string | null>(null);

  columns = computed(() => this.config()?.fields.filter(field => !field.hideInList) ?? []);

  constructor() {
    // Runs now, and again every time the :entity part of the URL changes
    this.route.paramMap.subscribe(params => this.load(params.get('entity')));
  }

  private load(entityKey: string | null) {
    const config: EntityConfig | undefined = entityKey ? ENTITIES[entityKey] : undefined;
    this.config.set(config ?? null);
    this.rows.set([]);
    this.options.set({});
    this.error.set(null);
    if (!config) return;

    this.loading.set(true);
    this.crud.list(config.key).subscribe({
      next: rows => {
        this.rows.set(rows);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load records. Is the backend running?');
        this.loading.set(false);
      }
    });

    // For reference fields we load the related records so we can show names instead of IDs
    for (const field of config.fields) {
      if (field.ref) {
        this.crud.options(field.ref).subscribe(opts =>
          this.options.update(map => ({ ...map, [field.key]: opts }))
        );
      }
    }
  }

  display(row: any, field: FieldConfig): string {
    const value = row[field.key];
    if (value === null || value === undefined || value === '') return '—';

    switch (field.type) {
      case 'checkbox':
        return value ? 'Yes' : 'No';
      case 'ref':
        return this.labelFor(field.key, value);
      case 'multiref':
        return (value as (string | number)[]).map(v => this.labelFor(field.key, v)).join(', ') || '—';
      default:
        return String(value);
    }
  }

  private labelFor(fieldKey: string, value: string | number): string {
    const option = this.options()[fieldKey]?.find(o => o.value === value);
    return option ? option.label : String(value);
  }
}