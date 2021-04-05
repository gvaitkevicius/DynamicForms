
var lista_select = [];
var lista_select_tipo = [];
var lista_menuInsert = [];
var lista_estrutura = {};

// mostra o menu para consultar e salvar o command sql
function mostrarSQL(num) {
    pesquisarComParametros();
    var sql = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
    if (sql != null) {
    var tabela = sql.entidades[0];
    var comand = JSON.parse(localStorage.getItem('SQL')); 
        $.ajax({
            type: "GET",
            url: "/PlugAndPlay/Dicionario/SepararChaveTabela",
            timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
            data: { tabelaNome: tabela, sql: comand, entidadesJSON: JSON.stringify(sql.entidades) },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.msg == "") {
                    var sqlFinal = " ";
                    sqlFinal = result.sqlNovo + pegarFiltros(result.chavePkAlias, result.chavePK);
                }
                else
                    alert(result.msg);

                // inserir dados no menu.
                $(function () {
                    $('#comandSql').val(sqlFinal)
                });

            }

        })
        if(num != 1)
           expandirMenuSQL();
    }
    else if (num != 1)
       expandirMenuSQL();

}

function listarColunas(nomeTabela) {
      $.ajax({
            type: "GET",
            url: "/PlugAndPlay/Dicionario/listarColunas",
            timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
            data: { nomeTabela: nomeTabela },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                var listaC = document.getElementById("listC");
                var linhas = "";
                for (var i = 0; i < result.length; i++) {
                    var template = `<option onclick="selecionarCampo('${result[i].Name}', '${result[i].Type}')" type="${result[i].Type}"
                    value="${result[i].Name}"
                    id="${result[i].Name}" >${result[i].Name}</option>`;
                    linhas += template;
                    
                }
                if (linhas == "") {
                    // linhas = `<tr><td colspan="3">Sem resultado.</td></tr>`
                }
                listaC.innerHTML = linhas;
            }
        });

      adicionarEntidadeViewSQL(nomeTabela);
}


function listarRelacoes(nomeTabela) {
        $.ajax({
            type: "GET",
            url: "/PlugAndPlay/Dicionario/listarRelacoes",
            timeout: 0, // 0 - sem tempo limite. Ou pode colocar outro valor em milessegundos
            data: { nomeTabela: nomeTabela },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                var listaC = document.getElementById("listE");
                var linhas = `<option onclick="listarColunas('${nomeTabela}')" value="${nomeTabela} " id="${nomeTabela}" >${nomeTabela}</option>`;
                for (var i = 0; i < result.length; i++) {
                    /**/
                    var template = `<option onclick="listarColunas('${result[i].Entity.Name}')" value="${result[i].Entity.Name}" id="${result[i].Entity.Name}" >${result[i].Entity.Name}</option>`
                    /**/
                    linhas += template;
                }
                if (linhas == "") {
                    // linhas = `<tr><td colspan="3">Sem resultado.</td></tr>`
                }
                listaC.innerHTML = linhas;
            }
        });
}

// atualiza o select quando uma coluna é inserida
function atualizaComboBox(nomeCampo) {
    let length = $(".linhaPesquisa").length;

        for (let i = 0;  i< length; i++) {
            var html = `<option value="${nomeCampo}" id="${nomeCampo}" >${nomeCampo}</option>`;
            $("#divSelectParam" + i).append(html);
        }
    
}

// add primeira vez as colunas no select quando uma linha for inserida depois.
function insereComboBox(idAtual) {
    for (let i = 0; i < lista_select.length; i++) {
        var html = `<option value="${lista_select[i]}" id="${lista_select[i]}" >${lista_select[i]}</option>`;
        $("#divSelectParam" + idAtual).append(html);
    }
}

