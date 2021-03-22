function manualInsertClp(maquina, equipe) {
    Dados = { MaqId: maquina, EquId: equipe };
    $.ajax({
        url: '/plugandplay/Medicoes/CreateCLPManual',
        type: "POST",
        data: { Dados: JSON.stringify(Dados) },
        dataType: "json",
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                var form = '';

                form += '<div class="form-horizontal">';
                form += '   <div class="form-group">';
                form += '       <label for="MaquinaId" class="control-label col-md-2">Máquina:</label>';
                form += '       <div class="col-md-4">';
                form += '           <span class="form-control" id="MaquinaId"disabled="disabled">' + maquina + '</span>';
                form += '       </div>';
                form += '       <label for="Quantidade" class="control-label col-md-2">Quantidade De Pulsos:</label>';
                form += '       <div class="col-md-4">';
                form += '           <input type="number" id="Quantidade" name="DataInicio" value="" class="form-control" />';
                form += '       </div>';
                form += '   </div>';
                form += '   <div class="form-group">';
                form += '       <label for="DataInicio" class="control-label col-md-2">Data de Início:</label>';
                form += '   <div class="col-md-4">';
                form += '           <input type="datetime-local" id="DataInicio" name="DataInicio" class="form-control">';
                form += '   </div>';
                form += '       <label for="DataFim" class="control-label col-md-2">Data Fim:</label>';
                form += '       <div class="col-md-4">';
                form += '           <input type="datetime-local" id="DataFim" name="DataFim" class="form-control">';
                form += '       </div>';
                form += '   </div>';
                form += '   <div class="form-group">';
                form += '       <label for="Grupo" class="control-label col-md-2">Grupo:</label>';
                form += '       <div class="col-md-4">';
                form += '           <span class="form-control" id="Grupo" disabled="disabled">' + result.Grupo + '</span>';
                form += '       </div>';
                form += '       <label for="ClpOrigem" class="control-label col-md-2">Origem (CLP):</label>';
                form += '       <div class="col-md-4">';
                form += '           <span class="form-control" id="ClpOrigem" disabled="disabled">' + result.Origem + '</span>';
                form += '       </div>';
                form += '   </div>';
                form += '   <div class="form-group">';
                form += '       <label for="Emissao" class="control-label col-md-2">Data de Emissão:</label>';
                form += '       <div class="col-md-4">';
                form += '           <span class="form-control" disabled="disabled">' + result.DataEmissao + '</span>';
                form += '       </div>';
                form += '   </div>';
                form += '</div>';

                $('#ClpMMBody').html(form);
                $('#modalClpMedicao').modal('show');
            }
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function saveManualMeasurement() {
    var MaquinaId = $('#MaquinaId').text();
    var DataInicio = $('#DataInicio').val();
    var DataFim = $('#DataFim').val();
    var Quantidade = $('#Quantidade').val();
    var Grupo = $('#Grupo').text();
    var Dados = { MaquinaId, DataInicio, DataFim, Quantidade, Grupo };

    var log = validarCampos(Quantidade, DataInicio, DataFim);
    if (log.st == 'OK') {
        Carregando.abrir('Processando ...');
        $.ajax({
            type: 'POST',
            url: '/plugandplay/Medicoes/InsertCLPManual',
            data: { dados: JSON.stringify(Dados) },
            dataType: "json",
            traditional: true,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.st === "OK") {
                    $('#modalClpMedicao').modal('hide');
                    Monitoramento.iniciar(MaquinaId);
                    Conexao.obterLinhaTempo(MaquinaId);
                } else {
                    alert(result.st);
                }
            },
            error: function (result) {
                alert(result.st)
            },
            complete: function () {
                Carregando.fechar();
            }
        });
    }
    else {
        alert(log.msg);
    }
}

function validarCampos(Quantidate, dIni, dFim) {
    var flag = true;
    var msg = '';
    if (dIni == "") {
        msg += 'Uma data/hora Inicial deve ser informada para o Apontamento.';
        flag = false;
    }
    if (dFim == "") {
        msg += 'Uma data/hora Final deve ser informada para o Apontamento.';
        flag = false;
    }
    if (flag) {
        var nDataI = dIni.split(' ');
        var nDataF = dFim.split('T');

        var horaI = nDataI[1];
        //Quando o valor é inserido direto pelo operadore a data é passada em formato EN e não PT_BR
        //Forçando um tratamento distindo entre formatos de entrada da data de inicio.
        if (typeof horaI == "undefined") {
            nDataI = dIni.split('T');
            horaI = nDataI[1];
            var dataI = nDataI[0];
            var horaIS = horaI.split(':');
            var dataIS = dataI.split('-');
            var _DataInicio = new Date(dataIS[0], (dataIS[1]) - 1, dataIS[2], horaIS[0], horaIS[1], 0);
        } else {
            var dataI = nDataI[0];
            var horaIS = horaI.split(':');
            var dataIS = dataI.split('/');
            var _DataInicio = new Date(dataIS[2], (dataIS[1]) - 1, dataIS[0], horaIS[0], horaIS[1], horaIS[2]);
        }

        var horaF = nDataF[1];
        var dataF = nDataF[0];

        var horaFS = horaF.split(':');
        var dataFS = dataF.split('-');

        var _DataFim = new Date(dataFS[0], (dataFS[1]) - 1, dataFS[2], horaFS[0], horaFS[1], 0);;

        if (typeof Quantidate == "undefined" || Quantidate == '') {
            msg += 'Uma quantidade deve ser informada.'
            flag = false;
        }
        if (flag && Quantidate < 0) {
            msg += 'A Quantidadede deve ser um numero positivo'
            flag = false;
        }
        if (typeof _DataInicio == "undefined" || _DataInicio == 'Invalid Date') {
            msg += 'Uma data/hora Inicial deve ser informada para o Apontamento.'
            flag = false;
        }

        if (typeof _DataFim == "undefined" || _DataFim == 'Invalid Date') {
            msg += 'Uma data/hora final deve ser informada para o Apontamento.'
            flag = false;
        }

        if (flag && _DataFim > Date.now()) {
            msg += 'A data/hora de fim não pode ser maior que a data/hora atual'
            flag = false;
        }
        if (flag && _DataFim < _DataInicio) {
            msg += 'A data/hora de fim não pode ser menor que a data/hora de início'
            flag = false;
        }
    }
    
    var st = (flag) ? 'OK' : 'ERRO';
    dados = { st, msg };
    return dados;
}