using System.Drawing;
using System.Net.Mime;
using DPD.HR.Kernel.Utils;
using DPD.HumanResources.Dtos.Custom;
using DPD.HumanResources.Interface;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace DPD.HR.Infrastructure.WebApi.Controllers;

[ApiController]
[Route("api/v1/markWorkPoints")]
public class MarkWorkPointsController : Controller
{
    #region ===[ Private Members ]=============================================================

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkWorkPointsController> _logger;

    #endregion

    #region ===[ Constructor ]=================================================================

    public MarkWorkPointsController(IUnitOfWork unitOfWork, ILogger<MarkWorkPointsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #endregion

    #region ===[ MarkWorkPointsController ]==============================================

    // GET: api/markWorkPoints/getFileExcelMarkWorkPoints
    [HttpPost]
    [Route("getFileExcelMarkWorkPoints")]
    public async Task<IActionResult> GetFileExcelMarkWorkPoints(FilterMarkWorkPointsModel model)
    {
        try
        {
            const string rootPath = AppSettings.Root;
            const string pathMarkWorkPoints = AppSettings.ServerFileMarkWorkPoints;
            const string excelFileName = "QuanLyChamCong.xlsx";

            var currentDirectory = Directory.GetCurrentDirectory();
            var sourceFilePath = Path.Combine(currentDirectory, "CommonFiles", excelFileName);
            var destinationFilePath = Path.Combine(pathMarkWorkPoints, excelFileName);

            // Ensure directories exist and copy the source file if needed
            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(pathMarkWorkPoints);
            if (!System.IO.File.Exists(destinationFilePath))
            {
                System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
            }

            var pathToExcel = Path.Combine(pathMarkWorkPoints, excelFileName);

            var results = await _unitOfWork.MarkWorkPoints.MarkWorkPointsEmployeeForExcel(model);
            var year = model.FromDate?.Split('/')[1];
            var month = model.FromDate?.Split('/')[0];
            var lastDayOfMonth = GetLastDayOfMonth(int.Parse(year), int.Parse(month));

            using (var excelPackage = new ExcelPackage(new FileInfo(pathToExcel)))
            {
                var namedWorksheet = excelPackage.Workbook.Worksheets[0];
                namedWorksheet.Cells["B3:B7"].Clear();
                namedWorksheet.Cells["C3"].Clear();
                namedWorksheet.Cells["D4:D7"].Clear();
                namedWorksheet.Cells["A10:E100"].Clear();

                namedWorksheet.Cells["B3:B7"].Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.CenterContinuous;
                namedWorksheet.Cells["C3"].Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.CenterContinuous;
                namedWorksheet.Cells["D3:D7"].Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.CenterContinuous;

                namedWorksheet.Cells["B3"].Value = $"01/{month}/{year}";
                namedWorksheet.Cells["C3"].Value = $"{lastDayOfMonth}/{month}/{year}";
                namedWorksheet.Cells["B4"].Value = results.OnLeaves is null ? 0 : results.OnLeaves?.Count;
                namedWorksheet.Cells["D4"].Value = results.Overtimes is null ? 0 : results.Overtimes?.Count;

                namedWorksheet.Cells["B5"].Value =
                    GetTheNumberOfDaysInMonthExceptSaturday(int.Parse(year), int.Parse(month));
                namedWorksheet.Cells["D5"].Value =
                    GetTheNumberOfDaysInMonthExceptSaturdayAndSunday(int.Parse(year), int.Parse(month));

                namedWorksheet.Cells["B6"].Value = results.Employee?.Code ?? "";
                namedWorksheet.Cells["B7"].Value = results.Employee?.Phone ?? "";
                namedWorksheet.Cells["D6"].Value = results.Employee?.Email ?? "";
                namedWorksheet.Cells["E6"].Value = "";
                namedWorksheet.Cells["D7"].Value = results.Employee?.Birthday?.ToString("dd/MM/yyyy");

                var rowBegin = 10;
                if (results.Overtimes is not null)
                {
                    foreach (var item in results.Overtimes)
                    {
                        namedWorksheet.Cells[$"A{rowBegin}:D{rowBegin}"].Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.CenterContinuous;

                        for (int k = 1; k < 5; k++)
                        {
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        namedWorksheet.Cells[rowBegin, 1].Value = results.Employee?.Name;
                        namedWorksheet.Cells[rowBegin, 2].Value = item.FromDate?.ToString("dd/MM/yyyy") + " - " +
                                                                  item.ToDate?.ToString("dd/MM/yyyy");
                        namedWorksheet.Cells[rowBegin, 3].Value = item.Description;
                        namedWorksheet.Cells[rowBegin, 4].Value = item.CreatedDate?.ToString("dd/MM/yyyy");

                        rowBegin++;
                    }
                }

                namedWorksheet.Cells[$"A{rowBegin}:E{rowBegin}"].Merge = true;
                namedWorksheet.Cells[$"A{rowBegin}:E{rowBegin}"].Value = "Ngày nghỉ";
                namedWorksheet.Row(rowBegin).Height = 35;
                namedWorksheet.Cells[$"A{rowBegin}:E{rowBegin}"].Style.Font.Bold = true;
                rowBegin++;
                for (int k = 1; k <= 6; k++)
                {
                    namedWorksheet.Cells[rowBegin, k].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    namedWorksheet.Cells[rowBegin, k].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    namedWorksheet.Cells[rowBegin, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    namedWorksheet.Cells[rowBegin, k].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                namedWorksheet.Cells[$"A{rowBegin}:F{rowBegin}"].Style.HorizontalAlignment =
                    ExcelHorizontalAlignment.CenterContinuous;
                namedWorksheet.Cells[$"A{rowBegin}"].Value = "Nhân viên";
                namedWorksheet.Cells[$"B{rowBegin}"].Value = "Thời gian";
                namedWorksheet.Cells[$"C{rowBegin}"].Value = "Ghi chú";
                namedWorksheet.Cells[$"D{rowBegin}"].Value = "Ngày tạo";
                namedWorksheet.Cells[$"E{rowBegin}"].Value = "File đính kèm";
                namedWorksheet.Cells[$"F{rowBegin}"].Value = "Nghỉ phép";
                namedWorksheet.Column(6).Width = 25;
                rowBegin++;
                if (results.OnLeaves is not null)
                {
                    foreach (var item in results.OnLeaves)
                    {
                        namedWorksheet.Cells[$"A{rowBegin}:F{rowBegin}"].Style.HorizontalAlignment =
                            ExcelHorizontalAlignment.CenterContinuous;
                        for (int k = 1; k < 7; k++)
                        {
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            namedWorksheet.Cells[rowBegin, k].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        namedWorksheet.Cells[rowBegin, 1].Value = results.Employee?.Name ?? "";
                        namedWorksheet.Cells[rowBegin, 2].Value = item.FromDate?.ToString("dd/MM/yyyy") + " - " +
                                                                  item.ToDate?.ToString("dd/MM/yyyy");
                        namedWorksheet.Cells[rowBegin, 3].Value = item.Description ?? "";
                        namedWorksheet.Cells[rowBegin, 4].Value = item.CreatedDate?.ToString("dd/MM/yyyy");
                        namedWorksheet.Cells[rowBegin, 5].Value = item.Attachments ?? "";
                        namedWorksheet.Cells[rowBegin, 6].Value =
                            item.UnPaidLeave ? "Nghỉ có lương" : "Nghỉ không lương";

                        rowBegin++;
                    }
                }

                // Save your file
                await excelPackage.SaveAsync();
            }

            _logger.LogInformation("Tải file thành công!");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(pathToExcel);
            return File(fileBytes, MediaTypeNames.Application.Octet, excelFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Đã xảy ra lỗi: {ex}");
            throw;
        }
    }

    private static int GetLastDayOfMonth(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    private static int GetTheNumberOfDaysInMonthExceptSaturday(int year, int month)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        int totalDaysInMonth = (int)(lastDayOfMonth - firstDayOfMonth).TotalDays + 1;

        int saturdaysCount = 0;
        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                saturdaysCount++;
            }
        }

        return totalDaysInMonth - saturdaysCount;
    }

    private static int GetTheNumberOfDaysInMonthExceptSaturdayAndSunday(int year, int month)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        int totalDaysInMonth = (int)(lastDayOfMonth - firstDayOfMonth).TotalDays + 1;

        int saturdaysAndSundayCount = 0;
        for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                saturdaysAndSundayCount++;
            }
        }

        return totalDaysInMonth - saturdaysAndSundayCount;
    }

    // HttpPost: api/markWorkPoints/getListMarkWorkPoints
    [HttpPost("getListMarkWorkPoints")]
    public async Task<IActionResult> GetListMarkWorkPoints(FilterMarkWorkPointsModel model)
    {
        var templateApi = await _unitOfWork.MarkWorkPoints.MarkWorkPointsEmployee(model);
        _logger.LogInformation("Thành công : {message}", templateApi.Message);
        return Ok(templateApi);
    }

    #endregion
}