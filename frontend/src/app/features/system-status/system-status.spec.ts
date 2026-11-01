import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { HealthService, HealthStatus } from '../../core/health/health.service';
import { SystemStatus } from './system-status';

const healthy: HealthStatus = {
  status: 'Healthy',
  environment: 'Development',
  checkedAtUtc: '2026-11-01T10:00:00Z',
};

describe('SystemStatus', () => {
  async function render(
    getHealth: HealthService['getHealth'],
  ): Promise<ComponentFixture<SystemStatus>> {
    TestBed.configureTestingModule({
      imports: [SystemStatus],
      providers: [{ provide: HealthService, useValue: { getHealth } }],
    });

    const fixture = TestBed.createComponent(SystemStatus);
    await fixture.whenStable();
    return fixture;
  }

  it('shows the API status when the call succeeds', async () => {
    const fixture = await render(() => of(healthy));

    expect(fixture.nativeElement.textContent).toContain('API status: Healthy');
  });

  it('shows an error when the call fails', async () => {
    const fixture = await render(() => throwError(() => new Error('offline')));

    expect(fixture.nativeElement.querySelector('[role="alert"]')).not.toBeNull();
  });
});
