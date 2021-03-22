var _modalDividirPeriodoProducao = (function () {
    var modal = $('#modalDividirPeriodo');
    $(document).ready(function () {
        //ocorre quando a modal é fechada
        $(document).on('hidden.bs.modal', '#modalDividirPeriodo', function () {
            $('#mdDiviPeriBtnSalvar').off();
        });
        //click do botao dividir periodo

        $(document).on('click', '#modalDividirPeriodo .table .btn-defaut', eventos.clickBtnDividirPeriodoProd);
        $(document).on('click', ' #mdDivQtdPartInconMenos', eventos.clickBtnDecrementQuantPart);
        $(document).on('click', ' #mdDivQtdPartInconMais', eventos.clickBtnIncremetQuantPart);
    });
    var eventos = {
        clickBtnDividirPeriodoProd: function () {
            var tabBody = modal.find('.tabTop table tbody');
            var numPartes = modal.find('.tabTop .jsNumPartes').val();
        },
        
        clickBtnIncremetQuantPart: function () {
            var valor = $('#mdDivPeriodoBtnNumPartes').val();
            $('#mdDivPeriodoBtnNumPartes').val(Number(valor) + 1);
        },
        clickBtnDecrementQuantPart: function () {
            var valor = $('#mdDivPeriodoBtnNumPartes').val();
            if (valor > 1)
                $('#mdDivPeriodoBtnNumPartes').val(Number(valor) - 1);
        }
    };
    var utils = {
        limparDivAlert: function () {
            DivAlertMsg.limpar('divMsgAlerts');
        }
    }
    var publico = {
        abrirModal: function (medicao) {
            $('#mdDiviPeriBtnSalvar').click(function () {//envia ao servidor a quantidade definida pelo usuário

                var quantidade = $('#mdDivPeriodoBtnNumPartes').val();
                medicao.Quantidade = quantidade;
                modal.modal('hide');
                _tabelaFeedback.dividirPeriodoProducao(medicao);
                
            });
            modal.find('.tabTop').html([
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfGrupo' }).val(medicao.Grupo),
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfMaquinaId' }).val(medicao.MaquinaId),
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfQuantidade' }).val(medicao.Quantidade),
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfTurno' }).val(medicao.TurnoId),
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfTurma' }).val(medicao.TurmaId),
                $('<input>', { type: 'hidden', id: 'mdDivPeriodoHfOp' }).val(medicao.OrdemProducaoId),
                $('<table>', { class: 'table' }).html([
                    $('<thead>').html([
                        $('<tr>').html([
                            $('<td>').text('Informe a Quantidade de Períodos:'),
                        ])
                    ]),
                    $('<tbody>').html([//gera a tabela no topo da modal
                        $('<tr>', { class: 'warning' }).html([
                            $('<td>', { class: 'text-right' }).html([
                                $('<div>', { class: 'group-controls' }).html([
                                    $('<input>', { id: 'mdDivPeriodoBtnNumPartes', type: 'text', class: 'jsNumPartes' }),
                                    $('<button>', { class: 'btn-incon', id: 'mdDivQtdPartInconMenos' }).html($('<i>', { class: 'fa fa-chevron-down' })),
                                    $('<button>', { class: 'btn-incon', id: 'mdDivQtdPartInconMais' }).html($('<i>', { class: 'fa fa-chevron-up' })),
                                ])
                            ]),
                        ])
                    ])
                ])
            ]);
            //mascara para os campos
            $('#mdDivPeriodoBtnNumPartes').inputmask({
                alias: 'integer',
                min: '1',
                rightAlign: false,
            });
            $('#mdDivPeriodoBtnNumPartes').val(1);
            modal.modal('show');
        }
    };
    return publico;
})();