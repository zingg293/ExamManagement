import { Navigation } from "@models/navigationDTO";

export interface RoleDTO {
  navigation: Navigation[];
  role: RoleDto;
}

export interface RoleDto {
  id: string;
  roleName: string | null;
  status: number | null;
  isDeleted: boolean;
  isAdmin: boolean | null;
  numberRole: number | null;
  roleCode: string | null;
}
