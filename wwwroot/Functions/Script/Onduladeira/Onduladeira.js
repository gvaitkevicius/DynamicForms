function pegarIniFacao(fila, facao) {
    var fim = 0;
    var i = 0;
    while (i < fila.length && fila[i].COR_FACAO != facao)
        i++;

    if (i < fila.length)
        return fila[i].MinutoIni;
    return -1;
}

/*REMOVER DEPOIS*/
function pegarCorAleatoria() {
    /*
        #3F51B5: Azul
        #f44336: Vermelho
        #FFC107: Amarelo
        #4CAF50: Verde
    */

    var numero;
    var num = Math.floor(Math.random() * 3);
    switch (num) {
        case 0:
            numero = '#f44336';
            break;
        case 1:
            numero = '#FFC107';
            break;
        case 2:
            numero = '#4CAF50';
            break;
    }

    return numero;
}

$(document).ready(function () {
    if ($('.maquina-onduladeira').length > 0) {

        var UrlBase = '/plugandplay/aps/';
        $.get(UrlBase + 'CarregarOnduladeira').done(function (fila) {
            var _html = '<div class="ped-fila"  style="background-color: 0a000e; width:' + (fila.tamanhoInicial) + 'px; height:100px; display:block"></div>';
            //$('#div_maq_ONDU').prepend(_html);
            $('#maq_ONDU').css({ "background-color": "#d5b69f", "height": "100px", "width": pegarFinal(fila.onduladeira) + "px" });

            var minIniF1 = pegarIniFacao(fila.onduladeira, 1);
            var minIniF2 = pegarIniFacao(fila.onduladeira, 2);
            var menorInicio = minIniF1 <= minIniF2 ? minIniF1 : minIniF2;
            var f1 = [];
            var f2 = [];

            var fila = fila.onduladeira;

            for (let i = 0; i < fila.length; i++) {

                if (fila[i].COR_FACAO == 1) {
                    f1.push(fila[i]);
                }
                else {
                    f2.push(fila[i]);
                }

            }

            var div1 = desenharF1(f1, menorInicio);
            var div2 = desenharF2(f2, f1, menorInicio);

            $('#maq_ONDU').append(div1);
            $('#maq_ONDU').append(div2);

        }).fail(function () {
            alert('Sadness');
        }).always(function () {
            //alert('RAINHA DOS FAIXA PRETA!');
        })
    }
    else {
        //alert('Não tem onduladeira cadastrada trouxa');
        console.log('Não tem onduladeira cadastrada ainda!');
    }
})

