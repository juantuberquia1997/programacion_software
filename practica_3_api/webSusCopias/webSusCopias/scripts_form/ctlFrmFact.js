var dir = "http://localhost:52245/api/"; //Dirección servidor - Cambiar las XXXXX por el nro. del puerto del servidor API

jQuery(function () {
    pedirValores();
    $("#btnLimpiar").on("click", function () {
        limpiarTotal();
    });
    $("#btnProcesar").on("click", function () {
        Procesar();
    });
});

function fnro(vr) {
    return vr.toLocaleString('es-CO', {
        minimumFractionDigits: 2, // Mínimo de decimales
        maximumFractionDigits: 2  // Máximo de decimales
    });
}

function validarVr(valor, txt) {
    if (isNaN(valor) || parseFloat(valor, 10) <= 0 || valor.trim() == "") {
        alert("Error, valor del tipo: " + txt + ", no es válido");
        return true;
    }
    else {
        return false;
    }
}

function pedirValores() {

    let vrCarta = prompt("¿Cuál es el valor de cada copia: Carta:?", "");
    let vrOfici = prompt("¿Cuál es el valor de cada copia: Oficio:?", "");
    let vrExtOf = prompt("¿Cuál es el valor de cada copia: ExtraOficio:?", "");

    // Verificamos si el usuario ingresó un valor
    if (validarVr(vrCarta, "Carta") || validarVr(vrOfici, "Oficio") || validarVr(vrExtOf, "ExtraOficio")) {
        return pedirValores();
    }
    else {
        $("#txtVrCarta").val(vrCarta);
        $("#txtVrOfic").val(vrOfici);
        $("#txtVrExtraOfic").val(vrExtOf);
    }
}

function limpiarTotal() {
    $("#txtCantCarta").val("");
    $("#txtCantOfic").val("");
    $("#txtCantExtraOfic").val("");
    limpiarRpta();
}

function limpiarRpta() {
    $("#txtSubTotCarta").val("");
    $("#txtSubTotOfic").val("");
    $("#txtSubTotExtOfic").val("");
    $("#txtSubTot").val("");
    $("#txtPorcDsc").val("");
    $("#txtDscto").val("");
    $("#txtIva").val("");
    $("#txtAPagar").val("");
    $("#txtCantCarta").trigger("focus");
}

//La función tiene que ser asíncrona para que pueda funcionar el fetch al invocar el servicio
async function Procesar() { //Definir vbles de entrada - let: vble local, var vble global
    let vrCa = $("#txtVrCarta").val();
    let vrOf = $("#txtVrOfic").val();
    let vrEx = $("#txtVrExtraOfic").val();
    let kCa = $("#txtCantCarta").val();
    let kOf = $("#txtCantOfic").val();
    let kEx = $("#txtCantExtraOfic").val();

    //Validar
    if (vrCa  == undefined || isNaN(vrCa)) {
        alert("Error, valor tipo carta, no válido");
        $("#txtVrCarta").trigger("focus");
        return;
    }

    if (vrOf == undefined || isNaN(vrOf)) {
        alert("Error, valor tipo oficio, no válido");
        $("#txtVrOfic").trigger("focus");
        return;
    }

    if (vrEx == undefined || isNaN(vrEx)) {
        alert("Error, valor tipo extraOficio, no válido");
        $("#txtVrExtOfic").trigger("focus");
        return;
    }

    let datosOut = {
        //Crear la estructura de Json, con los datos de entrada deben coincidir con los nombres de las propiedades del modelo
        vrC: vrCa,
        vrE: vrEx,
        vrO: vrOf,
        kC: kCa,
        kE: kEx,
        kO: kOf
    };


    //Invocar el servicio -  Enviar info y recuperar respuestas
    try {  //Utilizar el método: fetch de JavaScript (Asíncrono) que conecta con la API con path de donde corre el servicio
        const retornoSrv = await fetch(dir + "servSusCopias",   //fetch: realiza(envía) peticiones de red asíncronas hacia un servidor o API
            {
                method: "POST",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(datosOut),
            });  //JSON.stringify: convierte el texto JavaScript, en un texto de formato Json

        //Recuperar y leer la respuesta del servicio
        const Rpta = await retornoSrv.json();    //En la constante Rpta, queda la respuesta del servicio, en formato: JSon
        let error = Rpta.error;
        if (error != "") {
            alert("Error al procesar, " + error);
            return;
        }

        $("#txtSubTotCarta").val(fnro(Rpta.vrToctC));
        $("#txtSubTotOfic").val(fnro(Rpta.vrToTO));
        $("#txtSubTotExtraOfic").val(fnro(Rpta.vrTotE));
        $("#txtSubTot").val(fnro(Rpta.vrSubtot));
        $("#txtPorcDsc").val(fnro(Rpta.porcDsto));
        $("#txtDscto").val(fnro(Rpta.vrDsto));
        $("#txtIva").val(fnro(Rpta.valorIva));
        $("#txtAPagar").val(fnro(Rpta.vrApag));
    }
    catch (e) {
        alert("Error, " + e);
    }
}