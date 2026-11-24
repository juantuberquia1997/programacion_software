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
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero una ciudad");
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

        const datosFila = oTabla ? oTabla.row(fila).data() : $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) {
            alerta("No se pudieron leer los datos de la fila", "Error en procedimiento");
            return;
        }

        let nombreCiudad = datosFila.nombre ?? datosFila.nombreCiudad ?? "";
        $("#txtNombre").val(nombreCiudad);

        if (nombreCiudad.trim() !== "") {
            Consultar();
        }
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

function validar(name, depto) {
    let msj = "";
    if (!name || name.trim() === "") {
        msj += "El nombre de ciudad no es válido.\n";
    }
    if (!depto || isNaN(parseInt(depto, 10)) || parseInt(depto, 10) <= 0) {
        msj += "Debe seleccionar un departamento válido.\n";
    }
    if (msj !== "") {
        alerta(msj.trim(), "Error en procedimiento");
        return false;
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

        let nombreBuscado = $("#txtNombre").val();
        Cancelar();

        $("#txtNombre").val(nombreBuscado);

        if (nombreBuscado.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento")
            $("#txtNombre").trigger("focus");
            return;
        }
        let url = dir + "ciudad/consultaCiudad?comando=1&dato=" + encodeURIComponent(nombreBuscado);

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors", url,
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró la ciudad: " + nombreBuscado, "Información del proceso");
            return;
        }

        const reg = datosin[0];
        $("#txtCodigo").val(reg.codigo ?? reg.idCiudad ?? "0");
        $("#txtNombre").val(reg.nombre ?? reg.nombreCiudad ?? "");
        const idDepto = reg.idDep ?? reg.idDepartamento;
        if (idDepto !== undefined && idDepto !== null) {
            $("#cboDepart").val(idDepto);
        }


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtNombre").val();
    let Dpto = $("#cboDepart").val();

    if (!validar(Nomb, Dpto)) {
        $("#txtNombre").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm("¿Estás seguro de agregar los datos de: " + Nomb.trim() + "?");
        if (!rpta) {
            alerta("Cancelada acción de agregar", "Información");
            return;
        }
    }
    else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm("¿Estás seguro de modificar los datos de: " + Nomb.trim() + "?");
        if (!rpta) {
            alerta("Cancelada acción de modificar", "Información");
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    //Crear Json 
    let datosOut = {
        IdCiudad: accion === "POST" ? 0 : parseInt(Codi, 10),
        NombreCiudad: Nomb.trim(),
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
            await llenarTabla();
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
                nombre: item.nombre ?? item.nombreCiudad ?? "",
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