function desenharF1(f1, menorInicio) {
    var div1 = '<div style="display: flex; height:50px;">';
    espaco_branco = f1[0].MinutoIni - menorInicio;
    div1 += '<div style="width:' + espaco_branco + 'px;"></div>';

    var i = 0;
    while (i < f1.length - 1) {
        tamanhoF1 = f1[i].MinutoFim - f1[i].MinutoIni;

        posX = f1[i].MinutoIni;
        posY = 0;
        largura = tamanhoF1;
        altura = f1[i].COR_PECAS_LARGURA;
        cor = pegarCorAleatoria();

        div1 += '<div onmouseout="javascript:deselPedidos(\'' + f1[i].ORD_ID + '\', \'' + cor + '\');" id="OND_pedId_' + f1[i].ORD_ID + '" onmouseover="javascript:selPedidos(\'' + f1[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f1[i].ORD_ID + '\');" style="border-color: black; border-width:2px; border-style:dashed; overflow: hidden; color: #000; text-align: center; width: ' + largura + 'px; background-color:' + cor + '; height: ' + altura + 'px;" >' + f1[i].ORD_ID + '</div >';

        espaco_branco = f1[i + 1].MinutoIni - f1[i].MinutoFim;
        div1 += '<div style="width:' + espaco_branco + 'px"></div>';

        i++;
    }
    tamanhoF1 = f1[i].MinutoFim - f1[i].MinutoIni;
    posX = f1[i].MinutoIni;
    posY = 0;
    largura = tamanhoF1;
    altura = f1[i].COR_PECAS_LARGURA;
    cor = pegarCorAleatoria();
    div1 += '<div onmouseout="javascript:deselPedidos(\'' + f1[i].ORD_ID + '\', \'' + cor + '\');" id="OND_pedId_' + f1[i].ORD_ID + '" onmouseover="javascript:selPedidos(\'' + f1[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f1[i].ORD_ID + '\');" style="border-color: black; border-width:2px; border-style:dashed; overflow: hidden; color: #000; text-align: center; width: ' + largura + 'px; background-color:' + cor + '; height: ' + altura + 'px;" >' + f1[i].ORD_ID + '</div >';

    div1 += '</div>';

    return div1;
}


function desenharF2(f2, f1, menorInicio) {
    var div2 = '<div style="display: flex; height:50px">';
    espaco_branco = f2[0].MinutoIni - menorInicio;
    div2 += '<div style="width:' + espaco_branco + 'px;"></div>';

    var i = 0;
    while (i < f2.length - 1) {
        tamanhoF2 = f2[i].MinutoFim - f2[i].MinutoIni;

        posX = f2[i].MinutoIni;
        posY = pegarPosY(f1, f2[i].MinutoIni, f2[i].MinutoFim);
        //posY = 20;
        largura = tamanhoF2;
        altura = f2[i].COR_PECAS_LARGURA;
        cor = pegarCorAleatoria();


        div2 += '<div onmouseout="javascript:deselPedidos(\'' + f2[i].ORD_ID + '\', \'' + cor + '\');" id="OND_pedId_' + f2[i].ORD_ID + '" onmouseover="javascript:selPedidos(\'' + f2[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f2[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f2[i].ORD_ID + '\');" style="border-color: black; border-width:2px; border-style:dashed; overflow: hidden; color: #000; text-align: center; margin-top:' + -posY + 'px; width:' + largura + 'px; background-color:' + cor + '; height: ' + altura + 'px;" >' + f2[i].ORD_ID + '</div >';
        espaco_branco = f2[i + 1].MinutoIni - f2[i].MinutoFim;

        div2 += '<div style="width:' + espaco_branco + 'px"></div>';

        i++;
    }
    tamanhoF2 = f2[i].MinutoFim - f2[i].MinutoIni;
    posX = f2[i].MinutoIni;
    posY = pegarPosY(f1, f2[i].MinutoIni, f2[i].MinutoFim);
    largura = tamanhoF2;
    altura = f2[i].COR_PECAS_LARGURA;
    cor = pegarCorAleatoria();

    div2 += '<div onmouseout="javascript:deselPedidos(\'' + f2[i].ORD_ID + '\', \'' + cor + '\');" id="OND_pedId_' + f2[i].ORD_ID + '" onmouseover="javascript:selPedidos(\'' + f2[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f2[i].ORD_ID + '\');" ondblclick="javascript:dadosPedOnd(\'' + f2[i].ORD_ID + '\');" style="border-color: black; border-width:2px; border-style:dashed; overflow: hidden; color: #000; text-align: center; margin-top:' + -posY + 'px; width:' + largura + 'px; background-color:' + cor + '; height: ' + altura + 'px;" >' + f2[i].ORD_ID + '</div >';
    div2 += '</div>';

    return div2;
}

function pegarPosY(f1, ini, fim) {
    var i = 0;
    var altura = -1;
    while (i < f1.length) {
        var j = ini;

        while (j < fim) {

            if (j >= f1[i].MinutoIni && j <= f1[i].MinutoFim) {
                if (altura < f1[i].COR_PECAS_LARGURA)
                    altura = f1[i].COR_PECAS_LARGURA;
            }

            j++;
        }

        i++;
    }

    return 50 - altura;
}

function dadosPedOnd(ord_id) {
    var UrlBase = '/plugandplay/aps/';
    $.get(UrlBase + 'obterFilaOndu?ord_id=' + ord_id).done(function (fila) {
        var f = fila[0];
        dadosPed(
            f.grpDescricao,
            f.hieranquiaSeqTransf,
            f.GrupoProdutivo,
            f.CorBico5,
            f.CorBico4,
            f.CorBico3,
            f.CorBico2,
            f.CorBico1,
            f.grupoMaquinaId,
            f.TempoSetupA,
            f.TempoSetup,
            f.Performance,
            f.M2Total,
            f.M2Unitario,
            f.PecasPorPulso,
            f.CliNome,
            f.CliId,
            f.maquina,
            f.maquinaId,
            f.produtoId,
            f.pedidoId,
            f.produto,
            moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss"),
            moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss"),
            f.seqTransform,
            f.seqRepet,
            (f.MinutoFim - f.MinutoIni),
            f.Id,
            f.CongelaFila,
            f.OrdemDaFila,
            moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss"),
            f.qtd,
            moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss"),
            moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss"),
            f.OrdOpIntegracao);

    }).fail(function () {
        alert('Sadness');
    }).always(function () {
        //alert('Rainy lofi');
        console.log('O meu lugar...');
    })
}

function pegarFinal(fila) {
    var maiorFinal = -1;
    for (var i = 0; i < fila.length; i++) {
        if (maiorFinal < fila[i].MinutoFim)
            maiorFinal = fila[i].MinutoFim;
    }
    return maiorFinal;
}

/*
 function deselPedidos(pedido, cor_borda, status = null) {

    for (var y = 0; y < aMaquinas.length; y++) {
        var filhos = $('#maq_' + aMaquinas[y]).children().length;
        for (var i = 0; i < filhos; i++) {
            //'pedId -'
            //var tamanhoAtualDoFilho = $('#maq_' + aMaquinas[y]).children().eq(i).width();
            //var tamanhoAjustado = calculaEscala(select, tamanhoAtualDoFilho);
            //$('#maq_' + aMaquinas[y]).children().eq(i).width(tamanhoAjustado);
            if (status == null) {
                if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') == 'pedId_' + pedido) {
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", cor_borda);
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
                }
            }
            else { //krupck
                if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') == 'pedId_' + pedido) {
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", "#e30be3");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                    $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
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
            if ($('#maq_' + aMaquinas[y]).children().eq(i).attr('id') == 'pedId_' + pedido) {
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-color", "#FFFFFF");
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-style", "dashed");
                $('#maq_' + aMaquinas[y]).children().eq(i).css("border-width", border_width_fila_producao);
            }
        }
    }
}

 */