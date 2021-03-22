SalvarFeedback = (function () {
    var eventos = {
        clickBtnSalvar: function () {
            if (ValidarCampos.validarTodos()) {
                util.salvarFeedback();
            }
        }
    }
    var util = {
        salvarFeedback: function () {
            
            var OcoPerform = $('#sltOcorrenciaPerformace').val();
            var ObsPer = $('#txtJustificativaPerformace').val();

            var OcoSetupA = $('#sltOcorrenciaSetupA').val();
            var ObsSetpA = $('#txtJustificativaSetupA').val();

            var OcoSetup = $('#sltOcorrenciaSetup').val();
            var ObsSetup = $('#txtJustificativaSetup').val();

            var TargetId = $('#hfTargetId').val();
            var MovimentoId = $('#hfMovimentoId').val();
            

            var target = {
                OCO_ID_PERFORMANCE: OcoPerform,
                TAR_OBS_PERFORMANCE: ObsPer,
                OCO_ID_SETUP: OcoSetup,
                TAR_OBS_SETUP: ObsSetup,
                OCO_ID_SETUPA: OcoSetupA,
                TAR_OBS_SETUPA: ObsSetpA,
                MOV_ID: MovimentoId,
                TAR_ID: TargetId
            }; 
            Carregando.abrir();
            $.ajax({
                type: 'POST',
                url: '/PlugAndPlay/Medicoes/GravarFeedbackPerformace',
                data: { targetJSON: JSON.stringify(target) },
                dataType: 'json',
                traditional: true,
                contenttype: "application/json; charset=utf-8",
                success: function (ok) {
                    if (ok) {
                        var url = '';
                        if (ServerValues.equipeId != null && ServerValues.equipeId!='') {
                            url = '/PlugAndPlay/Medicoes/index?idEquipe=' + ServerValues.equipeId;
                        } else if (ServerValues.maquinaId != null && ServerValues.maquinaId != '') {
                            url = '/PlugAndPlay/Medicoes/index?id=' + ServerValues.maquinaId;
                        }
                        window.location.href = url;
                    }
                    else {
                        Carregando.fechar();
                        AlertPage.mostrar("erro",'Ocorreu um erro ao salvar os feedbacks.');
                    }
                },
                error: function () {
                    Carregando.fechar();
                    AlertPage.mostrar("erro",'Ocorreu um erro ao salvar os feedbacks.');
                }
            });
        }
    }
    var publico = {
        init: function () {
            $('#btnSalvarFeedback').click(eventos.clickBtnSalvar);
        },
        salvarFeedback: function (encerrar = false, ocorrencia = '', justifiativa = '') {
            return util.salvarFeedback(encerrar, ocorrencia, justifiativa);
        }
    }
    return publico;
})();