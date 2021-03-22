// variaveis para salvar os tipos testes
var IdOrd, IdPro, IdMaq, SeqRep, SeqTrans;
var Tem_id = '';
var quantidadeMinima = '';
var IdVisuais = '';
var Textomensagem = '', Textotipo = '', Textoids = '';

var idsInput = [];
var posObsInput = [];
var pos = [];
var idsInsp = [];

function DialogoTesteAmostras(mensagem, tipo, ids) {
    Textomensagem = mensagem;
    Textotipo = tipo;
    Textoids = ids;
    var msg = mensagem.split(";");

    var titulo = msg[0];
    quantidadeMinima = msg[1];
    Tem_id = msg[2];
    var ids = ids.split(";");
    var infoMaquina = ids[0].split(",");  // ORD_ID, ROT_PRO_ID, MAQ_ID, FPR_SEQ_REPETICAO, FPR_SEQ_TRANFORMACAO

    var InspecaoVisual = ids[1].split(",");

    idsInput = [];
    posObsInput = [];
    pos = [];
    idsInsp = [];

    PegarTurnos();
    PegarTurmas();
    PegarUsuarios();

    IdOrd = infoMaquina[0];
    IdPro = infoMaquina[1];
    IdMaq = infoMaquina[2];
    SeqRep = infoMaquina[3];
    SeqTrans = infoMaquina[4];

    // se não setar o tipo, o padrão é alert-info
    if (!tipo)
        var tipo = "info";

    // monta o css da mensagem para que fique flutuando na frente de todos elementos da página
    var cssMessage = "display: block; position: fixed; top: 0; left: 20%; right: 20%; width: 60%; padding-top: 10px; z-index: 9999; word-wrap: break-word; color: black;";
    var cssInner = "margin: 0 auto; box-shadow: 1px 1px 5px black; word-wrap: break-word; color:black;";



    // monta o html da mensagem com Bootstrap
    var dialogo = "";

    dialogo += '<div id="ModalAmostras" style="' + cssMessage + '">';
    dialogo += '    <div class="alert alert-' + tipo + ' alert-dismissable" style="' + cssInner + '">';
    dialogo += ' <div class="modal-content" style="' + cssInner + '">';
    dialogo += '<div class="modal-header" style="' + cssInner + '">';
    dialogo += '<h4 class="modal-title">' + titulo + '</h4>';
    dialogo += '</div>';
    dialogo += ' <div class="modal-body">';
    dialogo += '<div class="row">';
    dialogo += '<div class="col-md-12">';
    dialogo += ' <div class="col_md_12">';

    dialogo += '     <table class="table table-bordered table-responsive-md  text-center" style="' + cssInner + '">';
    dialogo += '       <thead>';
    dialogo += '         <tr>';
    dialogo += '           <th class="text-center col-md-1" style="' + cssInner + '">Qtd. Mín. Amostras</th>';
    dialogo += '           <th class="text-center col-md-1" style="' + cssInner + '">Coletada</th>';
    dialogo += '           <th class="text-center col-md-7" style="' + cssInner + '">OBS</th>';
    dialogo += '         </tr>';
    dialogo += '       </thead>';
    dialogo += '       <tbody>';

    dialogo += '       <tr>';
    dialogo += '<td class="col-md-1" contenteditable="false" id="qtdAmostra' + i + '" style="' + cssInner + '">' + quantidadeMinima + '</td>';
    dialogo += '<td class="col-md-1"><input type="checkbox" onchange="desabilitarOBS()" id="Coletada" checked></td>';
    dialogo += '<td class="col-md-7" contenteditable="false" style="' + cssInner + '" id="obs"><input class="col-md-12" type="text" id="obsTexto" style="color:black;" disabled></td>';
    dialogo += '       </tr>';


    dialogo += '     </tbody>';
    dialogo += '     </table>';
    dialogo += '</div>';
    dialogo += ' </div>';
    dialogo += ' </div>';
    dialogo += ' </div>';
    //Inspecoes Visuais
    if (InspecaoVisual.length - 1 > 0) {
        dialogo += '<div class="modal-body">';
        dialogo += '     <table class="table table-bordered table-responsive-md  text-center" style="' + cssInner + '">';
        dialogo += '       <thead>';
        dialogo += '         <tr>';
        dialogo += '           <th class="text-center col-md-2" style="' + cssInner + '"> Nome Técnico</th>';
        dialogo += '           <th class="text-center col-md-3" style="' + cssInner + '">Tolerâncias</th>';
        dialogo += '           <th class="text-center col-md-1" style="' + cssInner + '">Resultado</th>';
        dialogo += '           <th class="text-center col-md-6" style="' + cssInner + '">OBS</th>';
        dialogo += '         </tr>';
        dialogo += '       </thead>';
        dialogo += '       <tbody>';

        for (var i = 0; i < InspecaoVisual.length - 1; i++) {
            var aux = InspecaoVisual[i].split(":"); // 0 = nome, 1 = descricao, 2 = ID, 3 = Medida(S/N)
            dialogo += '       <tr>';
            dialogo += '<td class="col-md-2" contenteditable="false" id="NomeTecnico' + i + '" style="' + cssInner + '">' + aux[0] + '</td>';
            dialogo += '<td class="col-md-3" contenteditable="false" id="tolerancia' + i + '" style="' + cssInner + '">' + aux[1] + '</td>';
            if (aux[3] == 'S') {
                dialogo += '<td class="col-md-1" contenteditable="false" id="td' + aux[2] + '"> <input style="color:black;" type="text" id="input' + aux[2] + '" value=""></td>';
                idsInput.push(aux[2]);
                posObsInput.push(i);
            }
            else {
                dialogo += '<td class="col-md-1" id="comobox' + i + '"><select id="resultado' + i + '" style="color:black;"><option value="' + aux[2] + '" style="color:black;">NA</option></select></td>';
                pos.push(i);
                idsInsp.push(aux[2]);
            }
            dialogo += '<td class="col-md-6" contenteditable="false" style="' + cssInner + '" id="obs' + i + '"><input class="col-md-12" type="text" id="obsTexto' + i + '" style="color:black;"></td>';
            dialogo += '       </tr>';
        }

        dialogo += '     </tbody>';
        dialogo += '     </table>';
        dialogo += '</div>';
    }
    //dados de quem salva os testes
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
    dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="turno" style="width: 90px;color:black;"></select></td>';
    dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="turma" style="width: 90px;color:black;"></select></td>';
    dialogo += '<td contenteditable="false" style="' + cssInner + '"><select id="usuario" style="width: 90px;color:black;"></select></td>';
    dialogo += '       </tr>';


    dialogo += '     </tbody>';
    dialogo += '     </table>';
    dialogo += '</div>';
    dialogo += ' <div class="modal-footer">';
    dialogo += ' <button type="button" class="btn btn-default" data-dismiss="alert">Cancelar</button>';
    dialogo += ' <button type="button" id="btnSalvar" onclick="salvarTeste()" class="btn btn-success">Salvar</button>';
    dialogo += '</div>';
    dialogo += ' </div>';
    dialogo += ' </div>';
    dialogo += ' </div>';

    // adiciona ao body a mensagem com o efeito de fade
    $("body").append(dialogo);
}

