function abrirModalAmostras(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, TOTCOL, TEM_ID) {
    var isVisible = $("#dados").is(":visible");
    if (!isVisible) {
        var strJs = `salvarTesteFisico('${ORD_ID}','${ROT_PRO_ID}','${ROT_MAQ_ID}','${FPR_SEQ_REPETICAO}','${ROT_SEQ_TRANSFORMACAO}','${TOTCOL}','${TEM_ID}')`;
        var btn = '';
        var obj = '<div id="TestesOperacao">'
            + '<table class="table table-bordered table-responsive-md text-center" style="width:100%">'
            + '<thead class="thead-light">'
            + '<tr>'
            + '  <th style="width: 12% " class="text-center">Nº MÍN. DE AMOSTRAS</th>'
            + '  <th style="width: 12%" class="text-center">RECOLHIDA</th>'
            + '  <th style="width: 50%" class="text-center">OBSERVAÇÃO</th>'
            + '</tr>';
        +'</thead>'
            + '<tbody>'
            + '<tr>';
        obj += '<td>' + (TOTCOL) + ' </td>';
        obj += '<td>  <input type="checkbox" checked  value="' + TOTCOL + '" onchange="evento(this);" style="width: 100%" name="coletados" id="coletados" /> </td>';
        obj += '<td>  <input type="text" style="width: 100%"  disabled="1" name="observacao" id="observacao" /> </td>';
        obj += '</tr>';

        obj += '</tbody>';
        obj += '</table>';
        obj += '<table class="table table-bordered table-responsive-md text-center" style="width:100%">'
        obj += '<thead class="thead-light">'
        obj += '<tr>'
        obj += '  <th style="width: 33%" class="text-center">TURNO</th>'
        obj += '  <th style="width: 33% " class="text-center">TURMA</th>'
        obj += '  <th style="width: 33%" class="text-center">USUARIO</th>'
        obj += '</tr>'
        obj += '</thead>'
        obj += '<tbody>'
        obj += '<tr>';
        obj += '<td><select id="turno" style="width: 100px;color:black;"><option></option></select></td>';
        obj += '<td><select id="turma" style="width: 100px;color:black;"><option></option></select></td>';
        obj += '<td><select id="usuario" style="width: 100px;color:black;"><option></option></select></td>';
        obj += '</tr>';
        obj += '</tbody>';
        obj += '</table>';
        obj += '</div>';
        btn += '<button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>'
        btn += '<button type="button" id="btnSalvar" onclick="' + strJs + '" class="btn btn-success">Salvar</button>'
        $('#dados').html(obj);
        $('#btnBar').html(btn);
    }

    addSelect('turno', '/plugandplay/Qualidade/RetornarTurnos');
    addSelect('turma', '/plugandplay/Qualidade/RetornarTurmas');
    addSelect('usuario', '/plugandplay/Qualidade/RetornarUsuarios');

    $('#modalIniciarTestes').modal('show');
}


$("#modalIniciarTestes").on("hide.bs.modal", function () {

    Swal.fire({
        title: 'Deseja Cancelar?',
        text: "As amostras devem ser coletadas futuramente!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar!'
    }).then((result) => {
        if (result.value) {
            Swal.fire({
                title: 'Cancelado',
                text: "Colete as amostras posteriormente",
                icon: 'info',
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'OK!'
            }).then((result) => {
                if (result.value) {
                    $('#TestesOperacao').remove();
                }
            })
        } else {
            $('#modalIniciarTestes').modal('show');
        }
    })
});

function evento(div) {
    var qtde = $(div).prop("checked")
    if (!qtde)
        $("#observacao").prop("disabled", false);
    else
        $("#observacao").prop("disabled", true);
}

function salvarTesteFisico(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, TOTCOL, TEM_ID) {

    var Obs = '';
    var Coletado = $('#coletados').prop("checked");
    if (!Coletado) {
        Obs += $('#observacao').val();
    }
    var itemturno = document.getElementById("turno");
    var Turno = itemturno.options[itemturno.selectedIndex].value;
    var itemturma = document.getElementById("turma");
    var Turma = itemturma.options[itemturma.selectedIndex].value;
    var itemuser = document.getElementById("usuario");
    var User = itemuser.options[itemuser.selectedIndex].value;

    $('#modalIniciarTestes').modal('hide');
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
            if (result.value) {
                salvarTestes1(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, Coletado, Turno, Turma, User, Obs, TEM_ID);
                Swal.fire({
                    title: 'Gravado',
                    text: "Os Testes foram gravados!",
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',

                    confirmButtonText: 'OK!'
                }).then((result) => {
                    if (result.value) {
                        $('#TestesOperacao').remove();
                        alterarDadosButtonTeste(TOTCOL);
                    }
                })
            } else {
                $('#modalIniciarTestes').modal('show');
            }
        }
    })
}


