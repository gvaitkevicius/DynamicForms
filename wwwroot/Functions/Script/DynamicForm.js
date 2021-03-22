/*
    Anotações importantes:
    -Todas as variáveis locais com o nome 'str_html' indicam código html que será exibido na tela
*/
var url = "";

var lista_estrutura = []; //Estrutura de dados 
var lista_estrutura_secundarias = []; //Tabelas secundárias
var lista_estrutura_insert = [];
var lista_estrutura_1N = [];
var lista_estrutura_1N_secundarias = [];
var lista_estrutura_insert_1N = [];
var lista_estrutura_insert_1N_secundarias = [];
var lista_aux_secundaria = [];

var lista_methods_class = [];

var lista_indices_cabecalho = [];
var lista_indices_estrutura = [];
var lista_indices_dados = [];
var lista_indices_campos_visiveis = [];

var lista_indices_display1N = []; //Lista com os campos a serem mostrados no display 1N
var lista_indices_display = []; //Lista com os campos a serem mostrados no display
var lista_dados = []; //Lista com os dados ao buscar
var lista_dados_totais = []; //Lista com os dados completos
var lista_dados_totais_classes = [];
var dados_pesquisado_1N = [];
var dados_completos_1N = [];
var result_insert = [];

var lista_colunas = [];
var selecionados = [];
var lista_abas = [];
var lista_chaves_primarias = [];
var lista_paginas_1N = [];

var win_pai = null;
var campos_visiveis = 0;
var qtd_paginas = 1;
var tamanho_pagina = 10;
var qtd_dados_pesquisa = 0;
var id_exclusao = -1;
var namespace = "";
var tipo_de_tela = 1;
var funcao_pesquisa = "";
var result_global;
var qtd_display = 6;
var filtros_pesquisa_principal = ""; //Armazena o filtro da pesquisa principal.
var input_pesquisa_principal = "";
var result_log = [];
var masks = [];
/////VARIÁVEIS GLOBAIS ESPECÍFICAS;
//Todos os trechos com o comentário //APS_Comment envolvem a interface APS
var global_modal_id = "";
var indicator = -1;


//#################################### FUNÇÕES DE  CRIAÇÃO DE TELAS ###########################################//

function criarPaginaInicial(_url) {
    limparDados();
    url = _url;
    funcao_pesquisa = _url.indexOf('?') != -1 ? _url.substring(0, _url.indexOf('?')) : "";
    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: _url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                separarDados(result);
                pegarCamposPesquisa(lista_estrutura);
                pegarListaIndicesEstrutura(lista_estrutura);
                tipo_de_tela = 1;
                gerarHTMLInicial(); //Gerar o HTML principal
                gerarModalFiltros(result.prefencias);

                //se encontrar uma preferencia com tipo === buffer, desmonta ela e exibe na tela
                let ultima_pesquisa = result.prefencias.filter(function (value) { return value.PRE_TIPO === "BUFFER" })[0];
                if (ultima_pesquisa != "" && ultima_pesquisa != null && ultima_pesquisa != undefined) desmontarStringPesquisa(ultima_pesquisa.PRE_VALOR);

            } else {//entrou no try catch do controler 
                mostraDialogo(result.st, 'danger', 3000);
                //$("#insert" + num).append(result.st);
                $(".exibir").html(result.st);
            }
        },
        error: function (result) {
        },
        complete: function () {
            Carregando.fechar();
        }
    })

    //é necessário resetar a variavel de lista_indices_display, pois ela pode guardar informações de uma pagina ao mudar para outra
    window.addEventListener('hashchange', function () {
        lista_indices_display = [];
    }, false);

    //$("[onclick^='criarPagina']").on('click', function(){ lista_indices_display = [] });
}

//essa função é responsável por implementar o JS do right click nos campos do dynamic
function adicionarEventosRightClick() {
    const menu = document.querySelector(".menu");
    let menuVisible = false;

    //função para esconder/mostrar o menu
    const toggleMenu = command => {
        menu.style.display = command === "show" ? "block" : "none";
        menuVisible = !menuVisible;
    };

    //função para mover o menu
    const setPosition = ({ top, left }) => {
        menu.style.left = `${left}px`;
        menu.style.top = `${top}px`;
        toggleMenu("show");
    };

    //função para setar o link de abrir a tabela de logs com o campo, e de abrir a tabela de log do registro especifico
    const setLink = ({target, index_estrutura_clicada}) => {
        let chaves_primarias = pegarChavesPrimarias(lista_estrutura);
        let str_chaves_primarias = '';

        //se não tiver nada na lista_dados, quer dizer que esta clicano no campo em uma tela de insert, portanto, nao precisa criar a pesquisa de KEY
        for (let i = 0; i < chaves_primarias.length; i++){

            let campo = lista_estrutura.Propriedades[chaves_primarias[i]].Identifier;
            let valor = lista_dados[index_estrutura_clicada][campo];
            str_chaves_primarias += campo + ': ' + valor;
            if(i < chaves_primarias.length - 1) str_chaves_primarias += ', ';
        }


        let ver_log = `eventoAbrirNovaAba("DynamicForms.Models.T_LOGS_DATABASE", "LOGS_COLUMN=${target}");`
        let ver_log_registro = `eventoAbrirNovaAba("DynamicForms.Models.T_LOGS_DATABASE", "LOGS_COLUMN=${target}&;LOGS_KEY=${str_chaves_primarias}");`
        $(`#ver_log`).attr('onclick', ver_log);
        $(`#ver_log_registro`).attr('onclick', ver_log_registro);
    };

    //se o menu estiver vizivel, esconde
    window.addEventListener("click", e => {
        if (menuVisible) toggleMenu("hide");
    });

    //se clicar o direito num elemento com a classe especificada, move o elemento e o revela.
    $(document).on('contextmenu', function (e) {
        if (e.target.classList.contains("menu-right-click")) {
            e.preventDefault();

            //recupera o index da estrutura clicada, para conseguir acessar os dados corretos
            let index_estrutura = $(e.target).closest('.tr_pai').attr('index-estrutura');
            const campo_identifier = $('#' + e.target.id).attr('data-column');
            const origin = {
                left: e.pageX,
                top: e.pageY
            };

            setPosition(origin);
            setLink({ target: campo_identifier, index_estrutura_clicada: index_estrutura});

            return false;
        }
    })
}

function realizarTratamentoDeErros(event, jqxhr, settings, thrownError) {

    switch (jqxhr.status) {
        case 401:
            // Sem Autenticação.
            redirecionarPaginaInicial();
            break;
        case 403:
            // Sem Permissão.
            redirecionarPaginaSemAcesso();
            break;
        case 404:
            // Endereço não encontrado
            alert("Página não encontrada.")
            break;
        case 500:
            // Erros como exceção lançada no controller.
            redirecionarPaginaErro(jqxhr.responseText);
            break;
        default:
        // code block
    }
}

function redirecionarPaginaSemAcesso() {
    const origin = window.location.origin;
    const url = `${origin}/Acesso/SemAcesso`;

    window.location.href = url;
}

function redirecionarPaginaInicial() {
    const origin = window.location.origin;
    const url = `${origin}/Acesso/Login`;

    window.location.href = url;
}

function redirecionarPaginaErro(paginaErro = "") {
    document.open();
    document.write(paginaErro);//this will replace the current page with the developer exception page
    document.close();
}



//gera a modal de filtros e preenche a tabela baseado no retorno de preferencias do usuario
function gerarModalFiltros(filtros, origem = "") {

    //#region gera a modal
    let table = `
    <table class='table table-bordered table-striped' id='table_filtros' role='grid' aria-describedy='filtros_info'>
        <thead>
            <tr>
                <th>ID</th>
                <th>Opções</th>
                <th width='100%'>Descrição</th>
                <th width='100%'>Tipo</th>
                <th width='100%'>Valor</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>`;

    let button = elementoButton("", "fa fa-plus", "", "", "", "", "button", "", "", "", "", "background:transparent;border:none");
    let span = elementoSpan(button, "input-group-addon", "btnAdicionarFiltro", "", "onclick", "prepararNovoFiltro('insert', '', 'manual')");
    let text = elementoInputText("", "form-control", "input_descricao", "Adicionar novo filtro");
    let divInputGroup = elementoDiv(text + span, "input-group");
    let divFormGroup = elementoDiv(divInputGroup, "form-group col-md-3 colocapica");

    let header = `<header><h2 class='title pull-left'>PREFERÊNCIAS DE FILTROS</h2></header>`;
    let modal = '<div id="modal-filtros"  class="modal fade" role="dialog" style="z-index:9998 !important;">';
    modal += '<div class="modal-dialog" style="width: 70%;">';
    modal += '<div class="modal-content">';
    modal += `<div class="modal-header">${header}</div>`;
    modal += `<div class="modal-body">  
            <section id='salvar' class=''>
                <div class='panel-body'>
                    <div class='row'>
                        ${divFormGroup} 
                    </div>
                    <div class='row'>
                        ${table}
                    </div>
                </div>
            </section>

    </div>`;

    $('#main-content').append(modal);

    //#region gera e preenche tabela
    filtros.forEach(function (currentValue) {
        adicionarLinhaModalFiltros(currentValue.PRE_ID, currentValue.PRE_DESCRICAO, currentValue.PRE_VALOR, origem, currentValue.PRE_TIPO)
    });
}

var btn = $('#backtop');
$(window).scroll(function () {
    if ($(window).scrollTop() > 200) {
        btn.addClass('show');
    } else {
        btn.removeClass('show');
    }
});

function voltarTopo() {
    $('html, body').animate({ scrollTop: 0 }, '300');
}

var globs = [];
function renderizarBodyMaxTimeOut(_url) {
    url = _url;
    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: _url,
        //timeout: 9999999,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            Carregando.fechar();

            if (result.st === "OK") {
                $(".exibir").html(result);
            } else {
                $(".exibir").html(result); // pendencia tratar menssagem de erro
            }
        },
        error: function (exception) {
            alert(exception);
            globs = exception;
        },
        complete: function () {
            Carregando.fechar();
        }
    })
}

function alterarUrl(_title = "", _url = "/DynamicWeb/Index") {
    var infoPagina =
    {
        title: _title,
        url: _url
    };

    window.history.pushState(infoPagina, _title, _url);
}

function renderizarBody(_url) {
    url = _url;
    Carregando.abrir('Processando ...');
    $.ajax({
        type: "GET",
        url: _url,
        dataType: "html",
        contentType: "application/html; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                $(".exibir").html(result);
            } else {
                $(".exibir").html(result); // pendencia tratar menssagem de erro
            }
        },
        error: OnError,
        complete: function () {
            Carregando.fechar();
        }
    })
}

function criarPaginaInsert(_url, ArrayDeValoresDefault = "", ParametrosConstrutor = "") {
    limparDados();
    Carregando.abrir('Processando ...');
    url = _url;
    $.ajax({
        type: "GET",
        url: url,
        data: { arrayDeValoresDefault: ArrayDeValoresDefault, parametrosMetodoContrutor: ParametrosConstrutor },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            separarDados(result);
            pegarCamposPesquisa(lista_estrutura);
            pegarListaIndicesEstrutura(lista_estrutura);
            campos_visiveis = contarCamposVisiveis(lista_estrutura);

            lista_estrutura_insert = JSON.parse(result.instances);

            tipo_de_tela = 2;
            gerarHTMLInsertDireto(); //Gerar o HTML principal
        },
        error: OnError,
        complete: function () {
            Carregando.fechar();
        }
    })
}

function gerarHTMLInicial() {
    var header = gerarHeaderInicial();
    var div_pesquisa = gerarCamposPesquisaInicial(0);

    section = elementoSection(header + div_pesquisa, "box");
    var str_html = elementoDiv(section, "col-lg-12");

    str_html += '<style>.modal{overflow-y:auto}.colocapica{position:relative;top:50%;transform:translateY(25%)}.nopadding{padding:0;margin:0}.divselect{height:34px}#sub1{height:100%}#sub2{height:100%}#sel1{height:100%}.selectBox select{width:100%}.overSelect{position:absolute;left:0;right:0;top:0;bottom:0}#checkboxes{display:none;border:1px #dadada solid}#checkboxes label{display:block;background-color:#fff}#checkboxes label:hover{background-color:#1e90ff}</style>';
    $(".exibir").html(str_html);
    btn = $('#backtop');
}

function gerarHTMLPesquisa(pagina_selecionada) {
    var header = gerarHeaderInicial();
    var div_pesquisa = gerarCamposPesquisaInicial(0);

    var div_dados = gerarBodyPesquisa();
    div_dados += elementoModalExclusao();
    var div_rodape = gerarRodapePesquisa(pagina_selecionada);
    //var botao_top = elementoBotaoToTop(); //Não ta funcionando

    section = elementoSection(header + div_pesquisa + div_dados + div_rodape, "box");
    var str_html = elementoDiv(section, "col-lg-12");

    str_html += '<style>.modal{overflow-y:auto}.colocapica{position:relative;top:50%;transform:translateY(25%)}.nopadding{padding:0;margin:0}.divselect{height:34px}#sub1{height:100%}#sub2{height:100%}#sel1{height:100%}.selectBox select{width:100%}.overSelect{position:absolute;left:0;right:0;top:0;bottom:0}#checkboxes{display:none;border:1px #dadada solid}#checkboxes label{display:block;background-color:#fff}#checkboxes label:hover{background-color:#1e90ff}</style>';
    //str_html += botao_top;
    $(".exibir").html(str_html);
    btn = $('#backtop');

    //é preciso marcar quais os checkboxes selecionados da lista de display DEPOIS de gerar a página
    marcarCheckboxDisplay(lista_indices_display);
}

function gerarHTMLInsertDireto() {
    var header = gerarHeaderInicial();

    var div_dados = eventoNovo(lista_estrutura, lista_estrutura_insert[0]); //Formulário em si
    div_dados += gerarBotoesFormularioInsert(0, true);
    div_dados = elementoDiv(div_dados, '', 'bodyDadosPesquisado');
    div_dados += elementoModalExclusao();
    //var botao_top = elementoBotaoToTop(); //Não ta funcionando

    section = elementoSection(header + div_dados, "box");
    var str_html = elementoDiv(section, "col-lg-12");

    str_html += '<style>.modal{overflow-y:a0,uto}.colocapica{position:relative;top:50%;transform:translateY(25%)}.nopadding{padding:0;margin:0}.divselect{height:34px}#sub1{height:100%}#sub2{height:100%}#sel1{height:100%}.selectBox select{width:100%}.overSelect{position:absolute;left:0;right:0;top:0;bottom:0}#checkboxes{display:none;border:1px #dadada solid}#checkboxes label{display:block;background-color:#fff}#checkboxes label:hover{background-color:#1e90ff}</style>';
    //str_html += botao_top;
    $(".exibir").html(str_html);
    btn = $('#backtop');


    //depois de adicionar os campos, deve aplicar as mascaras pendentes
    aplicarMascaras();
    adicionarEventosRightClick();

}

function gerarHTMLInsert() {
    var header = gerarHeaderInicial();
    var div_pesquisa = gerarCamposPesquisaInicial(0);

    lista_dados = [];
    var div_dados = eventoNovo(lista_estrutura, lista_estrutura_insert); //Formulário em si
    div_dados += gerarBotoesFormularioInsert(0);
    div_dados += elementoModalExclusao();
    div_dados = elementoDiv(div_dados, '', 'bodyDadosPesquisado');
    //var botao_top = elementoBotaoToTop(); //Não ta funcionando

    section = elementoSection(header + div_pesquisa + div_dados, "box");
    var str_html = elementoDiv(section, "col-lg-12");

    str_html += '<style>.modal{overflow-y:auto}.colocapica{position:relative;top:50%;transform:translateY(25%)}.nopadding{padding:0;margin:0}.divselect{height:34px}#sub1{height:100%}#sub2{height:100%}#sel1{height:100%}.selectBox select{width:100%}.overSelect{position:absolute;left:0;right:0;top:0;bottom:0}#checkboxes{display:none;border:1px #dadada solid}#checkboxes label{display:block;background-color:#fff}#checkboxes label:hover{background-color:#1e90ff}</style>';
    //str_html += botao_top;
    $(".exibir").html(str_html);
    btn = $('#backtop');

    //depois de adicionar os campos, deve aplicar as mascaras pendentes
    aplicarMascaras();
    adicionarEventosRightClick();

}
function gerarBodyPesquisa() {
    var div = "";
    lista_abas = [];
    lista_abas = pegarAbas(lista_estrutura, lista_abas);

    var display_dropdown = elementoSpan('', 'caret');
    display_dropdown = elementoButton('Display' + display_dropdown, 'btn btn-default dropdown-toggle', '', '', '', '', '', '', '', 'dropdown');

    //Lista - ul
    var lista_ul = elementoListaUl(lista_estrutura);
    var toolbar_buttons = elementoDiv(display_dropdown + lista_ul, 'btn-group dropdown-btn-group pull-right');
    var table = elementoTable("table table-striped dataTable no-footer", "table0", "grid", "example2_info");
    var paginas = "";

    div = elementoDiv(toolbar_buttons + table + paginas, "dataTables_wrapper form-inline no-footer", "example2_wrapper");
    div = elementoDiv(div, "col-xs-12");
    div = elementoDiv(div, "row");
    div = elementoDiv(div, "panel-body", 'bodyDadosPesquisado');

    var str_html = div;
    return str_html;
}

function pegarAbas(lista_estrutura, lista_abas) {
    var lista = [];
    lista.push('Geral');
    var dado = {
        identifier: 'Geral',
        display: 'Geral',
        class: '',
        fk: false
    };
    lista_abas.push(dado);

    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        if (lista_estrutura.Propriedades[i].ForeignKey == "1N") {
            var display = pegarDisplay(lista_estrutura.Propriedades[i]);
            var dados = {
                identifier: lista_estrutura.Propriedades[i].Identifier,
                display: display,
                class: lista_estrutura.Propriedades[i].ForeignKeyClass,
                fk: true
            };
            lista_abas.push(dados);
            lista.push(display);
        }
        else {
            for (let j = 0; j < lista_estrutura.Propriedades[i].AnnotationsProp.length; j++) {
                var temp = lista_estrutura.Propriedades[i].AnnotationsProp[j];
                if (temp.AttributeName === 'TAB') {
                    if (!lista.includes(temp.Parametros[0].Value)) {
                        var dados = {
                            identifier: temp.Parametros[0].Value,
                            display: temp.Parametros[0].Value,
                            class: '',
                            fk: false
                        }
                        lista_abas.push(dados);
                        lista.push(temp.Parametros[0].Value);
                    }
                }
            }
        }
    }
    //return lista;
    return lista_abas;
}

function gerarHeaderInicial() {
    var link = elementoLinkA("", "box_toggle fa fa-chevron-down");
    link += elementoLinkA("", "box_setting fa fa-cog");
    link += elementoLinkA("", "box_close fa fa-times");

    var h2 = elementoMarkdown(pegarDisplayNomeClasse(lista_estrutura), "2", "title pull-left");
    var div = elementoDiv(link, "actions panel_actions pull-right");
    var header = elementoHeader(h2 + div, "panel_header");
    return '<a onclick="voltarTopo()" id="backtop"></a>' + header;
}

function novaLinhaPesquisa(origem = "") {
    //copia o conteudo da ultima linha e adiciona
    let clone = $(".linhaPesquisa").last().clone();
    var idAtual = 0;
    if (clone != undefined) {
        idAtual = clone.children().children().eq(0).children().children().prop('id');
        idAtual = parseFloat(idAtual.split("divselect")[1]);
        idAtual++;
    }
    var str = '';
    if (origem == 'aps')
        str = gerarCamposPesquisaInicialAPS(idAtual);
    else
        str = gerarCamposPesquisaInicialInterno(idAtual);


    $("#divPaiLinhas").append(str);

    //pega a quantidade de linhas
    let length = $(".linhaPesquisa").length;

    let select_condicao = ` <select class='form-control' id='divoperador${length - 2}'>
        <option value='0'>E</option>
        <option value='1'>OU</option>
    </select>`;

    //configura a linha anterior a ultima adicionada
    //deve remover os botoes de pesquisa, adicionar linha, ações e paginação
    $('#btnPesquisar').eq(0).remove(); //remove o botao de pesquisar
    $('#btnExcel').eq(0).remove(); //remove o botao de pesquisar
    $('#btnNovaLinha').eq(0).remove(); //remove o botao de adicionar
    $(`#divitens`).eq(0).parent().remove(); //remove o select de itens por pagina
    $("#acoes").eq(0).parent().parent().remove(); //remove o botao de acoes

    //adiciona os novos botoes
    $(`.inputLinha`).eq(length - 2).append(select_condicao);

    configurarIdsLinha(origem);
    //funcao de parenteses temprariamente comentada
    // ativarDesativarParenteses(`parenteses_final${length - 1}`, false);
    // ativarDesativarParenteses(`parenteses_inicial${length - 1}`, false); //desativa os parenteses
}

