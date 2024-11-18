import React from "react";

export interface NavigationDTO {
  navigation: Navigation;
  navigationsChild: Navigation[] | null;
}

export interface Navigation {
  id: string;
  menuName: string;
  idParent: string | null;
  status: number | null;
  createdDate: Date | null;
  path: string;
  iconLink: string;
  menuCode: string;
  sort: number | null;
  icon: React.JSX.Element[];
  children: Navigation[] | null;
}
