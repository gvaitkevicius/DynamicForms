var cargaPrincipal = [];
var listaCargas = [];
var cbCidadesMarcados = [];
var cargasSimuladas = [];
var listaCidadesAtual = [];
var flagCidade = -1;
var listaOrdersExclusao = []; //É uma lista de CarIds que serão desconsiderados na hora de exibir o caminhão
var listaVeiculos = [];
var indexTipVeiculo = -1;

function carregarTelaOtimizarCarga() {
    var carIdPrincipal = $('#cargaPrincipal').attr('class');
    var carIdLista = $('#listaCargas').attr('class');

    var cargas = obterCargasAPS(carIdPrincipal, carIdLista);
    cargaPrincipal = cargas.cargaPrincipal;
    listaCargas = cargas.listaCargas;

    pesquisarTipVeiculos();
    gerarTextoCabecalho();

    var temp = cargaPrincipal.concat(listaCargas);
    flagCidade = -1;
    gerarCidadesAtual(temp);
    gerarTbodyItensPedidos(temp);
    preencherTabelaCaminhao(temp);

    pesquisarPedidosFuturos();
    Show3d(cargaPrincipal[0].CAR_ID);



}

// <br><i class="fa fa-check" style="color:#f44336;"></i>
function gerarTextoCabecalho() {
    var listaIds = [];
    var str = '<br><i class="fa fa-check" style="color:#f44336;"></i>Carga Principal: ' + cargaPrincipal[0].CAR_ID;


    if (listaCargas.length > 0) {

        for (var i = 0; i < listaCargas.length; i++) {
            if (!listaIds.includes(listaCargas[i].CAR_ID)) {
                listaIds.push(listaCargas[i].CAR_ID);
            }
        }
        str += '  Cargas secundárias: ';
        var j = 0;
        while (j < listaIds.length - 1) {
            str += listaIds[j++] + ', ';
        }
        str += listaIds[j] + '.';

    }
    $('#cabecalho_' + cargaPrincipal[0].CAR_ID).html(str);

}


