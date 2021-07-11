using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentManagement.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }

        public string UserName { get; set; }

        public string GroupID { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string DepartmentID { get; set; }
    }
}