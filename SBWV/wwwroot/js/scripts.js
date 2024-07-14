function addFavorite(idBook) {


    $.ajax({
        url: '@Url.Action("AddFavorites", "Account")',
        data: { idBook },
        success: function (result) {
            alert(result.msg);
            //todo сердце меняет цвет добавить вместо удаления

            //  $('#' + id).remove();
            //alert(result.msg);
        },
        error: function (result) {

            window.location.href = '/Account/Login'
        }
    })
}

// function doUpdate() {

//     var formData = {
//         Id: 1,
//         Title: $('#Title').val(),
//         Author: $('#Author').val(),
//         Src: ["1", "2", "3"]
//     };

//     $.ajax({
//         type: "POST",
//         url: '@Url.Action("UpdateData", "Books")',
//         data: formData,
//         success: function (result) {
//             alert('success')
//         }
//     })
// }