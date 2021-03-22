var FilaProducao = new function () {

    let maquinaAtual;

    this.documentRead = function () {
        //botoes
        $(document).on('click', '.btnDesfazerOp', clickBtnDesfazerOP);
        $(document).on('click', '.btnEncerrarOp', ClickBtnEncerrarOp);
        $(document).on('click', '.btnProduzindoOp', ClickbtnProduzindoOp);
        $(document).on('click', '.btnDetalhesOp', ClckBtnDetalhesOp);
        $(document).on('click', '#gridListFilaProducao', ClckFilaProducao);
        $(document).on('click', '#gridListTargetsPendentes', ClckTargetsPendentes);
        $(document).on('click', '#gridListFilaProducaoHistorico', ClckFilaProducaoHistorico);
    };

    function ClckBtnDetalhesOp() {
        var btn = $(this);
        $.get($UrlLocal.concat('ObterObterOrdemProducao'), {
            maqId: btn.attr('data-maquina'),
            proId: btn.attr('data-produto'),
            order: btn.attr('data-order'),
            seqRep: btn.attr('data-seqRep'),
            seqTran: btn.attr('data-seqTran'),
            movId: btn.attr('data-movId'),
            pecasPulso: btn.attr('data-pecasPulso')

        }).done(function (result) {

        }).fail(function () {

        }).always(function () {

        })
    }

    function clickBtnDesfazerOP() {// botao desfazer op
        var btn = $(this);
        Modal.confirmacao({
            msg: 'Essa ação desfaz todos os feedbacks de producão, quantidade e performance relacionados a esta OP.',
            fnSucesso: function () {
                openAnLoad()
                $.post($UrlLocal.concat('DesfazerOpProduzida'), {
                    maquina: btn.attr('data-maquina'),
                    produto: btn.attr('data-produto'),
                    order: btn.attr('data-order'),
                    seqRep: btn.attr('data-seqRep'),
                    seqTran: btn.attr('data-seqTran'),
                    movId: btn.attr('data-movId')
                }).done(function (result) {
                    if (result.status) {
                        order = btn.attr('data-order').replaceAll('-', '_').replaceAll('/', '_');
                        $(`#${order}`).remove();

                        Global.atualizarPaginaAjax(ServerValues.maquinaId);
                    }
                    else {
                        AlertPage.erro(result.msg);
                    }
                }).fail(function () {
                    AlertPage.mostrar('erro', "Ocorreu um erro ao desfazer o OP.");
                }).always(function () {
                    closeAnLoad();
                });
            }
        });
    }

    function ClickBtnEncerrarOp() {
        var btn = $(this);
        var op = {
            maquina: btn.attr('data-maqId'),
            produto: btn.attr('data-proId'),
            order: btn.attr('data-order'),
            seqRep: btn.attr('data-seqRep'),
            seqTran: btn.attr('data-seqTran'),
            pecasPulso: btn.attr('data-pecasPulso')
        }

        Carregando.abrir('Processando ...');
        openAnLoad();
        $.get($UrlLocal.concat('VerificarUltimaSeqProduzida'), op).done(function (result) {
            if (result.msg !== 'OK') {
                AlertPage.mostrar('erro', result.msg);
                closeAnLoad();
            } else {
                Carregando.abrir('Processando ...');
                $.get($UrlLocal.concat('VerificarSeOpTemFeedback'), op).done(function (result) {
                    Carregando.fechar();
                    if (result) {
                        irParaPreApontamento(op.order, op.maquina, op.seqTran, op.seqRep, op.produto, op.pecasPulso);
                    }
                    else {
                        AlertPage.mostrar('erro', 'Não é possivel apontar a produção sem salvar feedbacks para esta OP.');
                    }
                }).fail(function () {
                    AlertPage.mostrar('erro', '(1) Ocorreu um erro no processamento de validação da OP');
                }).always(function () {
                    closeAnLoad();
                    Carregando.fechar();
                })
            }

        }).fail(function () {
            AlertPage.mostrar('erro', '(2) Ocorreu um erro no processamento de validação da OP');
        }).always(function () {
            closeAnLoad();
            Carregando.fechar();
        })
    }

    function irParaPreApontamento(order, maquina, seqTran, seqRep, produto, pecasPulso) {
        var url = $UrlLocal + "index?id=" + ServerValues.maquinaId;
        var urlEncode = encodeURIComponent(url);//codifica a url atual
        document.location.href = $UrlLocal.concat('FeedbackQuantidade?order=' + order
            + ' &maq=' + maquina + ' &seqTran=' + seqTran
            + ' &seqRep=' + seqRep + ' &produto=' + produto + ' &pecasPulso=' + pecasPulso + ' &url=' + urlEncode)
    }

    function ClickbtnProduzindoOp() {
        var btn = $(this);
        ServerValues.maquinaId = btn.attr('data-maqId');
        var op = {
            maquina: btn.attr('data-maqId'),
            produto: btn.attr('data-proId'),
            order: btn.attr('data-order'),
            seqRep: btn.attr('data-seqRep'),
            seqTran: btn.attr('data-seqTran')
        }
        var consulta = btn.attr('data-Consulta');
        openAnLoad();
        $.get($UrlLocal.concat('SetProduzindo'), op).done(function (result) {
            if (result) {
                if (consulta != 'OK') {
                    Global.atualizarPaginaAjax(ServerValues.maquinaId);
                } else {
                    Global.atualizarPaginaAjaxConsulta(ServerValues.maquinaId);
                }

            }
            else {
                AlertPage.mostrar('erro', 'Não é possivel apontar a produção sem salvar feedbacks para esta OP.');
            }
        }).fail(function () {
            AlertPage.mostrar('erro', 'Ocorreu um erro ao validar OP');
        }).always(function () {
            closeAnLoad();
        })
    }

    this.obterFila = function (maquina) {
        maquinaAtual = maquina;
        openAnLoad();
        $.get($UrlLocal + 'ObterFilaProducao', {
            maquina: maquina,
            equipe: ServerValues.equipeId
        }).done(function (result) {
            if (result.erro != 'OK') {
                alert(`Erro ao carregar a fila de produção ${result.erro}`);
                return;
            }

            $('#divFilaProducao').empty();
            // Fila de Produção
            if (result.fila.length > 0) {
                var list = [];
                result.fila.forEach(function (f, index) {
                    var addBtn = false;

                    if (f.produzindo === 1) {
                        addBtn = true;
                        $('#nomeLastro').text('Embalagens de ' + f.PRO_IMG_LASTRO);
                        var path = '/images/lastros/' + f.PRO_IMG_LASTRO + '.jpg';
                        $('#imgLastro').attr('src', path);
                    }

                    let maquinaDesc = "";
                    let realizados = f.TestesRealizados / f.TestesPorColeta;
                    
                    let TestesColetados = (realizados >= 0) ? `Testes: ${realizados}/${f.AmostrasAColetar}` : 'Testes:0/0';
                    if (f.AmostrasAColetar == -1)
                        TestesColetados = 'Testes:0/0';
                        
                    if (ServerValues.equipeId != null) {
                        maquinaDesc = f.maqDescricao;
                    }
                    var idStr = f.order.replaceAll('-', '_').replaceAll('/', '_') + "";
                    var divGridList = $('<div>', { class: 'grid-list', id: idStr }).html([
                        $('<div>', { class: 'conteudo' }).html([
                            $('<div>', { class: 'row' }).html([

                                $('<div>', { class: 'clearfix' }), // Quebra linha
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'titulo', title: 'Ordem de Produção' }).text(`${f.order.toUpperCase()} ${f.seqTran} ${f.seqRep}`)
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-3' }).html([
                                    $('<div>', { class: 'titulo', title: 'OP ERP' }).text(f.opErp)
                                ]),

                                $('<div>', { class: 'clearfix' }), // Quebra linha

                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'titulo', title: 'FT' }).text(f.proIntegracao.toUpperCase())
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info', title: 'Código Produto' }).text(`${f.proId}`)
                                ]),
                                $('<div>', { class: 'clearfix' }), // Quebra linha
                                $('<div>', { class: 'col-xs-12 col-sm-12' }).html([
                                    $('<div>', { class: 'info' }).text(`${f.proDesc.toUpperCase()}`)
                                ]),
                                $('<div>', { class: 'clearfix' }), // Quebra linha
                                $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                                    $('<div>', { class: 'info' }).text(maquinaDesc),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                                    $('<div>', { class: 'info' }).text(`Qtd. Prevista: ${f.qtd}`),
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

                    divGridList.find('.conteudo .row').append(

                        // Botão Exibir OP anterior
                        $('<div>', { class: 'col-xs-1 col-sm-1' }).html([
                            $('<div>', { class: 'acoes'}).html([
                                $('<a>', {
                                    title: 'Exibir Ordem de Produção Anterior',
                                    class: 'btn btn-default btn-xs',
                                    id: `exibir-op-anterior-${index}`,
                                    onclick: `ExibirDadosOp('${f.order}', '${f.proId}', '${f.seqTran}', '${index}')`
                                }).html([
                                    $('<i>', {
                                        id: `icon-exibir-op-anterior-${index}`,
                                        class: 'fa fa-refresh'
                                    })
                                ])
                            ])
                        ])
                    );
                    

                    // Botão Imprimir Etiqueta
                    divGridList.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-1' }).html([
                            $('<div>', { class: 'acoes' }).html([
                                $('<a>', {
                                    title: 'Imprimir Etiqueta',
                                    class: 'btn btn-default btn-xs',
                                    href: `/DynamicWeb/LinkSGI?str_namespace=${strNamespace}&ArrayDeValoresDefault=${strArrayDeValoresDefault}`,
                                    target: '_blank'
                                }).html([
                                    $('<i>', {
                                        class: 'fa fa-print'
                                    }).html(''), ' E'
                                ])
                            ])
                        ])
                    );

                    // Botão Imprimir Ordem de Fabricação
                    divGridList.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-1' }).html([
                            $('<div>', { class: 'acoes' }).html([
                                $('<a>', {
                                    title: 'Imprimir Ordem de Produção',
                                    class: 'btn btn-default btn-xs',
                                    href: `/PlugAndPlay/ReportOrdemFabricacao/GerarPDF?OrdId=${f.order}&SeqTran=${f.seqTran}&SeqRep=${f.seqRep}&ProLastro=${f.PRO_IMG_LASTRO}`,
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
                                    href: `/DynamicWeb/ListarDiretorio?Pro_Id=${f.proId}`,
                                    target: '_blank',
                                }).html([
                                    $('<i>', {
                                        class: 'fa fa-file-o'
                                    }).html(''), ' D'
                                ]),
                            ])
                        ])
                    );
                    //Botão  Inspeçõeos Visuais
                    divGridList.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-1' }).html([
                            $('<div>', { class: 'acoes' }).html([
                                $('<button>', {
                                    id: 'btnInspecao',
                                    title: 'Inspeções Visuais',
                                    class: 'btn btn-default btn-xs', id: 'btnInspecao',
                                    onClick: `iniciarInspecaoClick('${f.maqId}','${f.proId}','${f.order}',${f.seqRep},${f.seqTran},${f.TEMPLATE_TESTES})`
                                }).html(
                                    $('<i>', {
                                        class: 'fa fa-eye'
                                    }).html(' I'), ''
                                ),
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

                    //Botão de amostras coletdas
                    divGridList.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-2' }).html([
                            $('<div>', { class: 'acoes' }).html([
                                $('<button>', {
                                    id: `btnColetas_${index}`,
                                    title: 'Amostras Coletadas',
                                    class: 'btn btn-success btn-xs',
                                    'data-ped': f.order,
                                    'data-seqT': f.seqTran,
                                    'data-seqR': f.seqRep,
                                    'data-pro': f.proId,
                                    onclick: `modalRecolherAmostras('${f.order}', '${f.proId}','${f.maqId}','${f.seqRep}', '${f.seqTran}', '${index}','${f.TEMPLATE_TESTES}','${realizados}','${f.AmostrasAColetar}')`
                                }).text(TestesColetados)
                            ])
                        ])
                    )


                    if (f.produzindo === 1) {
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
                                        onclick: ''
                                    }).html('P'),
                                ])
                            ])
                        );
                    }
                    else {
                        for (count = 0; count < result.maquinas.length; count++) {
                            // percorre em todas as máquinas retornadas

                            if (f.maqId === result.maquinas[count].MAQ_ID) {
                                // A máquina da OP atual é igual a da lista de máquinas

                                if (result.maquinas[count].MAQ_CONGELA_FILA > 0) {
                                    // Insere o botão P
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
                                                    onclick: ''
                                                }).html('P'),
                                            ])
                                        ])
                                    );
                                    // decrementa o número de OPs que terão o botão 'P'
                                    result.maquinas[count].MAQ_CONGELA_FILA -= 1;

                                    // Utilizado para finalizar a iteração nas máquinas
                                    count = result.maquinas.length;
                                }
                            }
                        }
                    }
                    
                    //Botão de apontar produção
                    if (addBtn)
                        divGridList.find('.conteudo .row').append(
                            $('<div>', { class: 'col-xs-2' }).html([
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


                    //Botão encerrar OP
                    if (f.ATG_TOL_MIN) {
                        divGridList.find('.conteudo .row').append(
                            $('<div>', { class: 'col-xs-2' }).html([
                                $('<div>', { class: 'acoes' }).html([
                                    $('<button>', {
                                        title: 'Encerrar OP',
                                        class: 'btn btn-danger btn-xs',
                                        onclick: `encerrarOpDaFila('${f.order}', '${f.proId}', '${f.maqId}', '${f.seqTran}', '${f.seqRep}')`
                                    }).html('Encerrar OP'),
                                ])
                            ])
                        );
                    }

                    //Div com informações de matéria prima e endereçaemento
                    divGridList.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-12', id: 'divDadosOp' }).html([
                            $('<hr>')
                        ]),


                        
                        $('<div>', { class: 'col-xs-6', id: 'div_dados_01' }).html([
                            $('<div>', { class: 'clearfix' }), // Quebra linha
                            $('<div>', { class: 'col-xs-12 col-sm-12' }).html([
                                $('<div>', { class: '', id: `maqAnteriorDesc-${index}` }).text('')
                            ]),
                            $('<div>', { class: 'col-xs-12 col-sm-1' }).html([
                                $('<div>', { class: 'info' }).html([
                                    $('<span>', { class: '', id: `maqAnteriorSaldoProduzir-${index}` }).text('')
                                ]),
                            ]),
                            $('<div>', { class: 'col-xs-12 col-sm-3' }).html([
                                $('<div>', { class: 'info' }).html([
                                    $('<span>', { class: '', id: `maqAnteriorInicioPrevisto-${index}` }).text('')
                                ]),
                            ]),
                            $('<div>', { class: 'col-xs-12 col-sm-3' }).html([
                                $('<div>', { class: 'info' }).html([
                                    $('<span>', { class: '', id: `maqAnteriorFimPrevisto-${index}` }).text('')
                                ]),
                            ]),

                            //informações de paletes, cliches, tintas 
                            $('<div>', { class:'col-md-12'}).html([
                                $('<span>', { class: '', id: `paletes-${index}`}).text('')
                            ]),
                            $('<div>', { class:'col-md-12'}).html([
                                $('<span>', { class: '', id: `tintas-${index}`}).text('')
                            ]),
                            $('<div>', { class:'col-md-12'}).html([
                                $('<span>', { class: '', id: `cliches-${index}`}).text('')
                            ]),
                            $('<div>', { class:'col-md-12'}).html([
                                $('<span>', { class: '', id: `facas-${index}`}).text('')
                            ])
                        ]),

                        $('<div>', { class: 'col-xs-6', id: 'div_dados_02' }).html([
                            $('<p>').html([
                                $('<span>', { class: '', id: `enderecos-${index}`, title: 'Endereços no formato "endereço, quantidade de paletes"' })
                            ])
                        ])
                    );

                    list.push(divGridList)
                });
                
                $('#divFilaProducao').html(list);
            }

        }).fail(function (result) {
            alert(`Erro ao obter a fila de produção ${result.erro}`);
        }).always(function (result) {
            closeAnLoad()
        });
    }

    function ClckFilaProducao() {
        $('#consultarOpNaFila').show();
    }

    function ClckTargetsPendentes() {

        $('#consultarOpNaFila').hide();

        openAnLoad();
        $.get($UrlLocal + 'ObterTargetsPendentes', {
            maquina: maquinaAtual,
            equipe: ServerValues.equipeId
        }).done(function (result) {

            if (result.erro != 'OK') {
                alert(`Erro ao obter os targets pendentes: ${result.erro}`);
                return;
            }

            $('#divGridListTargetsPendentes').empty();

            var url = "";

            if (ServerValues.equipeId != null) {
                url = $UrlLocal + "index?idEquipe=" + ServerValues.equipeId
            } else if (ServerValues.maquinaId != null) {
                url = $UrlLocal + "index?id=" + ServerValues.maquinaId;
            }

            var urlEncode = encodeURIComponent(url);//codifica a url atual

            // Targets Pendetes
            if (result.opsTarPendentes.length > 0) {
                var listTargetsPendentes = [];
                result.opsTarPendentes.forEach(function (f, index) {
                    var addBtn = false;
                    if (index === 0)
                        addBtn = true

                    var idStr = f.order.replaceAll('-', '_').replaceAll('/', '_') + "";
                    var divGridListTargetsPendentes = $('<div>', { class: 'grid-list', id: idStr }).html([
                        $('<div>', { class: 'conteudo' }).html([
                            $('<div>', { class: 'row' }).html([
                                $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                                    $('<div>', { class: 'titulo' }).text(`${f.order.toUpperCase()} ${f.seqTran} ${f.seqRep}`)
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-8' }).html([
                                    $('<div>', { class: 'info' }).text(`${f.proId} ${f.proDesc.toUpperCase()}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                                    $('<div>', { class: 'info' }).text(f.usuarioNome),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-8' }).html([
                                    $('<div>', { class: 'info' }).text(`Turno: ${f.turno}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-4' }).html([
                                    $('<div>', { class: 'info' }).text(`Qtd: ${f.qtdProduzida}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-8' }).html([
                                    $('<div>', { class: 'info' }).html([
                                        $('<span>').text('Data: '),
                                        $('<span>', { class: 'text-primary' }).text(f.dataDaProducao)
                                    ])
                                ])
                            ])
                        ])
                    ]);
                    if (addBtn)
                        divGridListTargetsPendentes.find('.conteudo .row').append(

                            $('<div>', { class: 'col-xs-12' }).html([
                                $('<div>', { class: 'acoes' }).html([
                                    $('<a>', {
                                        title: 'Feedback de performance',
                                        class: 'btn btn-success btn-xs',
                                        href: $UrlLocal + `FeedbackPerformace?ordId=${f.order}&proId=${f.proId}&maqId=${f.maqId}&seqRep=${f.seqRep}&url=${urlEncode}`,
                                    }).html('Feedback de Desempenho'/*$('<i>', { class: 'fa fa-pencil-square-o btn-incon-azul' })*/)
                                ])
                            ])
                        );
                    listTargetsPendentes.push(divGridListTargetsPendentes)
                });
                $('#divGridListTargetsPendentes').html(listTargetsPendentes);
            }

        }).fail(function (result) {
            alert(`Erro ao obter os targets pendentes: ${result.erro}`);
        }).always(function (result) {
            closeAnLoad()
        });
    }

    function ClckFilaProducaoHistorico() {

        $('#consultarOpNaFila').hide();

        openAnLoad();
        $.get($UrlLocal + 'ObterOpsProduzidas', {
            maquina: maquinaAtual,
            equipe: ServerValues.equipeId
        }).done(function (result) {

            if (result.erro != 'OK') {
                alert(`Erro ao carregar as OPs produzidas: ${result.erro}`);
                return;
            }

            $('#divFilaProducaoHistorico').empty();
            // OPs Produzidas
            if (result.filaHistorico.length > 0) {
                var listHistorico = [];
                result.filaHistorico.forEach(function (f, index) {
                    var addBtn = false;
                    if (index === 0)
                        addBtn = true

                    var idStr = f.order.replaceAll('-', '_').replaceAll('/', '_') + "";
                    var divGridListHistorico = $('<div>', { class: 'grid-list', id: idStr }).html([
                        $('<div>', { class: 'conteudo' }).html([
                            $('<div>', { class: 'row' }).html([
                                $('<div>', { class: 'col-xs-12 col-sm-12' }).html([
                                    $('<div>', { class: 'titulo' }).text(`${f.order.toUpperCase()} ${f.seqTran} ${f.seqRep}`)
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-12' }).html([
                                    $('<div>', { class: 'info' }).text(`${f.proId} ${f.proDesc.toUpperCase()}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).text(f.usuarioNome),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).text(`Turno: ${f.turno}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).text(`Qtd. Produzida: ${f.qtdProduzida}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).text(`Peças Boas: ${f.qtdPecasBoas}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).text(`Qtd. Perdas: ${f.qtdPerdas}`),
                                ]),
                                $('<div>', { class: 'col-xs-12 col-sm-6' }).html([
                                    $('<div>', { class: 'info' }).html([
                                        $('<span>').text('Data: '),
                                        $('<span>', { class: 'text-primary' }).text(f.dataDaProducao)
                                    ])
                                ])
                            ])
                        ])
                    ]);
                    if (addBtn) {
                        divGridListHistorico.find('.conteudo .row').append(
                            $('<div>', { class: 'col-xs-4' }).html([
                                $('<div>', { class: 'acoes' }).html([
                                    $('<button>', {
                                        title: 'Defazer',
                                        class: 'btn btn-xs btnDesfazerOp btn-danger',
                                        'data-maquina': f.maqId,
                                        'data-produto': f.proId,
                                        'data-order': f.order,
                                        'data-seqRep': f.seqRep,
                                        'data-seqTran': f.seqTran,
                                        'data-movId': f.movId
                                    }).html(
                                        'Desfazer'
                                    )
                                ])
                            ])
                        );

                    }

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
                    divGridListHistorico.find('.conteudo .row').append(
                        $('<div>', { class: 'col-xs-6' }).html([
                            $('<div>', { class: 'acoes' }).html([
                                $('<a>', {
                                    title: 'Imprimir Etiqueta',
                                    class: 'btn btn-default btn-xs',
                                    href: `/DynamicWeb/LinkSGI?str_namespace=${strNamespace}&ArrayDeValoresDefault=${strArrayDeValoresDefault}`,
                                    target: '_blank'
                                }).html([
                                    $('<i>', {
                                        class: 'fa fa-print'
                                    }).html(''), ' E'
                                ]),
                            ])
                        ])
                    );

                    listHistorico.push(divGridListHistorico)
                });
                $('#divFilaProducaoHistorico').html(listHistorico);
            }

        }).fail(function (result) {
            alert(`Erro ao obter as OPs produzidas: ${result.erro}`);
        }).always(function (result) {
            closeAnLoad()
        });
    }

    //<uteis>
    this.limparFila = function () {
        $('#divFilaProducao').html("Selecione a Máquina");
    }
    function openAnLoad() {//abre a informacao "carregando"
        $('#dvLoadFila').show();
    }
    function closeAnLoad() {//fecha a informacao "carregando"
        $('#dvLoadFila').hide();
    }
};

function encerrarOpDaFila(ordId, proId, maqId, seqTran, seqRep) {
    
    $.ajax({
        type: 'POST',
        url: '/PlugAndPlay/Medicoes/EncerrarOP',
        data: { maqId, ordId, proId, seqTran, seqRep },
        dataType: "json",
        contenttype: "application/json; charset=utf-8",
        traditional: true,
        success: function (result2) {
            if (result2.st == "OK") {
                //encerrou a OP
                var url = encodeURIComponent((new URL(document.location.href)).searchParams.get('url'));
                window.location.href = `/PlugAndPlay/Medicoes/FeedbackPerformace?ordId=${ordId}&proId=${proId}&maqId=${maqId}&seqRep=${seqRep}&url=${url}`
            } else {
                //não encerrou a OP
                AlertPage.mostrar("Erro", result2.msg);
            }
        },
        error: function () {
        },
        complete: function () {
            Carregando.fechar();
        }
    });
}

//Busca no banco de dados uma série de dados relacionados a OP, como informações da máquina anterior, cliches, facas, tintas, paletes e endereçamento
function ExibirDadosOp(ordId, proId, seqTran, index) {

    $(`#exibir-op-anterior-${index}`).attr("disabled", true);
    $(`#icon-exibir-op-anterior-${index}`).attr("class", 'fa fa-spinner');

    $.get($UrlLocal + 'ObterDadosOrdemDeProducao', {
        ordId: ordId,
        proId: proId,
        seqTran: seqTran
    }).done(function (result) {
        if (result == null) return;

        let maqAnteriorId = "";
        let maqAnteriorDesc = "";
        let maqAnteriorSaldoProduzir = "";
        let maqAnteriorInicioPrevisto = "";
        let maqAnteriorFimPrevisto = "";

        if (result.opAnterior != null) {
            maqAnteriorId = result.opAnterior.maqId;
            maqAnteriorDesc = result.opAnterior.maqDescricao;
            if (result.opAnterior.saldoProduzir <= 0) {
                maqAnteriorSaldoProduzir = "PRONTO";
            } else {
                maqAnteriorSaldoProduzir = result.opAnterior.saldoProduzir.toString();
                maqAnteriorInicioPrevisto = result.opAnterior.inicioPrevisto;
                maqAnteriorFimPrevisto = result.opAnterior.fimPrevisto;
            }
        } else {
            maqAnteriorDesc = "PREVISÃO DE MATÉRIA PRIMA";
            maqAnteriorInicioPrevisto = result.PrevisaoMateriaPrima;
        }

        //#region Informações da máquina anterior
        $(`#maqAnteriorDesc-${index}`).prop('title', 'Descrição da máquina anterior');
        $(`#maqAnteriorDesc-${index}`).text(maqAnteriorDesc);

        $(`#maqAnteriorSaldoProduzir-${index}`).prop('title', 'Saldo a produzir');
        $(`#maqAnteriorSaldoProduzir-${index}`).text(maqAnteriorSaldoProduzir);

        $(`#maqAnteriorInicioPrevisto-${index}`).prop('title', 'Início previsto');
        $(`#maqAnteriorInicioPrevisto-${index}`).text(maqAnteriorInicioPrevisto);

        $(`#maqAnteriorFimPrevisto-${index}`).prop('title', 'Fim previsto');
        $(`#maqAnteriorFimPrevisto-${index}`).text(maqAnteriorFimPrevisto);
        //#endregion

        //#region Informações de paletes, tintas, cliches e facas
        if(result.paletes != null){
            $(`#paletes-${index}`).prop('title', 'Paletes');
            $(`#paletes-${index}`).text(result.paletes);
        }

        if(result.tintas != null){
            $(`#tintas-${index}`).prop('title', 'Tintas');
            $(`#tintas-${index}`).text(result.tintas);
        }

        if(result.cliches != null){
            $(`#cliches-${index}`).prop('title', 'Cliches');
            $(`#cliches-${index}`).text(result.cliches);
        }

        if(result.facas != null){
            $(`#facas-${index}`).prop('title', 'Facas');
            $(`#facas-${index}`).text(result.facas);
        }
        //#endregion

        //#region Informações de endereço
        if(result.enderecos != null){
            let enderecos = result.enderecos;
            let str_enderecos = "";

            enderecos.forEach(function(val, index){
                str_enderecos += val.MOV_ENDERECO + "," + val.QUANTIDADE_PALETES;

                if(index < enderecos.length - 1)
                    str_enderecos += " | ";
            });

            $(`#enderecos-${index}`).text(str_enderecos);
        }
        //#endregion
    }).fail(function (result) {
        AlertPage.mostrar("Erro", result);
    }).always(function (result) {
        $(`#exibir-op-anterior-${index}`).attr("disabled", false);
        $(`#icon-exibir-op-anterior-${index}`).attr("class", 'fa fa-refresh');
    });
}

