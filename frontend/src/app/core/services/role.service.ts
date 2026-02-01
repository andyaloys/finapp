import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, PagedResult } from '../../shared/models/common.model';
import { Role, CreateRoleDto, UpdateRoleDto, RoleSuboutput, AssignSuboutputsDto } from '../models/role.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiUrl = `${environment.apiUrl}/role`;

  constructor(private http: HttpClient) {}

  getRoles(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ''): Observable<ApiResponse<PagedResult<Role>>> {
    return this.http.get<ApiResponse<PagedResult<Role>>>(
      `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}&searchTerm=${searchTerm}`
    );
  }

  getAllRoles(): Observable<ApiResponse<Role[]>> {
    return this.http.get<ApiResponse<Role[]>>(`${this.apiUrl}/all`);
  }

  getRoleById(id: string): Observable<ApiResponse<Role>> {
    return this.http.get<ApiResponse<Role>>(`${this.apiUrl}/${id}`);
  }

  createRole(role: CreateRoleDto): Observable<ApiResponse<Role>> {
    return this.http.post<ApiResponse<Role>>(this.apiUrl, role);
  }

  updateRole(id: string, role: UpdateRoleDto): Observable<ApiResponse<Role>> {
    return this.http.put<ApiResponse<Role>>(`${this.apiUrl}/${id}`, role);
  }

  deleteRole(id: string): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }

  getRoleSuboutputs(roleId: string): Observable<ApiResponse<RoleSuboutput[]>> {
    return this.http.get<ApiResponse<RoleSuboutput[]>>(`${this.apiUrl}/${roleId}/suboutputs`);
  }

  assignSuboutputs(roleId: string, dto: AssignSuboutputsDto): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(`${this.apiUrl}/${roleId}/suboutputs`, dto);
  }
}
