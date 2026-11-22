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
        let usuario = $("#txtUsuario").val();
        if (!usuario || usuario.trim() === "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un usuario");
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

    inicializarUsuario()
});  //Del: jQuery


function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

async function inicializarUsuario() {
    await llenarComboEmpleado();
    await llenarTabla();
}

function registrarEventoIrUsuario() {
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
        $("#txtUsuario").val(datosFila.usuario ?? "");
        $("#txtClave").val(datosFila.clave ?? "");

        if (datosFila.idEmp !== undefined && datosFila.idEmp !== null && datosFila.idEmp !== "") {
            $("#cboEmpleado").val(String(datosFila.idEmp));
        }

        if (typeof datosFila.activo === "boolean") {
            $("#chkActivo").prop("checked", datosFila.activo);
        }
    });
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
    let codi = $("#txtCodigo").val();
    let idEmp = $("#cboEmpleado").val();
    let usuario = $("#txtUsuario").val();
    let clave = $("#txtClave").val();
    let activo = $("#chkActivo").prop("checked");

    if (!usuario || usuario.trim() === "") {
        alerta("El nombre de usuario no es válido", "Error en procedimiento");
        $("#txtUsuario").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm("¿Estás seguro de agregar el usuario: " + usuario + "?");
        if (!rpta) {
            alerta("Cancelada acción de agregar", "Información");
            return;
        }
    } else if (accion === "PUT") {
        if (!codi || codi.trim() === "") {
            alerta("Selecciona o consulta un registro antes de modificar", "Alerta");
            return;
        }
        rpta = window.confirm("¿Estás seguro de modificar el usuario: " + usuario + "?");
        if (!rpta) {
            alerta("Cancelada acción de modificar", "Información");
            return;
        }
    } else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    //Crear Json 
    let datosOut = {
        idUsuarios: codi && codi.trim() !== "" ? (isNaN(codi) ? codi.trim() : parseInt(codi, 10)) : 0,
        empleadoIdEmpleado: parseInt(idEmp, 10),
        Usuario: usuario.trim(),
        Clave: clave,
        activo: activo
    };
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "usuario", {
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
            $("#txtUsuario").val(rpta);
            await Consultar();
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }

}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#cboEmpleado")[0].options[0].selected = true;
    $("#txtUsuario").val("");
    $("#txtClave").val("");

}

async function Consultar() {
    try {
        let usuario = $("#txtUsuario").val();
        Cancelar();

        $("#txtUsuario").val(usuario);

        if (usuario == undefined || usuario.trim() == "") {
            alerta("Error, el nombre de usuario no es válido", "Error en procedimiento");
            $("#txtUsuario").trigger("focus");
            return;
        }

        let url = dir + "usuario/consultaUsuario?comando=1&dato=" + usuario

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró usuario ", "Informacion del proceso")
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#cboEmpleado").val(datosin[0].idEmp);
        $("#txtUsuario").val(datosin[0].usuario);
        $("#txtClave").val(datosin[0].clave);
        $("#chkActivo").prop("checked", datosin[0].act);

    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function llenarTabla() {
    const urlPrincipal = dir + "usuario/listadoUsuarios";

    try {
        let respuesta = await fetch(urlPrincipal, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        let crudo = respuesta.ok ? await respuesta.json() : null;
        let datos = Array.isArray(crudo)
            ? crudo
            : (crudo?.data ?? crudo?.datos ?? crudo?.resultado ?? null);

        if (!Array.isArray(datos)) {
            const respAlterna = await fetch(urlRespaldo, {
                method: "GET",
                mode: "cors",
                headers: {
                    "content-type": "application/json"
                }
            });
            const crudoAlt = respAlterna.ok ? await respAlterna.json() : [];
            datos = Array.isArray(crudoAlt)
                ? crudoAlt
                : (crudoAlt?.data ?? crudoAlt?.datos ?? crudoAlt?.resultado ?? []);
        }

        const filasFormateadas = Array.isArray(datos)
            ? datos.map(item => {
                const idEmp = item.idEmp ?? item.empleadoIdEmpleado ?? item.idEmpleado ?? "";
                const opcionEmp = $(`#cboEmpleado option[value="${idEmp}"]`);
                const nombreEmp = opcionEmp.length > 0
                    ? opcionEmp.text().trim()
                    : (item.nombreEmpleado ?? item.nombreApellidoEmp ?? idEmp);

                const estadoActivo = Boolean(item.act ?? item.activo ?? true);

                return {
                    codigo: item.codigo ?? item.idUsuarios ?? item.idUsuario ?? "",
                    usuario: item.usuario ?? item.Usuario ?? "",
                    clave: item.clave ?? item.Clave ?? "",
                    idEmp: idEmp,
                    empleado: nombreEmp,
                    activo: estadoActivo,
                    estadoTexto: estadoActivo ? "Activo" : "Inactivo"
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
                { data: "usuario", title: "Usuario" },
                { data: "empleado", title: "Empleado" },
                { data: "estadoTexto", title: "Estado" }
            ]
        });

        registrarEventoIrUsuario();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}
