import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CrudService } from '../crud.service';
import { ENTITIES } from '../entity-config';
import type { EntityConfig } from '../entity-config';

@Component({
  selector: 'app-entity-delete',
  imports: [RouterLink],
  templateUrl: './entity-delete.html'
})
export class EntityDelete {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private crud = inject(CrudService);

  config = signal<EntityConfig | null>(null);
  record = signal<any | null>(null);
  loading = signal(false);
  deleting = signal(false);
  error = signal<string | null>(null);

  // The human-readable name of the record, e.g. "Kovács Anna"
  name = computed(() => {
    const config = this.config();
    const record = this.record();
    return config && record ? String(record[config.labelField] ?? record.id) : '';
  });

  constructor() {
    this.route.paramMap.subscribe(params => this.load(params.get('entity'), params.get('id')));
  }

  private load(entityKey: string | null, id: string | null) {
    const config: EntityConfig | undefined = entityKey ? ENTITIES[entityKey] : undefined;
    this.config.set(config ?? null);
    this.record.set(null);
    this.error.set(null);
    if (!config || !id) return;

    this.loading.set(true);
    this.crud.get(config.key, id).subscribe({
      next: record => {
        this.record.set(record);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load the record.');
        this.loading.set(false);
      }
    });
  }

  confirmDelete() {
    const config = this.config();
    const record = this.record();
    if (!config || !record) return;

    this.deleting.set(true);
    this.crud.remove(config.key, String(record.id)).subscribe({
      next: () => this.router.navigate(['/', config.key]),
      error: () => {
        this.error.set('Deleting failed. Please try again.');
        this.deleting.set(false);
      }
    });
  }
}