using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Models;
using Registration.ViewModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;

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

            if (ModelState.IsValid)
            {
                if (_db.RegistrationDbs.Where(u => u.Email == register.Email).Any())
                {
                    ViewBag.Message = "Email is Already Exists.";
                    
                    return View();
                }
                if(_db.RegistrationDbs.Where(u => u.Username == register.Username).Any())
                {
                    ViewBag.Message = "UserName is already Exists.";
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
                    TempData["Success"] = "Registration SuccessFully!";
                    return RedirectToAction("Register");
                }
            }
            return View();
        }

        private string UploadFile(RegistrationDbViewModel register)
        {
            string fileName = null;

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

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(RegistrationDb register)
        {
            var myUser = _db.RegistrationDbs.Where( x => x.Username == register.Username && x.Password == register.Password).FirstOrDefault();
            
            if(myUser != null)
            {
                HttpContext.Session.SetString("UserSession",myUser.Username);
                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed.";
            }
            return View();
        }
        public IActionResult DashBoard()
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
              ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult LogOut()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
            }
            return RedirectToAction("Login");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [NonAction]
        private void DepartmentList()
        {
            var departmentList = _db.Departments.ToList();
            ViewBag.departmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName");
        }
    }
}
