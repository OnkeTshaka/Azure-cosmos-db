using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FirewallsAzureProject.Controllers
{
    public class SendMailController : Controller
    {
        // GET: SendMail
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(FirewallsAzureProject.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress("thefirewalls8@gmail.com", "The Firewalls Team");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                //{
                //    var docu = Request.Files["document"];

                //    string fileName = Path.GetFileName(docu.FileName);
                //    mail.Attachments.Add(new Attachment(docu.InputStream, fileName));
                //}


                mail.IsBodyHtml = true;
                string document = _objModelMail.document;
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(document);
                mail.Attachments.Add(attachment);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential

                ("thefirewalls8@gmail.com", "Dut@1234");// Enter senders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.Message = "Email Successfully Sent!!!!";
                //return View("Index", _objModelMail);
                return View("ThankYou");
            }
            else
            {
                return View();
            }
        }
    }
}