import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import type { ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CrudService } from '../crud.service';
import { ENTITIES } from '../entity-config';
import type { EntityConfig, FieldConfig, Option } from '../entity-config';

@Component({
  selector: 'app-entity-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './entity-form.html'
})
export class EntityForm {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private crud = inject(CrudService);

  config = signal<EntityConfig | null>(null);
  recordId = signal<string | null>(null);   // null = create mode, a value = edit mode
  isEdit = computed(() => this.recordId() !== null);

  form: FormGroup = new FormGroup({});
  options = signal<Record<string, Option[]>>({});
  loading = signal(false);
  saving = signal(false);
  error = signal<string | null>(null);

  constructor() {
    this.route.paramMap.subscribe(params => this.init(params.get('entity'), params.get('id')));
  }

  private init(entityKey: string | null, id: string | null) {
    const config: EntityConfig | undefined = entityKey ? ENTITIES[entityKey] : undefined;
    this.config.set(config ?? null);
    this.recordId.set(id);
    this.options.set({});
    this.error.set(null);
    if (!config) return;

    // 1. Build one form control per field in the config
    const controls: Record<string, FormControl> = {};
    for (const field of config.fields) {
      const validators: ValidatorFn[] = [];
      if (field.required) validators.push(Validators.required);
      if (field.min !== undefined) validators.push(Validators.min(field.min));
      controls[field.key] = new FormControl(this.emptyValue(field), validators);
    }
    this.form = new FormGroup(controls);

    // 2. Load the choices for dropdowns / checkbox lists
    for (const field of config.fields) {
      if (field.ref) {
        this.crud.options(field.ref).subscribe(opts => {
          // A record must not be chosen as its own parent
          const visible = field.ref === config.key && id ? opts.filter(o => String(o.value) !== id) : opts;
          this.options.update(map => ({ ...map, [field.key]: visible }));
        });
      }
    }

    // 3. Edit mode: load the record and fill the form
    if (id) {
      this.loading.set(true);
      this.crud.get(config.key, id).subscribe({
        next: record => {
          this.form.patchValue(record);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Could not load the record.');
          this.loading.set(false);
        }
      });
    }
  }

  private emptyValue(field: FieldConfig): any {
    switch (field.type) {
      case 'checkbox':
        return false;
      case 'multiref':
        return [];
      case 'select':
      case 'ref':
      case 'number':
        return null;
      default:
        return '';
    }
  }

  isChecked(key: string, value: string | number): boolean {
    const current: (string | number)[] = this.form.get(key)?.value ?? [];
    return current.includes(value);
  }

  toggle(key: string, value: string | number, event: Event) {
    const control = this.form.get(key)!;
    const current: (string | number)[] = control.value ?? [];
    const checked = (event.target as HTMLInputElement).checked;
    control.setValue(checked ? [...current, value] : current.filter(v => v !== value));
  }

  errorText(field: FieldConfig): string | null {
    const control = this.form.get(field.key);
    if (!control || !control.touched || control.valid) return null;
    if (control.hasError('required')) return 'This field is required.';
    if (control.hasError('min')) return `Must be at least ${field.min}.`;
    return 'Invalid value.';
  }

  save() {
    const config = this.config();
    if (!config) return;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // Empty optional inputs are sent as null instead of ''
    const body: Record<string, unknown> = {};
    for (const field of config.fields) {
      const value = this.form.value[field.key];
      body[field.key] = value === '' ? null : value;
    }

    this.saving.set(true);
    const id = this.recordId();
    const request = id ? this.crud.update(config.key, id, body) : this.crud.create(config.key, body);

    request.subscribe({
      next: () => this.router.navigate(['/', config.key]),
      error: () => {
        this.error.set('Saving failed. Please try again.');
        this.saving.set(false);
      }
    });
  }
}