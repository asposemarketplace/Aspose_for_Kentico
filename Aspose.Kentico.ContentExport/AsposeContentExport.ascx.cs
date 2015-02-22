using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.PortalControls;
using CMS.Helpers;
using Aspose.Words;
using System.IO;
using System.Net;
using System.Text;

namespace Aspose.Kentico.ContentExport
{
    public partial class AsposeContentExport : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WordsExportButton.Text = ValidationHelper.GetString(this.GetValue("WordExportButtonText"), "Export to Word");
            PdfExportButton.Text = ValidationHelper.GetString(this.GetValue("PdfExportButtonText"), "Export to PDF"); ;
        }

        private string GetOutputFileName(string extension)
        {
            string name = HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.LastIndexOf("/"));
            name = name.Replace("/", string.Empty).Replace(".aspx", extension);

            if (string.IsNullOrEmpty(name))
            {
                name = System.Guid.NewGuid().ToString();
            }

            if (!name.EndsWith(extension)) name = name + extension;

            return name;
        }

        private string CurrentPageURL
        {
            get
            {
                string url = Request.Url.Authority + HttpContext.Current.Request.RawUrl.ToString();

                if (Request.ServerVariables["HTTPS"] == "on")
                {
                    url = "https://" + url;
                }
                else
                {
                    url = "http://" + url;
                }

                return url;
            }
        }

        private string BaseURL
        {
            get
            {
                string url = Request.Url.Authority;

                if (Request.ServerVariables["HTTPS"] == "on")
                {
                    url = "https://" + url;
                }
                else
                {
                    url = "http://" + url;
                }

                return url;
            }
        }

        protected void WordsExportButton_Click(object sender, EventArgs e)
        {
            string html = new WebClient().DownloadString(CurrentPageURL);

            // To make the relative image paths work, base URL must be included in head section
            html = html.Replace("</head>", string.Format("<base href='{0}'></base></head>", BaseURL));

            // Check for license and apply if exists
            string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
            if (File.Exists(licenseFile))
            {
                License license = new License();
                license.SetLicense(licenseFile);
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            Document doc = new Document(stream);
            doc.Save(Response, GetOutputFileName(".doc"), ContentDisposition.Inline, null);
            Response.End();
        }

        protected void PdfExportButton_Click(object sender, EventArgs e)
        {
            string html = new WebClient().DownloadString(CurrentPageURL);

            // To make the relative image paths work, base URL must be included in head section
            html = html.Replace("</head>", string.Format("<base href='{0}'></base></head>", BaseURL));

            // Check for license and apply if exists
            string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
            if (File.Exists(licenseFile))
            {
                License license = new License();
                license.SetLicense(licenseFile);
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            Document doc = new Document(stream);
            doc.Save(Response, GetOutputFileName(".pdf"), ContentDisposition.Inline, null);
            Response.End();
        }
    }
}