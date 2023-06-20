using CrystalReportWebAPI.Utilities;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace CrystalReportWebAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    { 
        
        [AllowAnonymous]

        [HttpPost]
        [Route("PoliceReport")]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        public HttpResponseMessage PoliceReport(string reportFileName, string pdfFilename, List<Dictionary<string, string>> reportParams)
        {
           //  List<ReportParameters> reportParameters = new List<ReportParameters>();
           // reportParameters.Add(new ReportParameters { ParamName = "SalesId", ParamValue = "1" });
            //var rd = new ReportDocument();
            //rd.Load(@"C:\Users\cgvak-136\Downloads\CrystalReportWebAPI-master\CrystalReportWebAPI-master\CrystalReportWebAPI\Reports\Report1.rpt");
            //var fileStream = new FileStream(@"C:\temp\MyReport2.pdf", FileMode.Create);
            //rd.SetParameterValue(0, 1);

            string reportPath = "~/Reports";
            reportFileName = reportFileName + ".rpt";
            string exportFilename = pdfFilename +"pdf";

            HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, reportParams);
            return result;

        }

      



       
    }
}