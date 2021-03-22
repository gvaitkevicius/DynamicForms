var idOrd, idPro, idMaq, seqRep, index;
var contador = [];
function PlanilhaTestesVisuaisOP(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, INDEX) {
    idOrd = ORD_ID, idPro = ROT_PRO_ID, idMaq = ROT_MAQ_ID, seqRep = FPR_SEQ_REPETICAO; index = INDEX;
    var path = "/plugandplay/Qualidade/ObterTestesVisuaisPorOPTabela";
    var isVisible = $("#testesVisuais" + INDEX).is(":visible");
    $("#tabelaTesteVisualEdital").remove();
    $("#tabelaTesteEdital").remove();
    if (!isVisible) {
        $("div").removeClass('in');
        $("#collapse" + INDEX).addClass('in');
        $.ajax({
            url: path,
            type: "GET",
            data: { ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO },
            dataType: "json",
            success: function (result) {
                if (result != null) {
                    var divListaTestes = $('#testesVisuais' + INDEX);
                    let dados = result._listaInspecoes;
                    var cont = 0;
                    var maior = 0;
                    var _GrupoTeste = '';
                    var combobox = [];
                    var selecionado = [];
                    contador = [];
                    //Head
                    _GrupoTeste += '<div class="card" id="tabelaTesteVisualEdital">';
                    _GrupoTeste += ' <div class="card-body">';
                    _GrupoTeste += '   <div id="table" class="table-editable table-responsive bootgrid-wrapper " style="overflow: auto;">';
                    _GrupoTeste += '     <table id="tabelaTestes" class="table table-bordered table-responsive-md table-striped text-center tabelaEditavel">';
                    _GrupoTeste += '       <thead style ="overflow: auto;">';
                    _GrupoTeste += '         <tr>';
                    _GrupoTeste += '           <th class="text-center" > NOME_TECNICO</th>';

                    for (var i = 0; i < dados.length; i++) {
                        if (dados[i].length > maior) {
                            maior = dados[i].length;
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
                    var i = 0;
                    dados.forEach(function (item) {
                        _GrupoTeste += '<tr id="Linha' + cont + '">';
                        _GrupoTeste += '           <td class="pt-3-half" contenteditable="false" title="' + item[0].TipoInspecaoVisual.TIV_NOME + '" id="' + item[0].TipoInspecaoVisual.TIV_ID + '">' + item[0].TipoInspecaoVisual.TIV_NOME + '</td>';
                        i = 0;
                        item.forEach(function (aux) {
                            if (aux.TipoInspecaoVisual.TIV_MEDIDA == 'N') {
                                _GrupoTeste += '           <td class="pt-3-half" title="Data Coletada: ' + aux.IPV_DATA_COLETA + '" contenteditable="false" id="' + aux.IPV_ID + '"><select id="select' + aux.IPV_ID + '"><option id="op1' + aux.IPV_ID + '" value="OK">OK</option><option id="op2' + aux.IPV_ID + '" value="FE">FE</option></select></td>';
                                combobox.push('select' + aux.IPV_ID);
                                selecionado.push(aux.IPV_VALOR);
                                contador.push(cont);
                            } else {
                                _GrupoTeste += '           <td class="pt-3-half" title="Data Coletada: ' + aux.IPV_DATA_COLETA + '" contenteditable="false" id="' + aux.IPV_ID + '"> <input type="text" id="input' + aux.IPV_ID + '" value="' + aux.IPV_VALOR_MEDIDA + '" onblur="SalvarInput(' + aux.IPV_ID + ');"></td>';
                            }
                            i++;
                        })
                        for (i; i < maior; i++) {
                            _GrupoTeste += '           <td class="pt-3-half" contenteditable="false" id="desabilitado"></td>';
                        }
                        _GrupoTeste += '           <td class="pt-3-half" title="Status" contenteditable="false" id="StatusVisuais' + cont + '">';

                        _GrupoTeste += '<i id="ImgStatusVisuais' + cont + '" title="Status" class=""></i>';

                        _GrupoTeste += '</td>'
                        _GrupoTeste += '         </tr>';
                        j++;
                        cont++;
                    })

                    _GrupoTeste += '     </tbody>';
                    _GrupoTeste += '     </table>';
                    _GrupoTeste += '   </div>';
                    _GrupoTeste += '</div>';
                    _GrupoTeste += ' </div>';
                    _GrupoTeste += '</div>';


                    divListaTestes.append(_GrupoTeste);
                    selecionarCombobox(combobox, selecionado);
                    OnchangeSelect();
                }
            },
            error: function () {
                return "Erro nao tratado";
            }
        });
    } else {
        $("#tabelaTesteVisualEdital" + INDEX).remove();
    }
}

function SalvarInput(TesteId) {

    var texto = $("#input" + TesteId).val();

    var path = "/plugandplay/Qualidade/SalvarValorMedida";
    $.ajax({
        url: path,
        type: "GET",
        traditional: true,
        data: { Id: TesteId, Valor: texto },
        dataType: "json",
        success: function (result) {
        },
        error: function () {
            return "Erro nao tratado";
        }
    });

}

function selecionarCombobox(combo, selecionado) {
    var i = 0;
    combo.forEach(function (item) {
        $("#" + item).val(selecionado[i]);

        if (selecionado[i] == "OK") {

            $("#ImgStatusVisuais" + contador[i]).removeClass('fa fa-times-circle');
            $("#ImgStatusVisuais" + contador[i]).addClass('fa fa-check-circle');

        }
        else {
            $("#ImgStatusVisuais" + contador[i]).removeClass('fa fa-check-circle');
            $("#ImgStatusVisuais" + contador[i]).addClass('fa fa-times-circle');
        }
        i++;
    })
}

function OnchangeSelect() {
    $('select').blur(function () {
        var option = $("option:selected", this);
        var ids = [];
        var values = [];

        var idImg = $("#" + option.attr("id")).parent().parent().parent().children('td');
        var aux = idImg.get(idImg.length - 1).id;

        for (var i = 1; i < idImg.length - 1; i++) {
            ids.push(idImg.get(i).id);
            values.push($("#select" + idImg.get(i).id).val());
        }

        var path = "/plugandplay/Qualidade/GerarStatusVisuais";
        $.ajax({
            url: path,
            type: "GET",
            traditional: true,
            data: { Id: ids, Valor: values },
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    $("#Img" + aux).removeClass('fa fa-times-circle');
                    $("#Img" + aux).addClass('fa fa-check-circle').css('color', 'green');
                }
                else {
                    $("#Img" + aux).removeClass('fa fa-check-circle');
                    $("#Img" + aux).addClass('fa fa-times-circle').css('color', 'red');
                }
            },
            error: function () {
                return "Erro nao tratado";
            }
        });
    });
}


function abrirModalAmostrasVisuais(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO) {
    var path = "/plugandplay/Qualidade/ObterTestesVisuais";
    idOrd = ORD_ID, idPro = ROT_PRO_ID, idMaq = ROT_MAQ_ID, seqRep = FPR_SEQ_REPETICAO;
    $.ajax({
        url: path,
        type: "GET",
        data: { ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO },
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




