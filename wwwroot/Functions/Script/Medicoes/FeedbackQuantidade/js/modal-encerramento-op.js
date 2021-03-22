var ModalEncerramentoOp = (function () {
    var eventos = {
        clickCkEncerrarOP: function () {
            $('#divFormEncOpParcial').slideToggle();
            $('#divAlertEncOpParcial').slideToggle();
            eventos.limparErrosCampos();
        },
        clickBtnCancelar: function () {
            var urlTelaOperador = $('#btnCancelarFeedbackModal').attr("data-url");
            var origin = window.location.origin;

            const url = origin + urlTelaOperador;

            window.location.href = url;
        },
        clickBtnSalvar: function () {
            var podeEncerrar = $('#ckEncerrarOpParcial').prop('checked');
            var ddlOcorencia = $('#ddlOcorrenciaOpParcial');
            var txtJustificativa = $('#txtJustificativaEncOpParcial');

            if (podeEncerrar) {
                var ok = true;
                if (txtJustificativa.val().trim() == '') {
                    txtJustificativa.parents('.has-feedback').addClass('has-error');
                    txtJustificativa.next().text('informe a justificativa');
                    ok = false;
                }
                if (ddlOcorencia.val() == '') {
                    ddlOcorencia.parents('.has-feedback').addClass('has-error');
                    ddlOcorencia.next().text('Selecione a ocorrencia');
                    ok = false;
                }
                if (ok) {
                    ApApp.encerrarOP(podeEncerrar, ddlOcorencia.val(), txtJustificativa.val());
                }
            } else {
                ApApp.encerrarOP(podeEncerrar);
            }
        },
        resetar: function () {
            eventos.limparErrosCampos();
            $('#ckEncerrarOpParcial').prop('checked', true);
            $('#divFormEncOpParcial').hide();
            $('#divAlertEncOpParcial').show()

        },
        limparErroCampo: function (campo) {
            var campo = $(this);
            campo.parents('.has-feedback').removeClass('has-error');
            campo.next().empty();
        },
        limparErrosCampos: function () {
            $('#mdConfirmEncOp .help-block').empty();
            $('#mdConfirmEncOp .has-feedback').removeClass('has-error');
            $('#mdConfirmEncOp select').find('option [value=""]').prop('selected', true);
            $('#mdConfirmEncOp textarea').val('');
        }
    }
    var publico = {
        documentReady: function () {
            $('#btnCancelarFeedbackModal').click(eventos.clickBtnCancelar);
            $('#btnSalvarFeedbackModal').click(eventos.clickBtnSalvar);
            $('#mdConfirmEncOp select').change(eventos.limparErroCampo);
            $('#mdConfirmEncOp textarea').click(eventos.limparErroCampo);
            $('#mdConfirmEncOp #labelCkEnOp').change(eventos.clickCkEncerrarOP);
            $('#mdConfirmEncOp').on('hidden.bs.modal', eventos.resetar);

            $('#ckEncerrarOpParcial').prop('checked', true);
            $('#divFormEncOpParcial').slideToggle();
            $('#divAlertEncOpParcial').slideToggle();
            eventos.limparErrosCampos();
        },
        abrir: function (qtdPecaBoaProduzida) {
            var confirm = $('#mdConfirmEncOp');
            $('#qtdPecasBoasProd').text(qtdPecaBoaProduzida);
            confirm.modal('show');
        }
    }
    return publico;
})();