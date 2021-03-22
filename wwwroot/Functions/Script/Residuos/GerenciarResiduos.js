function gridReprogramacao(array) {
    var formGrid = '';
    for (var i = 0; i < array.length; i++) {
        var boas = parseInt(array[i].QTD_PECAS_BOAS);
        var perdas = parseInt(array[i].QTD_PERDAS);
        var PECAS_PRODUZIDAS = boas;
        formGrid += '<tr>';
        formGrid += '<td>';
        formGrid += '<input disabled class="form-check-input " type="checkbox" value="' + array[i].ROT_SEQ_TRANFORMACAO + '" id="itemSeqTrans' + i + '" name="itemSeqTrans" ' + " checked " + ' > ';
        formGrid += '<label disabled class="form-check-label" for= "l_' + i + '">' + array[i].ROT_SEQ_TRANFORMACAO + '</label>';
        formGrid += '<td>';
        formGrid += '<input disabled class="form-control" type="text" value="' + array[i].MAQUINAS + '" id="t_' + i + '" name="t_' + i + '">';
        formGrid += '</td>';
        formGrid += '</td>';
        formGrid += '<td>';
        formGrid += '<input disabled class="form-control" type="text" value="' + array[i].ROT_PRO_ID + '" id="produto_' + i + '" name="produto_' + i + '">';
        formGrid += '</td>';
        formGrid += '<td>';
        formGrid += '<input disabled class="form-control" type="text" value="' + PECAS_PRODUZIDAS + '" id="t_' + i + '" name="t_' + i + '">';
        formGrid += '</td>';
        formGrid += '<td>';
        formGrid += '<input class="form-control" type="text" value="' + array[i].SALDO_A_PRODUZIR + '" id="SaldoRep_' + i + '" name="SaldoRep_' + i + '">';
        formGrid += '</td>';
        formGrid += '</tr>';
        formGrid += '<tr>';
        if (array[i].OBS != 'N/A') {
            formGrid += '<td colspan="6">';
            formGrid += '<input class="form-control" style="font-weight: bold;" disbled type="text" value="' + array[i].OBS + '" id="SaldoRep_' + i + '" name="SaldoRep_' + i + '">';
            formGrid += '</td>';
            formGrid += '</tr>';
        }
        

    }
    return formGrid;
}

