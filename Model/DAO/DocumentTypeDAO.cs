using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DocumentTypeDAO
    {
        DocumentManagementDbContext db = null;

        public DocumentTypeDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public List<DocumentType> ListAll()
        {
            return db.DocumentTypes.ToList();
        }

        public IEnumerable<DocumentType> ListAllPaging(string searchString, int pageNumber, int pageSize)
        {
            var model = db.DocumentTypes.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.ID.ToUpper().Contains(searchString.ToUpper())).ToList();
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x => x.Name).ToPagedList(pageNumber, pageSize);
        }

        public string Insert(DocumentType entity)
        {
            db.DocumentTypes.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public DocumentType GetByID(string id)
        {
            return db.DocumentTypes.Find(id);
        }

        public bool Update(DocumentType entity)
        {
            try
            {
                var documentType = GetByID(entity.ID);
                documentType.Name = entity.Name;
                db.SaveChanges();
                return true;
            }
            catch
            {
                // log
            }
            return false;
        }

        public bool Delete(string id)
        {
            try
            {
                var documentType = db.DocumentTypes.Find(id);
                db.DocumentTypes.Remove(documentType);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }
    }
}