function salvarTestes1(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, Coletado, Turno, Turma, User, Obs, TEM_ID) {
    var path = "/PlugAndPlay/Medicoes/ColetarTestesFisicos";
    $.ajax({
        url: path,
        type: "GET",
        data: {
            ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, Coletado, Turno, Turma, User, Obs, TEM_ID
        },
        dataType: "json",
        error: function (resultado) {
            return "Erro:" + resultado;
        }
        
    });
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
                    $('<option>').val(d.Id).text(d.Descricao).appendTo(selectbox);
                });
            }
        }
    });
}

function VerificarTesteNaoColetado(order, proId, maqId, seqRep, seqTran) {
    var path = "/plugandplay/Qualidade/VerificaTesteNaoColetado";
    $.ajax({
        url: path,
        type: "get",
        data: {
            Ord_Id: order, Prod_Id: proId, Maq_Id: maqId, Seq_Rep: seqRep, Seq_Trans: seqTran
        },
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (obj) {
            if (obj == true) {
                swal.fire("IMPORTANTE", "Recolha as Amostras pendentes!", "info");
            }
        }
    });
}


/*--------------------------------------------Teste da BOBINA-----------------------------------------------------------*/

function TestesBobinas(ORD_ID, ROT_PRO_ID) {
    var path = "/PlugAndPlay/Medicoes/ObterTiposTestesNAmostras";
    idOrd = ORD_ID, idPro = ROT_PRO_ID;

    $.ajax({
        url: path,
        type: "GET",
        data: { ORD_ID, ROT_PRO_ID },
        dataType: "json",
        success: function (result) {//contem o q retorna do json do controller
            if (result.tipo.length !== null && result.tipo.length > 0) {
                TestesAGravar = result.tipo;
                NAmostras = result.numerominamostras;
                let dados = result;
                var obj = '<div id="TestesBobinas">'
                    + '<table class="table table-bordered table-responsive-md text-center" style="width:100%">'
                    + '<thead class="thead-light">'
                    + '<tr>'
                    + '  <th style="width: 14%" class="text-center">TIPO DE TESTE</th>'
                    + '  <th style="width: 12% " class="text-center">Nº MÍN. DE AMOSTRAS</th>'
                    + '  <th style="width: 12%" class="text-center">COLETADA</th>'
                    + '  <th style="width: 12%" class="text-center">QUANTIDADE</th>'
                    + '  <th style="width: 50%" class="text-center">OBSERVAÇÃO</th>'
                    + '</tr>';
                +'</thead>'
                    + '<tbody>'
                dados.tipo.forEach(function (m) {
                    + '<tr>';
                    obj += '<td>' + m.TT_NOME + ' </td>';
                    obj += '<td>' + dados.numerominamostras + ' </td>';
                    obj += '<td> <input type="checkbox" name="opcao"  id="opcao' + m.TT_ID + '" /> </td>';
                    obj += '<td>  <input type="number" min="1" value="' + dados.numerominamostras + '" onchange="evento(this,' + m.TT_ID + ');" style="width: 100%" name="coletados" id="coletados' + m.TT_ID + '" /> </td>';
                    obj += '<td>  <input type="text" style="width: 100%"  disabled="1" name="observacao" id="observacao' + m.TT_ID + '" /> </td>';

                    obj += '</tr>';
                })
                obj += '</tbody>';
                obj += '</table>';
                obj += '<table class="table table-bordered table-responsive-md text-center" style="width:100%">'
                obj += '<thead class="thead-light">'
                obj += '<tr>'
                obj += '  <th style="width: 33%" class="text-center">TURNO</th>'
                obj += '  <th style="width: 33% " class="text-center">TURMA</th>'
                obj += '  <th style="width: 33%" class="text-center">USUARIO</th>'
                obj += '</tr>'
                obj += '</thead>'
                obj += '<tbody>'
                obj += '<tr>';
                obj += '<td><select id="turno" style="width: 100px;color:black;"><option></option></select></td>';
                obj += '<td><select id="turma" style="width: 100px;color:black;"><option></option></select></td>';
                obj += '<td><select id="usuario" style="width: 100px;color:black;"><option></option></select></td>';
                obj += '</tr>';
                obj += '</tbody>';
                obj += '</table>';
                obj += '</div>';
                $('#testesFisico').append(obj);
            }
            else { return 'Não foram encontrados testes'; }

            addSelect('turno', '/plugandplay/Qualidade/RetornarTurnos');
            addSelect('turma', '/plugandplay/Qualidade/RetornarTurmas');
            addSelect('usuario', '/plugandplay/Qualidade/RetornarUsuarios');
        },
        error: function () {
            return "Erro nao tratado";
        }
    });

}

