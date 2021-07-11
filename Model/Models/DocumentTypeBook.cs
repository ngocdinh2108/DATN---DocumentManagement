using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class DocumentTypeBook
    {

        [StringLength(50)]
        public string DocumentTypeName { get; set; }

        [StringLength(50)]
        public string DocumentBookName { get; set; }

        public long ID { get; set; }

        [StringLength(50)]
        public string DocumentTypeID { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(150)]
        public string DepartmentIssued { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        public DateTime? DateIssued { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "ntext")]
        public string Opinion { get; set; }

        [StringLength(150)]
        public string ReceivingDepartment { get; set; }

        [StringLength(100)]
        public string AttachedFile { get; set; }

        [StringLength(50)]
        public string DocumentBookID { get; set; }

        public DateTime? DateArrived { get; set; }

        [StringLength(100)]
        public string ConfirmBy { get; set; }

        public bool OnlyView { get; set; }

        [StringLength(100)]
        public string To { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [StringLength(50)]
        public string DepartmentIssuedName { get; set; }

        [StringLength(50)]
        public string ReceivingDepartmentName { get; set; }
    }
}
