document.addEventListener('DOMContentLoaded', function () {
    var enviar = document.getElementById('submitBtn');

    function validateFields() {
        const email = document.getElementById('usernameL').value.trim();
        const password = document.getElementById('passwordL').value.trim();

        if (email && password) {
            enviar.disabled = false;
        } else {
            enviar.disabled = true;
        }
    }
    document.getElementById('usernameL').addEventListener('input', validateFields);
    document.getElementById('passwordL').addEventListener('input', validateFields);
});

//Consultar nombre Cedula

function ConsultarNombre() {

    let identificacion = $("#cedulaCliente").val();

    if (identificacion.length > 0) {
        $.ajax({
            type: "GET",
            url: "https://apis.gometa.org/cedulas/" + identificacion,
            dataType: "json",
            success: function (result) {
                var name = result.results[0].firstname1;
                var lastnames = result.results[0].lastname1 + " " + result.results[0].lastname2
                $("#nombreCliente").val(name);
                $("#apellidosCliente").val(lastnames);

            }
        });
    }
    else {
        $("#nombreCliente").val("");
        $("#nombreCliente").attr("placeholder", "Nombre");
        $("#apellidosCliente").attr("placeholder", "Apellidos");
    }
}

$(document).ready(function () {
    $('#montoDeVenta').on('focus', function () {

        var valorActual = $(this).val().trim();
        if (valorActual === '' || !valorActual.startsWith('₡')) {
            $(this).val('₡ ');
        }
    });

    // Manejar el evento cuando se cambia el contenido del campo de entrada
    $('#montoDeVenta').on('input', function () {
        var valorActual = $(this).val().trim();
        var valorNumerico = valorActual.replace('₡', '').trim();
        $(this).val('₡ ' + valorNumerico);


    });

    // // En caso se hacerlo por formulario
    // $('#formulario').on('submit', function (e) {
    //     // Obtener el valor numérico para enviar al backend
    //     var valorNumerico = $('#montoDeVenta').val().replace('₡', '').trim();
    //     // Asignar el valor numérico al campo oculto (opcional)
    //     $('#montoDeVentaHidden').val(valorNumerico);
    //     // Puedes continuar con el envío del formulario
    //     // Ejemplo: this.submit();
    //     // Evitar que el formulario se envíe realmente para este ejemplo
    //     e.preventDefault();
    //     console.log('Valor numérico para enviar al backend:', valorNumerico);
    // });
});