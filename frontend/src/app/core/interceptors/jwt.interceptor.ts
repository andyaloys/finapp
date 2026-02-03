import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { YearService } from '../services/year.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const yearService = inject(YearService);
  const token = authService.getToken();
  const selectedYear = yearService.getSelectedYear();

  // Skip adding year parameter for auth endpoints
  const skipYearParam = req.url.includes('/auth/');

  if (token) {
    let clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    // Add year query parameter to non-auth requests
    if (!skipYearParam) {
      const params = clonedReq.params.set('tahun', selectedYear.toString());
      clonedReq = clonedReq.clone({ params });
    }

    return next(clonedReq);
  }

  // For requests without token, still add year parameter if not auth endpoint
  if (!skipYearParam) {
    const params = req.params.set('tahun', selectedYear.toString());
    req = req.clone({ params });
  }

  return next(req);
};
