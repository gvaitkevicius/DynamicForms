var col, campo = "", texto, qtdExibir;

function ListarTestesAgrupadosOP() {
    var path = "/plugandplay/Qualidade/ObterTestesFisicosAgrupados";
    texto = $('#sel2').val();

    $('#orders').remove();

    if ($('#divselect').find(":selected").text() != "Selecione o campo") {
        col = $('#divselect').find(":selected").val();
    }
    campo = $('#divfiltros').find(":selected").text();
    qtdExibir = $('#divitens').find(":selected").text();

    $.ajax({
        url: path,
        type: "GET",
        data: { Coluna: col, Campo: campo, Pesquisa: texto, Quantidade: qtdExibir },
        dataType: "json",
        success: function (result) {
            if (result != null) {
                var divListaTestes = $('#ListaTestesPorOP');
                let dados = result._lista;
                var _GrupoTeste = '';
                var cont = 0;

                _GrupoTeste += '<div id="orders" style="width:100%;">';
                dados.forEach(function (item) {
                    var dd = new Date(item.TES_EMISSAO);
                    _GrupoTeste += '<div class="">';
                    _GrupoTeste += '         <div class="panel-heading" style="display:flex">';
                    _GrupoTeste += '             <h4 class="panel-title">';
                    _GrupoTeste += '<div class="row" style="display:flex;">';
                    _GrupoTeste += '              <div class="col-sm"> <a data-toggle="collapse" href="#collapse' + cont + '"><b>Pedido:</b> ' + item.ORD_ID + '&nbsp;&nbsp;&nbsp;&nbsp;<b>Produto:</b>' + item.ROT_PRO_ID + '&nbsp;&nbsp;&nbsp;&nbsp;<b>Repetição:</b> ' + item.FPR_SEQ_REPETICAO + '&nbsp;&nbsp;&nbsp;&nbsp;<b>Emissão:</b> ' + dd.toLocaleDateString() + '</a></div>';
                    _GrupoTeste += '                <div class="col-sm"><a href="#" onclick="AprovarReprovarLote(\'' +
                        item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\', \'' + item.TES_EMISSAO + '\',\'' + 'A' + '\');"><span title="Aprovar Lote" ><i style="color:green;font-size: 18px;" class="fa fa-check-circle fa-5x"></i></span></a></div>';
                    _GrupoTeste += '                <div class="col-sm"><a href="#" onclick="AprovarReprovarLote(\'' + item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\', \'' + item.TES_EMISSAO + '\',\'' + 'B' + '\');"><span title="Recusar Lote" ><i  style="color:red;font-size: 18px;" class="fa fa-times-circle fa-5x"></i></span></a></div>';


                    _GrupoTeste += '             </h4>';
                    _GrupoTeste += '         </div>';
                    _GrupoTeste += '       <div data-toggle="collapse" id="collapse' + cont + '" class="panel-collapse collapse">';

                    _GrupoTeste += '         <div class="panel-heading" style="display:flex;">';
                    _GrupoTeste += '                 <div style="width:90%;">';
                    _GrupoTeste += '                     <h4 class="panel-title" style="display:width:90%;">';
                    _GrupoTeste += '                     <a style="display:flex;" data-toggle="collapse" href="#testesFisicos' + cont + '" onclick="javascript:PlanilhaTestesOP(\'' +
                        item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.ROT_MAQ_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\', \'' + item.TES_EMISSAO + '\',\'' + cont +
                        '\');"><b>Testes Fisicos</b></a>';
                    _GrupoTeste += '                    </h4>';
                    _GrupoTeste += '                 </div>';

                    _GrupoTeste += '<div class="row" style="display:flex">';
                    _GrupoTeste += '<div class="col-sm"><span title="Total de Amostras" class="badge badge-primary pull-right" style="background-color:blue;font-size: 12px;">' + item.Total + '</span></div>';
                    _GrupoTeste += '<div class="col-sm"><span class="pull-right">&nbsp;&nbsp;&nbsp;</span></div>';
                    _GrupoTeste += '<div class="col-sm"><a href="#" onclick="abrirModalAmostras(\'' + item.ORD_ID + '\',\'' + item.ROT_PRO_ID + '\',\'' + item.FPR_SEQ_REPETICAO + '\');"><span title="Adicionar Amostra"><i style="font-size: 18px;" class="fa fa-plus-square fa-5x"></i></span></a></div>';
                    _GrupoTeste += '<div class="col-sm"><span class="pull-right">&nbsp;&nbsp;&nbsp;</span></div>';
                    _GrupoTeste += '<div class="col-sm"><a href="#" onclick="abrirRemoverAmostras(\'' + item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.ROT_MAQ_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\',\'' + item.TES_EMISSAO + '\');"><span title="Remover Amostra"><i style="font-size: 18px;color:red;" class="fa fa-times-circle fa-5x"></i></span></a></div>';
                    _GrupoTeste += '</div>';
                    _GrupoTeste += '         </div>';

                    _GrupoTeste += '       <div id="testesFisicos' + cont + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="#accordionTeste-' + cont + '" aria-expanded="true" style="height: 0px;">';
                    _GrupoTeste += '       </div>';

                    _GrupoTeste += '         <div class="panel-heading" style="display:flex">';
                    _GrupoTeste += '             <h4 class="panel-title" style="width:90%;">';
                    _GrupoTeste += '               <a data-toggle="collapse" href="#testesVisuais' + cont + '"onclick="javascript:PlanilhaTestesVisuaisOP(\'' +
                        item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.ROT_MAQ_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\', \'' + cont +
                        '\');"><b>Testes Visuais</b></a>';
                    _GrupoTeste += '             </h4>';
                    _GrupoTeste += '<div class="row"  style="display:flex;">';
                    _GrupoTeste += '<div class="col-sm"><span id="qtdInspecaoVisual" title="Total de inspeção visual" class="badge badge-primary pull-right" style="background-color:blue;font-size: 12px;">' + item.TotalInsp + '</span></div>';
                    _GrupoTeste += '<div class="col-sm"><span class="pull-right">&nbsp;&nbsp;&nbsp;</span></div>';
                    _GrupoTeste += '<div class="col-sm"><a href="#" onclick="novaInspecao(\'' + item.ROT_MAQ_ID + '\',\'' + item.ROT_PRO_ID + '\',\'' + item.ORD_ID + '\',\'' + item.FPR_SEQ_REPETICAO + '\');"><span title="Adicionar Teste Visual"><i style="font-size: 18px;" class="fa fa-plus-square fa-5x"></i></span></a></div>';
                    _GrupoTeste += '</div>';
                    _GrupoTeste += '         </div>';

                    _GrupoTeste += '       <div id="testesVisuais' + cont + '" class="panel-collapse collapse" aria-expanded="true" role="tabpanel" style="height: 0px;">';
                    _GrupoTeste += '       </div>';
                    _GrupoTeste += '     </div>';



                    _GrupoTeste += '</div>';
                    cont++;
                })
                _GrupoTeste += '</div>';
                divListaTestes.html(_GrupoTeste);
                cont = 0;
            }
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function AprovarReprovarLote(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO, TES_EMISSAO, status) {

    Swal.fire({
        title: 'Tem Certeza?',
        text: (status == 'A') ? 'Liberar Lote!' : 'Recusar Lote!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: (status == 'A') ? 'Liberar' : 'Recusar'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/plugandplay/Qualidade/AprovarReprovarLote",
                type: "GET",
                data: { ORD_ID: ORD_ID, ROT_PRO_ID: ROT_PRO_ID, FPR_SEQ_REPETICAO: FPR_SEQ_REPETICAO, Data: TES_EMISSAO, status: status },
                dataType: "json",
                success: function (result) {
                    if (result == "OK") {
                        Swal.fire((status == 'A') ? 'Liberado!' : 'Recusado!', (status == 'A') ? 'Lote liberado com Sucesso' : 'Lote recusado com Sucesso', "success");
                    } else {
                        Swal.fire("Error!", result, "error");
                    }
                },
                error: function () {
                    return "Erro nao tratado";
                }
            });
        }
    })
}

function abrirRemoverAmostras(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, DATA_EMISSAO) {
    var path = "/plugandplay/Qualidade/ObterTestesFisicosPorOP";
    var cssMessage = "display: block; position: fixed; top: 0; left: 20%; right: 20%; width: 60%; padding-top: 10px; z-index: 9999; word-wrap: break-word; color: black;";
    var cssInner = "margin: 0 auto; box-shadow: 1px 1px 5px black; word-wrap: break-word; color:black;";
    var dialogo = "";
    $('#ModalAmostras').remove();
    $.ajax({
        url: path,
        type: "GET",
        data: { ORD_ID: ORD_ID, ROT_PRO_ID: ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO: FPR_SEQ_REPETICAO, DATA: DATA_EMISSAO },
        dataType: "json",
        success: function (result) {
            if (result != null) {

                dialogo += '<div id="ModalAmostras" style="' + cssMessage + '">';
                dialogo += '<div class="alert alert-success alert-dismissable" style="' + cssInner + '">';
                dialogo += '<div class="modal-content" style="' + cssInner + '">';
                dialogo += '<div class="modal-header" style="' + cssInner + '">';
                dialogo += '<h4 class="modal-title">Testes e suas Amostras </h4>';
                dialogo += '</div>';


                dialogo += '  <div class="form-group col-xs-12">';
                dialogo += '    <div class="form-group col-md-5" >';
                dialogo += '       <label style="color:black;">Selecione o tipo de Teste:</label>';
                dialogo += '       <select multiple class="form-control" id="TiposTeste">';

                result._lista.forEach(function (item) {
                    dialogo += '<option style="color:black;" value="' + item.TES_ID + '" onclick="SelectTeste(\'' + item.TES_NOME_TECNICO + '\',\'' +
                        item.ORD_ID + '\', \'' + item.ROT_PRO_ID + '\', \'' + item.FPR_SEQ_REPETICAO + '\',\'' + DATA_EMISSAO + '\');">' + item.TES_NOME_TECNICO + '</option>';
                })

                dialogo += '      </select>';
                dialogo += '       <button type="button" id="btnExcluirTeste" class="btn btn-sm btn-default" onclick="ExcluirTeste(\'' + result._lista[0].ORD_ID + '\', \'' + result._lista[0].ROT_PRO_ID + '\',\'' + result._lista[0].FPR_SEQ_REPETICAO + '\')">';
                dialogo += '           <i class="fa fa-eraser"></i> Excluir Teste';
                dialogo += '        </button>';
                dialogo += '   </div>';
                dialogo += '   <div class="btn-group-vertical col-md-2">';
                dialogo += '   </div>';
                dialogo += '   <div class="form-group col-md-5" >';
                dialogo += '   <label style="color:black;">Amostra do Teste selecionado:</label>';
                dialogo += '    <select multiple class="form-control" id="AmostrasSelecionados">';
                dialogo += '  </select>';
                dialogo += '  <button type="button" id="btnExcluirAmostra" class="btn btn-sm btn-default" onclick="ExcluirAmostra(\'' + result._lista[0].ORD_ID + '\', \'' + result._lista[0].ROT_PRO_ID + '\',\'' + result._lista[0].FPR_SEQ_REPETICAO + '\')">';
                dialogo += '     <i class="fa fa-eraser"></i> Excluir Amostra';
                dialogo += '  </button>';
                dialogo += ' </div>';
                dialogo += ' </div>';


                dialogo += '<div class="modal-footer">';
                dialogo += '<button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>';
                dialogo += '</div>';
                dialogo += '</div>';
                dialogo += '</div>';
                dialogo += '</div>';

                $('#modalRemoverAmostra').append(dialogo);
            }
            else { return 'Não foram encontrados testes'; }
            $('#modalRemoverAmostra').modal('show');
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function SelectTeste(nome, ord_id, pro_id, seq_rep, DATA_EMISSAO) {
    $('#AmostrasSelecionados').find('option').each(function () {
        $(this).remove();
    });

    $.ajax({
        url: "/plugandplay/Qualidade/AmostrasDoTipoTeste",
        type: "GET",
        data: { ord_Id: ord_id, pro_Id: pro_id, seq_Rep: seq_rep, Nome: nome, Data: DATA_EMISSAO },
        dataType: "json",
        success: function (result) {
            if (result != null) {
                var i = 1;
                result.forEach(function (item) {
                    $('#AmostrasSelecionados').append('<option style="color:black;" value="' + item.TES_ID + '">Amostra' + i + '--Valor: ' + item.TES_VALOR_NUMERICO + '</option>');
                    i++;
                })

            }
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function ExcluirTeste(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO) {

    var ids = [];
    ids.push($('#TiposTeste').find(":selected").val());
    $('#modalRemoverAmostra').modal('hide');
    if (ids != null) {
        Swal.fire({
            title: 'Tem Certeza?',
            text: "Não poderá reverter!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Excluir!'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/plugandplay/Qualidade/ExcluirTeste",
                    type: "GET",
                    traditional: true,
                    data: { IdTestes: ids, ord_Id: ORD_ID, pro_Id: ROT_PRO_ID, seq_Rep: FPR_SEQ_REPETICAO, tipo: 0 },
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            Swal.fire({
                                title: 'Deletado',
                                text: "O Teste foi deletado!",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.value) {
                                    $('#AmostrasSelecionados').find('option').each(function () {
                                        $(this).remove();
                                        $('#TiposTeste').find(":selected").remove();
                                    });
                                    $('#modalRemoverAmostra').modal('show');
                                }
                            });
                        }
                    },
                    error: function () {
                        return "Erro nao tratado";
                    }
                });
            } else {
                $('#modalRemoverAmostra').modal('show');
            }
        })

    }

}

function ExcluirAmostra(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO) {

    var ids = [];
    ids.push($('#AmostrasSelecionados').find(":selected").val());
    $('#modalRemoverAmostra').modal('hide');
    if (ids != null) {
        Swal.fire({
            title: 'Tem Certeza?',
            text: "Não poderá reverter!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Excluir!'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/plugandplay/Qualidade/ExcluirTeste",
                    type: "GET",
                    traditional: true,
                    data: { IdTestes: ids, ord_Id: ORD_ID, pro_Id: ROT_PRO_ID, seq_Rep: FPR_SEQ_REPETICAO, tipo: 1 },
                    dataType: "json",
                    success: function (result) {
                        if (result) {
                            Swal.fire({
                                title: 'Deletado',
                                text: "A amostra foi deletada!",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.value) {
                                    $('#AmostrasSelecionados').find(":selected").remove();
                                    $('#modalRemoverAmostra').modal('show');
                                }
                            });
                        }
                    },
                    error: function () {
                        return "Erro nao tratado";
                    }
                });
            } else {
                $('#modalRemoverAmostra').modal('show');
            }
        })
    }
}

function mostraDialogo(mensagem, tipo) {

    // se houver outro alert desse sendo exibido, cancela essa requisição
    if ($("#message").is(":visible"))
        return false;


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
    }, 3000); // milliseconds
}

/*$("div").click(function () {
    $("div").removeClass('in');
});*/