using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class DepartmentDAO
    {
        DocumentManagementDbContext db = null;

        public DepartmentDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public string Insert(Department entity)
        {
            db.Departments.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Department GetByID(string id)
        {
            return db.Departments.Find(id);
        }

        public List<Department> listAll()
        {
            return db.Departments.ToList();
        }

        public IEnumerable<Department> ListAllPaging(string searchString, int pageNumber, int pageSize)
        {
            var model = db.Departments.ToList();
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

        public bool Update(Department entity)
        {
            try
            {
                var department = GetByID(entity.ID);
                department.Name = entity.Name;
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
                var department = db.Departments.Find(id);
                db.Departments.Remove(department);
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
