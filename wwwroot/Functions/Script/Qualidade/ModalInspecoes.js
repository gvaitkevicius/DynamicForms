var Maq_Id, Pro_Id, Ord_Id, Seq_Rep, Seq_Trans;
var idsInput = [];
var posObsInput = [];
var pos = [];
function iniciarInspecaoClick(ROT_MAQ_ID, ROT_PRO_ID, ORD_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO,Tem_id) {
    Maq_Id = ROT_MAQ_ID, Pro_Id = ROT_PRO_ID, Ord_Id = ORD_ID, Seq_Rep = FPR_SEQ_REPETICAO, Seq_Trans = ROT_SEQ_TRANSFORMACAO;

    var cssMessage = "display: block; position: fixed; top: 0; left: 20%; right: 20%; width: 60%; padding-top: 10px; z-index: 9999; word-wrap: break-word; color: black;";
    var cssInner = "margin: 0 auto; box-shadow: 1px 1px 5px black; word-wrap: break-word; color:black;";

    // monta o html da mensagem com Bootstrap
    var dialogo = "";
    var path = "/plugandplay/Qualidade/ObterInspecoes";
    $.ajax({
        url: path,
        type: "GET",
        data: { Ord_Id: ORD_ID, Prod_Id: ROT_PRO_ID, Maq_Id: ROT_MAQ_ID, Seq_Rep: FPR_SEQ_REPETICAO, Seq_Trans: ROT_SEQ_TRANSFORMACAO, TEM_ID: Tem_id },
        dataType: "json",
        success: function (result) {
            if (result.length>0) {
                dialogo += '<div id="ModalInspecoes" style="' + cssMessage + '">';
                dialogo += '    <div class="alert alert-success alert-dismissable" style="' + cssInner + '">';
                dialogo += ' <div class="modal-content" style="' + cssInner + '">';
                dialogo += '<div class="modal-header" style="' + cssInner + '">';
                dialogo += '<h4 class="modal-title">Inspeções</h4>';
                dialogo += '</div>';
                dialogo += ' <div class="modal-body">';
                dialogo += '<div class="row">';
                dialogo += '<div class="col-md-12">';
                dialogo += ' <div class="col_md_12">';

                dialogo += '     <table id="tabelaInspecoes" class="table table-bordered table-responsive-md  text-center" style="' + cssInner + '">';
                dialogo += '       <thead>';
                dialogo += '         <tr>';
                dialogo += '           <th class="text-center col-md-2" style="' + cssInner + '"> Nome Técnico</th>';
                dialogo += '           <th class="text-center col-md-3" style="' + cssInner + '">Tolerâncias</th>';
                dialogo += '           <th class="text-center col-md-1" style="' + cssInner + '">Resultado</th>';
                dialogo += '           <th class="text-center col-md-6" style="' + cssInner + '">OBS</th>';
                dialogo += '         </tr>';
                dialogo += '       </thead>';
                dialogo += '       <tbody>';

                for (i = 0; i < result.length; i++) {
                    dialogo += '       <tr>';
                    dialogo += '<td class="col-md-2" contenteditable="false" id="NomeTecnico' + i + '" style="' + cssInner + '">' + result[i].TIV_NOME + '</td>';
                    dialogo += '<td class="col-md-3" contenteditable="false" id="tolerancia' + i + '" style="' + cssInner + '">' + result[i].TIV_DESCRICAO + '</td>';
                    if (result[i].TIV_MEDIDA == 'S') {
                        dialogo += '<td class="col-md-1" contenteditable="false" id="input' + result[i].TIV_ID + '"> <input style="color:black;" type="text" id="' + result[i].TIV_ID + '" value=""></td>';
                        idsInput.push(result[i].TIV_ID);
                        posObsInput.push(i);
                    }
                    else {
                        dialogo += '<td class="col-md-1" id="comobox' + i + '"><select id="resultado' + i + '" style="color:black;"><option value="' + result[i].TIV_ID + '" style="color:black;">NA</option></select></td>';
                        pos.push(i);
                    }
                    dialogo += '<td class="col-md-6" contenteditable="false" style="' + cssInner + '" id="obs' + i + '"><input class="col-md-12" type="text" id="obsTexto' + i + '" style="color:black;"></td>';
                    dialogo += '       </tr>';
                }

                dialogo += '     </tbody>';
                dialogo += '     </table>';
                dialogo += '</div>';
                dialogo += ' </div>';
                dialogo += ' </div>';
                dialogo += ' </div>';

                dialogo += '<div class="modal-body">';
                dialogo += '     <table class="table table-bordered table-responsive-md  text-center" style="' + cssInner + '">';
                dialogo += '       <thead>';
                dialogo += '         <tr>';
                dialogo += '           <th class="text-center" style="' + cssInner + '"> Turno</th>';
                dialogo += '           <th class="text-center" style="' + cssInner + '">Turma</th>';
                dialogo += '           <th class="text-center" style="' + cssInner + '">Usuário</th>';
                dialogo += '         </tr>';
                dialogo += '       </thead>';
                dialogo += '       <tbody>';

                dialogo += '       <tr>';

                dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="turnos" style="width: 90px;color:black;"></select></td>';
                dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="turmas" style="width: 90px;color:black;"></select></td>';
                dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="usuarios" style="width: 90px;color:black;"></select></td>';
                dialogo += '       </tr>';


                dialogo += '     </tbody>';
                dialogo += '     </table>';
                dialogo += '</div>';
                dialogo += ' <div class="modal-footer">';
                dialogo += ' <button type="button" class="btn btn-default" onclick="fecharModal()">Cancelar</button>';
                dialogo += ' <button type="button" id="btnSalvar" onclick="salvarInspecoes()" class="btn btn-success">Salvar</button>';
                dialogo += '</div>';
                dialogo += ' </div>';
                dialogo += ' </div>';
                dialogo += ' </div>';


                addSelect('turnos', '/plugandplay/Qualidade/RetornarTurnos');
                addSelect('turmas', '/plugandplay/Qualidade/RetornarTurmas');
                addSelect('usuarios', '/plugandplay/Qualidade/RetornarUsuarios');

                $('#modalListarInspecoes').append(dialogo);
                $('#modalListarInspecoes').modal('show');
            } else { alert('Nenhuma inspeção visual cadastrada para este template de testes.'); }
        }, error: function (result) {
            alert(`Erro ao consultar Inspeçoes Visuais: ${result}`);
        }
    });

}

