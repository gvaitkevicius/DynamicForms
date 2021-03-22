
/**
 * Essa função move a coluna clicada para a direção no parametro 'direcao'.
 * Para fazer isso, ela copia todo o conteudo do body e head da coluna atual, e cola na coluna ao lado.
 * @param {*} index O index da aba
 * @param {*} coluna O index da coluna clicada
 * @param {*} direcao A direcao de movimento (1 ou -1)
 * @param {*} origem_1n 1 para origem 1N(apelido para tabelas estrangeiras) e 0 para normal
 */
function moverColuna(index, coluna, direcao, origem_1n) {
    let origem = origem_1n == 1 ? "1N" : "";
    let quantidade_colunas = $(`.colunas${origem}${index}`).length; //quantidade de colunas na tabela
    let coluna_target = coluna + direcao; //index da coluna destino que o usuario está tentando mover

    if (coluna_target >= 0 && coluna_target < quantidade_colunas) {

        moverConteudosCelula(index, coluna, coluna_target, origem_1n);
        moverHeaderColuna(index, coluna, coluna_target, origem_1n);

        //recupera a lista de indices das colunas na tabela
        let lista_indices = [];
        $(`th.colunas${origem}${index}`).each(function () {
            lista_indices.push(parseInt($(this).attr('value')));
        })

        //atualiza a variavel global que possue a lista dos indices
        lista_indices_display = lista_indices;

        //recupera a estrutura da aba
        if(origem_1n == 1){
            var aux_nome = lista_abas[index].class;
            var i = 0;
            while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
                i++;
    
            //converte a lista de indices para lista de nomes e salva na tabela de preferencias
            let lista_nomes = listaIndicesParaListaNomes(lista_indices, lista_estrutura_1N[i]);
            salvarPreferenciaDisplay(lista_nomes, lista_estrutura_1N[i]);
        }
        else{
            let lista_nomes = listaIndicesParaListaNomes(lista_indices, lista_estrutura);
            salvarPreferenciaDisplay(lista_nomes, lista_estrutura);
        }

    }
}

/**
 * Essa função irá percorrer cada célula da coluna target e irá mudar seu conteúdo para o conteúdo da coluna atual
 * Também irá mudar o conteúdo da coluna atual para o conteúdo contido na coluna target
 */
function moverConteudosCelula(index, coluna, coluna_target, origem_1n) {
    let origem = origem_1n == 1 ? "1N" : "";

    //se a origem for de uma tabela normal de pesquisa, é preciso pular 1 linha 
    coluna = origem != "1N" ? coluna + 1 : coluna;
    coluna_target = origem != "1N" ? coluna_target + 1 : coluna_target;

    let conteudo_coluna_atual = $(`table#table${index} tbody td:nth-child(${coluna + 1})`).toArray(); //array contendo todas as celulas da coluna atual

    //acesa cada célula da coluna destino e muda seu conteudo
    $(`table#table${index} tbody td:nth-child(${coluna_target + 1})`).each(function (i) {
        let value_coluna_target = conteudo_coluna_atual[i].innerHTML; //valor que a coluna destino assumirá
        let value_coluna_atual = $(this).html(); //valor que a coluna atual assumirá

        $(this).html(value_coluna_target); //coluna target recebe o valor target
        conteudo_coluna_atual[i].innerHTML = value_coluna_atual;
    });
}

/**
 * Essa função é responsável por copiar o conteúdo do header da coluna (TH) e colar na coluna target.
 */
function moverHeaderColuna(index, coluna, coluna_target, origem_1n) {

    //procura qual a posição da aba dentro do array lista_estrutura_1N
    var aux_nome = lista_abas[index].class;
    var i = 0;
    while (i < lista_estrutura_1N.length && aux_nome != lista_estrutura_1N[i].Namespace)
        i++;

    let index_estrutura = i;

    let origem = origem_1n == 1 ? "1N" : "";


    //index do atributo em relação a classe
    let index_header_atual = parseInt($(`#coluna${origem}${coluna}index${index}`).attr('value'));
    let index_header_target = parseInt($(`#coluna${origem}${coluna_target}index${index}`).attr('value'));

    //icone de ordenação
    let icone_orderby_atual = $(`#orderby${origem}Coluna${index_header_atual}Index${index}`).first().prop('outerHTML');
    let icone_orderby_target = $(`#orderby${origem}Coluna${index_header_target}Index${index}`).first().prop('outerHTML');

    let display_target = pegarDisplay(
            origem_1n == 1 ?
            lista_estrutura_1N[index_estrutura].Propriedades[index_header_target] :
            lista_estrutura.Propriedades[index_header_target]
    );

    let display_atual = pegarDisplay(
        origem_1n == 1 ?
            lista_estrutura_1N[index_estrutura].Propriedades[index_header_atual] :
            lista_estrutura.Propriedades[index_header_atual]
    );

    //constrói o html do header
    let html_final_header_target = icone_orderby_target + 
        `<i class="fa fa-arrow-left" onclick="moverColuna(${index}, ${coluna}, -1, ${origem_1n})"></i> ` +
        display_target + 
        `<i class="fa fa-arrow-right" onclick="moverColuna(${index}, ${coluna}, 1, ${origem_1n})"></i>`;

    let html_final_header_atual = icone_orderby_atual +
        `<i class="fa fa-arrow-left" onclick="moverColuna(${index}, ${coluna_target}, -1, ${origem_1n})"></i> `+ 
         display_atual + 
        `<i class="fa fa-arrow-right" onclick="moverColuna(${index}, ${coluna_target}, 1, ${origem_1n})"></i>`;


    //atribui os headers e ajusta o index
    $(`#coluna${origem}${coluna_target}index${index}`).html(html_final_header_atual); //substitui o html interior
    $(`#coluna${origem}${coluna_target}index${index}`).attr('value', index_header_atual); //substitui o index do atributo

    $(`#coluna${origem}${coluna}index${index}`).html(html_final_header_target);//substitui o html interior
    $(`#coluna${origem}${coluna}index${index}`).attr('value', index_header_target);//substitui o index do atributo

}
