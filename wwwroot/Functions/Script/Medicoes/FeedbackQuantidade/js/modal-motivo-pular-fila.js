var ModalMotivoPularFila = (function () {
    var eventos = {
        clickBtnSalvar: function () {

            let validou = eventos.validarMotivosPularFila();

            if (validou) {
                
                var maquina = $("#mdMotPularFilaBtnSalvar").attr("data-maquina");
                var produto = $("#mdMotPularFilaBtnSalvar").attr("data-produto");
                var order = $("#mdMotPularFilaBtnSalvar").attr("data-order");
                var seqRep = $("#mdMotPularFilaBtnSalvar").attr("data-seqRep");
                var seqTran = $("#mdMotPularFilaBtnSalvar").attr("data-seqTran");
                var ocorrenciaId = $('#slMotPularFila').val();
                var obs = $('#txtObsMotPularFila').val().trim();

                $.ajax({
                    type: "GET",
                    url: '/PlugAndPlay/Medicoes/GravarMotivoPularFila',
                    data: { maquina, produto, order, seqRep, seqTran, ocorrenciaId, obs },
                    //dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result.st == "OK") {
                            ApApp.salvarApontamentos();
                        } else {
                            AlertPage.mostrar("Erro", result.msg);
                        }
                    },
                    error: function () {
                    },
                    complete: function () {
                    }
                });

            }
        },
        validarMotivosPularFila: function () {
            var ocorrencia = $('#slMotPularFila');
            var obs = $('#txtObsMotPularFila');

            if (ocorrencia.val() == 0) {
                ocorrencia.parents('.has-feedback').addClass('has-error');
                ocorrencia.next().text('Selecione a ocorrencia.');
                return false;
            }
            if (obs.val().trim() == "") {
                obs.parents('.has-feedback').addClass('has-error');
                obs.next().text('informe as observações.');
                return false;
            }
            return true;
        },
        addAttrBtnSalvar(op) {
            $("#mdMotPularFilaBtnSalvar").attr("data-maquina", op.maquina);
            $("#mdMotPularFilaBtnSalvar").attr("data-produto", op.produto);
            $("#mdMotPularFilaBtnSalvar").attr("data-order", op.order);
            $("#mdMotPularFilaBtnSalvar").attr("data-seqRep", op.seqRep);
            $("#mdMotPularFilaBtnSalvar").attr("data-seqTran", op.seqTran);
        },
        addMotivos(ocorrencias) {

            var lb = $('<label>', { class: 'form-label' }).html('Motivo');

            var motivos = $('<select>', { class: 'form-control', style: 'width: 100%;', id: 'slMotPularFila' }).html([
                $('<option>')
                    .attr('value', '0')
                    .text('...')
            ]);
            ocorrencias.forEach(function (oc) {
                motivos.append($('<option>')
                    .attr('value', oc.OCO_ID)
                    .text(oc.OCO_DESCRICAO));
            });

            var sp = $('<span>', { class: 'help-block' });

            $("#divOcorrenciasPularFila").html(lb);
            $("#divOcorrenciasPularFila").append(motivos);
            $("#divOcorrenciasPularFila").append(sp);
        },
        limparErroCampo: function (campo) {
            var campo = $(this);
            campo.parents('.has-feedback').removeClass('has-error');
            campo.next().empty();
        },
        limparErrosCampos: function () {
            $('#mdMotPularFila .help-block').empty();
            $('#mdMotPularFila .has-feedback').removeClass('has-error');
            $('#mdMotPularFila select').find('option [value=0]').prop('selected', true);
            $('#mdMotPularFila textarea').val('');
        }
    }
    var publico = {
        abrir: function (op, ocorrencias) {

            eventos.addAttrBtnSalvar(op);
            eventos.addMotivos(ocorrencias);
            $('#mdMotPularFilaBtnSalvar').click(eventos.clickBtnSalvar);

            $('#mdMotPularFila select').change(eventos.limparErroCampo);
            $('#mdMotPularFila textarea').click(eventos.limparErroCampo);
            eventos.limparErrosCampos();

            var confirm = $('#mdMotPularFila');
            confirm.modal('show');
        }
    }
    return publico;
})();