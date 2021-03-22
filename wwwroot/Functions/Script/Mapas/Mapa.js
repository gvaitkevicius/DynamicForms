function eventOpenMap2(data_atual, carga, modo){
    $.get('/PlugAndPlay/PontoMapa/PontosEntrega?data=' + data_atual + '&carga=' + carga + '&modo=' + modo).done(function (result) {

    }).fail(function () {
        alert('Erro, contate a administração do software!');
    });
}

function eventOpenMap(data_atual, carga, modo) {
    $.get('/PlugAndPlay/PontoMapa/PontosEntrega?data=' + data_atual + '&carga=' + carga + '&modo=2').done(function (result) {

        $(document).ready(function () {
            // initialize Leaflet
            var map = L.map('exibir_pontosMapa').setView({
                lon: result.PON_LONGITUDE.toString().replace(",", "."), lat: result.PON_LATITUDE.toString().replace(",", ".")
            }, 5);

            // add the OpenStreetMap tiles
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 100,
                attribution: '&copy; <a href="https://openstreetmap.org/copyright">OpenStreetMap contributors</a>'
            }).addTo(map);
            // show the scale bar on the lower left corner
            L.control.scale().addTo(map);

            // show a marker on the map
            L.marker({ lon: result.PON_LONGITUDE.toString().replace(",", "."), lat: result.PON_LATITUDE.toString().replace(",", ".") }).bindPopup('Origem:result.PON_DESCRICAO.toString()').addTo(map);
            $.ajax({
                url: '/PlugAndPlay/PontoMapa/ObterPontosEntrega',
                data:
                {
                    dataAtual: result.CAR_EMBARQUE_ALVO.toString().trim(),
                    idCarga: result.CAR_ID.toString().trim(),
                    modo: result.Modo.toString().trim(),
                },
                method: 'GET',
                success: function (result) {
                    for (var i = 0; i < result.Db_pontosEntrega.length; i++) {
                        var d = result.Db_pontosEntrega[i];
                        L.marker({ lon: d.PON_LONGITUDE, lat: d.PON_LATITUDE }).bindPopup(d.PON_DESCRICAO + '- Carga: ' + d.CAR_ID).addTo(map);
                    }
                },
                error: function (msg) {
                    console.log(msg);
                }
            });
            //$('#exibir_pontosMapa').html(str_html);
            $('#modalOpenMapa').modal('show');

        });

        
    }).fail(function () {
        alert('Erro, contate a administração do software!');
    });

}
//onclick="location.href=\'/PlugAndPlay/PontoMapa/PontosEntrega?data=' + dataAtual + '&carga=' + f.CAR_ID + '&modo=2 \'" />';
/*
exibir_pontosMapa.on('click',, function (e) {
    $('#exibir_pontosMapa').modal('show');
})
*/