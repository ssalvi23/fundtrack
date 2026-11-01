import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';

import { HealthService, HealthStatus } from '../../core/health/health.service';

@Component({
  selector: 'app-system-status',
  imports: [DatePipe],
  templateUrl: './system-status.html',
  styleUrl: './system-status.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SystemStatus implements OnInit {
  private readonly healthService = inject(HealthService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly health = signal<HealthStatus | null>(null);
  protected readonly loading = signal(false);
  protected readonly failed = signal(false);

  ngOnInit(): void {
    this.check();
  }

  protected check(): void {
    this.loading.set(true);
    this.failed.set(false);

    this.healthService
      .getHealth()
      .pipe(
        finalize(() => this.loading.set(false)),
        // Cancels the request if the user leaves the page, like a CancellationToken.
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: (health) => this.health.set(health),
        error: () => {
          this.health.set(null);
          this.failed.set(true);
        },
      });
  }
}
