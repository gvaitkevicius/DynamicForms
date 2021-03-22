var ListaPendenciasCadastrais = new function () {
    this.carregarTabela = function () {
        var h = '<span title = "Pendencias cadastrais">';
        h += '    <i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>';
        h += '</span> ';
        $('#notificaoes_1').html(h);
        var n = 0;
        $.get(UrlBase + 'ObterPendenciasCadastrais').done(function (pen) {
            if (pen.length > 0) {
                h = "";
                pen.forEach(function (p) {//gera as linhas da tabela
                    n++;
                    for (var i = 0; i < 7; i++) {
                        href = '';
                        htipo = '';
                        if (i === 0 && p.ROTEIRO === '') {
                            href = '<a href="/PlugAndPlay/Roteiros?Prod=' + p.PRO_ID + '">';
                            htipo = '<strong>Roteiro precisa ser cadastrado</strong>';
                        }
                        if (i === 1 && p.PRODUTO === '') {
                            href = '';
                            htipo = '<strong>Falha na interface do produto</strong>';
                        }
                        if (i === 2 && p.ESTRUTURA === '') {
                            href = '';
                            htipo = '<strong>Falha na interface da estrutura</strong>';
                        }
                        if (i === 3 && p.CLIENTE === '') {
                            href = '';
                            htipo = '<strong>Falha na interface do cliente</strong>';
                        }
                        if (i === 4 && p.HORARIO_ENTREGA === '') {
                            href = '<a href="/PlugAndPlay/HorariosRecebimento/Create?idCliente=' + p.CLI_ID + '">';
                            htipo = '<strong>Cadastre os Horarios de entrega</strong>';
                        }

                        //pendencia m3
                        if (i === 6 && (p.ORD_M3_UNITARIO === '' || p.ORD_M3_UNITARIO === 0)) {
                            htipo = '<strong>Metro cubico invalido</strong>';
                        }

                        if (htipo !== '') {
                            h += '<li class="busy"> ';
                            h += href;
                            h += '<div class="notice-icon">';
                            h += '      <i class="fa fa-check"></i>';
                            h += '  </div>';
                            h += '  <div>';
                            h += '      <span class="name">';
                            h += htipo;
                            h += '          <span class="time small"> ' + p.CLI_NOME + ' <br> Pedido: ' + p.ORD_ID + ' Entrega: ' + moment(p.ENTREGA).format("DD/MM/YYYY") + '  Quantidade:' + p.ORD_QUANTIDADE + '</span>';

                            /*if (i == 1 ) {
                                h += '          <span class="time small"> ' + p.ORD_ID + '</span>';
                            }
                            if (i == 2 ) {
                                h += '          <span class="time small"> ' + p.ORD_ID + '</span>';
                            }
                            if (i == 3 ) {
                                h += '          <span class="time small"> ' + p.CLI_NOME + '</span>';
                            }
                            if (i == 4) {
                                h += '          <span class="time small"> ' + p.CLI_NOME + '</span>';
                            }
                            if (i == 6) {
                                h += '          <span class="time small"> ' + p.CLI_NOME + ' <br> Pedido: ' + p.ORD_ID + ' Entrega: ' + moment(p.ENTREGA).format("DD/MM/YYYY") + '  Quantidade:' + p.ORD_QUANTIDADE + '</span>';
                            }*/

                            h += '      </span>';
                            h += '  </div>';
                            h += '</a>';
                            h += '</li>';

                            /*$('<td>', { class: 'ORD_ID' }).text(p.ORD_ID),
                                $('<td>', { class: 'T_PRO_ID' }).text(p.T_PRO_ID),
                                $('<td>', { class: 'ENTREGA' }).text(),
                                $('<td>', { class: 'ORD_QUANTIDADE' }).text(p.ORD_QUANTIDADE),
                                $('<td>', { class: 'CLI_ID' }).text(p.CLI_ID),
                                $('<td>', { class: 'CLI_NOME' }).text(p.CLI_NOME),
                                */
                        }
                    }
                });


                var cab = ' <li class="notify-toggle-wrapper showopacity"> ';
                cab += '     <a href="#" data-toggle="dropdown" class="toggle" aria-expanded="false">';
                cab += '    <i class="fa fa-bell"></i>';
                cab += '    <span class="badge badge-accent">' + n + '</span>';
                cab += ' </a>';
                cab += ' <ul class="dropdown-menu notifications animated fadeIn">';
                cab += '    <li class="total">';
                cab += '        <span class="small"> Você tem <strong>' + n + '</strong> Pendencias de Interface.';
                cab += '        </span>';
                cab += '    </li>';
                cab += '    <li style="overflow:scroll;overflow-x: hidden;" class="list ps-container">';
                cab += '        <ul class="dropdown-menu-list list-unstyled ps-scrollbar">';

                var cab2 = '</ul>';
                cab2 += '<div class="ps-scrollbar-x-rail" style = "left: 0px; bottom: 3px;"> <div class="ps-scrollbar-x" style="left: 0px; width: 0px;"></div></div> <div class="ps-scrollbar-y-rail" style="top: 0px; right: 3px;"><div class="ps-scrollbar-y" style="top: 0px; height: 0px;"></div></div></li>';
                cab2 += '        <li class="external">';
                cab2 += '   <a href="javascript:;">';
                cab2 += '       <span>Read All Notifications</span>';
                cab2 += '   </a>';
                cab2 += '</li>';
                cab2 += '</ul>';
                cab2 += '</li>';

                $('#notificaoes_1').html(cab +
                    h + cab2
                );
            } else {
                cab = ' <li class="notify-toggle-wrapper showopacity"> ';
                cab += '     <a onclick ="ListaPendenciasCadastrais.carregarTabela()" data-toggle="dropdown" class="toggle" aria-expanded="false">';
                cab += '    <i class="fa fa-bell" title="Pendeências de Interface"></i>';
                cab += '    <span class="badge badge-accent">' + n + '</span>';
                cab += ' </a>';
                cab += '</li>';
                $('#notificaoes_1').html(cab);
                
            }
        }).fail(function () {
            Modal.erro('Ocorreu um erro ao carregar pendencias de cadastro.');
        }).always(function () {
            fecharIC();
        });

    }
    function fecharIC() {
        return Carregando.fecharParcial('secaoInfoCarregando');
    }
}