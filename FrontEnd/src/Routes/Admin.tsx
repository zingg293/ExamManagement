import { Button, Result } from "antd";
import React, { Suspense } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import { PrivateRoutes, PublicRoutes, WithAuthorization } from "~/package/admin/routes";
import { DashBoardLayout, LoginAdminLayout, UserManage, UserSetting } from "./listComponetsLazyAdmin";
import { globalVariable } from "~/globalVariable";
import { Main } from "@admin/components";
import Loading from "@units/loading/loading";
import { useGetListNavigationByTokenQuery } from "@API/services/Navigation.service";
import { Navigation } from "@models/navigationDTO";
import {
  // Allowance,
  Role,
  Unit,
  UserType,
  // WorkflowTemplate,
  Examination,
  ExaminationType,
  Lecturers,
  ExamSubject,
  TestSchedule,
  Room,
  EMS,
  RegisterExamSchedule,
  ArrangeLecturers,
  ExamScheduleRegistration,
  RollCall,
  TrainingSystem,
  StudyGroup,
  ExamForm,
  EducationProgram,
  
} from "./ListComponentLazy";
import { Faculty } from "@admin/features/Faculty";
import { ExamShift } from "~/package/admin/features/ExamShift";

type MenuComponentMap = {
  [key: string]: React.ReactNode | React.FC<any>;
};

