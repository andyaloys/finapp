export interface Role {
  id: string;
  name: string;
  description?: string;
  isAdmin: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface RoleSuboutput {
  id: string;
  roleId: string;
  kodeSuboutput: string;
  createdAt: Date;
}

export interface CreateRoleDto {
  name: string;
  description?: string;
  isAdmin: boolean;
}

export interface UpdateRoleDto {
  name?: string;
  description?: string;
  isAdmin?: boolean;
}

export interface AssignSuboutputsDto {
  kodeSuboutputs: string[];
}
