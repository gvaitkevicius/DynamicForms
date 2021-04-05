
var qtdeParam = 0;

// verifica se é consulta de uma ou mais tabela e faz o tratamento correto.
function iniciarConsulta() {
    var sql = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
    var tabela = sql.entidades[0];
    var comand = JSON.parse(localStorage.getItem('SQL'));

    nomeTabela = [];
    for (i = 0; i < sql.entidades.length; i++) {
        nomeTabela.push(sql.entidades[i])
    }


    if (sql.entidades.length > 1) {
        $.ajax({
            type: "GET",
            url: "/PlugAndPlay/Dicionario/SepararChaveTabela",
            timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
            data: { tabelaNome: tabela, sql: comand, entidadesJSON: JSON.stringify(sql.entidades)},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.msg == "") {
                    var sqlFinal = " ";
                    sqlFinal = result.sqlNovo + pegarFiltros(result.chavePkAlias, result.chavePK);
                    executaConsulta(sqlFinal);
                }
                else
                    alert(result.msg);
            }

        })
    }
    else {
        var sqlFinal = " ";
        sqlFinal = comand + pegarFiltros(tabela);
        executaConsulta(sqlFinal);
    }
    
}

// executa query db
function executaConsulta(comand) {

    var parametro = {
        name: $("#divSelectPesquisa option:selected").val(),
        value: $("#inputParam").val(),
        qtde: qtdeParam
    }

    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Dicionario/ExecutarConsulta",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { sql: comand, pesquisaParamJSON: JSON.stringify(parametro)},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.msg != "") {
                alert(result.msg);
            } else {
                var html = '<div style ="margin-bottom: 10px;"><span style="margin-right: 10px;">' +
                    (result.resultadoQuery.length - 1) + ' Registros</span>';
                html += '<button id="selecionar-tabela" class="fa fa-clone" aria-hidden="true" title = "copiar dados da tabela" onclick = "copiarDadosTabela()" ></button ></div >';
                html += '<div class="tableFixHead">';
                html += '<table border="1" id="resultQueryTable">';
                // cabeçalho
                html += '<thead>';
                html += '<tr>';
                for (i = 0; i < result.resultadoQuery[0].length; i++) {
                    html += '<th>' + result.resultadoQuery[0][i] + '</th>';
                }
                html += '</tr>';
                html += '</thead>';
                //corpo
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


function exportarDados() {
    var comand = JSON.parse(localStorage.getItem('SQL'));


    var parametro = {
        name: $("#divSelectPesquisa option:selected").val(),
        value: $("#inputParam").val(),
        qtde: qtdeParam
    }

    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Dicionario/Exportar",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { sql: comand, pesquisaParamJSON: JSON.stringify(parametro)},
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

function atualizarComboBoxPesquisa() {
    var linhas = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
    var selectPes = document.getElementById("divSelectPesquisa");
    var nomeTabela = document.getElementById("nomeTabela");
    nomeTabela.innerHTML = linhas.entidades[0];
    var html = "";
    if (linhas.campos !== undefined) {
        for (var i = 0; i < linhas.campos.length; i++) {
            var template = `<option  value="${linhas.campos[i]}" id="${linhas.campos[i]}" >${linhas.campos[i]}</option>`
            html += template;
        }
        selectPes.innerHTML = html;
    }
    else {
        var nomeTabela = linhas.entidades[0];
        $.ajax({
            type: "GET",
            url: "/PlugAndPlay/Dicionario/listarColunas",
            timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
            data: { nomeTabela: nomeTabela},
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    if (linhas.entidades.length === 1) {
                        var template = `<option value="${result[i].Name}" id="${result[i].Name}" >${result[i].Name}</option>`;
                        html += template;
                    }
                    else {
                        var template = `<option value="${linhas.entidades[0] + "." + result[i].Name}" id="${linhas.entidades[0] + "." + result[i].Name}" >${linhas.entidades[0] + "." + result[i].Name}</option>`;
                        html += template;
                    }
                }
                selectPes.innerHTML = html;
            }
        });

    }

}


function pegarFiltros(tabelaChave,chavePK) {
    var _linhas = JSON.parse(localStorage.getItem('LinhaParametro'));
    var res = "";
    var compara = " WHERE 1=1 AND "
    let j = 0;
     
    for (let i = 0; i < _linhas.coluna.length; i++) {
        if (i > _linhas.coluna.length)
            break;

        if (_linhas.coluna[i] === chavePK)
            _linhas.coluna[i] = tabelaChave;
       

        if (_linhas.coluna[i] !== "default" && _linhas.pesquisaTxt[i] !== "") {
            j++; qtdeParam++;
            if (_linhas.filtro[i] === 'parecido com') {
                res += _linhas.coluna[i] + ' LIKE ' + "'%" + _linhas.pesquisaTxt[i] + "%' ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }
            else if (_linhas.filtro[i] === 'igual a') {
                res += _linhas.coluna[i] + ' = ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }

            else if (_linhas.filtro[i] === 'maior que') {
                res += _linhas.coluna[i] + ' > ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }
            else if (_linhas.filtro[i] === 'maior ou igual a') {
                res += _linhas.coluna[i] + ' >= ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }
            else if (_linhas.filtro[i] === 'menor que') {
                res += _linhas.coluna[i] + ' < ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }
            else if (_linhas.filtro[i] === 'menor ou igual a') {
                res += _linhas.coluna[i] + ' <= ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }
            else if (_linhas.filtro[i] === 'diferente de') {
                res += _linhas.coluna[i] + ' <> ' + _linhas.pesquisaTxt[i] + " ";
                if (_linhas.operador[i] !== null)
                    res += _linhas.operador[i] + " ";
            }

        }
    }

    if (j !== 0)
        return compara + res;
    else
       return res;
}



function copiarDadosTabela() {

    var selecionaTabelaBtn = document.querySelector("#selecionar-tabela");
    var tabelaDeDados = document.querySelector("#resultQueryTable");

    Tabela.selecionarTabela(tabelaDeDados);

}

atualizarComboBoxPesquisa();