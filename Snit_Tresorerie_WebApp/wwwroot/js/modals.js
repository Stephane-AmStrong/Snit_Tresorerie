
//$(document).ready(function () {
//    $(".custom-file-input").on("change", function () {
//        var fileName = $(this).val().split("\\").pop();
//        $(this).next('.custom-file-label').html(fileName);
//    });
//});

$(document).ready(function () {
    $(".custom-file-input").on("change", function () {
        var filePath = $(this).val();
        var fileName = filePath.split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

$('#ImgFile').change(function (evt) {
    showimages(evt.target.files);
});

function showimages(files) {
    f = files[0];
    var reader = new FileReader();
    reader.onload = function (evt) {
        $('#images').replaceWith('<output id="images"></output><br />')
        var img = '<img style="width: 50%;" src="' + evt.target.result + '"/>';
        $('#images').append(img);
    }
    reader.readAsDataURL(f);
}

