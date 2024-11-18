using DPD.HumanResources.Entities.Entities;
using DPD.HumanResources.Interface;
using DPD.HumanResources.Interface.Interfaces;

namespace DPD.HR.Application.Implement;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(

        IAllowancePreviousSalaryInformationRepository allowancePreviousSalaryInformationRepository,
        IBankAccountInformationRepository bankAccountInformationRepository,
        ICategoryEducationalDegreeRepository categoryEducationalDegreeRepository,
        ICategoryGovernmentManagementRepository categoryGovernmentManagementRepository,
        ICategoryProfessionalQualificationRepository categoryProfessionalQualificationRepository,
        ICategorySalaryLevelRepository categorySalaryLevelRepository,
        ICategorySalaryScaleRepository categorySalaryScaleRepository,
        ICategoryTypeSalaryScaleRepository categoryTypeSalaryScaleRepository,
        ICertificateEmployeeRepository certificateEmployeeRepository,
        IEducationalBackgroundRepository educationalBackgroundRepository,
        IFamilyInformationEmployeeRepository familyInformationEmployeeRepository,
        IHouseholdRegistrationRepository householdRegistrationRepository,
        IHouseholdRegistrationTypeRepository householdRegistrationTypeRepository,
        IMilitaryInformationEmployeeRepository militaryInformationEmployeeRepository,
        IPassportVisaWorkPermitRepository passportVisaWorkPermitRepository,
        IPolicticialInformationEmployeeRepository policticialInformationEmployeeRepository,
        IPortfolioEmployeeRepository portfolioEmployeeRepository,
        IPreviousSalaryInformationRepository previousSalaryInformationRepository,
        ISalaryCoefficientRepository salaryCoefficientRepository,
        ISalaryNonCoefficientRepository salaryNonCoefficientRepository,
        ITrainingEmployeeRepository trainingEmployeeRepository,
        IWorkExperienceRepository workExperienceRepository,
        ICategoryPolicybeneficiaryRepository categoryPolicybeneficiaryRepository,
        ICategoryNationalityRepository categoryNationalityRepository,
        IDOEMSRepository doemsRepository,
        IStudyGroupRepository studyGroupRepository,
        IExamFormRepository examFormRepository,
        IDOTSRepository dotsRepository,
        IEducationProgramRepository educationProgramRepository,
        IEMSRepository emsRepository,
        ITrainingSystemRepository trainingSystemRepository,
        IRoomRepository roomRepository,
        ITestScheduleRepository testScheduleRepository,
        IExamSubjecRepository examSubjecRepository,
        IExaminationRepository examinationRepository,
        ILecturersRepository lecturersRepository,
        ICurriculumVitaeRepository curriculumVitaeRepository,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IUserTypeRepository userTypeRepository,
        IUnitRepository unitRepository,
        IAllowanceRepository allowanceRepository,
        ICategoryCityRepository categoryCityRepository,
        ICategoryDistrictRepository categoryDistrictRepository,
        ICategoryWardRepository categoryWardRepository,
        IEmployeeAllowanceRepository employeeAllowanceRepository,
        IEmployeeRepository employeeRepository,
        INavigationRepository navigationRepository,
        ICategoryCompensationBenefitsRepository categoryCompensationBenefitsRepository,
        IEmployeeBenefitsRepository employeeBenefitsRepository,
        IEmployeeDayOffRepository employeeDayOffRepository,
        ICategoryLaborEquipmentRepository categoryLaborEquipmentRepository,
        IRequestToHiredRepository requestToHiredRepository,
        ICategoryVacanciesRepository categoryVacanciesRepository,
        ITicketLaborEquipmentDetailRepository ticketLaborEquipmentDetailRepository,
        ITicketLaborEquipmentRepository ticketLaborEquipmentRepository,
        ICategoryPositionRepository categoryPositionRepository,
        ILaborEquipmentUnitRepository laborEquipmentUnitRepository,
        IPositionEmployeeRepository positionEmployeeRepository,
        IPromotionTransferRepository promotionTransferRepository,
        IInternRequestRepository internRequestRepository,
        IOnLeaveRepository onLeaveRepository,
        ITypeDayOffRepository typeDayOffRepository,
        IOvertimeRepository overtimeRepository,
        IResignRepository resignRepository,
        IBusinessTripEmployeeRepository businessTripEmployeeRepository,
        IBusinessTripRepository businessTripRepository,
        IWorkflowTemplateRepository workflowTemplateRepository,
        IWorkFlowRepository workFlowRepository,
        IWorkFlowHistoryRepository workFlowHistoryRepository,
        IWorkflowStepRepository workflowStepRepository,
        ICandidateRepository candidateRepository,
        ICompanyInformationRepository companyInformationRepository,
        ICategoryNewsRepository categoryNewsRepository,
        IMarkWorkPointsRepository markWorkPointsRepository,
        INewsRepository newsRepository,
        IEmployeeTypeRepository employeeTypeRepository,
        IExaminationTypeRepository examinationTypeRepository,
        IFacultyRepository facultyRepository,
        IArrangeLecturersRepository arrangeLecturersRepository,
        IExamScheduleRegistrationRepository examScheduleRegistrationRepository,
        IExamDutyRegistrationDetailsRepository examDutyRegistrationDetailsRepository,
        IExamShiftRepository examShiftRepository,
        IRollCallRepository rollCallRepository
        )
    {
        AllowancePreviousSalaryInformation = allowancePreviousSalaryInformationRepository;
        BankAccountInformation = bankAccountInformationRepository;
        CategoryEducationalDegree = categoryEducationalDegreeRepository;
        CategoryGovernmentManagement = categoryGovernmentManagementRepository;
        CategoryProfessionalQualification = categoryProfessionalQualificationRepository;
        CategorySalaryLevel = categorySalaryLevelRepository;
        CategorySalaryScale = categorySalaryScaleRepository;
        CategoryTypeSalaryScale = categoryTypeSalaryScaleRepository;
        CertificateEmployee = certificateEmployeeRepository;
        EducationalBackground = educationalBackgroundRepository;
        FamilyInformationEmployee = familyInformationEmployeeRepository;
        HouseholdRegistration = householdRegistrationRepository;
        HouseholdRegistrationType = householdRegistrationTypeRepository;
        MilitaryInformationEmployee = militaryInformationEmployeeRepository;
        PassportVisaWorkPermit = passportVisaWorkPermitRepository;
        PolicticialInformationEmployee = policticialInformationEmployeeRepository;
        PortfolioEmployee = portfolioEmployeeRepository;
        PreviousSalaryInformation = previousSalaryInformationRepository;
        SalaryCoefficient = salaryCoefficientRepository;
        SalaryNonCoefficient = salaryNonCoefficientRepository;
        TrainingEmployee = trainingEmployeeRepository;
        WorkExperience = workExperienceRepository;
        CategoryPolicybeneficiary = categoryPolicybeneficiaryRepository;
        CategoryNationality = categoryNationalityRepository;
        ExamSubjec = examSubjecRepository;
        Examination = examinationRepository;
        TestSchedule = testScheduleRepository;
        Lecturers = lecturersRepository;
        CurriculumVitae = curriculumVitaeRepository;
        Role = roleRepository;
        User = userRepository;
        UserRole = userRoleRepository;
        UserType = userTypeRepository;
        Unit = unitRepository;
        Allowance = allowanceRepository;
        CategoryCity = categoryCityRepository;
        CategoryDistrict = categoryDistrictRepository;
        CategoryWard = categoryWardRepository;
        EmployeeAllowance = employeeAllowanceRepository;
        Employee = employeeRepository;
        EmployeeType = employeeTypeRepository;
        Navigation = navigationRepository;
        EmployeeDayOff = employeeDayOffRepository;
        EmployeeBenefits = employeeBenefitsRepository;
        CategoryVacancies = categoryVacanciesRepository;
        CategoryLaborEquipment = categoryLaborEquipmentRepository;
        CategoryCompensationBenefits = categoryCompensationBenefitsRepository;
        RequestToHired = requestToHiredRepository;
        TicketLaborEquipmentDetail = ticketLaborEquipmentDetailRepository;
        TicketLaborEquipment = ticketLaborEquipmentRepository;
        CategoryPosition = categoryPositionRepository;
        LaborEquipmentUnit = laborEquipmentUnitRepository;
        PositionEmployee = positionEmployeeRepository;
        PromotionTransfer = promotionTransferRepository;
        InternRequest = internRequestRepository;
        Overtime = overtimeRepository;
        TypeDayOff = typeDayOffRepository;
        OnLeave = onLeaveRepository;
        WorkflowTemplate = workflowTemplateRepository;
        Resign = resignRepository;
        BusinessTripEmployee = businessTripEmployeeRepository;
        WorkflowStep = workflowStepRepository;
        WorkFlow = workFlowRepository;
        WorkFlowHistory = workFlowHistoryRepository;
        BusinessTrip = businessTripRepository;
        Candidate = candidateRepository;
        CategoryNews = categoryNewsRepository;
        CompanyInformation = companyInformationRepository;
        MarkWorkPoints = markWorkPointsRepository;
        News = newsRepository;
        ExaminationType = examinationTypeRepository;
        Room = roomRepository;
        EMS = emsRepository;
        DOEMS = doemsRepository;
        StudyGroup = studyGroupRepository;
        ExamForm = examFormRepository;
        DOTS = dotsRepository;
        EducationProgram = educationProgramRepository;
        TrainingSystem = trainingSystemRepository;
        Faculty = facultyRepository;
        ArrangeLecturers = arrangeLecturersRepository;
        ExamScheduleRegistration =  examScheduleRegistrationRepository;
        ExamDutyRegistrationDetails = examDutyRegistrationDetailsRepository;
        ExamShift = examShiftRepository;
        RollCall = rollCallRepository;
    }


    public IAllowancePreviousSalaryInformationRepository AllowancePreviousSalaryInformation { get; }
    public IBankAccountInformationRepository BankAccountInformation { get; }
    public ICategoryEducationalDegreeRepository CategoryEducationalDegree { get; }
    public ICategoryGovernmentManagementRepository CategoryGovernmentManagement { get; }
    public ICategoryProfessionalQualificationRepository CategoryProfessionalQualification { get; }
    public ICategorySalaryLevelRepository CategorySalaryLevel { get; }
    public ICategorySalaryScaleRepository CategorySalaryScale { get; }
    public ICategoryTypeSalaryScaleRepository CategoryTypeSalaryScale { get; }
    public ICertificateEmployeeRepository CertificateEmployee { get; }
    public IEducationalBackgroundRepository EducationalBackground { get; }
    public IFamilyInformationEmployeeRepository FamilyInformationEmployee { get; }
    public IHouseholdRegistrationRepository HouseholdRegistration { get; }
    public IHouseholdRegistrationTypeRepository HouseholdRegistrationType { get; }
    public IMilitaryInformationEmployeeRepository MilitaryInformationEmployee { get; }
    public IPassportVisaWorkPermitRepository PassportVisaWorkPermit { get; }
    public IPolicticialInformationEmployeeRepository PolicticialInformationEmployee { get; }
    public IPortfolioEmployeeRepository PortfolioEmployee { get; }
    public IPreviousSalaryInformationRepository PreviousSalaryInformation { get; }
    public ISalaryCoefficientRepository SalaryCoefficient { get; }
    public ISalaryNonCoefficientRepository SalaryNonCoefficient { get; }
    public ITrainingEmployeeRepository TrainingEmployee { get; }
    public IWorkExperienceRepository WorkExperience { get; }
    public ICategoryPolicybeneficiaryRepository CategoryPolicybeneficiary { get; }
    public ICategoryNationalityRepository CategoryNationality { get; }
    public IArrangeLecturersRepository ArrangeLecturers { get; }
    public IDOTSRepository DOTS { get; }
    public IRoomRepository Room { get; }
    public IEMSRepository EMS { get; }
    public IDOEMSRepository DOEMS { get; }
    public IStudyGroupRepository StudyGroup { get; }
    public IExamFormRepository ExamForm { get; }
    public IEducationProgramRepository EducationProgram { get; }
    public ITrainingSystemRepository TrainingSystem { get; }
    public IFacultyRepository Faculty { get; }
    public IRollCallRepository RollCall { get; }
    public ITestScheduleRepository TestSchedule { get; } 
    public IExamSubjecRepository ExamSubjec{ get; }
    public IExaminationRepository Examination { get; }
    public ILecturersRepository Lecturers { get; }
    public ICurriculumVitaeRepository CurriculumVitae { get; }
    public IRoleRepository Role { get; }
    public IMarkWorkPointsRepository MarkWorkPoints { get; }
    public INewsRepository News { get; }
    public ICompanyInformationRepository CompanyInformation { get; }
    public ICategoryNewsRepository CategoryNews { get; }
    public IUserRepository User { get; }
    public IUserRoleRepository UserRole { get; }
    public IUserTypeRepository UserType { get; }
    public IUnitRepository Unit { get; }
    public IAllowanceRepository Allowance { get; }
    public ICategoryCityRepository CategoryCity { get; }
    public ICategoryDistrictRepository CategoryDistrict { get; }
    public ICategoryWardRepository CategoryWard { get; }
    public IEmployeeAllowanceRepository EmployeeAllowance { get; }
    public IEmployeeRepository Employee { get; }
    public IEmployeeTypeRepository EmployeeType { get; }
    public INavigationRepository Navigation { get; }
    public ICategoryCompensationBenefitsRepository CategoryCompensationBenefits { get; }
    public IEmployeeBenefitsRepository EmployeeBenefits { get; }
    public IEmployeeDayOffRepository EmployeeDayOff { get; }
    public ICategoryVacanciesRepository CategoryVacancies { get; }
    public ICategoryLaborEquipmentRepository CategoryLaborEquipment { get; }
    public IRequestToHiredRepository RequestToHired { get; }
    public ITicketLaborEquipmentDetailRepository TicketLaborEquipmentDetail { get; }
    public ITicketLaborEquipmentRepository TicketLaborEquipment { get; }
    public ICategoryPositionRepository CategoryPosition { get; }
    public ILaborEquipmentUnitRepository LaborEquipmentUnit { get; }
    public IPositionEmployeeRepository PositionEmployee { get; }
    public IPromotionTransferRepository PromotionTransfer { get; }
    public IInternRequestRepository InternRequest { get; }
    public IOnLeaveRepository OnLeave { get; }
    public IOvertimeRepository Overtime { get; }
    public ITypeDayOffRepository TypeDayOff { get; }
    public IResignRepository Resign { get; }
    public IBusinessTripRepository BusinessTrip { get; }
    public IBusinessTripEmployeeRepository BusinessTripEmployee { get; }
    public IWorkflowTemplateRepository WorkflowTemplate { get; }
    public IWorkflowStepRepository WorkflowStep { get; }
    public IWorkFlowRepository WorkFlow { get; }
    public IWorkFlowHistoryRepository WorkFlowHistory { get; }
    public ICandidateRepository Candidate { get; }
    public IExaminationTypeRepository ExaminationType { get; }
    public IExamScheduleRegistrationRepository ExamScheduleRegistration { get; }
    public IExamDutyRegistrationDetailsRepository ExamDutyRegistrationDetails { get; }
    public IExamShiftRepository ExamShift { get; }
}