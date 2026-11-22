var dir = "http://localhost:5124/api/";
var oTabla = $("#tblDatos").DataTable();

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

    inicializarFormulario()

});  //Del: jQuery

async function inicializarFormulario() {
    await llenarCombosDpto();
    const okClientes = await llenarComboClientes();
    if (okClientes) {
        await llenarTabla();
    }
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(name) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El complemento no válido,\n"
    }

    if (msj != "Falta ") {
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

async function llenarTabla() {
    let clien = $("#cboCliente").val();
    if (!clien || clien === "null") {
        return;
    }

    let url = dir + "cliente/listadoDirXCliente?codCliente=" + clien;

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
                cliente: obtenerTextoCombo("#cboCliente", item.ciente),
                "Direccion/complemento": item.direccion,
                // Conservamos los IDs por si los necesitas al pulsar "ir"
                _idCiudad: item.idCiudad,
                _idDepart: item.iddepart,
                _idCliente: item.ciente
            }))
            : [];

        $("#tblDatos").DataTable({
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
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

function editarFila(datosFila) {
    let nroDoc = datosFila.find('td:eq(2)').text
    if (nroDoc > 0) {
        $("#cboCliente").val(nroDoc);
        Consultar();
        alerta("Ok", "Procedimiento con éxito");
        return;
    }
    alerta("Sin registros", "Procedimiento incompleto")
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

        let nroDoc = $("#cboCliente").val();
        Cancelar();

        $("#cboCliente").val(nroDoc);

        if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
            alerta("Error, el nro. del documento no es válido", "Error en procedimiento")
            $("#cboCliente").trigger("focus");
            return;
        }
        let url = dir + "cliente/listadoDirXCliente?codCliente=" + nroDoc

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors", url,
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró artista con nro. doc.: " + nroDoc, "Informacion del proceso")
            return;
        }

        $("#txtCodDirec").val(datosin[0].idDir);
        $("#cboCiudad").val(datosin[0].idCiudad);
        $("#cboDepart").val(datosin[0].iddepart);
        $("#cboCliente").val(datosin[0].ciente);
        $("#txtDirec").val(datosin[0].direccion);


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

    if (accion === "POST") {
        codi = "0";
        $("#txtCodDirec").val("0");
    }

    if (!validar(direccion)) {
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
        const response = await fetch(dir + "cliente", {
            method: accion,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        const rpta = await response.json();

        if (rpta == -1) {
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        } else if (rpta == 0) {
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        } else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodDirec").val(rpta);
            await llenarTabla();
        } else {
            alerta(rpta, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}