var dir = "http://localhost:5124/api/";

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    inicializarDescuentos();
}); //Del: jQuery

function alerta(msj, titulo) {
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

async function inicializarDescuentos() {
    await llenarTabla();
}

async function llenarTabla() {
    const url = dir + "descuento/listarDescuentosTabla";

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datos = await respuesta.json();

        const filasFormateadas = datos.map(item => ({
            codigo: item.codigo ?? item.idDescuento ?? "",
            prenda: item.prenda ?? item.nombrePrenda ?? "",
            cantMin: item.cantMin ?? item.cantidadMin ?? "",
            cantMax: item.cantMax ?? item.cantidadMax ?? "",
            porcen: item.porcen ?? item.porcentaje ?? "",
            activo: item.activo === true || item.activo === 1 || item.activo === "true" || item.activo === "1" ? "Sí" : "No"
        }));

        $("#tblDatos").DataTable({
            data: filasFormateadas,
            destroy: true,
            columns: [
                { data: "codigo", title: "Código" },
                { data: "prenda", title: "Prenda" },
                { data: "cantMin", title: "Cantidad Mínima" },
                { data: "cantMax", title: "Cantidad Máxima" },
                { data: "porcen", title: "Porcentaje (%)" },
                { data: "activo", title: "Activo" }
            ]
        });
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}
