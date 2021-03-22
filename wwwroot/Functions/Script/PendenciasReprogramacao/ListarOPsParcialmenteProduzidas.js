var ListaOPsParciais = new function () {
    this.carregarTabela = function () {

        var h = '<span title = "OPs parciais">';
        h += '    <i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>';
        h += '</span> ';
        $('#notificaoes_2').html(h);


        $.get(UrlBase + 'ObterOpsParciais').done(function (pen) {
            if (pen.length > -1) {
                var trs = [];
                var h = "";
                var n = 0;
                pen.forEach(function (p) {//gera as linhas da tabela
                    n++;
                   
                    var split = p.DATA_PRODUCAO.split('-');
                    var novadata = split[2].substring(0, 2) + "/" + split[1] + "/" + split[0];

                    href = "<a href=\"javascript:ResiduosReprogramacaoOP('" + p.FASE + "','" + p.ORD_ID + "','" + p.PRO_ID + "','" + p.PRO_DESCRICAO + "','" + p.SALDO_A_PRODUZIR + "','" + p.FPR_SEQ_REPETICAO + "','" + p.ORD_OP_INTEGRACAO + "','" + p.ORD_QUANTIDADE + "','" + p.CLI_NOME + "','" + p.FPR_PREVISAO_MATERIA_PRIMA + "','" + p.FPR_PREVISAO_MATERIA_PRIMA + "');\">";
                     
                    h += '<li class="busy"> ';
                    h += href;
                    h += '<div class="notice-icon">';
                    h += '      <i class="fa fa-check"></i>';
                    h += '  </div>';
                    h += '  <div>';
                    h += '      <span class="name">';
                    h += '<strong>OP parcialmente Produzida</strong>';
                    h += '          <span class="time small">FASE: ' + p.FASE + ' OP: ' + p.ORD_ID + '  ' + p.PRO_ID + '  ' + p.FPR_SEQ_REPETICAO + '  Qtd:' + p.QTD_PRODUZIDA + '  Saldo: ' + p.SALDO_A_PRODUZIR + ' DATA PRODUÇÃO: ' + novadata + ' </span>';//p.SALDO
                    h += '      </span>';
                    h += '  </div>';
                    h += '</a>';
                    h += '</li>';

                });

                var cab = ' <li class="notify-toggle-wrapper showopacity"> ';
                cab += '     <a href="#" data-toggle="dropdown" class="toggle" aria-expanded="false">';
                cab += '    <i class="fa fa-bell" title="Pendências de Reprogramação" ></i>';
                cab += '    <span class="badge badge-accent">' + n + '</span>';
                cab += ' </a>';
                
                cab += ' <ul  class="dropdown-menu notifications animated fadeIn ">';
                cab += '    <li class="total">';
                cab += '        <span class="small"> Você tem <strong>' + n + '</strong> Pendências de Reprogramação.';
                cab += '        </span>';
                cab += '    </li>';
                cab += '    <li style="overflow:scroll;overflow-x: hidden;" class="list ps-container">';
                cab += '        <ul class="dropdown-menu-list list-unstyled ps-scrollbar">';
                var cab2 = '</ul>';
                cab2 += '</ul>';
                cab2 += '</li>';


                $('#notificaoes_2').html(cab + h + cab2);
            }
        }).fail(function () {
            Modal.erro('Ocorreu um erro ao carregar pendencias de cadastro.');
        }).always(function () {

        });
    }
}
