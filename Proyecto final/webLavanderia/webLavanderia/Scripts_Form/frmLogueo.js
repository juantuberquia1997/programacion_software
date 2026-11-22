var dir = "http://localhost:XXXXX/api/";

jQuery(function () {
    //Registrar los botones para responder al evento click
    $("#btnLimpiar").on("click", function () {
        alerta("Limpiar", "En futura construcción");
        //Limpiar();
    });
    $("#btnIngresar").on("click", function () {
        Ingresar();
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

async function Ingresar() {

    let Usu = $("#txtUsuario").val();
    let Cla = $("#txtClave").val();

    let idUsu = "1111";
    let nUsu = "Daniel";
    let ok = true;

    if (ok) {
        if (window.sessionStorage) {
            sessionStorage.setItem("idUsu", idUsu);
            sessionStorage.setItem("noUsu", nUsu);
            location.href("frmInicio.html");
        }
    }

}