var dir = "http://localhost:XXXXX/api/";

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        alert("Agregar");
        //Agregar();
    });
    $("#btnModi").on("click", function () {
        alert("Modificar");
        //Modificar();
    });
    $("#btnBusc").on("click", function () {
        alert("Buscar");
        //Consultar();
    });
    $("#btnCanc").on("click", function () {
        alert("Cancelar");
        //Cancelar();
    });
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
        //Imprimir();
    });


});  //Del: jQuery


function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}



async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let _vr1 = $("#txt_vr1").val();
    let _vr2 = $("#cbo_vr2").val();
    let _vr3 = $("#txt_vr3").val();
    let _vr4 = $("#chk_vr4").prop("checked");

    alert("vr1: " + _vr1 + "..." + ", vr2: " + _vr2 + "..." + ", vr3: " + _vr3 + "..." + ", vr4: " + _vr4);

    if (_vr1 == undefined || _vr1.trim() == "" || parseInt(_vr1, 10) < 0) {
        alerta("xxx no válido", "Error");
        $("#txtCarnet").trigger("focus");
        return;
    }
    if (parseInt(_vr2, 10) <= 0) {
        alerta("xxx no válido", "Error");
        $("#cboPrograma").trigger("focus");
        return;
    }

    //Crear Json 
    let datosOut = {
        campo1: _vr1,
        campo2: _vr2,
        campo3: _vr3,
        campo4: _vr4
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "nombre servicio", {
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
            $("#txt_vr1").val(rpta);
            //llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}