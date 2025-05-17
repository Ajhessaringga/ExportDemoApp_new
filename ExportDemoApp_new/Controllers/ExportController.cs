using ExportDemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using System.Collections.Generic;
using System.IO;

namespace ExportDemoApp.Controllers
{
    public class ExportController : Controller
    {
        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Aloe Vera", Stock = 100 },
                new Product { Id = 2, Name = "Lidah Buaya", Stock = 50 },
                new Product { Id = 3, Name = "Aloe Vera Gel", Stock = 70 },
            };
        }

        public IActionResult Index()
        {
            var products = GetSampleProducts();
            return View(products);
        }

        public IActionResult ExportToExcel()
        {
            var products = GetSampleProducts();

            // ✅ Set license context di dalam method
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Stock";

            for (int i = 0; i < products.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = products[i].Id;
                worksheet.Cells[i + 2, 2].Value = products[i].Name;
                worksheet.Cells[i + 2, 3].Value = products[i].Stock;
            }

            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products.xlsx");
        }

public IActionResult ExportToPdf()
{
    try
    {
        var products = GetSampleProducts();
        return new ViewAsPdf("PdfTemplate", products)
        {
            FileName = "products.pdf"
        };
    }
    catch (Exception ex)
    {
        return Content("Error saat generate PDF: " + ex.Message + "\n" + ex.StackTrace);
    }
}

    }
}