function selecionarCampo(nomeCampo,tipo) {
        var listaS = document.getElementById("listS");
        adicionarCamposViewSQL(nomeCampo);
        var campo = listaS.querySelector("#" + nomeCampo);
        if (campo == undefined || campo == null) {
            var template = `<option value="${nomeCampo}"  id="${nomeCampo}" >${nomeCampo}</option>`;      
            listaS.innerHTML += template;
            lista_select.push(nomeCampo);
            lista_select_tipo.push(tipo);
            atualizaComboBox(nomeCampo);
        }
       
}

 function refreshPage() {
        window.location.reload();
     localStorage.clear();
     lista_select = [];
     lista_select_tipo = [];
}



// verifica se uma coluna ja foi add
function verificarSeEstaAdicionado(arr, str) {
    var adicionado = false;
    arr.forEach(function (ec) {
        if (ec.localeCompare(str) == 0) {
            adicionado = true;
        }
    })
    return adicionado;
}

// Pegar os parametros que foi inserido para a consulta.
function pesquisarComParametros() {
    linhaParametro = {};
    linhaParametro.coluna = [];
    linhaParametro.filtro = [];
    linhaParametro.operador = [];
    linhaParametro.pesquisaTxt = [];
    linhaParametro.tipo = [];

    let length = $(".linhaPesquisa").length;
    for (let i = 0; i < length; i++) {
        linhaParametro.coluna.push($(`#divSelectParam${i} option:selected`).val());
        linhaParametro.filtro.push($(`#divfiltro${i} option:selected`).val());
        linhaParametro.operador.push($(`#divoperador${i} option:selected`).val());
        linhaParametro.pesquisaTxt.push($(`#sel${i}`).val());
        linhaParametro.tipo.push(lista_select_tipo[i])
        
    }
    localStorage.setItem("LinhaParametro", JSON.stringify(linhaParametro));

}

// MONTAGEM DO SQL NA TELA
function montarSQLView() { 
    EntidadesCamposEscolhidos = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
        var sql = "";
        // Tratamento das colunas
        if (EntidadesCamposEscolhidos.campos == undefined || EntidadesCamposEscolhidos.campos.length == 0) {
            sql = "SELECT * FROM ";
        } else {
            sql = "SELECT ";
            for (var i = 0; i < EntidadesCamposEscolhidos.campos.length; i++) {
                if (i == EntidadesCamposEscolhidos.campos.length - 1) {
                     sql += EntidadesCamposEscolhidos.campos[i];                   
                }
                else
                {              
                    sql += EntidadesCamposEscolhidos.campos[i] + ", ";                   
                }
            }
            sql += " FROM "
         }
        // tratamento das entidades
        if (EntidadesCamposEscolhidos.entidades != undefined) {
            if (EntidadesCamposEscolhidos.entidades.length > 0) {
                sql += EntidadesCamposEscolhidos.entidades[0]; 

                for (var i = 1; i < EntidadesCamposEscolhidos.entidades.length; i++) {
                    alias = EntidadesCamposEscolhidos.entidades[i];
                    sql += " INNER JOIN " + EntidadesCamposEscolhidos.entidades[i] + " " + alias + " ON @TABELA.CHAVE=TABELA.CHAVE@ ";
                }
            }
        }
        localStorage.setItem("SQL", JSON.stringify(sql));
        localStorage.setItem("EntidadesCamposEscolhidos", JSON.stringify(EntidadesCamposEscolhidos));
    document.getElementById('comandSql').innerHTML = sql;
}
function adicionarEntidadeViewSQL(entidade) {
        EntidadesCamposEscolhidos = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
        if (EntidadesCamposEscolhidos == null) {
            EntidadesCamposEscolhidos = {};
            EntidadesCamposEscolhidos.entidades = [];
        }
        if (!verificarSeEstaAdicionado(EntidadesCamposEscolhidos.entidades, entidade)) {
            EntidadesCamposEscolhidos.entidades.push(entidade);
        }
        localStorage.setItem('EntidadesCamposEscolhidos', JSON.stringify(EntidadesCamposEscolhidos));       
        montarSQLView();
}


