var neglobal;//Lista de (Ops) nao embarcadas              --// Pendencia não migrado do SGI até 12/12/2019 -joao
var neTotItens; //Total de produtos (itens)nao embarcados --// Pendencia não migrado do SGI até 12/12/2019 -joao
var _listaUniaoCargas = []; //Vetor de cargas selecionadas para união //Funcional

$(document).ready(function () {
    cmLoadFlag = false;
    exLoadFlag = false;
    fpLoadFlag = false;
    //Cadastro.inicio(); //OK
    //ValidacaoFormulario.inicio(); //OK
    //ModalAlterarFila.inicio(); //OK

    //DadosOtimizador.iniciar();
    //DadosInterface.iniciar();

    TabelaCargaMaquina.iniciar(); //OK
    //TabelaCargaMaquina.carregarTabela('', '', ''); //OK

    //TabelaEstoque.iniciar();
    //TabelaEstoque.carregarTabela(); //O código está tudo comentado.

    //TabelaOnduladeira.iniciar(); //Onduladeira.js
    //TabelaOnduladeira.carregar(); //Onduladeira.js

    TabelaFilaProducao.iniciar();
    //TabelaFilaProducao.carregarTabela('');

    TabelaFilaExpedicao.iniciar();
    // ListaPendenciasCadastrais.iniciar();
    ListaPendenciasCadastrais.carregarTabela(); //Pendencia
    ListaOPsParciais.carregarTabela();
    ListaPedidosSuspensos.carregarTabela();

    ModalADDSeqerarFila.inicio();
});

var cmLoadFlag = false;
function cmLoad() {
    if (!cmLoadFlag) {
        TabelaCargaMaquina.carregarTabela('', '', '');
        cmLoadFlag = true;
    }
    else {
        //cmLoadFlag = false;
    }
}

var exLoadFlag = false;
function exLoad() {
    if (!exLoadFlag) {
        //TabelaFilaExpedicao.carregarTabela();
        exLoadFlag = true;
    }
    else {
        //exLoadFlag = false;
    }
}

var fpLoadFlag = false;
function fpLoad() {
    if (!fpLoadFlag) {
        TabelaFilaProducao.carregarTabela('');
        fpLoadFlag = true;
    }
    else {
        //fpLoadFlag = false;
    }
}


function removePedidos() {
    for (var y = 0; y < aMaquinas.length; y++) {
        $('#maq_' + aMaquinas[y]).empty();

        //while ($('#maq_' + aMaquinas[y]).children().length > 0) {
        //    $('#maq_' + aMaquinas[y]).children().eq(0).remove();
        //}
        //while ($('#contemLinhaDoTempo').children().length > 0) {
        //    $('#contemLinhaDoTempo').children().eq(0).remove();
        //}
    }
    $('#contemLinhaDoTempo').empty();

}

function deselPedidos(pedido, cor_borda, status = null) {

    for (var y = 0; y < aMaquinas.length; y++) {
        var filhos = $('#maq_' + aMaquinas[y]).children().length;
        for (var i = 0; i < filhos; i++) {
            //'pedId -'
            //var tamanhoAtualDoFilho = $('#maq_' + aMaquinas[y]).children().eq(i).width();
            //var tamanhoAjustado = calculaEscala(select, tamanhoAtualDoFilho);
            //$('#maq_' + aMaquinas[y]).children().eq(i).width(tamanhoAjustado);
            if (status === null) {
                if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') === 'pedId_' + pedido) {
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", cor_borda);
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
                }
                else if (aMaquinas[y] == 'ONDU') {
                    $('#OND_pedId_' + pedido).css("border-color", "#000");
                    $('#OND_pedId_' + pedido).css("border-style", "dashed");
                    $('#OND_pedId_' + pedido).css("border-width", border_width_fila_producao);
                }
            }
            else { //krupck
                if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') === 'pedId_' + pedido) {
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", "#81d6b5");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
                }
                else if (aMaquinas[y] == 'ONDU') {
                    $('#OND_pedId_' + pedido).css("border-color", "#000");
                    $('#OND_pedId_' + pedido).css("border-style", "dashed");
                    $('#OND_pedId_' + pedido).css("border-width", border_width_fila_producao);
                }
                //border-style: dashed; border-color: rgb(255, 168, 18); border-width: 4px
            }
        }
    }
}

