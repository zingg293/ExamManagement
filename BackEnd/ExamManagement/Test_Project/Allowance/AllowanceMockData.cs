using DPD.HumanResources.Dtos.Dto;
using DPD.HumanResources.Utilities.Utils;

namespace Test_Project.Allowance;

public static class AllowanceMockData
{
    private static List<AllowanceDto> MockListAllowance()
    {
        return new List<AllowanceDto>
        {
            new AllowanceDto
            {
                Id = Guid.NewGuid(),
                Name = "Play Games",
                Amount = 20.0f,
                CreatedDate = DateTime.Now.AddDays(-2),
                Status = 3
            },
            new AllowanceDto
            {
                Id = Guid.NewGuid(),
                Name = "Play Games",
                Amount = 20.0f,
                CreatedDate = DateTime.Now.AddDays(-2),
                Status = 3
            }
        };
    }

    public static TemplateApi<AllowanceDto> GetAllowance()
    {
        return new TemplateApi<AllowanceDto>(
            default,
            MockListAllowance().ToArray(),
            "Không tìm thấy dữ liệu !",
            true,
            0,
            0,
            0,
            0);
    }
}