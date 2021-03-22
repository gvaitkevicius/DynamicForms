// Uniao de duas ou mais cargas
// Recebe um vetor de ids de cargas
function _linkUnirCargas(Cargas,cargaSelecionada) {
    var path = "/plugandplay/APS/UnirCargas/";
    $.ajax({
        url: path,
        type: 'post',
        data: { CargasIds: Cargas, CargaSelecionada: cargaSelecionada },
    }).done(function (msg) {
        if (msg.st === 'OK') {
            alert('Cargas unidas com sucesso!\n Codigo nova Carga[' + msg.msg + ']');
            _listaUniaoCargas = [];
            TabelaFilaExpedicao.carregarTabela();
        } else
        {
            alert('Erro ao unir cargas!\n [' + msg.msg + ']');;
            _listaUniaoCargas = [];
        }
    }).fail(function (jqXHR, textStatus, msg) {
        alert('Erro ao unir cargas!\n ['+msg.msg+']');;
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
function unirCargasMarcadas(cargaSelecionada) {
    var strIds = "";
    for (var i = 0; i < _listaUniaoCargas.length; i++) {
        strIds += _listaUniaoCargas[i].idCarga;
        strIds += i < _listaUniaoCargas.length - 1 ? ";" : "";
    }
    _linkUnirCargas(strIds, cargaSelecionada);
}

