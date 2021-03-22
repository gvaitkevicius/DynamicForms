var ModalAlterarFila = (function () {
    var eventos = {
        clickBtnSalvar: function () {
            $('#mdAlterarFila').modal('hide');
            Carregando.abrir('Alterando');
            $.ajax({
                url: '/PlugAndPlay/FilaProducao/Edit',
                type: "POST",
                data: JSON.stringify({
                    OrderId: $('#mdAltFilaOrder').val(),
                    MaquinaId: $('#mdAltFilaMaquina').val(),
                    ProdutoId: $('#mdAltFilaProduto').val(),
                    SequenciaTansformacao: $('#mdAltFilaSequenciaTran').val(),
                    SequencaRepeticao: $('#mdAltFilaSequenciaRep').val(),
                    DataInicioPrevista: $('#mdAltFilaDataIni').data("DateTimePicker").date().format('YYYY/MM/DD HH:mm:ss'),
                    DataFimPrevista: $('#mdAltFilaDataFim').data("DateTimePicker").date().format('YYYY/MM/DD HH:mm:ss')
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result) {
                        TabelaFilaProducao.carregarTabela();
                    }
                    else {
                        alert('erro')
                    }
                },
                error: function () {
                    alert('erro')
                },
                complete: function () {
                    Carregando.fechar();
                }
            });
        }
    }
    var publico = {
        inicio: function () {
            $('#mdAltFilaDataIni').datetimepicker({
                sideBySide: true,
                showClose: true,
                widgetPositioning: {
                    horizontal: 'auto',
                    vertical: 'bottom'
                },
                useCurrent: false
            });
            $('#mdAltFilaDataFim').datetimepicker({
                sideBySide: true,
                showClose: true,
                widgetPositioning: {
                    horizontal: 'auto',
                    vertical: 'bottom'
                },
                useCurrent: false
            });
            $('#mdAltFilaBtnSalvar').click(eventos.clickBtnSalvar);
            $('#mdAlterarFila').on('hidden.bs.modal', function (e) {
                $('#mdAltFilaDataIni').data().DateTimePicker.date(null);
                $('#mdAltFilaDataFim').data().DateTimePicker.date(null);
            })
        },
        abrir: function (tr) {
            $('#mdAltFilaMaquina').val(tr.attr('data-maq-id'));
            $('#mdAltFilaProduto').val(tr.attr('data-pro-id'));
            $('#mdAltFilaSequenciaTran').val(tr.attr('data-seq-tran'));
            $('#mdAltFilaSequenciaRep').val(tr.attr('data-seq-rep'));
            $('#mdAltFilaOrder').val(tr.attr('data-ord-id'));

            var stringDataIni = tr.attr('data-data-inicio');
            var stringDataFim = tr.attr('data-data-fim');

            //$('#mdAltFilaDataIni').val(stringDataIni);
            $('#mdAltFilaDataIni').data("DateTimePicker").defaultDate(moment(stringDataIni, 'DD/MM/YYYY HH:mm:ss'));
            //$('#mdAltFilaDataFim').val(stringDataFim);
            $('#mdAltFilaDataFim').data("DateTimePicker").defaultDate(moment(stringDataFim, 'DD/MM/YYYY HH:mm:ss'));

            $('#mdAlterarFila').modal();
        }
    }
    return publico;
})();










var ModalADDSeqerarFila = (function () {
    var eventos = {
        //clickBtnResiduo: function () 
        clickBtnResiduo: function () {
            Carregando.abrir('Eliminando residuo da OP');
            var aOrderId = [['test1e', 'tes1t2', 'te1st3'], ['t2este', 'te2t2', 'tes2t3'], ['tes3te', 'te4t2', 'tes5t3']];

            var Dados = new FormData(document.querySelector('setAddseq'));


            $('table#cuuuuc tr:has(input,select,checkbox)').each(function () {
                var self = $(this);
                alert(this.name + ' @- ' + this.id + ' - ' + $(this).val() + ' - ' + $(this).text());
                // Passo 2 e 3.
                //arr_resp.push({
                //    'codigo': self.find('.txt_cod').val(),
                //    'resp': self.find('.cmb_resp').val(),
                //    'obs': self.find('.txt_obs').val()
                //});
            });



            $("#ffsetAddSeq :input").each(function () {
                if ($(this).val() == '') {
                    alert(this.name + ' - ' + this.id + ' - ' + $(this).val() + ' - ' + $(this).text());
                }

            });

            var $inputs = $('#ffsetAddSeq :input');

            // not sure if you wanted this, but I thought I'd add it.
            // get an associative array of just the values.
            var values = {};
            $inputs.each(function () {
                //alert(this.name);
                alert(this.name + ' - ' + this.id + ' - ' + $(this).val() + ' - ' + $(this).text() + ' k ' + $(this).is(':checked'));
            });

            //var arrayObj = new Array();
            //$("input[name='objetos']").each(function (i, valor) {
            //    var obj = new Object();
            //    obj.id = $(this).val();
            //    obj.nome = $(this).text();
            //    arrayObj.push(obj);

            //});





            //var jsonObjs = JSON.stringify(arrayObj);





            $.ajax({
                url: '/PlugAndPlay/FilaProducao/setAddseq',
                type: "POST",
                data: JSON.stringify({
                    d: aOrderId,//$('#add_ORD_ID').val(),
                    ProdId: $('#add_PRO_ID').val(),
                    SeqRep: $('#add_SEQ_REPETICAO').val(),
                    Qtd: -1
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result) {
                        ListaOPsParciais.carregarTabela();
                    }
                    else {
                        alert('erro')
                    }
                },
                error: function () {
                    alert('erro')
                },
                complete: function () {
                    $('#mADDsequenciaOP').modal('hide');
                    Carregando.fechar();
                }
            });
        },
        clickBtnSalvar: function () {
            $('#mdADDSeqerarFila').modal('hide');
            Carregando.abrir('Gerando OP');
            $.ajax({
                url: '/PlugAndPlay/FilaProducao/addseq',
                type: "POST",
                data: JSON.stringify({
                    OrderId: $('#add_ORD_ID').val(),
                    ProdId: $('#add_PRO_ID').val(),
                    SeqRep: $('#add_SEQ_REPETICAO').val(),
                    Qtd: $('#add_QTD').val()
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result) {
                        ListaOPsParciais.carregarTabela();
                    }
                    else {
                        alert('erro')
                    }
                },
                error: function () {
                    alert('erro')
                },
                complete: function () {
                    $('#mADDsequenciaOP').modal('hide');
                    Carregando.fechar();
                }
            });
        }
    }
    var publico = {
        inicio: function () {
            $('#mdADDSeqFilaBtnSalvar').click(eventos.clickBtnSalvar);
            $('#mdADDSeqFilaBtnEliminarResiduo').click(eventos.clickBtnResiduo);
        }
    }
    return publico;
})();



function AlteraFila() {

    var vmaquina = $('#rotManual').val();
    var vordem = $('#d_altOrdem').val();
    var dtprev = $('#d_prevMatPrima').val();
    var vid = $('#d_IdFila').text();

    $.ajax({
        url: '/PlugAndPlay/FilaProducao/AlteraFila',
        type: "POST",
        data: JSON.stringify({
            ordem: vordem,
            maqManual: vmaquina,
            idFila: vid,
            dtprevMat: dtprev

        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result = "OK") {
                $('#mDadosPed').modal('hide');
                TabelaFilaProducao.carregarTabela('sim');
            }
            else {
                alert('erro')
            }
        },
        error: function () {
            alert('erro')
        },
        complete: function () {
            Carregando.fechar();
        }
    });
}