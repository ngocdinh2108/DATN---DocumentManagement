using Model.EF;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserUserGroupDepartmentDAO
    {
        DocumentManagementDbContext db = null;

        public UserUserGroupDepartmentDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public UserUserGroupDepartment GetByID(long id)
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
                            DepartmentName = d.Name,
                            Avatar = u.Avatar
                        };
            return model.SingleOrDefault(x => x.ID == id);
        }
    }
}
