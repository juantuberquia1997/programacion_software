var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        //alerta("Agregar", "Agregando");
        $("#txtCodDirec").val("0");
        ejecutarComando("POST")
    });
    $("#btnModi").on("click", function () {
        //alerta("Modificar", "Modificando");
        let cod = $("#txtCodDirec").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero una dirección");
            return;
        }
        ejecutarComando("PUT")
    });
    $("#btnBusc").on("click", function () {
        //alerta("Buscar", "Buscando");
        Consultar();
    });
    $("#btnCanc").on("click", function () {
        //alerta("Cancelar", "Cancelando");
        Cancelar();
    });
    inicializarFormulario()

});  //Del: jQuery

async function inicializarFormulario() {
    await llenarCombosDpto();
    await llenarComboClientes();
    await llenarTabla();
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(direccion, idDepto, idCiudad, idCliente) {
    var msj = "Falta ";
    if (!direccion || direccion.trim() === "") {
        msj = msj + "La dirección o complemento no es válido,\n";
    }
    if (!idDepto || isNaN(parseInt(idDepto, 10)) || parseInt(idDepto, 10) <= 0) {
        msj = msj + "Seleccionar un departamento válido,\n";
    }
    if (!idCiudad || isNaN(parseInt(idCiudad, 10)) || parseInt(idCiudad, 10) <= 0) {
        msj = msj + "Seleccionar una ciudad válida,\n";
    }
    if (!idCliente || isNaN(parseInt(idCliente, 10)) || parseInt(idCliente, 10) <= 0) {
        msj = msj + "Seleccionar un cliente válido,\n";
    }
    if (msj !== "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

async function llenarCombosDpto() {
    let url = dir + "departamento/listadoDepartamentos";
    let rpta = await llenarComboGral(url, "#cboDepart", "idDepartamento", "nombreDepartamento");

    if (rpta != "Termino") {
        alerta("Error llenando combo de departamento!", "Error en procedimiento")
        return;
    }
    await llenarCombosCiudad();
}

async function llenarCombosCiudad(idCiu) {
    let idDpto = $("#cboDepart").val();
    let url = dir + "ciudad/listadoCiudadsXDpto?codDpto=" + idDpto;
    let rpta = await llenarComboGral(url, "#cboCiudad", "idCiudad", "nombreCiudad");

    if (rpta != "Termino") {
        alerta("Error llenando combo de ciudad!", "Error en procedimiento")
        return;
    } else if (idCiu != undefined) {
        $("#cboCiudad").val(idCiu);
    }
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

function obtenerTextoCombo(selectorCombo, valor) {
    const opcion = $(`${selectorCombo} option[value="${valor}"]`);
    return opcion.length > 0 ? opcion.text().trim() : (valor ?? "");
}

function registrarEventoIrDireccion() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = $("#tblDatos").DataTable().row(fila).data();
        console.log("Click IR -> datosFila:", datosFila);
        if (!datosFila) return;

        // Probamos cargar primero SIN llamar a Consultar() para ver si los campos toman el valor de la fila
        $("#txtCodDirec").val(datosFila.idDir ?? "");
        $("#txtCodigo").val(datosFila.idDir ?? "");
        $("#txtDirec").val(datosFila["Direccion/complemento"] ?? "");
        if (datosFila._idCliente) {
            $("#cboCliente").val(datosFila._idCliente);
        }
        if (datosFila._idDepart) {
            $("#cboDepart").val(datosFila._idDepart);
        }
        if (datosFila._idCiudad) {
            $("#cboCiudad").val(datosFila._idCiudad);
        }

        console.log("Valores asignados -> codDirec:", $("#txtCodDirec").val(), "codigo:", $("#txtCodigo").val(), "direc:", $("#txtDirec").val());
    });
}

async function llenarTabla() {
    let idCliente = $("#cboCliente").val();
    if (!idCliente || idCliente === "null" || isNaN(parseInt(idCliente, 10)) || parseInt(idCliente, 10) <= 0) {
        if ($.fn.DataTable.isDataTable("#tblDatos")) {
            $("#tblDatos").DataTable().clear().draw();
        }
        return;
    }

    let url = dir + "cliente/listadoDirXCliente?codCliente=" + idCliente.trim();

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datos = await respuesta.json();

        const filasFormateadas = Array.isArray(datos)
            ? datos.map(item => ({
                idDir: item.idDir,
                ciudad: item.ciudad,
                departamento: obtenerTextoCombo("#cboDepart", item.iddepart),
                cliente: obtenerTextoCombo("#cboCliente", item.ciente ?? idCliente),
                "Direccion/complemento": item.direccion,
                _idCiudad: item.idCiudad,
                _idDepart: item.iddepart,
                _idCliente: item.ciente ?? idCliente
            }))
            : [];

        oTabla = $("#tblDatos").DataTable({
            data: filasFormateadas,
            destroy: true,
            columns: [
                {
                    data: null,
                    title: "editar",
                    orderable: false,
                    searchable: false,
                    defaultContent: '<button type="button" class="btn btn-info btn-sm btn-ir">ir</button>'
                },
                { data: "idDir", title: "idDir" },
                { data: "ciudad", title: "Ciudad" },
                { data: "departamento", title: "Departamento" },
                { data: "cliente", title: "Cliente" },
                { data: "Direccion/complemento", title: "Direccion/complemento" }
            ]
        });

        registrarEventoIrDireccion();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

function Cancelar() {
    $("#txtCodDirec").val("0");
    $("#cboCiudad")[0].options[0].selected = true;
    $("#cboDepart")[0].options[0].selected = true;
    $("#cboCliente")[0].options[0].selected = true;
    $("#txtDirec").val("");

    $("#cboCliente").trigger("focus");
}

async function Consultar() {

    try {

        let codCliente = $("#cboCliente").val();
        Cancelar();
        $("#cboCliente").val(codCliente);

        if (isNaN(codCliente) || parseFloat(codCliente, 10) <= 0 || codCliente.trim() == "") {
            alerta("Error, el nro. del documento no es válido", "Error en procedimiento")
            $("#cboCliente").trigger("focus");
            return;
        }
        let url = dir + "cliente/listadoDirXCliente?codCliente=" + codCliente

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors", //url,
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró dirección para el cliente código: " + codCliente.trim(), "Información del proceso");
            return;
        }

        const reg = datosin[0];
        getCodigoInput().val(reg.idDir ?? "0");
        $("#cboCliente").val(reg.ciente ?? codCliente);
        $("#txtDirec").val(reg.direccion ?? "");

        if (reg.iddepart) {
            $("#cboDepart").val(reg.iddepart);
            await llenarCombosCiudad(reg.idCiudad);
        } else if (reg.idCiudad) {
            $("#cboCiudad").val(reg.idCiudad);
        }


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    let codi = $("#txtCodDirec").val();
    let ciu = $("#cboCiudad").val();
    let depto = $("#cboDepart").val();
    let clien = $("#cboCliente").val();
    let direccion = $("#txtDirec").val();

    // Ruta por defecto para modificar (PUT)
    let comp = "cliente/modificarDireccion";

    if (accion === "POST") {
        codi = "0";
        $("#txtCodDirec").val("0");
        // Ruta para agregar (POST)
        comp = "cliente/agregarDireccion";
    }

    if (!validar(direccion, depto, ciu, clien)) {
        $("#txtDirec").trigger("focus");
        return;
    }

    if (accion === "PUT" && (!codi || isNaN(codi) || parseInt(codi, 10) <= 0 || codi.trim() === "")) {
        alerta("Código no válido para modificar registro", "Alerta");
        return;
    }

    const textoAccion = accion === "POST" ? "agregar" : "modificar";
    const confirma = window.confirm(`¿Estás seguro de ${textoAccion} datos de: ${direccion.trim()}?`);
    if (!confirma) {
        alerta(`Cancelada acción de ${textoAccion}`, "Información");
        return;
    }

    const datosOut = {
        IdDireccion: parseInt(codi, 10),
        IdCiudad: parseInt(ciu, 10),
        IdCliente: parseInt(clien, 10),
        IdDepartamento: parseInt(depto, 10),
        DireccionCliente: direccion.trim()
    };

    try {
        const response = await fetch(dir + comp, {
            method: accion,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        const textoRpta = await response.text();
        let rpta = null;

        if (textoRpta && textoRpta.trim() !== "") {
            rpta = JSON.parse(textoRpta);
        } else if (response.ok) {
            rpta = codi;
        }

        if (rpta == -1) {
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        } else if (rpta == 0) {
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        } else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodDirec").val(rpta);
            await llenarTabla();
        } else {
            const msj = typeof rpta === "object" && rpta !== null ? JSON.stringify(rpta) : String(rpta ?? "");
            alerta(msj, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}