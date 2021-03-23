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
            data: { tabelaNome: tabela, sql: comand, entidadesJSON: JSON.stringify(sql.entidades) },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.msg == "") 
                    executaConsulta(result.sqlNovo);              
                else
                    alert(result.msg);
            }
          
        })
    }
    else
        executaConsulta(comand);
    
}


function executaConsulta(comand) {
   // var comand = JSON.parse(localStorage.getItem('SQL'));
    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Dicionario/ExecutarConsulta",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { sql: comand },
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
    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: "/PlugAndPlay/Dicionario/Exportar",
        timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
        data: { sql: comand },
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

function pegarFiltros(input, selecionados, condicoes) {
    var res = "";
    var operadores = [];

    //!!NAO REMOVER COMENTARIO !
    // var parenteses_inicial_selecionados = [];
    // $("[id^='parenteses_inicial'][data-ativo='true']").each(function(index, element){
    //     parenteses_inicial_selecionados.push($(element).attr('id'));
    // });

    // var parenteses_final_selecionados = [];
    // $("[id^='parenteses_final'][data-ativo='true']").each(function(index, element){
    //     parenteses_final_selecionados.push($(element).attr('id'));
    // });

    $("[id^='divoperador']").each(function () {
        operadores.push($(this).val());
    });

    for (let i = 0; i < selecionados.length; i++) {
        if (i > input.length)
            break;
        //!!NAO REMOVER COMENTARIO !
        // var parenteses_inicial_na_linha = "";
        // var parenteses_final_na_linha = "";

        // if(parenteses_inicial_selecionados != undefined && parenteses_final_selecionados != undefined){

        //     parenteses_inicial_selecionados.forEach(function(index){

        //         //pega o ultimo digitio do id (um numero contendo a linha que pertence)
        //         if(index.substring(index.length - 1) == i) //se na lihnha atual tiver um parenteses inicial, define a variavel
        //             parenteses_inicial_na_linha = "(";
        //     });

        //     parenteses_final_selecionados.forEach(function(index){

        //         if(index.substring(index.length - 1) == i) //se na lihnha atual tiver um parenteses final, define a variavel
        //             parenteses_final_na_linha = ")";
        //     });

        // }

        coluna = selecionados[i];
        if (condicoes[i] === 'parecido com')
            res += coluna + '%' + input[i];
        else if (condicoes[i] === 'igual a')
            res += coluna + '=' + input[i];
        else if (condicoes[i] === 'maior que')
            res += coluna + '>' + input[i];
        else if (condicoes[i] === 'maior ou igual a')
            res += coluna + '>=' + input[i];
        else if (condicoes[i] === 'menor que')
            res += coluna + '<' + input[i];
        else if (condicoes[i] === 'menor ou igual a')
            res += coluna + '<=' + input[i];
        else if (condicoes[i] === 'diferente de')
            res += coluna + '<>' + input[i];
        else if (condicoes[i] === 'between') {
            res += coluna + '><' + input[i];
        }


        if (i <= operadores.length - 1)
            res += operadores[i] == 0 ? '&;' : operadores[i] == 1 ? '|;' : null;
    }

    return res;
}

function copiarDadosTabela() {

    var selecionaTabelaBtn = document.querySelector("#selecionar-tabela");
    var tabelaDeDados = document.querySelector("#resultQueryTable");

    Tabela.selecionarTabela(tabelaDeDados);

}

