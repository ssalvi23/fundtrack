import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

/** Mirrors the JSON returned by GET /health. */
export interface HealthStatus {
  status: string;
  environment: string;
  checkedAtUtc: string;
}

@Injectable({ providedIn: 'root' })
export class HealthService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/health`;

  getHealth(): Observable<HealthStatus> {
    return this.http.get<HealthStatus>(this.url);
  }
}
