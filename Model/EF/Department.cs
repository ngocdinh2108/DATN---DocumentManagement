namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        [Required(ErrorMessage = "Bạn phải nhập mã phòng ban")]
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải nhập tên phòng ban")]
        public string Name { get; set; }
    }
}
