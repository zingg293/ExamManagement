export interface UserRoleDTO {
  numberRole: number;
  id: string;
  idRole: string;
  idUser: string;
  isAdmin: boolean;
}

export interface ListRole {
  id: string;
  roleName: string;
  status: number;
  isDeleted: boolean;
  numberRole: number;
  isAdmin: boolean;
}
