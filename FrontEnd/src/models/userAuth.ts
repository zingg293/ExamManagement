import { UserRoleDTO } from "@models/userRoleDto";

export interface UserLoginRequest {
  email?: string;
  password?: string;
}

export interface UserLoginResponse {
  id: string;
  isAdmin: boolean;
  data: string;
  roleList: UserRoleDTO[];
}
