using Model.EF;
using Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DocumentDAO
    {
        DocumentManagementDbContext db = null;

        public DocumentDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public int TotalDispatchIssued()
        {
            return db.Documents.Where(x => x.Status.Equals("ISSUED")).Count();
        }

        public int TotalDispatchWaitingIssued()
        {
            return db.Documents.Where(x => x.Status.Equals("WAITINGISSUED")).Count();
        }

        public int TotalDispatchPending()
        {
            return db.Documents.Where(x => x.Status.Equals("PENDING")).Count();
        }

        public int TotalDispatchCanceled()
        {
            return db.Documents.Where(x => x.Status.Equals("CANCELED")).Count();
        }

        public DocumentTypeBook ViewDetail(long id)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        where d.ID == id
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name
                        };
            return model.FirstOrDefault();
        }

        public long Insert(Document entity)
        {
            db.Documents.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Document GetByID(long id)
        {
            return db.Documents.Find(id);
        }

        public bool Update(Document entity)
        {
            try
            {
                var document = GetByID(entity.ID);
                document.DocumentTypeID = entity.DocumentTypeID;
                document.DocumentBookID = entity.DocumentBookID;
                document.Number = entity.Number;
                document.DepartmentIssued = entity.DepartmentIssued;
                document.Summary = entity.Summary;
                document.DateIssued = entity.DateIssued;
                document.Status = entity.Status;
                document.Opinion = entity.Opinion;
                document.ReceivingDepartment = entity.ReceivingDepartment;
                document.AttachedFile = entity.AttachedFile;
                document.DateArrived = entity.DateArrived;
                document.ConfirmBy = entity.ConfirmBy;
                document.OnlyView = entity.OnlyView;
                document.To = entity.To;
                document.CreatedBy = entity.CreatedBy;
                document.CreatedDate = entity.CreatedDate;
                document.ModifiedBy = entity.ModifiedBy;
                document.ModifiedDate = entity.ModifiedDate;
                db.SaveChanges();
                return true;

            }
            catch
            {
                // log err
            }
            return false;

        }

        public bool Issuing(Document entity)
        {
            try
            {
                var document = GetByID(entity.ID);
                document.DocumentTypeID = entity.DocumentTypeID;
                document.DocumentBookID = entity.DocumentBookID;
                document.Number = entity.Number;
                document.DepartmentIssued = entity.DepartmentIssued;
                document.Summary = entity.Summary;
                document.DateIssued = entity.DateIssued;
                document.Status = entity.Status;
                document.Opinion = entity.Opinion;
                document.ReceivingDepartment = entity.ReceivingDepartment;
                document.AttachedFile = entity.AttachedFile;
                document.DateArrived = entity.DateArrived;
                document.ConfirmBy = entity.ConfirmBy;
                document.OnlyView = entity.OnlyView;
                document.To = entity.To;
                document.CreatedBy = entity.CreatedBy;
                document.CreatedDate = entity.CreatedDate;
                document.ModifiedBy = entity.ModifiedBy;
                document.ModifiedDate = entity.ModifiedDate;
                db.SaveChanges();
                return true;
            }
            catch
            {
                // log err
            }
            return false;

        }

        public bool Delete(long id)
        {
            try
            {
                var model = GetByID(id);
                if (model.Status.Equals("PENDING"))
                {
                    db.Documents.Remove(model);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchArrivedPending(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateArrived).Where(x => x.DocumentBookID.Equals("DISPATCHARRIVEDBOOK") && x.Status.Equals("PENDING")).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchArrivedWaitingIssued(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateArrived).Where(x => x.DocumentBookID.Equals("DISPATCHARRIVEDBOOK") && x.Status.Equals("WAITINGISSUED")).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchArrivedIssued(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateIssued).Where(x => x.DocumentBookID.Equals("DISPATCHARRIVEDBOOK") && x.Status.Equals("ISSUED")).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchGoPending(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        join dp in db.Departments
                        on d.DepartmentIssued equals dp.ID
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate,
                            DepartmentIssuedName = dp.Name,
                            ReceivingDepartmentName = dp.Name
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateArrived).Where(x => x.DocumentBookID.Equals("DISPATCHGOBOOK") && x.Status.Equals("PENDING")).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchGoIssued(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        join dp in db.Departments
                        on d.DepartmentIssued equals dp.ID
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate,
                            DepartmentIssuedName = dp.Name,
                            ReceivingDepartmentName = dp.Name
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateIssued).Where(x => x.DocumentBookID.Equals("DISPATCHGOBOOK") && x.Status.Equals("ISSUED")).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<DocumentTypeBook> listAllPagingDispatchGoCanceled(string searchString, int pageNumber, int pageSize)
        {
            var model = from d in db.Documents
                        join dt in db.DocumentTypes
                        on d.DocumentTypeID equals dt.ID
                        join db in db.DocumentBooks
                        on d.DocumentBookID equals db.ID
                        orderby d.DateIssued descending
                        join dp in db.Departments
                        on d.DepartmentIssued equals dp.ID
                        select new DocumentTypeBook()
                        {
                            ID = d.ID,
                            DocumentTypeID = d.DocumentTypeID,
                            Number = d.Number,
                            DepartmentIssued = d.DepartmentIssued,
                            Summary = d.Summary,
                            DateIssued = d.DateIssued,
                            Status = d.Status,
                            Opinion = d.Opinion,
                            ReceivingDepartment = d.ReceivingDepartment,
                            AttachedFile = d.AttachedFile,
                            DocumentBookID = d.DocumentBookID,
                            DateArrived = d.DateArrived,
                            ConfirmBy = d.ConfirmBy,
                            OnlyView = d.OnlyView,
                            DocumentBookName = db.Name,
                            DocumentTypeName = dt.Name,
                            To = d.To,
                            CreatedBy = d.CreatedBy,
                            CreatedDate = d.CreatedDate,
                            ModifiedBy = d.ModifiedBy,
                            ModifiedDate = d.ModifiedDate,
                            DepartmentIssuedName = dp.Name,
                            ReceivingDepartmentName = dp.Name
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Number.ToUpper().Contains(searchString) || x.Summary.ToUpper().Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.DateIssued).Where(x => x.DocumentBookID.Equals("DISPATCHGOBOOK") && x.Status.Equals("CANCELED")).ToPagedList(pageNumber, pageSize);
        }

        public int CountDocumentType(string documentType)
        {
            return db.Documents.Count(x => x.DocumentTypeID.Equals(documentType) && x.Status.Equals("ISSUED"));
        }
    }
}
