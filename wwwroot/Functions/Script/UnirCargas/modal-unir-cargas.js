// Uniao de duas ou mais cargas
// Recebe um vetor de ids de cargas
function _linkUnirCargas(Cargas, cargaSelecionada, cidIds = "") {
    var path = "/plugandplay/APS/UnirCargas/";
    $.ajax({
        url: path,
        type: 'post',
        data: { CargasIds: Cargas, CargaSelecionada: cargaSelecionada, CidadesIds: cidIds },
    }).done(function (msg) {
        if (msg.st === 'OK') {
            alert('Cargas unidas com sucesso!\n Codigo nova Carga[' + msg.msg + ']');
            _listaUniaoCargas = [];
            listaCargaCidades = [];
            TabelaFilaExpedicao.carregarTabela();
        } else {
            alert('Erro ao unir cargas!\n [' + msg.msg + ']');;
            //_listaUniaoCargas = [];
            //listaCargaCidades = [];
        }
    }).fail(function (jqXHR, textStatus, msg) {
        alert('Erro ao unir cargas!\n [' + msg.msg + ']');;
    });
}


///SELECIONA UMA OU MAIS CARGAS NA TELA DE EXPEDIÇAO
function checkCarga(ck, carId, placa) {
    var item = { 'idCarga': carId, 'idVeiculo': placa };
    if (ck.checked === true) {
        _listaUniaoCargas.push(item);
    } else {
        for (var i = 0; i < _listaUniaoCargas.length; i++) {
            if (_listaUniaoCargas[i].idCarga === item.idCarga && _listaUniaoCargas[i].idVeiculo === item.idVeiculo) {
                _listaUniaoCargas.splice(i, 1);
            }
        }
    }
}
//REALIZA A UNIAO DAS CARGAS SELECIONADAS
function unirCargasMarcadas(cargaSelecionada = "") {

    
    var strIds = "";
    var cidIds = "";
    if (listaCargaCidades.length == 0) {
        for (var i = 0; i < _listaUniaoCargas.length; i++) {
            strIds += _listaUniaoCargas[i].idCarga;
            strIds += i < _listaUniaoCargas.length - 1 ? "," : "";
        }
         //Quando for unir clicando no botãozinho lá no menu de expedição preciso ter uma carga como base;
        if (cargaSelecionada == "")
            cargaSelecionada = _listaUniaoCargas[0].idCarga;

        //_linkUnirCargas(strIds, cargaSelecionada);
        console.log(strIds);
        console.log(cargaSelecionada);
        window.open('/PlugAndPlay/APS/OtimizarCarga?strCargas=' + strIds + '&carIdPrincipal=' + cargaSelecionada);
    }
    else {
        var i = 0;
        while (i < listaCargaCidades.length - 1) {
            strIds += listaCargaCidades[i].car_id + ";";
            cidIds += listaCargaCidades[i].cidade + ";";

            i++;
        }
        strIds += listaCargaCidades[i].car_id;
        cidIds += listaCargaCidades[i].cidade;

        //Quando for unir clicando no botãozinho lá no menu de expedição preciso ter uma carga como base;
        if (cargaSelecionada == "")
            cargaSelecionada = listaCargaCidades[0].idCarga;

        _linkUnirCargas(strIds, cargaSelecionada, cidIds);
    }

}

