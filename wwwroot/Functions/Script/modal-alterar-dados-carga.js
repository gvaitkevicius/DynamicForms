
function alterarCarga(carga_id) {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.Carga';
    var str = global_modal(carga_id, namespace);;
    if (str != null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

function atualizarCaminhao(data_inicio, carga_id, tipo) {
    if (tipo == 1) { //A data não foi alterada, portanto atualizar somente os valores.
        TabelaFilaExpedicao.carregarTabela(undefined, undefined, undefined, carga_id, tipo);
    }
    else { //A data é diferente, validar se existe ou não uma data assim.
        tipo = 3;
        var i = 0;
        var sair = false;
        data_inicio = data_inicio.split('T')[0].replace('-', '').replace('-', '');
        while ($('.exped' + i).length != 0 && !sair) {
            var data_exp = $('.exped' + i).attr('id');
            data_exp = data_exp.split("divexpedicao")[1];
            if (data_inicio == data_exp) //Existe uma data igual a nova data ja na expedicao?
                sair = true;
            else
                i++;
        }

        if (sair)
            tipo = 2;
        TabelaFilaExpedicao.carregarTabela(undefined, undefined, undefined, carga_id, tipo, i);
    }
}

function atualizarSomenteDiv(lista_dados, carga_id) {

    var dados = "";
    $.ajax({
        url: '/plugandplay/aps/ObterVeiculoTransportadora',
        type: "GET",
        data: { carga_id: carga_id },
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            console.log('Show de bola');
        },
        error: function () {
            dados = lista_dados['caR_ID'] + ', ' + lista_dados['tiP_ID'] + ', ' + lista_dados['veI_PLACA'] + ', ' + lista_dados['trA_ID'];
        }
    });

    if (dados == "")
        dados = lista_dados['caR_ID'] + ', ' + lista_dados['tiP_ID'] + ', ' + lista_dados['veI_PLACA'] + ', ' + lista_dados['trA_ID'];

    //Pegar no Banco
    /**
     * tiP_DESCRICAO - pesquisar por tiP_ID
     * trA_NOME - trA_ID
     */

    //Parte do header;
    var i = '<i class="fa fa-check" style="color:#f44336;"></i>' + dados;
    var subdiv1 = '<div class="col-xs-12"><br>' + i + '</div>';

    var a1 = '<a class="box_toggle fa fa-chevron-down" data-toggle="modal" href="javascript:Show3d(\'' + carga_id + '\');"></a>';
    var a2 = '<a class="box_setting fa fa-edit" data-toggle="modal" onclick="alterarCarga(\'' + carga_id + '\');" href=""></a>';
    var a3 = '<a class="box_close fa fa-times"></a>';
    var subdiv2 = '<div class="actions panel_actions pull-right">' + a1 + a2 + a3 + '</div>';

    var header = '<header class="panel_header">' + subdiv1 + subdiv2 + '</header>';

    //Parte do body
    var div = "";

    //Resultado final;
    var str = header + div;
    str = '<section class="box">' + str + '</section>';
    $('#carga' + carga_id).html(str);
}

function selectList(id, path, action, status) {
    var set = "";
    var get = action;
    $.ajax({
        type: 'GET',
        url: path + get,
        success: function (resultado) {
            var area = $('#divResultSelect_' + id);
            if (resultado.length > 0) {
                var slList = [];
                var cont = 0;
                var sl = ' selected';
                resultado.forEach(function (p) {
                    var js = "onchange=\"$('#slct_i_" + p.Id + "_" + cont + "').val(this.value);javascript:clickListSelect('" + p.Id + "','" + p.Descricao + "'," + cont + ");\"";
                    //var auxStr = '<option  id="slct_i_' + p.Id + "_" + cont + '"' + js + '" value="' + p.Id + '">"' + p.Descricao + '"</option>';
                    if (status === p.Id) {
                        var tagOption = $('<option  id="slct_i_' + p.Id + "_" + cont + '"' + sl + '" value="' + p.Id + '">"' + p.Descricao + '"</option>').attr('data-id', p.Id).text(p.Descricao);
                    } else {
                        tagOption = $('<option  id="slct_i_' + p.Id + "_" + cont + '"  value="' + p.Id + '">"' + p.Descricao + '"</option>').attr('data-id', p.Id).text(p.Descricao);
                    }
                    slList.push(tagOption);
                    cont++;
                });
                area.html($('<select class="form-control">').html(slList));
                //area.html($('<select class="form-control col-md-6">', ).html(slList));
                area.show();
            }
            else {
                var js = "onchange=\"$('#li_" + id + "');javascript:clickListPesq('" + id + "');\"";
                area.html($('<option>').html([
                    $('<option  id="slct_i_' + id + ' "' + js + '>').text('Nenhum resultado')
                ]));
                area.show();
            }
        },
        error: function (e) {
            alert(e);
        }
    });

}

function clickListSelect(idItem, idLista, cont) {

    if (idLista !== undefined && idLista !== '') {
        //$('#li_' + idLista+'').val(proID).attr('data-id', proID);
        var tt = '#hicab_' + idItem.slice(-1);
        $(tt).val(idItem);
        $(tt).text(idLista);
        $(tt).val(idItem);
        $('#' + idItem).val(idItem);
        tt = '#divResultSelect_' + idItem;
    }
    else {
        $('#' + idItem + '').val("").attr('data-id', '');
    }
    //$(tt).tab('hide');
    $(tt).hide();

}
function inputPesquisar(eve, id, chaveBusca, path, action) {
    var patch = path;
    var set = "";
    var get = action;
    if (eve.which === 32) {
        $.ajax({
            type: 'GET',
            url: patch + get,
            data: { pesquisa: chaveBusca },
            success: function (resultado) {
                var area = $('#divResultPesq_' + id);
                if (resultado.length > 0) {
                    var liList = [];
                    resultado.forEach(function (p) {
                        var js = "onclick=\"$('#li_" + p.id + "');javascript:clickListPesq('" + id + "','" + p.id + "');\"";
                        liList.push($('<li class="list-group-item" id="li_' + p.id + '"' + js + '>' + p.descricao).attr('data-id', p.id).text(p.descricao));
                    });
                    area.html($('<ul>', { class: 'list-group' }).html(liList));
                    area.show();
                }
                else {
                    var js = "onclick=\"$('#li_" + id + "');javascript:clickListPesq('" + id + "');\"";
                    area.html($('<ul>', { class: 'list-group' }).html([
                        $('<li class="list-group-item" id="li_' + id + ' "' + js + '>').text('Nenhum resultado')
                    ]));
                    area.show();
                }
            },
            error: function (e) {
                alert(e);
            }
        });
    }
}
function clickListPesq(idItem, idLista) {
    if (idLista !== undefined && idLista !== '') {
        //$('#li_' + idLista+'').val(proID).attr('data-id', proID);
        var tt = '#hicab_' + idItem.slice(-1);
        $(tt).val(idLista);
        $(tt).text(idLista);
        $(tt).val(idLista);
        $('#' + idItem).val(idLista);
        tt = '#divResultPesq_' + idItem;
    }
    else {
        $('#' + idItem + '').val("").attr('data-id', '');
    }
    //$(tt).tab('hide');
    $(tt).hide();
}