function salvarTesteBobina() {

    var TestesIds = ''; var QtdeColetada = '';
    TestesIdsNaoColetados = '';
    QtdNaoColetada = '';
    var Obs = '';
    var len = TestesAGravar.length;
    for (let i = 0; i < len; i++) {
        var checkValue = $('#opcao' + TestesAGravar[i].TT_ID + '');
        if (checkValue.prop("checked")) {
            TestesIds += TestesAGravar[i].TT_ID;
            if ($('#coletados' + TestesAGravar[i].TT_ID).val() != '') {
                QtdeColetada += $('#coletados' + TestesAGravar[i].TT_ID).val();
                if ($('#coletados' + TestesAGravar[i].TT_ID).val() != NAmostras)
                    Obs += $('#observacao' + TestesAGravar[i].TT_ID).val();
                else Obs += ' ';
            }
            if ($('#coletados' + TestesAGravar[i].TT_ID).val() == '') {
                QtdeColetada += NAmostras;
                Obs += ' ';
            }
        } else {
            TestesIdsNaoColetados += TestesAGravar[i].TT_ID;
            QtdNaoColetada += NAmostras;
        }
        if (i < len - 1) {
            TestesIdsNaoColetados += ';';
            QtdNaoColetada += ';';
            TestesIds += ';';
            QtdeColetada += ';';
            Obs += ';';
        }
    }
    var itemturno = document.getElementById("turno");
    var Turno = itemturno.options[itemturno.selectedIndex].value;
    var itemturma = document.getElementById("turma");
    var Turma = itemturma.options[itemturma.selectedIndex].value;
    var itemuser = document.getElementById("usuario");
    var User = itemuser.options[itemuser.selectedIndex].value;

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
            if (result.value) {
                var steste = TestesIds.split(';');
                if (steste.length > 0)
                    salvarTestes(idOrd, idPro, "", "", "", TestesIds, QtdeColetada, Turno, Turma, User, Obs, true);//salva os testes coletados
                steste = TestesIdsNaoColetados.split(';');
                if (steste.length > 0)
                    salvarTestes(idOrd, idPro, "", "", "", TestesIdsNaoColetados, QtdNaoColetada, Turno, Turma, User, Obs, false);// salva os testes nao coletados

                Swal.fire({
                    title: 'Gravado',
                    text: "Os Testes foram gravados!",
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'OK!'
                }).then((result) => {
                    if (result.value) {
                        // fechar a JANELA
                        window.close();
                    }
                })
            }
        }
    })
}


function Cancelar() {
    Swal.fire({
        title: 'Cancelar',
        text: "Deseja Cancelar!",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'OK!'
    }).then((result) => {
        if (result.value) {
            // fechar a JANELA
            window.close();
        }
    })
}

function VerificarHaTestes(ORD_ID, ROT_PRO_ID) {
    var path = "/plugandplay/Qualidade/VerificarHaTestesFeitos";
    $.ajax({
        url: path,
        type: "get",
        data: {
            ord: ORD_ID, prod: ROT_PRO_ID
        },
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (obj) {
            if (obj == true) {

                Swal.fire({
                    title: 'Já Existe Testes Coletados',
                    text: "",
                    icon: 'info',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'OK!'
                }).then((result) => {
                    if (result.value) {
                        // fechar a JANELA
                        window.close();
                    }
                })
            }
        }
    });
}


/*--------------------------------------------Funções da Tela do Operador----------------------------------------------*/

function alterarDadosButtonTeste(pos) {
    var textoBotao = $(`#btnColetas_${pos}`);
    var splited = textoBotao.text().split('/');
    var testesRealizados = parseInt(splited[0].split(':')[1].trim());
    testesRealizados++;
    var texto = `Testes: ${testesRealizados}/ ${splited[1]}`;
    textoBotao.text(texto);
}

function modalRecolherAmostras(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, TOTCOL, TEM_ID, REALIZADOS, TOTAL) {
    var textoBotao = $(`#btnColetas_${TOTCOL}`);
    var splited = textoBotao.text().split('/');
    var testesRealizados = parseInt(splited[0].split(':')[1].trim());
    REALIZADOS = (REALIZADOS > testesRealizados) ? REALIZADOS : testesRealizados;
    if (TOTAL > 0 && REALIZADOS < TOTAL) {
        if (TEM_ID > 0) {
            abrirModalAmostras(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, TOTCOL, TEM_ID);
        } else {
            alert('Não há nenhum teste (Template) associado a este Grupo de Produto, Roteiro ou Máquina.');
        }
    } else {
        alert('Todos as coletas para Testes já foram realizadas.');
    }
}

