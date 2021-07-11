using DocumentManagement.Areas.Admin.Models;
using DocumentManagement.Common;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, model.Password);
                if (result == 1)
                {
                    var user = dao.GetByName(model.UserName);
                    var userSession = new UserLogin();
                    var listCredentials = dao.GetListCredential(model.UserName);
                    userSession.UserID = user.ID;
                    userSession.UserName = user.UserName;
                    userSession.GroupID = user.GroupID;
                    userSession.Name = user.Name;
                    userSession.Avatar = user.Avatar;
                    userSession.DepartmentID = user.DepartmentID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    Session.Add(CommonConstants.CREDENTIALS_SESSION, listCredentials);
                    
                    return RedirectToAction("Index", "Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
                }
            }
            else
            {
                ModelState.AddModelError("", "Đăng nhập không thành công");
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return View("Index");
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            var dao = new UserDAO();
            var user = dao.GetByName(email);
            if (user != null)
            {
                Random random = new Random();
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[6];
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var newPassword = new String(stringChars);

                user.Password = newPassword;
                var result = dao.Update(user);
                if (result)
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/forgetPassword.html"));
                    content = content.Replace("{{UserName}}", user.UserName);
                    content = content.Replace("{{Name}}", user.Name);
                    content = content.Replace("{{NewPassword}}", newPassword);

                    new MailHelper().SendEmail(user.UserName, "Tìm lại mật khẩu trang quản lý văn bản UTT", content);
                    SetAlert("Mật khẩu mới đã được gửi đến email của bạn", "success");
                }
                else
                {
                    SetAlert("Có lỗi! Thay đổi mật khẩu thất bại", "danger");
                } 
            }
            else
            {
                SetAlert("Tài khoản không tồn tại trong hệ thống", "danger");
            }
            return View("Index");
        }

        public void SetAlert(string message, string type)
        {
            TempData["Message"] = message;
            if (type == "success")
            {
                TempData["Type"] = "success";
            }
            else if (type == "warning")
            {
                TempData["Type"] = "warning";
            }
            else if (type == "danger")
            {
                TempData["Type"] = "danger";
            }
            else
            {
                TempData["Type"] = "info";
            }
        }
    }
}