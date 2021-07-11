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
    public class DocumentTypeController : BaseController
    {
        // GET: Admin/DocumentType
        [HasCredential(RoleID = "VIEW_LIST_DOCUMENT_TYPE")]
        public ActionResult Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var dao = new DocumentTypeDAO();
            var model = dao.ListAllPaging(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_DOCUMENT_TYPE")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DOCUMENT_TYPE")]
        public ActionResult Create(DocumentType entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new DocumentTypeDAO();
                var model = dao.GetByID(entity.ID);
                if (model == null)
                {
                    var id = dao.Insert(entity);
                    if (id != null)
                    {
                        SetAlert("Tạo mới thành công", "success");
                        return RedirectToAction("Index", "DocumentType");
                    }
                    else
                    {
                        SetAlert("Tạo mới thất bại", "danger");
                        ModelState.AddModelError("", "Tạo mới thất bại!");
                    }
                }
                else
                {
                    SetAlert("Mã loại văn bản đã tồn tại", "danger");
                    ModelState.AddModelError("", "Mã loại văn bản đã tồn tại!");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "UPDATE_DOCUMENT_TYPE")]
        public ActionResult Edit(string id)
        {
            var dao = new DocumentTypeDAO();
            var model = dao.GetByID(id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_DOCUMENT_TYPE")]
        public ActionResult Edit(DocumentType entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new DocumentTypeDAO();
                var result = dao.Update(entity);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "DocumentType");
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
        [HasCredential(RoleID = "DELETE_DOCUMENT_TYPE")]
        public JsonResult Delete(string id)
        {
            var dao = new DocumentTypeDAO();
            var result = dao.Delete(id);
            return Json(new
            {
                status = result
            });
        }
    }
}