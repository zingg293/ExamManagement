using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface.Interfaces;

namespace DPD.HumanResources.Interface;

public interface IUnitOfWork

{
    IAllowancePreviousSalaryInformationRepository AllowancePreviousSalaryInformation { get; }
    IBankAccountInformationRepository BankAccountInformation { get; }
    ICategoryEducationalDegreeRepository CategoryEducationalDegree { get; }
    ICategoryGovernmentManagementRepository CategoryGovernmentManagement { get; }
    ICategoryProfessionalQualificationRepository CategoryProfessionalQualification { get; }
    ICategorySalaryLevelRepository CategorySalaryLevel { get; }
    ICategorySalaryScaleRepository CategorySalaryScale { get; }
    ICategoryTypeSalaryScaleRepository CategoryTypeSalaryScale { get; }
    ICertificateEmployeeRepository CertificateEmployee { get; }
    IEducationalBackgroundRepository EducationalBackground { get; }
    IFamilyInformationEmployeeRepository FamilyInformationEmployee { get; }
    IHouseholdRegistrationRepository HouseholdRegistration { get; }
    IHouseholdRegistrationTypeRepository HouseholdRegistrationType { get; }
    IMilitaryInformationEmployeeRepository MilitaryInformationEmployee { get; }
    IPassportVisaWorkPermitRepository PassportVisaWorkPermit { get; }
    IPolicticialInformationEmployeeRepository PolicticialInformationEmployee { get; }
    IPortfolioEmployeeRepository PortfolioEmployee { get; }
    IPreviousSalaryInformationRepository PreviousSalaryInformation { get; }
    ISalaryCoefficientRepository SalaryCoefficient { get; }
    ISalaryNonCoefficientRepository SalaryNonCoefficient { get; }
    ITrainingEmployeeRepository TrainingEmployee { get; }
    IWorkExperienceRepository WorkExperience { get; }
    ICategoryPolicybeneficiaryRepository CategoryPolicybeneficiary { get; }
    ICategoryNationalityRepository CategoryNationality { get; }
    IArrangeLecturersRepository ArrangeLecturers { get; }
    IFacultyRepository Faculty { get; }
    ITrainingSystemRepository TrainingSystem { get; }
    IEducationProgramRepository EducationProgram { get; }
    IDOTSRepository DOTS { get; }
    IEMSRepository EMS { get; }
    IDOEMSRepository DOEMS { get; }
    IStudyGroupRepository StudyGroup { get; }
    ITestScheduleRepository TestSchedule { get; }
    IExamSubjecRepository ExamSubjec { get; }
    IExaminationRepository Examination { get; }
    ILecturersRepository Lecturers { get; }
    ICurriculumVitaeRepository CurriculumVitae { get; }
    IRoleRepository Role { get; }
    IUserRepository User { get; }
    IUserRoleRepository UserRole { get; }
    IUserTypeRepository UserType { get; }
    IUnitRepository Unit { get; }
    IAllowanceRepository Allowance { get; }
    ICategoryCityRepository CategoryCity { get; }
    ICategoryDistrictRepository CategoryDistrict { get; }
    ICategoryWardRepository CategoryWard { get; }
    IEmployeeAllowanceRepository EmployeeAllowance { get; }
    IEmployeeRepository Employee { get; }
    IEmployeeTypeRepository EmployeeType { get; }
    INavigationRepository Navigation { get; }
    ICategoryCompensationBenefitsRepository CategoryCompensationBenefits { get; }
    IEmployeeBenefitsRepository EmployeeBenefits { get; }
    IEmployeeDayOffRepository EmployeeDayOff { get; }
    IExaminationTypeRepository ExaminationType { get; }
    ICategoryVacanciesRepository CategoryVacancies { get; }
    ICategoryLaborEquipmentRepository CategoryLaborEquipment { get; }
    IRequestToHiredRepository RequestToHired { get; }
    ITicketLaborEquipmentDetailRepository TicketLaborEquipmentDetail { get; }
    ITicketLaborEquipmentRepository TicketLaborEquipment { get; }
    ICategoryPositionRepository CategoryPosition { get; }
    ILaborEquipmentUnitRepository LaborEquipmentUnit { get; }
    IPositionEmployeeRepository PositionEmployee { get; }
    IPromotionTransferRepository PromotionTransfer { get; }
    IInternRequestRepository InternRequest { get; }
    IOnLeaveRepository OnLeave { get; }
    IOvertimeRepository Overtime { get; }
    ITypeDayOffRepository TypeDayOff { get; }
    IResignRepository Resign { get; }
    IBusinessTripRepository BusinessTrip { get; }
    IBusinessTripEmployeeRepository BusinessTripEmployee { get; }
    IWorkflowTemplateRepository WorkflowTemplate { get; }
    IWorkflowStepRepository WorkflowStep { get; }
    IWorkFlowRepository WorkFlow { get; }
    IWorkFlowHistoryRepository WorkFlowHistory { get; }
    ICandidateRepository Candidate { get; }
    ICategoryNewsRepository CategoryNews { get; }
    ICompanyInformationRepository CompanyInformation { get; }
    INewsRepository News { get; }
    IMarkWorkPointsRepository MarkWorkPoints { get; }
    IRoomRepository Room { get; }
    IExamFormRepository ExamForm { get; }
    IExamShiftRepository   ExamShift { get; }
    IExamScheduleRegistrationRepository     ExamScheduleRegistration { get; }
    IExamDutyRegistrationDetailsRepository ExamDutyRegistrationDetails { get; }
    IRollCallRepository RollCall { get; }
}