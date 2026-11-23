var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");
    $("#txtFechaIngreso").val(fechaHoy());

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        //alerta("Agregar", "Agregando");
        $("#txtCodEmp").val("0");
        ejecutarComando("POST");
    });
    $("#btnModi").on("click", function () {
        //alerta("Modificar", "Modificando");
        let cod = $("#txtCodEmp").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un empleado");
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
    inicializarEmpleado()

});  //Del: jQuery

function fechaHoy() {
    let n = new Date();
    let y = n.getFullYear();
    let m = n.getMonth() + 1;
    let d = n.getDate();
    if (d < 10) { d = '0' + d; }
    if (m < 10) { m = '0' + m; }
    var newfec = `${y}-${m}-${d}`;
    return newfec;
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(name, nroDoc) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El nombre no válido,\n"
    }
    if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
        msj = msj + "el nro. de documento no es válido,\n"
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }

    return true;
}

async function inicializarEmpleado() {
    await llenarComboEmpleado();
    await llenarComboTipoDoc();
    await llenarTabla();
}

function seleccionarComboPorTextoEmp(selectorCombo, texto) {
    if (!texto) return;
    const objetivo = texto.toString().trim().toLowerCase();
    $(`${selectorCombo} option`).each(function () {
        if ($(this).text().trim().toLowerCase() === objetivo) {
            $(selectorCombo).val($(this).val());
            return false;
        }
    });
}

function registrarEventoIrEmpleado() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) {
            alerta("No se pudieron leer los datos de la fila", "Error en procedimiento");
            return;
        }

        $("#txtCodEmp").val(datosFila.codigo ?? "");
        $("#txtNombre").val(datosFila.nombre ?? "");
        $("#txtNroDoc").val(datosFila.documento ?? "");
        $("#txtSalario").val(datosFila.salario ?? "");

        if (datosFila.fechaIngreso) {
            $("#txtFechaIngreso").val(datosFila.fechaIngreso.toString().substring(0, 10));
        }

        if (typeof datosFila.activo === "boolean") {
            $("#chkActivo").prop("checked", datosFila.activo);
        }

        if (datosFila.idTipoDoc != undefined) {
            $("#cboTipoDoc").val(datosFila.idTipoDoc);
        } else if (datosFila.tipoDocumento) {
            seleccionarComboPorTextoEmp("#cboTipoDoc", datosFila.tipoDocumento);
        }

        if (datosFila.idEmpleado != undefined) {
            $("#cboEmpleado").val(datosFila.idEmpleado);
        }

        if (datosFila.documento && datosFila.documento.toString().trim() !== "") {
            Consultar();
        }
    });
}

async function llenarComboTipoDoc() {
    let url = dir + "tipoDocumento/listadoTipoDocumentos";
    let rpta = await llenarComboGral(url, "#cboTipoDoc", "idTipoDocumento", "nombreDocumento");

    if (rpta != "Termino") {
        alerta("Error llenando combo de tipo de documento!", "Error en procedimiento");
        return;
    }
}

