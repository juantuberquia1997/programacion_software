var dir = "http://localhost:5124/api/";

$(document).ready(function () {
    $("#txtUsuario").focus();

    $("#btnLimpiar").on("click", function () {
        Limpiar();
    });

    $("#btnIngresar").on("click", function () {
        Ingresar();
    });

});

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function Limpiar() {
    $("#txtUsuario").val("");
    $("#txtClave").val("");
    $("#txtUsuario").focus();
}

async function Ingresar() {
    const usuario = $("#txtUsuario").val().trim();
    const clave = $("#txtClave").val().trim();

    if (!usuario || !clave) {
        Swal.fire("Atención", "Debe ingresar el usuario y la clave.", "warning");
        return;
    }

    try {
        const url = `${dir}usuario/validarLogin?usuario=${usuario}&clave=${clave}`;
        const response = await fetch(url, {
            method: "GET"
        });

        if (!response.ok) {
            Swal.fire("Acceso denegado", "Usuario o clave incorrectos.", "error");
            return;
        }

        const lista = await response.json();
        const reg = Array.isArray(lista) ? lista[0] : lista;

        if (!reg || !reg.idUsuario) {
            Swal.fire("Acceso denegado", "Usuario o clave incorrectos.", "error");
            return;
        }

        if (reg.activo !== true) {
            Swal.fire("Acceso denegado", "El usuario está inactivo. Contacte al administrador.", "warning");
            return;
        }

        if (window.sessionStorage) {
            sessionStorage.setItem("idUsu", reg.idUsuario);
            sessionStorage.setItem("noUsu", reg.empleado || reg.usuario);
            location.href = "frmInicio.html";
        } else {
            Swal.fire("Acceso denegado", "Usuario o clave incorrectos, o usuario inactivo.", "error");
        }
    } catch (error) {
        console.error("Error al conectar con la API:", error);
        Swal.fire("Error", "No se pudo conectar con la API.", "error");
    }
}