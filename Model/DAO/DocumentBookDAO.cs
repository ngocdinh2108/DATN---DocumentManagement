using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DocumentBookDAO
    {
        DocumentManagementDbContext db = null;

        public DocumentBookDAO()
        {
            db = new DocumentManagementDbContext();
        }

        public List<DocumentBook> ListAll()
        {
            return db.DocumentBooks.ToList();
        }
    }
}
