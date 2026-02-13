export interface User {
  id: string;
  username: string;
  email: string;
  fullName: string;
  role: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  username: string;
  fullName: string;
  email: string;
  role: string;
  menuPermissions: string[];
  expiresAt: Date;
}

export interface Menu {
  id: string;
  key: string;
  label: string;
  icon?: string;
  parentKey?: string;
  order: number;
  isActive: boolean;
}
