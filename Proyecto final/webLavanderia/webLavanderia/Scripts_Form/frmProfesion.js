var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        alerta("Agregar", "Agregando");
        $("#txtCodigo").val("0");
        ejecutarComando("POST");
    });

    $("#btnModi").on("click", function () {
        alerta("Modificar", "Modificando");
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero una profesión");
            return;
        }
        ejecutarComando("PUT");
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
        alerta("Impresión", "En futura construcción");
        //Imprimir();
    });

    inicializarProfesion();
}); //Del: jQuery


function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(nombreProfesion) {
    var msj = "Falta ";
    if (!nombreProfesion || nombreProfesion.trim() == "") {
        msj = msj + "El nombre de la profesión no es válido,\n";
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtNomProfe").val("");
    $("#txtNomProfe").trigger("focus");
}

async function inicializarProfesion() {
    await llenarTabla();
}

function registrarEventoIrProfesion() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", async function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = oTabla ? oTabla.row(fila).data() : $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) {
            alerta("No se pudieron leer los datos de la fila", "Error en procedimiento");
            return;
        }

        $("#txtCodigo").val(datosFila.codigo ?? "");
        $("#txtNomProfe").val(datosFila.nombre ?? "");

        if (datosFila.nombre && datosFila.nombre.toString().trim() !== "") {
            await Consultar();
        }
    });
}

async function Consultar() {
    try {
        // CAMBIO: selector ajustado a #txtNomProfe según el HTML[cite: 11]
        let datoBuscado = $("#txtNomProfe").val();
        Cancelar();
        $("#txtNomProfe").val(datoBuscado);

        if (!datoBuscado || datoBuscado.trim() == "") {
            alerta("Error, el dato de búsqueda no es válido", "Error en procedimiento");
            $("#txtNomProfe").trigger("focus");
            return;
        }

        // Ajusta la ruta/parámetros según tu controlador de Profesion en Swagger
        let url = dir + "profesion/consultaProfesion?comando=1&dato=" + datoBuscado.trim();

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datosin = await datosOut.json();

        if (!Array.isArray(datosin) || datosin.length === 0) {
            alerta("No se encontró profesión: " + datoBuscado, "Informacion del proceso");
            return;
        }

        const reg = datosin[0];
        $("#txtCodigo").val(reg.codigo ?? reg.idProfesion ?? "");
        $("#txtNomProfe").val(reg.nombre ?? reg.nombreProfesion ?? reg.profesion ?? "");
    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    // Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtNomProfe").val();

    if (accion === "POST") {
        Codi = "0";
        $("#txtCodigo").val("0");
    }

    if (!validar(Nomb)) {
        $("#txtNomProfe").trigger("focus");
        return;
    }

    // Confirmar procedimiento
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

    // Crear Json (revisa en Swagger los nombres exactos de propiedades de Profesión)
    let datosOut = {
        idProfesion: accion === "POST" ? 0 : (parseInt(Codi, 10) || 0),
        nombreProfesion: Nomb.trim()
    };

    // Invocar el servicio con fetch (ajusta la ruta si es "profesion" o "profesion/agregarProfesion", etc.)
    try {
        const response = await fetch(dir + "profesion", {
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
            rpta = Codi;
        }

        if (rpta == -1) {
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        } else if (rpta == 0) {
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        } else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);
            $("#txtNomProfe").val(Nomb.trim());
            await llenarTabla();
        } else {
            const msj = typeof rpta === "object" && rpta !== null ? JSON.stringify(rpta) : String(rpta ?? "");
            alerta(msj, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

async function llenarTabla() {
    const url = dir + "profesion/listadoProfesiones";

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
            ? datos.map(item => ({
                codigo: item.codigo ?? item.idProfesion ?? "",
                nombre: item.nombre ?? item.nombreProfesion ?? item.profesion ?? ""
            }))
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
                { data: "nombre", title: "Profesión" }
            ]
        });

        registrarEventoIrProfesion();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}