using DocumentManagement.Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Areas.Admin.Controllers
{
    public class DepartmentController : BaseController
    {
        // GET: Admin/Department
        [HasCredential(RoleID = "VIEW_LIST_DEPARTMENT")]
        public ActionResult Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var dao = new DepartmentDAO();
            var model = dao.ListAllPaging(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult Create(Department entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new DepartmentDAO();
                var model = dao.GetByID(entity.ID);
                if (model == null)
                {
                    var id = dao.Insert(entity);
                    if (id != null)
                    {
                        SetAlert("Tạo mới phòng ban thành công", "success");
                        return RedirectToAction("Index", "Department");
                    }
                    else
                    {
                        SetAlert("Tạo mới phòng ban thất bại", "danger");
                        ModelState.AddModelError("", "Tạo phòng ban thất bại!");
                    }
                }
                else
                {
                    SetAlert("Mã phòng ban đã tồn tại", "danger");
                    ModelState.AddModelError("", "Mã phòng ban đã tồn tại!");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "UPDATE_DEPARTMENT")]
        public ActionResult Edit(string id)
        {
            var dao = new DepartmentDAO();
            var model = dao.GetByID(id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_DEPARTMENT")]
        public ActionResult Edit(Department entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new DepartmentDAO();
                var result = dao.Update(entity);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    SetAlert("Cập nhật thất bại", "danger");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult Delete(string id)
        {
            var dao = new DepartmentDAO();
            var result = dao.Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}