//reajusta as settings das linhas, como, ids dos campos e indexes
function configurarIdsLinha(origem = "") {
    //configura o id dos selects de coluna
    $(`[id^='divselect']`).each(function (index) {
        $(this).attr('id', `divselect${index}`);
    });

    //configura o id dos selects de condicao
    $(`[id^='divfiltros']`).each(function (index) {
        $(this).attr('id', `divfiltros${index}`);
    });

    //configura o id dos inputs de valor
    $(`[id^='dataDe']`).each(function (index) {
        $(this).attr('id', `sel${index}`);
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
    $(`[id^='btnRemoverLinha']`).each(function (index) {
        $(this).attr('id', `btnRemoverLinha${index}`);
        $(this).attr('onclick', `excluirLinhaPesquisa('${index}', '${origem}')`);
    });

    // //configura os parenteses de inicio
    // $(`[id^='parenteses_inicial']`).each(function(index){
    //     $(this).attr('id', `parenteses_inicial${index}`);
    // });

    // //configura os parenteses de termino
    // $(`[id^='parenteses_final']`).each(function(index){
    //     $(this).attr('id', `parenteses_final${index}`);
    // });
}

//ajusta os valores de uma linha especifica
function configurarLinhaPorIndex(index, origem = "", coluna = -1, operador_condicao = "parecido com", valor = "", operador_logico = "") {
    //ajusta o operador de condicao para ser o mesmo dos options na pagina
    switch (operador_condicao) {
        case "%":
            operador_condicao = "parecido com";
            break;

        case "=":
            operador_condicao = "igual a";
            break;

        case ">":
            operador_condicao = "maior que";
            break;

        case ">=":
            operador_condicao = "maior ou igual a";
            break;

        case "<":
            operador_condicao = "menor que";
            break;

        case "<=":
            operador_condicao = "menor ou igual a";
            break;

        case "<>":
            operador_condicao = "diferente de";
            break;

        case "><":

            parent = $('#divfiltros' + index).parent();
            $('#divfiltros' + index).remove();
            $('#sel' + index).prop('type', 'datetime-local');

            strInput = elementoInputText('', 'form-control campo_pesquisa', 'dataDe' + index + '', 'Digite a pesquisa', 'datetime-local');
            parent.append(strInput);
            //"2020-12-23T15:28_2021-01-09T15:28"
            temp = valor.split("_");
            valor = temp[1];
            $('#dataDe' + index).val(temp[0]);

            break;
    }

    //define os valores do select de coluna, de condicao e também o valor do campo txt
    $(`#divselect${index}`).val(coluna);
    $(`#divfiltros${index}`).val(operador_condicao);
    $(`#sel${index}`).val(valor);

    if (operador_logico != "") {
        //ajusta o operador logico para equivaler ao option na pagina
        operador_logico = operador_logico == '&' ? 0 : 1;
        $(`#divoperador${index}`).val(operador_logico);
    }

    //se for a ultima linha, adiciona os botoes
    let length = $(".linhaPesquisa").length;
    if (index == length - 1) {
        adicionarBotoesLinha(length - 1, origem);
    }
}


function adicionarBotoesLinha(index, origem = "") {

    //se vier do aps, precisa fazer a pesquisa na tabela fila expedicao
    let btnPesquisar = elementoButton('', 'fa fa-search', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    if (origem.toLowerCase() == "aps")
        btnPesquisar = elementoSpan(btnPesquisar, 'input-group-addon', 'btnPesquisar', '', 'onclick="TabelaFilaExpedicao.eventoPesquisarPrincipalAPS()"');
    else
        btnPesquisar = elementoSpan(btnPesquisar, 'input-group-addon', 'btnPesquisar', '', 'onclick="eventoPesquisarPrincipal()"');

    let btnExcel = elementoButton('', 'fa fa-file-excel-o', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    btnExcel = elementoSpan(btnExcel, 'input-group-addon', 'btnExcel', '', 'onclick="eventoDonwloadExcel()"');

    let btnAdicionarLinha = elementoButton('', 'fa fa-plus', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    btnAdicionarLinha = elementoSpan(btnAdicionarLinha, 'input-group-addon', 'btnNovaLinha', '', `onclick="novaLinhaPesquisa('${origem}')"`);

    let selItensPorPagina = elementoCombobox(['10', '20', '30', '40', '50', '100'], "form-control", "divitens", "", "Itens por Página");
    selItensPorPagina = elementoDiv(selItensPorPagina, "col-xs-2 col-sm-2 colocapica");

    let dropdownAcoes = elementoComboboxArea(pegarListaMetodosEstrutura(lista_estrutura), '', 'acoes');

    //verifica se o botao nao existe(permitido apenas um botao de cada, na ultima linha), para entao adiciona-los na ultima linha   
    if (!$(`#btnPesquisar`).length) $(".inputLinha").last().append(btnPesquisar);
    if (!$(`#btnNovaLinha`).length) $(".inputLinha").last().append(btnAdicionarLinha);
    if (!$(`#btnExcel`).length) $(".inputLinha").last().append(btnExcel);
    if (!$(`#divitens`).length && origem != "aps") $(".linhaPesquisa > div").last().append(selItensPorPagina);
    if (!$(`#acoes`).length) $(".linhaPesquisa > div").last().append(dropdownAcoes);

    //$(`#btnRemoverLinha${index}`).remove();
    $(`#divoperador${index}`).remove();
}

//exclui a linha e em seguida reajusta os ids
function excluirLinhaPesquisa(index, origem = "") {
    let length = $(".linhaPesquisa").length - 1;

    //esta tentando excluir a ultima linha
    if ((length - 1) <= -1) {
        configurarLinhaPorIndex(index, origem, -1, "parecido com", "", "");
        return;
    }
    else if (index == length) { //esta tentando excluir a linha mas ainda existe outra acima
        //passa os botoes pra cima
        $(".linhaPesquisa").eq(index).remove();
        adicionarBotoesLinha(length - 1, origem);
    }
    else if (index != length) { //esta tentando excluir uma linha que nao é a ultima
        $(".linhaPesquisa").eq(index).remove();
    }

    configurarIdsLinha(origem);

}

function mudarCamposPesquisaInicial(lista, idDiv) {

    var valor = $('#divselect' + idDiv).val();
    console.log(valor);
    var i = 0;
    while (i < lista.length && valor != lista[i].id) {
        i++;
    }

    if (i < lista.length) {
        var tipo = lista[i].tipo;

        var strInput = '';
        var parent = '';
        parent = $('#divfiltros' + idDiv).parent();
        if (parent.length == 0) {
            parent = $('#dataDe' + idDiv).parent();
            $('#dataDe' + idDiv).remove();
        }
        else
            $('#divfiltros' + idDiv).remove();


        //dataDe

        if (tipo.includes('STRING')) {
            $('#sel' + idDiv).prop('type', 'text');

            strInput = elementoCombobox(['parecido com', 'igual a', 'maior que', 'maior ou igual a', 'menor que', 'menor ou igual a', 'diferente de'], "form-control", "divfiltros" + idDiv, "", "Filtros");
        }
        if (tipo.includes('INT') || tipo.includes('DOUBLE')) {
            $('#sel' + idDiv).prop('type', 'number');

            strInput = elementoCombobox(['parecido com', 'igual a', 'maior que', 'maior ou igual a', 'menor que', 'menor ou igual a', 'diferente de'], "form-control", "divfiltros" + idDiv, "", "Filtros");
        }
        if (tipo.includes('DATE')) {
            $('#sel' + idDiv).prop('type', 'datetime-local');

            strInput = elementoInputText('', 'form-control campo_pesquisa', 'dataDe' + idDiv + '', 'Digite a pesquisa', 'datetime-local');
        }
        parent.append(strInput);

    }
}

function gerarCamposPesquisaInicial(index) {

    var res_p1 = gerarCamposPesquisaInicialInterno(index);

    var res_final = elementoDiv(res_p1, '', 'divPaiLinhas');
    res_final = elementoNav(res_final, 'navbar navbar-default');
    res_final = elementoDiv(res_final, 'panel-body');

    return res_final;
}

function gerarCamposPesquisaInicialInterno(index) {
    var lista_res = criarListaCombobox();
    var listaJSON = JSON.stringify(lista_res);
    listaJSON = listaJSON.replaceAll('"', "'");

    var select = '<select class="form-control" id="divselect' + index + '" onchange="mudarCamposPesquisaInicial(' + listaJSON + ', ' + index + ')">';
    select += '<option selected="selected">VAZIO</option>';
    for (var i = 0; i < lista_res.length; i++) {
        select += '<option value="' + lista_res[i].id + '">' + lista_res[i].dado + '</option>';
    }
    select += '</select>';


    //var parte10 = elementoCombobox(lista_res, "form-control divselect", "divselect0", "", "Selecione o atributo", "", "", lista_res[1]);
    var parte10 = elementoDiv(select, 'input-group');
    parte10 = elementoDiv(parte10, "form-group  col-xs-3 col-sm-3 colocapica");

    var parte11 = elementoCombobox(['parecido com', 'igual a', 'maior que', 'maior ou igual a', 'menor que', 'menor ou igual a', 'diferente de'], "form-control", "divfiltros0", "", "Filtros");
    parte11 = elementoDiv(parte11, "col-xs-2 col-sm-2 colocapica");
    //////////////////////////////////////////////////////////////////////////////////
    var parte21 = elementoCombobox(['10', '20', '30', '40', '50', '100'], "form-control", "divitens", "", "Itens por Página");
    parte21 = elementoDiv(parte21, "col-xs-1 col-sm-1 colocapica");

    var parte20 = elementoInputText('', 'form-control campo_pesquisa', 'sel' + index + '', 'Digite a pesquisa', 'text');
    var parte22 = elementoButton('', 'fa fa-search', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    var parte27 = elementoButton('', 'fa fa-file-excel-o', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    var parte23 = elementoSpan(parte22, 'input-group-addon', 'btnPesquisar', '', 'onclick="eventoPesquisarPrincipal()"');
    let parte28 = elementoSpan(parte27, 'input-group-addon', 'btnExcel', '', 'onclick="eventoDonwloadExcel()"');
    var parte25 = elementoButton('', 'fa fa-plus', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    var parte26 = elementoSpan(parte25, 'input-group-addon', 'btnNovaLinha', '', 'onclick="novaLinhaPesquisa()"');
    var parte30 = elementoButton('', 'fa fa-trash', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none;');
    parte30 = elementoSpan(parte30, 'input-group-addon', 'btnRemoverLinha' + index + '', '', 'onclick', 'excluirLinhaPesquisa(' + index + ')');
    var parte24 = elementoDiv(parte20 + parte30 + parte26 + parte23 + parte28, 'input-group inputLinha');
    parte24 = elementoDiv(parte24, 'form-group col-xs-4 col-sm-4 colocapica', 'div2');

    ///////////////////////////////////////////////////////////////////////////////////

    var res_p3 = elementoComboboxArea(pegarListaMetodosEstrutura(lista_estrutura), '', 'acoes');

    var res_p1 = elementoDiv(parte10 + parte11 + parte24 + parte21 + res_p3, 'nav navbar-nav');
    var res_final = elementoDiv(res_p1, 'linhaPesquisa collapse navbar-collapse');
    //////////////////////////////////////////////////////////////////////////////////

    //return res_final;
    return res_final;
}

function gerarCamposPesquisaInicialAPS(index) {

    var lista_res = criarListaCombobox();
    var listaJSON = JSON.stringify(lista_res);
    listaJSON = listaJSON.replaceAll('"', "'");

    var select = '<select class="form-control" id="divselect' + index + '" onchange="mudarCamposPesquisaInicial(' + listaJSON + ', ' + index + ')">';
    for (var i = 0; i < lista_res.length; i++) {
        select += '<option value="' + lista_res[i].id + '">' + lista_res[i].dado + '</option>';
    }
    select += '</select>';


    //var parte10 = elementoCombobox(lista_res, "form-control divselect", "divselect0", "", "Selecione o atributo", "", "", lista_res[1]);
    var parte10 = elementoDiv(select, 'input-group');
    parte10 = elementoDiv(parte10, "form-group  col-xs-3 col-sm-3 colocapica");

    var parte11 = elementoCombobox(['parecido com', 'igual a', 'maior que', 'maior ou igual a', 'menor que', 'menor ou igual a', 'diferente de'], "form-control", "divfiltros0", "", "Filtros");
    parte11 = elementoDiv(parte11, "col-xs-2 col-sm-2 colocapica");
    //////////////////////////////////////////////////////////////////////////////////
    var parte21 = elementoCombobox(['10', '20', '30', '40', '50', '100'], "form-control", "divitens", "", "Itens por Página");
    parte21 = elementoDiv(parte21, "col-xs-1 col-sm-1 colocapica");

    var parte20 = elementoInputText('', 'form-control campo_pesquisa', 'sel' + index + '', 'Digite a pesquisa', 'text');
    var parte22 = elementoButton('', 'fa fa-search', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    var parte27 = elementoButton('', 'fa fa-file-excel-o', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    //var parte23 = elementoSpan(parte22, 'input-group-addon', 'btnPesquisar', '', 'onclick="eventoPesquisarPrincipal()"');
    var parte23 = elementoSpan(parte22, 'input-group-addon', 'btnPesquisar', '', 'onclick="TabelaFilaExpedicao.eventoPesquisarPrincipalAPS()"');
    let parte28 = elementoSpan(parte27, 'input-group-addon', 'btnExcel', '', 'onclick="eventoDonwloadExcel()"');
    var parte25 = elementoButton('', 'fa fa-plus', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    //var parte26 = elementoSpan(parte25, 'input-group-addon', 'btnNovaLinha', '', 'onclick="novaLinhaPesquisa()"');
    var parte26 = elementoSpan(parte25, 'input-group-addon', 'btnNovaLinha', '', 'onclick="novaLinhaPesquisa(`aps`)"');
    var parte30 = elementoButton('', 'fa fa-trash', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none;');
    //parte30 = elementoSpan(parte30, 'input-group-addon', 'btnRemoverLinha' + index + '', '', 'onclick', 'excluirLinhaPesquisa(' + index + ')');
    parte30 = elementoSpan(parte30, 'input-group-addon', 'btnRemoverLinha0', '', 'onclick', "excluirLinhaPesquisa(0, `aps`)");
    var parte24 = elementoDiv(parte20 + parte30 + parte26 + parte23 + parte28, 'input-group inputLinha');
    parte24 = elementoDiv(parte24, 'form-group col-xs-4 col-sm-4 colocapica', 'div2');
    /*
    */
    ///////////////////////////////////////////////////////////////////////////////////

    var res_p3 = elementoComboboxArea(pegarListaMetodosEstrutura(lista_estrutura), '', 'acoes');

    var res_p1 = elementoDiv(parte10 + parte11 + parte24 + parte21 + res_p3, 'nav navbar-nav');
    var res_final = elementoDiv(res_p1, 'linhaPesquisa collapse navbar-collapse');
    //////////////////////////////////////////////////////////////////////////////////

    //return res_final;
    return res_final;

}
//function selecionarParenteses(tipo, id){
//    if($(`#${id}`).attr('data-ativo') === 'true')
//        ativarDesativarParenteses(id, false);
//    else
//        ativarDesativarParenteses(id, true);


//    // //da um loop em todos os parenteses e se não for o mesmo que clicou, muda a classe do botao
//    // $(`[id^='parenteses_${tipo}']`).each(function(){
//    //     if($(this).attr('id') != id){
//    //         ativarDesativarParenteses($(this).attr('id'), false);
//    //     }
//    // });
//}

//function ativarDesativarParenteses(id, ativar){
//    if(ativar){
//        $(`#${id}`).attr('style', 'background: #c4c4c4');
//        $(`#${id}`).attr('data-ativo', 'true');

//    }
//    else{
//        $(`#${id}`).attr('style', 'background: #ffffff');
//        $(`#${id}`).attr('data-ativo', 'false');

//    }
//}

function pegarListaMetodosEstrutura(lista_estrutura) {
    var lista = [];
    if (lista_estrutura.length <= 0)
        return lista;

    if (lista_estrutura.Methods.length > 0) {
        for (let i = 0; i < lista_estrutura.Methods.length; i++) {
            lista.push(lista_estrutura.Methods[i]);
        }
    }
    return lista;
}

//elementos
function pegarDadosDosCampos() {

    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var j = 0
        while (j < lista_estrutura.Propriedades[i].AnnotationsProp.length &&
            !lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName.includes('Combobox'))
            j++;

        if (j < lista_estrutura.Propriedades[i].AnnotationsProp.length) {
            lista_estrutura_insert[0][lista_estrutura.Propriedades[i].Identifier] = ($('.inputprincipal' + i).val());
        }
        else {
            lista_estrutura_insert[0][lista_estrutura.Propriedades[i].Identifier] = $('.inputprincipal' + i).val();
        }

    }
    lista_estrutura_insert[0] = adicionarPlayAction(Object.getOwnPropertyNames(lista_estrutura_insert[0], 'insert')); //action
    var listaDados = [];
    listaDados.push(lista_estrutura_insert[0]);
    return listaDados;
}

async function eventoPesquisarPrincipal(id = "", salvar_filtro = true) {

    if (lista_indices_display.length == 0) {

        //pesquisa nas preferencias se existe uma configuração de display salva
        let preferencia = await pesquisarPreferencia(lista_estrutura.Namespace, "DISPLAY");
        lista_nomes = preferencia != null ? preferencia.PRE_VALOR : "";
        if (lista_nomes.length > 0) {
            selecionados = lista_nomes.split(';').filter(function (v) { return v != ""; });
            lista_indices_display = listaNomesParaListaIndices(lista_nomes, lista_estrutura);
        }
    }
    else {
        let lista_nomes = listaIndicesParaListaNomes(lista_indices_display, lista_estrutura);
        selecionados = lista_nomes.split(';').filter(function (v) { return v != ""; });
    }

    await eventoPesquisar(id, salvar_filtro).then(async function () {
        //se win_pai tiver um valor, quer dizer que foi a    var display_dropdown = elementoSpan('', 'caret');
        if (win_pai != null) {
            criarBotoesVoltar();
        }

        //depois de gerar a nova tabela de pesquisa, carrega as preferencias nela
        let preferencia = await pesquisarPreferencia(lista_estrutura.Namespace, "ORDERBY");
        let pre_valor = preferencia != null ? preferencia.PRE_VALOR : "";
        carregarOrderByPreferenciasNoHtml(pre_valor); //carrega as novas informações de ordenação no html
    });
}

function eventoDonwloadExcel() {
    Carregando.abrir();

    let result_selecionados = pegarColunasSelecionadas()[0];
    let string_pesquisa = gerarStringPesquisa(result_selecionados.SELECIONADOS_LOCAL);

    //precisa zerar a string de pesquisa, ou tentara converter um valor incorreto para json (MAQ_ID%;)
    let selecionou_algum_valor = false;
    $("[id^='sel'").each(function () {
        if ($(this).val() != "") {
            selecionou_algum_valor = true;
        }
    });
    if (!selecionou_algum_valor) {
        string_pesquisa = "";
    }

    //precisa zerar as colunas se nao selecionar nenhuma, para gerar o excel corretamente
    let selecionou_alguma_coluna = false;
    let columns = result_selecionados.SELECIONADOS_LOCAL;
    $(`[id^='divselect']`).each(function () {
        if ($(this).val() != -1) {
            selecionou_alguma_coluna = true;
        }
    })
    if (!selecionou_alguma_coluna) {
        columns = [];
    }

    //retira os ';' que separa cada linha no input pesquisa, para poder criar o json corretamente
    let input_pesquisa_formatado = string_pesquisa;
    var count = (string_pesquisa.match(/;/g) || []).length;
    for (let i = 0; i < count; i++) {
        input_pesquisa_formatado = input_pesquisa_formatado.replace(";", "");
    }

    var filtros = criarJsonPesquisa(input_pesquisa_formatado, lista_estrutura.Namespace, columns); //Cria o filtro no formato JSON que a parte back aceita para pesquisa

    $.ajax({
        type: "GET",
        url: "/DynamicWeb/ExportarPesquisa",
        data: {
            query_json: filtros, tipoPesquisa: 1
        },
        dataType: "json",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st == "OK" && result.urlDownload != "") {
                window.open(result.urlDownload)
            }
            else {
                var mensagem = "<strong>Erro ao gerar o excel!</strong><br>ERRO ao tentar gerar a planilha, a mensagem de erro é {0}."
                    .replace("{0}", result.st);
                mostraDialogo(mensagem, 'danger', 3000);
            }
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao gerar excel!</strong><br>ERRO ao tentar gerar o excel, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        },
        complete: function () {
            Carregando.fechar();
        }
    });
}

function wtf() {
    console.log('Checkin');
}

function executeMethod(indexMethod, indexDado = -1, is1N = 0, posIf1N = -1, namespace = "") {
    var nameMethod;
    var dados = [];
    var lista_dados_res = [];
    var aux = [];

    if (indexDado == -1) {
        aux = dados_insert;
        //namespace = lista_estrutura.Namespace;
    } else {
        if (is1N == 1) {
            aux = dados_pesquisado_1N[posIf1N][indexDado];
        }
        else {
            aux = lista_dados[indexDado];
            //namespace = lista_estrutura.Namespace;
        }
    }
    lista_dados_res.push(aux);
    dados = JSON.stringify(lista_dados_res);
    if (namespace == "") {
        namespace = lista_estrutura.Namespace;
        nameMethod = lista_methods_class[indexMethod];
    }
    else {
        lista_methods = pegarListaMetodosEstrutura(estrutura_1N);
        nameMethod = lista_methods[indexMethod];
    }

    //var dados = JSON.stringify(pegarDadosDosCampos());
    //var url = '/DynamicWeb/ExecuteMethod?list_obj={0}&class_name={1}&name_method={2}'
    //    .replace("{0}", dados)
    //    .replace("{1}", lista_estrutura.Namespace)
    //    .replace("{2}", nameMethod);
    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/ExecuteMethod",
        data: { list_obj: dados, class_name: namespace, name_method: nameMethod },
        dataType: "json",
        traditional: true,
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            executarProtocolos(result.log);

            let mostrou_erro = false; //só permite mostrar o erro 1x

            for (i = 0; i < result.log.length; i++) {

                if (result.log[i].Status === "ERRO" && !mostrou_erro) {
                    mostraDialogo(result.log[i].MsgErro, 'danger', 3000);
                    $(".exibir").append(" " + result.log[i].MsgErro);
                    mostrou_erro = true;
                }
                if (result.log[i].Status === "OK") {
                    let msg = result.log[i].MsgErro != "" ? result.log[i].MsgErro : result.log[i].PrimaryKey;
                    mostraDialogo(msg, 'success', 3000);
                    $(".exibir").append(" " + msg);
                }
                if(result.log[i].Status == "ALERT"){
                    let msg = result.log[i].MsgErro != "" ? result.log[i].MsgErro : result.log[i].PrimaryKey;
                    mostraDialogo(msg, 'info', 3000);
                    $(".exibir").append(" " + msg);
                }
            }
        },
        error: OnError,
        complete: onComplete
    })
}

function executarProtocolos(logs) {
    // percorre todos os logs verificando os protocolos
    for (var i = 0; i < logs.length; i++) {
        if (logs[i].NomeAtributo == 'PROTOCOLO') {
            if (logs[i].Status == 'REPORT_PDF') {
                //Abre um novo navegador com as etiquetas
                var url = window.location.href;
                var url1 = url.split('/D')[0];
                var newUrl = url1 + logs[i].MsgErro;
                window.open(newUrl.toString());
            }
            if (logs[i].Status == 'LINK') {
                //Abre um novo navegador com o link passado or parametro
                var url_atual = window.location.href;
                var ip_porta = url_atual.split('/')[2];
                var url = ip_porta + logs[i].MsgErro + logs[i].PrimaryKey;
                //window.location.href = logs[i].MsgErro + logs[i].PrimaryKey;
                window.open(logs[i].MsgErro + (logs[i].PrimaryKey == null ? "" : logs[i].PrimaryKey));
            }
            if (logs[i].Status == '_ETIQUETA') {
                //Executa uma funcao  JS pelo Menu de funcoes Associadas
                var nomeFuncao = logs[i].MsgErro;            // Nome da Função JS a ser executada
                var valores = logs[i].PrimaryKey.split(','); // Array de parâmetros da função
                //Iterando nos parâmetros para contrução do objeto Json contendo os parâmetros
                valoresJson = '{';
                for (let j = 0; j < valores.length; j++) {
                    var spl = valores[j].split(':');
                    var Campo = spl[0];
                    var Valor = spl[1];
                    valoresJson += '\"' + Campo + '\"';
                    valoresJson += ':';
                    valoresJson += '\"' + Valor + '\"';
                    if (j < valores.length - 1)
                        valoresJson += ','
                }
                valoresJson += '}'
                var arrayDeValoresDefault = JSON.parse(valoresJson);
                var strArrayDeValoresDefault = JSON.stringify(arrayDeValoresDefault);
                var strNamespace = 'DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaImpressaoEtiquetas';
                var strEndereco = '/DynamicWeb/LinkSGI?str_namespace=' + strNamespace + '&ArrayDeValoresDefault=' + strArrayDeValoresDefault;
                window.location.href = strEndereco;
            }
            if (logs[i].Status == 'ListarParametros') {
                listarParametros(logs[i].NomeClasse, logs[i].PrimaryKey, logs[i].MsgErro);
            }
        }
    }
}

function listarParametros(classeMetodo, primaryKey, parametros) {

    var nomeClasse = classeMetodo.split('#')[0];
    var metodo = classeMetodo.split('#')[1];
    var parameters = JSON.parse(parametros);

    if (parameters.length > 0) {

        var inputsHidden = '';
        inputsHidden += '<input id="qtdParametros-' + primaryKey + '" type="hidden" style="clear: both; " value="' + parameters.length + '"/>';
        inputsHidden += '<input id="classeMetodo-' + primaryKey + '" type="hidden" value="' + classeMetodo + '"/>';
        inputsHidden += '<input id="primaryKey-' + primaryKey + '" type="hidden" value="' + primaryKey + '"/>';
        var margin = 0;
        var htmlInputs = '<div>';
        for (var i = 0; i < parameters.length; i++) {
            if (i > 0) {
                margin = 20;
            }
            htmlInputs += '<label style="margin-left: ' + margin + 'px;">' + parameters[i].split('#')[0] + '</label>';
            if (parameters[i].split('#')[1].toLowerCase() == 'int') {
                htmlInputs += '<input id="input-' + primaryKey + '-' + i + '" type="number" style="height: 30px; margin-left: 10px;" />';
            } else if (parameters[i].split('#')[1].toLowerCase() == 'date-time') {
                htmlInputs += '<input id="input-' + primaryKey + '-' + i + '" type="datetime-local" style="height: 30px; margin-left: 10px;" />';
            } else if (parameters[i].split('#')[1].toLowerCase() == 'string') {
                htmlInputs += '<input id="input-' + primaryKey + '-' + i + '" type="text" style="height: 30px; margin-left: 10px;" />';
            }
        }
        htmlInputs += '</div><br/>';

        var htmlButton = '<button id="btnOK" type="button" onclick="executarMetodoComParametros(' + primaryKey + ')" class="btn btn-success btnPage">OK</button>';
        htmlButton += '<button class="btn btn-secondary" style="margin-left: 20px;" onclick="fecharModalParametros()">CANCELAR</button>';


        var html = '';
        html += '<div id="modal-parametros-method"  class="modal fade modal-method" role="dialog" style="z-index:9998 !important;">';
        html += '<div class="modal-dialog" style="width: 70%;">';
        html += '<div class="modal-content">';
        html += '<div class="modal-header"><strong>INFORME OS DADOS</strong></div>';
        html += '<div class="modal-body">';
        html += htmlInputs;
        html += inputsHidden;
        html += htmlButton;
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        $(".exibir").append(html);
        $('.modal-method').modal('show');
    }
}

function fecharModalParametros() {
    $('.modal-method').modal('hide');
}

function executarMetodoComParametros(primaryKey) {

    var qtdParametros = $('#qtdParametros-' + primaryKey).val();
    var classe = $('#classeMetodo-' + primaryKey).val().split('#')[0];
    var metodo = $('#classeMetodo-' + primaryKey).val().split('#')[1];

    var arrayParametros = [];
    for (var i = 0; i < qtdParametros; i++) {
        var valorParametro = $('#input-' + primaryKey + '-' + i).val();
        arrayParametros.push(valorParametro);
    }

    var objParametros = {
        primaryKey: primaryKey,
        parametros: arrayParametros
    };

    var parametrosDoMetodo = JSON.stringify(objParametros);
    fecharModalParametros();
    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/ExecuteMethodWithParameters",
        data: { list_obj: parametrosDoMetodo, class_name: classe, name_method: metodo },
        dataType: "json",
        traditional: true,
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            executarProtocolos(result.log);

            for (i = 0; i < result.log.length; i++) {

                if (result.log[i].Status === "ERRO") {
                    mostraDialogo(result.log[i].MsgErro, 'danger', 3000);
                    $(".exibir").append(result.log[i].MsgErro);
                }
                if (result.log[i].Status === "OK") {
                    mostraDialogo(result.log[i].MsgErro, 'success', 3000);
                    $(".exibir").append(result.log[i].MsgErro);
                }
            }
        },
        error: OnError,
        complete: onComplete
    });
}


function gerarRodapePesquisa(pagina_selecionada = 1) {
    var str_html = '<div>' + criarPaginacao(pagina_selecionada, qtd_paginas); + '</div>';
    //var str_html = '';

    /*str_html += "<form>";
    str_html += '<div class="{0}">'.replace("{0}", "form-group row");

    str_html += '<div class="{0}">'.replace("{0}", "col-sm-2");
    str_html += elementoButton("Salvar Todos", "btn btn-success", "", "onclick", "evtSalvarTodos()");
    str_html += '</div>';

    str_html += '<div class="{0}">'.replace("{0}", "col-sm-2");
    str_html += elementoButton("Cancelar Todos", "btn btn-secondary", "", "onclick", "evtCancelarTodos()");
    str_html += '</div>';

    str_html += '</div>';
    str_html += "</form>";*/

    return str_html;
}

function criarPaginacao(ativo = -1, qtd_paginas, posicao_1N = -1, text = '', tab = -1, index = -1) {
    var str = "";

    str = '<div class="ctrlPag" align="center">';
    str += '<ul class="pagination">';
    str += ativo == 1 ?
        '<li class="page-item pg disabled"><a href="#")" disabled>Anterior</a></li>' :
        '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + (ativo - 1) + ', ' + tab + ', ' + index + ')">Anterior</a></li>';

    if (qtd_paginas > 9) { //INICIO
        if (ativo < 5) {
            for (let i = 1; i <= ativo + 2; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>';
            }
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            str += '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + qtd_paginas + ', ' + tab + ', ' + index + ')">' + qtd_paginas + '</a></li>';
        }
        else if (ativo > qtd_paginas - 4) { //FIM
            str += '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(1, ' + tab + ', ' + index + ')">1</a></li>';
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            for (let i = ativo - 2; i <= qtd_paginas; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>';
            }
        }
        else { //MEIO
            str += '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(1, ' + tab + ', ' + index + ')">1</a></li>';
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            for (let i = ativo - 2; i <= ativo + 2; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>';
            }
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            str += '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + qtd_paginas + ', ' + tab + ', ' + index + ')">' + qtd_paginas + '</a></li>';
        }
    }
    else {
        for (let i = 1; i <= qtd_paginas; i++) {
            str += ativo == i ?
                '<li class="page-item active pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>' :
                '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + i + ', ' + tab + ', ' + index + ')">' + i + '</a></li>';
        }
    }
    str += ativo == qtd_paginas ?
        '<li class="page-item pg disabled"><a href="#">Proximo</a></li>' :
        '<li class="page-item pg"><a href="#" onclick="eventoMudarPagina' + text + '(' + (ativo + 1) + ', ' + tab + ', ' + index + ')">Proximo</a></li>';
    //str += '<li class="page-item pg"><a href="">Proximo</a></li>';
    str += '</ul></div>';
    return str;
}

function eventoMudarPagina(pagina_selecionada, tab = -1, index = -1) {
    var txt = $('#sel').val();
    /*  Ao mudar de página não mantém o filtro.
        var value = criarJsonPesquisa(txt, lista_estrutura.Namespace, selecionados);
    */

    lista_colunas = [];
    lista_dados_totais = [];
    lista_dados_totais_classes = [];
    lista_abas = [];

    //pesquisa no banco, e depois cria botoes de voltar caso tenha win_pai (ocorre em alguns locais do sistema onde é possível abrir a tela de cadastro de uma foreign key e retornar uma seleção para a primeira pagina)
    eventoPesquisarNoBanco(filtros_pesquisa_principal, input_pesquisa_principal, pagina_selecionada).then(function (result) {
        if (win_pai != null) {
            criarBotoesVoltar();
        }
    });

    $('html, body').animate({ scrollTop: 0 }, 600);
}

function eventoMudarPagina1N(pagina_selecionada, tab, index) {
    var posicao_dado = pegarPosicaoDado1N(tab);
    lista_paginas_1N[posicao_dado] = pagina_selecionada;

    pesquisar1N(tab, index, 'principal', posicao_dado, lista_paginas_1N[posicao_dado]);

    $('html, body').animate({ scrollTop: $("#ancora" + tab).offset().top }, 600);
}

async function eventoPesquisar(input = "", salvar_filtro = true) {
    //insere um novo filtro e retorna a string de pesquisa e as colunas selecionadas
    var input_pesquisa_e_selecionados = prepararNovoFiltro("update", input, "AUTO", '', salvar_filtro); //retorna um array contendo o string_pesquisa gerado e as colunas selecionadas na pesquisa;
    var string_pesquisa = input_pesquisa_e_selecionados[0];
    var selecionados_local = input_pesquisa_e_selecionados[1];
    var order_bys = pegarOrderByDoHtml();

    //se não encontrar order by no html, tenta procura-lo nas preferencias
    if (order_bys.length == 0) {
        let preferencia = await pesquisarPreferencia(lista_estrutura.Namespace, "ORDERBY");
        order_bys = preferencia != null ? preferencia.PRE_VALOR : "";
    }

    //retira os ';' que separa cada linha no input pesquisa, para poder criar o json corretamente
    let input_pesquisa_formatado = string_pesquisa;
    var count = (string_pesquisa.match(/;/g) || []).length;
    for (let i = 0; i < count; i++) {
        input_pesquisa_formatado = input_pesquisa_formatado.replace(";", "");
    }

    //cria o json de pesquisa
    var filtros = criarJsonPesquisa(input_pesquisa_formatado, lista_estrutura.Namespace, selecionados_local, order_bys); //Cria o filtro no formato JSON que a parte back aceita para pesquisa
    tamanho_pagina = $('#divitens').val() != null ? $('#divitens').val() : 10; //Pega a quantidade de itens por página que o usuário quer. Se o usuário não escolher nada, pega 10 por padrão.

    lista_colunas = [];
    lista_dados_totais = [];
    lista_dados_totais_classes = [];
    lista_abas = [];

    var temp = JSON.parse(filtros).Columns;
    for (let i = 0; i < temp.length; i++)
        lista_colunas.push(temp[i].NameColumn);

    filtros_pesquisa_principal = filtros;
    input_pesquisa_principal = string_pesquisa;

    return await eventoPesquisarNoBanco(filtros, string_pesquisa);
}

//funcao que gera uma nova string de pesquisa
function gerarStringPesquisa(selecionados_local, input = "") {
    var input_pesquisa = [];
    var input_pesquisaDe = [];

    $("[id^='sel'").each(function () {
        input_pesquisa.push($(this).val());
    });

    $("[id^='dataDe'").each(function () {
        input_pesquisaDe.push($(this).val());
    });

    //pega os operadores lógicos da pesquisa
    let condicoes = [];
    $("[id^='divfiltros'").each(function () {
        condicoes.push($(this).val())
    });

    if (input_pesquisaDe.length > 0) {
        for (var i = 0; i < input_pesquisaDe.length; i++) {
            var j = 0;
            while (j < input_pesquisa.length && input_pesquisaDe[i] != input_pesquisa[j])
                j++;

            if (j < input_pesquisa.length) {
                input_pesquisa[j] = input_pesquisaDe[i] + '_' + input_pesquisa[j + 1];
                input_pesquisa.splice(j + 1, 1);
                input_pesquisaDe.splice(i, 1);
                i--;
                condicoes.splice(j, 0, 'between');
            }
        }
    }

    //pega os valores de input pesquisa caso nao seja especificado um input por parametro
    input_pesquisa = input != "" ? input : input_pesquisa;


    if (input == "")
        return pegarFiltros(input_pesquisa, selecionados_local, condicoes);
    else
        return input;
}

//pega as colunas selecionadas no campo de pesquisa e retorna a variavel selecionados e selecionados local em um objeto
function pegarColunasSelecionadas() {
    var temp = []; //Temporário dos selecionados
    var selecionados_local = [];

    //verifica se selecionou alguma coluna, se sim, pega as colunas selecionadas
    //senao, pega as primarys keys da estrutura
    let selecionou_alguma_coluna = false;
    $("[id^='divselect'").each(function () {
        if ($(this).val() !== '-1')
            selecionou_alguma_coluna = true;
    });

    if (selecionou_alguma_coluna) {
        selecionados_local = pegarSelecionados();
        temp = pegarElementosSelecionados();
    }
    else {
        selecionados_local = pegarElementosSelecionados();
    }

    if (selecionados.length == 0) {
        selecionados = selecionados_local.filter((este, i) => selecionados_local.indexOf(este) === i); //Remove elementos duplicados.
        lista_indices_display = listaNomesParaListaIndices(selecionados, lista_estrutura);
    }

    { //Bloco de comando para pegar todos os elementos que devem aparecer no grid de pesquisa.
        for (let i = 0; i < selecionados_local.length; i++) //Inserindo todos as colunas que já devem aparecer, para executar o comando abaixo.
            temp.push(selecionados_local[i]);
        var temp2 = pegarElementosGrid(temp); //Pegando elementos com a anotação GRID

        if (selecionados.length == 0) {
            selecionados = temp2.filter((este, i) => temp2.indexOf(este) === i); //Remove elementos duplicados.
            lista_indices_display = listaNomesParaListaIndices(selecionados, lista_estrutura);
        }
    }

    return [{ SELECIONADOS: selecionados, SELECIONADOS_LOCAL: selecionados_local }];
}