function fecharModal() {
    $('#modalListarInspecoes').modal('hide');
    Swal.fire({
        title: 'Deseja Cancelar?',
        text: "Os Testes Visuais devem ser coletados futuramente!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar!'
    }).then((result) => {
        if (result.value) {
            $('#ModalInspecoes').remove();
        } else {
            $('#modalListarInspecoes').modal('show');
        }
    })
}

function addSelect(id, urlData) {
    $.ajax({
        type: "get",
        url: urlData,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (obj) {
            if (obj != null) {
                var data = obj.dados;
                var selectbox = $('#' + id);
                selectbox.find('option').remove();
                $.each(data, function (i, d) {
                    $('<option style="color:black;">').val(d.Id).text(d.Descricao).appendTo(selectbox);
                });
            }
        }
    });
}

function salvarInspecoes() {

    var resultado = [];
    var obs = [];
    var ids = [];
    var valInput = [];

    for (var i = 0; i < pos.length; i++) {
        obs.push($('#obsTexto' + pos[i]).val());
        resultado.push($('#resultado' + pos[i]).find(":selected").text());
        ids.push($('#resultado' + pos[i]).find(":selected").val());
    }

    for (var i = 0; i < idsInput.length; i++) {
        valInput.push($('#' + idsInput[i]).val());
        obs.push($('#obsTexto' + posObsInput[i]).val());
    }

    var turno = $('#turnos').find(":selected").val();
    var turma = $('#turmas').find(":selected").val();
    var usuario = $('#usuarios').find(":selected").val();

    $('#modalListarInspecoes').modal('hide');
    Swal.fire({
        title: 'Deseja Salvar?',
        text: "Os Testes Visuais serão gravados!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Salvar!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "get",
                url: "/plugandplay/Qualidade/GravarResultadoInspecaoVisual",
                traditional: true,
                data: { medidaInsp: idsInput, valInput: valInput, idsInspecao: ids, Resultado: resultado, Obs: obs, Turno: turno, Turma: turma, User: usuario, Ord_Id: Ord_Id, Pro_Id: Pro_Id, Maq_id: Maq_Id, Seq_Trans: Seq_Trans, Seq_Rep: Seq_Rep },
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (obj) {
                    if (obj) {
                        Swal.fire("Gravado", "Os Testes Visuais foram gravados!", "success")
                        $('#ModalInspecoes').remove();
                    }
                }
            });
        } else {
            $('#modalListarInspecoes').modal('show');
        }
    })
}

function novaInspecao(ROT_MAQ_ID, ROT_PRO_ID, ORD_ID, FPR_SEQ_REPETICAO) {
    Maq_Id = ROT_MAQ_ID, Pro_Id = ROT_PRO_ID, Ord_Id = ORD_ID, Seq_Rep = FPR_SEQ_REPETICAO;
    var obj = '';
    //barra pesquisa 
    obj += '<div class="input-group" id="">';
    obj += '<input type="text" class="form-control" id="barrabusca" placeholder="Digite a pesquisa">';
    obj += '<span class="input-group-addon" id="" onclick="">';
    obj += '<button type="button" class="fa fa-search" id="btnpesq" onclick="ListarInspecoes()" style="background:transparent;border:none"></button>';
    obj += '</span>';
    obj += '</div>';
    obj += '</div>';
    $('#dadosInspecao').html(obj);
    $('#modalNovasInspecoes').modal('show');
}

