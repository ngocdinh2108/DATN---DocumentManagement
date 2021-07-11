using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{

    [Serializable]
    public class UserCredential
    {
        [StringLength(50)]
        public string UserGroupID { get; set; }

        [StringLength(50)]
        public string RoleID { get; set; }
    }
}
