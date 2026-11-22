jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    let nombreUsu = sessionStorage.getItem("noUsu",);
    let idUsu = sessionStorage.getItem("idUsu",);

    if (nombreUsu.trim == "" || idUsu.trim == "" || idUsu <= 0) {
        location.href("../default1.aspx");
    }

    $("#txtEmpleado").text(nombreUsu);
});