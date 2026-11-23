var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        //alerta("Agregar", "Agregando");
        $("#txtCodigo").val("0");
        ejecutarComando("POST")
    });
    $("#btnModi").on("click", function () {
        //alerta("Modificar", "Modificando");
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un artista");
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
    $("#btnImpr").on("click", function () {
        alerta("Imprimir", "En futura construcción");
        //Imprimir();
    });

    inicializarTipoServicio()

});  //Del: jQuery

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

async function inicializarTipoServicio() {
    await llenarTabla();
}

function registrarEventoIrTipoServicio() {
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
        $("#txtNombre").val(datosFila.nombre ?? "");
        $("#txtValor").val(datosFila.valor ?? "");

        if (typeof datosFila.activo === "boolean") {
            $("#chkActivo").prop("checked", datosFila.activo);
        }
    });
}

function validar(name, nroDoc) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El nombre no válido,\n"
    }
    if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
        msj = msj + "el valor no es válido,\n"
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }

    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtNombre").val("");
    $("#txtValor").val("");
    $("#chkActivo").prop("checked", true);
    $("#txtNombre").trigger("focus");
}

async function Consultar() {

    try {

        let nroDoc = $("#txtNombre").val();
        Cancelar();

        $("#txtNombre").val(nroDoc);

        if (nroDoc.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento")
            $("#txtNombre").trigger("focus");
            return;
        }
        let url = dir + "tipoServicio/consultaTipoServicio?comando=1&dato=" + nroDoc

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors", url,
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró servicio: " + nroDoc, "Informacion del proceso")
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#txtNombre").val(datosin[0].nombre);
        $("#txtValor").val(datosin[0].valor);
        $("#chkActivo").prop("checked", datosin[0].act);


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtNombre").val();
    let Valo = $("#txtValor").val();
    let acti = $("#chkActivo").prop("checked");

    if (!validar(Nomb, Valo)) {
        $("#txtNombre").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('Estas seguro de agregar datos de: ' + Nomb + ', con valor: ' + Valo + '?');
        if (rpta == false) {
            alerta('Cancelada acción de gregar', "Información");
            return;
        }
    }
    else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm('Estas seguro de modificar datos de: ' + Nomb + ', con valor: ' + Valo + '?');
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
        IdTipoServicio: accion === "POST" ? 0 : (parseInt(Codi, 10) || 0),
        NombreServicio: Nomb ? Nomb.trim() : "",
        Valor: parseFloat(Valo) || 0,
        Activo: acti
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "tipoServicio", {
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
            $("#txtCodigo").val(rpta);
            await Consultar();
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }

}

async function llenarTabla() {
    const url = dir + "tipoServicio/consultaTipoServicio?comando=2&dato=0";

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
                const estadoActivo = Boolean(item.act ?? item.activo ?? true);
                return {
                    codigo: item.codigo ?? item.idTipoServicio ?? "",
                    nombre: item.nombre ?? item.nombreServicio ?? "",
                    valor: item.valor ?? "",
                    activo: estadoActivo,
                    activoTexto: estadoActivo ? "Activo" : "Inactivo"
                };
            })
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
                { data: "nombre", title: "Servicio" },
                { data: "valor", title: "Valor" },
                { data: "activoTexto", title: "Estado" }
            ]
        });

        registrarEventoIrTipoServicio();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}