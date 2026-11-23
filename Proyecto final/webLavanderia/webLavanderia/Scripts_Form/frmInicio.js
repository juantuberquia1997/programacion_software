jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    const nombreEmpleado = sessionStorage.getItem("noUsu");;
    const idUsuario = sessionStorage.getItem("idUsu");
    const idNum = Number(idUsuario);

    if (!nombreEmpleado || !idUsuario || Number.isNaN(idNum) || idNum <= 0) {
        location.href = "../default1.aspx";
        return;
    }

    $("#txtEmpleado").val(nombreEmpleado);
});
