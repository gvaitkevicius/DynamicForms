/*############################################# ESTOQUE PA - PRODUTOS ACABADOS ############################################*/
function EstoquePA(nivel, grupo, indice = -1, grp_id = '-1') {
    var path = "/plugandplay/APS/ObterEstoquePA";
    //Carregando.abrir('Processando....');
    $.ajax({
        url: path,
        type: "GET",
        data: { nivel: nivel, grupo: grupo, grp_id: grp_id },
        dataType: "json",
        success: function (result) {
            if (nivel == 'N1')
                gerarEstoquePA_HTML(result.listaN1);
            else if (nivel == 'N2') {
                gerarGrupoPA_HTML(result.listaN2, indice, grp_id);
            }
            else if (nivel == 'N3') {
                gerarProdutoPA_HTML(result.listaN3, indice);
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}

function gerarEstoquePA_HTML(result) {
    var str_html = "";
    //Modal de Estoque Lote e Pedidos Futuros.
    str_html += '<div id="ModalEstoquePA" style="z-index:9997 !important;" class="modal fade" role="dialog">';
    str_html += '<div style="width: 85%;" class="modal-dialog"></div>';
    str_html += '</div>';
    //Modal de Quantidade
    str_html += '<div id="ModalQuantidadePA" style="z-index:9998 !important;" class="modal fade" role="dialog">';
    str_html += '<div style="width:55%;" class="modal-dialog"></div>';
    str_html += '</div>';

    str_html += '<div class="panel-body">'
    str_html += '<div class="col-lg-12" id="Estoque">';
    for (var i = 0; i < result.length; i++) {
        var grp_id = 0;
        var temp_contem = 'CX';
        if (result[i].GRP_TIPO == "CHAPA") {
            grp_id = 2;
            temp_contem = 'CH';
        }
        //var grp_id = result[i].GRP_TIPO == "CHAPA" ? "2" : "0";

        str_html += '<div class="col-lg-12" id="EstoquePA_' + i + '">';
        str_html += '<div class="panel panel-default">';

        var header1 = '<div onclick="javascript:EstoquePA(\'N2\',\'' + temp_contem + '\', ' + i + ', \'' + grp_id + '\');" class="panel-heading PA_N2_' + i + ' PA_N2_' + temp_contem + '_' + i + '_' + grp_id + '" role="tab" id="PA_' + i + '">';
        header1 += '<a style="width:100%; padding:10px" data-toggle="collapse" data-parent="#EstoquePA_' + i + '" href="#PA_Nivel1_' + i + '" aria-expanded="false" aria-controls="collapseOne-3" class="collapsed">';
        header1 += '<div style="display:flex">';

        var tag_i = '<i class="fa fa-check"></i>' + result[i].GRP_TIPO + '&emsp;';
        var span1 = '<span class="label label-success label-aps-n3" id="pa_dis_' + i + '" title="Disponível">' + result[i].DISPONIVEL + ' Disp.</span>&emsp;';
        var span2 = '<span class="label label-danger label-aps-n3" id="pa_sob_' + i + '" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + ' Sobra de Prod.</span>&emsp;';
        var span3 = '<span class="label label-danger label-aps-n3" id="pa_exp_' + i + '" title="Sobra Expedição">' + result[i].SOBRA_EXPEDICAO + ' Sobra de Exp.</span>&emsp;';
        var span4 = '<span class="label label-danger label-aps-n3" id="pa_dev_' + i + '" title="Devolvidas">' + result[i].DEVOLUCAO + ' Devolv.</span>&emsp;';
        var span5 = '<span class="label label-success label-aps-n3" id="pa_com_' + i + '" title="Compromissado">' + result[i].COMPROMISSADO + ' Compromissado</span>&emsp;';
        var span7 = '<span class="label label-secondary label-aps-n3" id="pa_ret_' + i + '" title="Estoque Retido"> ' + result[i].SALDO_RETIDO + ' Retido</span>&emsp;';
        var span6 = '<span class="label label-info label-aps-n3" id="pa_fut_' + i + '" title="Ped. Futuros">' + result[i].PEDIDOS_FUTUROS + ' Pedidos Fut.</span>&emsp;';

        header1 += '<div style="width:10%">' + tag_i + '</div>' + '<div style="width:90%">' + span1 + span2 + span3 + span4 + span5 + span7 + span6 + '</div>';
        header1 += '</div></a>';
        header1 += '</div>'

        var body1 = '<div id="PA_Nivel1_' + i + '" class="panel-collapse collapse" role="tabpane1" aria-labelledby="headingOne4" aria-expanded="false">';
        body1 += '<div class="panel-body" id="PA_grupo_' + i + '">';

        body1 += '</div>';
        body1 += '</div>';

        str_html += header1 + body1;
        str_html += '</div>';
        str_html += '</div>';
    }
    str_html += '</div>';
    str_html += '</div>';

    $('#estoquePaArea').html(str_html);
}

function gerarGrupoPA_HTML(result, indice) {
    var str_html = "";

    for (var i = 0; i < result.length; i++) {
        str_html += '<div id="PA_Nivel2_' + indice + '_' + i + '">';
        str_html += '<div class="panel panel-default">';
        str_html += '<div onclick="javascript:EstoquePA(\'N3\',\'CXCN\', \'' + indice + '_' + i + '\', \'' + result[i].GRP_ID + '\');" class="panel-heading PA_N3_' + indice + '_' + i + ' PA_N3_' + result[i].GRP_ID + '" role="tab">';
        str_html += '<a style="width:100%; padding:10px" data-toggle="collapse" data-parent="#accordCargMaq" href="#PA_Nivel3_' + indice + '_' + i + '" aria-expanded="false" aria-controls="collapseOne-3" class="collapsed">';
        str_html += '<div style="display:flex">';

        var tag_i = '<i class="fa fa-check"></i>' + result[i].GRP_ID + ' - ' + result[i].GRP_DESCRICAO + '&emsp;';
        var span1 = '<span id="pa_dis_' + indice + '_' + i + '" class="label label-success label-aps-n4" title="Disponível">' + result[i].DISPONIVEL + '</span>&emsp;';
        var span2 = '<span id="pa_sob_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + '</span>&emsp;';
        var span3 = '<span id="pa_exp_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Sobra de Expedição">' + result[i].SOBRA_EXPEDICAO + '</span>&emsp;';
        var span4 = '<span id="pa_dev_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Devolvidas">' + result[i].DEVOLUCAO + '</span>&emsp;';
        var span5 = '<span id="pa_com_' + indice + '_' + i + '" class="label label-success label-aps-n4" title="Compromissado">' + result[i].COMPROMISSADO + '</span>&emsp;';
        var span7 = '<span id="pa_ret_' + indice + '_' + i + '" class="label label-secondary label-aps-n4" title="Estoque Retido">' + result[i].SALDO_RETIDO + '</span>&emsp;';
        var span6 = '<span id="pa_fut_' + indice + '_' + i + '" class="label label-info label-aps-n4" title="Pedidos Futuros">' + result[i].PEDIDOS_FUTUROS + '</span>&emsp;';

        str_html += '<div style="width:25%">' + tag_i + '</div>' + '<div style="width:75%">' + span1 + span2 + span3 + span4 + span5 + span7 + span6 + '</div>';

        str_html += '</div></a>';
        str_html += '</div>';
        str_html += '</div>';
        str_html += '</div>';

        //Corpo do item
        str_html += '<div id="PA_Nivel3_' + indice + '_' + i + '" class="panel-collapse collapse" role="tabpane1" aria-labelledby="headingOne4" aria-expanded="false">';
        str_html += '<div id="Produto_' + indice + '_' + i + '">';
        str_html += '</div>';
        str_html += '</div>';
    }

    $('#PA_grupo_' + indice).html(str_html);
}

function gerarProdutoPA_HTML(result, indice) {
    var str_html = '<div style="overflow: auto">';
    str_html += '<table class="table table-bordered table-striped text-center table_estoquePA">';
    str_html += '<thead>';
    str_html += '<tr>';

    str_html += '<td>PRO ID</td>';
    str_html += '<td>PRO DESCRIÇÃO</td>';
    str_html += '<td>DISPONÍVEL</td>';
    str_html += '<td>SOBRA DE PRODUÇÃO</td>';
    str_html += '<td>SOBRA DE EXPEDIÇÃO</td>';
    str_html += '<td>DEVOLVIDOS</td>';
    str_html += '<td>COMPROMISSADO</td>';
    str_html += '<td>ESTOQUE RETIDO</td>';
    str_html += '<td>PEDIDOS FUTUROS</td>';

    str_html += '</tr>';
    str_html += '</thead>';
    str_html += '<tbody>';
    for (let i = 0; i < result.length; i++) {
        var objeto = JSON.stringify(result[i]).replaceAll('"', '&quot;');
        str_html += '<tr onclick="javascript:abrirModalEstoque(' + objeto + ', \'PA\', \'PA_N3_' + indice + '_' + i + '\')">';
        str_html += '<td>' + result[i].PRO_ID + '</td>'; //Pro id
        str_html += '<td>' + result[i].PRO_DESCRICAO + '</td>'; //Pro nome
        str_html += '<td><span id="pa_dis_' + indice + '_' + i + '" class="label label-success" title="Disponível">' + result[i].DISPONIVEL + '</span></td>'; //Disponivel
        str_html += '<td><span id="pa_sob_' + indice + '_' + i + '" class="label label-danger" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + '</span></td>'; //Sobra de produção
        str_html += '<td><span id="pa_exp_' + indice + '_' + i + '" class="label label-danger" title="Sobra de Expedição">' + result[i].SOBRA_EXPEDICAO + '</span></td>'; //Sobra de expedição
        str_html += '<td><span id="pa_dev_' + indice + '_' + i + '" class="label label-danger" title="Devolvidas">' + result[i].DEVOLUCAO + '</span></td>'; //Devolvidos
        str_html += '<td><span id="pa_com_' + indice + '_' + i + '" class="label label-success" title="Compromissado">' + result[i].COMPROMISSADO + '</span></td>'; //Compromissados
        str_html += '<td><span id="pa_ret_' + indice + '_' + i + '" class="label label-secondary" title="Estoque Retido">' + result[i].SALDO_RETIDO + '</span></td>'; //Pedidos retidos
        str_html += '<td><span id="pa_fut_' + indice + '_' + i + '" class="label label-info" title="Pedidos Futuros">' + result[i].PEDIDOS_FUTUROS + '</span></td>'; //Pedidos futuros
        str_html += '</tr>';
    }
    str_html += '</tbody>';
    str_html += '</table>';
    str_html += '</div>';

    $('#PA_Nivel3_' + indice).html(str_html);
}

function checkboxEstoquePA(num) {
    if ($('#check_estoquePA_' + num).is(':checked'))
        $('#check_estoquePA_' + num).prop("checked", false);
    else
        $('#check_estoquePA_' + num).prop("checked", true);
}

function checkboxFuturosPA(num) {
    if ($('#check_futurosPA_' + num).is(':checked'))
        $('#check_futurosPA_' + num).prop("checked", false);
    else
        $('#check_futurosPA_' + num).prop("checked", true);
}

function limparCheckboxEstoque() {
    $('.check_estoquePA').prop("checked", false);
    $('.check_futurosPA').prop("checked", false);

}

/*############################################# ESTOQUE PI - PRODUTOS INTERMEDIÁRIOS ######################################*/
function EstoqueINT(nivel, grupo, indice = -1, grp_id = '-1') {
    var path = "/plugandplay/APS/ObterEstoqueINT";
    $.ajax({
        url: path,
        type: "GET",
        data: { nivel: nivel, grupo: grupo, grp_id: grp_id },
        dataType: "json",
        success: function (result) {
            if (nivel == 'N1')
                gerarEstoqueINT_HTML(result.listaN1);
            else if (nivel == 'N2') {
                gerarGrupoPI_HTML(result.listaN2, indice);
            }
            else if (nivel == 'N3') {
                //Nivel 3 Estoque intermediário.
                gerarProdutoPI_HTML(result.listaN3, indice);
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}

function gerarEstoqueINT_HTML(result) {
    var str_html = "";

    //Modal de Estoque Lote e Pedidos Futuros.
    str_html += '<div id="ModalEstoquePI" style="z-index:9997 !important;" class="modal fade" role="dialog">';
    str_html += '<div style="width: 85%;" class="modal-dialog"></div>';
    str_html += '</div>';
    //Modal de Quantidade
    str_html += '<div id="ModalQuantidadePI" style="z-index:9998 !important;" class="modal fade" role="dialog">';
    str_html += '<div style="width:55%;" class="modal-dialog"></div>';
    str_html += '</div>';

    str_html += '<div class="panel-body">'
    str_html += '<div class="col-lg-12" id="Estoque">';
    for (var i = 0; i < result.length; i++) {
        var grp_id = 0;
        var temp_contem = 'CX';
        if (result[i].GRP_TIPO == "CHAPA") {
            grp_id = 2;
            temp_contem = 'CH';
        }

        var grp_id = result[i].GRP_TIPO == "CHAPA" ? "2" : "0";

        str_html += '<div class="col-lg-12" id="EstoqueINT_' + i + '">';
        str_html += '<div class="panel panel-default">';

        var header1 = '<div onclick="javascript:EstoqueINT(\'N2\', \'CH\', ' + i + ', \'' + grp_id + '\');" class="panel-heading PI_N2_' + i + ' PI_N2_' + temp_contem + '_' + i + '_' + grp_id + '" role="tab" id="PI_' + i + '">';
        header1 += '<a style="width:100%; padding:10px" data-toggle="collapse" data-parent="#EstoqueINT_' + i + '" href="#INT_Nivel1_' + i + '" aria-expanded="false" aria-controls="collapseOne-3" class="collapsed">';
        header1 += '<div style="display:flex">';

        var tag_i = '<i class="fa fa-check"></i>' + result[i].GRP_TIPO + '&emsp;';
        var span1 = '<span class="label label-success label-aps-n3" id="pi_dis_' + i + '" title="Disponível">' + result[i].DISPONIVEL + ' Disp.</span>&emsp;';
        var span2 = '<span class="label label-danger label-aps-n3" id="pi_sob_' + i + '" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + ' Sobra de Prod.</span>&emsp;';
        var span3 = '<span class="label label-danger label-aps-n3" id="pi_exp_' + i + '" title="Sobra Expedição">' + result[i].SOBRA_EXPEDICAO + ' Sobra de Exp.</span>&emsp;';
        var span4 = '<span class="label label-danger label-aps-n3" id="pi_dev_' + i + '" title="Devolvidas">' + result[i].DEVOLUCAO + ' Devolv.</span>&emsp;';
        var span5 = '<span class="label label-success label-aps-n3" id="pi_com_' + i + '" title="Compromissado">' + result[i].COMPROMISSADO + ' Compromissado</span>&emsp;';
        var span7 = '<span class="label label-secondary label-aps-n3" id="pi_ret_' + i + '" title="Estoque Retido"> ' + result[i].SALDO_RETIDO + ' Retido</span>&emsp;';
        var span6 = '<span class="label label-info label-aps-n3" id="pi_fut_' + i + '" title="Ped. Futuros">' + result[i].PEDIDOS_FUTUROS + ' Pedidos Fut.</span>&emsp;';

        header1 += '<div style="width:10%">' + tag_i + '</div>' + '<div style="width:90%">' + span1 + span2 + span3 + span4 + span5 + span7 + span6 + '</div>';
        header1 += '</div></a>';
        header1 += '</div>'

        var body1 = '<div id="INT_Nivel1_' + i + '" class="panel-collapse collapse" role="tabpane1" aria-labelledby="headingOne4" aria-expanded="false">';
        body1 += '<div class="panel-body" id="PI_grupo_' + i + '">';

        body1 += '</div>';
        body1 += '</div>';

        str_html += header1 + body1;
        str_html += '</div>';
        str_html += '</div>';
    }
    str_html += '</div>';
    str_html += '</div>';

    $('#estoqueIntArea').html(str_html);
}

function gerarGrupoPI_HTML(result, indice) {
    var str_html = "";

    for (var i = 0; i < result.length; i++) {
        str_html += '<div id="INT_Nivel2_' + indice + '_' + i + '">';
        str_html += '<div class="panel panel-default">';
        str_html += '<div onclick="javascript:EstoqueINT(\'N3\',\'CXCN\', \'' + indice + '_' + i + '\', \'' + result[i].GRP_ID + '\');" class="panel-heading PI_N3_' + indice + '_' + i + ' PI_N3_' + result[i].GRP_ID + '" role="tab">';
        str_html += '<a style="width:100%; padding:10px" data-toggle="collapse" data-parent="#accordCargMaq" href="#INT_Nivel3_' + indice + '_' + i + '" aria-expanded="false" aria-controls="collapseOne-3" class="collapsed">';
        str_html += '<div style="display:flex">';

        var tag_i = '<i class="fa fa-check"></i>' + result[i].GRP_ID + ' - ' + result[i].GRP_DESCRICAO + '&emsp;';
        var span1 = '<span id="pi_dis_' + indice + '_' + i + '" class="label label-success label-aps-n4" title="Disponível">' + result[i].DISPONIVEL + '</span>&emsp;';
        var span2 = '<span id="pi_sob_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + '</span>&emsp;';
        var span3 = '<span id="pi_exp_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Sobra de Expedição">' + result[i].SOBRA_EXPEDICAO + '</span>&emsp;';
        var span4 = '<span id="pi_dev_' + indice + '_' + i + '" class="label label-danger label-aps-n4" title="Devolvidas">' + result[i].DEVOLUCAO + '</span>&emsp;';
        var span5 = '<span id="pi_com_' + indice + '_' + i + '" class="label label-success label-aps-n4" title="Compromissado">' + result[i].COMPROMISSADO + '</span>&emsp;';
        var span7 = '<span id="pi_ret_' + indice + '_' + i + '" class="label label-secondary label-aps-n4" title="Estoque Retido">' + result[i].SALDO_RETIDO + '</span>&emsp;';
        var span6 = '<span id="pi_fut_' + indice + '_' + i + '" class="label label-info label-aps-n4" title="Pedidos Futuros">' + result[i].PEDIDOS_FUTUROS + '</span>&emsp;';

        str_html += '<div style="width:25%">' + tag_i + '</div>' + '<div style="width:75%">' + span1 + span2 + span3 + span4 + span5 + span7 + span6 + '</div>';

        str_html += '</div></a>';
        str_html += '</div>';
        str_html += '</div>';
        str_html += '</div>';

        //Corpo do item
        str_html += '<div id="INT_Nivel3_' + indice + '_' + i + '" class="panel-collapse collapse" role="tabpane1" aria-labelledby="headingOne4" aria-expanded="false">';
        str_html += '<div id="Produto_' + indice + '_' + i + '">';
        str_html += '</div>';
        str_html += '</div>';
    }

    $('#PI_grupo_' + indice).html(str_html);
}

function gerarProdutoPI_HTML(result, indice) {
    var str_html = '<div style="overflow: auto">';
    str_html += '<table class="table table-bordered table-striped text-center table_estoquePI">';
    str_html += '<thead>';
    str_html += '<tr>';

    str_html += '<td>PRO ID</td>';
    str_html += '<td>PRO DESCRIÇÃO</td>';
    str_html += '<td>DISPONÍVEL</td>';
    str_html += '<td>SOBRA DE PRODUÇÃO</td>';
    str_html += '<td>SOBRA DE EXPEDIÇÃO</td>';
    str_html += '<td>DEVOLVIDOS</td>';
    str_html += '<td>COMPROMISSADO</td>';
    str_html += '<td>ESTOQUE RETIDO</td>';
    str_html += '<td>PEDIDOS FUTUROS</td>';

    str_html += '</tr>';
    str_html += '</thead>';
    str_html += '<tbody>';
    for (let i = 0; i < result.length; i++) {
        var objeto = JSON.stringify(result[i]).replaceAll('"', '&quot;');
        str_html += '<tr onclick="javascript:abrirModalEstoque(' + objeto + ', \'PI\', \'PI_N3_' + indice + '_' + i + '\')">';
        str_html += '<td>' + result[i].PRO_ID + '</td>'; //Pro id
        str_html += '<td>' + result[i].PRO_DESCRICAO + '</td>'; //Pro nome
        str_html += '<td><span id="pi_dis_' + indice + '_' + i + '" class="label label-success" title="Disponível">' + result[i].DISPONIVEL + '</span></td>'; //Disponivel
        str_html += '<td><span id="pi_sob_' + indice + '_' + i + '" class="label label-danger" title="Sobra de Produção">' + result[i].SOBRA_PRODUCAO + '</span></td>'; //Sobra de produção
        str_html += '<td><span id="pi_exp_' + indice + '_' + i + '" class="label label-danger" title="Sobra de Expedição">' + result[i].SOBRA_EXPEDICAO + '</span></td>'; //Sobra de expedição
        str_html += '<td><span id="pi_dev_' + indice + '_' + i + '" class="label label-danger" title="Devolvidas">' + result[i].DEVOLUCAO + '</span></td>'; //Devolvidos
        str_html += '<td><span id="pi_com_' + indice + '_' + i + '" class="label  label-success" title="Compromissado">' + result[i].COMPROMISSADO + '</span></td>'; //Compromissados
        str_html += '<td><span id="pi_ret_' + indice + '_' + i + '" class="label label-secondary" title="Estoque Retido">' + result[i].SALDO_RETIDO + '</span></td>'; //Pedidos retidos
        str_html += '<td><span id="pi_fut_' + indice + '_' + i + '" class="label label-info" title="Pedidos Futuros">' + result[i].PEDIDOS_FUTUROS + '</span></td>'; //Pedidos futuros
        str_html += '</tr>';
    }
    str_html += '</tbody>';
    str_html += '</table>';
    str_html += '</div>';

    $('#INT_Nivel3_' + indice).html(str_html);
}

/*############################################# ESTOQUE MP - MATÉRIA PRIMA ################################################*/
function pesquisarMP() {
    var path = "/plugandplay/APS/ObterEstoqueMP";
    var dataDe = $('#dataDeMP').val();
    var dataAte = $('#dataAteMP').val();

    $.ajax({
        url: path,
        type: "GET",
        data: { dataDe: dataDe, dataAte: dataAte },
        dataType: "json",
        success: function (result) {
            gerarEstoqueMP_HTML(result);
        },
        error: function () {
            return "Erro não tratado!";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });

}

function gerarEstoqueMP_HTML(result) {
    //#tabelaEstoqueMP
    console.log(result);

    $('#tabelaEstoqueMP2').remove();

    var str_html = '<div id="tabelaEstoqueMP2" class="panel panel-default">';
    str_html += '<div class="panel-heading" role="tab" id="headingOne5"></div>';
    str_html += '<div class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne5" aria-expanded="true">';
    str_html += '<div class="panel-body">';
    str_html += '<div>';

    str_html += '<div class="flow aps" style="display: flex; width: 100%; background-color: #EEE">';
    //Colunas da tabela
    str_html += '<div style="width:10%; display:flex" title="GRUPO ID">GRP ID</div>';
    str_html += '<div style="width:10%; display:flex" title="DESCRIÇÃO DO PRODUTO">PRO DESCRIÇÃO</div>';
    str_html += '<div style="width:10%; display:flex" title="ID DO COMPONENTE">PRO ID COMPONENTE</div>';
    str_html += '<div style="width:10%; display:flex" title="">CONSUMO PREVISTO</div>';
    str_html += '<div style="width:10%; display:flex" title="">CONSUMO HOJE</div>';
    str_html += '<div style="width:10%; display:flex" title="">CONSUMO AMANHÃ</div>';
    str_html += '<div style="width:10%; display:flex" title="CONSUMO NOS PRÓXIMOS CINCO DIAS">CONSUMO CINCO</div>';
    str_html += '<div style="width:10%; display:flex" title="CONSUMO NOS PRÓXIMOS DEZ DIAS">CONSUMO DEZ</div>';
    str_html += '<div style="width:10%; display:flex" title="CONSUMO NOS PRÓXIMOS QUINZE DIAS">CONSUMO QUINZE</div>';
    str_html += '<div style="width:10%; display:flex" title="">CONSUMO DO PERÍODO</div>';
    str_html += '</div>';

    var cor1 = '#F9F9F9';
    var cor2 = '#FFFFFF';
    var corAtual = cor1;

    result.listaMP.forEach(function (item) {
        corAtual = corAtual == cor1 ? cor2 : cor1;
        str_html += '<div class="flow_aps" style="display: flex; width: 100%; background-color:' + corAtual + '">';


        str_html += '<div style="width:10%; display:flex;">' + item.GRP_ID + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.PRO_DESCRICAO + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.PRO_ID_COMPONENTE + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_PREVISTO + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_HOJE + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_AMANHA + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_CINCO + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_DEZ + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_QUINZE + '</div>';
        str_html += '<div style="width:10%; display:flex;">' + item.CONSUMO_DO_PERIODO + '</div>';

        str_html += '</div>';
    })

    str_html += '</div>';
    str_html += '</div>';
    str_html += '</div>';
    str_html += '</div>';
    $('#tabelaEstoqueMP').append(str_html);
}

function exportarExcelMP() {

    var UrlBase = '/plugandplay/aps/CriarArquivoMP';
    //Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: UrlBase,
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.msg != "") {
                alert(result.msg);
            } else {
                window.open(result.downloadUrl);
            }
        },
        error: OnError,
        complete: function () {
            //Carregando.fechar();
        }
    })
}

/*############################################# OUTROS ####################################################################*/

function aproveitarEstoque(tipo_estoque, indice) {
    var lc_estoque_lote = [];
    var lc_pedidos_futuros = [];

    for (var i = 0; i < estoque_lote.length; i++)
        if ($('#check_estoquePA_' + i).is(':checked'))
            lc_estoque_lote.push(estoque_lote[i]);

    for (var i = 0; i < pedidos_futuros.length; i++)
        if ($('#check_futurosPA_' + i).is(':checked'))
            lc_pedidos_futuros.push(pedidos_futuros[i]);

    if (lc_estoque_lote.length == 0 || lc_pedidos_futuros.length == 0)
        alert('Selecione pelo menos um Lote de produto e somente um Pedido futuro para fazer o aproveitamento.');
    else if (lc_pedidos_futuros.length > 1)
        alert('Selecione somente um Pedido futuro para fazer o aproveitamento.');
    else {
        //id="ModalQuantidadePA"
        var str_html = '<div class="modal-content">';
        var header = '<div class="modal-header">';
        header += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        header += '<h5 class="modal-title">Informe a quantidade para cada lote:</h5>';
        header += '</div>';

        var body = '<div class="modal-body">';
        body += '<div style="overflow:auto;">';
        body += '<table class="table table-striped table-bordered table-hover table_estoquePA">';

        body += '<thead>';
        body += '<tr>';
        body += '<td>LOTE</td>';
        body += '<td>SUB LOTE</td>';
        body += '<td>SALDO</td>';
        body += '<td>PEDIDO</td>';
        //body += '<td>QTD. A DESIGNAR</td>'; V2 - Proxima versão
        body += '</tr>';
        body += '</thead><tbody>';
        for (var i = 0; i < lc_estoque_lote.length; i++) {
            body += '<tr>';
            body += '<td>' + lc_estoque_lote[i].MOV_LOTE + '</td>';
            body += '<td>' + lc_estoque_lote[i].MOV_SUB_LOTE + '</td>';
            body += '<td>' + lc_estoque_lote[i].SALDO + '</td>';
            body += '<td>' + lc_pedidos_futuros[0].ORD_ID + '</td>';
            //body += '<td><input type="input" placeholder="Digite:"></td>'; //V2 - Proxima versão
            body += '</tr>';
        }
        body += '</tbody></table>';
        body += '</div>';
        body += '</div>';

        var footer = '<div class="modal-footer">';
        var lc_estoque_lote = JSON.stringify(lc_estoque_lote).replaceAll('"', '&quot;');
        var lc_pedidos_futuros = JSON.stringify(lc_pedidos_futuros).replaceAll('"', '&quot;');

        footer += '<button type="button" class="btn btn-default" data-dismiss="modal" onclick="aplicarAproveitamento(' + lc_estoque_lote + ', ' + lc_pedidos_futuros + ', \'' + indice + '\', \'' + tipo_estoque + '\')">Aplicar</button>';
        footer += '<button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>';
        footer += '</div>';


        str_html += header + body + footer;
        str_html += '</div>';

        $('#ModalQuantidade' + tipo_estoque + ' div').html(str_html);
        $('#ModalQuantidade' + tipo_estoque).modal('show');

    }
    //PA_mov_lote_5;
    //PA_ord_id_5;

}

function aplicarAproveitamento(lc_estoque_lote, lc_pedidos_futuros, indice, tipo_estoque) {
    lc_estoque_lote = JSON.stringify(lc_estoque_lote);
    lc_pedidos_futuros = JSON.stringify(lc_pedidos_futuros);
    
    var path = '/plugandplay/APS/setReserva';
    $.ajax({
        url: path,
        type: "POST",
        data: {
            lc_estoque_lote: lc_estoque_lote,
            lc_pedidos_futuros: lc_pedidos_futuros
        },
        dataType: "json",
        success: function (result) {
            $('#ModalEstoque' + tipo_estoque_temp + ' div').html('');
            if (result.status == 'ERRO') {
                mostraDialogo(result.msg, 'danger', 3000);
            }
            else {
                if (result.idEtiquetas != 'undefined') {
                    var url_final = '/PlugAndPlay/ReportEtiqueta/EtiquetaIndividualLoteProduto?etiqueta=' + result.idEtiquetas;
                    window.open(url_final);
                }
                mostraDialogo(result.msg, 'success', 3000);

                AtualizarInfoEstoque(indice, tipo_estoque);
                //objeto_temp;
                //tipo_estoque_temp;

            }
        },
        error: function () {
            alert('Exception');
        },
        complete: function (r) {
            Carregando.fechar(r);
            abrirModalEstoque(objeto_temp, tipo_estoque_temp);
        }
    });
    
}

function AtualizarInfoEstoque(indice, tipo_estoque) {
    //N3_0_0_0
    var grp_id = indice.split('_')[2];
    var tipoLow = tipo_estoque.toLowerCase();
    tipo_estoque = tipo_estoque == 'PI' ? 'INT' : 'PA'; //Pequeno desavença entre PI e INT, em uns lugares é PI e em outros é INT

    var grupo = '.' + indice.split('_')[0] + '_N2_' + indice.split('_')[2];
    var temp = $(grupo).attr('class');
    temp = temp.split(' ')[2];
    temp = temp.split('_');

    var path = "/plugandplay/APS/ObterEstoque" + tipo_estoque;
    $.ajax({
        url: path,
        type: "GET",
        data: { nivel: "N2", grupo: temp[2], grp_id: temp[3] },
        dataType: "json",
        success: function (result) {

            var n1_compromissado = 0;
            var n1_devolucao = 0;
            var n1_disponivel = 0;
            var n1_ped_fut = 0;
            var n1_saldo_retido = 0;
            var n1_sobra_exp = 0;
            var n1_sobra_prod = 0;

            var ind = indice.split('_')[2];
            var temp = indice.split('_')[3];

            for (let i = 0; i < result.listaN2.length; i++) {

                n1_compromissado += result.listaN2[i].COMPROMISSADO;
                n1_devolucao += result.listaN2[i].DEVOLUCAO;
                n1_disponivel += result.listaN2[i].DISPONIVEL;
                n1_ped_fut += result.listaN2[i].PEDIDOS_FUTUROS;
                n1_saldo_retido += result.listaN2[i].SALDO_RETIDO;
                n1_sobra_exp += result.listaN2[i].SOBRA_EXPEDICAO;
                n1_sobra_prod += result.listaN2[i].SOBRA_PRODUCAO;

                if (temp == i) {

                    $('#' + tipoLow + '_com_' + ind + '_' + temp).text(n1_compromissado);
                    $('#' + tipoLow + '_dev_' + ind + '_' + temp).text(n1_devolucao);
                    $('#' + tipoLow + '_dis_' + ind + '_' + temp).text(n1_disponivel);
                    $('#' + tipoLow + '_fut_' + ind + '_' + temp).text(n1_ped_fut);
                    $('#' + tipoLow + '_ret_' + ind + '_' + temp).text(n1_saldo_retido);
                    $('#' + tipoLow + '_sob_' + ind + '_' + temp).text(n1_sobra_prod);
                    $('#' + tipoLow + '_exp_' + ind + '_' + temp).text(n1_sobra_exp);
                }
            }
            
            $('#' + tipoLow+'_com_' + ind).text(n1_compromissado);
            $('#' + tipoLow+'_dev_' + ind).text(n1_devolucao);
            $('#' + tipoLow+'_dis_' + ind).text(n1_disponivel);
            $('#' + tipoLow+'_fut_' + ind).text(n1_ped_fut);
            $('#' + tipoLow+'_ret_' + ind).text(n1_saldo_retido);
            $('#' + tipoLow+'_sob_' + ind).text(n1_sobra_prod);
            $('#' + tipoLow+'_exp_' + ind).text(n1_sobra_exp);
           

        },
        error: function () {

        }
    });

    var temp = indice.split('_');
    temp = '.' + indice.split('_')[0] + '_N3_' + temp[2] + '_' + temp[3];
    temp = $(temp).attr('class');
    temp = temp.split(' ')[2];
    var grupo = temp.split('_')[2];

    $.ajax({
        url: path,
        type: "GET",
        data: { nivel: "N3", grupo: grupo, grp_id: grupo },
        dataType: "json",
        success: function (result) {
            var id = indice.split('N3')[1];
            var pos = id.split('_')[3];

            $('#' + tipoLow + '_com' + id).text(result.listaN3[pos].COMPROMISSADO);
            $('#' + tipoLow + '_dev' + id).text(result.listaN3[pos].DEVOLUCAO);
            $('#' + tipoLow + '_dis' + id).text(result.listaN3[pos].DISPONIVEL);
            $('#' + tipoLow + '_fut' + id).text(result.listaN3[pos].PEDIDOS_FUTUROS);
            $('#' + tipoLow + '_ret' + id).text(result.listaN3[pos].SALDO_RETIDO);
            $('#' + tipoLow + '_sob' + id).text(result.listaN3[pos].SOBRA_PRODUCAO);
            $('#' + tipoLow + '_exp' + id).text(result.listaN3[pos].SOBRA_EXPEDICAO);
         
        },
        error: function () {

        }
    });

}

var estoque_lote = [];
var pedidos_futuros = [];
var objeto_temp;
var tipo_estoque_temp;
function abrirModalEstoque(objeto, tipo_estoque, indice) {
    var path = "/plugandplay/APS/ObterLotesPedFut" + tipo_estoque;
    //Carregando.abrir('Processando....');
    $.ajax({
        url: path,
        type: "GET",
        data: { pro_id: objeto.PRO_ID },
        dataType: "json",
        success: function (result) {
            estoque_lote = result.estoque_lote;
            pedidos_futuros = result.pedidos_futuros;
            objeto_temp = objeto;
            tipo_estoque_temp = tipo_estoque;

            var str_modal = '';
            str_modal += '<div class="modal-content">';

            //Cabeçalho
            str_modal += '<div class="modal-header">';
            {
                str_modal += '<span class="modal-title">' + objeto.PRO_ID + ' -</span>&emsp;';
                str_modal += '<span class="modal-title">' + objeto.PRO_DESCRICAO + '</span>&emsp;';
                str_modal += '<span class="label label-success" title="Disponível">' + objeto.DISPONIVEL + '</span>&emsp;';
                str_modal += '<span class="label label-danger" title="Sobra de Produção">' + objeto.SOBRA_PRODUCAO + '</span>&emsp;';
                str_modal += '<span class="label label-danger" title="Sobra de Expedição">' + objeto.SOBRA_EXPEDICAO + '</span>&emsp;';
                str_modal += '<span class="label label-danger" title="Devolvidas">' + objeto.DEVOLUCAO + '</span>&emsp;';
                str_modal += '<span class="label label-success" title="Compromissado">' + objeto.COMPROMISSADO + '</span>&emsp;';
                str_modal += '<span class="label label-info" title="Pedidos Futuros">' + objeto.PEDIDOS_FUTUROS + '</span>&emsp;';
                str_modal += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
                str_modal += '</div>';
            }
            //Corpo
            str_modal += '<div class="modal-body" style="display:flex;justify-content:space-evenly">';
            {
                //DIV LOTES DO PRODUTO
                str_modal += '<div style="width:40%; overflow:auto;">';
                str_modal += '<p>Lotes do Produto</p>';
                str_modal += '<div class="input-group">';
                str_modal += '<input name="lista" class="form-control">';
                str_modal += '<span class="input-group-addon">';
                str_modal += '<button class="fa fa-search" style="background:transparent;border:none"></button>';
                str_modal += '</span>';
                str_modal += '</div>';
                //Vai a tabela de itens Estoque lote.
                str_modal += '<div style="height: 380px; overflow:auto;">';

                str_modal += '<table style="font-size:11px;" class="table-striped table-bordered table-hover table_estoquePA">';
                str_modal += '<thead>';
                str_modal += '<tr>';
                str_modal += '<th></th>';
                str_modal += '<th>LOTE</th>';
                str_modal += '<th>SUB LOTE</th>';
                str_modal += '<th>SALDO</th>';
                str_modal += '<th>QTD. RESERVA</th>';
                str_modal += '<th>ENDEREÇO</th>';
                str_modal += '<th>OPÇÕES</th>';
                str_modal += '</tr>';
                str_modal += '</thead>';

                str_modal += '<tbody>';
                for (var i = 0; i < result.estoque_lote.length; i++) {
                    str_modal += '<tr>';
                    str_modal += '<td><input type="checkbox" class="check_estoquePA" id="check_estoquePA_' + i + '"></td>';
                    str_modal += '<td onclick="checkboxEstoquePA(' + i + ')">' + result.estoque_lote[i].MOV_LOTE + '</td>';
                    str_modal += '<td onclick="checkboxEstoquePA(' + i + ')">' + result.estoque_lote[i].MOV_SUB_LOTE + '</td>';
                    str_modal += '<td onclick="checkboxEstoquePA(' + i + ')">' + result.estoque_lote[i].SALDO + '</td>';
                    str_modal += '<td onclick="checkboxEstoquePA(' + i + ')">' + result.estoque_lote[i].QTD_RESERVA + '</td>';
                    str_modal += '<td onclick="checkboxEstoquePA(' + i + ')">' + result.estoque_lote[i].MOV_ENDERECO + '</td>';
                    str_modal += "<td><div title='Desmontar lote' class='btn btn-default btn-xs' onclick='abrirTelaDesmontagemLotes(`" + JSON.stringify(result.estoque_lote[i]) + "`)'><i class='fa fa-object-ungroup'></i></div></td>";
                    str_modal += '</tr>';
                }
                str_modal += '</tbody>';
                str_modal += '</table>';
                str_modal += '</div>';
                str_modal += '</div>';


                //DIV BOTÕES
                str_modal += '<div align="center" style="width:15%; overflow:auto;">';
                str_modal += '<div style="display:column;">';


                str_modal += '<div style="padding:15px"><button type="button" class="btn btn-light" onclick="aproveitarEstoque(\'' + tipo_estoque + '\', \'' + indice + '\')">Aproveitar</button></div>';
                str_modal += '</div>';
                str_modal += '</div>'; //fecha a div maior

                //DIV PEDIDOS FUTUROS
                str_modal += '<div style="width:45%; overflow:auto;">';
                str_modal += '<p>Pedidos Futuros</p>';


                str_modal += '<div class="input-group">';
                str_modal += '<input name="lista" class="form-control">';
                str_modal += '<span class="input-group-addon">';
                str_modal += '<button class="fa fa-search" style="background:transparent;border:none"></button>';
                str_modal += '</span>';
                str_modal += '</div>';
                //Vai a tabela de itens
                str_modal += '<div style="height: 380px; overflow:auto;">';
                str_modal += '<table style="font-size:11px;" class="table-striped table-bordered table-hover table_futurosPA">';
                str_modal += '<thead>';
                str_modal += '<tr>';
                str_modal += '<th></th>';
                str_modal += '<th>PEDIDO</th>';
                str_modal += '<th>TIPO PEDIDO</th>';
                str_modal += '<th>CLIENTE</th>';
                str_modal += '<th title="QUANTIDADE FALTANTE">QTD. FALTANTE</th>';
                str_modal += '<th title="QUANTIDADE DO PEDIDO">QTD. PEDIDO</th>';
                str_modal += '<th title="SALDO EM ESTOQUE">SALDO EM ESTOQUE</th>';
                str_modal += '<th>ENTREGA DE</th>';
                str_modal += '<th>ENTREGA ATÉ</th>';
                str_modal += '<th>TOLERÂNCIA MAIS</th>';
                str_modal += '<th>TOLERÂNCIA MENOS</th>';
                str_modal += '</tr>';
                str_modal += '</thead>';

                str_modal += '<tbody>';
                for (var i = 0; i < result.pedidos_futuros.length; i++) {
                    var faltante = parseInt(result.pedidos_futuros[i].ORD_QUANTIDADE) - parseInt(result.pedidos_futuros[i].SALDO_ESTOQUE);

                    str_modal += '<tr>';
                    str_modal += '<td><input type="checkbox" class="check_futurosPA" id="check_futurosPA_' + i + '"></td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].ORD_ID + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].ORD_TIPO_DESCRICAO + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].CLI_NOME + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + faltante + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].ORD_QUANTIDADE + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].SALDO_ESTOQUE + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + formatarData(result.pedidos_futuros[i].ORD_DATA_ENTREGA_DE) + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + formatarData(result.pedidos_futuros[i].ORD_DATA_ENTREGA_ATE) + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].ORD_TOLERANCIA_MAIS + '</td>';
                    str_modal += '<td onclick="checkboxFuturosPA(' + i + ')">' + result.pedidos_futuros[i].ORD_TOLERANCIA_MENOS + '</td>';
                    str_modal += '</tr>';
                }
                str_modal += '</tbody>';
                str_modal += '</table>';
                str_modal += '</div>'; //fecha a div table

                str_modal += '</div>';//Fecha o body
                str_modal += '</div>';//Fecha o body
            }
            //Rodapé
            str_modal += '<div class="modal-footer">';
            {
                str_modal += '<button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>';
                str_modal += '</div>';
            }

            str_modal += '</div>';

            $('#ModalEstoque' + tipo_estoque + ' div').html(str_modal);
            $('#ModalEstoque' + tipo_estoque).modal('show');
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });


}

String.prototype.replaceAll = String.prototype.replaceAll || function (needle, replacement) {
    return this.split(needle).join(replacement);
};

$(document).ready(function () {
    //carrega os estoques ao iniciar a pagina
    EstoquePA('N1', 'CXCN');
    EstoqueINT('N1', 'CXCN');
});

/** Recebe uma string contendo um objeto em string json, converte essa string em um objeto e passa para o método DesmontarLote 
 * que irá montar um link para abrir a tela de desmontagem de lotes com os campos (MOV_LOTE_ORIGEM, MOV_SUB_LOTE_ORIGEM, MOV_LOTE_DESTINO, MOV_SUB_LOTE_DESTINO, PRO_ID_DESTINO, ORD_ID e MOV_ENDERECO) preenchidos. 
 * */
function abrirTelaDesmontagemLotes(json_string){
    Carregando.abrir('Processando....');
    $.ajax({
        url: "/plugandplay/APS/DesmontarLote",
        type: "POST",
        data: { item_json: json_string },
        dataType: "json",
        success: function (result) {
            executarProtocolos(result);
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao pesquisar os dados!</strong><br>ERRO ao tentar pesquisar os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        },
        complete: function(){
            Carregando.fechar();
        }
    });
}