var department = {
    init: function () {
        department.registerEvents();
    },

    registerEvents: function () {
        $('.btnRemove').off('click').on('click', function (e) {
            e.preventDefault();
            var select = confirm('Bạn có muốn xóa bản ghi này');
            var btnThis = $(this);
            if (select == true) {
                var id = btnThis.data('id');
                $.ajax({
                    url: "/Admin/Department/Delete",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        if (response.status == true) {
                            $('#row_' + id + '').remove();
                        }
                        else {
                            alert('Xóa thất bại');
                        }
                    }
                })
            }
            else {
                alert('Thao tác xóa bị hủy bỏ');
            }
        });
    }
}

department.init();