var dir = "http://localhost:5124/api/";

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        alerta("Agregar", "Agregando");
        $("#txtCodigo").val("0");
        ejecutarComando("POST")
    });
    $("#btnModi").on("click", function () {
        alerta("Modificar", "Modificando");
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un artista");
            return;
        }
        ejecutarComando("PUT")
    });
    $("#btnBusc").on("click", function () {
        alerta("Buscar", "Buscando");
        Consultar();
    });
    $("#btnCanc").on("click", function () {
        alerta("Cancelar", "Cancelando");
        Cancelar();
    });
    $("#btnImpr").on("click", function () {
        alerta("Imprimir", "En futura construcción");
        //Imprimir();
    });

    inicializarFormulario();

});  //Del: jQuery

async function inicializarFormulario() {
    await llenarComboClientes();
    await llenarComboTipoTelefono();
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

async function llenarComboClientes() {
    let url = dir + "cliente/listadoClientes";
    let rpta = await llenarComboGral(url, "#cboCliente", "idCliente", "nombreApellidoCliente");

    if (rpta != "Termino") {
        alerta("Error llenando combo de cliente!", "Error en procedimiento");
        return false;
    }
    return true;
}

async function llenarComboTipoTelefono() {
    let url = dir + "tipoTelefono/listadoTipoTelefonos";
    let rpta = await llenarComboGral(url, "#cboTipoTel", "idTipoTelefono", "tipoTelefono");

    if (rpta != "Termino") {
        alerta("Error llenando combo de tipo de teléfono!", "Error en procedimiento");
        return false;
    }
    return true;
}