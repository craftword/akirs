using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Vanso.SXMP;

namespace Akirs.client.Models
{
    public class EmailerNotification
    {
        //public void SendEmail(string body, string recipent)
        //{
        //    this.SendHtmlFormattedEmail(recipent, "New Task Created!", body);
        //}
        //    Public Sub SendEmail(ByVal body As String, ByVal recipent As String)
        //    'body = Me.PopulateBody()
        //    Me.SendHtmlFormattedEmail(recipent, "New Task Created!", body)
        //End Sub

        ////public string PopulateBody(string createdby, string url, string eventMsg, string eventlabel, DateTime createdDate)
        ////{
        ////    string body = string.Empty;
        ////    StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/EmailTemplate.htm"));
        ////    body = reader.ReadToEnd();
        ////    body = body.Replace("{CreatedBy}", createdby);
        ////    //body = body.Replace("{url}", url);
        ////    body = body.Replace("{EventType}", eventMsg);
        ////    //  body = body.Replace("{EventLabel}", eventlabel);
        ////    body = body.Replace("{CreatedDate}", createdDate.ToString("dd-MMM-yyyy"));
        ////    //body = body.Replace("{Description}", description);

        ////    //body = body.Replace("{createdby}", createdBy);
        ////    return body;
        ////}


        //public string PopulateMerchantReportBody(string mid, string mName, string date)
        //{
        //    string body = string.Empty;
        //    StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/MerchantReportTemplate.html"));
        //    body = reader.ReadToEnd();
        //    body = body.Replace("{mid}", mid);
        //    // body = body.Replace("{url}", url);
        //    body = body.Replace("{MerchantName}", mName);
        //    body = body.Replace("{date}", date);

        //    return body;
        //}
        //public string PopulateUploadErrorMessage(string message, int record, string batchid, string FullName)
        //{
        //    string body = string.Empty;
        //    StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/FailedMerchantUpld.html"));
        //    body = reader.ReadToEnd();
        //    body = body.Replace("{message}", message);
        //    body = body.Replace("{record}", record.ToString());
        //    body = body.Replace("{batchid}", batchid);
        //    //  body = body.Replace("{FullName}", FullName);

        //    return body;
        //}

        //public string PopulateMerchantBody(string batchid, string institution_inputername, string request_type, string reason, DateTime createdDate)
        //{
        //    string body = string.Empty;
        //    StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Template/UploadNotificationTemplate.html"));
        //    body = reader.ReadToEnd();
        //    body = body.Replace("{batchid}", batchid);
        //    body = body.Replace("{institution_inputername}", institution_inputername);
        //    body = body.Replace("{request_type}", request_type);
        //    //  body = body.Replace("{EventLabel}", eventlabel);
        //    body = body.Replace("{reason}", reason);
        //    //body = body.Replace("{Description}", description);

        //    //body = body.Replace("{createdby}", createdBy);
        //    return body;
        //}

        public string PopulateUserBody(string UserName, string Password, string FullName, string Template)
        {
            string body = string.Empty;
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Template/UserCreationTemplate.html");
            StreamReader reader = new StreamReader(path);
            body = reader.ReadToEnd();
            body = body.Replace("{0}", UserName);
            body = body.Replace("{1}", Password);
            body = body.Replace("{2}", FullName);

            return body;
        }

        public string SendEmailAscync(string recepientEmail, string subject, string body)
        {
            ////MailMessage mailMessage = new MailMessage();
            var resp = string.Empty;
            MailMessage m = new MailMessage();
            /*SmtpClient sc = new SmtpClient()*/;
            m.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
            m.To.Add(recepientEmail);
            m.Subject = subject;
            m.Body = body;
            m.IsBodyHtml = true;
            using (SmtpClient sc = new SmtpClient())
            {
                sc.Host = ConfigurationManager.AppSettings["emailhost"];
                string str1 = "gmail.com";
                string str2 = ConfigurationManager.AppSettings["UserName"];
                if (str2.Contains(str1))
                {
                    try
                    {
                        sc.UseDefaultCredentials = false;
                        sc.Port = 587;
                        sc.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["emailpassword"]);
                        sc.EnableSsl = true;
                        sc.Send(m);
                        resp = "sent";
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        sc.Port = 25;
                        sc.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["emailpassword"]);
                        sc.EnableSsl = false;
                        sc.Send(m);
                        resp = "sent";

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }

            return resp.ToString();

        }

        public async Task<string> SendSMSAscync(string recepientPhone, string Header, string body)
        {

            Vanso.SXMP.Response resp = null;
            //private Vanso.SXMP.Response Vanso(string message, string receiverList, string bankname)
            //{
                try
                {
                    string smsipaddrress = ConfigurationManager.AppSettings["smsipaddrress"]; 
                    string smsport = ConfigurationManager.AppSettings["smsport"];
                    string smsuser = ConfigurationManager.AppSettings["smsuser"];
                    string smsPassword = ConfigurationManager.AppSettings["smsPassword"];

                    SubmitRequest request = new SubmitRequest();
                    request.account = new Account(smsuser, smsPassword);
                    request.SourceAddress = new MobileAddress(MobileAddress.Type.alphanumeric, Header);
                    request.DestinationAddress = new MobileAddress(MobileAddress.Type.international, "+" + recepientPhone);
                    request.Text = body;
                    var sender = new SXMPSender(smsipaddrress, int.Parse(smsport));

                      resp = sender.Submit(request);

             
                 }
                catch (Exception ex)
                {
                    string ss = ex.Message;
                }

            return await Task.Run(() => resp.ToString());

        }

    }
}