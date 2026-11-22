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

    inicializarCiudad()

});   //Del: jQuery

async function inicializarCiudad() {
    await llenarCombosDepartamento();
    await llenarTabla();
}

function registrarEventoIrCiudad() {
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
    });
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

async function llenarCombosDepartamento() {
    let url = dir + "departamento/listadoDepartamentos";
    let rpta = await llenarComboGral(url, "#cboDepart", "idDepartamento", "nombreDepartamento");

    if (rpta != "Termino") {
        alerta("Error llenando combo de departamento!", "Error en procedimiento")
        return;
    }
}

function validar(name) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El nombre no válido,\n"
    }

    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtNombre").val("");
    $("#cboDepart")[0].options[0].selected = true;
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
        let url = dir + "ciudad/consultaCiudad?comando=1&dato=" + nroDoc

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors", url,
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró telefono: " + nroDoc, "Informacion del proceso")
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#txtNombre").val(datosin[0].nombre);
        $("#cboDepart").val(datosin[0].idDep);


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtNombre").val();
    let Dpto = $("#cboDepart").val();

    if (!validar(Nomb)) {
        $("#txtNombre").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('Estas seguro de agregar datos de: ' + Nomb + ' ?');
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
        rpta = window.confirm('Estas seguro de modificar datos de: ' + Nomb + ' ?');
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
        IdCiudad: accion === "POST" ? 0 : (parseInt(Codi, 10) || 0),
        NombreCiudad: Nomb ? Nomb.trim() : "",
        IdDepartamento: parseInt(Dpto, 10)
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "ciudad", {
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
            // await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }

}

async function llenarTabla() {
    const url = dir + "ciudad/consultaCiudad?comando=2&dato=0";

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
                codigo: item.codigo ?? item.idCiudad ?? "",
                nombre: item.nombre ?? item.nombreCiudad ?? ""
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
                { data: "nombre", title: "Ciudad" },
            ]
        });

        registrarEventoIrCiudad();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}