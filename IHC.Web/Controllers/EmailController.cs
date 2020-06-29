using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IHC.Web.Controllers
{
    public class EmailController : Controller
    {

        public static bool EnviarEmail(string subject, string emailBody, string toEmail, HttpPostedFileBase arquivo)
        {
            try
            {
                string senderEmail = ConfigurationManager.AppSettings["senderemail"].ToString();
                string senderPassword = ConfigurationManager.AppSettings["senderpassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;

                if (arquivo != null)
                {
                    string FileName = Path.GetFileName(arquivo.FileName);
                    mailMessage.Attachments.Add(new Attachment(arquivo.InputStream, FileName));
                }

                mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

       
    }
}