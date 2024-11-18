import { lazy } from "react";
import { ExamDutyRegistrationDetailsApisService } from "~/API/services/ExamDutyRegistrationDetails.service";
import { TrainingSystemApisService } from "~/API/services/TrainingSystem.service";

const lazyImports = {
  ExamScheduleRegistration: lazy(() =>
    import("@admin/features/ExamScheduleRegistration").then((module) => ({
      default: module.ExamScheduleRegistration
    }))
  ),
  UserType: lazy(() =>
    import("@admin/features/userType").then((module) => ({
      default: module.UserType
    }))
  ),
  Allowance: lazy(() =>
    import("@admin/features/allowance").then((module) => ({
      default: module.Allowance
    }))
  ),
  Role: lazy(() =>
    import("@admin/features/role").then((module) => ({
      default: module.Role
    }))
  ),
  CategoryCompensationBenefits: lazy(() =>
    import("@admin/features/categoryCompensationBenefits").then((module) => ({
      default: module.CategoryCompensationBenefits
    }))
  ),
  EmployeeDayOff: lazy(() =>
    import("@admin/features/employeeDayOff").then((module) => ({
      default: module.EmployeeDayOff
    }))
  ),
  EmployeeType: lazy(() =>
    import("@admin/features/employeeType").then((module) => ({
      default: module.EmployeeType
    }))
  ),
  Employee: lazy(() =>
    import("@admin/features/employee").then((module) => ({
      default: module.Employee
    }))
  ),
  CategoryVacancies: lazy(() =>
    import("@admin/features/categoryVacancies").then((module) => ({
      default: module.CategoryVacancies
    }))
  ),
  RequestToHiredDepartment: lazy(() =>
    import("@admin/features/requestToHired").then((module) => ({
      default: module.RequestToHiredDepartment
    }))
  ),
  CategoryLaborEquipment: lazy(() =>
    import("@admin/features/CategoryLaborEquipment").then((module) => ({
      default: module.CategoryLaborEquipment
    }))
  ),
  TicketLaborEquipment: lazy(() =>
    import("@admin/features/TicketLaborEquipment").then((module) => ({
      default: module.TicketLaborEquipment
    }))
  ),
  TicketLaborEquipmentHistory: lazy(() =>
    import("@admin/features/TicketLaborEquipment").then((module) => ({
      default: module.TicketLaborEquipmentHistory
    }))
  ),
  CreateTicketLaborEquipment: lazy(() =>
    import("@admin/features/TicketLaborEquipment").then((module) => ({
      default: module.CreateTicketLaborEquipment
    }))
  ),
  WorkflowTemplate: lazy(() =>
    import("@admin/features/WorkflowTemplate").then((module) => ({
      default: module.WorkflowTemplate
    }))
  ),
  CreateRequestToHired: lazy(() =>
    import("@admin/features/requestToHired").then((module) => ({
      default: module.CreateRequestToHired
    }))
  ),
  RequestToHiredHistory: lazy(() =>
    import("@admin/features/requestToHired").then((module) => ({
      default: module.RequestToHiredHistory
    }))
  ),
  CreateInternRequest: lazy(() =>
    import("@admin/features/internRequest").then((module) => ({
      default: module.CreateInternRequest
    }))
  ),
  InternRequest: lazy(() =>
    import("@admin/features/internRequest").then((module) => ({
      default: module.InternRequest
    }))
  ),
  InternRequestHistory: lazy(() =>
    import("@admin/features/internRequest").then((module) => ({
      default: module.InternRequestHistory
    }))
  ),
  CreateOnLeave: lazy(() =>
    import("@admin/features/onLeave").then((module) => ({
      default: module.CreateOnLeave
    }))
  ),
  OnLeave: lazy(() =>
    import("@admin/features/onLeave").then((module) => ({
      default: module.OnLeave
    }))
  ),
  OnLeaveHistory: lazy(() =>
    import("@admin/features/onLeave").then((module) => ({
      default: module.OnLeaveHistory
    }))
  ),
  CreateOvertime: lazy(() =>
    import("@admin/features/overTime").then((module) => ({
      default: module.CreateOvertime
    }))
  ),
  Overtime: lazy(() =>
    import("@admin/features/overTime").then((module) => ({
      default: module.Overtime
    }))
  ),
  OvertimeHistory: lazy(() =>
    import("@admin/features/overTime").then((module) => ({
      default: module.OvertimeHistory
    }))
  ),
  CreateResign: lazy(() =>
    import("@admin/features/resign").then((module) => ({
      default: module.CreateResign
    }))
  ),
  Resign: lazy(() =>
    import("@admin/features/resign").then((module) => ({
      default: module.Resign
    }))
  ),
  ResignHistory: lazy(() =>
    import("@admin/features/resign").then((module) => ({
      default: module.ResignHistory
    }))
  ),
  BusinessTrip: lazy(() =>
    import("@admin/features/BusinessTrip").then((module) => ({
      default: module.BusinessTrip
    }))
  ),
  BusinessTripHistory: lazy(() =>
    import("@admin/features/BusinessTrip").then((module) => ({
      default: module.BusinessTripHistory
    }))
  ),
  CreateBusinessTrip: lazy(() =>
    import("@admin/features/BusinessTrip").then((module) => ({
      default: module.CreateBusinessTrip
    }))
  ),
  CategoryPosition: lazy(() =>
    import("@admin/features/CategoryPosition").then((module) => ({
      default: module.CategoryPosition
    }))
  ),
  CreatePromotionTransfer: lazy(() =>
    import("@admin/features/promotionTransfer").then((module) => ({
      default: module.CreatePromotionTransfer
    }))
  ),
  PromotionTransfer: lazy(() =>
    import("@admin/features/promotionTransfer").then((module) => ({
      default: module.PromotionTransfer
    }))
  ),
  PromotionTransferHistory: lazy(() =>
    import("@admin/features/promotionTransfer").then((module) => ({
      default: module.PromotionTransferHistory
    }))
  ),
  CategoryNews: lazy(() =>
    import("@admin/features/categoryNews").then((module) => ({
      default: module.CategoryNews
    }))
  ),
  CompanyInformation: lazy(() =>
    import("@admin/features/companyInformation").then((module) => ({
      default: module.CompanyInformation
    }))
  ),
  Candidate: lazy(() =>
    import("@admin/features/candidate").then((module) => ({
      default: module.Candidate
    }))
  ),
  News: lazy(() =>
    import("@admin/features/news").then((module) => ({
      default: module.News
    }))
  ),
  ManageOverTime: lazy(() =>
    import("@admin/features/overTime").then((module) => ({
      default: module.ManageOverTime
    }))
  ),
  ManageInternRequest: lazy(() =>
    import("@admin/features/internRequest").then((module) => ({
      default: module.ManageInternRequest
    }))
  ),
  ManageOnLeave: lazy(() =>
    import("@admin/features/onLeave").then((module) => ({
      default: module.ManageOnLeave
    }))
  ),
  ManageResign: lazy(() =>
    import("@admin/features/resign").then((module) => ({
      default: module.ManageResign
    }))
  ),
  ManageTicketLaborEquipment: lazy(() =>
    import("@admin/features/TicketLaborEquipment").then((module) => ({
      default: module.ManageTicketLaborEquipment
    }))
  ),
  ManageCalculateWorkingDays: lazy(() =>
    import("@admin/features/CalculateWorkingDays").then((module) => ({
      default: module.ManageCalculateWorkingDays
    }))
  ),
  ManagePromotionTransfer: lazy(() =>
    import("@admin/features/promotionTransfer").then((module) => ({
      default: module.ManagePromotionTransfer
    }))
  ),
  ManageBusinessTrip: lazy(() =>
    import("@admin/features/BusinessTrip").then((module) => ({
      default: module.ManageBusinessTrip
    }))
  ),
  ManageRequestToHired: lazy(() =>
    import("@admin/features/requestToHired").then((module) => ({
      default: module.ManageRequestToHired
    }))
  ),
  Unit: lazy(() =>
    import("@admin/features/unit").then((module) => ({
      default: module.Unit
    }))
  ),
  CurriculumVitae: lazy(() =>
    import("@admin/features/CurriculumVitae").then((module) => ({
      default: module.CurriculumVitae
    }))
  ),
  CategoryPolicyBeneficiary: lazy(() =>
    import("@admin/features/CategoryPolicyBeneficiary").then((module) => ({
      default: module.CategoryPolicyBeneficiary
    }))
  ),
  CategoryProfessionalQualification: lazy(() =>
    import("@admin/features/CategoryProfessionalQualification").then((module) => ({
      default: module.CategoryProfessionalQualification
    }))
  ),
  CategoryGovernmentManagement: lazy(() =>
    import("@admin/features/CategoryGovernmentManagement").then((module) => ({
      default: module.CategoryGovernmentManagement
    }))
  ),
  CategoryTypeSalaryScale: lazy(() =>
    import("@admin/features/CategoryTypeSalaryScale").then((module) => ({
      default: module.CategoryTypeSalaryScale
    }))
  ),
  CategorySalaryScale: lazy(() =>
    import("@admin/features/CategorySalaryScale").then((module) => ({
      default: module.CategorySalaryScale
    }))
  ),
  CategoryEducationalDegree: lazy(() =>
    import("@admin/features/CategoryEducationalDegree").then((module) => ({
      default: module.CategoryEducationalDegree
    }))
  ),
  CategorySalaryLevel: lazy(() =>
    import("@admin/features/CategorySalaryLevel").then((module) => ({
      default: module.CategorySalaryLevel
    }))
  ),
  Examination: lazy(() =>
    import("@admin/features/Examination").then((module) => ({
      default: module.Examination
    }))
  ),
  ExaminationType: lazy(() =>
    import("@admin/features/ExaminationType").then((module) => ({
      default: module.ExaminationType
    }))
  ),

  Faculty: lazy(() =>
    import("@admin/features/Faculty").then((module) => ({
      default: module.Faculty
    }))
  ),
  ExamSubject: lazy(() =>
    import("@admin/features/ExamSubject").then((module) => ({
      default: module.ExamSubject
    }))
  ),
  Lecturers: lazy(() =>
    import("@admin/features/Lecturers").then((module) => ({
      default: module.Lecturers
    }))
  ),
  TestSchedule: lazy(() =>
    import("@admin/features/TestSchedule").then((module) => ({
      default: module.TestSchedule
    }))
  ),
  Room: lazy(() =>
    import("@admin/features/Room").then((module) => ({
      default: module.Room
    }))
  ),
  EMS: lazy(() =>
    import("@admin/features/EMS").then((module) => ({
      default: module.EMS
    }))
  ),
  CategoryNationality: lazy(() =>
    import("@admin/features/CategoryNationality").then((module) => ({
      default: module.CategoryNationality
    }))
  ),
  RollCall: lazy(() =>
    import("@admin/features/RollCall").then((module) => ({
      default: module.RollCall
    }))
  ),
  ExamDutyRegistrationDetails: lazy(() =>
    import("@admin/features/ExamDutyRegistrationDetails").then((module) => ({
      default: module.ExamDutyRegistrationDetails
    }))
  ),
  TrainingSystem: lazy(() =>
    import("@admin/features/TrainingSystem").then((module) => ({
      default: module.TrainingSystem
    }))
  ),
  StudyGroup: lazy(() =>
    import("@admin/features/StudyGroup").then((module) => ({
      default: module.StudyGroup
    }))
  ),
  ExamForm: lazy(() =>
    import("@admin/features/ExamForm").then((module) => ({
      default: module.ExamForm
    }))
  ),
  EducationProgram: lazy(() =>
    import("@admin/features/EducationProgram").then((module) => ({
      default: module.EducationProgram
    }))
  ),
  ArrangeLecturers: lazy(() =>
    import("@admin/features/ArrangeLecturers").then((module) => ({
      default: module.ArrangeLecturers
    }))
  ),
  DOEMS: lazy(() =>
    import("@admin/features/DOEMS").then((module) => ({
      default: module.DOEMS
    }))
  ),
  RegisterExamSchedule: lazy(() =>
    import("@admin/features/RegisterExamSchedule").then((module) => ({
      default: module.RegisterExamSchedule
    }))
  )
};
export const {
  ExamScheduleRegistration,
  CategoryNationality,
  ArrangeLecturers,
  DOEMS,
  RegisterExamSchedule,
  EMS,
  Room,
  TestSchedule,
  ExamSubject,
  Lecturers,
  Faculty,
  Examination,
  CategorySalaryLevel,
  CategoryEducationalDegree,
  CategorySalaryScale,
  CategoryTypeSalaryScale,
  CategoryGovernmentManagement,
  CategoryProfessionalQualification,
  CategoryPolicyBeneficiary,
  CurriculumVitae,
  UserType,
  Unit,
  Allowance,
  Role,
  CategoryCompensationBenefits,
  EmployeeDayOff,
  EmployeeType,
  Employee,
  CategoryVacancies,
  RequestToHiredDepartment,
  CategoryLaborEquipment,
  TicketLaborEquipment,
  WorkflowTemplate,
  CreateRequestToHired,
  RequestToHiredHistory,
  TicketLaborEquipmentHistory,
  CreateTicketLaborEquipment,
  CreateInternRequest,
  InternRequest,
  InternRequestHistory,
  CreateOnLeave,
  OnLeave,
  OnLeaveHistory,
  CreateOvertime,
  Overtime,
  OvertimeHistory,
  CreateResign,
  Resign,
  ResignHistory,
  BusinessTrip,
  BusinessTripHistory,
  CreateBusinessTrip,
  CategoryPosition,
  CreatePromotionTransfer,
  PromotionTransfer,
  PromotionTransferHistory,
  CategoryNews,
  CompanyInformation,
  Candidate,
  News,
  ManageOverTime,
  ManageInternRequest,
  ManageOnLeave,
  ManageResign,
  ManageTicketLaborEquipment,
  ManageCalculateWorkingDays,
  ManagePromotionTransfer,
  ManageRequestToHired,
  ManageBusinessTrip,
  ExaminationType,
  RollCall,
  ExamDutyRegistrationDetails,
  TrainingSystem,
  StudyGroup,
  ExamForm,
  EducationProgram
} = lazyImports;
