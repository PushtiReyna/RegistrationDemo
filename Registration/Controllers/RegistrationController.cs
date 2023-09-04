using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Models;
using Registration.ViewModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net;

namespace Registration.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegistrationController(RegistrationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            DepartmentList();
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegistrationDbViewModel register)
        {
            DepartmentList();

            ModelState.Remove("DepartmentName");
            ModelState.Remove("Department");
           // ModelState.Remove("UserImage");
            if (ModelState.IsValid)
            {
                if (_db.RegistrationDbs.Where(u => u.Email == register.Email).Any())
                {
                    TempData["EmailMessage"] = "Email is Already Exists.";
                    return View();
                }
                if(_db.RegistrationDbs.Where(u => u.Username == register.Username).Any())
                {
                    TempData["UserNameMessage"] = "UserName is already Exists.";
                    return View(); ;
                }
                else
                {
                    string stringFileName = UploadFile(register);

                    var addUser = new RegistrationDb()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Dob = register.Dob,
                        Gender = register.Gender,
                        Email = register.Email,
                        Phone = register.Phone,
                        Username = register.Username,
                        Password = register.Password,
                        DepartmentId = register.DepartmentId,
                        IsActive = true,
                        UserImage = stringFileName,

                    };
                    _db.RegistrationDbs.Add(addUser);
                    _db.SaveChanges();
                    return RedirectToAction("Register");
                }
            }
            return View();
        }

        private string UploadFile(RegistrationDbViewModel register)
        {
            string fileName = null; ;
            if (register.UserImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + register.UserImage.FileName;
                if (fileName != null)
                {

                    string filePath = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        register.UserImage.CopyTo(fileStream);
                    }
                    return fileName;
                }
            }
            else
            {
                fileName = "D:\\project\\RegistrationDemo\\RegistrationDemo\\Registration\\wwwroot\\Images\\Default.png";
                return fileName;
            }
            return fileName;
        }
        


        [NonAction]
        private void DepartmentList()
        {
            var departmentList = _db.Departments.ToList();
            ViewBag.departmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName");
        }
    }
}
