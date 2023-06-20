using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Configuration;

namespace CrystalReportWebAPI.Utilities
{
    public static class CrystalReport
    {
        public static HttpResponseMessage RenderReport(string reportPath, string reportFileName, string exportFilename,
            List<Dictionary<string, string>> reportParams)
        {
           
            var rd = new ReportDocument();

            rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));
            
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;


            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];

            CrTables = rd.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }


            foreach (var rparam in reportParams)
            {
                // System.Diagnostics.Debug.WriteLine($"Key: {key}, Value: {value}");
                string pName = rparam["key"];
                string pValue = rparam["value"];
                rd.SetParameterValue(pName, pValue);
            }

            MemoryStream ms = new MemoryStream();
            using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(ms);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportFilename
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return result;
        }
    }
}