﻿$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
$(function () {
    $(".crear-coupon").click(function (event) {
        event.preventDefault();
        console.log("crear cupón");
        var form = $("#form-agregar-coupon");

        if (form.valid())
        {
            modalToClose = $("#crear_coupon");
            var formData = $("#form-agregar-coupon").serializeObject();

            formData.Status = (formData.Status == "Activo") ? true : false;

            $.ajax({
                type: 'POST',
                url: "/Coupon/CreateCoupon",
                data: formData,
                success: function (data) {
                    if (data.success) {
                        verifCloseModal = true;
                        mostrarMensaje(form, "userMensaje", "Cupón registrado", "alert-warning");
                        window.location.reload(false);
                    } else {
                        mostrarMensaje(form, "userMensaje", "Error al registrar cupón intentelo de nuevo", "alert-warning");
                    }
                }, error: function (error) {
                    mostrarMensaje(form, "userMensaje", "Error al registrar cupón intentelo de nuevo", "alert-warning");
                }, complete: function (data) {
                    console.log(data);
                }
            });
        }
        
    });
    $(".editar-coupon-button").click(function (event) {

        var countCheckbox = $("#tabla-coupon").find('input:checkbox:checked').length;
        if (countCheckbox == 0) {
            alert("Debe seleccionar un cupón para editar");
            return false;
        }
        console.log("editar cupón");
        var form = $("#form-editar-coupon");
        var formData = {};

        formData.id = actualEdicion;

        if (actualEdicion != null) {
            $("#editar-id-coupon").val(actualEdicion);

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'GET',
                url: '/Coupon/GetCoupon',
                data: formData,
                success: function (data) {
                    console.log("success data");
                    console.log(data);
                    if (data) {
                        form.find("#Name").val(data.Name);
                        form.find("#Quantity").val(data.Quantity);
                        form.find("#Discount").val(data.Discount);
                        form.find("#fecha_cupon_inicio_edit").val(data.From);
                        form.find("#fecha_cupon_vigencia_edit").val(data.To);
                        form.find("#Created_by").val(data.Created_by);
                        form.find("#Created_by").trigger("chosen:updated");
                        //form.find("#fecha_cupon_creacion_edit").val(data.Slu);
                        var statusname = "Activo"
                        if (!data.Status)
                            statusname = "Inactivo"
                        form.find("#Status").val(statusname);
                        form.find("#Status").trigger("chosen:updated");

                        $("#editar_coupon").modal("show");
                    }
                }, error: function (error) {
                    mostrarMensaje(form, "userMensaje", "Error al registrar cupón intentelo de nuevo", "alert-warning");
                }, statusCode: {
                    404: function () {
                        alert("page not found");
                    }
                }, complete: function (data) {
                    console.log(data);
                }
            });
        } else {
            alert("Debe seleccionar un cupón para editar")
        }

    });
    $(".modificar-coupon").click(function (event) {
        event.preventDefault();
        modalToClose = $("#editar_coupon");
        console.log("modificar cupón");
        var form = $("#form-editar-coupon");

        if (form.valid())
        {
            var formData = $("#form-editar-coupon").serializeObject();

            formData.status = (formData.status == "Activo") ? true : false;

            $.ajax({
                type: 'POST',
                url: '/Coupon/UpdateCoupon',
                data: formData,
                success: function (data) {
                    if (data.success) {
                        mostrarMensaje(form, "userMensaje", "Cupón modificado", "alert-warning");
                        verifCloseModal = true;
                        actualElement.closest("tr").find(".tname").text(form.find("#Name").val());
                        actualElement.closest("tr").find(".tquantity").text(form.find("#Quantity").val());
                        actualElement.closest("tr").find(".tdiscount").text(form.find("#Discount").val());
                        actualElement.closest("tr").find(".tfrom").text(form.find("#fecha_cupon_inicio_edit").val());
                        actualElement.closest("tr").find(".tto").text(form.find("#fecha_cupon_vigencia_edit").val());
                        //actualElement.closest("tr").find(".tcreated").text(form.find("#secure").val());
                        //actualElement.closest("tr").find(".tdatecreated").text(form.find("#slu").val());
                        actualElement.closest("tr").find(".tstatus").text(form.find("#status").val());
                    } else {
                        mostrarMensaje(form, "userMensaje", "Error al modificar cupón intentelo de nuevo", "alert-warning");
                    }
                }, error: function (error) {
                    mostrarMensaje(form, "userMensaje", "Error al modificar cupón intentelo de nuevo", "alert-warning");
                }, complete: function (data) {
                    console.log(data);
                }
            });
        }
        
    });
    $(".eliminar-coupon").click(function (event) {
        if (actualEdicion != null) {
            var mensaje = "Esta seguro que desea eliminar el cupón " + actualName + "?";
            if (confirm(mensaje)) {
                var formData = {};
                formData.id = actualEdicion;
                if (countCheckbox > 1) {
                    $("#tabla-coupon").find('input:checkbox:checked').each(function () {
                        formData = {};
                        formData.id = $(this).attr("data-id");
                        deleteCoupon(formData, $(this));
                    })
                } else if (countCheckbox == 1) {
                    deleteCoupon(formData, actualElement);
                }
            }
        } else {
            alert("Debe seleccionar un cupón para eliminar")
        }
    });
    var actualEdicion = null;
    var actualName = "";
    var actualElement = null;
    $(document).on("click", "#tabla tr", function () {
        $(this).toggleClass('line_selected').siblings().removeClass('line_selected');
        if ($(this).hasClass('line_selected')) {
            actualElement = $(this);
            actualEdicion = $(this).attr("data-id");
            actualName = $(this).closest("tr").find(".tname").text();
        } else {
            actualEdicion = null;
            actualNombre = "";
            actualElement = null;
        }
    });
    //-------------------------JQuery Validate----------------------
    $('#form-agregar-coupon').validate({
        rules: {
            Name: {
                required: true
            },
            Quantity: {
                required: true
            },
            Discount: {
                required: true
            },
            From: {
                required: true
            },
            To: {
                required: true
            },
            "Created_by": {
                required: true
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    $('#form-editar-coupon').validate({
        rules: {
            Name: {
                required: true
            },
            Quantity: {
                required: true
            },
            Discount: {
                required: true
            },
            From: {
                required: true
            },
            To: {
                required: true
            },
            "Created_by": {
                required: true
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    $('.pagination-coupons').pagination({
        items: $('#total-coupons').val(),
        itemsOnPage: 10,
        cssStyle: 'light-theme',
        onPageClick: function (pageNumber, event) {
            event.preventDefault();

            $("#tabla-coupon").html('');
            pageLoading(true);
            var formData = { idPagination: $('#idpagination-coupons').val(), page: pageNumber };
            var cadhtml = "";
            var couponsresult = "";

            $.ajax({
                type: 'POST',
                url: '@Url.Action("PaginationAjax", "Coupon")',
                data: formData,
                success: function (result) {

                    couponsresult = "";

                    for (var i = 0; i < result.Coupons.length; i++) {

                        var couponv = result.Coupons[i];

                        cadhtml = "";
                        cadhtml += "<tr>";
                        cadhtml += "    <td><input type=\"checkbox\" data-id=\"" + couponv.Id + "\" class=\"select-coupon\" \/><\/td>";
                        cadhtml += "    <td class=\"tname\">" + couponv.Name + "<\/td>";
                        cadhtml += "    <td class=\"tquantity\">" + couponv.Quantity + "<\/td>";
                        cadhtml += "    <td class=\"tdiscount\">" + couponv.Discount % +"<\/td>";
                        cadhtml += "    <td class=\"tfrom\">" + couponv.From + "<\/td>";
                        cadhtml += "    <td class=\"tto\">" + couponv.To + "<\/td>";
                        cadhtml += "    <td class=\"tcreated\">-<\/td>";
                        cadhtml += "    <td class=\"tdatecreated\">-<\/td>";
                        cadhtml += "    <td class=\"tstatus\">" + couponv.Statusname + "<\/td>";
                        cadhtml += "<\/tr>";

                        couponsresult += cadhtml;
                    }

                    $("#tabla-coupon").html(couponsresult);

                    console.log(result);
                    pageLoading(false);

                }
            });

        }
    });

    
});
function deleteCoupon(formData, element) {
    $.ajax({
        type: 'POST',
        url: '/Coupon/DeleteCoupon',
        data: formData,
        success: function (data) {
            alert("Cupón eliminado correctamente");
            element.closest("tr").find(".tstatus").text("Inactivo");
        }, error: function (error) {
            alert("Error al eliminar cupón, intentelo nuevamente");
        }, complete: function (data) {
            console.log(data);
        }
    });
}