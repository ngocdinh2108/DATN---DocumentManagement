namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tài khoản")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn quyền")]
        [StringLength(50)]
        public string GroupID { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập họ tên")]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public bool Status { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Bạn phải chọn ảnh đại diện")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn phòng ban")]
        [StringLength(50)]
        public string DepartmentID { get; set; }
    }
}