export function Admin() {
  const { data } = useGetListNavigationByTokenQuery({ pageNumber: 0, pageSize: 0 });
  const navigate = useNavigate();
  const path = useLocation();

  const menuComponentMap: MenuComponentMap = {
    // new
    MENU_DANH_MUC_CA_THI: <ExamShift />,
    MENU_DANH_MUC_XEP_LICH_GAC_THI: <ArrangeLecturers />,
    MENU_DANH_MUC_DANG_KI_GAC_THI: <ExamScheduleRegistration />,
    QUANLI_DANGKY_LICHGACTHI: <RegisterExamSchedule />,
    QUANLI_LICHGACTHI: <EMS />,
    QUANLI_PHONGTHI: <Room />,
    QUANLI_LICHTHI: <TestSchedule />,
    MENU_DANH_MUC_MONTHI: <ExamSubject />,
    MENU_DANH_MUC_GIANGVIEN: <Lecturers />,
    MENU_DANH_MUC_KHOA: <Faculty />,
    MENU_DANH_MUC_KI_THI: <Examination />,
    MENU_DANH_MUC_LOAI_KI_THI: <ExaminationType />,
    MENU_DANH_MUC_DIEM_DANH_GAC_THI: <RollCall />,
    MENU_DANH_MUC_HE_DAO_TAO: <TrainingSystem/>,
    MENU_DANH_MUC_NHOM_HOC_PHAN : <StudyGroup/>,
    MENU_DANH_MUC_HINH_THUC_THI : <ExamForm/>,
    MENU_DANH_MUC_CHUONG_TRINH_DAO_TAO : <EducationProgram/>,
    //admin
    MENU_DON_VI_PHONG_BAN: <Unit />,
    MENU_LOAI_NGUOI_DUNG: <UserType />,
    MENU_NGUOI_DUNG: <UserManage />,
    // MENU_DANH_MUC_PHU_CAP: <Allowance />,
    MENU_QUYEN_NGUOI_DUNG: <Role />,
    // MENU_QUY_TRINH: <WorkflowTemplate />
    //  MENU_QUY_TRINH: <WorkflowTemplate />,
    //employee
    // MENU_SOYEULILICHNHANVIEN: <CurriculumVitae />,
    // DANHMUC_DOITUONGCHINHSACH: <CategoryPolicyBeneficiary />,
    // DANHMUC_TRINHDOCHUYENMON: <CategoryProfessionalQualification />,
    // DANHMUC_QUANLYNHANUOC: <CategoryGovernmentManagement />,
    // DANHMUC_LOAINGACHLUONG: <CategoryTypeSalaryScale />,
    // DANHMUC_NGACHLUONG: <CategorySalaryScale />,
    // DANHMUC_BANGCAP: <CategoryEducationalDegree />,
    // DANHMUC_BACLUONG: <CategorySalaryLevel />,
    //
    // MENU_DANH_MUC_BOI_THUONG_VA_PHUC_LOI: <CategoryCompensationBenefits />,
    // MENU_BOI_THUONG_VA_PHUC_LOI_NHAN_VIEN: <EmployeeDayOff />,
    // MENU_NGAY_NGHI_NHAN_VIEN: <EmployeeDayOff />,
    // MENU_LOAI_NHAN_VIEN: <EmployeeType />,
    // MENU_QUAN_LY_NHAN_VIEN: <Employee />,
    // //request to hired
    // MENU_VI_TRI_UNG_TUYEN: <CategoryVacancies />,
    // MENU_YEU_CAU_TUYEN_DUNG: <RequestToHiredDepartment />,
    // MENU_LICH_SU_YEU_CAU_TUYEN_DUNG: <RequestToHiredHistory />,
    // MENU_TAO_PHIEU_YEU_CAU_TUYEN_DUNG: <CreateRequestToHired />,
    // MENU_QUAN_LY_TUYEN_DUNG: <ManageRequestToHired />,
    // //ticket labor equipment
    // MENU_DANH_MUC_TRANG_THIET_BI_LAO_DONG: <CategoryLaborEquipment />,
    // MENU_PHIEU_YEU_CAU_TRANG_THIET_BI_LAO_DONG: <TicketLaborEquipment />,
    // MENU_TAO_PHIEU_YEU_CAU_TRANG_THIET_BI_LAO_DONG: <CreateTicketLaborEquipment />,
    // MENU_LICH_SU_TRANG_THIET_BI_LAO_DONG: <TicketLaborEquipmentHistory />,
    // MENU_QUAN_LY_THIET_BI_LAO_DONG: <ManageTicketLaborEquipment />,
    // // Promote transfer
    // MENU_DANH_MUC_BO_NHIEM_VA_DIEU_CHUYEN: <CategoryPosition />,
    // MENU_TAO_PHIEU_BO_NHIEM_DIEU_CHUYEN: <CreatePromotionTransfer />,
    // MENU_DANH_SACH_BO_NHIEM_DIEU_CHUYEN: <PromotionTransfer />,
    // MENU_LICH_SU_DANH_SACH_BO_NHIEM_DIEU_CHUYEN: <PromotionTransferHistory />,
    // MENU_QUAN_LY_BO_NHIEM_VA_DIEU_CHUYEN: <ManagePromotionTransfer />,
    // //Intern request
    // MENU_TAO_PHIEU_THUC_TAP: <CreateInternRequest />,
    // MENU_YEU_CAU_THUC_TAP: <InternRequest />,
    // MENU_LICH_SU_YEU_CAU_THUC_TAP: <InternRequestHistory />,
    // MENU_QUAN_LY_THU_VIEC: <ManageInternRequest />,
    // // On leave
    // MENU_TAO_PHIEU_DANG_KY_NGAY_NGHI: <CreateOnLeave />,
    // MENU_DANH_SACH_DANG_KY_NGAY_NGHI: <OnLeave />,
    // MENU_LICH_SU_DANG_KY_NGAY_NGHI: <OnLeaveHistory />,
    // MENU_QUAN_LY_NGHI_PHEP: <ManageOnLeave />,
    // // overtime
    // MENU_LICH_SU_DANG_KY_LICH_TANG_CA: <OvertimeHistory />,
    // MENU_TAO_PHIEU_DANG_KY_LICH_TANG_CA: <CreateOvertime />,
    // MENU_DANG_KY_LICH_TANG_CA: <Overtime />,
    // MENU_QUAN_LY_TANG_CA: <ManageOverTime />,
    // // resign
    // MENU_DANH_SACH_DON_XIN_THOI_VIEC: <Resign />,
    // MENU_TAO_PHIEU_XIN_THOI_VIEC: <CreateResign />,
    // MENU_LICH_SU_DANH_SACH_XIN_THOI_VIEC: <ResignHistory />,
    // MENU_QUAN_LY_THOI_VIEC: <ManageResign />,
    // // businessTrip
    // MENU_DANH_SACH_DANG_KY_CONG_TAC: <BusinessTrip />,
    // MENU_LICH_SU_DANG_KY_CONG_TAC: <BusinessTripHistory />,
    // MENU_TAO_PHIEU_DANG_KY_CONG_TAC: <CreateBusinessTrip />,
    // MENU_QUAN_LY_CONG_TAC: <ManageBusinessTrip />,
    // // Hiring
    // MENU_DANH_MUC_THONG_TIN_TUYEN_DUNG: <CategoryNews />,
    // MENU_THONG_TIN_CONG_TY: <CompanyInformation />,
    // MENU_THONG_TIN_UNG_VIEN: <Candidate />,
    // MENU_THONG_TIN_TUYEN_DUNG: <News />,
    // // Calculate working days
    // MENU_QUAN_LY_CHAM_CONG: <ManageCalculateWorkingDays></ManageCalculateWorkingDays>
  };
  const menuMap = data?.listPayload?.map((item) => item.navigationsChild)?.flat() as Navigation[];
  return (
    <Suspense
      fallback={
        <div
          style={{
            display: "grid",
            placeItems: "center",
            zIndex: "100",
            width: "100vw",
            height: "100vh",
            backgroundColor: JSON.parse(localStorage.getItem("setting")!).darkMode ? "#000000" : "#ffffff"
          }}
        >
          <Loading />
        </div>
      }
    >
      {path.pathname.length === 1 ? (
        <Routes>
          <Route path={globalVariable.pathNameLogin} element={<PublicRoutes />}>
            <Route path={globalVariable.pathNameLogin} element={<LoginAdminLayout />} />
          </Route>
        </Routes>
      ) : path.pathname.includes("/Print") ? (
        <Routes>
          <Route path="/Print" element={<PrivateRoutes />}>
            <Route index path="/Print/PrintLibraryCard/:id" element={<></>} />
          </Route>
          <Route
            path="/Print-authorized-403"
            element={
              <Result
                status="403"
                title="403"
                subTitle="Xin lỗi, bạn không được phép truy cập trang này."
                extra={
                  <Button
                    type="primary"
                    onClick={() => {
                      navigate(globalVariable.pathNameLogin, { state: { from: location.pathname } });
                    }}
                  >
                    Đăng nhập
                  </Button>
                }
              />
            }
          />
          {/* delete account succsess */}
          <Route
            path="/Print/user/setting/delete-account-success"
            element={
              <Result
                status="success"
                title="Bạn đã xóa tài khoản thành công"
                subTitle="Tài khoản và thông tin liên quan của bạn đã được xóa khỏi hệ thống. Vui lòng đặng nhập lại để sử dung"
                extra={
                  <Button
                    type="primary"
                    onClick={() => {
                      navigate(globalVariable.pathNameLogin);
                    }}
                  >
                    Trở lại đăng nhập
                  </Button>
                }
              />
            }
          />
        </Routes>
      ) : (
        <Main>
          <Route path="/admin" element={<PrivateRoutes />}>
            <Route path="/admin/user/setting" element={<UserSetting />} />
            <Route
              index
              path="/admin/dashboard"
              element={
                <WithAuthorization requiredRole={"Trang chủ"}>
                  <DashBoardLayout />
                </WithAuthorization>
              }
            />
            {menuMap?.map((item) => {
              const Component = menuComponentMap[item.menuCode];
              return <Route path={item.path} element={Component as React.ReactNode} key={item.id} />;
            })}
          </Route>

          {/* 404 not found */}
          <Route
            path="*"
            element={
              <Result
                status="404"
                title="404"
                subTitle="Sorry, the page you visited does not exist."
                extra={
                  <Button
                    type="primary"
                    onClick={() => {
                      navigate(globalVariable.pathNameLogin);
                    }}
                  >
                    Back Home
                  </Button>
                }
              />
            }
          />
          {/*403 not access*/}
          <Route
            path="/admin/403"
            element={
              <Result
                status="403"
                title="403"
                subTitle="Xin lỗi, bạn không có quyền truy cập trang này."
                extra={
                  <Button
                    type="primary"
                    onClick={() => {
                      navigate(-1);
                    }}
                  >
                    Quay lại
                  </Button>
                }
              />
            }
          />
        </Main>
      )}
    </Suspense>
  );
}
