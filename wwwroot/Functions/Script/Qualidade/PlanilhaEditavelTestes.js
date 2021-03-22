var idOrd, idPro, idMaq, seqRep;
function PlanilhaTestesOP(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, DATA, INDEX) {
    var path = "/plugandplay/Qualidade/ObterTestesFisicosPorOP";
    var isVisible = $("#testesFisicos" + INDEX).is(":visible");
    $("#tabelaTesteEdital").remove();
    $("#tabelaTesteVisualEdital").remove();
    if (!isVisible) {
        $("div").removeClass('in');
        $("#collapse" + INDEX).addClass('in');
        //$("#testesFisicos" + INDEX).addClass('in');
        $.ajax({
            url: path,
            type: "GET",
            data: { ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, DATA },
            dataType: "json",
            success: function (result) {
                if (result != null) {
                    var divListaTestes = $('#testesFisicos' + INDEX);
                    let dados = result._lista;
                    let amostras = result._listaAmostras;
                    var cont = 0;
                    var maior = 0;
                    var _GrupoTeste = '';
                    //Head
                    _GrupoTeste += '<div class="card" id="tabelaTesteEdital">';
                    _GrupoTeste += ' <div class="card-body">';
                    _GrupoTeste += '   <div id="table" class="table-editable table-responsive bootgrid-wrapper " style="overflow: auto;">';
                    _GrupoTeste += '     <table id="tabelaTestes" class="table table-bordered table-responsive-md table-striped text-center tabelaEditavel">';
                    _GrupoTeste += '       <thead style ="overflow: auto;">';
                    _GrupoTeste += '         <tr>';
                    _GrupoTeste += '           <th class="text-center" > NOME_TECNICO</th>';

                    for (var i = 0; i < amostras.length; i++) {
                        if (amostras[i].length > maior) {
                            maior = amostras[i].length;
                        }
                    }

                    for (var i = 0; i < maior; i++) {

                        if (i < 10)
                            _GrupoTeste += '<th class="text-center" id ="Amostra' + (i + 1) + '">Amostra: 0' + (i + 1) + '</th>';
                        else
                            _GrupoTeste += '<th class="text-center" id ="Amostra' + (i + 1) + '">Amostra: ' + (i + 1) + '</th>';

                    }
                    _GrupoTeste += '           <th class="text-center">Status</th>';
                    _GrupoTeste += '         </tr>';
                    _GrupoTeste += '       </thead>';
                    //Body
                    _GrupoTeste += '       <tbody id="PlanilhaTestes">';

                    cont = 1;
                    var j = 0;
                    var auxTes_Id = '';
                    var auxValues = '';

                    dados.forEach(function (item) {
                        _GrupoTeste += '<tr id="Linha' + cont + '">';
                        _GrupoTeste += '           <td class="pt-3-half" contenteditable="false" id="' + item.TES_NOME_TECNICO + '" title="' + item.TES_NOME_TECNICO + '">' + item.TES_NOME_TECNICO + '</td>';
                        for (var i = 0; i < maior; i++)
                            if (i < amostras[j].length) {
                                var estilo = (amostras[j][i].TES_STATUS_LIBERACAO == "AMOSTRA_NAO_COLETADA") ? "naoColetado" : "";
                                _GrupoTeste += '           <td class="pt-3-half ' + estilo + '" title="Data Emissão: ' + amostras[j][i].TES_EMISSAO + '" contenteditable="false" id="' + amostras[j][i].TES_ID + '">' + amostras[j][i].TES_VALOR_NUMERICO + '</td>';
                                auxTes_Id += amostras[j][i].TES_ID + ';'; // contem os tes_id para salvar depois
                                auxValues += amostras[j][i].TES_VALOR_NUMERICO + ';';
                            }
                            else
                                _GrupoTeste += '           <td class="pt-3-half" contenteditable="false" id="desabilitado"></td>';

                        _GrupoTeste += '           <td class="pt-3-half" contenteditable="false" title="Status" id="Status' + cont + '">';

                        _GrupoTeste += '<i id="ImgStatus' + cont + '" title="Status" class=""></i>';

                        _GrupoTeste += '</td>'
                        _GrupoTeste += '         </tr>';
                        Resultado(auxTes_Id, auxValues, cont);
                        j++;
                        cont++;
                        auxTes_Id = '';
                        auxValues = '';
                    })

                    _GrupoTeste += '     </tbody>';
                    _GrupoTeste += '     </table>';
                    _GrupoTeste += '   </div>';
                    _GrupoTeste += '</div>';
                    _GrupoTeste += ' </div>';
                    _GrupoTeste += '</div>';


                    divListaTestes.append(_GrupoTeste);
                    dbclick();
                }
            },
            error: function () {
                return "Erro nao tratado";
            }
        });
    } else {
        $("#tabelaTesteEdital" + INDEX).remove();
    }
}

