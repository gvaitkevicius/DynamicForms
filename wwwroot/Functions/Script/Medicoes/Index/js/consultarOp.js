

function verificarConsultaPrevia(maqId, equipeId, OpConsultada) {
    if (OpConsultada != null && OpConsultada != '') {
        ClckBtnFindOp(maqId, equipeId, OpConsultada);
    }
}


function ClckBtnFindOp(maqId, equipeId, opConsultada) {
    var order = opConsultada != null && opConsultada != '' ? opConsultada : $('#FindOp').val();
    var idDiv = order.replaceAll('-', '_').replaceAll('/', '_') + "";
    var dados = { maqId: maqId, equipeId: equipeId, order: order };
    Carregando.abrir('Processando ...');
    $.ajax({
        type: 'POST',
        url: '/plugandplay/Medicoes/PesquisarOrdemProducao',
        data: { dados: JSON.stringify(dados) },
        dataType: "json",
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {

                $('#' + idDiv).remove(".grid-list");
                var minhaDiv = $('#' + idDiv)
                minhaDiv.remove();
                
                montarItemLista(result);

            } else {
                alert(result.st);
            }
        },
        error: function (result) {
            Carregando.fechar();
            alert(result.st)
        },

        complete: function () {
            Carregando.fechar();
        }
    });

}
function montarItemLista(result) {
    if (result.fila.length > 0) {
        var list = [];
        result.fila.forEach(function (f, index) {
            var addBtn = false;

            if (f.produzindo === 1)
                addBtn = true;

            var maqAnteriorId = "";
            var maqAnteriorDesc = "";
            var maqAnteriorFimPrevisto = "";
            if (f.maqAnterior != null) {
                maqAnteriorId = f.maqAnterior.maqId;
                maqAnteriorDesc = 'OP Ant.: ' + f.maqAnterior.maqDescricao;
                maqAnteriorFimPrevisto = f.maqAnterior.fimPrevisto;
            }

            var maquinaDesc = "";
            if (ServerValues.equipeId != null) {
                maquinaDesc = f.maqDescricao;
            }
            var idStr = f.order.replaceAll('-', '_').replaceAll('/', '_') + "";
            var divGridList = $('<div>', { class: 'grid-list', id: idStr, style: 'background-color:#D3D3D3' }).html([
                $('<div>', { class: 'conteudo' }).html([
                    $('<div>', { class: 'row' }).html([
                        $('<div>', { class: 'col-xs-12 col-sm-8' }).html([
                            $('<div>', { class: 'info', title: maqAnteriorId }).text(maqAnteriorDesc)
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                            $('<div>', { class: 'info' }).html([
                                $('<span>', { class: 'text-primary', title: 'Fim Previsto' }).text(maqAnteriorFimPrevisto)
                            ]),
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                            $('<div>', { class: 'titulo', title: 'Ordem de Produção' }).text(f.order.toUpperCase() + ' ' + f.seqTran + ' ' + f.seqRep)
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-3' }).html([
                            $('<div>', { class: 'titulo', title: 'OP ERP' }).text(f.opErp)
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-3' }).html([
                            $('<div>', { class: 'titulo', title: 'FT' }).text(f.proIntegracao.toUpperCase())
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-12' }).html([
                            $('<div>', { class: 'info' }).text(f.proId + ' ' + f.proDesc.toUpperCase())
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                            $('<div>', { class: 'info' }).text(maquinaDesc),
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                            $('<div>', { class: 'info' }).text('Qtd. Prevista: ' + f.qtd),
                        ]),
                        $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                            $('<div>', { class: 'info' }).html([
                                $('<span>').text('Previsão: '),
                                $('<span>', { class: 'text-primary' }).text(f.dataInicio)
                            ]),
                        ])
                    ])
                ])
            ]);

            var strNamespace = 'DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaImpressaoEtiquetas';
            var arrayDeValoresDefault = {
                ORD_ID: f.order,
                ROT_PRO_ID: f.proId,
                ROT_SEQ_TRANFORMACAO: f.seqTran,
                FPR_SEQ_REPETICAO: f.seqRep,
                ETI_QUANTIDADE_PALETE: f.ETI_QUANTIDADE_PALETE,
                MAQ_ID: f.maqId,
                ETI_IMPRIMIR_DE: f.ETI_IMPRIMIR_DE,
                ETI_IMPRIMIR_ATE: f.ETI_IMPRIMIR_ATE,
                ETI_NUMERO_COPIAS: f.ETI_NUMERO_COPIAS
            };

            var strArrayDeValoresDefault = JSON.stringify(arrayDeValoresDefault);
            // Botão Imprimir Etiqueta
            divGridList.find('.conteudo .row').append(
                $('<div>', { class: 'col-xs-2' }).html([
                    $('<div>', { class: 'acoes' }).html([
                        $('<a>', {
                            title: 'Imprimir Etiqueta',
                            class: 'btn btn-default btn-xs',
                            href: '/DynamicWeb/LinkSGI?str_namespace=' + strNamespace + '&ArrayDeValoresDefault=' + strArrayDeValoresDefault,
                            target: '_blank'
                        }).html([
                            $('<i>', {
                                class: 'fa fa-print'
                            }).html(''), ' E'
                        ]),
                    ])
                ])
            );

            // Botão Imprimir Ordem de Fabricação
            divGridList.find('.conteudo .row').append(
                $('<div>', { class: 'col-xs-2' }).html([
                    $('<div>', { class: 'acoes' }).html([
                        $('<a>', {
                            title: 'Imprimir Ordem de Produção',
                            class: 'btn btn-default btn-xs',
                            href: '/PlugAndPlay/ReportOrdemFabricacao/GerarPDF?OrdId=' + f.order + '&SeqTran=' + f.seqTran + '&SeqRep=' + f.seqRep + '&ProId=' + f.proId,
                            target: '_blank'
                        }).html([
                            $('<i>', {
                                class: 'fa fa-print'
                            }).html(''), ' OP'
                        ]),
                    ])
                ])
            );

            // Botão Exibir Documentos
            divGridList.find('.conteudo .row').append(
                $('<div>', { class: 'col-xs-1' }).html([
                    $('<div>', { class: 'acoes' }).html([
                        $('<a>', {
                            title: 'Documentos',
                            class: 'btn btn-default btn-xs',
                            href: '/DynamicWeb/ListarDiretorio?Pro_Id=' + f.proId,
                        }).html([
                            $('<i>', {
                                class: 'fa fa-file-o'
                            }).html(''), ' D'
                        ]),
                    ])
                ])
            );

            var cor_fila = f.CorFila != null ? f.CorFila : 'gray'; //cor dos botoes
            //botao prioridade
            if (f.FPR_PRIORIDADE > 0) {
                divGridList.find('.conteudo .row').append(
                    $('<div>', { class: 'col-xs-1' }).html([
                        $('<div>', { 'class': 'acoes' }).html([
                            $('<button>', {
                                title: 'Prioridade',
                                class: 'btn btn-secondary btn-xs',
                                style: `background: ${cor_fila}`
                            }).html(
                                f.FPR_PRIORIDADE
                            ),
                        ])
                    ])
                );
            } else {
                // deixa o botão transparente para manter o alinhamento
                divGridList.find('.conteudo .row').append(
                    $('<div>', { class: 'col-xs-1' }).html([
                        $('<div>', { 'class': 'acoes' }).html([
                            $('<button>', {
                                class: 'btn btn-secondary btn-xs',
                                style: 'background: rgba(255, 255, 255, 0); pointer-events: none;'
                            }).html(''),
                        ])
                    ])
                );
            }

            if (f.ORD_LOTE_PILOTO > 0) {
                //botao lote piloto
                divGridList.find('.conteudo .row').append(
                    $('<div>', { class: 'col-xs-1' }).html([
                        $('<div>', { 'class': 'acoes' }).html([
                            $('<button>', {
                                title: 'Lote piloto',
                                class: 'btn btn-secondary btn-xs',
                                style: `background: ${cor_fila}`
                            }).html(
                                f.ORD_LOTE_PILOTO
                            ),
                        ])
                    ])
                );
            } else {
                // deixa o botão transparente para manter o alinhamento
                divGridList.find('.conteudo .row').append(
                    $('<div>', { class: 'col-xs-1' }).html([
                        $('<div>', { 'class': 'acoes' }).html([
                            $('<button>', {
                                class: 'btn btn-secondary btn-xs',
                                style: 'background: rgba(255, 255, 255, 0); pointer-events: none;'
                            }).html(''),
                        ])
                    ])
                );
            }

            // Botão P
            divGridList.find('.conteudo .row').append(
                $('<div>', { class: 'col-xs-1' }).html([
                    $('<div>', { class: 'acoes' }).html([
                        $('<button>', {
                            title: 'OP Produzindo',
                            class: 'btn btn-success btnProduzindoOp btn-xs',
                            'data-maqId': f.maqId,
                            'data-order': f.order,
                            'data-seqTran': f.seqTran,
                            'data-seqRep': f.seqRep,
                            'data-proId': f.proId,
                            'data-Consulta': 'OK',
                            onclick: ''
                        }).html('P'),
                    ])
                ])
            );
            
            //if (addBtn)
            divGridList.find('.conteudo .row').append(
                $('<div>', { class: 'col-xs-4' }).html([
                    $('<div>', { class: 'acoes' }).html([
                        $('<button>', {
                            title: 'Apontar Produção',
                            class: 'btn btn-success btnEncerrarOp btn-xs',
                            'data-maqId': f.maqId,
                            'data-order': f.order,
                            'data-seqTran': f.seqTran,
                            'data-seqRep': f.seqRep,
                            'data-proId': f.proId,
                            'data-pecasPulso': f.pecasPulso,
                        }).html('Ap. Produção'),
                    ])
                ])
            );

            list.push(divGridList)
        });
    }

    $('#divFilaProducao').prepend(list);
}
