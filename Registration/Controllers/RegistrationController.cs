using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Models;
using Registration.ViewModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using NuGet.Common;

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
                if (_db.RegistrationDbs.Where(u => u.Username == register.Username).Any())
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
                    TempData["successRegister"] = "Registration SuccessFully!";
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
            //if session is not remove then redirect to dashboard.
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(RegistrationDb register)
        {
            var user = _db.RegistrationDbs.Where(x => x.Username == register.Username && x.Password == register.Password).FirstOrDefault();

            if (user != null)
            {
                //create session key.
                HttpContext.Session.SetString("UserSession", user.Username);
                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.messageLoginFail = "Login Failed.";
                return View();
            }
        }
        public IActionResult DashBoard()
        {
            //Get value and display it.
            if (HttpContext.Session.GetString("UserSession") != null)
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
            //Remove Session
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

        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            var forgotPassword = _db.RegistrationDbs.Where(x => x.Email == Email).FirstOrDefault();

            if (forgotPassword != null)
            {
                string resetPasswordCode = Guid.NewGuid().ToString();
                SendLinkEmail(forgotPassword.Email, resetPasswordCode);
                forgotPassword.ResetPasswordCode = resetPasswordCode;

                _db.SaveChanges();
                TempData["messageForgotPassword"] = "Reset password link has been sent to your email.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["messageFail"] = "Account Not Found";
                return View();
            }
        }

        [NonAction]
        public void SendLinkEmail(string Email, string resetPasswordCode)
        {
            var link = "https://localhost:7193/Registration/ResetPassword/" + resetPasswordCode;

            var fromEmail = new MailAddress("pushti.prajapati2001@gmail.com", "Reset Password");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "qzdayomvdiardnuz";

            string subject = "Reset Password";
            string body = "Hello,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                   "<br/><br/><a href=" + link + ">Reset Password</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
           smtp.Send(message);
        }


        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            var user = _db.RegistrationDbs.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.ResetPasswordCode = id;
                return View(model);
            }
            else
            {             
                return View("Views/Shared/Error.cshtml");
            }

        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.RegistrationDbs.Where(a => a.ResetPasswordCode == model.ResetPasswordCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = model.NewPassword;
                    user.ResetPasswordCode = "";
                    _db.SaveChanges();
                    TempData["messageResetPassword"] = "New password updated successfully";
                    return RedirectToAction("Login");
                }
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.message = "Something invalid";
                return View();
            }

        }


        

        [NonAction]
        private void DepartmentList()
        {
            var departmentList = _db.Departments.ToList();
            ViewBag.departmentList = new SelectList(departmentList, "DepartmentId", "DepartmentName");
        }
    }
}