async function llenarComboEmpleado() {
    let url = dir + "empleado/listadoEmpleados";
    let rpta = await llenarComboGral(url, "#cboEmpleado", "idEmpleado", "nombreApellidoEmp");

    if (rpta != "Termino") {
        alerta("Error llenando combo de empleados!", "Error en procedimiento");
        return;
    }
}

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodEmp").val();
    let Empl = $("#cboEmpleado").val();
    let Nomb = $("#txtNombre").val();
    let idTD = $("#cboTipoDoc").val();
    let nroD = $("#txtNroDoc").val();
    let fech = $("#txtFechaIngreso").val();
    let sala = $("#txtSalario").val();
    let acti = $("#chkActivo").prop("checked");

    if (!validar(Nomb, nroD)) {
        $("#txtNombre").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('¿Estás seguro de agregar datos de: ' + Nomb.trim() + ', con Nro. Doc.: ' + nroD.trim() + '?');
        if (rpta == false) {
            alerta('Cancelada acción de agregar', "Información");
            return;
        }
    }
    else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm('Estas seguro de modificar datos de: ' + Nomb + ', con bro. Doc.: ' + nroD + '?');
        if (rpta == false) {
            alerta('Cancelada acción de modificar', "Información");
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    //Crear Json 
    let datosOut = {
        tblEmpleado: {
            idEmpleado: Codi && Codi.trim() !== "" ? parseInt(Codi, 10) : 0,
            empleadoIdEmpleado: parseInt(Empl, 10),
            idTipoDocumento: parseInt(idTD, 10),
            documentoEmpleado: nroD.trim(),
            nombreApellidoEmp: Nomb.trim(),
            salario: parseFloat(sala),
            fechaIngreso: fechaHoy(),
            activo: acti
        }
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "empleado", {
            method: accion,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(datosOut),
        });   // stringify() convierte un objeto o valor de JavaScript en una cadena de texto JSON
        const rpta = await response.json();
        if (rpta == -1)
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodEmp").val(rpta);
            await llenarComboEmpleado();
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }

}

function Cancelar() {
    $("#txtCodEmp").val("0");
    $("#cboEmpleado")[0].options[0].selected = true;
    $("#txtNombre").val("");
    $("#cboTipoDoc")[0].options[0].selected = true;
    $("#txtNroDoc").val("");
    $("#txtFechaIngreso").val(fechaHoy());
    $("#txtSalario").val("");
    $("#chkActivo").prop("checked", true);
    $("#txtNombre").trigger("focus");
}

async function Consultar() {

    try {

        let nroDoc = $("#txtNroDoc").val();
        Cancelar();

        $("#txtNroDoc").val(nroDoc);

        if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
            alerta("Error, el nro. del documento no es válido", "Error en procedimiento")
            $("#txtNroDoc").trigger("focus");
            return;
        }
        let url = dir + "empleado/consultaEmpleado?comando=1&dato=" + nroDoc

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró empleado con nro. doc.: " + nroDoc, "Informacion del proceso");
            return;
        }

        $("#txtCodEmp").val(datosin[0].codigo);
        $("#cboEmpleado").val(datosin[0].empleado);
        $("#cboTipoDoc").val(datosin[0].idTD);
        $("#txtNroDoc").val(datosin[0].documento);
        $("#txtNombre").val(datosin[0].nombre);
        $("#txtFechaIngreso").val(datosin[0].fechaIng);
        $("#txtSalario").val(datosin[0].salario);
        $("#chkActivo").prop("checked", datosin[0].act);


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function llenarTabla() {
    const url = dir + "empleado/consultaEmpleado?comando=2&dato=0";

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
                codigo: item.codigo ?? item.idEmpleado ?? "",
                nombre: item.nombre ?? item.nombreApellidoEmp ?? "",
                tipoDocumento: item.tipoDoc ?? item.nombreDocumento ?? "",
                documento: item.documento ?? item.documentoEmpleado ?? "",
                salario: item.salario ?? "",
                fechaIngreso: item.fechaIng ?? item.fechaIngreso ?? "",
                activo: item.act ?? item.activo ?? true,
                idTipoDoc: item.idTD ?? item.idTipoDocumento,
                idEmpleado: item.empleado ?? item.idEmpleado
            }))
            : [];

        $("#tblDatos").DataTable({
            data: filasFormateadas,
            destroy: true,
            columns: [
                {
                    data: null,
                    title: "Editar",
                    orderable: false,
                    searchable: false,
                    defaultContent: '<button type="button" class="btn btn-info btn-sm btn-ir">ir</button>'
                },
                { data: "codigo", title: "Código" },
                { data: "nombre", title: "Nombre" },
                { data: "tipoDocumento", title: "Tipo Documento" },
                { data: "documento", title: "Nro. Documento" },
                { data: "salario", title: "Salario" }
            ]
        });

        registrarEventoIrEmpleado();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

