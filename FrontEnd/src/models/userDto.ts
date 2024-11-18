import { UserRoleDTO } from "~/models/userRoleDto";
import { RoleDto } from "@models/roleDTO";

export interface UserDTO {
  id: string;
  fullname: string;
  description: string;
  password: string;
  email: string;
  phone: string;
  userTypeId: string;
  address: string;
  status: number;
  createdDate: Date;
  userCode: string;
  isLocked: boolean;
  isDeleted: boolean;
  unitId: string;
  isActive: boolean;
  createdBy: string;
  activeCode: string;
  avatar: string;
  refreshToken: string;
  roles: RoleDto[];
}

export interface UserResponse {
  data: UserDTO;
  roleList: UserRoleDTO[];
  isAdmin: boolean;
}