function adicionarCamposViewSQL(campo) {
    var alias = $('#listE :selected').text();
        EntidadesCamposEscolhidos = JSON.parse(localStorage.getItem('EntidadesCamposEscolhidos'));
        if (EntidadesCamposEscolhidos.campos == undefined) {
            EntidadesCamposEscolhidos.campos = [];
            EntidadesCamposEscolhidos.campos.push(campo);
            if (EntidadesCamposEscolhidos.campos.length == 1) {
                if (alias !== "") {
                    var teste = alias + "." + EntidadesCamposEscolhidos.campos[0];
                    EntidadesCamposEscolhidos.campos[0] = teste;
                }
                else {
                    var teste = EntidadesCamposEscolhidos.entidades[0] + "." + EntidadesCamposEscolhidos.campos[0];
                    EntidadesCamposEscolhidos.campos[0] = teste;
                }
            }
        } else {
                if (alias != "") {
                    if (!verificarSeEstaAdicionado(EntidadesCamposEscolhidos.campos, alias + "." + campo)) {
                        EntidadesCamposEscolhidos.campos.push(alias + "." + campo);
                    }
                }
                else {
                    if (!verificarSeEstaAdicionado(EntidadesCamposEscolhidos.campos, EntidadesCamposEscolhidos.entidades[0] + "." + campo))
                        EntidadesCamposEscolhidos.campos.push(EntidadesCamposEscolhidos.entidades[0] + "." + campo);               
                }       
         }   

        localStorage.setItem('EntidadesCamposEscolhidos', JSON.stringify(EntidadesCamposEscolhidos)); 
        montarSQLView();
}



// gera o painel de parametros
function gerarPesquisaParametros(teste) {

    var select ='<div class="col-xs-3 col-sm-3 fixo"  style="margin-bottom:20px">';
    select += '<select class="form-control" id="divSelectParam" style="width:100% ">';
    select += '<option value="default">Selecione o campo</option>';
    select += '</select>';
    select += '</div>';

    var select2 = '<div class="col-xs-2 col-sm-2 fixo">';
    select2 += '<select class="form-control" id="divfiltro" style="width:110% ">';
    select2 += '<option value="parecido com">parecido com</option>';
    select2 += '<option value="igual a">igual a</option>';
    select2 += '<option value="maior que">maior que</option>';
    select2 += '<option value="maior ou igual a">maior ou igual a</option>';
    select2 += '<option value="menor que">menor que</option>';
    select2 += '<option value="menor ou igual a">menor ou igual a</option>';
    select2 += '<option value="diferente de">diferente de</option>';
    select2 += '</select>';
    select2 += '</div>';


    var input = '<div class="form-group col-xs-3 col-sm-3 fixo" id="">';
    input += '<div class="input-group inputLinha" id="" style="width:200%; margin-left: 15%;">';
    input += '<input type="text" class="form-control" id="sel" placeholder="Digite a pesquisa">';
    input += '<span class="input-group-addon" id="excluirLinhaPes" onclick="excluirPesquisaParam()">';
    input += '<button type="button" class="fa fa-trash" style="background:transparent;border:none"></button></span>';
    input += '<span class="input-group-addon" id="btnNovaLinha" onclick="adicionarLinhaPesquisa()">';
    input += '<button type="button" class="fa fa-plus" style="background:transparent;border:none"></button></span>';
    input += '</div>';
    input += '</div>';
   
  
    var html = '<div class="divLinhas" id="linhas">';
    html += '<div  class="linhaPesquisa collapse navbar-collapse">';

    html += select + select2 + input + '</div></div>';
        
    if(teste == 1)
        return html;
     else
        $('#painelParametros').html(html);
}

