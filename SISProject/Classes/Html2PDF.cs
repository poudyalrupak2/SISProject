using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Path = System.IO.Path;
using DMSClassLibrary;
using iTextSharp.text.html.simpleparser;
using System.Net;
using System.Drawing;

using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace DMSClassLibrary
{
    public class Html2PDF
    {
        public static string FormatLink(string imageData)
        {

            return null;// HttpContext.Current.Server.MapPath("~\\images\\add.gif"); ; 
        }

        public static string FormatImage(string imageData)
        {

            return null;// HttpContext.Current.Server.MapPath("~\\images\\add.gif"); ; 
        }

        public static string FormatString(string input)
        {
            if (input.Contains("<body>") && input.Contains("</body>"))
            {
                input = input.Substring(input.IndexOf("<body>") + 6, input.IndexOf("</body>") + 1 - input.IndexOf("<body>") - 7);
            }
            var regex = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string ouput = regex.Replace(input, "");

            regex = new Regex("(\\<img(.+?)\\</img\\>)|(\\<img(.+?)\\/\\>)|(\\<img(.+?)\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            ouput = regex.Replace(ouput, match => FormatImage(match.Value.ToString()));


            regex = new Regex("(\\<a(.+?)\\</a\\>)|(\\<a(.+?)\\/\\>)|(\\<a(.+?)\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            ouput = regex.Replace(ouput, match => FormatLink(match.Value.ToString()));

            return ouput;
        }

        public static void Convert(string inputFile, string outputFile)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 15f, 15f, 80f, 0f);
            var styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            try
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                document.Open();
                WebClient wc = new WebClient();
                string param1 = wc.DownloadString(inputFile);

                param1 = FormatString(param1);
                // param1 = FormatImages(param1);

                List<IElement> param2 = HTMLWorker.ParseToList(new StringReader(param1), styles);
                for (int k = 0; k < param2.Count; k++)
                {
                    switch (param2[k].Type)
                    {
                        case Element.IMGRAW:
                        case Element.IMGTEMPLATE:
                        case Element.HEADER:
                        case Element.TITLE:
                            break;
                        //case Element.Script:
                        case (Element.PARAGRAPH):
                        default: document.Add((IElement)param2[k]);
                            break;
                    }
                }

                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