function Resultado(tes_ids, tes_values, cont) {
    var path = "/plugandplay/Qualidade/GerarStatus";
    $.ajax({
        url: path,
        type: "GET",
        traditional: true,
        data: { tes_id: tes_ids, tes_value: tes_values },
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $("#ImgStatus" + cont).removeClass('fa fa-times-circle');
                $("#ImgStatus" + cont).addClass('fa fa-check-circle');
                $("#ImgStatus" + cont).css({ "color": "green" });
            }
            else {
                $("#ImgStatus" + cont).removeClass('fa fa-check-circle');
                $("#ImgStatus" + cont).addClass('fa fa-times-circle');
                $("#ImgStatus" + cont).css({ "color": "red" });
            }
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function salvarTesteEditado(tes_ids, tes_values) {
    var path = "/plugandplay/Qualidade/GravarTesteFisico";

    $.ajax({
        url: path,
        type: "GET",
        traditional: true,
        data: { tes_id: tes_ids, valTes_id: tes_values },
        dataType: "json",
        success: function (resultado) {
            if (resultado !== null) {
                //alert('Alterações gravadas!');
            }
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}


function abrirModalAmostras(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO) {
    var path = "/plugandplay/Qualidade/ObterTestes";
    idOrd = ORD_ID, idPro = ROT_PRO_ID, seqRep = FPR_SEQ_REPETICAO;
    $.ajax({
        url: path,
        type: "GET",
        data: { ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO },
        dataType: "json",
        success: function (result) {
            if (result.tipo.lenght !== null) {
                let dados = result.tipo;
                var obj = '';
                dados.forEach(function (m) {
                    obj += '<option id="cbteste" value=' + m.TT_ID + '>' + m.TT_NOME + '</option>';
                })
                $('#dados').html(obj);
                var campo = '';
                campo += 'Nº de amostras: <input id="txtamostra" type=text style="width:50px">';
                $('#amostras').html(campo);
            }
            else { return 'Não foram encontrados testes'; }
            $('#modalNovasAmostras').modal('show');
        },
        error: function () {
            return "Erro nao tratado";
        }
    });
}

function dbclick() {
    $(function () {
        $("td").dblclick(function () {

            var conteudoOriginal = $(this).text();

            var id = $(this).attr("id"); // tes_id atual selecionado
            var pai = $("td#" + id).parent(); // pega o Pai da <td> atual
            var filhos = $("tr#" + pai.attr("id")).children('td'); // retornas todos os <td> quem contem na <tr>
            var path = "/plugandplay/Qualidade/GerarStatus";

            if (id != "desabilitado" && id != filhos.get(0).id && id != filhos.get(filhos.length - 1).id) {
                $(this).removeClass("naoColetado");
                $(this).addClass("celulaEmEdicao");
                $(this).html("<input type='text' value='" + conteudoOriginal + "' />");
                $(this).children().first().focus();

                $(this).children().first().blur(function () {
                    var novoConteudo = $(this).val();
                    $(this).parent().text(novoConteudo);
                    $(this).parent().removeClass("celulaEmEdicao");

                    var tes_ids = "";
                    var tes_values = "";
                    for (var i = 1; i < filhos.length - 1; i++) {
                        if (filhos.get(i).id != "desabilitado") {
                            tes_ids += filhos.get(i).id + ";";
                            tes_values += filhos.get(i).textContent + ";";
                        }
                    }

                    $.ajax({
                        url: path,
                        type: "GET",
                        traditional: true,
                        data: { tes_id: tes_ids, tes_value: tes_values },
                        dataType: "json",
                        success: function (result) {
                            if (result == true) {
                                $("#Img" + filhos.get((filhos.length - 1)).id).removeClass('fa fa-times-circle');
                                $("#Img" + filhos.get((filhos.length - 1)).id).addClass('fa fa-check-circle');
                                $("#Img" + filhos.get((filhos.length - 1)).id).css({ "color": "green" });
                            }
                            else {
                                $("#Img" + filhos.get((filhos.length - 1)).id).removeClass('fa fa-check-circle');
                                $("#Img" + filhos.get((filhos.length - 1)).id).addClass('fa fa-times-circle');
                                $("#Img" + filhos.get((filhos.length - 1)).id).css({ "color": "red" });
                            }
                            salvarTesteEditado(tes_ids, tes_values);
                        },
                        error: function () {
                            return "Erro nao tratado";
                        }
                    });
                });
            }
        });
    });

}

function salvarAmostra() {
    var path = "/plugandplay/Qualidade/GravarNovoTesteFisico";
    var item = document.getElementById("dados");
    var TesteId = item.options[item.selectedIndex].value;
    var NAmostra = $("#txtamostra").val();

    $('#modalNovasAmostras').modal('hide');
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
            $.ajax({
                url: path,
                type: "GET",
                data: {
                    idOrd: idOrd, idPro: idPro, seqRep: seqRep, TesteId: TesteId, QtdeColetada: NAmostra
                },
                dataType: "json",
                success: function (resultado) {
                    if (resultado !== null) {

                        Swal.fire("Gravado", "Os Testes foram gravados!", "success")
                    }
                },
                error: function () {
                    return "Erro nao tratado";
                }
            });
            $('#modalNovasAmostras').modal('hide');
        } else {
            $('#modalNovasAmostras').modal('show');
        }
    })


}
