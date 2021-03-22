var _tabelaFeedback = (function () {
    var eventos = {
        clickBtnDividir: function () {
            //addLoading
            var linha = $(this).parents('tr');
            var dataIni = moment(linha.find('.jsPeriodo').attr('data-data-ini'), 'DD/MM/YYYY HH:mm:ss');
            var dataFim = moment(linha.find('.jsPeriodo').attr('data-data-fim'), 'DD/MM/YYYY HH:mm:ss');
            var maquinaId = ServerValues.maquinaId;
            var grupo = linha.attr('data-grupo');
            var quantidade = linha.find('.jsQuantidade').text();
            var origem = linha.find('.jsOrigem').text();
            var turno = linha.find('.jsTurnosTab').val();
            var turma = linha.find('.jsTurmasTab').val();
            var op = linha.find('.jsSltOpTab').val();
            
            if (origem != 'M') {

                var medicao = {
                    DataInicio: dataIni.format('YYYY/MM/DD HH:mm:ss'),
                    DataFim: dataFim.format('YYYY/MM/DD HH:mm:ss'),
                    MaquinaId: maquinaId,
                    Grupo: grupo,
                    Quantidade: quantidade,
                    TurnoId: turno,
                    TurmaId: turma,
                    OrdemProducaoId: op,
                    QtdOriginal: quantidade
                };

                if (quantidade > 0) {

                    Carregando.abrir();
                    $.ajax({
                        type: 'POST',
                        url: '/PlugAndPlay/Medicoes/VerificarSeEhUltimoGrupo',
                        data: { medicaoJson: JSON.stringify(medicao) },
                        dataType: "json",
                        traditional: true,
                        contenttype: "application/json; charset=utf-8",
                        success: function (ultimoGrupo) {
                            if (ultimoGrupo) {
                                _tabelaFeedback.dividirPeriodoProducao(medicao);
                            }
                            else {
                                _modalDividirPeriodoProducao.abrirModal(medicao);
                            }
                        },
                        error: function () {
                        },
                        complete: function () {
                            Carregando.fechar();
                        }
                    });
                } else {
                    _modalDividirPeriodo.abrirModal(medicao);
                }
                
            } else {
                AlertPage.mostrar("erro", 'Você não pode dividir um apontamento de origem manual.')
            }
                
        },
        clickBtnExcluirFeedBack: function () {
            var tr = $(this).parents('tr');
            var feeId = tr.attr('data-id');
            var maqId = tr.attr('data-maq-id');
            var grupo = tr.attr('data-grupo');
            var dataIni = tr.attr('data-')
            Modal.confirmacao({
                msg:'Excluir Feedback?',
                fnSucesso: function () {
                    $.ajax({
                        type: 'POST',
                        url: '/PlugAndPlay/Medicoes/CancelarFeedback',
                        data: { medId: feeId, maquina: maqId, grupo: grupo.replace('.', ',') },
                        success: function (result) {
                            if (result.ok) {
                                Conexao.obterLinhaTempo(maqId);
                            }
                            else if (result.msg.length > 0) {
                                AlertPage.mostrar("erro",result.msg);
                            }
                            else {
                                AlertPage.mostrar("erro",'Ocorreu um erro ao excluir o feedback')
                            }
                        },
                        error: function () {
                            AlertPage.mostrar("erro",'Ocorreu um erro ao excluir o feedback')
                        }
                    });
                }
            });
        }
    };
    var util = {
        abrirModalDivPeriodo: function (linha) {
            
        }
    }
    var publico = {
        documentRead: function () {
            $(document).on('click', '#exMedicoes .jsBtnDividir', eventos.clickBtnDividir);
            $(document).on('click', '#exMedicoes .jsCancelarFeedback', eventos.clickBtnExcluirFeedBack);
        },
        dividirPeriodoProducao: function (medicao) {
            Carregando.abrir();
            $.ajax({
                type: 'POST',
                url: '/PlugAndPlay/Medicoes/DividirPeriodoProducao',
                data: { medicaoJson: JSON.stringify(medicao) },
                dataType: "json",
                traditional: true,
                contenttype: "application/json; charset=utf-8",
                success: function (resp) {
                    if (resp.status) {
                        if (resp.msgAtencao != undefined) {
                            AlertPage.mostrar("sucesso", resp.msgAtencao);
                        }
                        Conexao.obterLinhaTempo(medicao.MaquinaId);
                    }
                    else if (resp.msgError.length > 0) {
                        AlertPage.mostrar("erro", resp.msgError);
                    }
                },
                error: function () {
                },
                complete: function () {
                    Carregando.fechar();
                }
            });
        }
    };
    return publico;
})();