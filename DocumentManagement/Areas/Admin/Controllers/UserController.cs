using DocumentManagement.Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;

namespace DocumentManagement.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_LIST_USER")]
        public ActionResult Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new UserDAO().listAllPaging(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var model = dao.GetByName(user.UserName);
                if (model == null)
                {
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = ((UserLogin)Session[Common.CommonConstants.USER_SESSION]).Name;
                    long id = dao.Insert(user);
                    if (id != 0)
                    {
                        SetAlert("Tạo mới tài khoản thành công", "success");
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        SetAlert("Tạo mới tài khoản thất bại", "warning");
                        ModelState.AddModelError("", "Tạo tài khoản thất bại!");
                    }

                }
                else
                {
                    SetAlert("Tên tài khoản đã tồn tại", "danger");
                    ModelState.AddModelError("", "Tên tài khoản đã tồn tại, mời bạn nhập lại!");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "UPDATE_USER")]
        public ActionResult Edit(long id)
        {
            var dao = new UserDAO();
            var model = dao.GetByID(id);
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_USER")]
        public ActionResult Edit(User entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var currentEntity = dao.GetByName(entity.UserName);
                entity.CreatedDate = currentEntity.CreatedDate;
                entity.CreatedBy = currentEntity.CreatedBy;
                entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                entity.ModifiedDate = DateTime.Now;
                var result = dao.Update(entity);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Cập nhật thất bại", "danger");
                    ModelState.AddModelError("", "Cập nhật thất bại!");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            return View();
        }

        public ActionResult Delete(long id)
        {
            var dao = new UserDAO();
            dao.Delete(id);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var dao = new UserDAO();
            var result = dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HasCredential(RoleID = "VIEW_DETAIL_USER")]
        public ActionResult ViewDetail(long id)
        {
            var dao = new UserUserGroupDepartmentDAO();
            var model = dao.GetByID(id);
            return View(model);
        }

        public void SetViewBag()
        {
            var userGroupDAO = new UserGroupDAO();
            var departmentDAO = new DepartmentDAO();
            ViewBag.GroupID = new SelectList(userGroupDAO.listAll(), "ID", "Name");
            ViewBag.DepartmentID = new SelectList(departmentDAO.listAll(), "ID", "Name");
        }

        [HttpGet]
        public ActionResult EditPersonalInfo(long id)
        {
            var dao = new UserDAO();
            var model = dao.GetByID(id);
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPersonalInfo(User entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var currentEntity = dao.GetByName(entity.UserName);
                entity.CreatedDate = currentEntity.CreatedDate;
                entity.CreatedBy = currentEntity.CreatedBy;
                entity.ModifiedBy = currentEntity.ModifiedBy;
                entity.ModifiedDate = currentEntity.ModifiedDate;
                var result = dao.Update(entity);
                if (result)
                {
                    Session[CommonConstants.USER_SESSION] = null;
                    SetAlert("Cập nhật thông tin thành công", "success");
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    SetAlert("Cập nhật thất bại", "danger");
                    ModelState.AddModelError("", "Cập nhật thất bại!");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string newPassword, string confirmNewPassword)
        {
            if (newPassword != "" && confirmNewPassword != "")
            {
                if (newPassword.Equals(confirmNewPassword))
                {
                    var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                    var dao = new UserDAO();
                    var currentUser = dao.GetByID(session.UserID);
                    currentUser.Password = newPassword;
                    var result = dao.Update(currentUser);
                    if (result)
                    {
                        Session[CommonConstants.USER_SESSION] = null;
                        SetAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        SetAlert("Đổi mật khẩu thất bại", "danger");
                    }
                }
                else
                {
                    SetAlert("Mật khẩu và xác nhận mật khẩu không trùng khớp", "danger");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập các trường thông tin cần thiết", "danger");
            }
            return View();
        }

    }
}