// adicionar mais uma linha de parametros
function adicionarLinhaPesquisa(){
    let clone = $(".linhaPesquisa").last().clone();
    var idAtual = 0;
    if (clone != undefined) {
        idAtual = clone.children().eq(0).children().prop('id');
        idAtual = parseFloat(idAtual.split("divSelectParam")[1]);
        idAtual++;
    }
    var str = '';
    var teste = 1;
    str = gerarPesquisaParametros(teste);

    $("#painelParametros").append(str);

    let length = $(".linhaPesquisa").length;
    let select_condicao = ` <select style="margin-left: 20%;" class='form-control' id='divoperador${length - 2}'>
        <option value='AND'>E</option>
        <option value='OR'>OU</option>
    </select>`;

    $('#btnNovaLinha').eq(0).remove(); //remove o botao de adicionar
    //adiciona os novos botoes
    $(`.inputLinha`).eq(length - 2).append(select_condicao);
    configurarIdsLinha();
    insereComboBox(idAtual);
    

 
}

//insere os id das linhas do painel de parametros
function configurarIdsLinha() {
    //configura o id dos selects de coluna
    $(`[id^='divSelectParam']`).each(function (index) {
        $(this).attr('id', `divSelectParam${index}`);
    });

    //configura o id dos selects de condicao
    $(`[id^='divfiltro']`).each(function (index) {
        $(this).attr('id', `divfiltro${index}`);
    });

    //configura o id dos inputs de valor
    $(`[id^='sel']`).each(function (index) {
        $(this).attr('id', `sel${index}`);
    });


    //configura o id do select de operador
    $(`[id^='divoperador']`).each(function (index) {
        $(this).attr('id', `divoperador${index}`);
    });

    //configura o id e o onclick do btn de remover linha
    $(`[id^='excluirLinhaPes']`).each(function (index) {
        $(this).attr('id', `excluirLinhaPes${index}`);
        $(this).attr('onclick', `excluirPesquisaParam('${index}')`);
    });


}


// excluir linha inserido
function excluirPesquisaParam(index) {
    let length = $(".linhaPesquisa").length - 1;

    if ((length - 1) <= -1) {
        return;
    }
    //nao deixa excluir a linha principal e nem quando so tem 1
    else if (index != length && (length - 1) != -1) { 
        $(".linhaPesquisa").eq(index).remove();
    } else if (index == length ) { //esta tentando excluir a linha mas ainda existe outra acima
        //passa os botoes pra cima
        $(".linhaPesquisa").eq(index).remove();
        adicionarBotoesLinha(length - 1);
    }



    configurarIdsLinha();

}

function adicionarBotoesLinha(index) {

    let btnAdicionarLinha = elementoButton('', 'fa fa-plus', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    btnAdicionarLinha = elementoSpan(btnAdicionarLinha, 'input-group-addon', 'btnNovaLinha', '', `onclick="adicionarLinhaPesquisa()"`);


    //verifica se o botao nao existe(permitido apenas um botao de cada, na ultima linha), para entao adiciona-los na ultima linha   
    if (!$(`#btnNovaLinha`).length) $(".inputLinha").last().append(btnAdicionarLinha);

      $(`#divoperador${index}`).remove();
}

function expandirMenuSQL() {

    var container = document.getElementById("menuSQL");
    if (container.style.display === "none") {
        container.style.display = "block";
    } else {
        container.style.display = "none";
    }

}

function inserirConsultaDicionario() {
    var dados_lista = [];
    var classeName = 'DynamicForms.Areas.PlugAndPlay.Models.Consultas';
    lista_estrutura.CON_ID = $('#conID').val();
    lista_estrutura.CON_COMAND = $('#comandSql').val();
    lista_estrutura.CON_DESCRICAO = $('#conDESCRICAO').val();
    lista_estrutura.CON_GRUPO = $('#conGRUPO').val();
    lista_estrutura.CON_CONEXAO = $('#conCONEXAO').val();
    lista_estrutura.CON_TITULO = $('#conTITULO').val(); 
    lista_estrutura.CON_TIPO = $('#conTIPO').val();
    lista_estrutura.IndexClone = undefined;
    lista_estrutura.PlayAction = 'insert';
    lista_estrutura.PlayMsgErroValidacao = undefined;

     dados_lista.push(lista_estrutura);
    insertBanco(dados_lista, classeName, 0); 

  

    dados_lista = [];
    lista_estrutura = {};
}


gerarPesquisaParametros();
configurarIdsLinha();