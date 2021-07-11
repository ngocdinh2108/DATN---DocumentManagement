using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserGroupDAO
    {
        DocumentManagementDbContext db = null;

        public UserGroupDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public List<UserGroup> listAll()
        {
            return db.UserGroups.ToList();
        }
    }
}
