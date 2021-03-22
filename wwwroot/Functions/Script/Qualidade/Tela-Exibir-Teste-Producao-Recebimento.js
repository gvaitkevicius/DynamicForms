$(document).ready(function () {
    ExiberTestesProducaoRecebimento();
});

function ExiberTestesProducaoRecebimento() {

    var path = '/plugandplay/Qualidade/' + "ObterTestesFisicosAgrupados";
    var divs;

    $.ajax({
        url: path,
        type: "GET",
        data: {},
        dataType: "json",
        success: function (result) { //Retorna os Teste encontrados por json do controller
            if (result != null) {

                for (var i = 0; i < result._lista.length; i++) {
                    divs +=
                        '<div class="panel-group">' +
                        '<div class="panel panel-default">' +
                        '+<div class="panel-heading">' +
                    '<h4 class="panel-title">' +
                    '<a data-toggle="collapse" href="#collapse' + i + '">'+'<b>Order: </b> '+ result._lista[i].ORD_ID + '</a>' +
                        '</h4>' +
                        '</div>' +
                        '<div id="collapse'+i+'" class="panel-collapse collapse">' +
                            '<div class="panel-body">'+'Produto: ' + result._lista[i].ROT_PRO_ID +'</div>' +
                            '<div class="panel-body">Máquina: ' + result._lista[i].ROT_MAQ_ID+'</div>' +
                            '<div class="panel-body">Repeticao: ' + result._lista[i].FPR_SEQ_REPETICAO + '</div>' +
                            '<div class="panel-footer">Transformacao: ' + result._lista[i].ROT_SEQ_TRANSFORMACAO+'</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
             
                }
    
                $('#ListaTestesPorOP').html(divs);
            }
            else { return 'Não foram encontrados testes';}          
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}