function pesquisarPreferencia(namespace, tipo) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/AtualizarPreferenciaUsuario",
            data: {
                preTipo: tipo, preNamespace: namespace
            },
            dataType: "json",
            traditional: true,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.preferenciaUsuario != null) {
                    resolve(result.preferenciaUsuario);
                }
                else {
                    resolve();
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown, result) {
                var mensagem = `<strong>Erro ao tentar pesquisar preferência!</strong><br>ERRO ao tentar pesquisar preferência, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);

                reject();
            }
        });
    });
}

/**
 * pesquisa as informações de order by da preferencia
 * depois de pesquisar, desmonta as informações de preferencia no html
 * 
 * formato: 
 * PRE_VALOR = 'ICA_ID,0;CAL_ID,1' 
 * ICA ID DESCENDENTE E CAL ID ASCENDENTE
 */
function carregarOrderByPreferenciasNoHtml(val, index = 0, origem = "") {

    val.split(';').forEach(function (val) {
        let coluna = val.split(',')[0];
        let tipo_ordem = val.split(',')[1];

        //vai no icone de ordenação da coluna respectiva, e atribui seus dados
        if (coluna != "" && tipo_ordem != "") {
            $(`.orderby${origem}${index}[data-identifier='${coluna}']`).attr('data-orderby', 'this'); //marca o campo para participar do order by
            $(`.orderby${origem}${index}[data-identifier='${coluna}']`).attr('data-tipoordem', tipo_ordem); //marca o tipo de ordenação
            $(`.orderby${origem}${index}[data-identifier='${coluna}']`).attr('class', `orderby${origem}${index} fa fa-arrow-${tipo_ordem == 1 ? "up" : "down"}`); //muda a seta para cima ou para baixo de acordo com o tipo de ordenação
        }
    });

}

/**
 * pega as informações de ORDER BY do HTML e salva nas preferencias
 */
async function salvarOrderByPreferencias(namespace, index, origem = "") {
    let str_order_by = pegarOrderByDoHtml(index, origem);

    return await atualizarPreferencia("update", "Configurações de Order By", namespace, str_order_by, "ORDERBY");
}

// async function salvarOrderByPreferencias1N(index){
//     let str_order_by = pegarOrderByDoHtml("1N");

//     return await atualizarPreferencia("update", "Configurações de Order By", lista_abas[index].class, str_order_by, "ORDERBY", '','', false);
// }

function pegarOrderByDoHtml(index, origem = "") {
    let str_order_by = "";

    //recupera todas as colunas selecionadas de acordo com a ordem clicada
    var elementos = $(`.orderby${origem}${index}[data-orderby='this']`).sort(function (a, b) {
        return a.dataset.ordemclicada > b.dataset.ordemclicada;
    }).toArray();


    //percorre cada coluna para construir uma string a ser salva nas preferencias
    elementos.forEach(function (current) {
        str_order_by += `${current.dataset.identifier},${current.dataset.tipoordem};`;
    });

    return str_order_by;
}

//prepara um novo filtro, a insere no banco e retorna a string de pesquisa gerada, e as colunas selecionadas na pesquisa
function prepararNovoFiltro(playAction, input = "", origem_filtro = "", origem = "", salvar_filtro = true) {

    let result_selecionados = pegarColunasSelecionadas()[0];
    let string_pesquisa = gerarStringPesquisa(result_selecionados.SELECIONADOS_LOCAL, input);

    let selecionou_algum_valor = false;
    $("[id^='sel'").each(function () {
        if ($(this).val() != "") {
            selecionou_algum_valor = true;
        }
    });

    if (!selecionou_algum_valor && input == "") { //precisa verificar se o input pe vazio, pois se nao for, a pesquisa esta sendo utilizada apartir da funcao abrirNovaAba
        string_pesquisa = ""; //precisa zerar a string de pesquisa, ou tentara converter um valor incorreto para json (MAQ_ID%;)
    }

    if (salvar_filtro) {
        //gera uma descricao que recebe um valor default caso nao esteja preenchida e salva na tabela de preferencias
        var descricao = $("#input_descricao").val() != "" ? $("#input_descricao").val() : 'ÚLTIMA PESQUISA REALIZADA.';
        let pre_tipo = "";

        //origem do filtro diz se é MANUAL (um filtro inserido intencionalmente pelo usuario) ou AUTO (filtro salvo automaticamente pelo sistema a cada pesquisa)
        if (origem_filtro.toLowerCase() == "manual") {
            pre_tipo = "FILTRO";
        }
        else if (origem_filtro.toLowerCase() == "auto") {
            pre_tipo = "BUFFER";
        }

        if (playAction == "insert") {
            inserirPreferencia(playAction, descricao, lista_estrutura.Namespace, string_pesquisa, pre_tipo).then(function (result) {
                atualizarModalFiltros(result, origem);
            });
        }
        else if (playAction == "update") {
            atualizarPreferencia("", descricao, lista_estrutura.Namespace, string_pesquisa, pre_tipo).then(function (result) {
                atualizarModalFiltros(result, origem);
            });
        }
    }

    return [string_pesquisa, result_selecionados.SELECIONADOS_LOCAL];
}

//exclui o filtro do banco e da tabela de filtros
function excluirFiltro(pre_id) {
    var preferencia = [];
    preferencia.push({
        PRE_ID: pre_id,
        PRE_DESCRICAO: null,
        PRE_NAMESPACE: null,
        PRE_VALOR: null,
        PRE_TIPO: null,
        USE_ID: null,
        PER_ID: null,
        PlayAction: "delete"
    });

    excluirBanco(preferencia, 'DynamicForms.Areas.PlugAndPlay.Models.T_PREFERENCIAS', 0);
    $(`#${pre_id}`).remove();
}

//recebe o ultimo filtro adicionado para atualizar a nova linha na modal
function atualizarModalFiltros(result, origem = "") {
    //usa o id retornado do log e o objeto inserido para adicionar a nova linha na tabela de filtros
    if (result != null) {
        if (result.STATUS === true) {
            let newest_object = result.LIST_OBJETOS;
            newest_object = newest_object[0][0];

            let newest_id = result.NEWEST_PRIMARY_KEYS;
            newest_id = newest_id[0]; //pega a primeira pk
            let identifier = Object.getOwnPropertyNames(newest_id)[0]; //pega o primeiro identifier da pk
            newest_id = newest_id[identifier];

            if (newest_object.PRE_NAMESPACE.toLowerCase() == lista_estrutura.Namespace.toLowerCase()) {
                adicionarLinhaModalFiltros(newest_id, newest_object.PRE_DESCRICAO, newest_object.PRE_VALOR, origem, newest_object.PRE_TIPO);
            }
        }
    }
}

//adiciona o filtro a tabela
function adicionarLinhaModalFiltros(pre_id, pre_descricao, pre_valor, origem = "", pre_tipo = "") {

    //se encontrar um tr com esse id, retorna da função
    if ($(`#${pre_id}`).length)
        $(`#${pre_id}`).remove();

    let linha = `<tr id='${pre_id}'>
        <td>${pre_id}</td>
        <td>
            <span class="input-group-addon" id="" aria-hidden="" onclick="desmontarStringPesquisa('${pre_valor}', '${origem}'); $('#modal-filtros').modal('hide');" data-toggle="tooltip" title="Carregar filtro">
                <button type="" class="fa fa-plus" id="" =""="" data-dismiss="" aria-label="" data-toggle="" data-target="" style="background:transparent;border:none"></button>
            </span>
            <span class="input-group-addon" id="" aria-hidden="" onclick="excluirFiltro('${pre_id}'); $('#modal-filtros').modal('hide');" data-toggle="tooltip" title="Excluir filtro">
                <button type="" class="fa fa-trash" id="" =""="" data-dismiss="" aria-label="" data-toggle="" data-target="" style="background:transparent;border:none"></button>
            </span>
        </td>
        <td>${pre_descricao}</td>
        <td>${pre_tipo}</td>
        <td>${pre_valor}</td>
    </tr>`;

    $('#table_filtros tbody').append(linha);
}

//faz a inserção intencional (por parte do usuario) de um filtro no banco de dados
//ao completar a inserção, retorna uma promise
function inserirPreferencia(playAction, pre_descricao, pre_namespace, pre_valor, pre_tipo, use_id = '', per_id = '') {
    return new Promise((resolve, reject) => {
        var preferencia = [];
        preferencia.push({
            PRE_ID: null,
            PRE_DESCRICAO: pre_descricao,
            PRE_NAMESPACE: pre_namespace,
            PRE_VALOR: pre_valor,
            PRE_TIPO: pre_tipo,
            USE_ID: use_id,
            PER_ID: per_id,
            PlayAction: playAction
        });

        insertBanco(preferencia, 'DynamicForms.Areas.PlugAndPlay.Models.T_PREFERENCIAS', 0).then(function (result) {
            resolve(result);
        });
    });
}

//verifica se deve inserir uma preferencia no sistema, ou se deve apenas atualiza-lo caso ja exista
async function atualizarPreferencia(playAction, pre_descricao, pre_namespace, pre_valor, pre_tipo, use_id = '', per_id = '', mostrar_dialogo = true) {
    let pre_id = 0;
    let preferencia = await pesquisarPreferencia(pre_namespace, pre_tipo);

    if (preferencia == null) {
        // Não existe a preferência do usuário, deverá criar nova.
        playAction = "insert";
    } else {
        // Existe a preferência, mas ela precisa ser atualizada.
        playAction = "update";
        pre_id = preferencia.PRE_ID;
    }

    if (playAction == "insert" || playAction == "update") {

        //cria o objeto de preferencia e chama a funcao para atualizar/inserir no banco
        var obj_preferencia = [];
        obj_preferencia.push({
            PRE_ID: pre_id,
            PRE_DESCRICAO: pre_descricao,
            PRE_NAMESPACE: pre_namespace,
            PRE_VALOR: pre_valor,
            PRE_TIPO: pre_tipo,
            USE_ID: null,
            PER_ID: null,
            PlayAction: playAction
        });

        return await atualizarBanco(obj_preferencia, 'DynamicForms.Areas.PlugAndPlay.Models.T_PREFERENCIAS', 0, mostrar_dialogo);

    }
}

function ObterUsuarioLogado() {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/ObterJsonUsuarioLogado",
            dataType: "json",
            traditional: true,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.st === "OK") {
                    if (result.usuario != null) {
                        resolve(result.usuario);
                    }
                    else {
                        reject(result.st);
                    }
                } else {//entrou no try catch do controler 
                    mostraDialogo(result.st, 'danger', 3000);
                    reject(result.st);
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao obter usuário logado!</strong><br>ERRO ao tentar obter usuário logado, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);
                reject();
            },
            complete: function () {
                Carregando.fechar();
            }
        });
    })

}

//recebe uma string de pesquisa e remonta o campo de pesquisa da pagina
function desmontarStringPesquisa(string_pesquisa, origem = "") {
    //separa cada conjunto da pesquisa  ex: MAQ_ID=33& ; COD_DESC=DESC
    let array_condicoes = string_pesquisa.split(';').filter(function (v) { return v != ""; });
    let length = $(".linhaPesquisa").length;

    //exclui linhas que sobraram
    for (let i = 0; i < length - array_condicoes.length; i++) {
        excluirLinhaPesquisa(length - i - 1, origem);
    }

    //cria as linhas que faltam
    for (let i = 0; i < array_condicoes.length - length; i++) {
        novaLinhaPesquisa(origem);
    }

    //depois de excluir ou criar todas as linhas necessarias, configura seus valores
    for (let index = 0; index < array_condicoes.length; index++) {
        currentValue = array_condicoes[index];

        let valor = "";
        let coluna = "";
        let operador_logico = "";
        let operador_condicao = "";

        //procura qual o operador de condicao presente na string
        if (currentValue.indexOf('%') != -1) {
            operador_condicao = '%';
        }
        else if (currentValue.indexOf('=') != -1 && currentValue.indexOf('>=') == -1 && currentValue.indexOf('<=') == -1) { //é preciso fazer validacoes extras pois ele pode encontrar um = em uma condicao que é >=
            operador_condicao = '=';
        }
        else if (currentValue.indexOf('><') != -1) {
            operador_condicao = '><';
        }
        else if (currentValue.indexOf('>') != -1 && currentValue.indexOf('>=') == -1 && currentValue.indexOf('<>') == -1) {
            operador_condicao = '>';
        }
        else if (currentValue.indexOf('>=') != -1) {
            operador_condicao = '>=';
        }
        else if (currentValue.indexOf('<') != -1 && currentValue.indexOf('<=') == -1 && currentValue.indexOf('<>') == -1) {
            operador_condicao = '<';
        }
        else if (currentValue.indexOf('<=') != -1) {
            operador_condicao = '<=';
        }
        else if (currentValue.indexOf('<>') != -1) {
            operador_condicao = '<>';
        }


        //procura qual o operador logico presente na string
        if (currentValue.indexOf('&') != -1) {
            operador_logico = '&';
        }
        else if (currentValue.indexOf('|') != -1) {
            operador_logico = '|';
        }

        //se a string de pesquisa nao for vazia, configura, senao, configura a linha para os valores padroes
        if (currentValue != "") {

            //da um split pelo operador de condicao e pega a primeira parte como a coluna, e a segunda parte como o valor
            coluna = currentValue.split(operador_condicao)[0];
            valor = currentValue.substring(currentValue.indexOf(operador_condicao) + (operador_condicao.length - 1) + 1); //é preciso adicionar com operador_condicao.length - 1, pois se o operador tiver 2 caracteres o valor buga
            if (operador_logico != "" && operador_logico != undefined)
                valor = valor.substring(0, valor.indexOf(operador_logico));

            configurarLinhaPorIndex(index, origem, coluna, operador_condicao, valor, operador_logico);
        }
        else {
            configurarLinhaPorIndex(index, origem);
        }

    }

    configurarIdsLinha(origem);
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

function pegarElementosGrid(lista) {
    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var j = 0;
        while (j < lista_estrutura.Propriedades[i].AnnotationsProp.length && lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName.toUpperCase() != 'GRID')
            j++;

        if (j < lista_estrutura.Propriedades[i].AnnotationsProp.length)
            lista.push(lista_estrutura.Propriedades[i].Identifier);
    }

    return lista;
}

//função que retorna quantas colunas a tabela terá
//para isso, ela verifica a quantidade de atributos com a notation grid e soma com a quantidade de pks
function quantidadeCamposNaTabela() {
    var qtd_grids = 0; //qtd atributos com nottation grid
    var qtd_pks = lista_chaves_primarias.length; //qtd atributos pks

    //verifica os atributos com a nottation ' grid '
    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var j = 0;
        while (j < lista_estrutura.Propriedades[i].AnnotationsProp.length && lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName.toUpperCase() != 'GRID')
            j++;

        if (j < lista_estrutura.Propriedades[i].AnnotationsProp.length)
            qtd_grids++;
    }

    return qtd_grids + qtd_pks;
}

function pegarElementosSelecionados() {
    var lista = [];
    for (let i = 0; i < lista_chaves_primarias.length; i++)
        lista.push(lista_estrutura.Propriedades[lista_chaves_primarias[i]].Identifier);
    return lista;
}

//#################################### FUNÇÕES DE ELEMENTOS HTML ###########################################//
//Cria o componente header do HTML <header></header>
function elementoHeader(valor = "", classe = "", id = "") {
    var str = '<header class="{1}" id="{2}">{0}'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        ;
    str += '</header>';
    return str;
}

//Cria o componente seção do HTML <section></section>
function elementoSection(valor = "", classe = "", id = "") {
    var str = '<section class="{1}" id="{2}">{0}'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        ;

    str += '</section>';

    return str;
}

//Cria o componente parágrafo do HTML <p>
function elementoParagrafo(valor, classe = "#", id = "", _for = "", style = "") {
    //return '<p class="{1}" id="{2}">{0}</p><br>'.format(valor, classe, id);
    var str = '<p class="{1}" id="{2}" for="{3}" style="{4}">{0}</p>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", _for)
        .replace("{4}", style);
    return str;
}

