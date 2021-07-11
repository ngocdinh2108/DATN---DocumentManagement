var openFile = {
    init: function () {
        openFile.registerEvent();
    },

    registerEvent: function () {
        $('.btnOpenFile').off('click').on('click', function (e) {
            e.preventDefault();
            var btnThis = $(this);
            var id = btnThis.data('id');
            $.ajax({
                url: "/Admin/Document/OpenFile",
                data: { fileName: id },
                type: "POST",
                success: function () {
                    window.open('/Admin/Document/OpenFile', "_blank"); 
                }
            })
        });
    }
}

openFile.init();