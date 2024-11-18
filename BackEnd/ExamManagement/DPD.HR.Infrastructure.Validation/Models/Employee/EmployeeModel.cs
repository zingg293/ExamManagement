//using Microsoft.AspNetCore.Http;

namespace DPD.HR.Infrastructure.Validation.Models.Employee;

public class EmployeeModel
{
    public Guid? Id {get;set;}
    public string? Name {get;set;}
    public bool? Sex {get;set;}
    public DateTime? Birthday {get;set;}
    public string? Phone {get;set;}
    public string? Email {get;set;}
    public string? Address {get;set;}
    public Guid? IdCity {get;set;}
    public Guid? IdDistrict {get;set;}
    public Guid? IdWard {get;set;}
    public string? TaxNumber {get;set;}
    public string? AccountNumber {get;set;}
    public string? Note {get;set;}
    public Guid? IdUnit {get;set;}
    public double? SalaryBase {get;set;}
    public double? SocialInsurancePercent {get;set;}
    public double? TaxPercent {get;set;}
    public double? JobGrade {get;set;}
    public Guid? TypeOfEmployee {get;set;}
    public Guid IdUser {get;set;}
    public Guid? IdFile {get;set;}
}