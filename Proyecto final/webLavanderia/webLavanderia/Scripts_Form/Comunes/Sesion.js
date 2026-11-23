function cargarEmpleadoEnFormulario(selectorInput = "#txtEmpleado") {
    const nombreEmpleado = (sessionStorage.getItem("noUsu") || "").trim();
    const idUsuario = (sessionStorage.getItem("idUsu") || "").trim();
    const idNum = Number(idUsuario);

    if (!nombreEmpleado || !idUsuario || Number.isNaN(idNum) || idNum <= 0) {
        location.href = "../default1.aspx";
        return null;
    }

    $(selectorInput).val(nombreEmpleado);
    return { idUsuario: idNum, nombreEmpleado };
}