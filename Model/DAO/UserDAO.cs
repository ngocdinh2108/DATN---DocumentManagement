using Model.EF;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class UserDAO
    {
        DocumentManagementDbContext db = null;

        public UserDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public int Login(string userName, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Password != password)
                {
                    return -1;
                }
                else
                {
                    if (result.Status == true)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

        public User GetByName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public User GetByID(long id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<UserUserGroupDepartment> listAllPaging(string searchString, int pageNumber, int pageSize)
        {
            var model = from u in db.Users
                        join ug in db.UserGroups
                        on u.GroupID equals ug.ID
                        join d in db.Departments
                        on u.DepartmentID equals d.ID
                        orderby u.CreatedDate descending
                        select new UserUserGroupDepartment()
                        {
                            ID = u.ID,
                            UserName = u.UserName,
                            Password = u.Password,
                            GroupID = u.GroupID,
                            Name = u.Name,
                            CreatedDate = u.CreatedDate,
                            ModifiedDate = u.ModifiedDate,
                            CreatedBy = u.CreatedBy,
                            ModifiedBy = u.ModifiedBy,
                            Status = u.Status,
                            Address = u.Address,
                            Phone = u.Phone,
                            Email = u.Email,
                            GroupName = ug.Name,
                            DepartmentName = d.Name
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.UserName.Contains(searchString));
                if (model.Count() > 0)
                {
                    pageSize = model.Count();
                }
            }
            return model.OrderByDescending(x=>x.CreatedDate).ToPagedList(pageNumber, pageSize);
        }

        public bool Update(User entity)
        {
            try
            {
                var user = GetByID(entity.ID);
                user.UserName = entity.UserName;
                user.Password = entity.Password;
                user.CreatedBy = entity.CreatedBy;
                user.CreatedDate = entity.CreatedDate;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = entity.ModifiedDate;
                user.GroupID = entity.GroupID;
                user.DepartmentID = entity.DepartmentID;
                user.Email = entity.Email;
                user.Address = entity.Address;
                user.Avatar = entity.Avatar;
                user.Name = entity.Name;
                user.Phone = entity.Phone;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }

        public bool Delete(long id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = from a in db.Credentials
                       join b in db.UserGroups
                       on a.UserGroupID equals b.ID
                       join c in db.Roles
                       on a.RoleID equals c.ID
                       where user.GroupID == b.ID
                       select new UserCredential
                       {
                           UserGroupID = a.UserGroupID,
                           RoleID = a.RoleID
                       };
            return data.Select(x => x.RoleID).ToList();
        }
    }
}
