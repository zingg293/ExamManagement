import {
  Account,
  AllowanceEmployee,
  BenefitEmployee,
  businessTrip,
  Candidate,
  categoryLaborEquipment,
  CategoryNews,
  CategoryPosition,
  categoryVacancies,
  CompanyInformation,
  createTicketCommonly, Curriculum,
  DayOff,
  employee,
  historyCommonly,
  internRequest,
  manageCommonly,
  News,
  onLeave,
  overtime,
  PromotionTransfer,
  requestToHire,
  resign,
  ticketLaborEquipment,
  Unit,
  UserRole,
  UserType,
  WorkFlow
} from "@admin/components";
import { useGetListNavigationByTokenQuery } from "@API/services/Navigation.service";
import React from "react";

type Map = {
  [key: string]: React.JSX.Element[];
};
export const DataNavigate = () => {
  const { data: listNavigation } = useGetListNavigationByTokenQuery({ pageNumber: 0, pageSize: 0 });
  const setting = localStorage.getItem("setting")!;
  const color = JSON.parse(setting)?.PrimaryColor;
  const iconMap: Map = {
    //admin
    ICON_MENU_NGUOI_DUNG: Account(color),
    ICON_MENU_LOAI_NGUOI_DUNG: UserType(color),
    ICON_MENU_DON_VI_PHONG_BAN: Unit(color),
    ICON_MENU_QUYEN_NGUOI_DUNG: UserRole(color),
    ICON_QUY_TRINH: WorkFlow(color),
    //employee
    ICON_MENU_NGAY_NGHI_NHAN_VIEN: DayOff(color),
    ICON_MENU_DANH_MUC_PHU_CAP: AllowanceEmployee(color),
    ICON_MENU_DANH_MUC_BOI_THUONG_VA_PHUC_LOI: BenefitEmployee(),
    ICON_MENU_QUAN_LY_NHAN_VIEN: employee(color),
    ICON_MENU_LOAI_NHAN_VIEN: UserType(color),
    ICON_MENU_SYLL_NHANVIEN: Curriculum(color),
    //request to hire
    ICON_MENU_VI_TRI_UNG_TUYEN: categoryVacancies(color),
    ICON_MENU_YEU_CAU_TUYEN_DUNG: requestToHire(color),
    ICON_MENU_LICH_SU_YEU_CAU_TUYEN_DUNG: historyCommonly(color),
    ICON_MENU_TAO_PHIEU_YEU_CAU_TUYEN_DUNG: createTicketCommonly(color),
    ICON_MENU_QUAN_LY_TUYEN_DUNG: manageCommonly(color),
    //ticket labor equipment
    ICON_MENU_DANH_MUC_TRANG_THIET_BI_LAO_DONG: categoryLaborEquipment(color),
    ICON_MENU_PHIEU_YEU_CAU_TRANG_THIET_BI_LAO_DONG: ticketLaborEquipment(color),
    ICON_MENU_LICH_SU_TRANG_THIET_BI_LAO_DONG: historyCommonly(color),
    ICON_MENU_TAO_PHIEU_YEU_CAU_TRANG_THIET_BI_LAO_DONG: createTicketCommonly(color),
    ICON_MENU_QUAN_LY_THIET_BI_LAO_DONG: manageCommonly(color),
    //intern request
    ICON_MENU_TAO_PHIEU_THUC_TAP: createTicketCommonly(color),
    ICON_MENU_LICH_SU_YEU_CAU_THUC_TAP: historyCommonly(color),
    ICON_MENU_YEU_CAU_THUC_TAP: internRequest(color),
    ICON_MENU_QUAN_LY_THU_VIEC: manageCommonly(color),
    // on leave
    ICON_MENU_TAO_PHIEU_DANG_KY_NGAY_NGHI: createTicketCommonly(color),
    ICON_MENU_LICH_SU_DANG_KY_NGAY_NGHI: historyCommonly(color),
    ICON_MENU_DANH_SACH_DANG_KY_NGAY_NGHI: onLeave(color),
    ICON_MENU_QUAN_LY_NGHI_PHEP: manageCommonly(color),
    // overtime
    ICON_MENU_TAO_PHIEU_DANG_KY_LICH_TANG_CA: createTicketCommonly(color),
    ICON_MENU_LICH_SU_DANG_KY_LICH_TANG_CA: historyCommonly(color),
    ICON_MENU_DANG_KY_LICH_TANG_CA: overtime(color),
    ICON_MENU_QUAN_LY_TANG_CA: manageCommonly(color),
    // resign
    ICON_MENU_TAO_PHIEU_XIN_THOI_VIEC: createTicketCommonly(color),
    ICON_MENU_LICH_SU_DANH_SACH_XIN_THOI_VIEC: historyCommonly(color),
    ICON_MENU_DANH_SACH_DON_XIN_THOI_VIEC: resign(color),
    ICON_MENU_QUAN_LY_THOI_VIEC: manageCommonly(color),
    // business trip
    ICON_MENU_TAO_PHIEU_DANG_KY_CONG_TAC: createTicketCommonly(color),
    ICON_MENU_LICH_SU_DANG_KY_CONG_TAC: historyCommonly(color),
    ICON_MENU_DANH_SACH_DANG_KY_CONG_TAC: businessTrip(color),
    ICON_MENU_QUAN_LY_CONG_TAC: manageCommonly(color),
    //PromotionTransfer
    ICON_MENU_TAO_PHIEU_BO_NHIEM_DIEU_CHUYEN: createTicketCommonly(color),
    ICON_MENU_LICH_SU_DANH_SACH_BO_NHIEM_DIEU_CHUYEN: historyCommonly(color),
    ICON_MENU_DANH_SACH_BO_NHIEM_DIEU_CHUYEN: PromotionTransfer(color),
    ICON_MENU_DANH_MUC_BO_NHIEM_VA_DIEU_CHUYEN: CategoryPosition(color),
    ICON_MENU_QUAN_LY_BO_NHIEM_VA_DIEU_CHUYEN: manageCommonly(color),
    //hiring
    ICON_MENU_THONG_TIN_TONG_QUAT: CompanyInformation(color),
    ICON_MENU_THONG_TIN_UNG_VIEN: Candidate(color),
    ICON_MENU_DANH_MUC_THONG_TIN_TUYEN_DUNG: CategoryNews(color),
    ICON_MENU_THONG_TIN_TUYEN_DUNG: News(color),
    // calculate working days
    ICON_MENU_QUAN_LY_CHAM_CONG: manageCommonly(color)
  };
  const mapNavigation = listNavigation?.listPayload?.map((items) => {
    return {
      ...items,
      navigationsChild: items?.navigationsChild?.map((x) => {
        return {
          ...x,
          icon: iconMap[x?.iconLink]
        };
      })
    };
  });
  return mapNavigation || [];
};
