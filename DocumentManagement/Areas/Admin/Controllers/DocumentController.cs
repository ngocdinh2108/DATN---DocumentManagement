using DocumentManagement.Common;
using Model.DAO;
using Model.EF;
using Spire.Pdf;
using Spire.Pdf.Exporting;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Areas.Admin.Controllers
{
    public class DocumentController : BaseController
    {
        public static string temp; // lưu link văn bản để đọc File
        // GET: Admin/Document

        [HttpGet]
        [HasCredential(RoleID = "ADD_DISPATCH_ARRIVED")]
        public ActionResult CreateDispatchArrived()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DISPATCH_ARRIVED")]
        public ActionResult CreateDispatchArrived(Document document)
        {
            var dao = new DocumentDAO();
            if (ModelState.IsValid)
            {
                if (document.DateArrived <= DateTime.Now)
                {
                    if (document.DocumentBookID.Equals("DISPATCHARRIVEDBOOK"))
                    {
                        document.CreatedDate = DateTime.Now;
                        document.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                        document.Status = "PENDING";
                        document.To = "BGH";
                        long id = dao.Insert(document);
                        if (id != 0)
                        {
                            SetAlert("Tạo mới thành công", "success");
                            return RedirectToAction("IndexDispatchArrivedPending", "Document", FormMethod.Get);
                        }
                        else
                        {
                            SetAlert("Tạo mới thất bại", "danger");
                        }
                    }
                    else
                    {
                        SetAlert("Sổ văn bản không hợp lệ", "danger");
                    }

                }
                else
                {
                    SetAlert("Ngày đến không hợp lệ", "danger");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "VIEW_LIST_DISPATCH_ARRIVED_PENDING")]
        public ActionResult IndexDispatchArrivedPending(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchArrivedPending(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "UPDATE_DISPATCH_ARRIVED")]
        public ActionResult EditDispatchArrivedPending(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            DateTime dt = DateTime.ParseExact(model.DateArrived.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string dateArrived = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.DateArrived = dateArrived;
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_DISPATCH_ARRIVED")]
        public ActionResult EditDispatchArrivedPending(Document entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.DateArrived <= DateTime.Now)
                {
                    if (entity.DocumentBookID.Equals("DISPATCHARRIVEDBOOK"))
                    {
                        var dao = new DocumentDAO();
                        var currentDocument = dao.GetByID(entity.ID);
                        if (currentDocument.Status.Equals("PENDING"))
                        {
                            entity.CreatedBy = currentDocument.CreatedBy;
                            entity.CreatedDate = currentDocument.CreatedDate;
                            entity.Status = currentDocument.Status;
                            entity.OnlyView = currentDocument.OnlyView;
                            entity.ModifiedDate = DateTime.Now;
                            entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                            entity.To = currentDocument.To;
                            var result = dao.Update(entity);
                            if (result)
                            {
                                SetAlert("Cập nhật thành công", "success");
                                return RedirectToAction("IndexDispatchArrivedPending", "Document");
                            }
                            else
                            {
                                SetAlert("Cập nhật thất bại", "danger");
                            }
                        }
                        else
                        {
                            SetAlert("Không thể cập nhật vì văn bản không phải là văn bản chờ duyệt", "danger");
                        }
                    }
                    else
                    {
                        SetAlert("Sổ văn bản không hợp lệ", "danger");
                    }
                }
                else
                {
                    SetAlert("Ngày đến không hợp lệ", "danger");
                }

            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            SetDateArrived(entity.ID);
            return View();
        }

        [HasCredential(RoleID = "DELETE_DISPATCH")]
        public JsonResult DeleteDispatchPending(long id)
        {
            var dao = new DocumentDAO();
            var result = dao.Delete(id);
            return Json(new
            {
                status = result
            });
        }

        public void SetDateArrived(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            DateTime dt = DateTime.ParseExact(model.DateArrived.ToString(), "M/d/yyyy h:m:s tt", CultureInfo.InvariantCulture);
            string dateArrived = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.DateArrived = dateArrived;
        }

        public void SetDateIssued(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            DateTime dt = DateTime.ParseExact(model.DateIssued.ToString(), "M/d/yyyy h:m:s tt", CultureInfo.InvariantCulture);
            string dateIssued = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.DateIssued = dateIssued;
        }

        [HttpGet]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_ARRIVED")]
        public ActionResult ApprovalDispatchArrived(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            SetDateArrived(id);
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_ARRIVED")]
        public ActionResult ApprovalDispatchArrived(Document entity)
        {
            var dao = new DocumentDAO();
            var currentDocument = dao.GetByID(entity.ID);
            if (entity.Opinion != null)
            {
                if (currentDocument.Status.Equals("PENDING"))
                {
                    entity.AttachedFile = currentDocument.AttachedFile;
                    entity.CreatedDate = currentDocument.CreatedDate;
                    entity.CreatedBy = currentDocument.CreatedBy;
                    entity.ConfirmBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                    entity.DocumentBookID = currentDocument.DocumentBookID;
                    entity.DocumentTypeID = currentDocument.DocumentTypeID;
                    entity.ModifiedBy = currentDocument.ModifiedBy;
                    entity.ModifiedDate = currentDocument.ModifiedDate;
                    entity.OnlyView = currentDocument.OnlyView;
                    entity.To = "VANTHU";
                    entity.Status = "WAITINGISSUED";
                    var result = dao.Update(entity);
                    if (result)
                    {
                        SetAlert("Xét duyệt thành công! Văn bản đã được chuyển đến văn thư đợi ban hành", "success");
                        return RedirectToAction("IndexDispatchArrivedPending", "Document");
                    }
                    else
                    {
                        SetAlert("Xét duyệt thất bại!", "danger");
                    }
                }
                else
                {
                    SetAlert("Không thể duyệt vì văn bản không phải văn bản chờ duyệt!", "danger");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập ý kiến chỉ đạo!", "danger");
            }
            SetDateArrived(entity.ID);
            SetViewBag();
            return View(entity);
        }

        public void SetViewBag()
        {
            var documentTypeDAO = new DocumentTypeDAO();
            var documentBookDAO = new DocumentBookDAO();
            var departmentDAO = new DepartmentDAO();
            ViewBag.DocumentTypeID = new SelectList(documentTypeDAO.ListAll(), "ID", "Name");
            ViewBag.DocumentBookID = new SelectList(documentBookDAO.ListAll(), "ID", "Name");
            ViewBag.ReceivingDepartment = new SelectList(departmentDAO.listAll(), "ID", "Name");
            ViewBag.DepartmentIssued = new SelectList(departmentDAO.listAll(), "ID", "Name");
        }

        public void SetReceivingDepartmentName(long id)
        {
            var dao = new DocumentDAO();
            var dao2 = new DepartmentDAO();
            var model = dao.GetByID(id);
            if (model.ReceivingDepartment != null)
            {
                ViewBag.ReceivingDepartmentName = dao2.GetByID(model.ReceivingDepartment).Name;
            }
        }

        [HasCredential(RoleID = "VIEW_LIST_DISPATCH_ARRIVED_WAITNG_ISSUED")]
        public ActionResult IndexDispatchArrivedWaitingIssued(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchArrivedWaitingIssued(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ISSUING_DISPATCH_ARRIVED")]
        public ActionResult IssuingDispatchArrived(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            DateTime dt = DateTime.ParseExact(model.DateArrived.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string dateArrived = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.DateArrived = dateArrived;
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "ISSUING_DISPATCH_ARRIVED")]
        public ActionResult IssuingDispatchArrived(Document entity)
        {
            var dao = new DocumentDAO();
            var currentDocument = dao.GetByID(entity.ID);
            entity.AttachedFile = currentDocument.AttachedFile;
            entity.CreatedDate = currentDocument.CreatedDate;
            entity.CreatedBy = currentDocument.CreatedBy;
            entity.ConfirmBy = currentDocument.ConfirmBy;
            entity.DateIssued = DateTime.Now;
            entity.DocumentBookID = currentDocument.DocumentBookID;
            entity.DocumentTypeID = currentDocument.DocumentTypeID;
            entity.ModifiedBy = currentDocument.ModifiedBy;
            entity.ModifiedDate = currentDocument.ModifiedDate;
            entity.To = entity.ReceivingDepartment;
            entity.Status = "ISSUED";
            var result = dao.Issuing(entity);
            if (result)
            {
                SetAlert("Ban hành thành công!", "success");
                return RedirectToAction("IndexDispatchArrivedWaitingIssued", "Document");
            }
            else
            {
                SetAlert("Ban hành thất bại!", "danger");
            }
            SetViewBag();
            return View();
        }

        public ActionResult IndexDispatchArrivedIssued(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchArrivedIssued(searchString, pageNumber, pageSize);
            return View(model);
        }

        public ActionResult ViewDetailDispatchArrived(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            SetDateArrived(id);
            SetDateIssued(id);
            SetReceivingDepartmentName(id);
            SetViewBag();
            return View(model);
        }

        public FileResult OpenFile(string fileName)
        {
            if (fileName == null)
            {
                fileName = temp;
            }
            else
            {
                temp = fileName;
            }
            string filePath = Server.MapPath(Url.Content(fileName));
            byte[] FileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(FileBytes, "application/pdf");
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_DISPATCH_GO")]
        public ActionResult CreateDispatchGo()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DISPATCH_GO")]
        public ActionResult CreateDispatchGo(Document document)
        {
            var dao = new DocumentDAO();
            if (ModelState.IsValid)
            {
                if (document.DateArrived <= DateTime.Now)
                {
                    if (document.DocumentBookID.Equals("DISPATCHGOBOOK"))
                    {
                        document.CreatedDate = DateTime.Now;
                        document.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                        document.Status = "PENDING";
                        document.To = "BGH";
                        long id = dao.Insert(document);
                        if (id != 0)
                        {
                            SetAlert("Tạo mới thành công", "success");
                            return RedirectToAction("IndexDispatchGoPending", "Document", FormMethod.Get);
                        }
                        else
                        {
                            SetAlert("Tạo mới thất bại", "danger");
                        }
                    }
                    else
                    {
                        SetAlert("Sổ văn bản không hợp lệ", "danger");
                    }
                }
                else
                {
                    SetAlert("Ngày đến không hợp lệ", "danger");
                }

            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "VIEW_LIST_DISPATCH_GO_PENDING")]
        public ActionResult IndexDispatchGoPending(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchGoPending(searchString, pageNumber, pageSize);
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_GO")]
        public ActionResult ApprovalDispatchGo(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            SetDateArrived(id);
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_GO")]
        public ActionResult ApprovalDispatchGo(Document entity)
        {
            var dao = new DocumentDAO();
            var currentDocument = dao.GetByID(entity.ID);
            if (currentDocument.Status.Equals("PENDING"))
            {
                string attachedFileNew = SignedDocument(currentDocument.AttachedFile);
                if (attachedFileNew == null)
                {
                    SetAlert("File lỗi! Không tìm được chỗ để ký duyệt", "danger");
                }
                else
                {
                    entity.AttachedFile = attachedFileNew;
                    entity.CreatedDate = currentDocument.CreatedDate;
                    entity.CreatedBy = currentDocument.CreatedBy;
                    entity.ConfirmBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                    entity.DocumentBookID = currentDocument.DocumentBookID;
                    entity.DocumentTypeID = currentDocument.DocumentTypeID;
                    entity.ModifiedBy = currentDocument.ModifiedBy;
                    entity.ModifiedDate = currentDocument.ModifiedDate;
                    entity.OnlyView = currentDocument.OnlyView;
                    entity.DateIssued = DateTime.Now;
                    entity.ReceivingDepartment = currentDocument.DepartmentIssued;
                    entity.Status = "ISSUED";
                    entity.DepartmentIssued = currentDocument.DepartmentIssued;
                    var result = dao.Update(entity);
                    if (result)
                    {
                        SetAlert("Xét duyệt thành công! Văn bản đã được chuyển đến văn thư đợi ban hành", "success");
                        return RedirectToAction("IndexDispatchGoPending", "Document");
                    }
                    else
                    {
                        SetAlert("Xét duyệt thất bại!", "danger");
                    }
                }
            }
            else
            {
                SetAlert("Không thể duyệt vì văn bản không phải văn bản chờ duyệt!", "danger");
            }
            entity.DepartmentIssued = currentDocument.DepartmentIssued;
            entity.DocumentBookID = currentDocument.DocumentBookID;
            entity.DocumentTypeID = currentDocument.DocumentTypeID;
            SetDateArrived(entity.ID);
            SetViewBag();
            return View(entity);
        }

        [HttpGet]
        [HasCredential(RoleID = "UPDATE_DISPATCH_GO")]
        public ActionResult EditDispatchGoPending(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            DateTime dt = DateTime.ParseExact(model.DateArrived.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string dateArrived = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.DateArrived = dateArrived;
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "UPDATE_DISPATCH_GO")]
        public ActionResult EditDispatchGoPending(Document entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.DateArrived <= DateTime.Now)
                {
                    if (entity.DocumentBookID.Equals("DISPATCHGOBOOK"))
                    {
                        var dao = new DocumentDAO();
                        var currentDocument = dao.GetByID(entity.ID);
                        if (currentDocument.Status.Equals("PENDING"))
                        {
                            entity.CreatedBy = currentDocument.CreatedBy;
                            entity.CreatedDate = currentDocument.CreatedDate;
                            entity.Status = currentDocument.Status;
                            entity.OnlyView = currentDocument.OnlyView;
                            entity.ModifiedDate = DateTime.Now;
                            entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                            entity.To = currentDocument.To;
                            var result = dao.Update(entity);
                            if (result)
                            {
                                SetAlert("Cập nhật thành công", "success");
                                return RedirectToAction("IndexDispatchGoPending", "Document");
                            }
                            else
                            {
                                SetAlert("Cập nhật thất bại", "danger");
                            }
                        }
                        else
                        {
                            SetAlert("Không thể cập nhật vì văn bản không phải là văn bản chờ duyệt", "danger");
                        }
                    }
                    else
                    {
                        SetAlert("Sổ văn bản không hợp lệ", "danger");
                    }
                }
                else
                {
                    SetAlert("Ngày đến không hợp lệ", "danger");
                }

            }
            else
            {
                SetAlert("Bạn phải nhập đầy đủ các thông tin cần thiết", "danger");
            }
            SetViewBag();
            SetDateArrived(entity.ID);
            return View();
        }

        public string SignedDocument(string filePath)
        {
            try
            {
                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromFile(Server.MapPath(Url.Content(filePath)));
                int pageNumber = pdf.Pages.Count;
                PdfImageInfo[] imageInfo = pdf.Pages[pageNumber - 1].ImagesInfo;
                RectangleF rect = imageInfo[0].Bounds;
                float x = rect.X;
                float y = rect.Y;
                float width = rect.Width;
                float height = rect.Height;

                // remove image
                PdfPageBase page = pdf.Pages[pageNumber - 1];
                page.DeleteImage(0);

                // add image
                PdfImage image = PdfImage.FromFile(Server.MapPath(Url.Content("~/data/chuky.png")));
                PdfImage image2 = PdfImage.FromFile(Server.MapPath(Url.Content("~/data/condau.png")));
                page.Canvas.DrawImage(image, x, y, width, height);
                page.Canvas.DrawImage(image2, x - 40, y - 5, height, height);
                string fileInpName = filePath.Substring(filePath.LastIndexOf("/"));
                int index = fileInpName.IndexOf(".pdf");
                string filePathOut = fileInpName.Insert(index, "Signed");
                pdf.SaveToFile(Server.MapPath(Url.Content("~/DocumentMemory" + filePathOut + "")));
                return "/DocumentMemory" + filePathOut + "";
            }
            catch
            {
                // lỗi khi không tìm được chỗ ký
            }
            return null;
        }

        public ActionResult IndexDispatchGoIssued(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchGoIssued(searchString, pageNumber, pageSize);
            SetViewBag();
            return View(model);
        }

        public ActionResult ViewDetailDispatchGo(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            SetDateArrived(id);
            SetDateIssued(id);
            SetReceivingDepartmentName(id);
            SetViewBag();
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_GO")]
        public ActionResult RejectApproval(long id)
        {
            var dao = new DocumentDAO();
            var model = dao.GetByID(id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "APPROVAL_DISPATCH_GO")]
        public ActionResult RejectApproval(Document entity)
        {
            if (entity.Opinion != null)
            {
                var dao = new DocumentDAO();
                var currentDocument = dao.GetByID(entity.ID);
                currentDocument.Opinion = entity.Opinion;
                currentDocument.Status = "CANCELED";
                currentDocument.DateIssued = DateTime.Now;
                currentDocument.ConfirmBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Name;
                var result = dao.Update(currentDocument);
                if (result)
                {
                    SetAlert("Từ chối duyệt thành công", "success");
                    return RedirectToAction("IndexDispatchGoPending", "Document");
                }
                else
                {
                    SetAlert("Lỗi!", "danger");
                }
            }
            else
            {
                SetAlert("Bạn phải nhập lý do từ chối duyệt", "danger");
            }
            return View();
        }

        [HasCredential(RoleID = "VIEW_LIST_DISPATCH_GO_CANCELED")]
        public ActionResult IndexDispatchGoCanceled(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var model = new DocumentDAO().listAllPagingDispatchGoCanceled(searchString, pageNumber, pageSize);
            SetViewBag();
            return View(model);
        }
    }
}