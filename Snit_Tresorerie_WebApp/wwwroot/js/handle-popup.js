// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//(function () {
//    $("#loaderbody").addClass('hide');

//    $(document).bind('ajaxStart', function () {
//        $("#loaderbody").removeClass('hide');
//    }).bind('ajaxStop', function () {
//        $("#loaderbody").addClass('hide');
//    });
//});

//$(document).ready(function () {
//    $("#loaderbody").addClass('hide');
//});




showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

jQueryAjaxPost = form => {
    alert("jQueryAjaxPost run");
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function OnBegin() {
    $('#overlay').html('<div class="overlay"><i class="fas fa-2x fa-sync fa-spin"></i></div>');
}

function OnComplete(data) {
    $('#overlay').html('');
}

function OnSuccess(res) {

    if (res.isValid) {
        $('#view-all').html(res.html)
        $('#form-modal .modal-body').html('');
        $('#form-modal .modal-title').html('');
        $('#form-modal').modal('hide');

        $.notify('submitted successfully', { globalPosition: 'top center', className: 'success' })
    }
    else {
        $('#form-modal .modal-body').html(res.html);
    }
}

function OnFailure(err) {
    console.log(err)
    $.notify('an error occured' + err.html, { globalPosition: 'top center', className: 'danger' })
}


//$(function () {
//    var Toast = Swal.mixin({
//        toast: true,
//        position: 'top-end',
//        showConfirmButton: false,
//        timer: 3000
//    });

//    $('.swalDefaultSuccess').click(function () {
//        Toast.fire({
//            icon: 'success',
//            title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });

//    $('.swalDefaultInfo').click(function () {
//        Toast.fire({
//            icon: 'info',
//            title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.swalDefaultError').click(function () {
//        Toast.fire({
//            icon: 'error',
//            title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.swalDefaultWarning').click(function () {
//        Toast.fire({
//            icon: 'warning',
//            title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.swalDefaultQuestion').click(function () {
//        Toast.fire({
//            icon: 'question',
//            title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });

//    $('.toastrDefaultSuccess').click(function () {
//        toastr.success('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.')
//    });
//    $('.toastrDefaultInfo').click(function () {
//        toastr.info('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.')
//    });
//    $('.toastrDefaultError').click(function () {
//        toastr.error('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.')
//    });
//    $('.toastrDefaultWarning').click(function () {
//        toastr.warning('Lorem ipsum dolor sit amet, consetetur sadipscing elitr.')
//    });

//    $('.toastsDefaultDefault').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultTopLeft').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            position: 'topLeft',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultBottomRight').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            position: 'bottomRight',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultBottomLeft').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            position: 'bottomLeft',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultAutohide').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            autohide: true,
//            delay: 750,
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultNotFixed').click(function () {
//        $(document).Toasts('create', {
//            title: 'Toast Title',
//            fixed: false,
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultFull').click(function () {
//        $(document).Toasts('create', {
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            icon: 'fas fa-envelope fa-lg',
//        })
//    });
//    $('.toastsDefaultFullImage').click(function () {
//        $(document).Toasts('create', {
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            image: '../../dist/img/user3-128x128.jpg',
//            imageAlt: 'User Picture',
//        })
//    });
//    $('.toastsDefaultSuccess').click(function () {
//        $(document).Toasts('create', {
//            class: 'bg-success',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultInfo').click(function () {
//        $(document).Toasts('create', {
//            class: 'bg-info',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultWarning').click(function () {
//        $(document).Toasts('create', {
//            class: 'bg-warning',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultDanger').click(function () {
//        $(document).Toasts('create', {
//            class: 'bg-danger',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//    $('.toastsDefaultMaroon').click(function () {
//        $(document).Toasts('create', {
//            class: 'bg-maroon',
//            title: 'Toast Title',
//            subtitle: 'Subtitle',
//            body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//        })
//    });
//});
