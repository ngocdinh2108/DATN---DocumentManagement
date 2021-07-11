var user = {
    init: function () {
        user.registerEvents();
    },

    registerEvents: function () {
        $('.btnStatus').off('click').on('click', function (e) {
            e.preventDefault();
            var btnThis = $(this);
            var id = btnThis.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btnThis.text('Kích hoạt');
                    }
                    else {
                        btnThis.text('Khóa');
                    }
                }
            })
        });
    }
}

user.init();