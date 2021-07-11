namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Document")]
    public partial class Document
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phải chọn loại văn bản")]
        public string DocumentTypeID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phải nhập số kí hiệu văn bản")]
        public string Number { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Phải nhập đơn vị ban hành")]
        public string DepartmentIssued { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Phải nhập trích yếu")]
        public string Summary { get; set; }

        public DateTime? DateIssued { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "ntext")]
        public string Opinion { get; set; }

        [StringLength(150)]
        public string ReceivingDepartment { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Phải có file văn bản")]
        public string AttachedFile { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phải chọn sổ văn bản")]
        public string DocumentBookID { get; set; }

        [Required(ErrorMessage = "Phải nhập ngày đến")]
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
    }
}