$("body").on('close.bs.alert', function () {

    Swal.fire({
        title: 'Deseja Cancelar?',
        text: "Os Testes devem ser coletados futuramente!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar!'
    }).then((result) => {
        if (result.value) {
            salvarTestes(IdOrd, IdPro, IdMaq, SeqRep, SeqTrans, "A", "A", 0, "", false, Tem_id);
            salvarInspecoes(idsInput, [], idsInsp, [], [], "A", "A", 0);
            Swal.fire('Cancelado!', 'Colete os Testes posteriormente', 'info');
        } else {
            DialogoTesteAmostras(Textomensagem, Textotipo, Textoids);
        }
    })
});

function desabilitarOBS() {
    if ($("#Coletada").is(":checked"))
        $('#obsTexto').attr("disabled", true);
    else
        $('#obsTexto').attr("disabled", false);
}

function salvarTeste() {

    var obs = '';
    var bool = false;

    if (!$("#Coletada").is(":checked")) {

        if ($('#obsTexto').val() == "") {
            $('#obsTexto').focus();
            return;
        }
        else
            obs = $('#obsTexto').val();
    } else
        bool = true;

    if ($('#turno').find(":selected").text() == "...") {
        $('#turno').focus().select();
        return;
    }
    if ($('#turma').find(":selected").text() == "...") {
        $('#turma').focus().select();
        return;
    }
    if ($('#usuario').find(":selected").text() == "...") {
        $('#usuario').focus().select();
        return;
    }

    //---------------------Inspeções visuais---------------------------

    var resultado = [];
    var obsInsp = [];
    var ids = [];
    var valInput = [];

    for (var i = 0; i < pos.length; i++) {
        obsInsp.push($('#obsTexto' + pos[i]).val());
        resultado.push($('#resultado' + pos[i]).find(":selected").text());
        ids.push($('#resultado' + pos[i]).find(":selected").val());
    }

    for (var i = 0; i < idsInput.length; i++) {
        valInput.push($('#input' + idsInput[i]).val());
        obsInsp.push($('#obsTexto' + posObsInput[i]).val());
    }

    var turno = $('#turno').find(":selected").text();
    var turma = $('#turma').find(":selected").text();
    var user = $('#usuario').find(":selected").val();

    $('#ModalAmostras').remove();

    Swal.fire({
        title: 'Deseja Salvar?',
        text: "Os Testes serão gravados!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Salvar!'
    }).then((result) => {
        if (result.value) {
            salvarTestes(IdOrd, IdPro, IdMaq, SeqRep, SeqTrans, turno, turma, user, obs, bool, Tem_id);
            salvarInspecoes(idsInput, valInput, ids, resultado, obsInsp, turno, turma, user);
            Swal.fire("Gravado", "Os Testes foram gravados!", "success");
        } else {
            DialogoTesteAmostras(Textomensagem, Textotipo, Textoids);
        }
    })
}

