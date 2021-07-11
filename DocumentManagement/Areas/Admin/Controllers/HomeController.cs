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
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var documentDAO = new DocumentDAO();
            ViewBag.DispatchIssued = documentDAO.TotalDispatchIssued();
            ViewBag.DispatchWaitingIssued = documentDAO.TotalDispatchWaitingIssued();
            ViewBag.DispatchPending = documentDAO.TotalDispatchPending();
            ViewBag.DispatchCanceled = documentDAO.TotalDispatchCanceled();
            return View();
        }

        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            ViewBag.SessionViewBag = Session[CommonConstants.USER_SESSION];
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            ViewBag.SessionViewBag = Session[CommonConstants.USER_SESSION];
            return PartialView();
        }

        public ActionResult GetData()
        {
            var results = new List<SimpleClass>();
            var documentDAO = new DocumentDAO();
            var documentTypeDAO = new DocumentTypeDAO();
            List<DocumentType> listDocumentTypeExisting = documentTypeDAO.ListAll();
            foreach (var item in listDocumentTypeExisting)
            {
                var check = documentDAO.CountDocumentType(item.ID.ToString());
                if (check > 0)
                {
                    results.Add(new SimpleClass { Key = item.Name.ToString(), Value = documentDAO.CountDocumentType(item.ID.ToString()) });
                }
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public class SimpleClass
        {
            public string Key { get; set; }

            public int Value { get; set; }
        }
    }
}