function obterCargasAPS(carIdPrincipal, carIdLista) {
    var resultJson = [];
    $.ajax({
        url: UrlBase + "/obterCargasAPS",
        data: { strCargas: carIdLista, carIdPrincipal: carIdPrincipal },
        type: "post",
        dataType: "json",
        async: false,
        success: function (result) {
            resultJson = result;
            return resultJson;
        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {

        }
    });

    return resultJson;
}

function preencherTabelaCaminhao(cargas) {
    var ocupacao = 0;
    var volumeTotal = 0;
    var volumeDisponivel = 0;
    var inicioCarregamento = '2222-12-30T00:00';
    var fimCarregamento = '1900-01-01T:00:00';
    var fimProducao = '1900-01-01T:00';

    var preenchimentoCaminhao = "";
    var tamanhoTotal = 0;
    var flagCor = 0;

    var tipId = cargaPrincipal[0].TIP_ID;
    var capacidadeM3 = cargaPrincipal[0].TIP_CAPACIDADE_M3
    if (indexTipVeiculo != -1) {
        tipId = listaVeiculos[indexTipVeiculo].TIP_ID;
        capacidadeM3 = listaVeiculos[indexTipVeiculo].TIP_CAPACIDADE_M3;
    }

    var inputCidades = '';
    var listaCidades = [];
    var trClientes = '';
    var listaClientes = [];

    for (var i = 0; i < cargas.length; i++) {
        ocupacao += cargas[i].M3_PLANEJADO;
        inicioCarregamento = inicioCarregamento > cargas[i].CAR_DATA_INICIO_PREVISTO ? cargas[i].CAR_DATA_INICIO_PREVISTO : inicioCarregamento; //Vai pegar a menor data dentre todas de início de carregamento previsto. 
        fimCarregamento = fimCarregamento < cargas[i].CAR_DATA_FIM_PREVISTO ? cargas[i].CAR_DATA_FIM_PREVISTO : fimCarregamento; //Vai pegar a maior data dentre todas de fim do carregamento previsto.
        fimProducao = fimProducao < cargas[i].FIMPREVISTO ? cargas[i].FIMPREVISTO : fimProducao; //Vai pegar a maior data dentre todas de fim de produção previsto.

        var tamanho = cargas[i].M3_PLANEJADO * 100 / capacidadeM3; //Calcula em porcentagem da ocupação dos itens para pintar proporcionalmente no caminhão.
        var corItem = pegarCorItem(flagCor, cargas[i].COR_OTIF); //Pega a cor atual do caminhão.

        preenchimentoCaminhao += '<td width="' + tamanho.toFixed(2) + '%" class="' + corItem + '"></td>'; //Código HTML que exibe a cor do itenCarga do caminhão.
        tamanhoTotal += tamanho; //Somatório para ver a porcentagem total de ocupação de todos os itens do caminhão, a partir disso vai calcular o espaço vazio para pintar de preto.

        flagCor = flagCor == 1 ? 0 : 1; //Serve para pintar a caminhãozinho. O motivo da flag é que o caminhão fica assim: Cor forte, cor fraca, cor forte, cor fraca...;

        //Parte da tabela de cidades no canto direito da tela
        if (!listaClientes.includes(cargas[i].CLI_ID)) {
            trClientes += '<tr>';
            trClientes += '<td><p style="font-size:11px;line-height:15px;">' + cargas[i].CLI_NOME + '</p></td>';
            trClientes += '<td><p style="font-size:11px;line-height:15px;">' + formatarData(cargas[i].ORD_DATA_ENTREGA_DE) + '</p></td>';
            trClientes += '</tr>';
            listaClientes.push(cargas[i].CLI_ID);
        }
    }

    //Parte do combobox de cidades
    for (var i = 0; i < listaCidadesAtual.length; i++) {
        inputCidades += '<input onclick="marcarCidades(' + listaCidadesAtual[i].PON_ID + ')" id="cbCidadePonId_' + listaCidadesAtual[i].PON_ID + '" class="cbCidade" type="checkbox">' + listaCidadesAtual[i].PON_DESCRICAO;
    }

    if (tamanhoTotal < capacidadeM3) {
        var preenchimentofinal = 100 - tamanhoTotal;
        preenchimentoCaminhao += '<td width="' + preenchimentofinal.toFixed(2) + '%" style="background-color:#000"></td>';
    }

    volumeTotal = ocupacao;
    
    volumeDisponivel = capacidadeM3 - volumeTotal;
    ocupacao = ocupacao * 100 / capacidadeM3;

    $('#ocup').text(ocupacao.toFixed(2) + "%");
    $('#volCar').text(volumeTotal.toFixed(2));
    $('#volLiv').text(volumeDisponivel.toFixed(2));
    $('#dataFimprev').text(padronizarData(fimProducao));

    $('#iniFimPrev').text('INÍCIO: ' + padronizarData(inicioCarregamento) + '  FIM:' + padronizarData(fimCarregamento));
    $('#tipIdOriAtual').text('TIPO VEÍCULO: ORIGINAL: ' + cargas[0].TIP_ID + '  ATUAL: ' + tipId);
    $('#tabelaCaminhao').html(preenchimentoCaminhao);
    $('#bodyCidades_' + cargaPrincipal[0].CAR_ID).html(trClientes);
    $('#cbCidades_' + cargaPrincipal[0].CAR_ID).html(inputCidades);

    selecionarCbCidadesMarcados();
    carregarMapa();
}

function selecionarCbCidadesMarcados() {

    for (var i = 0; i < cbCidadesMarcados.length; i++) {
        $('#' + cbCidadesMarcados[i]).prop('checked', true);
    }
}


function marcarCidades() {
    var listaIds = [];
    cbCidadesMarcados = [];

    $(".cbCidade:checked").each(function () {
        var temp = $(this).attr("id");
        listaIds.push(temp.split('_')[1]);
        cbCidadesMarcados.push($(this).attr("id"));
    });

    var temp = flagCidade == -1 ? cargaPrincipal.concat(listaCargas) : cargasSimuladas[flagCidade];


    gerarCidadesAtual(temp);
    var cargasAux = [];
    if (listaIds.length > 0) {
        for (var i = 0; i < temp.length; i++) {
            if (listaIds.includes(temp[i].PON_ID)) {
                cargasAux.push(temp[i]);
            }
        }
    } else {
        cargasAux = temp;
    }
    //cbCidadesMarcados
    preencherTabelaCaminhao(cargasAux);

}

function gerarTbodyItensPedidos(cargas) {
    var cliente_antigo = '';
    var carId_antigo = '';
    var str = "";

    for (var i = 0; i < cargas.length; i++) {
        if (cargas[i].CLI_ID != cliente_antigo || cargas[i].CAR_ID != carId_antigo) {
            cliente_antigo = cargas[i].CLI_ID;
            carId_antigo = cargas[i].CAR_ID;

            str += '<tr height="8" style="white-space:nowrap;">';
            str += '<td class="cab_carga" style="text-align:left" colspan="23" >CARGA ATUAL: {0}. DOCA: {8}. TIPO DE CAMINHÃO: {1}. OCUPAÇÃO {2}%. DATA INICIO PREVISTO: {3}. DATA FIM PREVISTO: {4}. CARGA STATUS: {5} PLACA: {6}. TRANSPORTADORA: {7}</td>'
                .replace('{0}', cargas[i].CAR_ID)
                .replace('{1}', cargas[i].TIP_ID)
                .replace('{2}', cargas[i].OCUPACAO)
                .replace('{3}', formatarData(cargas[i].CAR_DATA_INICIO_PREVISTO))
                .replace('{4}', formatarData(cargas[i].CAR_DATA_FIM_PREVISTO))
                .replace('{5}', getStatus(cargas[i].CAR_STATUS))
                .replace('{6}', cargas[i].VEI_PLACA)
                .replace('{7}', cargas[i].TRA_NOME)
                .replace('{8}', cargas[i].CAR_ID_DOCA)
                ;
            str += '</tr>';

            str += '<tr height="8" style="white-space:nowrap;">';
            str += '<td onclick="cliqueClientePlanilha(\'' + cargas[i].CLI_ID + '\')" style="text-align:left"  colspan="23"> EMBARQUE: ' + formatarData(cargas[i].DTEMBARQUE) + '. CLIENTE: ' + cargas[i].CLI_NOME + '. ' + cargas[i].MUN + ', ' + cargas[i].UF + ', DATA ENTREGA: {99}</td>';
            str += '</tr>';
        }

        var cor_otif = getCorOTIF(cargas[i].COR_OTIF);
        var cor_fila = getCorFila(cargas[i].FPR_COR_FILA);

        var aux_id = 'ant_' + cargas[i].CAR_ID.replaceAll('/', '_').replaceAll('.', '_') + '_' + cargas[i].ORD_ID.replaceAll('/', '_').replaceAll('.', '_');
        str += '<tr height="8" style="white-space:nowrap;" id="' + aux_id + '">';
        //Dados editáveis
        str += '<td><span><button type="button" class="btn btn-primary btn-rounded btn-sm my-0" onclick="eventoAlterar(\'' + cargas[i].CAR_ID + '\', \'' + cargas[i].ORD_ID + '\', \'' + cargas[i].DTEMBARQUE + '\')">Alterar</button ></span ></td>'; //Botão salvar
        str += '<td class="td_exped new_carga" contenteditable="true" style="border: 1px solid #999;" onclick="cliqueColarClipBoard(\'' + cargas[i].CAR_ID + '\', \'' + cargas[i].ORD_ID + '\')">' + '' + '</td>'; //Nova carga
        str += '<td class="td_exped new_itc_qtd_plan" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova qtd planejada
        str += '<td class="td_exped new_ord_entrega" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova ordem entrega
        str += '<td class="td_exped new_tipo_caminhao" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //novo tipo caminhão

        //Dados fixos
        str += '<td style="color:#000; background-color:' + cor_fila + '" class="old_carga" id="old_carga_' + cargas[i].CAR_ID + '" onclick=cliqueClipBoard(\'' + cargas[i].CAR_ID + '\')>' + cargas[i].CAR_ID + '</td>'; //Carga atual
        str += '<td class="old_itc_qtd_plan">' + cargas[i].ITC_QTD_PLANEJADA + '</td>'; //Quantidade planejada atual
        str += '<td class="old_ord_entrega">' + cargas[i].ITC_ORDEM_ENTREGA + '</td>'; //Ordem de entrega
        str += '<td style="display:none;" class="old_tipo_caminhao">' + cargas[i].TIP_ID + '</td>'; //Tipo caminhão
        str += '<td style="color:#000; background-color:' + cor_otif + '">' + cargas[i].ORD_ID + '</td>'; //Número do pedido
        str += '<td>' + cargas[i].ORD_QUANTIDADE + '</td>'; //Produto
        str += '<td>' + cargas[i].PRO_ID + '</td>'; //Produto
        str += '<td>' + cargas[i].PRO_DESCRICAO + '</td>'; //Pro descrição
        str += '<td>' + cargas[i].SALDO_ESTOQUE + '</td>'; //Saldo estoque
        var peso_total = cargas[i].ORD_QUANTIDADE * cargas[i].ORD_PESO_UNITARIO;
        str += '<td>' + peso_total + '</td>'; //Peso total
        str += '<td>' + cargas[i].M3_PLANEJADO + '</td>'; //M³
        str += '<td>' + cargas[i].PECENT_ESTOQUE_PRONTO + '</td>'; //Percentual estoque pronto
        str += '<td>' + cargas[i].PRO_LARGURA_EMBALADA + ' x ' + cargas[i].PRO_ALTURA_EMBALADA + ' x ' + cargas[i].PRO_COMPRIMENTO_EMBALADA + '</td>'; //Dimensões do pallet
        str += '<td>' + formatarData(cargas[i].ORD_DATA_ENTREGA_DE) + '</td>'; //Data de entrega
        str += '<td>' + formatarDataHora(cargas[i].FIMPREVISTO) + '</td>'; //Fim previsto produção
        str += '<td>' + cargas[i].CAR_ID_JUNTADA + '</td>'; //Carga id juntada


        str += '</tr>'
    }
    $('#tbodyItePedidos').html(str);
}

function eventoAlterar(carId, ordId, dataOrd) {

    if (eventoAlterarItemCarga(carId, ordId, dataOrd)) {
        var i = 0;
        var encontrou = false;
        var tr_id = '#ant_' + carId.replaceAll('/', '_').replaceAll('.', '_') + '_' + ordId.replaceAll('/', '_').replaceAll('.', '_');
        var new_carga = $(tr_id + ' .new_carga').text() != '' ? $(tr_id + ' .new_carga').text() : $(tr_id + ' .old_carga').text();


        var temp = cargaPrincipal.concat(listaCargas);
        while (i < temp.length) {
            if (temp[i].CAR_ID == new_carga)
                encontrou = true;
            i++;
        }

        //Achou ou não achou na lista de cargas
        if (!encontrou) { //Não achou, então vai remover da tela dos caminhõezinhos
            if (!listaOrdersExclusao.includes(ordId))
                listaOrdersExclusao.push(ordId);
        }
        else { //Achou, então vai remover da lista de CarIdsExclusao
            //listaCarIdsExclusao
            var j = 0;
            while (j < listaOrdersExclusao.length && listaOrdersExclusao[j] != ordId)
                j++;

            if (j < listaOrdersExclusao.length) {
                listaOrdersExclusao.splice(j, 1);
            }
        }

        //Só vai pegar as cargas que tem que exibir, desconsiderando todas as cargas que estão na lista de excluir.
        var temp2 = [];
        for (var i = 0; i < temp.length; i++) {
            if (!listaOrdersExclusao.includes(temp[i].ORD_ID)) {
                temp2.push(temp[i]);
            }
        }

        preencherTabelaCaminhao(temp2);

    }
}

//Entrada: 2020-01-21T16:00:00
function padronizarData(data) {
    var temp = data.split('T');

    var hora = temp[1].split(':');
    data = temp[0].split('-');

    return data[2] + '/' + data[1] + '/' + data[0] + ' ' + hora[0] + ':' + hora[1];
}

function pegarCorItem(flag, corOTIF) {
    if (corOTIF == "VERDE")
        cor = flag == 1 ? "verde1" : "verde2";
    if (corOTIF == "VERMELHO")
        cor = flag == 1 ? "vermelho1" : "vermelho2";
    if (corOTIF == "AMARELO")
        cor = flag == 1 ? "amarelo1" : "amarelo2";
    if (corOTIF == "AZUL")
        cor = flag == 1 ? "azul1" : "azul2";

    return cor;
}


function getStatus(num) {
    var status = "nulo";
    switch (num) {
        case -1:
            status = "Deletada";
            break;
        case 1:
            status = "Aberta";
            break;
        case 1.1:
            status = "Reotimizar";
            break;
        case 1.2:
            status = "Antecipar alguém";
            break;
        case 2:
            status = "Aprovada";
            break;
        case 3:
            status = "Agenciada";
            break;
        case 4:
            status = "Piking";
            break;
        case 5:
            status = "Carregando";
            break;
        case 6:
            status = "Despachada (Consolidada)";
            break;
        case 7:
            status = "Faturada";
            break;
        case 8:
            status = "Etregue Parcial";
            break;
        case 9:
            status = "Entregue";
            break;
        case 99:
            status = "Estornada";
            break;
    }

    return status;
}

function getCorOTIF(cor) {
    if (cor == 'VERDE')
        return '#6acd6d'; //'#4CAF50';
    else if (cor == 'VERMELHO')
        return '#ff6254'; //'#f44336';
    else if (cor == 'AZUL')
        return '#5d6fc9'; //'#3F51B5';

    else
        return '#FFFFFF';
}

function getCorFila(cor) {
    /* #4CAF50, #f44336 #3F51B5k */
    return cor == '#4CAF50' ? '#6acd6d' : //Verde
        cor == '#f44336' ? '#ff6254' : //Vermelho
            cor == '#3F51B5k' ? '#5d6fc9' : //Azul
                '#FFF';
}


function pesquisarPedidosFuturos() {
    var inputCargas = $('#inputCarga').val().toUpperCase();
    var inputRaio = $('#inputRaio').val().toUpperCase();
    var inputUfs = $('#inputUf').val().toUpperCase();
    var inputMunicipios = $('#inputMunicipio').val().toUpperCase();
    var inputClientes = $('#inputCliente').val().toUpperCase();
    var inputDiasAntecipacao = $('#inputDiasAntecipacao').val().toUpperCase();

    inputCargas = inputCargas.replace(/\s/g, '');
    inputRaio = inputRaio.replace(/\s/g, '');
    inputUfs = inputUfs.replace(/\s/g, '');

    var pedidosFuturos = obterPedidosFuturosAPS(inputCargas, inputRaio, inputUfs, inputMunicipios, inputClientes, inputDiasAntecipacao);
    if (gerarTabelaPedidosFuturos(pedidosFuturos.listaPedFut)) {
        gerarBotoesPedidosFuturos();
    }
}

function obterPedidosFuturosAPS(cargas = '', raio = '', ufs = '', municipios = '', clientes = '', diasAntecipacao = '') {
    var resultJson = [];
    //Latitude e longitude dos pontos de origem
    var classes = $('.pontoMapaClass').attr('class').split(' ');
    var title = classes[2].split('_')[1];
    var latitude = parseFloat(classes[3].split('_')[1].replace(',', '.'));
    var longitude = parseFloat(classes[4].split('_')[1].replace(',', '.'));

    $.ajax({
        url: UrlBase + "/obterPedidosFuturosAPS",
        data: {
            cargas: cargas,
            raio: raio,
            ufs: ufs,
            municipios: municipios,
            clientes: clientes,
            diasAntecipacao: diasAntecipacao,
            latitude: latitude,
            longitude: longitude
        },
        type: "post",
        dataType: "json",
        async: false,
        success: function (result) {
            resultJson = result;
            return resultJson;
        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {

        }
    });

    return resultJson;
}


function gerarTabelaPedidosFuturos(cargas) {
    var cliente_antigo = '';
    var carId_antigo = '';
    var str = "";
    if (cargas.length) {

        //Thead (Cabeçalho)
        str += '<thead>';
        str += '<tr>';
        str += '<th>SELECIONAR</th>';
        str += '<th>CARGA ATUAL</th>';
        str += '<th data-toggle="tooltip" title="QUANTIDADE PLANEJADA ATUAL">QTD. PLAN.</th>';
        str += '<th data-toggle="tooltip" title="ORDEM ENTREGA ATUAL">ORD. ENTREGA</th>';
        str += '<th data-toggle="tooltip" title="NÚMERO DO PEDIDO">N°. PEDIDO</th>';
        str += '<th data-toggle="tooltip" title="PEDIDO QUANTIDADE">PEDIDO QTD.</th>';
        str += '<th>PRODUTO ID</th>';
        str += '<th>PRODUTO DESCRIÇÃO</th>';
        str += '<th>SALDO ESTOQUE</th>';
        //str += '<th>PESO TOTAL PEDIDO</th>';
        //str += '<th>M³</th>';
        str += '<th data-toggle="tooltip" title="PERCENTUAL ESTOQUE PRONTO">% ESTOQUE PRONTO</th>';
        str += '<th data-toggle="tooltip" title="DIMENSÕES DO PALLET (LARGURA x ALTURA x COMPRIMENTO)">DIMENSÕES DO PALLET (LxAxC)</th>';
        str += '<th data-toggle="tooltip" title="DATA DE ENTREGA">DATA DE ENTREGA</th>';
        //str += '<th data-toggle="tooltip" title="FIM PREVISTO PRODUÇÃO">FIM PREV. PROD.</th>';
        str += '</tr>';
        str += '</thead>';

        str += '<body>';
        for (var i = 0; i < cargas.length; i++) {

            if (cargas[i].ClienteId != cliente_antigo || cargas[i].CAR_ID != carId_antigo) {
                cliente_antigo = cargas[i].ClienteId;
                carId_antigo = cargas[i].CAR_ID;

                str += '<tr height="8" style="white-space:nowrap;">';
                str += '<td class="cab_carga" style="text-align:left" colspan="23" >CARGA ATUAL: {0}. TIPO DE CAMINHÃO: {1}. DATA INICIO PREVISTO: {3}. DATA FIM PREVISTO: {4}. CARGA STATUS: {5}.</td>'
                    .replace('{0}', cargas[i].CAR_ID)
                    .replace('{1}', cargas[i].TIP_ID)
                    .replace('{3}', formatarDataHora(cargas[i].InicioJanelaEmbarque))
                    .replace('{4}', formatarDataHora(cargas[i].FimJanelaEmbarque))
                    .replace('{5}', getStatus(cargas[i].CAR_STATUS))
                    ;
                str += '</tr>';

                str += '<tr height="8" style="white-space:nowrap;">';
                str += '<td onclick="cliqueClientePlanilha(\'' + cargas[i].ClienteId + '\')" style="text-align:left"  colspan="23"> EMBARQUE: ' + formatarDataHora(cargas[i].CAR_EMBARQUE_ALVO) + '. CLIENTE: ' + cargas[i].CLI_NOME + '. ' + cargas[i].MUN_NOME + ', ' + cargas[i].UF_COD + ', DATA ENTREGA: {99}</td>';
                str += '</tr>';
            }

            var cor_otif = '#FFF'; //getCorOTIF(cargas[i].COR_OTIF); --don't have
            var cor_fila = '#FFF'; //getCorFila(cargas[i].FPR_COR_FILA); --don't have

            var aux_id = 'ant_' + cargas[i].CAR_ID.replaceAll('/', '_').replaceAll('.', '_') + '_' + cargas[i].Id.replaceAll('/', '_').replaceAll('.', '_');
            str += '<tr height="8" style="white-space:nowrap;" id="' + aux_id + '">';

            //Dados fixos
            str += '<td><input type="checkbox" class="cbPedFut" id="pedFut_' + cargas[i].CAR_ID + '_' + cargas[i].Id + '"></td>';
            str += '<td style="color:#000; background-color:' + cor_fila + '")>' + cargas[i].CAR_ID + '</td>'; //Carga atual
            str += '<td>' + cargas[i].ITC_QTD_PLANEJADA + '</td>'; //Quantidade planejada atual
            str += '<td>nulo</td>'; //Ordem de entrega //cargas[i].ITC_ORDEM_ENTREGA --don't have
            str += '<td style="display:none;">' + cargas[i].TIP_ID + '</td>'; //Tipo caminhão
            str += '<td style="color:#000; background-color:' + cor_otif + '">' + cargas[i].Id + '</td>';
            str += '<td>' + cargas[i].Quantidade + '</td>'; //Produto //cargas[i].ORD_QUANTIDADE
            str += '<td>' + cargas[i].ProdutoId + '</td>'; //Produto
            str += '<td>' + cargas[i].PRO_DESCRICAO + '</td>'; //Pro descrição //cargas[i].PRO_DESCRICAO --don't have
            str += '<td>' + cargas[i].SALDO_ESTOQUE + '</td>'; //Saldo estoque 
            var peso_total = 0 * cargas[i].ORD_PESO_UNITARIO; //cargas[i].ORD_QUANTIDADE --don't have
            //str += '<td>' + peso_total + '</td>'; //Peso total
            //str += '<td>nulo</td>'; //M³ // cargas[i].M3_PLANEJADO --don't have
            str += '<td>' + cargas[i].PECENT_ESTOQUE_PRONTO + '</td>'; //Percentual estoque pronto
            str += '<td>' + cargas[i].PRO_LARGURA_EMBALADA + ' x ' + cargas[i].PRO_ALTURA_EMBALADA + ' x ' + cargas[i].PRO_COMPRIMENTO_EMBALADA + '</td>'; //Dimensões do pallet
            str += '<td>' + formatarData(cargas[i].DataEntregaDe) + '</td>'; //Data de entrega
            //str += '<td>nulo</td>'; //Fim previsto produção //formatarDataHora(cargas[i].FIMPREVISTO) --don't have


            str += '</tr>'
        }
        str += '</body>';

        $('#table_pedFut').html(str);
    }

    if (str.length > 0)
        return true;
    return false;
}

function gerarBotoesPedidosFuturos() {
    var str = '';
    str += '<div class=" col-sm-1 col-md-1 col-xs-6 padding">';
    str += '<p class="col-sm-12 col-md-12" for="" style="visibility:hidden;">_</p>';
    str += '<span class="input-group-addon" id="btnOtimizar" onclick="otimizarPedidosFuturos()">';
    str += '<button class="fa fa-bolt" style="background:transparent;border:none"></button>';
    str += '</span>';
    str += '</div>';


    $('#botoesPedidosFuturos').html(str);
}

function otimizarPedidosFuturos() {
    var resultJson = [];
    var listaIds = [];
    var cargaIdPrincipal = cargaPrincipal[0].CAR_ID;

    //Adiciona todos os itens da cargaPrincipal e também das cargas secundáras.
    var temp = cargaPrincipal.concat(listaCargas);
    for (var i = 0; i < temp.length; i++) {
        var str = 'pedFut_' + temp[i].CAR_ID + '_' + temp[i].ORD_ID;
        listaIds.push(str);
    }

    $(".cbPedFut:checked").each(function () {
        listaIds.push($(this).attr("id"));
    });
    //Se a lista de checkbox estiver vazia, então vai otimizar todos

    Carregando.abrir('Carregando');
    $.ajax({
        url: UrlBase + "/OtimizarPedidosFuturos",
        data: {
            cargaPrincipal: cargaIdPrincipal,
            listaIds: listaIds
        },
        type: "post",
        dataType: "json",
        success: function (result) {
            resultJson = result;
            carregarTelaCargasSimuladas(result);
            //return resultJson;
        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {
            Carregando.fechar();
        }
    });

    //return resultJson;
}

function carregarTelaCargasSimuladas(cargasSimuladasResult) {
    cargasSimuladas = cargasSimuladasResult.resultCargasWeb;
    var tempCargas = cargasSimuladasResult.resultCargasWeb;
    var flagCor = 1;

    var maiorQuantidade = 0;
    for (let x = 0; x < tempCargas.length; x++) {
        if (tempCargas[x].length > maiorQuantidade)
            maiorQuantidade = tempCargas[x].length;
    }

    var str = '<table class="table table-bordered table-striped text-center ">';
    str += '<thead>';
    str += '<tr>';
    str += '<td>SELECIONAR</td>';
    str += '<td>CARGA</td>';
    for (var i = 0; i < maiorQuantidade; i++) {
        str += '<td>ITEM CARGA</td>';
    }
    str += '</tr>';
    str += '</thead>';


    str += '<tbody>';
    str += '<tr style="background-color:#DDDDDD" onmouseover="mouseSelecionado(this, \'#BBBBBB\')" onmouseout="mouseDeselecionado(this, \'#DDDDDD\')"  id="cargaPrincipal_' + cargaPrincipal[0].CAR_ID + '" onclick="chamarCaminhao(-1);">';
    str += '<td> - </td>';
    str += '<td>' + cargaPrincipal[0].CAR_ID + '(original)</td>';
    var temp = cargaPrincipal.concat(listaCargas);
    for (var i = 0; i < maiorQuantidade; i++) {
        if (i < temp.length) {
            str += '<td>' + temp[i].ORD_ID + '</td>';
        }
    }
    str += '</tr > ';

    var cor = '#f1f1f1';
    var corMouseOver = '#c4c4c4';
    for (var i = 0; i < tempCargas.length; i++) {
        str += '<tr id="cargaSimuladaIndex_"' + i + '  onclick="chamarCaminhao(' + i + ');" onmouseover="mouseSelecionado(this, \'' + corMouseOver + '\')" onmouseout="mouseDeselecionado(this, \'' + cor + '\')">';

        str += '<td><input class="cbCarSim" id="cargaSimulada_' + tempCargas[i][0].CAR_ID + '" type="checkbox"></td>';
        str += '<td>' + tempCargas[i][0].CAR_ID + '</td>';
        for (var j = 0; j < maiorQuantidade; j++) {
            if (j < tempCargas[i].length) {
                var corFundo = cor;
                if (tempCargas[i][j].PlayAction == "OK")
                    corFundo = '#87e880';
                else
                    corFundo = '#eb7f7f';


                str += '<td style="background-color:' + corFundo + ';">' + tempCargas[i][j].ORD_ID + '</td>';
            }
            else {
                str += '<td>-</td>';
            }
        }
        str += '</tr>';


        if (flagCor == 1) {
            cor = '#ffffff';
            corMouseOver = '#c4c4c4';
            flagCor = 0;
        }
        else {
            cor = '#f1f1f1';
            corMouseOver = '#c4c4c4';
            flagCor = 1;
        }
    }
    str += '</tbody>';
    str += '</table>';

    str += '<div id="botoesCargasSimuladas">';
    str += '<div class=" col-sm-1 col-md-1 col-xs-6 padding">';
    str += '<p class="col-sm-12 col-md-12" for="" style="visibility:hidden;">_</p>';
    str += '<span class="input-group-addon" id="btnOtimizar" onclick="gravarCargasSimuladas()">';
    str += '<button class="fa fa-bolt" style="background:transparent;border:none"></button>';
    str += '</span>';
    str += '</div>';
    str += '</div>';

    $('#tableCarSimuladas').html(str);
}

function mouseSelecionado(x, cor) {
    x.style.backgroundColor = cor;
}

function mouseDeselecionado(x, cor) {
    x.style.backgroundColor = cor;
}

function chamarCaminhao(index) {
    var temp = [];
    cbCidadesMarcados = [];
    if (index == -1) {
        temp = cargaPrincipal.concat(listaCargas);
        flagCidade = -1;
    }
    else {
        temp = cargasSimuladas[index];
        flagCidade = index;
    }
    gerarCidadesAtual(temp);
    preencherTabelaCaminhao(temp);
}

function gerarCidadesAtual(cargas) {
    var temp = [];
    listaCidadesAtual = [];

    for (var i = 0; i < cargas.length; i++) {
        if (!temp.includes(cargas[i].PON_ID)) {
            temp.push(cargas[i].PON_ID);
            var objeto = {
                PON_ID: cargas[i].PON_ID,
                PON_DESCRICAO: cargas[i].PON_DESCRICAO
            }
            listaCidadesAtual.push(objeto);
        }
    }
}

var map = null;
function carregarMapa() {
    var strHtml = '<div class="map" id="pontosMapa" style="height:600px"></div>';
    $('.pontoMapaClass div').html(strHtml);

    var listaCidades = '';
    for (var i = 0; i < listaCidadesAtual.length; i++) {
        listaCidades += listaCidadesAtual[i].PON_ID + ',';
    }


    $.ajax({
        url: UrlBase + "/BuscarCidadesPontoMapa",
        data: {
            listaCidades: listaCidades,
        },
        type: "post",
        dataType: "json",
        async: true,
        success: function (result) {

            var classes = $('.pontoMapaClass').attr('class').split(' ');
            var title = classes[2].split('_')[1];
            var latitude = parseFloat(classes[3].split('_')[1].replace(',', '.'));
            var longitude = parseFloat(classes[4].split('_')[1].replace(',', '.'));

            if (map != null)
                map.remove();
            map = L.map('pontosMapa').setView([latitude, longitude], 13);


            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(map);

            L.marker([latitude, longitude]).addTo(map)
                .bindPopup('ORIGEM: ' + title)
                .openPopup();


            for (var i = 0; i < result.cidades.length; i++) {
                L.marker([result.cidades[i].MUN_LATITUDE, result.cidades[i].MUN_LONGITUDE]).addTo(map)
                    .bindPopup('DESTINO: ' + result.cidades[i].MUN_NOME)
                    .openPopup();

            }
            map.setZoom(7);

        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {
            Carregando.fechar();
        }
    });

}


function gravarCargasSimuladas() {
    var listaIds = [];
    $(".cbCarSim:checked").each(function () {
        var temp = $(this).attr("id");
        listaIds.push(temp.split('_')[1]);
    });

    var listaCargas = [];
    for (var i = 0; i < cargasSimuladas.length; i++) {
        if (listaIds.includes(cargasSimuladas[i][0].CAR_ID)) {
            listaCargas.push(cargasSimuladas[i]);
        }
    }

    var listaStrCargas = JSON.stringify(listaCargas);


    $.ajax({
        url: UrlBase + "/gravarCargasSimuladas",
        data: {
            listaStrCargas: listaStrCargas,
            cargaPrincipal: cargaPrincipal[0].CAR_ID
        },
        type: "post",
        dataType: "json",
        async: true,
        success: function (result) {
            if (result.status == "OK") {
                alert('A nova carga foi criada com sucesso!');
                var url = window.location.href.split('?');
                var novaUrl = url[0] + '?strCargas=' + result.msgRetorno + '&carIdPrincipal=' + result.msgRetorno;
                window.location.replace(novaUrl).reload(true);
            }
            else {
                alert('Houve erro com a criação da nova carga');
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {
            Carregando.fechar();
        }
    });

}

function pesquisarTipVeiculos() {
    $.ajax({
        url: UrlBase + "/ObterTiposVeiculos",

        type: "post",
        dataType: "json",
        async: true,
        success: function (result) {
            console.log('aqui');
            listaVeiculos = result;
            carregarTelaTiposVeiculos(result);

        },
        error: function (xhr, ajaxOptions, thrownError) {

        },
        complete: function () {
        }
    });
}

function carregarTelaTiposVeiculos(listaVeiculos) {
    var flagCor = 1;
    var str = '<table class="table table-bordered table-striped text-center ">';
    str += '<thead>';
    str += '<tr>';
    str += '<td>ID</td>';
    str += '<td>DESCRIÇÃO</td>';
    str += '<td>CAPACIDADE M³</td>';
    str += '<td>QTD. DISPONÍVEL</td>';
    str += '<td>VALOR KM</td>';
    str += '<td>VALOR DIÁRIA</td>';
    str += '<td>VALOR AJUDANTE</td>';
    str += '<td>QTD. EIXOS</td>';
    str += '<td>VELOCIDADE MÉDIA</td>';
    str += '</tr>';
    str += '</thead>';

    str += '<tbody>';

    var cor = '#f1f1f1';
    var corMouseOver = '#c4c4c4';
    for (var i = 0; i < listaVeiculos.length; i++) {
        str += '<tr id="tipVeiculoIndex_"' + i + '  onclick="atualizarTipVeiculo(' + i + ');" onmouseover="mouseSelecionado(this, \'' + corMouseOver + '\')" onmouseout="mouseDeselecionado(this, \'' + cor + '\')">';
        str += '<td>' + listaVeiculos[i].TIP_ID + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_DESCRICAO + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_CAPACIDADE_M3 + '</td>';

        str += '<td>' + listaVeiculos[i].TIP_QTD_DISPONIVEL + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_VALOR_KM + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_VALOR_DIARIA + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_VALOR_AJUDANTE + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_QTD_EIXOS + '</td>';
        str += '<td>' + listaVeiculos[i].TIP_VELOCIDADE_MEDIA + '</td>';
        str += '</tr>';

        if (flagCor == 1) {
            cor = '#ffffff';
            corMouseOver = '#c4c4c4';
            flagCor = 0;
        }
        else {
            cor = '#f1f1f1';
            corMouseOver = '#c4c4c4';
            flagCor = 1;
        }
    }
    str += '</tbody>';
    str += '</table>';

    str += '<div id="botoesCargasSimuladas">';
    str += '<div class=" col-sm-1 col-md-1 col-xs-6 padding">';
    str += '<p class="col-sm-12 col-md-12" for="" style="visibility:hidden;">_</p>';
    str += '<span class="input-group-addon" id="btnOtimizar" onclick="alterarTipVeiculo()">';
    str += '<button class="fa fa-bolt" style="background:transparent;border:none"></button>';
    str += '</span>';
    str += '</div>';
    str += '</div>';

    $('#tableTipVeiculos').html(str);
}

function atualizarTipVeiculo(index) {
    indexTipVeiculo = index;

    var temp = cargaPrincipal.concat(listaCargas);
    preencherTabelaCaminhao(temp);
}

function alterarTipVeiculo() {
    var listaIdsCargas = '';
    var temp = cargaPrincipal.concat(listaCargas);

    for (var i = 0; i < temp.length; i++) {
        if (!listaIdsCargas.includes(temp[i].CAR_ID))
            listaIdsCargas += temp[i].CAR_ID + ',';
    }
    if (indexTipVeiculo != -1) {
        var tipId = listaVeiculos[indexTipVeiculo].TIP_ID;

        $.ajax({
            url: UrlBase + "/AtualizarTiposVeiculos",
            data: {
                tipId: tipId,
                listaIdsCargas: listaIdsCargas
            },
            type: "post",
            dataType: "json",
            async: true,
            success: function (result) {


            },
            error: function (xhr, ajaxOptions, thrownError) {

            },
            complete: function () {
            }
        });
    }
}