function selPedidos(pedido) {

    for (var y = 0; y < aMaquinas.length; y++) {
        var filhos = $('#maq_' + aMaquinas[y]).children().length;
        for (var i = 0; i < filhos; i++) {
            //'pedId -'
            //var tamanhoAtualDoFilho = $('#maq_' + aMaquinas[y]).children().eq(i).width();
            //var tamanhoAjustado = calculaEscala(select, tamanhoAtualDoFilho);
            //$('#maq_' + aMaquinas[y]).children().eq(i).width(tamanhoAjustado);
            if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') === 'pedId_' + pedido) {
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", "#FFFFFF");
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
            }
            else if (aMaquinas[y] == 'ONDU') {
                $('#OND_pedId_' + pedido).css("border-color", "#FFFFFF");
                $('#OND_pedId_' + pedido).css("border-style", "dashed");
                $('#OND_pedId_' + pedido).css("border-width", border_width_fila_producao);
            }
        }
    }
}

function abrirPedido() {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.Order';
    var ord_id = $('#d_pedidoId').text();
    var str = global_modal(ord_id, namespace, 2);
    if (str !== null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

function abrirProduto() {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaProdutoCaixa';
    var pro_id = $('#d_produtoId').text();
    var str = global_modal(pro_id, namespace, 2);
    if (str !== null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

function abrirFila() {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.FilaProducao';
    var fila_id = $('#d_IdFila').text();
    var str = global_modal(fila_id, namespace, 2);
    if (str !== null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

function abrirMaquina() {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.Maquina';
    var maq_id = $('#d_maquinaId').text();
    var str = global_modal(maq_id, namespace, 2);
    if (str !== null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

///monta a string de pesquisa, esconde a modal, minimiza a aba de fila produção e abre a aba de expedição com a pesquisa
function abrirCarga(value, column) {
    let string_pesquisa = pegarFiltros([`${value}`], [`${column}`], ["igual a"]);
    desmontarStringPesquisa(string_pesquisa, "aps");
    jQuery('#mDadosPed').modal('hide');

    $('#collapseThree-4').removeClass("collapse in");
    $('#collapseThree-4').addClass("collapse");

    $('#collapseTwo-4').removeClass("collapse");
    $('#collapseTwo-4').addClass("collapse in");
    $('#collapseTwo-4').removeAttr('style');
    TabelaFilaExpedicao.eventoPesquisarPrincipalAPS();
}


function dadosPed(equipeId, grpDescricao, hieranquiaSeqTransf, grupoProdutivo, CorBico5, CorBico4, CorBico3, CorBico2, CorBico1, grupoMaquinaId, TempoSetupA, TempoSetup, Performance, M2Total, M2Unitario, PecasPorPulso, CliNome, CliId, maquina, maquinaId, produtoId, pedidoId, produto, inicioPrevisto, fimPrevisto, seqTransform, seqRepet, tempoOpMin, IdFila, Congela, OrdemDaFila, dtPrevisaoMat, qtd, EntregaDe, EntregaAte, ordOpIntegracao, ordTipo, status) {

    status = status.toUpperCase();
    status =
        status == 'R1' ? '(R1) Reserva Fraca' :
        status == 'R2' ? '(R2) Reserva Forte' :
        status == 'E1' ? '(E1) Entrada por Simulação' :
        status == 'EI' ? '(EI) Encerrada pela Interface' :
        status == 'EC' ? '(EC) Encerrado por Cancelamento' : "";

    $(document).ready(function () {
        jQuery('#d_maquina').text(maquina);
        jQuery('#d_maquinaId').text(maquinaId);
        jQuery('#d_ordOpIntegracao').text(ordOpIntegracao);
        jQuery('#d_produtoId').text(produtoId);
        jQuery('#d_pedidoId').text(pedidoId);
        jQuery('#d_produto').text(produto);
        jQuery('#d_EntregaDe').text(EntregaDe);
        jQuery('#d_EntregaAte').text(EntregaAte);
        jQuery('#d_OrdStatus').text(status);
        jQuery('#d_inicioPrevisto').text(inicioPrevisto);
        jQuery('#d_fimPrevisto').text(fimPrevisto);
        jQuery('#d_EquipeId').text(equipeId);
        jQuery('#d_seqTransform').text(seqTransform);
        jQuery('#d_seqRepet').text(seqRepet);
        jQuery('#d_tempoOpMin').text(tempoOpMin);
        jQuery('#d_qtd').text(qtd);
        jQuery('#d_IdFila').text(IdFila);
        jQuery('#d_grupoProdutivo').text(grupoProdutivo);
        jQuery('#d_congela').text(Congela);
        jQuery('#d_prevMatPrima').text(dtPrevisaoMat);
        jQuery('#d_CliId').text(CliId);
        jQuery('#d_CliNome').text(CliNome);
        jQuery('#d_PecasPorPulso').text(PecasPorPulso);
        jQuery('#d_M2Unitario').text(M2Unitario);
        jQuery('#d_M2Total').text(M2Total);
        jQuery('#d_Performance').text(Performance);
        jQuery('#d_TempoSetup').text(TempoSetup);
        jQuery('#d_TempoSetupA').text(TempoSetupA);
        jQuery('#d_hieranquiaSeqTransf').text(hieranquiaSeqTransf);
        jQuery('#d_tipo_produto').text(grpDescricao);

        //desabilita o botao de abrir a carga
        $('#icon-abrir-carga').prop('disabled', true);
        $('#icon-abrir-carga').removeClass('pointer');
        $('#icon-abrir-carga').removeClass('fa fa-car');
        $('#icon-abrir-carga').off('click');

        //se for somente produção, o botao continuara desabilitado
        if (ordTipo != 2) {
            $('#icon-abrir-carga').prop('disabled', false);
            $('#icon-abrir-carga').addClass('pointer');
            $('#icon-abrir-carga').addClass('fa fa-car');
            $('#icon-abrir-carga').on('click');

            //se for uma doca (gma_id = srv expedicao) irá fazer uma pesquisa de cargas pelo MAQ_ID
            //se nao, ira fazer a pesquisa pelo ORD_ID
            if (grupoMaquinaId == "SRV_EXPEDICAO") {
                $('#icon-abrir-carga').click(function (event) { event.preventDefault(); abrirCarga(`${pedidoId}`, 'CAR_ID'); });
            }
            else {
                $('#icon-abrir-carga').click(function (event) { event.preventDefault(); abrirCarga(`${pedidoId}`, 'ORD_ID'); });
            }
        }

        $('.tabela-tintas-maquina tr').remove();
        if (CorBico1 === '')
            CorBico1 = 'indef.';
        if (CorBico2 === '')
            CorBico2 = 'indef.';
        if (CorBico3 === '')
            CorBico3 = 'indef.';
        if (CorBico4 === '')
            CorBico4 = 'indef.';
        if (CorBico5 === '')
            CorBico5 = 'indef.';

        //if (grupoMaquinaId === 'IMP') {
        $('.tabela-tintas-maquina').append([
            $('<tr>').html([
                $('<th>').html('Bico 1'),
                $('<th>').html('Bico 2'),
                $('<th>').html('Bico 3'),
                $('<th>').html('Bico 4'),
                $('<th>').html('Bico 5')
            ]),
            $('<tr>').html([
                $('<td>').html(CorBico1),
                $('<td>').html(CorBico2),
                $('<td>').html(CorBico3),
                $('<td>').html(CorBico4),
                $('<td>').html(CorBico5)
            ])
        ]);
        //}


        $('#altOrdem').html('');
        $('#altmaq').html('<span onclick="javascript: obterRoteirospossiveis();">Alterar maquina</span>');
        _html = '<select ID="d_altOrdem" class="form-control input-sm m-bot15">';

        if (OrdemDaFila > Congela) {
            _html += '<option value="9999999" selected>Calcular automaticamente</option>';
        } else {
            _html += '<option value="9999999">Calcular automaticamente</option>';

        }
        var valor = Congela * -1;
        for (var i = 1; i <= Congela; i++) {
            if (valor === OrdemDaFila) {
                _html += '<option value="' + valor + '" selected> ' + i + '</option>';
            } else {
                _html += '<option value="' + valor + '" > ' + i + '</option>';
            }
            valor++;
        }
        $('#altOrdem').append(_html + '</select>');


        jQuery('#mDadosPed').modal('show', { backdrop: 'stand' });
    });
}

function ConversorTempo(segundos, unidade_tempo) {
    /*
        MONTH - MES
        WEEK - SEMANA
        DAY - DIA
        HOUR - HORA
        MINUTE - MINUTO
        SECOND - SEGUNDO
        MILLISECOND - MILISEGUNDO
     */

    var res_numerico;
    var res_string = "error";

    //Da para melhorar fazendo recursivo
    switch (unidade_tempo.toUpperCase()) {
        case "MES":
            break;
        case "SEMANA":
            break;
        case "DIA":
            break;
        case "HORA":
            res_numerico = Math.trunc(segundos / 3600);
            var resto = segundos % 3600;
            res_string = res_numerico + "h";

            res_numerico = Math.trunc(resto / 60);
            resto = resto % 60;
            res_string += res_numerico + "m" + resto + "s";

            break;
        case "MINUTO":
            res_numerico = Math.trunc(segundos / 60);
            var resto_sec = segundos % 60;
            res_string = res_numerico + "m" + resto_sec + "s";

            break;
        case "SEGUNDO":
            res_string = segundos + "s";
            break;
        case "MILISEGUNDO":
            res_numerico = segundos * 1000;
            res_string = res_numerico + "ms";

            break;
    }

    return res_string;
}

function compararDataHora(data1, data2) {
    var hora1 = data1.split(' ')[1];
    var hora2 = data2.split(' ')[1];

    var invD1 = data1.split(' ')[0].split('/')[1] + '/' + data1.split(' ')[0].split('/')[0] + ' ' + hora1;
    var invD2 = data2.split(' ')[0].split('/')[1] + '/' + data2.split(' ')[0].split('/')[0] + ' ' + hora2;

    if (invD1 == invD2)
        return 0;
    else if (invD1 > invD2)
        return 1;
    else //invD2 > invD1
        return -1;
}

//Formato de entrada: YYYY-MM-DDTHH:MM:SS
function formatarDataHora(str) {
    //2020-12-27T17:00:24
    str = str.split('T');
    var data = str[0];
    var res = data.split('-')[2] + '/' + data.split('-')[1] + ' ' + str[1].split(':')[0] + ':' + str[1].split(':')[1];
    return res;
}

function formatarData(str) {
    str = str.split('T');
    var data = str[0];
    var res = data.split('-')[2] + '/' + data.split('-')[1];
    return res;
}

//valida se é um formato iso
function isIsoDate(data) {
    var RegExPattern = /^\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d/;

    if (!((data.match(RegExPattern)) && (data != ''))) {
        return false;
    }
    else
        return true;
}

function validaData(day, month, year) {
    if (month < 1 || month > 12) { // check month range
        alert("Month must be between 1 and 12");
        return false;
    }
    if (day < 1 || day > 31) {
        alert("Day must be between 1 and 31");
        return false;
    }
    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
        alert("Month " + month + " doesn't have 31 days!")
        return false;
    }
    if (month == 2) { // check for february 29th
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day == 29 && !isleap)) {
            alert("February " + year + " doesn't have " + day + " days!");
            return false;
        }
    }
    return true;  // date is valid
}

function cliqueClipBoard(text) {
    var textArea = document.createElement("textarea");

    // Place in top-left corner of screen regardless of scroll position.
    textArea.style.position = 'fixed';
    textArea.style.top = 0;
    textArea.style.left = 0;

    // Ensure it has a small width and height. Setting to 1px / 1em
    // doesn't work as this gives a negative w/h on some browsers.
    textArea.style.width = '2em';
    textArea.style.height = '2em';

    // We don't need padding, reducing the size if it does flash render.
    textArea.style.padding = 0;

    // Clean up any borders.
    textArea.style.border = 'none';
    textArea.style.outline = 'none';
    textArea.style.boxShadow = 'none';

    // Avoid flash of white box if rendered for any reason.
    textArea.style.background = 'transparent';


    textArea.value = text;

    document.body.appendChild(textArea);

    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }

    document.body.removeChild(textArea);
}

async function cliqueColarClipBoard(cargaId, pedidoId) {
    const text = await navigator.clipboard.readText();

    var tr_id = '#ant_' + cargaId.replaceAll('/', '_').replaceAll('.', '_') + '_' + pedidoId.replaceAll('/', '_').replaceAll('.', '_');
    $(tr_id + ' .new_carga').text(text);


}


function eventoAlterarItemCarga(cargaId, pedidoId, embarque) {
    var retorno = true;
    var tr_id = '#ant_' + cargaId.replaceAll('/', '_').replaceAll('.', '_') + '_' + pedidoId.replaceAll('/', '_').replaceAll('.', '_');

    var old_carga = $(tr_id + ' .old_carga').text();
    var old_ord_entrega = $(tr_id + ' .old_ord_entrega').text();
    var old_itc_qtd_plan = $(tr_id + ' .old_itc_qtd_plan').text();
    var old_tipo_caminhao = $(tr_id + ' .old_tipo_caminhao').text();


    var new_carga = $(tr_id + ' .new_carga').text() != '' ? $(tr_id + ' .new_carga').text() : $(tr_id + ' .old_carga').text();
    var new_ord_entrega = $(tr_id + ' .new_ord_entrega').text() != '' ? $(tr_id + ' .new_ord_entrega').text() : $(tr_id + ' .old_ord_entrega').text();
    var new_itc_qtd_plan = $(tr_id + ' .new_itc_qtd_plan').text() != '' ? $(tr_id + ' .new_itc_qtd_plan').text() : $(tr_id + ' .old_itc_qtd_plan').text();
    var new_tipo_caminhao = $(tr_id + ' .new_tipo_caminhao').text() != '' ? $(tr_id + ' .new_tipo_caminhao').text() : $(tr_id + ' .old_tipo_caminhao').text();

    if (parseInt(new_itc_qtd_plan)) {
        var aux_num = parseInt(new_itc_qtd_plan);
        if (aux_num > 0 && aux_num <= old_itc_qtd_plan) {
            new_carga = new_carga.toUpperCase();
            if (new_carga == "NOVO" || new_carga == "NOVA" || new_carga == "NEW") {
                var today = new Date();
                var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
                var time = today.getHours() + ":" + today.getMinutes();
                var dateTime = date + 'T' + time;

                var novaCarga_str = '[{"CAR_STATUS":"","CAR_EMBARQUE_ALVO":"' + embarque + '","TIP_ID":"","VEI_PLACA":"","TRA_ID":"","CAR_OBSERVACAO_DE_TRANSPORTE":"","CAR_ID_DOCA":"","CAR_PREVISAO_MATERIA_PRIMA":"","CAR_DATA_INICIO_REALIZADO":"","CAR_DATA_FIM_REALIZADO":"","CAR_PESO_TEORICO":"","CAR_VOLUME_TEORICO":"","CAR_PESO_REAL":"","CAR_VOLUME_REAL":"","CAR_PESO_EMBALAGEM":"","CAR_PESO_ENTRADA":"","CAR_PESO_SAIDA":"","CAR_GRUPO_PRODUTIVO":"","ROT_ID":"","CAR_JUSTIFICATIVA_DE_CARREGAMENTO":"","PlayAction":"CARGA_APS","MovimentoEstoqueProducao":[],"MovimentoEstoqueReservaDeEstoque":[],"MovimentoEstoqueVendas":[],"MovimentoEstoqueSaidaInventario":[],"MovimentoEstoqueEntradaInventario":[],"MovimentoEstoqueTransferenciaSimples":[],"MovimentoEstoqueConsumoMateriaPrima":[],"MovimentoEstoquePerdas":[],"ItensCarga":[]}]';
                var novaCarga = JSON.parse(novaCarga_str);
                var namespace = 'DynamicForms.Areas.PlugAndPlay.Models.Carga';

                insertBanco(novaCarga, namespace, 0);

                new_carga = result_log[0].PrimaryKey.split(':')[1];
            }

            $.ajax({
                type: "POST",
                url: UrlBase + "GerenciarCargas",
                async: false,
                data: {
                    pedidoId: pedidoId,
                    old_carga: old_carga,
                    new_carga: new_carga,
                    old_ord_entrega: old_ord_entrega,
                    new_ord_entrega: new_ord_entrega,
                    old_itc_qtd_plan: old_itc_qtd_plan,
                    new_itc_qtd_plan: new_itc_qtd_plan,
                    old_tipo_caminhao: old_tipo_caminhao,
                    new_tipo_caminhao: new_tipo_caminhao
                },
                dataType: "json",
                traditional: true,
                async: false,
                contenttype: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.msg != "OK") {
                        //Deu algum erro esperado. Erro causado pelo usuário.
                        alert(result.msg);
                        retorno = false;
                    }
                    else {
                        alert("Carga movimentada com sucesso!");
                        $(tr_id + ' .old_carga').text(new_carga); //Só vai setar o texto de nova carga quando for sucesso na transação;
                        $(tr_id + ' .new_carga').text("");
                    }
                },
                error: function () {
                    alert('Erro inesperado:' + erro);
                    retorno = false;
                }
            });
        }
        else {
            alert('A nova quantidade planejada deve ser maior que zero e menor igual à ' + old_itc_qtd_plan);
            retorno = false;
        }
    }
    else {
        alert('O valor digitado no campo nova quantidade planejada deve ser em branco ou um valor inteiro maior que zero e menor igual à ' + old_itc_qtd_plan);
        retorno = false;
    }

    //Atualizar a tabela.
    return retorno;
}

//url padrao para as solicitacoes ajax
var UrlBase = '/plugandplay/aps/';
var UrlBaseConsultas = '/plugandplay/consultas/';


var horaAtual = 0;
var minAtual = 30;
var aMaquinas = {};