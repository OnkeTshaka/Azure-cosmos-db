using FirewallsAzureProject.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.Documents.Client;
using Rotativa;
using System.Net.Mail;

namespace FirewallsAzureProject.Controllers
{
    public class StudentController : Controller
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static DocumentClient client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
        // GET: Student
        BlobOperations blobOperations;
        public StudentController()
        {
            blobOperations = new BlobOperations();
        }
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {

            var students = await DocumentDBRepository<Student>.GetStudentsAsync(d => !d.isActive);
            return View(students);
        }
        [ActionName("Index2")]
        public async Task<ActionResult> Index2Async()
        {

            var students = await DocumentDBRepository<Student>.GetStudentsAsync(d => !d.isActive);
            return View(students);
        }

        public ActionResult ExportPDF()
        {
            return new ActionAsPdf("Index2");


        }

        [HttpPost]
        public ActionResult SearchStudent(string name)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(name)))
            {
                IQueryable<Student> employeeQuery = client.CreateDocumentQuery<Student>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(f => (!f.isActive && f.FirstName.Contains(name))
                         || (!f.isActive && f.LastName.Contains(name))
                         || (!f.isActive && f.StudentNumber.Contains(name))|| name == null);
                return View("Index", employeeQuery);
            }

            return RedirectToAction("Index");
        }
        //#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }
        ////#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,StudentNumber,FirstName,LastName,Email,HomeAddress,Mobile,isActive,imagFile,acadademicYear,GetRandomNumber,StoreAcad1st,StudentNumber,ProfilePath")] Student student, HttpPostedFileBase profileFile)
        {
            if (ModelState.IsValid)
            {
                student.StudentNumber = Guid.NewGuid().ToString();
                student.GetRandomNumber = student.GetNumber();
                student.StoreAcad1st = student.GetAcad1st();
                student.StoreAcad2nd = student.GetAcad2nd();
                student.StudentNumber = student.GetStudentNumber();



                CloudBlockBlob profileBlob = null;
                //Uploaded File in BLob Storage
                if (profileFile != null && profileFile.ContentLength != 0)
                {
                    profileBlob = await blobOperations.UploadBlob(profileFile);
                    student.ProfilePath = profileBlob.Uri.ToString();
                }
                await DocumentDBRepository<Student>.CreateStudentAsync(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,StudentNumber,FirstName,LastName,Email,HomeAddress,Mobile,isActive,imagFile,acadademicYear,GetRandomNumber,StoreAcad1st,StudentNumber,ProfilePath")] Student student, HttpPostedFileBase profileFile)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Student>.UpdateStudentAsync(student.Id, student);
                CloudBlockBlob profileBlob = null;
                //Uploaded File in BLob Storage
                if (profileFile != null && profileFile.ContentLength != 0)
                {
                    profileBlob = await blobOperations.UploadBlob(profileFile);
                    student.ProfilePath = profileBlob.Uri.ToString();
                }
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id, string studentNo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id, string studentNo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id, StudentNumber")] string id, string studentNo)
        {
            await DocumentDBRepository<Student>.DeleteStudentAsync(id, studentNo);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id, string studentNo)
        {
            Student student = await DocumentDBRepository<Student>.GetStudentAsync(id, studentNo);
            return View(student);
        }
        public ActionResult PrintAll()
        {
            var q = new ActionAsPdf("Details","Student") { FileName = "A.pdf" };
            return q;
        }
        public ActionResult SendEmail()
        {
            return View();
        }
    }
}