function elementoComboboxEditavel(lista, dado, classe = "", id = "", disabled = "", placeholder = "", id_campo = 0, pos_tabela = -1, pos_lista = -1, estrutura, prop, data_names, data_values, index_estrutura = null) {
    //constroi os data-attributes utilizando o name passado e o value no mesmo index
    let html_data_attributes = "";
    if (data_names.length > 0 && data_values.length > 0) {
        data_names.forEach(function (currentValue, index) {
            html_data_attributes += `${currentValue} = "${data_values[index]}"`;
        });
    }

    var str = '';
    str += '<input data-column="{8}" list="lista{7}" name="lista" value="{1}" class="{0}" id="{7}" placeholder="{3}" onkeypress="evtKeyPress(event, {5}, {6})" {2} {9}>'
        .replace("{0}", classe + " pos_" + pos_lista)
        .replace("{1}", dado)
        .replace("{2}", disabled)
        .replace("{6}", pos_tabela)
        .replaceAll("{7}", id_campo)
        .replaceAll("{5}", "'" + id_campo + "'")
        .replace("{3}", placeholder)
        .replace('{8}', prop.Identifier)
        .replace('{9}', html_data_attributes)
        ;

    str += '<datalist class="data{0}" id="lista{0}">'
        .replaceAll("{0}", id_campo);

    for (let i = 0; i < lista.length; i++) {
        str += '<option value="{0}">{1}'
            .replace("{0}", i)
            .replace("{1}", lista[i]);
    }

    str += '</datalist>';

    var pos_estrutura = lista_aux_secundaria.indexOf(estrutura);
    var namespace = estrutura.Namespace;

    var aux = elementoButton('', 'fa fa-search', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    if (index_estrutura != null) {

        aux = elementoSpan(aux, 'input-group-addon', '', '', 'onclick="evtKeyPressBanco({0}, {1}, 2, {2})"')
            .replace('{0}', "'" + id_campo + "'")
            .replace('{1}', pos_tabela)
            .replace('{2}', index_estrutura)
            ;
    }
    else {
        aux = elementoSpan(aux, 'input-group-addon', '', '', 'onclick="evtKeyPressBanco({0}, {1}, 2)"')
            .replace('{0}', "'" + id_campo + "'")
            .replace('{1}', pos_tabela)
            ;
    }

    var aux2 = elementoButton('', 'fa fa-arrow-circle-right', '', '', '', '', '', '', '', '', '', 'background:transparent;border:none');
    aux2 = elementoSpan(aux2, 'input-group-addon', '', '', `onclick='prepararAbrirNovaAba(event, "${namespace}", ${pos_estrutura}, "${prop.ForeignKeyReference}", "${prop.Identifier}", true)'`);


    var aux5 = elementoDiv(str + aux + aux2, 'input-group', id);

    return aux5;
}

function elementoComboboxArea(lista, classe = "#", id = "", disabled = "", placeholder = "", typeFunc = "", nameFunction = "", evento_incluir = true, index = -1, is_1N = 0, posIf1N = -1, namespace = "", style = "") {
    var str = "";

    str += '<ul class="nav navbar-nav"><li class="dropdown">';
    str += '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Ações<span class="caret"></span></a>';
    str += '<ul style="z-index:100; {3}" class="dropdown-menu" id="{2}">'
        .replace("{2}", id)
        .replace("{3}", style);

    if (evento_incluir) {
        str += '<li class="dropdown-header">Ações padrão</li>';
        str += '<li onclick="gerarHTMLInsert()"><a href="#">Incluir</a></li>';
        str += '<li role="separator" class="divider"></li>'
    }
    str += '<li class="dropdown-header">Ações específicas</li>';
    str += `<li onclick="$('#modal-filtros').modal('show');"><a href="#">Preferências de Filtros</a></li>`;

    for (let i = 0; i < lista.length; i++) {
        str += '<li onclick="executeMethod({1}, {2}, {3}, {4}, \'{5}\')"><a>{0}</a></li>'
            .replace("{0}", lista[i])
            .replace("{1}", i)
            .replace("{2}", index)
            .replace("{3}", is_1N)
            .replace("{4}", posIf1N)
            .replace("{5}", namespace)
            ;
    }

    str += '<ul></li></ul>';

    return str;
}

//Cria o componente combobox do HTML
//param: first_disabled: define se o option do placeholder sera disable ou nao
function elementoCombobox(lista_id, classe = "#", id = "", disabled = "", placeholder = "", typeFunc = "", nameFunction = "", lista_dados = [], dado_selecionado = "", lista_cb_disabled = [], flag = false, style = "", data_names = [], data_values = [], first_disabled = true) {
    //var str = '<input type="text" name="test" class="{1}" list="{2}"/>'.replace("{1}", classe).replace("{2}", id);
    if (lista_dados.length == 0)
        lista_dados = lista_id;

    //constroi os data-attributes utilizando o name passado e o value no mesmo index
    let html_data_attributes = "";
    if (data_names.length > 0 && data_values.length > 0) {
        data_names.forEach(function (currentValue, index) {
            html_data_attributes += `   ${currentValue} = "${data_values[index]}"  `;
        });
    }

    var str = '<select class="{1}" id="{2}" {3} {4}="{5}" style="{6}" {7}>'
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", disabled)
        .replace("{4}", typeFunc)
        .replace("{5}", nameFunction)
        .replace("{6}", style)
        .replace("{7}", html_data_attributes);

    placeholder = placeholder !== "" ? placeholder : "";
    if (placeholder !== "" && dado_selecionado !== "")
        str += `<option value="" ${first_disabled ? disabled : ""} selected>{0}</option>`.replace("{0}", placeholder);

    for (let index = 0; index < lista_id.length; index++) {
        //str += '<option value="{0}">'.replace("{0}", index).replace("{1}", lista[index]);
        if (dado_selecionado !== "" && dado_selecionado.toString().trim() == lista_id[index].toString().trim()) {
            if (lista_cb_disabled.length > 0 && lista_cb_disabled[index] == 1)
                str += '<option value="{0}" selected="selected" disabled>{1}</option>'.replace("{0}", lista_id[index]).replace("{1}", lista_dados[index]);
            else
                str += '<option value="{0}" selected="selected">{1}</option>'.replace("{0}", lista_id[index]).replace("{1}", lista_dados[index]);
        }
        else {
            if (lista_cb_disabled.length > 0 && lista_cb_disabled[index] == 1)
                str += '<option value="{0}" disabled>{1}</option>'.replace("{0}", lista_id[index]).replace("{1}", lista_dados[index]);
            else
                str += '<option value="{0}">{1}</option>'.replace("{0}", lista_id[index]).replace("{1}", lista_dados[index]);
        }
    }

    /*
    if (flag && dado_selecionado === "") {
        str += '<script>$(document).ready(function(){$("#' + id + '")[0].selectedIndex = -1;})</script>';
    }*/

    str += '</select>'
    return str;
}

//Cria o componente checkbox do HTML
//é necessário envolve-lo em uma div class = "form-check"
//target_valor é o valor que o checkbox devera assumir caso esteja checado (assume valor 0 se nao esta checado)
//a variavel dado é trazida do banco e se ela tiver o valor igual ao do target_valor, quer dizer que esse checkbox deve vir checado
function elementoCheckbox(target_valor = "", classe = "", id = "", description = "", name = "", disabled = "", dado = null, data_names = [], data_values = []) {
    var str = '';
    //constroi os data-attributes utilizando o name passado e o value no mesmo index
    let html_data_attributes = "";
    if (data_names.length > 0 && data_values.length > 0) {
        data_names.forEach(function (currentValue, index) {
            html_data_attributes += `${currentValue} = "${data_values[index]}"`;
        });
    }

    //se o dado for == o valor quer dizer que deve ser checked
    if (dado != null && dado == target_valor) {
        str = `<input class="form-check-input ${classe}" onchange='definir_valor_checkbox(id)' type="checkbox" value='${target_valor}' checked='true' target-value="${target_valor}" id="${id}" name="${name}" ${disabled} ${html_data_attributes}>`;
    }
    else {
        str = `<input class="form-check-input ${classe}" onchange='definir_valor_checkbox(id)' type="checkbox" value='0' target-value="${target_valor}" id="${id}" name="${name}" ${disabled} ${html_data_attributes}>`;
    }

    return str;
}

//ao mudar o estado do checkbox, define o valor correto: 0 se não checado, e target-value se checado
function definir_valor_checkbox(id) {
    let element = $(`#${id}`);
    if ($(`#${id}`).is(':checked'))
        $(`#${id}`).val(element.attr('target-value'));
    else
        $(`#${id}`).val(0);
}

function isPrimitive(test) {
    switch (teste) {
        case typeof "" == teste:
            return true;

        case typeof 1 == teste:
            return true;

        case typeof true == teste:
            return true;

        default:
            return false;
    }
};


//Cria componente input do HTML
function elementoInputText(valor = "", classe = "#", id = "", placeholder = "", type = "", required = "", maxLength = "", min_range = "", max_range = "", title = "", disabled = "", funcao = "", text = "", name = "", size = "", data_names = [], data_values = []) {
    //constroi os data-attributes utilizando o name passado e o value no mesmo index
    let html_data_attributes = "";
    if (data_names.length > 0 && data_values.length > 0) {
        data_names.forEach(function (currentValue, index) {
            html_data_attributes += `${currentValue} = "${data_values[index]}"`;
        });
    }

    var str = '<input {11} type="{4}" value="{0}" class="{1}" id="{2}" placeholder="{3}" maxlength="{6}" min="{7}" max="{8}" value="{D}" name="{E}" title="{9}" size="{10}" {5} {B} {A}/>{C}'
        .replace("{0}", (type.toUpperCase() == "PASSWORD") ? valor.substring(0, 5) : valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", placeholder)
        .replace("{4}", type)
        .replace("{5}", required)
        .replace("{6}", maxLength)
        .replace("{7}", min_range)
        .replace("{8}", max_range)
        .replace("{9}", title)
        .replace("{10}", size)
        .replace("{A}", disabled)
        .replace("{B}", funcao)
        .replace("{C}", text)
        .replace("{E}", name)
        .replace("{11}", html_data_attributes)
        ;

    return str;
}

function elementoTextArea(texto = "", classe = "", id = "", required = "", disabled = "", rows = 1, data_names = [], data_values = []) {
    //constroi os data-attributes utilizando o name passado e o value no mesmo index
    let html_data_attributes = "";
    if (data_names.length > 0 && data_values.length > 0) {
        data_names.forEach(function (currentValue, index) {
            html_data_attributes += `${currentValue} = "${data_values[index]}"`;
        });
    }

    var str = '<textarea id="{0}" class="{1}" rows="{5}" cols="50" {3} {4} {6} >{2}'
        .replace('{0}', id)
        .replace('{1}', classe)
        .replace('{2}', texto)
        .replace('{3}', required)
        .replace('{4}', disabled)
        .replace('{5}', rows)
        .replace('{6}', html_data_attributes)
        ;

    str += '</textarea>';
    return str;
}

function elementoLabel(valor = "", classe = "", id = "", _for = "") {
    var str = '<label class="{1}" id="{2}" for="{3}">{0}</label>'
        .replace('{0}', valor)
        .replace('{1}', classe)
        .replace('{2}', id)
        .replace('{3}', _for)
        ;

    return str;
}

//Cria o componente não nativo do HTML combobox-checkbox
function elementoComboboxCheckbox(lista, classe = "", id = "", name = "", disabled = "", checked = "") {

    var header = '<select id="sel1"><option>Selecione os campos para serem visualizados</option></select>';
    var temp = elementoDiv("", "overSelect");
    header = header + temp;
    header = elementoDiv(header, "selectBox", "sub2", "", "", "", "", "exibirCheckboxes()");

    var campos = "";
    var temp;
    for (let i = 0; i < lista.length; i++) {
        temp = elementoInputText(lista[i], "", "combo" + i, "", "checkbox", "", "", "", "", "", "", 'onclick="exibirInputs(' + i + ')"', lista[i]);
        temp = '<label for="combo' + i + '">' + temp + '</label>';

        campos += temp;
    }

    campos = elementoDiv(campos, "", 'checkboxes');
    str = elementoDiv(header + campos, 'multiselect', 'sub1');
    return str;
}

var expanded = false;
function exibirCheckboxes() {
    var checkboxes = document.getElementById("checkboxes");
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}

function exibirInputs(num) {
    console.log('Se constrói com humildade');
}

//Cria o componente Button do HTML
function elementoButton(valor, classe = "#", id = "", evento = "", funcaoEvento = "", form = "", type = "button", data_dismiss = "", aria_label = "", data_toggle = "", data_target = "", style = "", disabled = "") {
    var str = '<button type="{6}" class="{1}" id="{2}" {3}="{4}" data-dismiss="{7}" aria-label="{8}" data-toggle="{9}" data-target="{A}" style="{B}" {C}>{0}</button>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", evento)
        .replace("{4}", funcaoEvento)
        //.replace("{5}", form)
        .replace("{6}", type)
        .replace("{7}", data_dismiss)
        .replace("{8}", aria_label)
        .replace("{9}", data_toggle)
        .replace("{A}", data_target)
        .replace("{B}", style)
        .replace("{C}", disabled);

    return str;
}

//Cria o componente div do HTML
function elementoDiv(valor = "", classe = "#", id = "", role = "", aria_labelledby = "", aria_expanded = "", aria_multiselectable = "", onclick = "", tab_index = "", aria_hidden = "") {
    var str = '<div class="{1}" id="{2}" role="{3}" aria-labelledby="{4}" aria-expanded="{5}" aria-multiselectable="{6}" onclick="{7}" tabindex="{8}" aria-hidden="{9}">{0}</div>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", role)
        .replace("{4}", aria_labelledby)
        .replace("{5}", aria_expanded)
        .replace("{6}", aria_multiselectable)
        .replace("{7}", onclick)
        .replace("{8}", tab_index)
        .replace("{9}", aria_hidden)
        ;
    return str;
}

//Cria o componente nav do HTML
function elementoNav(valor = "", classe = "") {
    var str = '<nav class="{1}">{0}</nav>'
        .replace('{0}', valor)
        .replace('{1}', classe);

    return str;
}

//Cria o componente form do HTML
function elementoForm(valor = "", classe = "", id = "", name = "", action = "") {
    var str = '<form class="{1}" id="{2}" name="{3}" action="{4}">{0}</form>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", name)
        .replace("{4}", action);

    return str;
}

//Cria o componente Markdown do HTML (Títulos H1, H2, etc...)
function elementoMarkdown(valor = "", enfase = "1", classe = "#", id = "") {
    var str = '<h{1} class="{2}" id="{3}">{0}</h{1}>'.replace("{0}", valor).replaceAll("{1}", enfase).replace("{2}", classe).replace("{3}", id);

    return str;
}

//Cria o componente link (<a>)
function elementoLinkA(valor = "", classe = "#", id = "", href = "", data_toggle = "", data_parent = "", aria_expanded = "", aria_controls = "") {
    var str = '<a class="{1}" id="{2}" href="{3}" data-toggle="{4}" data-parent="{5}" aria-expanded="{6}" aria-controls="{7}">{0}</a>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id)
        .replace("{3}", href)
        .replace("{4}", data_toggle)
        .replace("{5}", data_parent)
        .replace("{6}", aria_expanded)
        .replace("{7}", aria_controls);

    return str;
}

//Cria um texto em itálico <i>
function elementoItalic(valor = "", classe = "#", id = "") {
    var str = '<i class="{1}" id="{2}">{0}</i>'
        .replace("{0}", valor)
        .replace("{1}", classe)
        .replace("{2}", id);

    return str;
}

function elementoSpan(value = "", classe = "", id = "", aria_hidden = "", evento = "", eventoMetodo = "", dataset = "", dataset_value = "") {
    var str = '';

    if (dataset != "" && data_values != "") {
        var str = '<span class="{1}" id="{2}" aria-hidden="{3}" {4}="{5}" {6}="{7}">{0}</span>'
            .replace("{0}", value)
            .replace("{1}", classe)
            .replace("{2}", id)
            .replace("{3}", aria_hidden)
            .replace("{4}", evento)
            .replace("{5}", eventoMetodo)
            .replace("{6}", dataset)
            .replace("{7}", dataset_value);
    }
    else {
        var str = '<span class="{1}" id="{2}" aria-hidden="{3}" {4}="{5}" >{0}</span>'
            .replace("{0}", value)
            .replace("{1}", classe)
            .replace("{2}", id)
            .replace("{3}", aria_hidden)
            .replace("{4}", evento)
            .replace("{5}", eventoMetodo)
            ;
    }

    return str;
}

function elementoModalExclusao() {
    var str = "";
    //Parte do cabeçalho
    str = elementoSpan("", "", "", "true");
    str = elementoButton(str, "close", "", "", "", "", "button", "modal", "Fechar");
    var markdown = elementoMarkdown('Excluir', '5', 'modal-title', 'exemplo-modal');
    var header = elementoDiv(markdown + str, 'modal-header');
    //Parte do corpo
    str = 'Deseja excluir o item selecionado?';
    var body = elementoDiv(str, 'modal-body');
    //Parte do rodapé
    str = elementoButton('Confirmar', 'btn btn-primary', '', 'onclick', 'prosseguirExclusao()', '', 'button', '');
    str += elementoButton('Cancelar', 'btn btn-secondary', '', 'onclick', 'cancelarExclusao()', '', 'button', '');
    var footer = elementoDiv(str, 'modal-footer');

    str = header + body + footer;

    str = elementoDiv(str, 'modal-content');
    str = elementoDiv(str, 'modal-dialog', '', 'document');
    str = elementoDiv(str, 'modal fade', 'modal-exclusao', 'dialog', 'exampleModalLabel', '', '', '', '-1', 'true');
    return str;
}

function elementoTabs(qtde_tabs, valor_atual, class_tab, lista_abas, str_id) {

    //var str_html = '<div class="container">';
    var str_html = '<ul class="nav nav-tabs {1}">'
        .replace("{1}", "form" + class_tab)
        .replace("{2}", class_tab);

    //Remover a linha abaixo depois
    var evento_fk = lista_abas[0].fk ? 'evtMudarAbaBanco' : 'evtMudarAba';

    str_html += '<li class=active><a href="#menu{0}" onclick="{3}({2}, {4}, {5})">{1}</a></li>'
        .replace("{0}", "" + str_id + valor_atual)
        .replace("{1}", "" + lista_abas[0].display)
        .replace("{2}", "" + valor_atual)
        .replace("{3}", "" + evento_fk)
        .replace("{4}", "" + 0)
        .replace("{5}", "'" + str_id + "'")
        ;

    valor_atual++;
    for (let index = 1; index < qtde_tabs; index++) {
        var evento_fk = lista_abas[index].fk ? 'evtMudarAbaBanco' : 'evtMudarAba';
        str_html += '<li><a href="#menu{0}" onclick="{3}({2}, {4}, {5})">{1}</a></li>'
            .replace("{0}", "" + str_id + valor_atual)
            .replace("{1}", "" + lista_abas[index].display)
            .replace("{2}", "" + valor_atual)
            .replace("{3}", "" + evento_fk)
            .replace("{4}", "" + index)
            .replace("{5}", "'" + str_id + "'")
            ;
        valor_atual++;
    }

    str_html += '</ul>';
    return str_html;
}

function elementoTable(classe = "", id = "", role = "", aria_describedy = "") {
    var lista_cabecalho = selecionados;
    var lista_body = lista_dados;

    var header = elementoTHead(lista_cabecalho);
    var body = elementoTBody(lista_body, lista_cabecalho);

    var tabela = '<table class="{0}" id="{1}" role="{2}" aria-describedy="{3}">{4}{5}</table>'
        .replace("{0}", classe)
        .replace("{1}", id)
        .replace("{2}", role)
        .replace("{3}", aria_describedy)
        .replace("{4}", header)
        .replace("{5}", body)
        ;

    return tabela;
}

function elementoTHead(lista) {
    const index = 0;

    var aux_lista = [];
    for (let i = 0; i < lista.length; i++) {
        var j = 0;
        while (j < lista_estrutura.Propriedades.length && lista[i].toUpperCase() != lista_estrutura.Propriedades[j].Identifier.toUpperCase()) {
            j++;
        }

        if (j < lista_estrutura.Propriedades.length)
            aux_lista.push({ INDEX: j, DISPLAY: pegarDisplay(lista_estrutura.Propriedades[j]), IDENTIFIER: lista_estrutura.Propriedades[j].Identifier.toUpperCase() });
    }

    var str = '<thead><tr role="row">';

    str += '<th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-label=": activate to sort column ascending" style="width: 40px;"></th>';
    for (let i = 0; i < aux_lista.length; i++) {
        str += `<th id="coluna${i}index${0}" class="colunas0 sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-label=": activate to sort column ascending" style="width: 100px;" value="${aux_lista[i].INDEX}">
        <i data-toggle="tooltip" title="Clique duas vezes para ordernar, clique uma vez para mudar o tipo de ordenação." class="orderby${index} fa fa-square-o" data-orderby="" data-tipoordem="1" data-ordemclicada="" data-identifier="${aux_lista[i].IDENTIFIER}" id="orderbyColuna${aux_lista[i].INDEX}Index${index}" onclick="mudarTipoOrdenacao(this)" ondblclick="eventoOrderBy(this)"></i>
        <i class="fa fa-arrow-left movercoluna" onclick="moverColuna(${index}, ${i}, -1, 0)"></i>
        {0}
        <i class="fa fa-arrow-right movercoluna " onclick="moverColuna(${index},${i}, 1, 0)"></i>
        </th>`
            .replace("{0}", aux_lista[i].DISPLAY)
            ;
    }

    str += '</tr></thead>';
    return str;
}

function elementoTBody(lista, lista_cabecalho) {
    var qtde_campos = lista_cabecalho.length;
    var str = '<tbody>';
    var corFundo = "#FAFAFA";

    for (let i = 0; i < lista.length; i++) {

        str += '<tr onclick="eventoExpandir({1}, {2})" id="more{0}">'
            .replace("{0}", i)
            .replace("{1}", i)
            .replace("{2}", qtde_campos);

        str += `<td class="center">
                    <div class='row'>
                        <div class='col-md-6'>
                            <i class="fa fa-plus circle"> </i>
                        </div>
                    </div>
                </td>`;

        corFundo = corFundo == '#EFEFEF' ? '#FAFAFA' : '#EFEFEF';

        for (let j = 0; j < qtde_campos; j++) {
            var aux = lista_cabecalho[j].toLowerCase();
            pos = 0;
            while (pos < lista_indices_cabecalho.length && aux !== lista_indices_cabecalho[pos])
                pos++;

            var res = lista[i][Object.getOwnPropertyNames(lista[i])[pos]];


            var posEstrutura = 0;
            while (posEstrutura < lista_estrutura.Propriedades.length && aux !== lista_estrutura.Propriedades[posEstrutura].Identifier.toLowerCase())
                posEstrutura++;

            if (posEstrutura < lista_estrutura.Propriedades.length) {
                var auxId = lista_estrutura.Propriedades[posEstrutura].Identifier.toLowerCase();
                if (lista_estrutura.Propriedades[posEstrutura].ForeignKey == "ForeignKey") {

                    var className1N = lista_estrutura.Propriedades[posEstrutura].ForeignKeyClass;
                    var posEstrutura1N = 0;
                    while (posEstrutura1N < lista_aux_secundaria.length && className1N != lista_aux_secundaria[posEstrutura1N].ClassName)
                        posEstrutura1N++;

                    if (posEstrutura1N < lista_aux_secundaria.length) {
                        var namespace1N = lista_aux_secundaria[posEstrutura1N].Namespace;
                        var campoName = lista_estrutura.Propriedades[posEstrutura].Identifier;
                        str += '<td title="Navegar para a tela de: ' + className1N + '" style="background-color:' + corFundo + '"><a onclick="prepararAbrirNovaAba(null, \'' + namespace1N + '\', ' + posEstrutura1N + ', \'' + campoName + '\', \'' + campoName + '\', true, \'' + res + '\')"><p align="center">' + res + '</p></a></td>';
                    }

                }
                else {
                    str += '<td><p align="center">{0}</p></td>'
                        .replace("{0}", res);

                }
            }

        }
        str += '</tr>';
    }
    str += '</tbody>';
    return str;
}

function pegarPosicaoListaDados(nome_indice, lista_dados, num) {
    var i = 0;
    while (i < lista_indices_cabecalho.length && nome_indice !== lista_indices_cabecalho[i])
        i++;

    if (i < lista_indices_cabecalho.length)
        return lista_dados[num][Object.getOwnPropertyNames(lista_dados[num])[i]];

    return null;
}

function eventoExpandir(num, colspan) {
    var str;
    var i;
    var dado;
    var attrib_id = 'moreX' + num;
    if ($('#more' + num).next('tr').attr('id') === attrib_id) { //Bloco de código para contrair os dados buscados, bem como remover o mesmo da lista dados totais.
        $('#more' + num + ' td:first-child > div:first-child > div:first-child > i').removeClass();
        $('#more' + num + ' td:first-child > div:first-child > div:first-child > i').addClass('fa fa-plus circle');
        $('#more' + num).next('tr').remove();

        var lista_pk = pegarChavesPrimarias(lista_estrutura);
        for (let i = 0; i < lista_pk.length; i++)
            lista_pk[i] = lista_estrutura.Propriedades[lista_pk[i]].Identifier.toLowerCase();

        var lista_index = [];
        for (let i = 0; i < lista_pk.length; i++)
            lista_index.push(lista_indices_dados.indexOf(lista_pk[i]));

        i = 0;
        while (i < lista_chaves_primarias.length - 1) {
            dado = pegarPosicaoListaDados(lista_indices_estrutura[lista_chaves_primarias[i]], lista_dados, num);
            pesquisa += lista_estrutura.Propriedades[lista_chaves_primarias[i]].Identifier + "=" + dado + "&";
            i++;
        }
        dado = pegarPosicaoListaDados(lista_indices_estrutura[lista_chaves_primarias[i]], lista_dados, num);

        i = 0;
        while (i < lista_dados_totais.length && dado != lista_dados_totais[i][0][Object.getOwnPropertyNames(lista_dados_totais[i][0])[lista_index[0]]])
            i++;

        lista_dados_totais.splice(i, 1);
    }
    else { //Bloco de código para expandir e exibir todos os campos dos dados buscados, bem como adicionar na lista de dados totais.
        str = eventoExpandirExibir(num, colspan);
        $('#more' + num + ' td:first-child > div:first-child > div:first-child > i').removeClass();
        $('#more' + num + ' td:first-child > div:first-child > div:first-child > i').addClass('fa fa-minus circle');
        $('#more' + num).after(str);
        eventoAlterarIndividual(num);
    }

    //depois de adicionar os campos, deve aplicar as mascaras pendentes
    aplicarMascaras();
    adicionarEventosRightClick();

}

function eventoExpandirExibir(num, colspan) {
    var i;
    var dado;
    lista_abas = [];
    lista_abas = pegarAbas(lista_estrutura, lista_abas);
    var pesquisa = "";

    i = 0;
    var lista_pk = pegarChavesPrimarias(lista_estrutura);
    while (i < lista_pk.length - 1) {
        dado = pegarPosicaoListaDados(lista_indices_estrutura[lista_pk[i]], lista_dados, num);
        if (dado != "")
            pesquisa += lista_estrutura.Propriedades[lista_pk[i]].Identifier + "=" + dado + "&";
        i++;
    }
    dado = pegarPosicaoListaDados(lista_indices_estrutura[lista_pk[i]], lista_dados, num);
    if (dado != "")
        pesquisa += lista_estrutura.Propriedades[lista_pk[i]].Identifier + "=" + dado;
    else if (pesquisa[pesquisa.length - 1] == '&')
        pesquisa = pesquisa.substring(0, (pesquisa.length - 1));

    //var pesquisa = $('#combo0').val() + "=" + lista_dados[num][Object.getOwnPropertyNames(lista_dados[num])[0]];
    var filtros = criarJsonPesquisa(pesquisa, lista_estrutura.Namespace, []);
    var dados_pesquisado = eventoPesquisarNoBancoExpandir(filtros, 1);
    //Lista de índices
    lista_indices_dados = Object.getOwnPropertyNames(dados_pesquisado[0]);
    for (let i = 0; i < lista_indices_dados.length; i++)
        lista_indices_dados[i] = lista_indices_dados[i].toLowerCase();

    var str = '<tr id="moreX{1}" index-estrutura="{2}" class="tr_pai"><td colspan="{0}">'
        .replace("{0}", ++colspan)
        .replace("{1}", num)
        .replace("{2}", num)
        ;
    lista_dados_totais.push(dados_pesquisado);
    lista_dados_totais_classes.push(namespace + lista_estrutura.ClassName);

    var str_id = "principal";
    str += gerarFormulario(dados_pesquisado, num, lista_estrutura, lista_estrutura_secundarias, lista_abas, lista_indices_dados, str_id);
    str += gerarBotoesFormulario(num);

    str += '</div></td></tr>';
    return str;

}

function gerarFormulario(dados, posicao, lista_estrutura, lista_estrutura_secundarias, lista_abas, lista_indices_dados, str_id, index_estrutura = null) {
    var id_campo = posicao * lista_estrutura.Propriedades.length;
    var lista_str = [];
    var indice_aba = 0;
    for (let i = 0; i < lista_abas.length; i++)
        lista_str.push("");

    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {

        var propriedade = lista_estrutura.Propriedades[i];
        var j = 0;
        if (propriedade.ForeignKey == "1N") {
            while (j < lista_abas.length && propriedade.Identifier != lista_abas[j].identifier)
                j++;

            if (j < lista_abas.length)
                indice_aba = lista_abas[j].index;
        }
        else {
            while (j < propriedade.AnnotationsProp.length && propriedade.AnnotationsProp[j].AttributeName !== 'TAB')
                j++;

            if (j < propriedade.AnnotationsProp.length) {

                var k = 0;
                while (k < lista_abas.length && lista_abas[k].identifier != propriedade.AnnotationsProp[j].Parametros[0].Value)
                    k++;

                if (k < lista_abas.length) {
                    indice_aba = k;
                }
            }
        }

        var identifier = lista_estrutura.Propriedades[i].Identifier.toLowerCase();
        var pos = lista_indices_dados.indexOf(identifier);

        lista_str[indice_aba] += configuraTipoCampo(lista_estrutura.Propriedades[i], dados[0][Object.getOwnPropertyNames(dados[0])[pos]], id_campo++, 'read', lista_estrutura_secundarias, str_id, i, index_estrutura);
        indice_aba = 0;
    }
    if (lista_str[0].length == 0) {
        lista_abas.shift();
        lista_str.shift();
    }

    indice_aba = posicao * lista_abas.length;

    var str = "";
    var tabs = elementoTabs(lista_abas.length, indice_aba, 0, lista_abas, str_id);
    //str += elementoDiv(lista_str[0], "tab-pane container-fluid fade in active", "menu" + str_id + indice_aba);
    str += elementoDiv(lista_str[0], "tab-pane fade in active", "menu" + str_id + indice_aba);
    indice_aba++;
    for (let i = 1; i < lista_str.length; i++) {
        //str += elementoDiv(lista_str[i], "tab-pane container-fluid fade", "menu" + str_id + indice_aba);
        str += elementoDiv(lista_str[i], "tab-pane fade", "menu" + str_id + indice_aba);
        indice_aba++;
    }
    str = elementoDiv(str, "tab-content row");
    //str = elementoDiv(tabs + str, "container-fluid");
    str = elementoDiv(tabs + str);
    str = elementoDiv(str, "panel-body");
    return str;
}

//Clique para alterar a aba
function evtMudarAba(tab, index, str_id) {
    console.log('Mudar aba normal');
    $('.nav-tabs a[href="#menu' + str_id + tab + '"]').tab('show');
    lista_aux_secundaria = lista_estrutura_secundarias; //Muda a lista de estrutura para as listas de estruturas primárias.
}

function limparEstruturaInsert(estrutura_insert) {
    var lista_atributos = Object.getOwnPropertyNames(estrutura_insert);

    for (let i = 0; i < lista_atributos.length; i++) {
        estrutura_insert[Object.getOwnPropertyNames(estrutura_insert)[i]] = "";
    }


    return estrutura_insert;
}

function eventoNovo1N(tab, str_id, index) {
    var posicao_dado = Math.floor(tab / lista_abas.length);

    var aux_nome = lista_abas[index].class;
    var i = 0;
    lista_aux_secundaria = lista_estrutura_1N_secundarias; //Muda a lista de estrutura secundária para as listas de estruturas secundárias 1N.
    while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
        i++;

    if (i < lista_estrutura_1N.length) {
        var estrutura_1N = lista_estrutura_1N[i];
        var estrutura_insert_1N = limparEstruturaInsert(lista_estrutura_insert_1N[i]);

        var str_remove = '#table' + tab + ' #insert1N' + index;
        var str_insert = '#table' + tab + ' .tbody1N';
        var pos_pkfk = pegarPKdoFK(estrutura_1N, lista_estrutura);
        if (pos_pkfk != -1) { //Setando o id da classe pai.
            //#######################################################################//
            var obj_read = { 'AttributeName': 'READ', 'Parametros': Array(0) };
            estrutura_1N.Propriedades[pos_pkfk].AnnotationsProp.push(obj_read);

            var lista_indices_dados_sec = Object.getOwnPropertyNames(estrutura_insert_1N);
            for (let k = 0; k < lista_indices_dados_sec.length; k++)
                lista_indices_dados_sec[k] = lista_indices_dados_sec[k].toLowerCase();

            var identifier = estrutura_1N.Propriedades[pos_pkfk].Identifier.toLowerCase();
            var pos_sec = lista_indices_dados_sec.indexOf(identifier);

            //#######################################################################//
            var lista_pk = pegarChavesPrimarias(lista_estrutura);
            var lista_indices_dados_pri = Object.getOwnPropertyNames(lista_dados[posicao_dado]);
            for (let k = 0; k < lista_indices_dados_pri.length; k++)
                lista_indices_dados_pri[k] = lista_indices_dados_pri[k].toLowerCase();

            var identifier = lista_estrutura.Propriedades[lista_pk[0]].Identifier.toLowerCase();
            var pos_pri = lista_indices_dados_pri.indexOf(identifier);

            estrutura_insert_1N[Object.getOwnPropertyNames(estrutura_insert_1N)[pos_sec]] = lista_dados[posicao_dado][Object.getOwnPropertyNames(lista_dados[posicao_dado])[pos_pri]];
        }


        var campo = 'insert1N' + tab + 'C';

        var lista_abas_1N = [];
        lista_abas_1N = pegarAbas(estrutura_1N, lista_abas_1N);

        //pega os campos em comum com a tela anterior.
        //ex: tela de roteiro e tela de operacoes tem os campos em comum: maq_id, seq_transformacao e cod_produto
        //retorna o index do atributo dentro da estrutura_1N
        var campos_da_tela_anterior = pegarCamposTelaAnterior(estrutura_1N, lista_estrutura);
        campos_da_tela_anterior.forEach(function (currentValue) {

            //desabilita o campo com um annotation
            var obj_read = { 'AttributeName': 'READ', 'Parametros': Array(0) };
            estrutura_1N.Propriedades[currentValue].AnnotationsProp.push(obj_read);

            //utiliza o index retornado para pegar o identifier do atributo
            var identifier = estrutura_1N.Propriedades[currentValue].Identifier;
            var fk_identifier = estrutura_1N.Propriedades[currentValue].ForeignKeyReference;
            console.log("CAMPO: " + identifier + " / VALOR: " + lista_dados[posicao_dado][fk_identifier]);

            //utiliza o identifier dele para preencher os dados
            estrutura_insert_1N[identifier] = lista_dados[posicao_dado][fk_identifier];
        });

        var formulario = gerarFormularioInsert(estrutura_1N, estrutura_insert_1N, lista_estrutura_1N_secundarias, lista_abas_1N, 0, campo, i);
        var botoes = gerarBotoesFormulario1NInsert(i, tab, index, str_id, str_remove, posicao_dado);

        //var formulario = eventoNovo(estrutura_1N, estrutura_insert_1N);
        var str = '<tr><td colspan="' + qtd_display + '" id="insert1N{0}">{1}{2}</td></tr>'
            .replace('{0}', "" + index)
            .replace('{1}', "" + formulario)
            .replace('{2}', "" + botoes);

        if (pos_pkfk != -1) {
            //Retirar o LOCK do campo de estrutura_1N.
            estrutura_1N.Propriedades[pos_pkfk].AnnotationsProp.splice(estrutura_1N.Propriedades[pos_pkfk].AnnotationsProp.length - 1, 1);
        }

        $(str_remove).remove();
        $(str_insert).prepend(str);

        //depois de adicionar os campos, deve aplicar as mascaras pendentes
        aplicarMascaras();
        adicionarEventosRightClick();

    }
}

function aplicarMascaras() {
    masks.forEach(function (current) {
        let id = current._target;
        let value = current._mask;

        $(`.input${id}`).mask(value);
    });

    //é preciso resetar a variavel pois ela guarda valores por ser global
    masks = [];
}

function pegarCamposTelaAnterior(estrutura_1N, estrutura_principal) {
    var pk_iguais = [];
    let i = 0;

    //percorre cada propriedade, comparando com as propriedades da estrutura principal
    while (i < estrutura_1N.Propriedades.length) {
        for (let x = 0; x < estrutura_principal.Propriedades.length; x++) {

            //se encontrar propriedades com o mesmo nome, adiciona elas ao array
            if (estrutura_1N.Propriedades[i].ForeignKeyClass != "") { //se o atributo for uma foreign key é preciso olhar para a foreign key reference e verificar se aponta para a mesma classe

                if (estrutura_1N.Propriedades[i].ForeignKeyReference == estrutura_principal.Propriedades[x].Identifier &&
                    estrutura_1N.Propriedades[i].ForeignKeyClass == estrutura_principal.ClassName) {
                    pk_iguais.push(i);
                }
            }

        }

        i++;
    }

    return pk_iguais;
}

function pegarPKdoFK(estrutura_1N, estrutura_principal) {
    var i = 0;
    while (i < estrutura_1N.Propriedades.length && estrutura_1N.Propriedades[i].ForeignKeyClass != estrutura_principal.ClassName)
        i++;

    if (i < estrutura_1N.Propriedades.length)
        return i;
    return -1;
}

function eventoDisplay1N(tab, str_id, index, i_clicado) {
    var aux_nome = lista_abas[index].class;
    var i = 0;
    let estrutura_1N;
    lista_aux_secundaria = lista_estrutura_1N_secundarias; //Muda a lista de estrutura secundária para as listas de estruturas secundárias 1N.
    while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
        i++;

    if (i < lista_estrutura_1N.length) {
        estrutura_1N = lista_estrutura_1N[i];

        //pega o indice clicado e remove do array ou adiciona no final
        //dessa forma é possivel organizar a ordem dos elementos conforme se adiciona-os no final do array
        if (lista_indices_display1N.indexOf(i_clicado) == -1)
            lista_indices_display1N.push(i_clicado);
        else
            lista_indices_display1N.splice(lista_indices_display1N.indexOf(i_clicado), 1);

        lista_indices = lista_indices_display1N;

        var posicao_dado = pegarPosicaoDado1N(tab);
        var ativo = lista_paginas_1N[posicao_dado];

        pesquisar1N(tab, index, str_id, posicao_dado, ativo, lista_indices);
        marcarCheckboxDisplay1N(lista_indices, tab);

        //converte a lista de indices para lista de nomes e salva na tabela de preferencias
        let lista_nomes = listaIndicesParaListaNomes(lista_indices, estrutura_1N);
        salvarPreferenciaDisplay(lista_nomes, estrutura_1N);
    }
}

function eventoDisplay(i_clicado) {

    //pega o indice clicado e remove do array ou adiciona no final
    //dessa forma é possivel organizar a ordem dos elementos conforme se adiciona-os no final do array
    if (lista_indices_display.indexOf(i_clicado) == -1)
        lista_indices_display.push(i_clicado);
    else
        lista_indices_display.splice(lista_indices_display.indexOf(i_clicado), 1);

    //obtem uma lista de atributos separados por ; e transforma num array
    var temp = listaIndicesParaListaNomes(lista_indices_display, lista_estrutura);
    if (temp.indexOf(';') != -1) {
        selecionados = temp.split(';').filter(function (v) { return v != ""; });
    }

    eventoPesquisarPrincipal();

    //converte a lista de indices para lista de nomes e salva na tabela de preferencias
    let lista_nomes = listaIndicesParaListaNomes(lista_indices_display, lista_estrutura);
    salvarPreferenciaDisplay(lista_nomes, lista_estrutura);
}

//salva as configurações de display na tabela de preferencias
function salvarPreferenciaDisplay(lista_nomes, estrutura) {
    atualizarPreferencia("update", "Configurações de Display", estrutura.Namespace, lista_nomes, "DISPLAY");
}

//converte uma lista de indices que apontam para atributos, para uma lista de nomes dos respectivos atributos
//a lista de nomes é retornada como uma string com seus valores separados por ;
function listaIndicesParaListaNomes(lista_indices, estrutura) {
    let lista_nomes = '';

    lista_indices.forEach(function (currentValue) {
        let nome = estrutura.Propriedades[currentValue].Identifier;
        lista_nomes += nome + ';';
    });

    return lista_nomes;
}

//converte uma lista de nomes de atributos para uma lista de indices
function listaNomesParaListaIndices(lista_nomes, estrutura) {
    let lista_indices = [];

    if (Array.isArray(lista_nomes)) {

        lista_nomes.forEach(function (nome) {
            let _nome = nome;

            //para cada nome no array, percorre as propriedades para achar o identifier correto
            estrutura.Propriedades.every(function (propriedade, index) {
                if (_nome.toUpperCase() == propriedade.Identifier.toUpperCase()) {
                    lista_indices.push(index);
                    return false;
                }

                return true;
            });
        });
    }
    else {

        lista_nomes.split(';').forEach(function (nome) {
            let _nome = nome;

            //para cada nome no array, percorre as propriedades para achar o identifier correto
            estrutura.Propriedades.every(function (propriedade, index) {
                if (_nome.toUpperCase() == propriedade.Identifier.toUpperCase()) {
                    lista_indices.push(index);
                    return false;
                }

                return true;
            });
        });
    }

    return lista_indices;
}

//recebe a lista de indices e marca as colunas selecionadas
function marcarCheckboxDisplay1N(lista_indices, tab) {
    for (let j = 0; j < lista_indices.length; j++) {
        $('#toggle-playsis-' + tab + '-col' + lista_indices[j]).prop('checked', true);
    }
}

function marcarCheckboxDisplay(lista_indices) {
    for (let j = 0; j < lista_indices.length; j++) {
        $('#toggle-playsis-' + lista_indices[j]).prop('checked', true);
    }
}

function displayAll(tab, str_id, index) {
    var aux_nome = lista_abas[index].class;
    var i = 0;
    lista_aux_secundaria = lista_estrutura_1N_secundarias; //Muda a lista de estrutura secundária para as listas de estruturas secundárias 1N.
    while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
        i++;

    if (i < lista_estrutura_1N.length) {
        var estrutura_1N = lista_estrutura_1N[i];
        var lista_indices = [];
        for (let i = 0; i < estrutura_1N.Propriedades.length; i++)
            lista_indices.push(i);

        var posicao_dado = pegarPosicaoDado1N(tab);
        var ativo = lista_paginas_1N[posicao_dado];

        pesquisar1N(tab, index, str_id, posicao_dado, ativo, lista_indices);
    }
}

function evtMudarAbaBanco(tab, index, str_id) {
    $('.nav-tabs a[href="#menu' + str_id + tab + '"]').tab('show');
    var posicao_dado = pegarPosicaoDado1N(tab);
    lista_paginas_1N[posicao_dado] = 1;

    pesquisar1N(tab, index, str_id, posicao_dado, lista_paginas_1N[posicao_dado]);
}


function pegarPosicaoDado1N(tab) { //Pega a posição dos dados na lista de dados.
    if ($('#divexpedicao').length == 0) { //APS_Comment
        return Math.floor(tab / lista_abas.length);
    }
    else {
        var lista_pk = pegarChavesPrimarias(lista_estrutura);
        var lista_indices = Object.getOwnPropertyNames(lista_dados[0]);
        for (let i = 0; i < lista_indices.length; i++)0,
            lista_indices[i] = lista_indices[i].toLowerCase();

        var identifier = lista_indices.indexOf(lista_estrutura.Propriedades[lista_pk[0]].Identifier.toLowerCase());
        var aux_i = 0;
        while (aux_i < lista_dados.length && lista_dados[aux_i][Object.getOwnPropertyNames(lista_dados[aux_i])[identifier]] != global_modal_id)
            aux_i++;

        if (aux_i < lista_dados.length)
            return aux_i;
    }
    return -1;
}

async function pesquisar1N(tab, index, str_id, posicao_dado, valor_ativo, lista_indices = []) { //A Flag diz o método está sendo chamado do Dynamics(true) ou lá do APS(false)
    var dado_principal = [];
    dado_principal = lista_dados[posicao_dado];

    var lista_pk = pegarChavesPrimarias(lista_estrutura);

    var aux_nome = lista_abas[index].class;
    var i = 0;
    lista_aux_secundaria = lista_estrutura_1N_secundarias; //Muda a lista de estrutura secundária para as listas de estruturas secundárias 1N.
    while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
        i++;

    if (i < lista_estrutura_1N.length) {
        var estrutura_1N = lista_estrutura_1N[i];

        if (lista_indices.length == 0) {

            //pesquisa nas preferencias se existe uma configuração de display salva
            let preferencia = await pesquisarPreferencia(lista_estrutura_1N[i].Namespace, "DISPLAY");
            lista_nomes = preferencia != null ? preferencia.PRE_VALOR : "";
            if (lista_nomes.length > 0) {
                lista_indices = listaNomesParaListaIndices(lista_nomes, lista_estrutura_1N[i]); //converte a lista de nomes para uma lista de indices
            }
            else {
                //se não encontrar nenhuma configuração de display, utiliza os atributos marcaods no checkbox
                //caso não tenha nenhum marcado no checkbox ainda, a função gerarTela1N irá detectar que o lista_indices está vazio e irá pegar os 5 primeiros atributos da classe
                for (let i = 0; i < estrutura_1N.Propriedades.length; i++) {
                    if ($('#toggle-playsis-' + tab + '-col' + i).is(':checked'))
                        lista_indices.push(i);
                }
            }
        }

        var j = 0;
        var lista_fk = [];

        while (j < estrutura_1N.Propriedades.length) {
            if (estrutura_1N.Propriedades[j].ForeignKeyClass == lista_estrutura.ClassName) /*|| estrutura_1N.Propriedades[j].ForeignKeyClass == lista_estrutura.Namespace)*/
                lista_fk.push(j);
            j++;
        }

        //if (j < estrutura_1N.Propriedades.length && dado_principal != undefined) {
        if (lista_fk.length > 0 && dado_principal != undefined) {
            var temp_id = "";
            var clausula_where = "";

            var lista_pos_principal = [];
            for (let wi = 0; wi < lista_fk.length; wi++) {
                var wj = 0;
                //while (wj < lista_estrutura.Propriedades.length && (estrutura_1N.Propriedades[lista_fk[wi]].Identifier != lista_estrutura.Propriedades[wj].Identifier || estrutura_1N.Propriedades[lista_fk[wi]].ForeignKeyReference != lista_estrutura.Propriedades[wj].Identifier))
                while (wj < lista_estrutura.Propriedades.length && estrutura_1N.Propriedades[lista_fk[wi]].ForeignKeyReference != lista_estrutura.Propriedades[wj].Identifier)
                    wj++;

                if (wj < lista_estrutura.Propriedades.length)
                    lista_pos_principal.push(lista_estrutura.Propriedades[wj].Identifier);
            }

            /* Verificando se a variável 'lista_pos_principal' possui elemento repetidos */
            var novaArray = lista_pos_principal.filter(function (este, i) {
                return lista_pos_principal.indexOf(este) === i;
            });

            lista_pos_principal = novaArray;

            var w = 0;
            while (w < lista_pos_principal.length - 1) {
                temp_id = dado_principal[lista_pos_principal[w]];
                clausula_where += estrutura_1N.Propriedades[lista_fk[w]].Identifier + "=" + temp_id + '&'; //Erro do identificador
                w++;
            }
            temp_id = dado_principal[lista_pos_principal[w]];
            clausula_where += estrutura_1N.Propriedades[lista_fk[w]].Identifier + "=" + temp_id; ////Erro do identificador

            let order_bys = pegarOrderByDoHtml("1N");

            //se não encontrar order by no html, tenta procura-lo nas preferencias
            if (order_bys.length == 0) {
                let preferencia = await pesquisarPreferencia(lista_abas[index].class, "ORDERBY");
                order_bys = preferencia != null ? preferencia.PRE_VALOR : "";
            }

            var filtro_pesquisa = criarJsonPesquisa(clausula_where, estrutura_1N.Namespace, [], order_bys);
            dados_pesquisado_1N[posicao_dado] = eventoPesquisarNoBancoExpandir(filtro_pesquisa, 50, valor_ativo);
            //lista_paginas_1N[posicao_dado] = valor_ativo;

            gerarTela1N(estrutura_1N, dados_pesquisado_1N[posicao_dado], tab, str_id, index, lista_indices, posicao_dado, valor_ativo);
            marcarCheckboxDisplay1N(lista_indices, tab); //marca as checkboxes do dropdown de display utilizando a lista_indices.

            lista_indices_display1N = lista_indices;

            //após pesquisar e gerar a tela, é necessário colocar as informações de ordenação no HTML nvoamente
            let preferencia = await pesquisarPreferencia(lista_abas[index].class, 'ORDERBY');
            let pre_valor = preferencia != null ? preferencia.PRE_VALOR : "";
            carregarOrderByPreferenciasNoHtml(pre_valor, index, "1N");
        }

    }
}

//pesquisa nas preferencias se existe uma configuração de display salva
function pesquisarDisplay(namespace) {
    return new Promise(function (resolve, reject) {
        let pre_tipo = `DISPLAY`;

        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/AtualizarPreferenciaUsuario",
            data: {
                preTipo: pre_tipo, preNamespace: namespace
            },
            dataType: "json",
            traditional: true,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.preferenciaUsuario != null) {
                    resolve(result.preferenciaUsuario.PRE_VALOR);
                }
                else {
                    resolve([]);
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown, result) {
                var mensagem = `<strong>Erro ao tentar inserir preferência!</strong><br>ERRO ao tentar inserir preferência, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);

                reject();
            },
        });

    });
}

var tamanho_total_1N = 0;
function gerarTela1N(estrutura, dados, tab, str_id, index, lista_indices, posicao_dado, valor_ativo) {
    var str_html = "";

    var toolbar_buttons = elementoToolbar1N(estrutura, tab, str_id, index);
    var table_responsive = elementoTableResponsive1N(estrutura, dados, tab, str_id, index, lista_indices);
    var qtd_paginas = Math.ceil(tamanho_total_1N / 50); //Para fazer a paginação
    var ancora = '<a name="ancora{0}" id="ancora{0}"></a>'.replaceAll("{0}", tab);
    var table_paginas = '<div>' + criarPaginacao(valor_ativo, qtd_paginas, posicao_dado, '1N', tab, index); + '</div>';

    var str = ancora + toolbar_buttons + table_responsive + table_paginas;
    str = elementoDiv(str, 'table-wrapper');
    str = elementoDiv(str, 'col-xs-12');
    str_html = elementoDiv(str, 'row', 'conteudo' + tab);
    //str_html = elementoDiv(str, 'content-body');

    //Carregando.abrir('Processando ...');
    $('#menu' + str_id + tab + ' #conteudo' + tab).remove();
    $('#menu' + str_id + tab).append(str_html);

}

function elementoToolbar1N(estrutura, tab, str_id, index) {
    var str = "";
    var btn_focus = "";
    var btn_group_display = "";

    //Botão focus
    var temp = "";
    temp = elementoSpan('', 'glyphicon glyphicon-screenshot');
    temp = elementoButton(temp + 'Focus', 'btn btn-default', 'focus' + tab, '', '', '', 'button');
    btn_focus = elementoDiv(temp, 'btn-group focus-btn-group');

    //Grupo de display
    var novo1N = elementoButton('Incluir', 'btn btn-default', 'novo1N' + tab, 'onclick', 'eventoNovo1N({0}, {1}, {2})')
        .replace('{0}', tab)
        .replace('{1}', "'" + str_id + "'")
        .replace('{2}', index)
        ;
    //Display all
    var event_name = 'displayAll({0}, {1}, {2})'
        .replace('{0}', tab)
        .replace('{1}', "'" + str_id + "'")
        .replace('{2}', index)
    var display_all = elementoButton('Display all', 'btn btn-default', 'displayall' + tab, 'onclick', event_name);
    //Display
    var display_dropdown = elementoSpan('', 'caret');
    display_dropdown = elementoButton('Display' + display_dropdown, 'btn btn-default dropdown-toggle', '', '', '', '', '', '', '', 'dropdown');

    var lista_ul = elementoListaUl1N(estrutura, tab, str_id, index);

    btn_group_display = elementoDiv(novo1N + display_all + display_dropdown + lista_ul, 'btn-group dropdown-btn-group pull-right');

    str = btn_focus + btn_group_display;
    str = elementoDiv(str, 'btn-toolbar');

    return str;
}

function elementoListaUl1N(estrutura, tab, str_id, index) {
    //Lista - ul
    var lista_ul = "";
    var li = "";
    var temp_li = "";
    var aux_input = "";
    for (let i = 0; i < estrutura.Propriedades.length; i++) {
        //se tiver o atributo not mapped, irá pular para a próxima propriedade
        let not_mapped = false;
        estrutura.Propriedades[i].AnnotationsProp.forEach(function (currentValue) {
            if (currentValue.AttributeName == "NotMappedAttribute")
                not_mapped = true;
        });

        if (not_mapped)
            continue;

        var display = pegarDisplay(estrutura.Propriedades[i]);
        aux_input = elementoInputText('', '', 'toggle-playsis-' + tab + '-col' + i, '', 'checkbox', '', '', '', '', '', '', '', '', 'playsis-' + tab + '-col' + i, 'toggle-playsis-' + tab + '-col' + i);
        aux_label = elementoLabel(display, '', '', 'toggle-playsis-' + tab + '-col' + i);

        temp_li += `<li class="checkbox-row" onclick="eventoDisplay1N({2}, {3}, {4}, ${i});">{0}{1}</li>`
            .replace('{0}', aux_input)
            .replace('{1}', aux_label)
            .replace('{2}', tab)
            .replace('{3}', "'" + str_id + "'")
            .replace('{4}', index)
            .replace('{5}', i)
            ;
    }
    lista_ul = '<ul class="dropdown-menu">{0}</ul>'
        .replace('{0}', temp_li);

    return lista_ul;
}

function elementoListaUl(estrutura) {
    //Lista - ul
    var lista_ul = "";
    var li = "";
    var temp_li = "";
    var aux_input = "";
    for (let i = 0; i < estrutura.Propriedades.length; i++) {

        //se tiver o atributo not mapped, irá pular para a próxima propriedade
        let not_mapped = false;
        estrutura.Propriedades[i].AnnotationsProp.forEach(function (currentValue) {
            if (currentValue.AttributeName == "NotMappedAttribute")
                not_mapped = true;
        });

        if (not_mapped)
            continue;

        var display = pegarDisplay(estrutura.Propriedades[i]);
        aux_input = elementoInputText('', '', 'toggle-playsis-' + i, '', 'checkbox', '', '', '', '', '', '', '', '', '', 'toggle-playsis-col');
        aux_label = elementoLabel(display, '', '', '');

        temp_li += `<li class="checkbox-row" onclick="eventoDisplay(${i});">{0}{1}</li>`
            .replace('{0}', aux_input)
            .replace('{1}', aux_label)
            .replace('{5}', i)
            ;

    }
    lista_ul = '<ul class="dropdown-menu">{0}</ul>'
        .replace('{0}', temp_li);

    return lista_ul;
}

function elementoTableResponsive1N(estrutura, dados, tab, str_id, index, lista_indices) {
    var str = "";

    if (lista_indices.length == 0) {
        let index = 0;
        let adicionados = 0;
        while (adicionados < 5 && index < estrutura.Propriedades.length) {
            let prop = estrutura.Propriedades[index];

            //se a fk começar com DynamicForm quer dizer que é uma propriedade usada para mapeamento do FK
            //exemplo: public TipoVeiculo TipoVeiculo {get; set;} é uma propriedade usada apenas para mapear o atributo fk, e seu valor é sempre nulo.
            if (prop.ForeignKey != null || prop.ForeignKeyClass == null) {

                if (prop.Identifier != "PlayAction" && prop.Identifier != "PlayMsgErroValidacao" && prop.Identifier != "IndexClone") {
                    lista_indices.push(index);
                    adicionados++;
                }
            }

            index++;
        }
    }
    qtd_display = lista_indices.length + 1;

    var thead = elementoThead1N(estrutura, lista_indices, tab, index, str_id);
    var tbody = dados.length > 0 ? //Há dados no relacionamento 1N?
        elementoTbody1N(estrutura, dados, lista_indices, tab, str_id, index) : //Se sim, Gera corpo da tabela com os dados.
        '<tbody class="tbody1N"><tr><td colspan="{0}">Não há dados para essa tabela</td></tr></tbody>'
            .replace('{0}', (lista_indices.length + 1)); //Se não, gera corpo da tabela vazio.

    str = elementoTable1N(thead, tbody, 'table table-small-font table-bordered table-striped', 'table' + tab);
    str = elementoDiv(str, 'table-responsive');
    return str;
}

function elementoTable1N(thead = "", tbody = "", classe = "", id = "") {
    var str = '<table height=100 class="{2} table1N" id="{3}">{0}{1}</table>'
        .replace('{0}', thead)
        .replace('{1}', tbody)
        .replace('{2}', classe)
        .replace('{3}', id);

    return str;
}

function elementoThead1N(estrutura, lista_indices, index, tab, str_id) {
    var str = '<thead class="thead1N"><tr>';
    var th = "";
    for (let i = 0; i < lista_indices.length; i++) {
        var display = pegarDisplay(estrutura.Propriedades[lista_indices[i]]);

        // !! A FUNÇÃO moverColuna ESTÁ NO ARQUIVO mover-coluna-tabela.js !! \\
        th += `<th id="coluna1N${i}index${index}" class="th1N colunas1N${index}" value="${lista_indices[i]}">
        <i data-toggle="tooltip" title="Clique duas vezes para ordernar, clique uma vez para mudar o tipo de ordenação." class="orderby1N${index} fa fa-square-o" data-orderby="" data-tipoordem="1" data-ordemclicada="" data-identifier="${estrutura.Propriedades[lista_indices[i]].Identifier}" id="orderby1NColuna${lista_indices[i]}Index${index}" ondblclick="eventoOrderBy1N(this, ${tab}, ${index}, '${str_id}')" onclick="mudarTipoOrdenacao1N(this, ${tab}, ${index}, '${str_id}')"></i>
        <i class="fa fa-arrow-left movercoluna" onclick="moverColuna(${index}, ${i}, -1, 1)"></i>
        {0}
        <i class="fa fa-arrow-right movercoluna" onclick="moverColuna(${index}, ${i}, 1, 1)"></i>
        </th>`.replace('{0}', display);
    }
    //Adicionar o campo visualizar/editar/remover aqui;
    th += '<th class="th1N">Editar</th>';
    th += '<th class="th1N">Abrir nova guia</th>';

    str += th;
    str += '</tr></thead>';

    return str;
}

function pesquisarFKdo1N(id, valor, campo, classe) {
    if ($('#' + id).hasClass('flag_false')) {
        var i = 0;
        while (i < lista_estrutura_1N_secundarias.length && lista_estrutura_1N_secundarias[i].ClassName != classe)
            i++;

        if (i < lista_estrutura_1N_secundarias.length) {
            var lista_pk = pegarChavesPrimarias(lista_estrutura_1N_secundarias[i]);

            var text = lista_estrutura_1N_secundarias[i].Propriedades[lista_pk[0]].Identifier + '=' + valor; //Ver depois HK
            var filtros = criarJsonPesquisa(text, lista_estrutura_1N_secundarias[i].Namespace, []);

            var dados_pesquisado = eventoPesquisarNoBancoExpandir(filtros, 1);

            //Campos SEARCH e transformando tudo em minusculo.
            var lista_search = pegarCamposSearch(lista_estrutura_1N_secundarias[i]);
            for (let k = 0; k < lista_search.length; k++)
                lista_search[k] = lista_search[k].toLowerCase();

            //Lista de indices do dado retornado final
            var lista_indices = Object.getOwnPropertyNames(dados_pesquisado[0]);
            for (let k = 0; k < lista_indices.length; k++)
                lista_indices[k] = lista_indices[k].toLowerCase();

            //Montando o texto que vai ser substituido na tela.
            var text = "";
            if (lista_search.length == 0)
                text = dados_pesquisado[0][Object.getOwnPropertyNames(dados_pesquisado[0])[1]];
            else {

                var k = 0;
                while (k < lista_search.length - 1) {
                    var pos = lista_indices.indexOf(lista_search[k]);
                    text += dados_pesquisado[0][Object.getOwnPropertyNames(dados_pesquisado[0])[pos]] + '/ ';
                    k++;
                }
                var pos = lista_indices.indexOf(lista_search[k]);
                text += dados_pesquisado[0][Object.getOwnPropertyNames(dados_pesquisado[0])[pos]];
            }

            $('#' + id).text(text);
            $('#' + id).removeClass('flag_false');
            $('#' + id).addClass('flag_true');
        }
    }
    else {
        $('#' + id).removeClass('flag_true');
        $('#' + id).addClass('flag_false');
        $('#' + id).text($('#' + id).attr('data-value'));
    }
}

function elementoTbody1N(estrutura, dados, lista_indices, tab, str_id, index) {

    var pos_pai = Math.floor(tab / lista_abas.length);
    var str = '<tbody class="tbody1N">';
    var tr = "";
    var td = "";

    var lista_indices_dados_1N = Object.getOwnPropertyNames(dados[0]);
    for (let i = 0; i < lista_indices_dados_1N.length; i++) {
        lista_indices_dados_1N[i] = lista_indices_dados_1N[i].toLowerCase();
    }

    for (let i = 0; i < dados.length; i++) {
        td = "";
        for (let j = 0; j < lista_indices.length; j++) {
            var identifier = estrutura.Propriedades[lista_indices[j]].Identifier.toLowerCase();
            var pos = lista_indices_dados_1N.indexOf(identifier);

            if (estrutura.Propriedades[lista_indices[j]].ForeignKey != "ForeignKey") {
                var property = estrutura.Propriedades[lista_indices[j]];
                var propertyValue = dados[i][Object.getOwnPropertyNames(dados[i])[pos]];

                if (property.TypeProp === "DateTime") {

                    var meses = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];

                    var data = new Date(dados[i][Object.getOwnPropertyNames(dados[i])[pos]]);
                    var dia = data.getDate();
                    var mes = data.getMonth();
                    var ano = data.getFullYear();
                    var strData = dia + '/' + meses[mes] + '/' + ano + " ";

                    var hora = data.getHours();
                    var minutos = data.getMinutes();

                    strData += hora + ":" + minutos;

                    propertyValue = strData;
                }

                td += '<td align="center" class="td1N">{0}</td>'.replace('{0}', propertyValue);
            }
            else {
                var res = dados[i][Object.getOwnPropertyNames(dados[i])[pos]];
                var namespace = estrutura.Propriedades[lista_indices[j]].ForeignKeyNamespace;
                var referencia = estrutura.Propriedades[lista_indices[j]].ForeignKeyReference;
                td += '<td align="center" class="td1N"><a onclick="prepararAbrirNovaAba(null, \'' + namespace + '\', ' + -1 + ', \'' + referencia + '\', \'' + referencia + '\', true, \'' + res + '\')"  id="link{1}tr{2}1N{3}" class="flag_false" data-value="' + res + '" >' + res + '</a></td>'
                    .replace('{1}', j)
                    .replace('{2}', tab)
                    .replace('{3}', i)
                    .replace('{4}', "'" + estrutura.Propriedades[lista_indices[j]].Identifier + "'")
                    .replace('{5}', "'" + estrutura.Propriedades[lista_indices[j]].ForeignKeyClass + "'")
                    .replace('{6}', "'" + dados[i][Object.getOwnPropertyNames(dados[i])[pos]] + "'")
                    ;

                td = td.replace('{Z}', "'" + 'link' + j + 'tr' + tab + '1N' + i + "'");
            }

        }

        var temppk = pegarChavesPrimarias(estrutura);
        var temp_dados = [];
        var string_pesquisa = "";
        for (let k = 0; k < temppk.length; k++) {
            string_pesquisa += estrutura.Propriedades[temppk[k]].Identifier + '=' + dados[i][Object.getOwnPropertyNames(dados[i])[temppk[k]]] + '&';
        }
        string_pesquisa = string_pesquisa.substring(0, string_pesquisa.length - 1);

        td += `<td class="td1N" align="center"><i class="fa fa-edit circle" onclick="eventoExpandir1N({0}, {1}, {2}, {3}, {4}, {5}, {6});"></i>
        <td class="td1N" align="center"><i align="center" class="fa fa-plus-square-o" onclick="eventoAbrirNovaAba({7}, {8});"></i>`
            .replace('{0}', i)
            .replace('{1}', pos_pai)
            .replace('{2}', "'" + estrutura.Namespace + "'")
            .replace('{3}', lista_indices.length)
            .replace('{4}', tab)
            .replace('{5}', "'" + str_id + "'")
            .replace('{6}', index)
            .replace('{7}', "'" + estrutura.Namespace + "'")
            .replace('{8}', "'" + string_pesquisa + "'")//Deixar isso aqui dinâmico = tratar chaves compostas
            ;

        tr += '<tr class="tr{2}1N{1}">{0}</tr>'
            .replace('{0}', td)
            .replace('{1}', i)
            .replace('{2}', tab)
            ;
    }
    str += '{0}'.replace('{0}', tr);

    str += '</tbody>';
    return str;
}

function criarBotoesVoltar() {
    //percorre cada linha da tabela e adiciona o botao
    var trs = $("[id^=more");
    trs.each(function (index) {

        //adiciona um icon com acao de retornarDado onclick passando o numero da linha atual
        $(`#more${index} > td > div`).append(`<div class="col-md-6">
            <i class="fa fa-arrow-circle-left circle btnVoltar" onclick="retornarDado(${index})" style=""></i>
        </div>`);
    });
}

async function eventoPesquisarNovaAba(id = "", foreignKeyReference = null, criar_botoes_voltar = null, expandir = true) {
    selecionados = [];
    pegarColunasSelecionadas();

    eventoPesquisarPrincipal(id, false).then(function (result) {
        //se tiver um id especifico a procurar, expande o resultado
        if (id != "" && expandir) {
            eventoExpandir(0, selecionados.length);
        }

        //se tiver um win_pai, veio de uma tela de cadastro
        if (win_pai != null && foreignKeyReference != null && criar_botoes_voltar != null) {
            criarBotoesVoltar();
        }


        return new Promise(function (resolve, reject) {
            resolve();
        });
    }).catch(function (error) {
        return new Promise(function (resolve, reject) {
            reject(error);
        });
    });
}


function eventoAbrirNovaAba(namespace, ids = "", foreignKeyReference = null, criar_botoes_voltar = null, identifier = null) { //flag define se deve mostrar ou nao o botao de retornar na nova aba

    //abre a nova janela no url baseado no namespace da classe
    var win = window.open('/DynamicWeb/DynamicForm?tipo=CLASSE&url=/DynamicWeb/GetClassForm?nome_classe=' + namespace);
    win.addEventListener('DOMContentLoaded', function () {

        //espera meio segundo para evitar bug no jquery
        setTimeout(function () {

            //se o campo clicado for diferente de nulo, quer dizer que veio de uma tela de cadastro
            if (identifier != null && foreignKeyReference != "") {
                win.win_pai = window.self; //define a janela pai como esta
                //win.document.body.innerHTML += 
                //win.document.body.innerHTML += `<input type='hidden' id='hdnForeignKeyReference' value=${foreignKeyReference}\>`;
                win.document.getElementById('logo-marca-link').innerHTML += `<input type='hidden' id='hdnForeignKeyReference' value=${foreignKeyReference}\>`;
                win.document.getElementById('logo-marca-link').innerHTML += `<input type='hidden' id='hdnIdentifier' value=${identifier}\>`;
            }

            //so faz a pesquisa se tiver um id
            if (ids !== "") {
                win.eventoPesquisarNovaAba(ids, foreignKeyReference, criar_botoes_voltar, false);
            }
            else {
                //foca o campo de pesquisa
                win.document.getElementsByClassName("campo_pesquisa")[0].focus();
            }
        }, 500);

    });
}

function prepararAbrirNovaAba(event, namespace, pos_estrutura, foreignKeyReference, identifier, criar_botoes_voltar, value = "") {

    //se o método vier de um evento (onclick por exemplo), é preciso recuperar o valor do campo clicado para montar a pesquisa
    if (event != null) {
        
        //pega o primeiro elemento parente, e vai para o proximo parente enquanto nao encontrar um input
        var currentChild = event.currentTarget.parentNode.firstChild;

        while (currentChild.localName != 'input') {
            currentChild = currentChild.nextSibiling;
        }

        //se o campo estiver desabilitado, aborta o método
        if($(currentChild).prop('disabled'))
            return;

        // if (currentChild.className.toLowerCase().indexOf("lock") !== -1)
        //     return;

        var value = currentChild.value;
    }

    //se tiver algum valor no campo, monta a string de pesquisa e a passa por parametro, se nao, deixa o segundo parametro vazio
    if (value != "") {
        var string_pesquisa = "";

        if (pos_estrutura > -1) {
            //Pega todas as chaves primarias da classe estrangeira em questão.
            var estrutura = lista_aux_secundaria[pos_estrutura];
            var temppk = pegarChavesPrimarias(estrutura);
            for (let k = 0; k < temppk.length; k++) {
                string_pesquisa += estrutura.Propriedades[temppk[k]].Identifier + '%' + value + '|';
            }
            string_pesquisa = string_pesquisa.substring(0, string_pesquisa.length - 1);
        }
        else //Caso seja uma classe que não veio na lista de classes estrangeiras.
            string_pesquisa = foreignKeyReference + '%' + value;


        eventoAbrirNovaAba(namespace, string_pesquisa, foreignKeyReference, criar_botoes_voltar, identifier);
    }
    else {
        eventoAbrirNovaAba(namespace, "", foreignKeyReference, criar_botoes_voltar, identifier);
    }

}

//retorna para a janela pai a estrutura clicada
function retornarDado(pos_lista) {
    var identifier = $("#hdnIdentifier").val();
    var foreignKeyReference = $("#hdnForeignKeyReference").val();
    win_pai.pegarDadoRetornado(lista_dados[pos_lista], foreignKeyReference, identifier);
    window.close();
}

//recebe a estrutura clicada na aba filha
function pegarDadoRetornado(estrutura, foreignKeyReference, identifier) {
    var id = estrutura[`${foreignKeyReference}`];

    //pegar o input clicado e preencher com o id
    $(`[data-column='${identifier}']`).val(id);
}

//Tratar o PAGODE de ir e voltar entre os dados
function eventoExpandir1N(index, pos_pai, namespace, colspan, tab, str_id_pai, index_tab) {

    var k = 0;
    while (k < lista_estrutura_1N.length && namespace != lista_estrutura_1N[k].Namespace)
        k++;

    if (k < lista_estrutura_1N.length) {
        estrutura_1N = lista_estrutura_1N[k];

        var lista_pk = pegarChavesPrimarias(estrutura_1N);

        var lista_indices_dados_1N = Object.getOwnPropertyNames(dados_pesquisado_1N[pos_pai][index]);
        for (let i = 0; i < lista_indices_dados_1N.length; i++)
            lista_indices_dados_1N[i] = lista_indices_dados_1N[i].toLowerCase();

        var lista_indices_estrutura_1N = [];
        for (let i = 0; i < estrutura_1N.Propriedades.length; i++)
            lista_indices_estrutura_1N.push(estrutura_1N.Propriedades[i].Identifier.toLowerCase()); //OK

        var dado;
        var temp_pos;
        var pesquisa = "";

        var attrib_id = 'trX{0}1N{1}'
            .replace("{0}", tab)
            .replace("{1}", index);

        //Contrair e remover os dados
        if ($('.tr' + tab + '1N' + index).next().attr('class') === attrib_id) {
            $('.tr' + tab + '1N' + index).next('tr').remove();

            temp_pos = lista_indices_estrutura_1N[lista_pk[0]];
            temp_pos = lista_indices_dados_1N.indexOf(temp_pos);
            dado = dados_pesquisado_1N[pos_pai][index][Object.getOwnPropertyNames(dados_pesquisado_1N[pos_pai][index])[temp_pos]];

            var i = 0;
            while (i < dados_completos_1N.length && dados_completos_1N[i][0][Object.getOwnPropertyNames(dados_completos_1N[i][0])[temp_pos]] != dado)
                i++;

            if (i < dados_completos_1N.length)
                dados_completos_1N.splice(i, 1);

        } else { //Expandir e exibir os dados
            var i = 0;
            while (i < lista_pk.length - 1) {
                temp_pos = lista_indices_estrutura_1N[lista_pk[i]];
                temp_pos = lista_indices_dados_1N.indexOf(temp_pos);
                dado = dados_pesquisado_1N[pos_pai][index][Object.getOwnPropertyNames(dados_pesquisado_1N[pos_pai][index])[temp_pos]];
                pesquisa += estrutura_1N.Propriedades[lista_pk[i]].Identifier + '=' + dado + '&'; //OK
                i++;
            }
            temp_pos = lista_indices_estrutura_1N[lista_pk[i]];
            temp_pos = lista_indices_dados_1N.indexOf(temp_pos);
            dado = dados_pesquisado_1N[pos_pai][index][Object.getOwnPropertyNames(dados_pesquisado_1N[pos_pai][index])[temp_pos]];
            pesquisa += estrutura_1N.Propriedades[lista_pk[i]].Identifier + '=' + dado; //OK

            var filtros = criarJsonPesquisa(pesquisa, namespace, []);
            var dados_busca = eventoPesquisarNoBancoExpandir(filtros, 1);
            dados_completos_1N.push(dados_busca);
            var pos_dados_completos = dados_completos_1N.length - 1;

            lista_indices_dados_pesquisados_1N = Object.getOwnPropertyNames(dados_busca[0]);
            for (let i = 0; i < lista_indices_dados_pesquisados_1N.length; i++)
                lista_indices_dados_pesquisados_1N[i] = lista_indices_dados_pesquisados_1N[i].toLowerCase();


            var str = '<tr class="trX{0}1N{1}"><td colspan="{2}">'
                .replace("{0}", tab)
                .replace("{1}", index)
                .replace("{2}", ++colspan)
                ;

            var lista_abas_1N = [];
            lista_abas_1N = pegarAbas(estrutura_1N, lista_abas_1N);

            var str_id = 'P' + tab + 'I' + index + 'C';// Exemplo: .inputLOCKP1I1C23, onde P=Página, I=Index do dado e C=Campo
            str += gerarFormulario(dados_busca, 0, estrutura_1N, lista_estrutura_1N_secundarias, lista_abas_1N, lista_indices_dados_pesquisados_1N, str_id, k);
            str += gerarBotoesFormulario1N(index, k, pos_dados_completos, tab, str_id_pai, index_tab, pos_pai);

            str += '</div></td></tr>';

            $('.tr' + tab + '1N' + index).after(str);

            //depois de adicionar os campos, deve aplicar as mascaras pendentes
            aplicarMascaras();
            adicionarEventosRightClick();
            eventoAlterar1N(tab, index, k);
        }
    }
}

function configuraTipoCampo(prop, dado, id_campo, flag, lista_estrutura_secundarias, str_id, pos_lista, index_estrutura = null) {
    id_campo = str_id + id_campo;
    if (dado == null)
        dado = "";

    var tipo = prop.TypeProp.toLowerCase();
    var str_html, val;

    var pk = prop.PrimaryKey; //É primary key?

    if (prop.ForeignKey === "ForeignKey") {
        val = prop.AlternativeForeignKeyClass != null ? prop.AlternativeForeignKeyClass.split(" ")[0] : prop.ForeignKeyClass.split(" ")[0];

        var i = 0;
        while (i < lista_estrutura_secundarias.length && val.toLowerCase() != lista_estrutura_secundarias[i].ClassName.toLowerCase())
            i++;

        if (i < lista_estrutura_secundarias.length) {
            var classe_secundaria = lista_estrutura_secundarias[i];
            str_html = validarAnotacoes11(prop.AnnotationsProp, classe_secundaria, dado, id_campo, i, flag, pk, pos_lista, prop, index_estrutura);
            return str_html;
        }
    }
    else if (prop.ForeignKey === "1F") {
        console.log('1F');
    }
    else if (prop.ForeignKey === "1N") {
        //Remover isso depois
    }
    else {
        //console.log('Normal');

        if (tipo === "string") { //Tipo de dado string
            return validarAnotacoesGeral(prop.AnnotationsProp, prop.Identifier, "text", dado, id_campo, flag, pk);
        } else if (tipo.includes("double") || tipo.includes("float") || tipo.includes("single") || tipo.includes("int32") || tipo.includes("decimal")) { //Tipo de dado numérico
            return validarAnotacoesGeral(prop.AnnotationsProp, prop.Identifier, "number", dado, id_campo, flag, pk);
        } else if (tipo.includes("char")) { //Tipo de dado caractere
            return validarAnotacoesGeral(prop.AnnotationsProp, prop.Identifier, "text", dado, id_campo, flag, pk);
        } else if (tipo === "datetime") {
            return validarAnotacoesGeral(prop.AnnotationsProp, prop.Identifier, "datetime-local", dado, id_campo, flag, pk);
        }
        else {
            //tipo = (Foreign Key);
        }
    }

    return "";
}

function pegarEstrutura1N(nome_classe) {

    $.ajax({
        type: 'POST',
        url: funcao_pesquisa,
        data: { nome_classe: nome_classe },
        dataType: 'json',
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            //console.log('result 1N');
            //console.log(result);
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao pegar estrutura!</strong><br>ERRO ao tentar pegar estrutura, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        }
    })
}

//Validação das anotações
function validarAnotacoesGeral(anot, identifier, type, dado, id_campo, flag, pk) {
    var str_campo = "", _aux;
    var lock = false; //Variável para validar se o campo é somente leitura ou leitura/escrita.
    var display = "", required = "", email, maxLength = "", min_range = "", max_range = "", aux_div;
    var lista_cb_value = [];
    var lista_cb_display = [];
    var lista_cb_disabled = [];
    var lista_checkbox_value_checked = [];
    var lista_checkbox_display = [];
    var disabled;
    var edit = false;
    var flag_textArea = false;
    display = identifier;

    if (anot[0] != null) {
        for (let index = 0; index < anot.length; index++) {
            _aux = anot[index].AttributeName.toLowerCase();
            if (_aux.includes("display")) //Nome que vai aparecer no placeholder
            {
                display = anot[index].Parametros[0].Value;
            }
            else if (_aux.includes("edit")) {
                edit = true;
            }
            else if (_aux.includes("password")) {
                type = "password";
            }
            else if (_aux.includes("required")) { //Campo obrigatório
                required = "required";
            }
            else if (_aux.includes("maxlength")) { //Quantidade de caracteres que podem ser digitados.
                maxLength = "999999999";
            }
            else if (_aux.includes("range")) { //Somente para valores numéricos
                required = "required";
                minRange = "-9999";
                maxRange = "9999";
            }
            else if (_aux.includes("textarea")) {
                flag_textArea = true;
            }
            else if (_aux.includes("combobox")) {
                //se o primeiro parametro for descrição, addiciona ele no lista_cb_display
                if (anot[index].Parametros[0].Name == 'Description') {
                    lista_cb_display.push(anot[index].Parametros[0].Value.toUpperCase());
                    lista_cb_value.push(anot[index].Parametros[1].Value.toUpperCase());
                }
                else { //if(anot[index].Parametros[0].Name == 'Value'){
                    lista_cb_value.push(anot[index].Parametros[0].Value.toUpperCase());
                    lista_cb_display.push(anot[index].Parametros[1].Value.toUpperCase());
                }

                if (anot[index].Parametros.length > 2)
                    lista_cb_disabled.push(1);
                else
                    lista_cb_disabled.push(0);

            }
            else if (_aux.includes("checkbox")) {

                //se o primeiro parametro for descrição, addiciona ele no lista_cb_display
                if (anot[index].Parametros[0].Name == 'Description') {
                    let display = anot[index].Parametros[0].Value.toUpperCase();
                    lista_checkbox_display.push(display);

                    let value = anot[index].Parametros[1].Value.toUpperCase();
                    lista_checkbox_value_checked.push(value);
                }
                else {
                    let display = anot[index].Parametros[1].Value.toUpperCase();
                    lista_checkbox_display.push(display);

                    let value = anot[index].Parametros[0].Value.toUpperCase();
                    lista_checkbox_value_checked.push(value);
                }

            }
            else if (_aux.includes("notmappedattribute") || _aux.includes("hidden")) {
                return "";
            }
            else if (_aux.includes("read") || (pk && flag == 'read')) {
                id_campo += "";
                if (!id_campo.includes("LOCK"))
                    id_campo = 'LOCK' + id_campo;
                lock = true;
            }
            else if (_aux.includes("mask")) {
                //coloca o campo target e o valor da mascara para esse campo em um array global. 
                //!! DEPOIS DE EFETIVAMENTE INSERIR OS CAMPOS NA PAGINA, DEVE-SE UTILIZAR ESSE ARRAY PARA APLICAR AS MASCARAS !!\\
                let obj = { _target: id_campo, _mask: anot[index].Parametros[0].Value };
                masks.push(obj);
            }

        }
    }
    //dado = dado.toUpperCase();
    disabled = flag === "insert" && !lock ? "" : "disabled";

    str_campo = elementoParagrafo(display, "col-sm-12 col-md-12", "", identifier);
    var pos_busca = 1;
    if (edit)
        disabled = "";
    //console.log(addInputText(value, "form-control input" + pos_busca, identifier, display, type, required, maxLength, min_range, max_range, display, "disabled"));
    if (lista_cb_value.length != 0) {
        str_campo += elementoCombobox(lista_cb_value, "menu-right-click form-control col-sm-6 col-md-6 input" + id_campo, identifier, disabled, display, "", "", lista_cb_display, dado, lista_cb_disabled, true, '', ['data-column'], [`${identifier}`]);
        aux_div = elementoDiv(str_campo, "col-sm-4 col-md-4 col-xs-12 padding ");
    }
    else if (lista_checkbox_value_checked.length != 0) {
        let label = str_campo;

        //adiciona todos os checkboxes da coluna
        //atualmente o sistema suporta apenas 1 checkbox, mas, vou deixar o forEach para caso seja preciso implementar esse feature
        lista_checkbox_value_checked.forEach(function (currentValue, index) {
            let checkbox = elementoCheckbox(currentValue, "menu-right-click col-sm-1 input" + id_campo, `checkbox_${id_campo}`, lista_checkbox_display[index], `name_${currentValue}_${index}`, disabled, dado, ['data-column'], [`${identifier}`]); //cria o checkbox
            let checkboxDescription = elementoParagrafo(lista_checkbox_display[index], "col-sm-6", '', '', 'overflow:hidden;');
            label += elementoLabel(checkbox + checkboxDescription, 'col-sm-12', '');
        });

        //envolve o checkbox num form-check com col-sm-1
        aux_div = elementoDiv(label, "form-check col-sm-4 col-md-4 col-xs-4 padding");
    }
    else if (flag_textArea == true) {
        str_campo += elementoTextArea(dado, "menu-right-click form-control col-12 input" + id_campo, identifier, required, disabled, 4, ['data-column'], [`${identifier}`]);
        aux_div = elementoDiv(str_campo, " col-sm-12 col-md-12 col-lg-12 col-xs-12 padding");
    }
    else {
        str_campo += elementoInputText(dado, "menu-right-click form-control input" + id_campo, identifier, display, type, required, maxLength, min_range, max_range, display, disabled, '', '', '', '', ['data-column'], [`${identifier}`]);
        aux_div = elementoDiv(str_campo, " col-sm-4 col-md-4 col-xs-12 padding");
    }


    return aux_div;
}

function validarAnotacoes11(anot, classe, dado, id_campo, pos_tabela, flag, pk, pos_lista, prop, index_estrutura = null) {
    var lista = [];
    var str_campo = "", _aux;
    var lock = false; //Variável para validar se o campo é somente leitura ou leitura/escrita.
    var display = "", required = "", email, password, maxLength = "", min_range = "", max_range = "";
    var lista_cb_value = [];
    var lista_cb_display = [];
    var disabled;
    var edit = false;
    var aux_nome = classe.ClassName.split(".");
    aux_nome = aux_nome[aux_nome.length - 1];
    display = aux_nome;

    if (anot[0] != null) {
        for (let index = 0; index < anot.length; index++) {
            _aux = anot[index].AttributeName.toLowerCase();
            if (_aux.includes("display")) //Nome que vai aparecer no placeholder
            {
                display = anot[index].Parametros[0].Value;
            }
            else if (_aux.includes("required")) { //Campo obrigatório
                required = "required";
            }
            else if (_aux.includes("edit")) {
                edit = true;
            }
            else if (_aux.includes("maxlength")) { //Quantidade de caracteres que podem ser digitados.
                maxLength = "999999999";
            }
            else if (_aux.includes("range")) { //Somente para valores numéricos
                required = "required";
                minRange = "-9999";
                maxRange = "9999";
            }
            else if (_aux.includes("combobox")) {
                if (anot[index].Parametros[0].Name == 'Description') {
                    lista_cb_display.push(anot[index].Parametros[0].Value.toUpperCase());
                    lista_cb_value.push(anot[index].Parametros[1].Value.toUpperCase());
                }
                else { //if(anot[index].Parametros[0].Name == 'Value'){
                    lista_cb_value.push(anot[index].Parametros[0].Value.toUpperCase());
                    lista_cb_display.push(anot[index].Parametros[1].Value.toUpperCase());
                }
            }
            else if (_aux.includes("notmappedattribute") || _aux.includes("hidden")) {
                return "";
            }
            else if (_aux.includes("read") || (pk && flag == 'read')) {
                id_campo += "";
                if (!id_campo.includes("LOCK"))
                    id_campo = 'LOCK' + id_campo;
                lock = true;
            }
        }
    }
    //dado = dado.toUpperCase();

    disabled = flag === "insert" && !lock ? "" : "disabled";
    str_campo = elementoParagrafo(display, "", "");

    if (edit)
        disabled = "";
    var str_campo = elementoParagrafo(display, "col-sm-12 col-md-12", "");
    str_campo += elementoComboboxEditavel(lista, dado, 'menu-right-click form-control input' + id_campo, 'datalist' + id_campo, disabled, display, id_campo, pos_tabela, pos_lista, classe, prop, ['data-pos'], [pos_lista], index_estrutura);

    str_campo = elementoDiv(str_campo, "col-sm-4 col-md-4 col-xs-12 padding");
    return str_campo;
}

function evtKeyPress(event, id_campo, pos_tabela) {
    var tecla = event.which || event.keyCode;
    if (tecla === 32) {
        evtKeyPressBanco(id_campo, pos_tabela, 1);
    }
}

function evtKeyPressBanco(id_campo, pos_tabela, key_or_button, index_estrutura = null) {
    var texto = "";
    if ($('.input' + id_campo).val() != "")
        texto = montarText($('.input' + id_campo).val(), lista_aux_secundaria[pos_tabela]);

    var aux_chaves_primarias = pegarChavesPrimarias(lista_aux_secundaria[pos_tabela]);
    var lista_colunas = [];
    for (let i = 0; i < aux_chaves_primarias.length; i++)
        lista_colunas.push(lista_aux_secundaria[pos_tabela].Propriedades[aux_chaves_primarias[i]].Identifier);

    for (let i = 0; i < lista_aux_secundaria[pos_tabela].Propriedades.length; i++) {
        var j = 0;
        while (j < lista_aux_secundaria[pos_tabela].Propriedades[i].AnnotationsProp.length && lista_aux_secundaria[pos_tabela].Propriedades[i].AnnotationsProp[j].AttributeName != 'SEARCH')
            j++;

        if (j < lista_aux_secundaria[pos_tabela].Propriedades[i].AnnotationsProp.length)
            lista_colunas.push(lista_aux_secundaria[pos_tabela].Propriedades[i].Identifier);

    }




    var query = criarJsonPesquisa(texto, lista_aux_secundaria[pos_tabela].Namespace, lista_colunas, "", lista_aux_secundaria[pos_tabela]);
    Carregando.abrir('Processando ...');
    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/Pesquisar",
        data: { query_json: query, page_size: tamanho_pagina, index: 1, tipoPesquisa: 1 },
        dataType: "json",
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                if (result.objects[0].length > 0) {
                    $('#datalist' + id_campo + ' datalist option').remove();

                    var lista_indices = Object.getOwnPropertyNames(result.objects[0][0]);
                    for (let i = 0; i < lista_indices.length; i++)
                        lista_indices[i] = lista_indices[i].toLowerCase();
                    for (let i = 0; i < lista_colunas.length; i++)
                        lista_colunas[i] = lista_colunas[i].toLowerCase();

                    //Pego o numero que veio na ultima classe para ver qual a posicao na lista_estrutura que pertence a este input: Ex: pos_5 = 5.
                    var pos_lista_estrutura = parseInt($('.input' + id_campo).attr('data-pos'));
                    var dado_pos;

                    if (index_estrutura != null)
                        dado_pos = lista_estrutura_1N[index_estrutura].Propriedades[pos_lista_estrutura].Identifier.toLowerCase();
                    else
                        dado_pos = lista_estrutura.Propriedades[pos_lista_estrutura].Identifier.toLowerCase();

                    var colunas_res = [];
                    if (lista_colunas.indexOf(dado_pos) != -1) {
                        colunas_res.push(dado_pos);
                        for (let i = 0; i < lista_colunas.length; i++) {
                            if (lista_colunas[i] != dado_pos)
                                colunas_res.push(lista_colunas[i]);
                        }
                    }
                    else
                        colunas_res = lista_colunas;


                    var pos_id = lista_indices.indexOf(colunas_res[0]);
                    var pos_desc = colunas_res.length > 1 ? lista_indices.indexOf(colunas_res[1]) : pos_id; //Pegar por index;

                    var id = 0;
                    var str = "";
                    var desc = "";
                    for (let i = 0; i < result.objects[0].length; i++) {
                        id = result.objects[0][i][Object.getOwnPropertyNames(result.objects[0][i])[pos_id]];
                        desc = result.objects[0][i][Object.getOwnPropertyNames(result.objects[0][i])[pos_desc]];

                        str += '<option value="{0}">{1}</option>'
                            .replace("{0}", id)
                            .replace("{1}", desc);
                    }
                }

                if (key_or_button == 1) {
                    $.when(
                        $('#datalist' + id_campo + ' input').val($('#datalist' + id_campo + ' input').val().substring(0, $('#datalist' + id_campo + ' input').val().length - 1))
                    ).done(
                        $('#datalist' + id_campo + ' datalist').append(str)
                    )
                }
                else { //Veio do button;
                    $('#datalist' + id_campo + ' datalist').append(str);
                    $('.input' + id_campo).focus();
                }

            } else {//entrou no try catch do controler 
                mostraDialogo(result.st, 'danger', 3000);
                $("#insert0").append(result.st);
            }

        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao pesquisar dados!</strong><br>ERRO ao tentar pesquisar os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        },
        complete: function () {
            Carregando.fechar();
        }
    });

}

function montarText(input, classe) {
    var lista_indices_pk = pegarChavesPrimarias(classe);
    for (let i = 0; i < classe.Propriedades.length; i++) {
        var j = 0;
        while (j < classe.Propriedades[i].AnnotationsProp.length && classe.Propriedades[i].AnnotationsProp[j].AttributeName != 'SEARCH')
            j++;

        if (j < classe.Propriedades[i].AnnotationsProp.length)
            lista_indices_pk.push(i);

    }

    var res = "";
    i = 0;
    while (i < lista_indices_pk.length - 1) {
        res += '{0}%{1}|'
            .replace("{0}", classe.Propriedades[lista_indices_pk[i]].Identifier)
            .replace("{1}", input)
            ;
        i++;
    }
    res += '{0}%{1}'
        .replace("{0}", classe.Propriedades[lista_indices_pk[i]].Identifier)
        .replace("{1}", input)
        ;

    return res;
}
function gerarBotoesFormularioAPS(index, campo_id) {
    var _aux = "form" + index;
    var botoes = '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Salvar", "btn btn-success", "salvar" + index, "onclick", "eventoSalvarIndividualAPS(" + index + ",'" + campo_id + "')", _aux, "submit", "", "", "", "", "", "disabled");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Alterar", "btn btn-warning", "alter" + index, "onclick", "eventoAlterarIndividualAPS({0})", _aux, "submit")
        .replace("{0}", "'" + index + "'");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Excluir", "btn btn-danger", "excluir" + index, "onclick", "eventoExcluirIndividualAPS(" + index + ")", _aux, "submit");
    botoes += '</div>';


    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Cancelar", "btn btn-secondary", "cancelar" + index, "onclick", "eventoCancelarIndividualAPS(" + index + ", 1)", _aux, "submit");
    botoes += '</div>';

    //Ações relacionadas.
    var lista_metodos = pegarListaMetodosEstrutura(lista_estrutura);
    if (lista_metodos.length > 0) {
        botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
        botoes += elementoComboboxArea(lista_metodos, "", "acoes" + index, "", "", "", "", false, index);
        botoes += '</div>';
    }

    var div = elementoDiv(botoes, "panel-body", "update" + index);
    return div;
}

function gerarBotoesFormulario(index) {
    var _aux = "form" + index;
    var botoes = '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Salvar", "btn btn-success", "salvar" + index, "onclick", "eventoSalvarIndividual(" + index + ")", _aux, "submit", "", "", "", "", "", "disabled");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Alterar", "btn btn-warning", "alter" + index, "onclick", "eventoAlterarIndividual(" + index + ")", _aux, "submit");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Excluir", "btn btn-danger", "excluir" + index, "onclick", "eventoExcluirIndividual(" + index + ", " + 1 + ")", _aux, "submit", "", "", "", "");
    botoes += '</div>';


    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Cancelar", "btn btn-secondary", "cancelar" + index, "onclick", "eventoCancelarIndividual(" + index + ", 1)", _aux, "submit", "", "", "", "", "", "disabled");
    botoes += '</div>';

    //Ações relacionadas.
    var lista_metodos = pegarListaMetodosEstrutura(lista_estrutura);
    if (lista_metodos.length > 0) {
        botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
        botoes += elementoComboboxArea(lista_metodos, "", "acoes" + index, "", "", "", "", false, index);
        botoes += '</div>';
    }

    var div = elementoDiv(botoes, "panel-body", "update" + index);
    return div;
}

function gerarBotoesFormulario1N(index, pos_estrutura, pos_dados_completos, pagina, str_id, index_tab, pos_pai) {
    var id = 'P' + pagina + 'I' + index;
    var _aux = "form" + index;
    var botoes = '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Salvar", "btn btn-success", "salvar1N" + id, "onclick", "eventoSalvar1N({0}, {1}, {2}, {3}, {4}, {5})", _aux, "submit", "", "", "", "", "", "disabled")
        .replace('{0}', index)
        .replace('{1}', pos_estrutura)
        .replace('{2}', pos_dados_completos)
        .replace('{3}', pagina)
        .replace('{4}', "'" + str_id + "'")
        .replace('{5}', index_tab)
        ;
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Alterar", "btn btn-warning", "alter1N" + id, "onclick", "eventoAlterar1N(" + pagina + ',' + index + ',' + pos_estrutura + ")", _aux, "submit");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Excluir", "btn btn-danger", "excluir1N" + id, "onclick", "eventoExcluir1N({0}, {1}, {2}, {3}, {4}, {5})", _aux, "submit", "", "", "", "")
        .replace('{0}', index)
        .replace('{1}', pos_estrutura)
        .replace('{2}', pos_dados_completos)
        .replace('{3}', pagina)
        .replace('{4}', "'" + str_id + "'")
        .replace('{5}', index_tab)
        ;
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Cancelar", "btn btn-secondary", "cancelar1N" + id, "onclick", "eventoCancelar1N(" + pagina + ',' + index + ", 1," + pos_estrutura + ',' + pos_dados_completos + ")", _aux, "submit", "", "", "", "", "", "disabled");
    botoes += '</div>';

    //Ações relacionadas.
    var lista_metodos = pegarListaMetodosEstrutura(lista_estrutura_1N[pos_estrutura]);
    if (lista_metodos.length > 0) {
        botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
        botoes += elementoComboboxArea(lista_metodos, "", "acoes1N" + index, "", "", "", "", false, index, 1, pos_pai, lista_estrutura_1N[pos_estrutura].Namespace, "position: relative;");
        botoes += '</div>';
    }

    var div = elementoDiv(botoes, "panel-body", "update" + id);
    return div;
}

function eventoSalvarIndividualAPS(index, campo_id) {
    //INICIO PREVISTO
    var pos = pegarIndexInicioPrevistoAPS(index);
    var inicio_previsto_antes = lista_dados[index][Object.getOwnPropertyNames(lista_dados[index])[pos]];
    if (eventoSalvarIndividual(0)) {
        if (indicator == -1) {
            var inicio_previsto_depois = lista_dados[index][Object.getOwnPropertyNames(lista_dados[index])[pos]];
            //A função modal show está no arquivo: modal-alterar-dados-carga.js
            var tipo = 1; //Tipo representa como a div com o caminhaozinho deve ser atualizada.
            if (inicio_previsto_antes != inicio_previsto_depois)
                tipo = 2;
            atualizarCaminhao(inicio_previsto_depois, campo_id, tipo);
        }
        else {
            atualizarTabelaCargaMaquina();
        }

        $('#modalGlobalDynamic').modal('hide');
    }


    //Atualizar os caminhazinhos;
}

function pegarIndexInicioPrevistoAPS(index) {
    var lista_names = Object.getOwnPropertyNames(lista_dados[index]);
    for (let i = 0; i < lista_names.length; i++)
        lista_names[i] = lista_names[i].toLowerCase();

    return lista_names.indexOf("car_data_inicio_previsto");
}

function eventoAlterarIndividualAPS(index) {
    eventoAlterarIndividual(0);
}

function eventoExcluirIndividualAPS(index) {
    var pos = pegarIndexInicioPrevistoAPS(index);
    var inicio_previsto_antes = lista_dados[index][Object.getOwnPropertyNames(lista_dados[index])[pos]];
    if (eventoExcluirIndividual(0, 0)) {
        //A função modal show está no arquivo: modal-alterar-dados-carga.js
        $('#modalGlobalDynamic').modal('hide');
        if (indicator == -1) {
            atualizarCaminhao(inicio_previsto_antes, global_modal_id, 2);
        }
        else if (indicator == 1) {
            atualizarTabelaCargaMaquina();
        }
        else {

        }
    }
}

function eventoCancelarIndividualAPS(index) {
    var pos = pegarIndexInicioPrevistoAPS(index);
    var inicio_previsto_antes = lista_dados[index][Object.getOwnPropertyNames(lista_dados[index])[pos]];
    //A função modal show está no arquivo: modal-alterar-dados-carga.js
    $('#modalGlobalDynamic').modal('hide');
    //Atualizar os caminhazinhos;
    if (indicator == -1) {
        atualizarCaminhao(inicio_previsto_antes, global_modal_id, 1);
    }
    else if (indicator == 1) {
        //atualizarTabelaCargaMaquina();
    }
    else {

    }
}

function eventoAlterar1N(pagina, index, pos_estrutura) {
    var estrutura_1N = lista_estrutura_1N[pos_estrutura];
    var str_id = "";

    for (let i = 0; i < estrutura_1N.Propriedades.length; i++) {
        str_id = ".inputP" + pagina + 'I' + index + 'C' + i;
        if (!(estrutura_1N.Propriedades[i].ForeignKey == "ForeignKey" && estrutura_1N.Propriedades[i].ForeignKeyClass == lista_estrutura.ClassName))
            $(str_id).prop('disabled', false);
    }
    $(".inputP" + pagina + 'I' + index + 'C' + 1).focus();

    str_id = '1NP' + pagina + 'I' + index;
    habilitarBotoes(str_id, false, true, true, false);
}

function eventoExcluir1N(index, pos_estrutura, pos_dados_completos, tab, str_id, index_tab) {
    var res = confirm('Deseja excluir este item?');
    if (res == true) {
        var estrutura_1N = lista_estrutura_1N[pos_estrutura];
        dados_completos_1N[pos_dados_completos][0] = adicionarPlayAction(dados_completos_1N[pos_dados_completos][0], 'delete');
        if (excluirBanco(dados_completos_1N[pos_dados_completos], estrutura_1N.Namespace, index))
            evtMudarAbaBanco(tab, index_tab, str_id);
    }
}

function adicionarPlayAction(lista_estrutura, action) {
    var lista_indices = Object.getOwnPropertyNames(lista_estrutura);
    for (let i = 0; i < lista_indices.length; i++)
        lista_indices[i] = lista_indices[i].toLowerCase();
    var pos = lista_indices.indexOf('playaction');

    lista_estrutura[Object.getOwnPropertyNames(lista_estrutura)[pos]] = action;
    return lista_estrutura;
}

function eventoSalvar1N(index, pos_estrutura, pos_dados_completos, tab, str_id_pai, index_tab) {
    var estrutura_1N = lista_estrutura_1N[pos_estrutura];

    var lista_indices_dados_1N = Object.getOwnPropertyNames(dados_completos_1N[0][0]);
    for (let i = 0; i < lista_indices_dados_1N.length; i++)
        lista_indices_dados_1N[i] = lista_indices_dados_1N[i].toLowerCase();


    var p_indice = 0;
    var str_id;
    for (let i = 0; i < estrutura_1N.Propriedades.length - 2; i++) {
        p_indice = lista_indices_dados_1N.indexOf(estrutura_1N.Propriedades[i].Identifier.toLowerCase()); //Ver depois HK
        str_id = ".inputP" + tab + 'I' + index + 'C' + i;
        if ($(str_id).val() != undefined)
            dados_completos_1N[pos_dados_completos][0][Object.getOwnPropertyNames(dados_completos_1N[pos_dados_completos][0])[p_indice]] = $(str_id).val();
    }

    dados_completos_1N[pos_dados_completos][0] = adicionarPlayAction(dados_completos_1N[pos_dados_completos][0], 'update'); //action

    str_id = '1NP' + tab + 'I' + index;
    atualizarBanco(dados_completos_1N[pos_dados_completos], estrutura_1N.Namespace, index).then(function (result) {
        if (result.STATUS === true) {
            evtMudarAbaBanco(tab, index_tab, str_id_pai);
        }
    })

}

function eventoCancelar1N(pagina, index, tipo, pos_estrutura, pos_dados_completos) {
    var estrutura_1N = lista_estrutura_1N[pos_estrutura];

    var lista_indices_dados_1N = Object.getOwnPropertyNames(dados_completos_1N[pos_dados_completos][0]);
    for (let i = 0; i < lista_indices_dados_1N.length; i++)
        lista_indices_dados_1N[i] = lista_indices_dados_1N[i].toLowerCase();

    var p_indice = 0;
    var str_id;
    for (let i = 0; i < estrutura_1N.Propriedades.length - 2; i++) {
        p_indice = lista_indices_dados_1N.indexOf(estrutura_1N.Propriedades[i].Identifier.toLowerCase()); //Ver depois HK
        str_id = ".inputP" + pagina + 'I' + index + 'C' + i;
        $(str_id).val(dados_completos_1N[pos_dados_completos][0][Object.getOwnPropertyNames(dados_completos_1N[pos_dados_completos][0])[p_indice]]);
        $(str_id).prop('disabled', true);
    }

    str_id = '1NP' + pagina + 'I' + index;
    habilitarBotoes(str_id, true, false, false, true);
}

function gerarBotoesFormularioInsert(index, flag = false) {
    var insertDireto = flag ? -1 : 0;
    var _aux = "form" + index;
    var botoes = '<div class="{0}">'.replace("{0}", "col-sm-3 col-md-2");
    botoes += elementoButton("Salvar", "btn btn-success", "insertSalvar", "onclick", "eventoSalvar(" + index + ", " + flag + ")", _aux, "submit");
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-3 col-md-2");
    botoes += elementoButton("Cancelar", "btn btn-secondary", "", "onclick", "eventoCancelarIndividual(" + index + ", " + insertDireto + ")", _aux, "submit");
    botoes += '</div>';

    //Ações relacionadas.
    var lista_metodos = pegarListaMetodosEstrutura(lista_estrutura);
    if (lista_metodos.length > 0) {
        botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
        botoes += elementoComboboxArea(lista_metodos, "", "acoes" + index, "", "", "", "", false, index);
        botoes += '</div>';
    }

    var div = elementoDiv(botoes, "panel-body", "insert" + index);
    return div;
}

function gerarBotoesFormulario1NInsert(pos_estrutura, tab, index, str_id, str_rmv, posicao_dado) {
    var _aux = "form1N" + tab + index;
    var botoes = '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Salvar", "btn btn-success", "", "onclick", "eventoSalvarIndividualInsert1N({0}, {1}, {2}, {3})", _aux, "submit")
        .replace('{0}', tab)
        .replace('{1}', index)
        .replace('{2}', "'" + str_id + "'")
        .replace('{3}', pos_estrutura)
        ;
    botoes += '</div>';

    botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
    botoes += elementoButton("Cancelar", "btn btn-secondary", "", "onclick", "eventoCancelarIndividual1N({0}), {1}, 'submit'")
        .replace('{0}', "'" + str_rmv + "'")
        .replace('{1}', "'" + _aux + "'")
        ;
    botoes += '</div>';

    //Ações relacionadas.
    var lista_metodos = pegarListaMetodosEstrutura(lista_estrutura_1N[pos_estrutura]);
    if (lista_metodos.length > 0) {
        botoes += '<div class="{0}">'.replace("{0}", "col-sm-2 col-md-2");
        botoes += elementoComboboxArea(lista_metodos, "", "acoes" + index, "", "", "", "", false, index, 1, posicao_dado, lista_estrutura_1N[pos_estrutura].Namespace);
        botoes += '</div>';
    }

    var div = elementoDiv(botoes, "panel-body", "insert" + index);
    return div;
}

//eventoSalvarIndividual
//eventoAlterarIndividual
//eventoExcluirIndividual
//eventoCancelarIndividual
function eventoCancelarIndividual1N(str_rmv) {
    $(str_rmv).remove();
}

function habilitarBotoes(num, btn_salvar, btn_alterar, btn_excluir, btn_cancelar) {
    $('#salvar' + num).prop('disabled', btn_salvar);
    $('#alter' + num).prop('disabled', btn_alterar);
    $('#excluir' + num).prop('disabled', btn_excluir);
    $('#cancelar' + num).prop('disabled', btn_cancelar);
}

function eventoAlterarIndividual(num) {
    var indice = num * lista_estrutura.Propriedades.length;
    for (let i = indice; i < indice + lista_estrutura.Propriedades.length; i++) {
        $('.inputprincipal' + i).prop("disabled", false);
    }
    $(".inputprincipal" + (indice + 1)).focus();
    $('#mensagem' + num).remove();

    habilitarBotoes(num, false, true, true, false);
}

function eventoCancelarIndividual(num, tipo) {

    var indice = 0;
    if (tipo == -1) { //Tipo insert direto
        gerarHTMLInsertDireto();
    }
    else if (tipo == 0) { //Tipo insert
        /*for (let i = 0; i < lista_estrutura.Propriedades.length - 2; i++) {
            if ($('.inputprincipal' + (indice + i)).prop("disabled"))
                $('.inputprincipal' + (indice + i)).prop("disabled", false);
            $('.inputprincipal' + (indice + i)).val("");
            $('.inputLOCKprincipal' + (indice + i)).val("");
        }*/
        gerarHTMLInsert();
    }
    else { //Tipo update
        indice = num * lista_estrutura.Propriedades.length;
        var lista_pk = pegarChavesPrimarias(lista_estrutura);
        lista_indices_dados = pegarIndicesDados(lista_dados_totais[0][0]);
        var pos = pegarPosicao(indice, lista_pk, lista_indices_dados);

        var p_indice = 0;
        for (let i = 0; i < lista_estrutura.Propriedades.length - 2; i++) {
            p_indice = lista_indices_dados.indexOf(lista_estrutura.Propriedades[i].Identifier.toLowerCase());

            var dado = lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]];
            //if (typeof dado == 'string')
            //    dado = dado.trim();

            $('.inputprincipal' + (indice + i)).val(dado);
            $('.inputprincipal' + (indice + i)).prop("disabled", true);
        }
    }
    $('.inputprincipal' + indice).focus();
    $('#insertSalvar').css({ display: "" });
    $('#concluirInsert').remove();
    $('#mensagem' + num).remove();
    habilitarBotoes(num, true, false, false, true);
}

function pegarIndicesDados(lista) {
    var res = Object.getOwnPropertyNames(lista);
    for (let i = 0; i < res.length; i++)
        res[i] = res[i].toLowerCase();

    return res;
}

function eventoSalvarIndividualInsert1N(tab, index, str_id, pos_estrutura) {

    var estrutura_insert_1N = lista_estrutura_insert_1N[pos_estrutura];
    var estrutura_1N = lista_estrutura_1N[pos_estrutura];
    //inputinsert1N16C2
    var lista_indices = Object.getOwnPropertyNames(estrutura_insert_1N);
    for (let i = 0; i < lista_indices.length; i++)
        lista_indices[i] = lista_indices[i].toLowerCase();

    for (let i = 0; i < lista_indices.length - 2; i++) {
        var identifier = estrutura_1N.Propriedades[i].Identifier.toLowerCase();
        var pos = lista_indices.indexOf(identifier);

        var str_campo = '.inputinsert1N' + tab + 'C' + i;
        if ($(str_campo).val() == undefined)
            str_campo = '.inputLOCKinsert1N' + tab + 'C' + i;

        var aux_dado = $(str_campo).val();
        estrutura_insert_1N[Object.getOwnPropertyNames(estrutura_insert_1N)[pos]] = aux_dado;
    }

    estrutura_insert_1N = adicionarPlayAction(estrutura_insert_1N, 'insert'); //action

    var dados_insert = []
    dados_insert.push(estrutura_insert_1N);
    //if(validarRequired(indice)){
    insertBanco(dados_insert, estrutura_1N.Namespace, 0).then(function (result) {
        if (result.STATUS === true) {
            evtMudarAbaBanco(tab, index, str_id);
        }
    });
}


function eventoSalvarIndividual(num) {
    var indice = num * lista_estrutura.Propriedades.length;
    var lista_pk = pegarChavesPrimarias(lista_estrutura);
    lista_indices_dados = pegarIndicesDados(lista_dados_totais[0][0]);
    var pos = pegarPosicao(indice, lista_pk, lista_indices_dados);
    var dados_salvar = [];

    if (pos < lista_dados_totais.length && pos != -1) {
        var dados_salvar = [];
        var res_temp = lista_dados[num];
        var temp = pegarDados(indice, pos);
        dados_salvar.push(res_temp);
        temp[0] = adicionarPlayAction(temp[0], 'update'); //action

        var atributos = Object.getOwnPropertyNames(dados_salvar[0]);
        for (let i = 0; i < atributos.length; i++) {
            dados_salvar[0][atributos[i]] = temp[0][atributos[i]];
        }

        if (validarRequired(indice)) {
            atualizarBanco(dados_salvar, lista_estrutura.Namespace, num).then(function (result) {
                if (result.STATUS === true) {
                    desabilitarCampos(num, lista_estrutura.Propriedades.length); //elemento
                    habilitarBotoes(num, true, false, false, false);
                    return true;
                }
            });
        }
    }
    else {
        alert('Ops! Isso não deveria acontecer');
    }
    return false;
}

async function eventoSalvar(num, flag) {

    Carregando.abrir('Processando ...');
    await new Promise(r => setTimeout(r, 100));
    eventoSalvarIndividualInsert(num, flag);
}

function eventoSalvarIndividualInsert(num, flag) {
    var indice = num * lista_estrutura.Propriedades.length;
    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var j = 0;
        let combobox = false;
        let checkbox = false;

        while (j < lista_estrutura.Propriedades[i].AnnotationsProp.length) {
            let name = lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName;
            if (name.includes("Combobox")) {
                combobox = true;
                break;
            }
            else if (name.includes("Checkbox")) {
                checkbox = true;
                break;
            }

            j++;
        }

        if (combobox) {
            lista_estrutura_insert[0][lista_estrutura.Propriedades[i].Identifier] = ($('.inputprincipal' + i).val());
        }
        else if (checkbox) {
            lista_estrutura_insert[0][lista_estrutura.Propriedades[i].Identifier] = $(`.inputprincipal${i}`).val();
        }
        else {
            lista_estrutura_insert[0][lista_estrutura.Propriedades[i].Identifier] = ($('.inputprincipal' + i).val());
        }

    }
    lista_estrutura_insert[0] = adicionarPlayAction(lista_estrutura_insert[0], 'insert'); //action

    var dados_insert = [];
    dados_insert.push(lista_estrutura_insert[0]);
    if (validarRequired(indice)) {
        $("#mensagem" + num).remove(); //Limpando as mensagens.
        insertBanco(dados_insert, lista_estrutura.Namespace, num).then(function (result) {
            if (result.STATUS === true) {

                if (flag) { //Insert direto
                    var lista = Object.getOwnPropertyNames(lista_estrutura_insert[0]);
                    for (let i = 0; i < lista.length; i++) {
                        lista_estrutura_insert[0][lista[i]] = "";
                    }

                    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
                        var k = 0;
                        while (k < lista_estrutura.Propriedades[i].AnnotationsProp.length && lista_estrutura.Propriedades[i].AnnotationsProp[k].AttributeName != 'Persistent')
                            k++;

                        if (k >= lista_estrutura.Propriedades[i].AnnotationsProp.length) {
                            $('.inputprincipal' + i).val('');
                        }
                    }
                    //eventoCancelarIndividual(0, -1);
                    if (result_global.list_objetos != null && result_global.list_objetos[0][0].PlayAction != "ALERT") {
                        var mensagem = "<strong>Cadastro realizado com sucesso!</strong><br>Suas informações foram cadastradas e já podem ser visualizadas no painel de pesquisa.";
                        mostraDialogo(mensagem, 'success', 3000, num);

                        var dialogo_rodape = elementoDiv("<span>SUCESSO! ID: {1} </span>", "col-sm-5 col-md-5 sucesso", "mensagem" + num)
                            .replace("{1}", result_global.log[0].PrimaryKey);
                    }

                    $("#insert" + num).append(dialogo_rodape);
                }
                else {
                    var res = confirm('Deseja continuar editando este item?');
                    if (res === true) {
                        preencherCamposAutoIncremento();
                        inserirBotaoConcluir();
                        continuarInsert();
                    }
                    else {
                        eventoCancelarIndividual(0, 0);
                        var mensagem = "<strong>Cadastro realizado com sucesso!</strong><br>Suas informações foram cadastradas e já podem ser visualizadas no painel de pesquisa.";
                        mostraDialogo(mensagem, 'success', 3000, num);

                        var dialogo_rodape = elementoDiv("<span>SUCESSO! ID: {1} </span>", "col-sm-5 col-md-5 sucesso", "mensagem" + num)
                            .replace("{1}", result_global.log[0].PrimaryKey);

                        $("#insert" + num).append(dialogo_rodape);

                        $('.inputprincipal0').focus(); //focus
                    }

                }
            }
        })

    }


}

function preencherCamposAutoIncremento() {
    var lista_pk = pegarChavesPrimarias(lista_estrutura);
    var lista_indices = Object.getOwnPropertyNames(lista_estrutura_insert[0]);
    for (let i = 0; i < lista_indices.length; i++)
        lista_indices[i] = lista_indices[i].toLowerCase();

    for (let i = 0; i < lista_pk.length; i++) {
        var pos = lista_indices.indexOf(lista_estrutura.Propriedades[lista_pk[i]].Identifier.toLowerCase());
        var j = 0;
        while (j < result_global.log[0].Properties.length && result_global.log[0].Properties[j].Name.toLowerCase() != lista_estrutura.Propriedades[lista_pk[i]].Identifier.toLowerCase()) {
            j++;
        }

        if (j < result_global.log[0].Properties.length) {
            var valor = result_global.log[0].Properties[j].Value;
            lista_estrutura_insert[0][Object.getOwnPropertyNames(lista_estrutura_insert[0])[pos]] = valor;
            $('.inputLOCKprincipal' + lista_pk[i]).val(valor);
        }

    }

    lista_dados = [];
    lista_dados.push(lista_estrutura_insert[0]);
}

function inserirBotaoConcluir() {
    $('#insertSalvar').css({ display: "none" });
    var str_button = elementoButton("Concluir", "btn btn-success", "concluirInsert", "onclick", "eventoConcluirInsert()", "", "submit");
    $('#insertSalvar').after(str_button);
}

function eventoConcluirInsert() {
    lista_indices_dados = Object.getOwnPropertyNames(lista_estrutura_insert[0]);
    for (let i = 0; i < lista_indices_dados.length; i++)
        lista_indices_dados[i] = lista_indices_dados[i].toLowerCase();

    var identifier = "";
    var pos = "";
    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        identifier = lista_estrutura.Propriedades[i].Identifier.toLowerCase();
        pos = lista_indices_dados.indexOf(identifier);
        if ($('.inputprincipal' + i).prop('disabled') == false)
            lista_dados[0][Object.getOwnPropertyNames(lista_dados[0])[pos]] = $('.inputprincipal' + i).val();
    }
    lista_dados[0] = adicionarPlayAction(lista_dados[0], 'update'); //action

    lista_dados_totais.push(lista_dados);
    if (eventoSalvarIndividual(0)) {
        lista_dados_totais = [];
        lista_dados = [];
        eventoCancelarIndividual(0, 0);

        $('#insertSalvar').css({ display: "" });
        $('#concluirInsert').remove();
    }
}

function continuarInsert() {
    var lista_pk = pegarChavesPrimarias(lista_estrutura);

    var lista_indices = Object.getOwnPropertyNames(lista_estrutura_insert[0]);
    for (let i = 0; i < lista_indices.length; i++) {
        lista_indices[i] = lista_indices[i].toLowerCase();
    }

    for (let i = 0; i < lista_pk.length; i++) {
        var pos = lista_indices.indexOf(lista_estrutura.Propriedades[lista_pk[i]].Identifier.toLowerCase());
        $('.inputprincipal' + pos).prop('disabled', true);
    }
}

function validarRequired(indice) {
    var i = indice;
    //elemento
    while (i < lista_estrutura.Propriedades.length + indice && !($('.inputprincipal' + i).prop('required') == true && $('.inputprincipal' + i).val() == ""))
        i++;

    if (i < lista_estrutura.Propriedades.length + indice) {

        let identifier = lista_estrutura.Propriedades[i - indice].Identifier;
        var text = `<strong>Erro ao cadastrar os dados!</strong><br>ERRO ao tentar inserir os dados, a mensagem de erro é: O campo ${identifier} requerido.`;

        $('.inputprincipal' + i).focus();
        mostraDialogo(text, 'error', 3000);
        Carregando.fechar();

        return false;
    }
    return true;
}

function pegarDados(indice, pos) {
    if (pos >= 0) {
        var p_indice;
        for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
            p_indice = lista_indices_dados.indexOf(lista_estrutura.Propriedades[i].Identifier.toLowerCase());

            if ($('.inputprincipal' + (indice + i)).val() != null) {
                if ($('.inputprincipal' + (indice + i)).val() == "") {
                    if (lista_estrutura.Propriedades[i].ForeignKey == null)
                        lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = "";
                    else
                        lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = null;

                } else {
                    lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = $('.inputprincipal' + (indice + i)).val();
                }
            }
            else {
                if ($('.inputLOCKprincipal' + (indice + i)).val() == "") {
                    if (lista_estrutura.Propriedades[i].ForeignKey == null)
                        lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = "";
                    else
                        lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = null;

                } else {
                    lista_dados_totais[pos][0][Object.getOwnPropertyNames(lista_dados_totais[pos][0])[p_indice]] = $('.inputLOCKprincipal' + (indice + i)).val();
                }
            }
        }
        return lista_dados_totais[pos];
    }
    return null;
}

//Flag diz se é uma exclusão normal, ou uma exclusão pelo APS.
function eventoExcluirIndividual(num, flag = 1) {
    id_exclusao = num;

    var res = confirm('Deseja excluir este item?');
    if (res === true)
        return prosseguirExclusao(flag);
}

function prosseguirExclusao(flag = 1) {
    var indice = id_exclusao * lista_estrutura.Propriedades.length;
    var lista_pk = pegarChavesPrimarias(lista_estrutura);
    lista_indices_dados = pegarIndicesDados(lista_dados_totais[0][0]);
    var pos = pegarPosicao(indice, lista_pk, lista_indices_dados);

    if (pos < lista_dados_totais.length) {
        var dados_exclusao = [];

        var dados_exclusao = [];
        var res_temp = lista_dados[id_exclusao];
        var temp = pegarDados(indice, pos);
        dados_exclusao.push(res_temp);

        var atributos = Object.getOwnPropertyNames(dados_exclusao[0]);
        for (let i = 0; i < atributos.length; i++) {
            if (temp[0][atributos[i]] != undefined) {
                dados_exclusao[0][atributos[i]] = temp[0][atributos[i]];
            }
            /*
            if (temp[0][atributos[i]] === "") {
                dados_exclusao[0][atributos[i]] = 0;
            }
            */
        }
        dados_exclusao[0] = adicionarPlayAction(dados_exclusao[0], 'delete'); //action

        //Limpando as mensagens.
        $("#mensagem" + id_exclusao).remove();
        if (excluirBanco(dados_exclusao, lista_estrutura.Namespace, id_exclusao)) {
            if (flag == 1)
                eventoPesquisar();
            return true;
        }
    }
}

function cancelarExclusao() {
    $('#modal-exclusao').modal('hide');
}

//Dado o índice HTML, exemplo:'.inputprincipal5' do campo e a lista de chaves primárias, a função busca qual a posição na lista de dados totais o dado está.
function pegarPosicao(indice_campo, lista_pk, lista_indices_dados) {
    var sair = false;
    var i = 0;
    var j = 0;

    while (i < lista_dados_totais.length) {
        j = 0;
        sair = false;
        while (j < lista_pk.length && !sair) {
            var identifier = lista_estrutura.Propriedades[lista_pk[j]].Identifier.toLowerCase();
            var pos = lista_indices_dados.indexOf(identifier);

            var valor = $('.inputLOCKprincipal' + (indice_campo + lista_pk[j])).val() != undefined ?
                $('.inputLOCKprincipal' + (indice_campo + lista_pk[j])).val() :
                $('.inputprincipal' + (indice_campo + lista_pk[j])).val();

            if (valor == undefined)
                valor = "";

            if (valor != lista_dados_totais[i][0][Object.getOwnPropertyNames(lista_dados_totais[i][0])[pos]])
                sair = true;
            else
                j++;
        }

        if (j >= lista_pk.length)
            return i;

        i++;
    }

    return -1;
}

function excluirBanco(dados_exclusao, lista_classes, num) {
    var dados_exclusao_JSON = JSON.stringify(dados_exclusao);
    var saida = false;
    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/Insert",
        data: { listJSON: dados_exclusao_JSON, class_name: lista_classes },
        dataType: "json",
        traditional: true,
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {

            for (i = 0; i < result.log.length; i++) {

                if (result.log[i].Status === "OK") {
                    var mensagem = 'O item foi excluído com sucesso!'
                    mostraDialogo(mensagem, 'success', 3000);

                    saida = true;
                } else if (result.log[i].Status === "ERRO") {
                    var mensagem = "<strong>Erro ao excluir os dados!</strong><br>ERRO ao tentar excluir os dados, a mensagem de erro é {0}."
                        .replace("{0}", result.log[0].MsgErro);
                    mostraDialogo(mensagem, 'danger', 3000);

                    var dialogo_rodape = elementoDiv("<span>ERRO! Mensagem de erro: {0}</span>", "col-sm-5 col-md-5 erro", "mensagem" + num)
                        .replace("{0}", result.log[0].MsgErro);

                    $("#excluir" + num).append(dialogo_rodape);
                }
            }
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao excluir os dados!</strong><br>ERRO ao tentar excluir os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        }
    });
    return true;
}

function atualizarBanco(dados_salvar, lista_classes, num, mostrar_dialogo = true) {
    return new Promise((resolve, reject) => {
        var dados_salvar_JSON = JSON.stringify(dados_salvar);
        var saida = false;
        let pks = [];
        let sucesso = true;
        let list_objetos = null;
        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/Insert",
            data: { listJSON: dados_salvar_JSON, class_name: lista_classes },
            dataType: "json",
            traditional: true,
            async: false,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                executarProtocolos(result.log);

                for (i = 0; i < result.log.length; i++) {
                    if (result.log[i].Status === "OK") {
                        var mensagem = "<strong>Alteração realizada com sucesso!</strong><br>As informações foram atualizadas e já podem ser visualizadas no painel de pesquisa.";
                        mostraDialogo(mensagem, 'success', 3000, num);

                        var dialogo_rodape = elementoDiv("<span>SUCESSO! O Id atualizado foi {0}</span>", "col-sm-12 col-md-12 sucesso", "mensagem" + num)
                            .replace("{0}", result.log[0].PrimaryKey)

                        //cria um objeto contendo o pk identifier : pk value e adiciona no array
                        if (result.log[i].PrimaryKey != null && result.log[i].PrimaryKey != "") {

                            let pk = result.log[i].PrimaryKey.split('.')[1] //T_PREFERENCIAS.PRE_ID:300 //pega do . pra frente
                            pkIdentifier = pk.split(':')[0]; //PRE_ID:300 pega do : pra tras
                            pkValue = pk.split(':')[1]; //PRE_ID:300 pega do : pra frente

                            var pkObject = { [pkIdentifier]: pkValue };
                            pks.push(pkObject);
                        }

                        if (mostrar_dialogo) $("#update" + num).prepend(dialogo_rodape);

                        saida = true;
                        list_objetos = result.list_objetos;

                    } else if (result.log[i].Status === "ERRO") {
                        var mensagem = "<strong>Erro ao atualizar os dados!</strong><br>ERRO ao tentar atualizar os dados, a mensagem de erro é: {0}."
                            .replace("{0}", result.log[i].MsgErro);
                        mostraDialogo(mensagem, 'danger', 3000);

                        var dialogo_rodape = elementoDiv("<span>ERRO! Mensagem de erro: {0}</span>", "col-sm-12 col-md-12 erro", "mensagem" + num)
                            .replace("{0}", result.log[i].MsgErro);

                        $("#update" + num).prepend(dialogo_rodape);
                        sucesso = false;
                    }
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao atualizar os dados!</strong><br>ERRO ao tentar atualizar os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);

                var dialogo_rodape = elementoDiv("<span>ERRO! Mensagem de erro: {0}</span>", "col-sm-12 col-md-12 erro", "mensagem")
                    .replace("{0}", result.log[0].MsgErro);

                $("#update" + num).prepend(dialogo_rodape);
                sucesso = false;
            }
        });

        resolve({ STATUS: sucesso, SAIDA: saida, NEWEST_PRIMARY_KEYS: pks, LIST_OBJETOS: list_objetos });
    });
}

function desabilitarCampos(pos, qtd_elementos) {
    var pos_inicial = pos * qtd_elementos;
    for (let i = pos_inicial; i < pos_inicial + qtd_elementos; i++) {
        $('.inputprincipal' + i).prop("disabled", true);
    }
}

//funcao converte os dados para json e insere no banco de dados
//retorna uma promise ao concluida e um objeto contendo o status, a saida, e um array das novas pks inseridas
function insertBanco(dados_insert, lista_classes, num) {
    return new Promise((resolve, reject) => {
        var dados_insert_JSON = JSON.stringify(dados_insert);
        $(".sucesso").remove();
        $(".erro").remove();
        var saida = false;
        let pks = [];
        let sucesso = true;
        let list_objetos = null;
        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/Insert",
            data: { listJSON: dados_insert_JSON, class_name: lista_classes },
            dataType: "json",
            traditional: true,
            async: false,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {

                executarProtocolos(result.log);
                result_log = result.log;

                for (i = 0; i < result.log.length; i++) {

                    if (result.log[i].Status === "OK" || result.log[i].Status === "REPORT_PDF") {
                        //criarPaginaInicial(url);
                        var mensagem = "<strong>Cadastro realizado com sucesso!</strong><br>Suas informações foram cadastradas e já podem ser visualizadas no painel de pesquisa.";
                        mostraDialogo(mensagem, 'success', 3000, num);

                        var dialogo_rodape = elementoDiv("<span>SUCESSO! ID: {1} </span>", "col-sm-5 col-md-5 sucesso", "mensagem" + num)
                            .replace("{1}", result.log[i].PrimaryKey);

                        if (result.log[i].PrimaryKey != null && result.log[i].PrimaryKey != "") {

                            let pk = result.log[i].PrimaryKey.split('.')[1] //T_PREFERENCIAS.PRE_ID:300 //pega do . pra frente
                            pkIdentifier = pk.split(':')[0]; //PRE_ID:300 pega do : pra tras
                            pkValue = pk.split(':')[1]; //PRE_ID:300 pega do : pra frente

                            var pkObject = { [pkIdentifier]: pkValue };
                            pks.push(pkObject);
                        }

                        $("#insert" + num).append(dialogo_rodape);
                        result_global = result;
                        saida = true;
                        list_objetos = result.list_objetos;
                    }
                    else if (result.log[i].Status === "ERRO") {
                        var mensagem = "<strong>Erro ao cadastrar os dados!</strong><br>ERRO ao tentar inserir os dados, a mensagem de erro é: {0}."
                            .replace("{0}", result.log[i].MsgErro);
                        mostraDialogo(mensagem, 'danger', 3000);

                        var dialogo_rodape = elementoDiv("<span>ERRO! Mensagem de erro: {0}</span>", "col-sm-5 col-md-5 erro", "mensagem" + num)
                            .replace("{0}", result.log[i].MsgErro);

                        $("#insert" + num).append(dialogo_rodape);
                        sucesso = false;
                    }
                    else if (result.log[i].Status === "ALERT") {
                        var dialogo_rodape = elementoDiv("<span>{1}</span>", "col-sm-5 col-md-5 sucesso", "mensagem" + num)
                            .replace("{1}", result.list_objetos[0][0].PlayMsgErroValidacao);

                        $("#insert" + num).append(dialogo_rodape);
                        result_global = result;
                        saida = true;
                        list_objetos = result.list_objetos;
                    }
                }

            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao cadastrar os dados!</strong><br>ERRO ao tentar inserir os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);

                var dialogo_rodape = elementoDiv("<span>ERRO! Mensagem de erro: {0}</span>", "col-sm-12 col-md-12 erro", "mensagem")
                    .replace("{0}", result.log[0].MsgErro);

                $("#insert" + num).prepend(dialogo_rodape);

                sucesso = false;
            }
        });


        $(document).ready(function () {
            Carregando.fechar();
        });

        resolve({ STATUS: sucesso, SAIDA: saida, NEWEST_PRIMARY_KEYS: pks, LIST_OBJETOS: list_objetos });
    });

}
//#################################### FUNÇÕES DE UTILIDADES ###########################################//
function separarDados(result) {
    lista_estrutura_insert = JSON.parse(result.instances);

    var lista_aux = JSON.parse(result.estrutura_classe);
    for (let i = 0; i < lista_aux.length; i++) {
        lista_aux[i].Namespace = lista_aux[i].ClassName;

        var temp = lista_aux[i].ClassName.split('.');
        namespace = "";
        for (let i = 0; i < temp.length - 1; i++) {
            namespace += temp[i] + '.';
        }
        lista_aux[i].ClassName = temp[temp.length - 1];
    }

    lista_estrutura = lista_aux[0];
    lista_estrutura_secundarias = lista_aux;
    lista_estrutura_secundarias.shift();
    lista_aux_secundaria = lista_estrutura_secundarias;

    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var j = 0;
        while (j < lista_estrutura.Propriedades[i].AnnotationsProp.length &&
            !(lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName == 'HIDDEN' ||
                lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName == 'NotMappedAttribute'))
            j++;

        if (j == lista_estrutura.Propriedades[i].AnnotationsProp.length)
            lista_indices_campos_visiveis.push(i);
    }
    lista_chaves_primarias = pegarChavesPrimarias(lista_estrutura);

    separarDados1N(result);
    lista_methods_class = pegarListaMetodosEstrutura(lista_estrutura);
}

function separarDados1N(result) {
    var estrutura_temp = JSON.parse(result.estrutura_classe_1N);
    var estrutura_insert_temp = JSON.parse(result.instances_1N);

    for (let i = 0; i < estrutura_temp.length; i++) {
        if (!estrutura_temp[i].SubClasse) {
            lista_estrutura_1N.push(estrutura_temp[i]);
            lista_estrutura_insert_1N.push(estrutura_insert_temp[i]);
        }
        else {
            lista_estrutura_1N_secundarias.push(estrutura_temp[i]);
            lista_estrutura_insert_1N_secundarias.push(estrutura_insert_temp[i]);
        }
    }

    var aux_str = "";
    for (let i = 0; i < lista_estrutura_1N.length; i++) {
        aux_str = lista_estrutura_1N[i].ClassName.split('.');
        aux_str = aux_str[aux_str.length - 1];

        lista_estrutura_1N[i].Namespace = lista_estrutura_1N[i].ClassName;
        lista_estrutura_1N[i].ClassName = aux_str;
    }

    for (let i = 0; i < lista_estrutura_1N_secundarias.length; i++) {
        aux_str = lista_estrutura_1N_secundarias[i].ClassName.split('.');
        aux_str = aux_str[aux_str.length - 1];

        lista_estrutura_1N_secundarias[i].Namespace = lista_estrutura_1N_secundarias[i].ClassName;
        lista_estrutura_1N_secundarias[i].ClassName = aux_str;
    }
}

function pegarChavesPrimarias(lista) {
    var lista_res = [];
    for (let i = 0; i < lista.Propriedades.length; i++) {
        if (lista.Propriedades[i].PrimaryKey == true)
            lista_res.push(i);
    }
    return lista_res;
}

function pegarChavesEstrangeiras(lista) {
    var lista_res = [];
    for (let i = 0; i < lista.Propriedades.length; i++) {
        if (lista.Propriedades[i].ForeignKey == "ForeignKey")
            lista_res.push(i);
    }

    return lista_res;
}

function pegarCamposSearch(lista) {
    var lista_res = [];
    for (let i = 0; i < lista.Propriedades.length; i++) {

        var j = 0;
        while (j < lista.Propriedades[i].AnnotationsProp.length && !(
            lista.Propriedades[i].AnnotationsProp[j].AttributeName == 'SEARCH' ||
            lista.Propriedades[i].AnnotationsProp[j].AttributeName == 'SEARCH_NOT_FK')) {
            j++;
        }

        if (j < lista.Propriedades[i].AnnotationsProp.length) {
            lista_res.push(lista.Propriedades[i].Identifier);
        }
    }

    return lista_res;
}

function pegarCamposPesquisa(lista) {
    for (let i = 0; i < lista.Propriedades.length; i++) {
        var j = 0;
        while (j < lista.Propriedades[i].AnnotationsProp.length && lista.Propriedades[i].AnnotationsProp[j].AttributeName !== 'SEARCH')
            j++;

        if (j < lista.Propriedades[i].AnnotationsProp.length)
            lista_chaves_primarias.push(i);
    }
}

function pegarListaIndicesEstrutura(lista) {
    for (let i = 0; i < lista.Propriedades.length; i++)
        lista_indices_estrutura.push(lista.Propriedades[i].Identifier.toLowerCase());
}

function pegarListaIndicesCabecalho(lista) {
    var lista_aux = Object.keys(lista[0]);
    for (let i = 0; i < lista_aux.length; i++)
        lista_indices_cabecalho.push(lista_aux[i].toLowerCase());
}

function mostraDialogo(mensagem, tipo, tempo, num) {

    // se houver outro alert desse sendo exibido, cancela essa requisição
    if ($("#message").is(":visible"))
        return false;

    // se não setar o tempo, o padrão é 3 segundos
    if (!tempo)
        var tempo = 3000;

    // se não setar o tipo, o padrão é alert-info
    if (!tipo)
        var tipo = "info";

    // monta o css da mensagem para que fique flutuando na frente de todos elementos da página
    var cssMessage = "display: block; position: fixed; top: 0; left: 20%; right: 20%; width: 60%; padding-top: 10px; z-index: 9999; word-wrap: break-word;";
    var cssInner = "margin: 0 auto; box-shadow: 1px 1px 5px black; word-wrap: break-word;";

    // monta o html da mensagem com Bootstrap
    var dialogo = "";
    dialogo += '<div id="message" style="' + cssMessage + '">';
    dialogo += '    <div class="alert alert-' + tipo + ' alert-dismissable" style="' + cssInner + '">';
    dialogo += '    <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>';
    dialogo += mensagem;
    dialogo += '    </div>';
    dialogo += '</div>';

    // adiciona ao body a mensagem com o efeito de fade
    $("body").append(dialogo);
    $("#message").hide();
    $("#message").fadeIn(200);

    // contador de tempo para a mensagem sumir
    setTimeout(function () {
        $('#message').fadeOut(300, function () {
            $(this).remove();
        });
    }, tempo); // milliseconds
}

//Função replace all;
String.prototype.replaceAll = String.prototype.replaceAll || function (needle, replacement) {
    return this.split(needle).join(replacement);
};
//Função insert em uma lista;
Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

function limparDados() {
    lista_estrutura = []; //Estrutura de dados 
    lista_estrutura_secundarias = []; //Tabelas secundárias
    lista_estrutura_insert = [];
    lista_estrutura_1N = [];
    lista_estrutura_1N_secundarias = [];
    lista_estrutura_insert_1N = [];
    lista_estrutura_insert_1N_secundarias = [];
    lista_aux_secundaria = [];

    lista_methods_class = [];

    lista_indices_cabecalho = [];
    lista_indices_estrutura = [];
    lista_indices_dados = [];
    lista_indices_campos_visiveis = [];
    lista_paginas_1N = [];

    lista_dados = []; //Lista com os dados ao buscar
    lista_dados_totais = []; //Lista com os dados completos
    lista_dados_totais_classes = [];
    dados_pesquisado_1N = [];
    dados_completos_1N = [];

    lista_colunas = [];
    selecionados = [];
    lista_abas = [];
    lista_chaves_primarias = [];

    campos_visiveis = 0;
    qtd_paginas = 1;
    tamanho_pagina = 10;
    qtd_dados_pesquisa = 0;
    id_exclusao = -1;
    namespace = "";
    tipo_de_tela = 1;
    funcao_pesquisa = "";
    qtd_display = 6;

    $("#modal-filtros").html("");
    $("#modal-filtros").remove();
}

function pegarDisplayNomeClasse(estrutura) {
    var i = 0;
    while (i < estrutura.AnnotationsClass.length && estrutura.AnnotationsClass[i].AttributeName != 'DisplayAttribute')
        i++;

    if (i < estrutura.AnnotationsClass.length)
        return estrutura.AnnotationsClass[i].Parametros[0].Value;

    return estrutura.ClassName;
}

function pegarDisplay(prop) {
    var i = 0;
    while (i < prop.AnnotationsProp.length && prop.AnnotationsProp[i].AttributeName != 'DisplayAttribute')
        i++;

    if (i < prop.AnnotationsProp.length)
        return prop.AnnotationsProp[i].Parametros[0].Value;

    return prop.Identifier;
}

//Pega os campos visíveis da lista_estrutura e põe em uma lista.
//A estrutura de cada objeto da lista é: id, dado, tipo;
function criarListaCombobox() {
    var lista_res = [];

    var objeto = {
        id: -1, //O id do campo é o identifier da lista_estrutura
        dado: 'SELECIONE O CAMPO', //O dado vai ser o identifier da lista_estrutura caso esse identifier não tenha o atributo display 
        tipo: 'STRING' //Esse é o tipo do campo: string, int, double, etc...
    }
    lista_res.push(objeto);

    //A lista de campos visíveis é uma lista com os identificadores 
    for (let i = 0; i < lista_indices_campos_visiveis.length; i++) {
        var aux = lista_estrutura.Propriedades[lista_indices_campos_visiveis[i]];
        var temp = aux.TypeProp.toUpperCase();
        if (temp.includes("STRING") || temp.includes("DATETIME") || temp.includes("INT") || temp.includes("DOUBLE")) {

            var j = 0;
            while (j < aux.AnnotationsProp.length && aux.AnnotationsProp[j].AttributeName !== 'DisplayAttribute')
                j++;

            if (j < aux.AnnotationsProp.length)
                strTemp = aux.AnnotationsProp[j].Parametros[0].Value;

            else
                strTemp = aux.Identifier;


            objeto = {
                id: aux.Identifier.toUpperCase(),
                dado: strTemp.toUpperCase(),
                tipo: aux.TypeProp.toUpperCase()
            }
            lista_res.push(objeto);
        }

    }

    var j = 0;
    for (var i = 1; i < lista_res.length; i++) {
        var atual = lista_res[i];
        j = i;
        while (j > 1 && lista_res[j - 1].dado > atual.dado) {
            lista_res[j] = lista_res[j - 1];
            j = j - 1;
        }
        lista_res[j] = atual;
    }
    return lista_res;
}

//Pega os elementos selecionados.
function pegarSelecionados() {
    var list_atributos = [];

    let valores = []; //lista com todos os campos selecionados
    $("[id^='divselect'").each(function () {
        valores.push($(this).val());
    })

    if (valores.length > 0) {

        //compara cada valor com a propriedade da estrutura e adiciona no array 
        var j = 0;
        while (j < valores.length) {
            var i = 0;
            while (i < lista_estrutura.Propriedades.length) {
                if (lista_estrutura.Propriedades[i].Identifier == valores[j]) {
                    list_atributos.push(valores[j]);
                }

                i++;
            }

            j++;
        }

        return list_atributos;
    }
    return [];
}


//Cria a estrutura no formato JSON que vai devolver para o back
function criarJsonPesquisa(str, classe_nome, lista_colunas, orderbys = "", estrutura = []) {
    if (estrutura.length == 0)
        estrutura = lista_estrutura;

    var str_json;
    var frases = str.split(/[&|]/);
    var operadores = str.split(/[^&|]/);
    operadores = removerPorValor(operadores, "");
    frases = removerPorValor(frases, "");
    frases = criarFiltersJson(frases, estrutura);
    operadores = criarOperatorsJson(operadores);
    colunas = criarColunasJson(lista_colunas, estrutura);
    orderbysJson = criarOrderByJson(orderbys);

    str_json = '{"ClassName": "{0}", {1}, {2}, {3}'
        .replace("{0}", classe_nome)
        .replace("{1}", frases)
        .replace("{2}", operadores)
        .replace("{3}", colunas)
        .replace("{4}", orderbysJson);

    //adiciona os orderbys no arquivo JSON caso seja diferente de vazio
    if (orderbysJson != "") {
        str_json += ',{4}'.replace("{4}", orderbysJson);
    }

    str_json += '}';

    return str_json;
}

//Cria a parte dos Operators no Json;
function criarOperatorsJson(operadores) {
    var str = '"Operators": [';
    if (operadores.length > 0) {
        for (let i = 0; i < operadores.length - 1; i++) {
            str += operadores[i] == "&" ? '{"TypeOperator" : "AND"},' :
                operadores[i] == "|" ? '{"TypeOperator" : "OR"},' :
                    operadores[i] == "!" ? '{"TypeOperator" : "NOT"}' :
                        '{"TypeOperator" : "ERROR"},';
        }
        str += operadores[operadores.length - 1] == "&" ? '{"TypeOperator" : "AND"}' :
            operadores[operadores.length - 1] == "|" ? '{"TypeOperator" : "OR"}' :
                operadores[operadores.length - 1] == "!" ? '{"TypeOperator" : "NOT"}' :
                    '{"TypeOperator" : "ERROR"}';
    }
    else if (lista_chaves_primarias.length > 1) {
        for (let i = 0; i < lista_chaves_primarias.length - 2; i++)
            str += '{"TypeOperator" : "OR"},';
        str += '{"TypeOperator" : "OR"}';
    }
    str += "]";
    return str;
}

//cria uma estrutura Json contendo os order bys
function criarOrderByJson(orderbys) {
    let str = "";

    if (orderbys == "" || orderbys == undefined || orderbys.length == 0 || orderbys == null)
        return "";

    let primeiro_order_by = orderbys.split(';')[0];
    str += `"OrderBys" : [{"NameProperty" : "${primeiro_order_by.split(',')[0]}","Order": "${primeiro_order_by.split(',')[1]}"}`;
    orderbys.split(';').forEach(function (val, index) {
        if (index > 0) {  //não pega o primeiro order by, pois ele é recuperado antes do forEach 
            let coluna = val.split(',')[0];
            let tipo_ordem = val.split(',')[1];

            if (coluna != "" && tipo_ordem != "")
                str += ',{"NameProperty" : "{0}","Order": "{1}" }'.replace("{0}", coluna).replace("{1}", tipo_ordem);
        }
    });
    str += ']';


    return str;
}

//Colunas que serão exibidas na pesquisa
function criarColunasJson(lista, estrutura) {
    var associacoes = [];
    for (let i = 0; i < lista.length; i++) {
        var j = 0;
        while (j < estrutura.Propriedades.length && lista[i] !== estrutura.Propriedades[j].Identifier)
            j++;

        if (j < estrutura.Propriedades.length)
            associacoes.push(estrutura.Propriedades[j].ForeignKey !== null ? estrutura.Propriedades[j].ForeignKeyClass : 'null');

    }
    var str = '"Columns": [';
    if (lista.length > 0) {
        for (let i = 0; i < lista.length - 1; i++)
            str += '{"NameColumn" : "{0}", "Table" : "{1}"},'.replace("{0}", lista[i]).replace("{1}", associacoes[i]);
        str += '{"NameColumn" : "{0}", "Table" : "{1}"}'.replace("{0}", lista[lista.length - 1]).replace("{1}", associacoes[lista.length - 1]);
    }
    str += ']';

    return str;
}

function relacionarOperadores(lista) {
    if (lista[1] === '%') {
        lista[1] = 'like';
        lista[2] = '%' + lista[2] + '%';
    }
    else if (lista[1] === '%=') {
        lista[1] = 'like';
        lista[2] = '%' + lista[2];
    }
    else if (lista[1] === '=%') {
        lista[1] = 'like';
        lista[2] = lista[2] + '%';
    }
    return lista;
}

//Remove um elemento da lista pelo seu valor
function removerPorValor(lista, valor) {
    for (let i = 0; i < lista.length; i++) {
        if (valor === lista[i]) {
            lista.splice(i, 1);
            i--;
        }
    }
    return lista;
}

//Criar a parte dos filtros Json;
function criarFiltersJson(frases, estrutura) {

    var str = '"Filters": [';
    var value;
    var operador, valor;
    if (frases.length > 0) {
        temp = frases[0].split(/(><|<>|[<>!%]=?|=|=%|%)([\w\W]+)/);
        if (temp.length === 1) {

            for (let i = 0; i < lista_chaves_primarias.length; i++) {
                operador = 'like';
                valor = '%{2}%';
                if (lista_estrutura.Propriedades[lista_chaves_primarias[i]].TypeProp == 'datetime') {
                    operador = 'between';
                    valor = '{2}';
                }

                str += '{"NameProperty":"{0}", "OpRelational": "' + operador + '", "Value":"' + valor + '", "Type":"{3}"},'
                    .replace("{0}", lista_estrutura.Propriedades[lista_chaves_primarias[i]].Identifier)
                    .replace("{2}", temp[0])
                    .replace("{3}", lista_estrutura.Propriedades[lista_chaves_primarias[i]].TypeProp);
            }
            str = str.substring(0, str.length - 1);
        }
        else {
            for (let i = 0; i < frases.length; i++) {
                temp = frases[i].split(/(><|<>|[<>!%]=?|=|=%|%)([\w\W]+)/);
                temp[2] += temp[3];
                value = verificarValue(temp, estrutura);
                temp[2] = temp[2].replaceAll("'", "").replaceAll('"', "");
                temp = relacionarOperadores(temp);

                valor = '%{2}%';
                if (value == 'datetime')
                    temp[1] = 'between';


                str += '{"NameProperty":"{0}", "OpRelational": "{1}", "Value":"{2}", "Type":"{3}"},'
                    .replace("{0}", temp[0])
                    .replace("{1}", temp[1])
                    .replace("{2}", temp[2])
                    .replace("{3}", value);
            }
            str = str.substring(0, str.length - 1);
        }

    }

    str += ']';
    return str;
}

function verificarValue(lista, estrutura) {
    var i = 0;
    while (i < estrutura.Propriedades.length && lista[0] != estrutura.Propriedades[i].Identifier)
        i++;

    var resultado = "string";
    if (i < estrutura.Propriedades.length) {
        var resultado = estrutura.Propriedades[i].TypeProp.toLowerCase();
        if (resultado.toLowerCase().includes('double') || resultado.toLowerCase().includes('int') || resultado.toLowerCase().includes('float'))
            return "";
    }
    return resultado;
}

//Retorna todas as colunas
function pegarTodasColunas() {
    var list_atributos = [];
    for (let i = 0; i < qtde_campos; i++)
        list_atributos.push($('#combo' + i).val());

    return list_atributos;
}

//Conta quantos campos do modelo que podem ser mostrados na tela;
function contarCamposVisiveis(lista_estrutura) {
    var qtde = lista_estrutura.Propriedades.length;
    var contador = qtde;

    for (let i = 0; i < qtde; i++) {
        for (let j = 0; j < lista_estrutura.Propriedades[i].AnnotationsProp.length; j++) {
            var attrib = lista_estrutura.Propriedades[i].AnnotationsProp[j].AttributeName.toLowerCase();
            if (attrib.includes("notmappedattribute"))
                contador--;
        }
    }
    return contador;
}

function eventoNovo(lista_estrutura, lista_estrutura_insert) {
    var div = "";

    lista_abas = [];
    lista_abas = pegarAbas(lista_estrutura, lista_abas);

    lista_dados_totais_classes = [];
    lista_aux_secundaria = lista_estrutura_secundarias;
    var table = gerarFormularioInsert(lista_estrutura, lista_estrutura_insert, lista_estrutura_secundarias, lista_abas, 0, "principal");

    div = elementoDiv(table, "dataTables_wrapper form-inline no-footer", "example2_wrapper");
    div = elementoDiv(div, "col-xs-12");
    div = elementoDiv(div, "row");
    div = elementoDiv(div, "panel-body");

    var str_html = div;
    return str_html;
}

function gerarFormularioInsert(lista_estrutura, dados = "", lista_estrutura_secundarias, lista_abas, id_campo = 0, str_id = "principal", index_estrutura = null) {
    var lista_str = [];
    var indice_aba = 0;

    for (let i = 0; i < lista_abas.length; i++)
        lista_str.push("");

    for (let i = 0; i < lista_estrutura.Propriedades.length; i++) {
        var propriedade = lista_estrutura.Propriedades[i];
        var j = 0;
        if (propriedade.ForeignKey == "1N") {
            while (j < lista_abas.length && propriedade.Identifier != lista_abas[j].identifier)
                j++;

            if (j < lista_abas.length)
                indice_aba = lista_abas[j].index;
        }
        else {
            while (j < propriedade.AnnotationsProp.length && propriedade.AnnotationsProp[j].AttributeName !== 'TAB')
                j++;

            if (j < propriedade.AnnotationsProp.length) {

                var k = 0;
                while (k < lista_abas.length && lista_abas[k].identifier != propriedade.AnnotationsProp[j].Parametros[0].Value)
                    k++;

                if (k < lista_abas.length) {
                    indice_aba = k;
                }
            }
        }

        //O que eu retirei daqui está em dontpad.com/play5
        lista_indices_dados = Object.getOwnPropertyNames(dados);
        for (let i = 0; i < lista_indices_dados.length; i++)
            lista_indices_dados[i] = lista_indices_dados[i].toLowerCase();

        var identifier = lista_estrutura.Propriedades[i].Identifier.toLowerCase();
        var pos = lista_indices_dados.indexOf(identifier);

        lista_str[indice_aba] += configuraTipoCampo(lista_estrutura.Propriedades[i], dados[Object.getOwnPropertyNames(dados)[pos]], id_campo++, 'insert', lista_estrutura_secundarias, str_id, i, index_estrutura);
        indice_aba = 0;
    }
    if (lista_str[0].length == 0) {
        lista_abas.shift();
        lista_str.shift();
    }

    indice_aba = 0;
    var str = "";
    var tabs = elementoTabs(lista_abas.length, indice_aba, 0, lista_abas, str_id);
    //str += elementoDiv(lista_str[0], "tab-pane container-fluid fade in active", "menu" + str_id + indice_aba);
    str += elementoDiv(lista_str[0], "tab-pane fade in active", "menu" + str_id + indice_aba);
    indice_aba++;
    for (let i = 1; i < lista_str.length; i++) {
        //str += elementoDiv(lista_str[i], "tab-pane container-fluid fade", "menu" + str_id + indice_aba);
        str += elementoDiv(lista_str[i], "tab-pane fade", "menu" + str_id + indice_aba);
        indice_aba++;
    }
    str = elementoDiv(str, "tab-content row");
    str = elementoDiv(tabs + str);
    str = elementoDiv(str, "panel-body");
    return str;
}

//evento de double click no order by
//irá alterar o html do icone de ordenação, colocando a ordem clicada, o tipo de ordem, e em seguida atualizando a preferencia e chamando a função para re-fazer a pesquisa.
async function eventoOrderBy(e) {
    const index = 0;

    //por enquanto só é possivel selecionar 1 order by
    if ($(`.orderby${index}[data-orderby='this']`).length) {
        $(`.orderby${index}`).attr('data-orderby', '');
        $(`.orderby${index}`).attr('class', `orderby${index} fa fa-square-o`);
    }

    //verifica se a coluna já nao esta selecionada para ser filtrada
    if ($(`#` + e.id).data('orderby') != "this") {
        //#region adicionar a coluna como a última clicada

        //pega o numero total de colunas clicadas, e adiciona essa coluna como a ultima clicada
        let colunas_clicadas = $(`.orderby${index}[data-orderby='this']`).length;
        $(`#${e.id}`).attr('data-ordemclicada', colunas_clicadas + 1);


        //selecionar essa coluna para ordernar
        $(`#${e.id}`).attr('data-orderby', 'this');
        $(`#${e.id}`).attr('class', `orderby${index} fa fa-arrow-up"`);
    }

    await salvarOrderByPreferencias(lista_estrutura.Namespace, index);

    //faz uma nova pesquisa com as instruções de ordenação atualizadas
    eventoPesquisarPrincipal();

}

//evento de double click no order by
//irá alterar o html do icone de ordenação, colocando a ordem clicada, o tipo de ordem, e em seguida atualizando a preferencia e chamando a função para re-fazer a pesquisa.
async function eventoOrderBy1N(e, tab, index, str_id) {
    //por enquanto só é possivel selecionar 1 order by
    if ($(`.orderby1N${index}[data-orderby='this']`).length) {
        $(`.orderby1N${index}`).attr('data-orderby', '');
        $(`.orderby1N${index}`).attr('class', `orderby1N${index} fa fa-square-o`);
    }

    //verifica se a coluna já nao esta selecionada para ser filtrada
    if ($(`.orderby1N${index}#${e.id}`).data('orderby') != "this") {
        //#region adicionar a coluna como a última clicada

        //pega o numero total de colunas clicadas, e adiciona essa coluna como a ultima clicada
        let colunas_clicadas = $(`.orderby1N${index}[data-orderby='this']`).length;
        $(`.orderby1N${index}#${e.id}`).attr('data-ordemclicada', colunas_clicadas + 1);


        //selecionar essa coluna para ordernar
        $(`.orderby1N${index}#${e.id}`).attr('data-orderby', 'this');
        $(`#${e.id}`).attr('class', `orderby1N${index} fa fa-arrow-down"`);
    }

    await salvarOrderByPreferencias(lista_abas[index].class, index, "1N");

    //faz uma nova pesquisa com as instruções de ordenação atualizadas
    evtMudarAbaBanco(tab, index, str_id);
}

//evento de clique único no icone de ordenação
//irá recuperar a ordem atual do campo e inverte-la (0 para 1 ou 1 para 0)
async function mudarTipoOrdenacao(e) {
    const index = 0;

    if ($(`#` + e.id).data('orderby') == 'this') {

        //ordem == 1 ASCENDENTE | ordem == 0 DESCENDENTE
        let ordem = $(`#${e.id}`).attr('data-tipoordem') == 0 ? 1 : 0; //inverte de 1 para 0 ou 0 para 1
        $(`#${e.id}`).attr('data-tipoordem', ordem);
        $(`#${e.id}`).attr('class', `orderby${index} fa fa-arrow-${ordem == 1 ? "up" : "down"}`);


        await salvarOrderByPreferencias(lista_estrutura.Namespace, index);

        //faz uma nova pesquisa com as instruções de ordenação atualizadas
        eventoPesquisarPrincipal();
    }
}

//evento de clique único no icone de ordenação
//irá recuperar a ordem atual do campo e inverte-la (0 para 1 ou 1 para 0)
async function mudarTipoOrdenacao1N(e, tab, index, str_id) {
    if ($(`#${e.id}`).data('orderby') == 'this') {

        //ordem == 1 ASCENDENTE | ordem == 0 DESCENDENTE
        let ordem = $(`#${e.id}`).attr('data-tipoordem') == 0 ? 1 : 0; //inverte de 1 para 0 ou 0 para 1
        $(`#${e.id}`).attr('data-tipoordem', ordem);
        $(`#${e.id}`).attr('class', `orderby1N${index} fa fa-arrow-${ordem == 1 ? "up" : "down"}`);

        await salvarOrderByPreferencias(lista_abas[index].class, index, "1N");

        //faz uma nova pesquisa com as instruções de ordenação atualizadas]
        evtMudarAbaBanco(tab, index, str_id);
    }
}

//#################################### FUNÇÕES DO CRUD ###########################################//


function eventoPesquisarNoBanco(filtro_pesquisa = "", string_pesquisa = "", pagina_atual = 1) {
    Carregando.abrir('Processando ...');

    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'POST',
            url: "/DynamicWeb/Pesquisar",
            data: {
                query_json: filtro_pesquisa, page_size: tamanho_pagina, index: pagina_atual, tipoPesquisa: 1
            },
            dataType: "json",
            traditional: true,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.st === "OK") {
                    if (result.objects[1] > 0) {
                        if (result.objects[0].length > 0) {
                            lista_dados = result.objects[0]; //Lista com os dados da pesquisa
                            pegarListaIndicesCabecalho(lista_dados);

                            qtd_paginas = Math.ceil(result.objects[1] / tamanho_pagina); //Para fazer a paginação
                            qtd_dados_pesquisa = result.objects[0].length; //Para fazer a paginação

                            var gambiarra = [];
                            dados_pesquisado_1N = [];
                            for (let i = 0; i < tamanho_pagina; i++) {
                                dados_pesquisado_1N.push(gambiarra);
                            }

                            lista_paginas_1N = []
                            for (let i = 0; i < tamanho_pagina; i++) {
                                lista_paginas_1N.push(0);
                            }

                            gerarHTMLPesquisa(pagina_atual);
                            $('#divitens').val(tamanho_pagina);

                            resolve(result);
                            if (qtd_dados_pesquisa == 1) {
                                eventoExpandir(0, selecionados.length);
                            }
                        }
                        else {
                            var mensagem = 'O banco de dados não possui registros para esta consulta!';
                            mostraDialogo(mensagem, 'info', 3000);
                            $('#bodyDadosPesquisado').remove();
                            $('.ctrlPag').remove();

                            reject(result);
                        }
                    }
                    else {
                        var mensagem = 'O banco de dados não possui registros para esta tabela!';
                        mostraDialogo(mensagem, 'info', 3000);
                        $('#bodyDadosPesquisado').remove();
                        $('.ctrlPag').remove();

                        reject(result);
                    }
                } else {//entrou no try catch do controler 
                    mostraDialogo(result.st, 'danger', 3000);
                    reject(result);
                    //$("#insert0").append(result.st);
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao pesquisar os dados!</strong><br>ERRO ao tentar pesquisar os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);
                reject();
            },
            complete: function () {
                Carregando.fechar();

                if (string_pesquisa != "")
                    desmontarStringPesquisa(string_pesquisa);

            }
        });
    });
}

