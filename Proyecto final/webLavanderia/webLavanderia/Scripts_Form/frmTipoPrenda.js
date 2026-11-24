var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        $("#txtCodigo").val("0");
        ejecutarComando("POST");
    });

    $("#btnModi").on("click", function () {
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero un tipo de prenda");
            return;
        }
        ejecutarComando("PUT");
    });

    $("#btnBusc").on("click", function () {
        Consultar();
    });

    $("#btnCanc").on("click", function () {
        Cancelar();
    });

    inicializarTipoPrenda();
}); //Del: jQuery

async function inicializarTipoPrenda() {
    await llenarTabla();
}

function registrarEventoIrTipoPrenda() {
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

        $("#txtCodigo").val(datosFila.codigo ?? "");
        $("#txtTipoPren").val(datosFila.nombre ?? "");
        $("#txtNombre").val(datosFila.nombre ?? "");
        $("#chkActivo").prop("checked", datosFila.activo);
    });
}

function alerta(msj, titulo) {
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(name) {
    var msj = "Falta ";
    if (!name || name.trim() == "") {
        msj = msj + "El nombre no válido,\n";
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtTipoPren").val("");
    $("#chkActivo").prop("checked", true);  //
    $("#txtTipoPren").trigger("focus");
}

async function Consultar() {
    try {
        let tipPren = $("#txtTipoPren").val();
        Cancelar();
        $("#txtTipoPren").val(tipPren);

        if (tipPren.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento");
            $("#txtTipoPren").trigger("focus");
            return;
        }

        let url = dir + "tipoPrenda/consultaTipoPrenda?comando=1&dato=" + tipPren.trim();

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró tipo prenda: " + tipPren, "Informacion del proceso")
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#txtTipoPren").val(datosin[0].nombre);
        $("#chkActivo").prop(datosin[0].activo);
    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtTipoPren").val();
    let acti = $("#chkActivo").prop("checked");

    if (!validar(Nomb)) {
        $("#txtTipoPren").trigger("focus");
        return;
    }

    if (accion === "POST") {
        Codi = "0";
        $("#txtCodigo").val("0");
    }

    let confirma = false;
    if (accion === "POST") {
        confirma = window.confirm('¿Estás seguro de agregar datos de: ' + Nomb.trim() + '?');
        if (!confirma) {
            alerta('Cancelada acción de agregar', "Información");
            return;
        }
    } else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        confirma = window.confirm('¿Estás seguro de modificar datos de: ' + Nomb.trim() + '?');
        if (!confirma) {
            alerta('Cancelada acción de modificar', "Información");
            return;
        }
    } else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    let datosOut = {
        IdTipoPrenda: accion === "POST" ? 0 : (parseInt(Codi, 10) || 0),
        nombrePrenda: Nomb.trim(),
        activo: Boolean(acti)
    };

    try {
        const response = await fetch(dir + "tipoPrenda", {
            method: accion,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        const rpta = await response.json();
        if (rpta == -1)
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);
            // $("#txtTipoPren").val(Nomb.trim());
            // $("#chkActivo").prop("checked", acti);
            await llenarTabla();
        } else {
            alerta(rpta, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

async function llenarTabla() {
    const url = dir + "tipoPrenda/listadoTipoPrendas";

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const crudo = respuesta.ok ? await respuesta.json() : null;
        const datos = Array.isArray(crudo)
            ? crudo
            : (crudo?.data ?? crudo?.datos ?? crudo?.resultado ?? []);

        const filasFormateadas = Array.isArray(datos)
            ? datos.map(item => {
                const estadoActivo = Boolean(item.activo ?? item.act ?? true);
                return {
                    codigo: item.codigo ?? item.idTipoPrenda ?? "",
                    nombre: item.nombre ?? item.nombrePrenda ?? "",
                    activo: estadoActivo,
                    activoTexto: estadoActivo ? "Activo" : "Inactivo"
                };
            })
            : [];

        oTabla = $("#tblDatos").DataTable({
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
                { data: "nombre", title: "Tipo de Prenda" },
                { data: "activoTexto", title: "Estado" }
            ]
        });

        registrarEventoIrTipoPrenda();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}