function salvarInspecoes(idsInput, valInput, ids, resultado, obs, turno, turma, usuario) {
    $.ajax({
        type: "get",
        url: "/plugandplay/Qualidade/GravarResultadoInspecaoVisual",
        traditional: true,
        data: { medidaInsp: idsInput, valInput: valInput, idsInspecao: ids, Resultado: resultado, Obs: obs, Turno: turno, Turma: turma, User: usuario, Ord_Id: IdOrd, ProId: IdPro, Maq_id: IdMaq, Seq_Trans: SeqTrans, Seq_Rep: SeqRep },
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (obj) {
        }
    });
}

function salvarInspecao(idOrd, idPro, idMaq, seqRep, seqTrans, Turno, Turma, User, coletado, Tem_id) {

    var path = "/PlugAndPlay/Qualidade/GerarAmostrasInspecaoVisual";
    $.ajax({
        url: path,
        type: "GET",
        data: {
            idOrd: idOrd, idPro: idPro, idMaq: idMaq, seqRep: seqRep, seqTrans: seqTrans, Turno: Turno, Turma: Turma, User: User, Obs: Obs, coletado: coletado, Tem_id: Tem_id
        },
        dataType: "json",
        success: function (resultado) {
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function salvarTestes(idOrd, idPro, idMaq, seqRep, seqTrans, Turno, Turma, User, Obs, coletado, Tem_id) {

    var path = "/PlugAndPlay/Qualidade/GerarAmostrasTesteFisico";
    $.ajax({
        url: path,
        type: "GET",
        data: {
            idOrd: idOrd, idPro: idPro, idMaq: idMaq, seqRep: seqRep, seqTrans: seqTrans, Turno: Turno, Turma: Turma, UserL: User, Obs: Obs, coletado: coletado, Tem_id: Tem_id
        },
        dataType: "json",
        success: function (resultado) {
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function PegarUsuarios() {

    var path = "/plugandplay/Qualidade/RetornarUsuarios";

    $.ajax({
        url: path,
        type: "GET",
        data: {},
        dataType: "json",
        success: function (resultado) {
            if (resultado != null) {
                $('#usuario').append('<option value="default" style="color:black;">...</option>');
                for (i = 0; i < resultado.dados.length; i++) {
                    $('#usuario').append('<option value="' + resultado.dados[i].Id + '" style="color:black;">' + resultado.dados[i].Descricao + '</option>');
                }
            }
        },
        error: function () {
            return "Erro não tratado";
        }
    });
}

function PegarTurnos() {

    var path = "/plugandplay/Qualidade/RetornarTurnos";

    $.ajax({
        url: path,
        type: "GET",
        data: {},
        dataType: "json",
        success: function (resultado) {
            if (resultado != null) {
                $('#turno').append('<option value="default" style="color:black;">...</option>');
                for (i = 0; i < resultado.dados.length; i++) {
                    $('#turno').append('<option value="' + i + '" style="color:black;">' + resultado.dados[i].Id + '</option>');
                }
            }
        },
        error: function () {
            return "Erro não tratado";
        }
    });
}

function PegarTurmas() {
    var path = "/plugandplay/Qualidade/RetornarTurmas";

    $.ajax({
        url: path,
        type: "GET",
        data: {},
        dataType: "json",
        success: function (resultado) {
            if (resultado != null) {
                $('#turma').append('<option value="default" style="color:black;">...</option>');
                for (i = 0; i < resultado.dados.length; i++) {
                    $('#turma').append('<option value="' + i + '" style="color:black;">' + resultado.dados[i].Id + '</option>');
                }
            }
        },
        error: function () {
            return "Erro não tratado";
        }
    });
}

