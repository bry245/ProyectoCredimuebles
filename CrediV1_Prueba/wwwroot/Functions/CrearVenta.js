var productos = @Html.Raw(JsonConvert.SerializeObject(ViewBag.Productos));

function buscarProducto() {
    var textoBusqueda = $('#buscarProducto').val().toLowerCase();

    var html = '<ul class="list-group">';
    productos.forEach(function (producto) {
        if (producto.nombre.toLowerCase().includes(textoBusqueda)) {
            html += '<li class="list-group-item" style="cursor: pointer;" onclick="agregarProducto(' + producto.idProducto + ', \'' + producto.nombre + '\')">' + producto.nombre + ' ₡' + producto.costo + '</li>';
        }
    });
    html += '</ul>';

    $('#resultadosProductos').html(html);
}

function agregarProducto(id, nombre) {
    var fila = '<tr data-id="' + id + '"><td>' + nombre + '</td><td><input type="number" min="1" value="1" class="form-control" /></td></tr>';
    $('#productosSeleccionados').append(fila);
    limpiarBuscador();
}

function limpiarBuscador() {
    $('#buscarProducto').val('');
    $('#resultadosProductos').html('');
}

$(document).ready(function () {
    $('#buscarProducto').keyup(function () {
        buscarProducto();
    });
});

function registrarSalida() {
    var productosSeleccionados = [];
    $('#productosSeleccionados tr').each(function () {
        var id = $(this).data('id');
        var nombre = $(this).find('td:eq(0)').text().trim();
        var cantidad = $(this).find('td:eq(1) input').val();

        var producto = {
            idProducto: id,
            nombre: nombre,
            cantidadSalida: cantidad
        };

        productosSeleccionados.push(producto);
    });


    var datos = {
        cedulaCliente: $('#cedulaCliente').val(),
        nombreCliente: $('#nombreCliente').val(),
        apellidosCliente: $('#apellidosCliente').val(),
        telefonoCliente: $('#telefonoCliente').val(),
        correoCliente: $('#correoCliente').val(),
        direccionCliente: $('#direccionCliente').val(),
        numeroFactura: $('#numeroFactura').val(),
        fecha: $('#fecha').val(),
        metodoPago: $('#metodoPago').val(),
        vendedor: $('#vendedor').val(),
        MontoDeVenta: $('#montoDeVenta').val().replace('₡', '').trim(),
        productosCompra: productosSeleccionados
    };
    console.log('Datos a enviar2:', JSON.stringify(datos));
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "3500",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    $.ajax({
        url: '@Url.Action("RegistrarSalida", "Salidas")',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(datos),
        success: function (response) {
            console.log('Registro de salida exitoso:', response);
            console.log('Datos a enviar2:', JSON.stringify(datos));
            localStorage.setItem('successMessage', 'Salida registrada con éxito');

            window.location.href = '@Url.Action("ListadoSalidas", "Salidas")';


        },
        error: function (error) {
            console.error('Error al registrar salida:', error);
            toastr.error('Ocurrió un error, por favor valide los datos');

        }
    });
}

$(document).ready(function () {
    $('#buscarProducto').keyup(function () {
        buscarProducto();
    });
});