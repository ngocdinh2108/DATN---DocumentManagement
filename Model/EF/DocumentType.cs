namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocumentType")]
    public partial class DocumentType
    {
        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải nhập mã loại văn bản")]
        public string ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải nhập tên loại văn bản")]
        public string Name { get; set; }
    }
}
