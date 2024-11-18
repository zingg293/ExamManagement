using DPD.HR.Infrastructure.WebApi.Controllers;
using DPD.HumanResources.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test_Project.Allowance;

public class TestAllowanceController
{
    //[Fact]
    [Theory]
    [InlineData(1, 2)]
    public async Task GetAllAsync_ShouldReturn200Status(int pageNumber, int pageSize)
    {
        //Arrange   
        var todoService = new Mock<IUnitOfWork>();
        todoService.Setup(_ => _.Allowance.GetAllAsync(pageNumber, pageSize))
            .ReturnsAsync(AllowanceMockData.GetAllowance());
        var sut = new AllowanceController(todoService.Object);

        //Act
        var result = await sut.GetListAllowance(pageNumber, pageSize);

        //Assert
        var statusTest = (OkObjectResult)result;
        Assert.Equal(200, statusTest.StatusCode);
    }
}