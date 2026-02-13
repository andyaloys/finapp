import { inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const menuPermissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  const user = authService.getCurrentUser();
  
  // If no user or no permissions, redirect to no-access
  if (!user || !user.menuPermissions || user.menuPermissions.length === 0) {
    router.navigate(['/no-access']);
    return false;
  }

  // Get required menu key from route data
  const requiredMenuKey = route.data['menuKey'] as string;
  
  // If no menu key required, allow access (public route)
  if (!requiredMenuKey) {
    return true;
  }

  // Check if user has permission
  const hasPermission = user.menuPermissions.includes(requiredMenuKey);
  
  if (!hasPermission) {
    router.navigate(['/no-access']);
    return false;
  }

  return true;
};
