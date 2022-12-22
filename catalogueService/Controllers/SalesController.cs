using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catalogueService.Interfaces;
using catalogueService.Repositories;
using AutoMapper;
using catalogueService.Database;
using catalogueService.Models;
using catalogueService.requestETresponse;
using System;
using Microsoft.AspNetCore.Authorization;
using catalogueService.Database.DBContextFiles;
using catalogueService.Database;
using System.Security.Claims;
using catalogueService.Database.DBsets;
using iTextSharp.text;
//using Select.Pdf;
using System.IO;

namespace catalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Super Admin")]
    public class SalesController : Controller
    {
        private readonly ISale _saleRep;
        private readonly IMapper _mapper;

        public SalesController(IOrder orderRep, IMapper mapper, ISale saleRep)
        { 
            this._mapper = mapper;
            this._saleRep = saleRep;
        }

        [HttpGet("View all Sales")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var (IsSucess, mbokoDomain) = await _saleRep.GetSalesAsync();
            if (!IsSucess)
            {
                return NotFound();
            }
            var mbokoDTO = _mapper.Map<IEnumerable<saleModel>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpGet("View Specific Sale")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var saleOrder = await _saleRep.GetByIdAsync(id);
            if (saleOrder == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var saleDTO = _mapper.Map<saleModel>(saleOrder);

            return Ok(saleDTO);
        }

        [HttpPost("Generate Report")]
        public async Task<IActionResult> GenerateReportAsync(int id)
        {
            #region I intended to develop an algorithm that collects all information from the database table to generate a report in pdf format.
            #endregion

            //string receipthtml = File.ReadAllText(HttpContext.Current.Server.MapPath("~/catalogueService/ReportTemplate/reports.html"));
            //StringBuilder strHTMLBuilder = new StringBuilder(receipthtml);

            //strHTMLBuilder = strHTMLBuilder.Replace("{CustomerName}", customerName);
            //strHTMLBuilder = strHTMLBuilder.Replace("{TransDate}", transdate.ToLongDateString());
            //strHTMLBuilder = strHTMLBuilder.Replace("{TransRef}", transref);
            //SelectPdf.GlobalProperties.LicenseKey = ConfigurationManager.AppSettings["SelectPDF_LicenseKey"].ToString(); //- buy license and apply it here
            //converter.Options.PdfPageSize = PdfPageSize.A4;
            //converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            //converter.Options.MarginLeft = 10;
            //converter.Options.MarginRight = 10;
            //converter.Options.MarginTop = 0;
            //converter.Options.MarginBottom = 0;
            //converter.Options.DisplayFooter = false;
            //converter.Footer.DisplayOnFirstPage = true;
            //converter.Footer.DisplayOnOddPages = true;
            //converter.Footer.DisplayOnEvenPages = true;
            //converter.Footer.Height = 0;
            //SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(strHTMLBuilder.ToString()); //, Page.Requests.Url.AbsoluteUri
            //string FilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/catalogueService/GeneratedReports/");
            //if (!Directory.Exists(FilePath))
            //{
            //    Directory.CreateDirectory(FilePath);
            //}
            //string fullWindowsPath = Path.GetFullPath(FilePath);
            //string full = Path.Combine(fullWindowsPath, merchant_name + "_" + transref + ".pdf");
            //doc1.Save(full);

            return Ok("Sales reports generated successfully");
        }
    }
}
