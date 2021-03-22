

function gerarRelatorio(CargaId = "") {
    //alert(CargaId);
    if (CargaId == "") {
        alert("Não foi possível obter o Id da carga");
    }
    else {
        var form = document.createElement("form");

        form.method = "get";
        form.action = "/PlugAndPlay/ReportPlanoCarga/GerarPDF?CargaId=" + CargaId;
        form.setAttribute("target", "ViewIndex");

        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "json");
        hiddenField.setAttribute("name", "CargaId");
        hiddenField.setAttribute("value", CargaId);
        form.setAttribute("enctype", "multipart/form-data");

        form.appendChild(hiddenField);


        document.body.appendChild(form);
        window.open('', 'ViewIndex');

        form.submit();
    }
}