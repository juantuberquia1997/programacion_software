var dir = "http://localhost:5124/api/";

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
    $("#btnImpr").on("click", function () {
        alerta("Imprimir", "En futura construcción");
        //Imprimir();
    });

    inicializarDireccionEmpleado()
});  //Del: jQuery


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

async function inicializarDireccionEmpleado() {
    await llenarComboEmpleado();
    await llenarCombosDpto();
    //registrarCambioDepartamento();
    await llenarTabla();
}

function registrarEventoIrDireccion() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", async function (evento) {
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
        $("#txtDir").val(datosFila.direccion ?? "");

        if (datosFila.idEmpleado !== undefined && datosFila.idEmpleado !== null && datosFila.idEmpleado !== "") {
            $("#cboEmpleado").val(String(datosFila.idEmpleado));
        }

        if (datosFila.idDepartamento !== undefined && datosFila.idDepartamento !== null && datosFila.idDepartamento !== "") {
            $("#cboDepart").val(String(datosFila.idDepartamento));
            await llenarCombosCiudad(datosFila.idCiudad);
        } else if (datosFila.idCiudad !== undefined && datosFila.idCiudad !== null && datosFila.idCiudad !== "") {
            $("#cboCiudad").val(String(datosFila.idCiudad));
        }

        if (datosFila.idEmpleado && datosFila.idEmpleado.toString().trim() !== "") {
            await Consultar();
        }
    });
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

async function llenarComboEmpleado() {
    let url = dir + "empleado/listadoEmpleados";
    let rpta = await llenarComboGral(url, "#cboEmpleado", "idEmpleado", "nombreApellidoEmp");

    if (rpta != "Termino") {
        alerta("Error llenando combo de empleados!", "Error en procedimiento");
        return;
    }
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#cboCiudad")[0].options[0].selected = true;
    $("#cboDepart")[0].options[0].selected = true;
    $("#cboEmpleado")[0].options[0].selected = true;
    $("#txtDir").val("");
    $("#cboEmpleado").trigger("focus");
}

async function Consultar() {

    try {

        let codEmpleado = $("#cboEmpleado").val();
        Cancelar();
        $("#cboEmpleado").val(codEmpleado);

        if (!codEmpleado || isNaN(codEmpleado) || parseFloat(codEmpleado, 10) <= 0 || codEmpleado.trim() == "") {
            // CAMBIO: mensaje ajustado al empleado seleccionado
            alerta("Error, el empleado seleccionado no es válido", "Error en procedimiento");
            $("#cboEmpleado").trigger("focus");
            return;
        }
        let url = dir + "empleado/listadoDirXEmpleado?codEmpleado=" + codEmpleado

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });

        const datosin = await datosOut.json();

        if (datosin == "") {
            alerta("No se encontró dirección", "Informacion del proceso")
            return;
        }

        $("#txtCodigo").val(datosin[0].idDir);
        $("#cboEmpleado").val(datosin[0].empleado);
        $("#txtDir").val(datosin[0].direccion);

        $("#cboDepart").val(datosin[0].idDepart);
        await llenarCombosCiudad(datosin[0].idDir);

        $("#cboCiudad").val(datosin[0].idCiudad);


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    let codi = $("#txtCodigo").val();
    let ciu = $("#cboCiudad").val();
    let depto = $("#cboDepart").val();
    let emple = $("#cboEmpleado").val();
    let direccion = $("#txtDir").val();

    // CAMBIO: rutas diferenciadas para POST y PUT según el método
    let comp = "empleado/modificarDireccion";

    if (accion === "POST") {
        codi = "0";
        $("#txtCodigo").val("0");
        comp = "empleado/agregarDireccion";
    }

    if (!validar(direccion)) {
        return;
    }

    if (!emple || isNaN(emple) || parseInt(emple, 10) <= 0 || emple.trim() === "") {
        alerta("Debe seleccionar un empleado válido", "Error en procedimiento");
        $("#cboEmpleado").trigger("focus");
        return;
    }

    const textoAccion = accion === "POST" ? "agregar" : "modificar";
    const confirma = window.confirm(`¿Estás seguro de ${textoAccion} datos de: ${direccion.trim()}?`);
    if (!confirma) {
        alerta(`Cancelada acción de ${textoAccion}`, "Información");
        return;
    }

    const datosOut = {
        idDireccionEmpleado: parseInt(codi, 10),
        idCiudad: parseInt(ciu, 10),
        empleadoIdEmpleado: parseInt(emple, 10),
        direccionEmpleado: direccion.trim()
    };
    console.log("Enviando a:", dir + comp, "Method:", accion, "datosOut:", datosOut);
    try {
        const response = await fetch(dir + comp, {
            method: accion,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        // CAMBIO: lectura segura por texto para soportar respuestas vacías en PUT
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
            $("#txtCodigo").val(rpta);
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
    let emple = $("#cboEmpleado").val()
    const urlPrincipal = dir + "empleado/listadoDirXEmpleado?codEmpleado=" + emple;

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
                const idEmp = item.empleado ?? item.idEmpleado ?? item.empleadoIdEmpleado ?? "";
                const idDepto = item.idDepart ?? item.idDepartamento ?? "";
                const idCiu = item.idCiudad ?? "";

                const opEmp = $(`#cboEmpleado option[value="${idEmp}"]`);
                const opDepto = $(`#cboDepart option[value="${idDepto}"]`);
                const opCiu = $(`#cboCiudad option[value="${idCiu}"]`);

                const nombreEmpleado = opEmp.length > 0
                    ? opEmp.text().trim()
                    : (item.nombreEmpleado ?? item.nombreApellidoEmp ?? idEmp);

                const nombreDepto = opDepto.length > 0
                    ? opDepto.text().trim()
                    : (item.nombreDepartamento ?? item.departamento ?? idDepto);

                const nombreCiudad = opCiu.length > 0
                    ? opCiu.text().trim()
                    : (item.nombreCiudad ?? item.ciudad ?? idCiu);

                return {
                    codigo: item.idDir ?? item.idDireccionEmpleado ?? item.codigo ?? "",
                    idEmpleado: idEmp,
                    empleado: nombreEmpleado,
                    idDepartamento: idDepto,
                    departamento: nombreDepto,
                    idCiudad: idCiu,
                    ciudad: nombreCiudad,
                    direccion: item.direccion ?? item.direccionEmpleado ?? ""
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
                { data: "empleado", title: "Empleado" },
                { data: "departamento", title: "Departamento" },
                { data: "ciudad", title: "Ciudad" },
                { data: "direccion", title: "Dirección" }
            ]
        });

        registrarEventoIrDireccion();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}