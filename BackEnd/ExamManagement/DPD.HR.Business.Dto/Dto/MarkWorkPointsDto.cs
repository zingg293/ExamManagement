using DPD.HumanResources.Entities.Entities;

namespace DPD.HumanResources.Dtos.Dto;

public class MarkWorkPointsDto
{
    public Employee? Employee { get; set; }
    public List<OnLeave>? OnLeaves { get; set; }
    public List<Overtime>? Overtimes { get; set; }
}