function ListarInspecoes() {
    var path = "/PlugAndPlay/Qualidade/BuscarInspecoes";
    var itembusca = document.getElementById("barrabusca");
    $.ajax({
        url: path,
        type: "GET",
        data: {
            termopesquisado: itembusca.value
        },
        dataType: "json",
        success: function (resultado) {
            if (resultado != null) {
                var dialogo = "";
                dialogo += ' <div class="modal-body">';
                dialogo += '<div class="row">';
                dialogo += '<div class="col-md-12">';
                dialogo += ' <div class="col_md_12">';

                dialogo += '<table id="tabelaInspecoes" class="table table-bordered table-responsive-md  text-center" >';
                dialogo += '    <thead>';
                dialogo += '         <tr>';
                dialogo += '           <th class="text-center col-md-5" > Nome Técnico</th>';
                dialogo += '           <th class="text-center col-md-5" >Tolerâncias</th>';
                dialogo += '           <th class="text-center col-md-2" >Adicionar</th>';
                dialogo += '         </tr>';
                dialogo += '   </thead>';
                dialogo += '<tbody>';
                for (i = 0; i < resultado.length; i++) {
                    dialogo += '       <tr>';
                    dialogo += '<td class="col-md-5" contenteditable="false" id="NomeTecnico' + i + '" >' + resultado[i].TIV_NOME + '</td>';
                    dialogo += '<td class="col-md-5" contenteditable="false" id="tolerancia' + i + '" >' + resultado[i].TIV_DESCRICAO + '</td>';
                    dialogo += '<td class="col-md-2" contenteditable="false"  id="btnAdd' + i + '"><div class="col-sm"><a href="#" onclick="addNovaInspecao(' + resultado[i].TIV_ID + ')"><span title="Adicionar Teste visual" ><i style="color:green;font-size: 18px;" class="fa fa-plus fa-5x"></i></span></a></div></td>';
                    dialogo += '       </tr>';
                }

                dialogo += '</tbody>';
                dialogo += '</table>';
                dialogo += '<table class="table table-bordered table-responsive-md text-center" style="width:100%">'
                dialogo += '<thead class="thead-light">'
                dialogo += '<tr>'
                dialogo += '  <th style="width: 33%" class="text-center">TURNO</th>'
                dialogo += '  <th style="width: 33% " class="text-center">TURMA</th>'
                dialogo += '  <th style="width: 33%" class="text-center">USUARIO</th>'
                dialogo += '</tr>'
                dialogo += '</thead>'
                dialogo += '<tbody>'
                dialogo += '<tr>';
                dialogo += '<td><select id="turno" style="width: 100px;color:black;"><option></option></select></td>';
                dialogo += '<td><select id="turma" style="width: 100px;color:black;"><option></option></select></td>';
                dialogo += '<td><select id="usuario" style="width: 100px;color:black;"><option></option></select></td>';
                dialogo += '</tr>';
                dialogo += '</tbody>';
                dialogo += '</table>';
                dialogo += '</div>';
                dialogo += '</div>';
                dialogo += ' </div>';
                dialogo += ' </div>';

                addSelect('turno', '/plugandplay/Qualidade/RetornarTurnos');
                addSelect('turma', '/plugandplay/Qualidade/RetornarTurmas');
                addSelect('usuario', '/plugandplay/Qualidade/RetornarUsuarios');

                $('#dialogo').html(dialogo);

            } else { Swal.fire("Inspeção Visual", 'Nenhuma inspeção visual cadastrada', 'info'); }
        },
        error: function () {
            return "Erro";
        }
    });
}

function addNovaInspecao(id) {
    var itemturno = document.getElementById("turno");
    var Turno = itemturno.options[itemturno.selectedIndex].value;
    var itemturma = document.getElementById("turma");
    var Turma = itemturma.options[itemturma.selectedIndex].value;
    var itemuser = document.getElementById("usuario");
    var User = itemuser.options[itemuser.selectedIndex].value;
    $('#modalNovasInspecoes').modal('hide');
    Swal.fire({
        title: 'Deseja Salvar?',
        text: "O Teste Visual será gravado!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Salvar!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "get",
                url: "/plugandplay/Qualidade/AdicionarNovaInspecaoVisual",
                traditional: true,
                data: {
                    idInspecao: id, Ord_Id: Ord_Id, Pro_Id: Pro_Id, Maq_id: Maq_Id, Seq_Trans: Seq_Trans, Seq_Rep: Seq_Rep, turno: Turno, turma: Turma, usuario: User
                },
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (obj) {
                    if (obj) {
                        Swal.fire("Gravado", "O Testes Visual foi adicionado!", "success").then((result) => { $('#modalNovasInspecoes').modal('show'); });
                    }
                }
            });
        } else {
            $('#modalNovasInspecoes').modal('show');
        }
    });
}