function ResiduosReprogramacaoOP(FASE, ORD_ID, ROT_PRO_ID, PRO_DESCRICAO, SALDO_A_PRODUZIR, FPR_SEQ_REPETICAO, ORD_OP_INTEGRACAO, ORD_QUANTIDADE, CLI_NOME, FPR_PREVISAO_MATERIA_PRIMA) {
    let NameSpace = 'DynamicForms.Areas.PlugAndPlay.Models.MovimentoEstoque'
    let path = "/plugandplay/APS/ResiduosReprogramacaoOP";
    Carregando.abrir('Processando....');
    $.ajax({
        url: path,
        type: "POST",
        data: { ORD_ID, FASE },
        dataType: "json",
        success: function (r) {
            if (r.st === "OK") {
                if (r.m !== "" && r.m !== null) {
                    $('#modalResiduos').modal('show');
                    let formFields = '';
                    let formActions = '';
                    let formGrid = '';
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Cliente</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + CLI_NOME + '" id="Cliente" name="Cliente">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Produto</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + ROT_PRO_ID + '" id="Produto" name="Produto">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-12 form-group">';
                    formFields += '<label class="form-label">Descrição</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + PRO_DESCRICAO + '" id="Descricao" name="Descricao">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Pedido</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + ORD_ID + '" id="Pedido" name="Pedido">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Id Integração</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + ORD_OP_INTEGRACAO + '" id="idIntegracao" name="idIntegracao">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Quantidade Pedido</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + ORD_QUANTIDADE + '" id="QtdPed" name="QtdPed">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Sequencia Repetição</label>';
                    formFields += '<input disabled class="form-control" type="text"  value="' + FPR_SEQ_REPETICAO + '" id="SeqRep" name="SeqRep">';
                    formFields += '</div>';
                    //--
                    formFields += '<div class="col-md-6 form-group">';
                    formFields += '<label class="form-label">Data previsão matéria prima</label>';
                    formFields += '<input type="datetime-local" value="' + FPR_PREVISAO_MATERIA_PRIMA + '" id="PrevMat" name="PrevMat" class="form-control"><span class="field-validation-valid text-danger" data-valmsg-for="DataFim" data-valmsg-replace="true">';
                    formFields += '</div>';
                    //---
                    formFields += '</div>';
                    if (r.Db_Itens !== null) {
                        formGrid += '<div class="row"><table class="table" id="dados" name="dados"><thead>';
                        formGrid += '<th> Sequencia Transformação </th>';
                        formGrid += '<th> Maquinas </th>';
                        formGrid += '<th> Produto </th>';
                        formGrid += '<th> QTD. Produzida </th>';
                        formGrid += '<th> Saldo a Reprogramar </th>';
                        formGrid += '</tr></thead><tbody>';
                        formGrid += gridReprogramacao(r.Db_Itens);
                        formGrid += '</tbody></table></div>';
                        //--
                    }
                    //--
                    //js = `onclick="javascript:eventoAbrirNovaAba('${NameSpace}','ORD_ID=${ORD_ID}');"`;
                    //formActions += '<button ' + js + ' type="button" class="btn btn-info" data-dismiss="modal">Motivos de reprogramação</button>';
                    formActions += '<button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>';
                    js = 'onclick="javascript:EliminarResiduos(\'' + ORD_ID + '\',\'' + ROT_PRO_ID + '\',\'' + FPR_SEQ_REPETICAO + '\');"';
                    formActions += '<button ' + js + ' type="button" class="btn btn-primary" data-dismiss="modal">Elimina residuo</button>';
                    js = 'onclick="javascript:ReplanejarPedidos(\'' + ORD_ID + '\',\'' + ROT_PRO_ID + '\',\'' + FPR_SEQ_REPETICAO + '\');"';
                    formActions += '<button ' + js + ' type="button" class="btn btn-primary" data-dismiss="modal">Replanejar Marcados</button>';
                    formActions = '<div id="" class="modal-footer">' + formActions + '</div>';
                    formGrid += formActions;
                    formFields += formGrid;
                    $('#divResiduos').html(formFields);
                }
                return "OK";
            }
            else {
                return r.st;
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

function EliminarResiduos(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO) {
    url = '/plugandplay/APS/EliminarResiduosOP';
    var Cabecalho = { Pedido: ORD_ID, Produto: ROT_PRO_ID, SeqRep: FPR_SEQ_REPETICAO, PrevMat: $('#PrevMat').val() };
    var listaPedidos = document.getElementsByName("itemSeqTrans");;
    itensEliminacao = [];
    for (var i = 0; i < listaPedidos.length; i++) {
        if (listaPedidos[i].checked === true) {
            var item = { SeqTrans: listaPedidos[i].value, SaldoRep: $('#SaldoRep_' + i).val() };
            itensEliminacao.push(item);
        }
    }
    $.ajax({
        type: 'POST',
        url: url,
        data: { Cab: JSON.stringify(Cabecalho) },
        dataType: "json",
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                alert("Resíduos eliminados e OP encerrada com sucesso!")
                ListaOPsParciais.carregarTabela();
            }
        },
        error: OnError,
        complete: function () {
        }
    });
}

function ReplanejarPedidos(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO) {
    let erro;
    url = '/plugandplay/APS/ReprogramarOP';
    var previsaoMateriaPrima = null;

    //verifica se esta no formato correto
    if ($('#PrevMat').val() !== "" && isIsoDate($('#PrevMat').val())) {
        let data = new Date($('#PrevMat').val());
        let day = data.getDate();
        let month = data.getMonth() + 1;
        let year = data.getFullYear();

        //verifica se é uma data valida
        if (validaData(day, month, year)) {
            previsaoMateriaPrima = $('#PrevMat').val().replace("T", " ");
        }
        else {
            erro = true;
            alert("Ocorreu um erro na validação da data de materia prima.");
            return;
        }
    }
    else {
        erro = true;
        alert("Ocorreu um erro na validação da data de materia prima.");
        return;
    }

    var Cabecalho = { Pedido: ORD_ID, Produto: ROT_PRO_ID, SeqRep: FPR_SEQ_REPETICAO, PrevMat: previsaoMateriaPrima };
    var listaPedidos = document.getElementsByName("itemSeqTrans");;
    itensReplanejamento = [];
    for (var i = 0; i < listaPedidos.length; i++) {
        if (listaPedidos[i].checked === true) {
            var item = { SeqTrans: listaPedidos[i].value, SaldoRep: $('#SaldoRep_' + i).val(), produto: $('#produto_' + i).val() };
            itensReplanejamento.push(item);
        }
    }

    if (erro) {
        alert("Ocorreu um erro na validação dos dados.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: url,
        data: { Itens: JSON.stringify(itensReplanejamento), Cab: JSON.stringify(Cabecalho) },
        dataType: "json",
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                alert("Pedido reprogamado com sucesso!")
                ListaOPsParciais.carregarTabela();
            } else {
                alert(result.st);
            }
        },
        error: function (result) {
            alert("Ocorreu um erro no processo de reprogramação:" + result.st)
        },

        complete: function () {
        }
    });
}
