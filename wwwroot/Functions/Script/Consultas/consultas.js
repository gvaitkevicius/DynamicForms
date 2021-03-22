
function executarConsulta() {
    var CON_ID = $("#idConsulta").val();
    var qtdParametros = $("#qtdParametros").val();

    listaParametros = [];
    for (i = 0; i < qtdParametros; i++) {
        var parametro = {
            name: $("#inputName-" + i).val(),
            value: $("#inputParam-" + i).val()
        }
        listaParametros.push(parametro);
    }

    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Consultas/ExecutarConsulta",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { idConsulta: CON_ID, parametrosJSON: JSON.stringify(listaParametros) },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.msg != "") {
                alert(result.msg);
            } else {
                var html = '<div style="margin-bottom: 10px;"><span style="margin-right: 10px;">' + (result.resultadoQuery.length - 1) + ' Registros</span>';
                html += '<button id="selecionar-tabela" class="fa fa-clone" aria-hidden="true" title="copiar dados da tabela" onclick="copiarDadosTabela()"></button></div>';
                html += '<div class="tableFixHead">';
                html += '<table border="1" id="resultQueryTable">';
                //Cabeçalho
                html += '<thead>';
                html += '<tr>';
                for (i = 0; i < result.resultadoQuery[0].length; i++) {
                    html += '<th>' + result.resultadoQuery[0][i] + '</th>';
                }
                html += '</tr>';
                html += '</thead>';
                //Corpo
                html += '<tbody>';
                for (i = 1; i < result.resultadoQuery.length; i++) {
                    html += '<tr>';
                    for (j = 0; j < result.resultadoQuery[i].length; j++) {
                        html += '<td>' + result.resultadoQuery[i][j] + '</td>';
                    }
                    html += '</tr>';
                }
                html += '</tbody>';
                html += '</table>';
                html += '</div>';

                $('#divResultQuey').html(html);
            }
        },
        error: OnError,
        complete: function () {
            Carregando.fechar();
        }
    })
}

function exportar() {
    var CON_ID = $("#idConsulta").val();
    var qtdParametros = $("#qtdParametros").val();

    listaParametros = [];
    for (i = 0; i < qtdParametros; i++) {
        var parametro = {
            name: $("#inputName-" + i).val(),
            value: $("#inputParam-" + i).val()
        }
        listaParametros.push(parametro);
    }

    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Consultas/Exportar",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { idConsulta: CON_ID, parametrosJSON: JSON.stringify(listaParametros) },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.msg != "") {
                alert(result.msg);
            } else {
                window.open(result.downloadUrl);
            }
        },
        error: OnError,
        complete: function () {
            Carregando.fechar();
        }
    })
}


var Tabela = {
    selecionarTabela: function (el) {
        var body = document.body, range, sel;
        if (document.createRange && window.getSelection) {
            range = document.createRange();
            sel = window.getSelection();
            sel.removeAllRanges();
            try {
                range.selectNodeContents(el);
                sel.addRange(range);
            } catch (e) {
                range.selectNode(el);
                sel.addRange(range);
            }
        } else if (body.createTextRange) {
            range = body.createTextRange();
            range.moveToElementText(el);
            range.select();
        }
        try {
            document.execCommand('copy');
            range.blur();
        } catch (error) {
            // Exceção aqui
        }
    }
}

function copiarDadosTabela() {

    var selecionaTabelaBtn = document.querySelector("#selecionar-tabela");
    var tabelaDeDados = document.querySelector("#resultQueryTable");

    Tabela.selecionarTabela(tabelaDeDados);

}

$(document).ready(function () {
    var url_total = window.location.href;
    var param = url_total.split("&");
    param.splice(0, 1);
    for (let i = 0; i < param.length; i++){
        var temp = param[i].split("=");

        $('.consulta_' + temp[0]).val(temp[1]);
        //$('#inputParam-0').val(temp[1]);
    }
});
