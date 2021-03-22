var ListaPedidosSuspensos = new function () {
    this.carregarTabela = function () {

        var h = '<span title = "Pedidos Suspensos">';
        h += '    <i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>';
        h += '</span> ';
        $('#notificaoes_3').html(h);

        $.get(UrlBase + 'ObterPedidosSuspensos').done(function (pen) {
            if (pen.length > -1) {
               
                var h = "";
                var n = 0;
                pen.forEach(function (p) {//gera as linhas da tabela
                    n++;

                    href = "<a href=\"javascript:\">";
                    var split = p.ORD_DATA_ENTREGA_ATE.split('-');
                    var novadata = split[2].substring(0, 2) + "/" + split[1] + "/" + split[0];
                    h += '<li class="busy"> ';
                    h += href;
                    h += '<div class="notice-icon">';
                    h += '      <i class="fa fa-check"></i>';
                    h += '  </div>';
                    h += '  <div>';
                    h += '      <span class="name">';
                    h += '<strong>Pedido suspenso</strong>';
                    h += '          <span class="time small">' + p.ORD_ID + ' - ' + p.PRO_ID + ' - ' + novadata + ' - ' + p.CLI_NOME +'  </span > ';
                    h += '      </span>';
                    h += '  </div>';
                    h += '</a>';
                    h += '</li>';

                });

                var cab = ' <li class="notify-toggle-wrapper showopacity"> ';
                cab += '     <a href="#" data-toggle="dropdown" class="toggle" aria-expanded="false">';
                cab += '    <i class="fa fa-bell" title="Pedidos Suspensos"></i>';
                cab += '    <span class="badge badge-accent">' + n + '</span>';
                cab += ' </a>';

                cab += ' <ul  class="dropdown-menu notifications animated fadeIn ">';
                cab += '    <li class="total">';
                cab += '        <span class="small"> Você tem <strong>' + n + '</strong> Pedidos suspensos.';
                cab += '        </span>';
                cab += '    </li>';
                cab += '    <li style="overflow:scroll;overflow-x: hidden;" class="list ps-container">';
                cab += '        <ul class="dropdown-menu-list list-unstyled ps-scrollbar">';
                var cab2 = '</ul>';
                cab2 += '</ul>';
                cab2 += '</li>';

                $('#notificaoes_3').html(cab + h + cab2);
            }
        }).fail(function () {
            Modal.erro('Ocorreu um erro ao carregar Pedidos suspensos.');
        }).always(function () {

        });
    }
}