function eventoPesquisarNoBancoExpandir(filtro_pesquisa, page_size, index = 1) {
    var dados_pesquisado = [];
    Carregando.abrir('Processando ...');
    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/Pesquisar",
        data: {
            query_json: filtro_pesquisa,
            page_size: page_size,
            index: index,
            tipoPesquisa: 2
        },
        dataType: "json",
        async: false,
        traditional: true,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                dados_pesquisado = result.objects[0]; //Lista com os dados da pesquisa
                tamanho_total_1N = result.objects[1];
            } else {//entrou no try catch do controler 
                mostraDialogo(result.st, 'danger', 3000);
            }
        },
        error: function (XMLHttpRequest, txtStatus, errorThrown) {
            var mensagem = `<strong>Erro ao pesquisar os dados!</strong><br>ERRO ao tentar pesquisar os dados, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
            mostraDialogo(mensagem, 'danger', 3000);
        },
        complete: function () {
            $(document).ready(function () {
                Carregando.fechar();
            });
        }
    });
    return dados_pesquisado;
}

//#################################### FUNÇÕES GLOBAIS ###########################################//
function global_modal(campo_id, namespace, _indicator) {
    limparDados();
    //acoes_modal_global = acoes; ->Implementar depois?
    $.ajax({
        type: "GET",
        url: namespace,
        dataType: "json",
        async: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            separarDados(result);
        },
        error: OnError,
        complete: function () {
            Carregando.fechar();
        }
    });
    var str = "";
    var lista_pk = pegarChavesPrimarias(lista_estrutura);
    var pesquisa = "";
    var i = 0;
    indicator = _indicator;
    while (i < lista_pk.length - 1)
        pesquisa += lista_pk[i++] + "=" + campo_id + "&";
    pesquisa += lista_estrutura.Propriedades[lista_pk[i]].Identifier + "=" + campo_id;

    var filtros = criarJsonPesquisa(pesquisa, lista_estrutura.Namespace, []);
    var dados_pesquisado = eventoPesquisarNoBancoExpandir(filtros, 1);
    lista_indices_dados = Object.getOwnPropertyNames(dados_pesquisado[0]);
    for (let i = 0; i < lista_indices_dados.length; i++)
        lista_indices_dados[i] = lista_indices_dados[i].toLowerCase();

    global_modal_id = campo_id;
    lista_dados_totais.push(dados_pesquisado);
    lista_dados.push(dados_pesquisado[0]);
    lista_abas = [];
    lista_abas = pegarAbas(lista_estrutura, lista_abas);

    //lista_dados_totais_classes.push(namespace + lista_estrutura.ClassName);
    var str_id = "principal";

    var header = '<div class="modal-header">' + lista_estrutura.ClassName + '</div>';
    var body = '<div class="modal-body">' + gerarFormulario(dados_pesquisado, 0, lista_estrutura, lista_estrutura_secundarias, lista_abas, lista_indices_dados, str_id); +'</div>';
    var footer = '<div>' + gerarBotoesFormularioAPS(0, campo_id); +'</div>';
    var str = '<div class="modal-content">' + header + body + footer + '</div>';
    str = '<div class="modal-dialog">' + str + '</div>';
    str = '<div class="modal fade" id="modalGlobalDynamic" data-backdrop="static" style="z-index:9998 !important;">' + str + '</div>';

    return str;
}

function openFile(path) {
    var urlCompleta = window.location.href;
    var url = urlCompleta.split('/D')[0];
    var newUrl = url + path;
    window.open(newUrl.toString());
}