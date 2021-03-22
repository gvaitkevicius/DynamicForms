function avaliarCargaTotal(idCarga) {
    Carregando.abrir();
    $.ajax({
        url: '/PlugAndPlay/APS/AvaliarCargaTotal',
        type: "GET",
        data: { idCarga: idCarga },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Show3dNovaCarga(result);
            }
            else {
                alert('erro !!! ');
            }
        },
        error: function () {
            alert('erro');
        },
        complete: function () {
            Carregando.fechar();
        }
    });

}