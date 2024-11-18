using CT.EXAMM.Application.Implement.Repositories;
using DPD.HR.Application.Implement.Repositories;
using DPD.HumanResources.Interface;
using DPD.HumanResources.Interface.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DPD.HR.Application.Implement;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// this service to register dependencies into controller
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterServices(this IServiceCollection services)
    {
        // phải tạo file implement này trong thư mục Repository
        services.AddTransient<IAllowancePreviousSalaryInformationRepository, AllowancePreviousSalaryInformationRepository>();
        services.AddTransient<IBankAccountInformationRepository, BankAccountInformationRepository>();
        services.AddTransient<ICategoryEducationalDegreeRepository, CategoryEducationalDegreeRepository>();
        services.AddTransient<ICategoryGovernmentManagementRepository, CategoryGovernmentManagementRepository>();
        services.AddTransient<ICategoryProfessionalQualificationRepository, CategoryProfessionalQualificationRepository>();
        services.AddTransient<ICategorySalaryLevelRepository, CategorySalaryLevelRepository>();
        services.AddTransient<ICategorySalaryScaleRepository, CategorySalaryScaleRepository>();
        services.AddTransient<ICategoryTypeSalaryScaleRepository, CategoryTypeSalaryScaleRepository>();
        services.AddTransient<ICertificateEmployeeRepository, CertificateEmployeeRepository>();
        services.AddTransient<IEducationalBackgroundRepository, EducationalBackgroundRepository>();
        services.AddTransient<IFamilyInformationEmployeeRepository, FamilyInformationEmployeeRepository>();
        services.AddTransient<IHouseholdRegistrationRepository, HouseholdRegistrationRepository>();
        services.AddTransient<IHouseholdRegistrationTypeRepository, HouseholdRegistrationTypeRepository>();
        services.AddTransient<IMilitaryInformationEmployeeRepository, MilitaryInformationEmployeeRepository>();
        services.AddTransient<IPassportVisaWorkPermitRepository, PassportVisaWorkPermitRepository>();
        services.AddTransient<IPolicticialInformationEmployeeRepository, PolicticialInformationEmployeeRepository>();
        services.AddTransient<IPortfolioEmployeeRepository, PortfolioEmployeeRepository>();
        services.AddTransient<IPreviousSalaryInformationRepository, PreviousSalaryInformationRepository>();
        services.AddTransient<ISalaryCoefficientRepository, SalaryCoefficientRepository>();
        services.AddTransient<ISalaryNonCoefficientRepository, SalaryNonCoefficientRepository>();
        services.AddTransient<ITrainingEmployeeRepository, TrainingEmployeeRepository>();
        services.AddTransient<IWorkExperienceRepository, WorkExperienceRepository>();
        services.AddTransient<ICategoryPolicybeneficiaryRepository, CategoryPolicybeneficiaryRepository>();
        services.AddTransient<ICurriculumVitaeRepository, CurriculumVitaeRepository>();
        services.AddTransient<IUnitRepository, UnitRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        services.AddTransient<IUserTypeRepository, UserTypeRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IExamScheduleRegistrationRepository, ExamScheduleRegistrationRepository>();
        services.AddTransient<IExamDutyRegistrationDetailsRepository, ExamDutyRegistrationDetailsRepository>();
        services.AddTransient<IExamShiftRepository, ExamShiftRepository>();

        services.AddTransient<ICategoryCityRepository, CategoryCityRepository>();
        services.AddTransient<ICategoryNationalityRepository, CategoryNationalityRepository>();
        services.AddTransient<IExamSubjecRepository, ExamSubjectRepository>();
        services.AddTransient<IExaminationRepository, ExaminationRepository>();
        services.AddTransient<ITestScheduleRepository, TestScheduleRepository>();
        services.AddTransient<IRoomRepository, RoomRepository>();
        services.AddTransient<IEMSRepository, EMSRepository>();
        services.AddTransient<IDOEMSRepository, DOEMSRepository>();
        services.AddTransient<IStudyGroupRepository, StudyGroupRepository>();
        services.AddTransient<IExamFormRepository, ExamFormRepository>();
        services.AddTransient<IDOTSRepository, DOTSRepository>();
        services.AddTransient<IEducationProgramRepository, EducationProgramRepository>();
        services.AddTransient<ITrainingSystemRepository, TrainingSystemRepository>();
        services.AddTransient<IFacultyRepository, FacultyRepository>();


        services.AddTransient<IExaminationTypeRepository, ExaminationTypeRepository>();
        services.AddTransient<ILecturersRepository, LecturersRepository>();
        services.AddTransient<ICategoryDistrictRepository, CategoryDistrictRepository>();
        services.AddTransient<ICategoryWardRepository, CategoryWardRepository>();
        services.AddTransient<IArrangeLecturersRepository, ArrangeLecturersRepository>();

        services.AddTransient<IAllowanceRepository, AllowanceRepository>();
        services.AddTransient<IEmployeeAllowanceRepository, EmployeeAllowanceRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IEmployeeTypeRepository, EmployeeTypeRepository>();

        services.AddTransient<INavigationRepository, NavigationRepository>();
        services.AddTransient<ICategoryCompensationBenefitsRepository, CategoryCompensationBenefitsRepository>();
        services.AddTransient<IEmployeeBenefitsRepository, EmployeeBenefitsRepository>();
        services.AddTransient<IEmployeeDayOffRepository, EmployeeDayOffRepository>();
        services.AddTransient<ICategoryVacanciesRepository, CategoryVacanciesRepository>();
        services.AddTransient<ICategoryLaborEquipmentRepository, CategoryLaborEquipmentRepository>();
        services.AddTransient<IRequestToHiredRepository, RequestToHiredRepository>();
        services.AddTransient<ITicketLaborEquipmentDetailRepository, TicketLaborEquipmentDetailRepository>();
        services.AddTransient<ITicketLaborEquipmentRepository, TicketLaborEquipmentRepository>();
        services.AddTransient<ICategoryPositionRepository, CategoryPositionRepository>();
        services.AddTransient<ILaborEquipmentUnitRepository, LaborEquipmentUnitRepository>();
        services.AddTransient<IPositionEmployeeRepository, PositionEmployeeRepository>();
        services.AddTransient<IPromotionTransferRepository, PromotionTransferRepository>();
        services.AddTransient<IInternRequestRepository, InternRequestRepository>();
        services.AddTransient<IOnLeaveRepository, OnLeaveRepository>();
        services.AddTransient<IOvertimeRepository, OvertimeRepository>();
        services.AddTransient<ITypeDayOffRepository, TypeDayOffRepository>();
        services.AddTransient<IResignRepository, ResignRepository>();

        services.AddTransient<IBusinessTripRepository, BusinessTripRepository>();
        services.AddTransient<IBusinessTripEmployeeRepository, BusinessTripEmployeeRepository>();

        services.AddTransient<IWorkflowTemplateRepository, WorkflowTemplateRepository>();
        services.AddTransient<IWorkflowStepRepository, WorkflowStepRepository>();
        services.AddTransient<IWorkFlowRepository, WorkFlowRepository>();
        services.AddTransient<IWorkFlowHistoryRepository, WorkFlowHistoryRepository>();

        services.AddTransient<ICandidateRepository, CandidateRepository>();
        services.AddTransient<INewsRepository, NewsRepository>();
        services.AddTransient<ICategoryNewsRepository, CategoryNewsRepository>();
        services.AddTransient<ICompanyInformationRepository, CompanyInformationRepository>();
        services.AddTransient<IMarkWorkPointsRepository, MarkWorkPointsRepository>();
        services.AddTransient<IRollCallRepository, RollCallRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}