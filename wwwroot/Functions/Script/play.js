var global_act = "";
function sd(u, get, set) {
    Carregando.abrir('Processando....');
    var row = $('#row_col').attr('row');
    var col = $('#row_col').attr('col');
    var cabcol = $('#cabcol').attr('col');
    var arrayObj = new Array();
    var html = '';
    var objl = "";
    var cab = "";
    for (var i = 1; i <= cabcol; i++) {
        if (cab === "") {
            cab = "{'" + $('#hicab_' + i).attr('name') + "':'" + $('#hicab_' + i).val() + "'";
        } else {
            cab += ",'" + $('#hicab_' + i).attr('name') + "':'" + $('#hicab_' + i).val() + "'";
        }
    }
    cab += "}";
    entrou = false;
    for (var l = 1; l <= row; l++) {
        var obj = "";
        for (var c = 1; c <= col; c++) {
            entrou = true;
            if (obj === "") {
                obj = "{'" + $('#hi_' + l + '_' + c).attr('name') + "':'" + $('#hi_' + l + '_' + c).val() + "'";
            } else {
                obj += ",'" + $('#hi_' + l + '_' + c).attr('name') + "':'" + $('#hi_' + l + '_' + c).val() + "'";
            }
        }
        obj += "}";
        if (objl === "") {
            objl = "[" + obj;
        } else {
            objl += "," + obj;
        }
    }
    objl += "]";

    if (!entrou) {
        objl = "";
    }
    //var $inputs = $('#frm_' + set + ' :input');
    //var values = {};
    //$inputs.each(function () {
    //    if 

    //    //alert(this.name);
    //    //alert(this.name + ' - ' + this.id + ' - ' + $(this).val() + ' - ' + $(this).text() + ' k ' + $(this).is(':checked'));
    //});

    $.ajax({
        url: u,
        type: "POST",
        data: JSON.stringify({
            Itens: objl,
            Cab: cab,
            Acao: $('#hi_acao').val()
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.st === "OK") {
                ListaOPsParciais.carregarTabela();
            }else {
                Carregando.fechar();
                alert("ERRO-" + result.st);
            }
            execute_('set', get, set, global_act);
        },
        error: function () {
            Carregando.fechar();
            alert("Erro não tratado");
        },
        complete: function () {
            Carregando.fechar();
        }
    });
}
function fecharTela() {
    $('#modalgetAddAltCarga').modal('show');
}

function execute_(origem, get, set, act) {
    global_act = act;

    try {

        if (origem === "get") {
            for (var i = 0; i < act.length; i++) {

                if (act[i][0] === "modal") {
                    $('#modal' + get).modal('show');
                }
                if (act[i][0] === "antes") {
                    eval(act[i][1]);
                }
            }
        }
        if (origem === "set") {
            for (i = 0; i < act.length; i++) {
                if (act[i][0] === "depois") {
                    eval(act[i][1]);
                }
            }
        }
        var s = "";
        var l = "";
        if (origem === "string") {
            for (i = 0; i < act.length; i++) {
                for (var x = 0; x < act[i].length; x++) {
                    if (s !== "") {
                        s += ",";
                    }
                    s += '"' + act[i][x] + '"';
                }
                if (l !== "") {
                    l += ",";
                }
                l += "{" + s + "}";
                s = "";
            }
            return "{" + l + "}";
        }

    }
    catch (err) {
        alert("Erro na formula " + act[i][0] + " - " + act[i][1] + "    internal erro  " + err.message);
    }
}


function gd(d, u, get, set, act) {
    Carregando.abrir('Processando....');
    $.ajax({
        url: u + get,
        type: "POST",
        data: JSON.stringify(d),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            if (r.st === "OK") {
                // função a executar depois de completar o codigo 
                if (r.m !== "" && r.m !== null) {
                    execute_("get", get, set, act);
                    $('#' + get).html(grid(r.cab, r.itens, get, set, act, u)); // implementar outros modelos 
                }
                return "OK";
            }
            else {
                return r.st;
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}


function actionAjax(ids, action, par) {
    Carregando.abrir('Processando....');
    $.ajax({
        url: action,
        type: "POST",
        data: JSON.stringify({
            ids: ids,
            pars: par
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            if (r.st === "OK") {
                // função a executar depois de completar o codigo 
                $("#" + ids + "").addClass('btn-disabled');
                $("#" + ids + "").prop('value', 'Excluido').prop('disabled', true);
                $("#" + ids + "").text("Excluido");
                $("#" + ids + "").attr('disabled', 'disabled');
                return "OK";
            }
            else {
                return r.st;
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}

function actionAjaxBtn(ids, action, par, get) {
    Carregando.abrir('Processando....');
    $.ajax({
        url: action,
        type: "POST",
        data: JSON.stringify({
            ids: ids,
            pars: par
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            if (r.st === "OK") {
                // função a executar depois de completar o codigo 
                return "OK";
            }
            else {
                return r.st;
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}

function existElement(e, list) {
    var ret = false;
    for (var i = 0; i < list.length; i++) {
        if (e === list[i]) {
            ret = true;
        }
    }
    return ret;
}

function grid(cab, itens, get, set, act, u) {
    var h = "";
    var cabgrid = "";
    var cab_cab = "";
    var bod = "";
    var x = 0;
    var id = 0;
    var row = 0;// linha 
    var col = 0;// coluna 
    var box = -1;

    for (var i in cab) {
            var o = i;

            if (o.type === "checkbox") {
                col++;
                id = col;
                var isDisable = o.disable === "true" ? "disabled" : "";
                var js = "onchange=\"(($(this).is(':checked')) ? $('#hicab_" + id + "').val('" + o.value + "') : $('#hicab_" + id + "').val(''));\"";
                bod += '<div class="col-md-6 form-group">';
                bod += '<label class="form-label">' + i + '</label>';
                bod += '<input ' + isDisable + '  ' + js + ' class="form-check-input" type="checkbox" value="' + o.value + '" id="i_' + id + '" name="' + i + '" ' + (o.check === true ? " checked " : "") + ' > ';
                bod += '<input type="hidden" id="hicab_' + id + '" name="' + i + '"  value="' + (o.check === true ? o.value : "") + '">';
                bod += '</div>';

            } else if (o.type === "select") {
                col++;
                id = col;
                isDisable = o.disable === "true" ? "disabled" : "";
                selectList('i_' + id + '', '' + o.path + '', '' + o.action + '', '' + o.value + '');
                var seletor = $('#divResultSelect_i_' + id);
                bod += '<div class="col-md-6 form-group">';
                bod += '<label for select' + isDisable + '" id="i_' + id + '" name="' + i + '">' + i + '</label>';
                bod += '<input type="hidden" id="hicab_' + id + '" name="' + i + '"  value="' + o.value + '">';

                bod += '<select onchange="ListSelected(' + id + ',this.value);"class="form-control"  id="divResultSelect_i_' + id + '"></div>';
                bod += '<option selected value="' + o.value + '" >' + o.value + '</option>';
                bod += '</select>';
                bod += '</div>';
            } else if (o.type === "text") {
                col++;
                id = col;
                isDisable = o.disable === "true" ? "disabled" : "";
                js = o.search === "true" ? "onkeyup=\"$('#hicab_" + id + "').val(this.value);javascript:inputPesquisar(event,'i_" + id + "', $(i_" + id + ").val(),'" + o.path + "','" + o.action + "');\"" : "onkeyup=\"$('#hicab_" + id + "').val(this.value);\"";
                var color = o.changecolor === "true" ? "background-color : #b91400" : "";
                bod += '<div class="col-md-6 form-group">';
                bod += '<label class="form-label">' + i + '</label>';
                bod += '<input ' + isDisable + ' ' + js + ' class="form-control ' + color + '" type="text"  value="' + o.value + '" id="i_' + id + '" name="' + i + '">';
                bod += '<input type="hidden" id="hicab_' + id + '" name="' + i + '"  value="' + o.value + '">';
                bod += '<div id="divResultPesq_i_' + id + '" class="list-group" ></div>';
                bod += '</div>';
            } else if (o.type === "button") {
                col++;
                id = col;
                isDisable = o.disable === "true" ? "disabled" : "";
                bod += '<div class="btn-group ">';
                bod += '<div>';
                if (o.ids === null) {
                     js = "onclick=\"$('#hi_acao').val('" + o.value + "');sd('" + o.path + "','" + o.action + "','" + o.action + "');\"";
                } else {
                     js = "onclick=\"javascript:actionAjaxBtn('" + o.ids + "','" + o.path + "','" + o.action + "');\"";
                }
                bod += '<button ' + js + ' type="button" class="btn btn-primary">' + o.value + '</button>';
                bod += '</div>';

            } else {
                col++;
                id = col;
                bod += '<div class="col-md-6 form-group">';
                bod += '<label class="form-label">' + i + '</label>';
                bod += '<input disabled class="form-control" type="text" value="' + o + '" id="i_' + id + '" name="' + i + '">';
                bod += '<input type="hidden" id="hicab_' + id + '" name="' + i + '"  value="' + o + '">';
                bod += '</div>';
            }
        
    }

    cab_cab = '<div class="row"><input type="hidden" id="cabcol" col="' + col + '">' + bod + '</section></div></div>';
    bod = "";

    col = 0;




    itens.forEach(function (l) {//gera as linhas da tabela
        x++;
        if (cabgrid === "") {
            cabgrid += '<div class="row"><table class="table" id="dados" name="dados"><thead>';
            for (var i in l) {
                if (l.hasOwnProperty(i)) {
                    var o = l[i];
                    if (o !== null) {
                        if (o.hideitem === null || o.hideitem === "false") {
                            cabgrid += '<th>' + i + '</th>';
                        }
                    }
                }
            }
            cabgrid += '</tr></thead><tbody>';
        }


        bod += '<tr id="joao_' + row + '" name="joao_' + row + '" >';
        row++;
        col = 0;
        for (i in l) {
            if (l.hasOwnProperty(i)) {
                o = l[i];
                if (o !== null) {
                    if (o.type === "checkbox") {
                        col++;
                        id = row + '_' + col;
                        var js = "onchange=\"(($(this).is(':checked')) ? $('#hi_" + id + "').val('" + o.value + "') : $('#hi_" + id + "').val(''));\"";
                        var color = o.changecolor === "true" ? "text-danger" : "";
                        bod += '<td>';
                        bod += '<input ' + js + ' class="form-check-input " type="checkbox" value="' + o.value + '" id="i_' + id + '" name="' + i + '" ' + (o.check === true ? " checked " : "") + ' > ';
                        bod += '<label class="form-check-label ' + color + '" for= "i_' + id + '">' + o.value + '</label>';
                        bod += '<input type="hidden" id="hi_' + id + '" name="' + i + '"  value="' + (o.check === true ? o.value : "") + '">';
                        bod += '</td>';
                    } else if (o.type === "text") {
                        col++;
                        id = row + '_' + col;
                        isDisable = o.disable === "true" ? "disabled " : "";
                        js = "onkeyup=\"$('#hi_" + id + "').val(this.value)\"";
                        color = o.changecolor === "true" ? "text-danger" : "";
                        bod += '<td>';
                        bod += '<input ' + isDisable + js + ' class="form-control ' + color + '" type="text" value="' + o.value + '" id="i_' + id + '" name="' + i + '">';
                        bod += '<input type="hidden" id="hi_' + id + '" name="' + i + '"  value="' + o.value + '">';
                        bod += '</td>';
                    } else if (o.type === "button") {
                        //no onclick do botao fazer chamada para ajax
                        col++;
                        id = row + '_' + col;
                        js = "onclick=\"javascript:actionAjax('" + o.ids + "','" + o.action + "','" + row + "');\"";
                        bod += '<td>';
                        bod += '<button  type="button" name="' + o.ids + '" id="' + o.ids + '" value="' + row + '" class="btn btn-primary"' + js + '">' + o.value + '</button>';
                        bod += '</td>';
                    }else {
                        bod += '<td>' + l[i] + '</td>';
                    }
                }
            }
        }
        bod += '</tr>';
    });
    bod += '<input type="hidden" id="row_col" row="' + row + '" col="' + col + '">';
    bod += '</tbody></table></div>';

    var botao = "";
    for (i = 0; i < act.length; i++) {
        if (act[i][0] === "button") {
            if (act[i][1].toUpperCase() === "CANCELAR" || act[i][1].toUpperCase() === "SAIR" || act[i][1].toUpperCase() === "FECHAR") {
                botao += '<button type="button" class="btn btn-default" data-dismiss="modal">' + act[i][1] + '</button>';
            } else {
                js = "onclick=\"$('#hi_acao').val('" + act[i][2] + "');sd('" + u + set + "','" + get + "','" + set + "');\"";
                botao += '<button ' + js + ' type="button" class="btn btn-primary">' + act[i][1] + '</button>';
            }
        }
    }
    botao = '<div id="" class="modal-footer">' + botao + '</div>';
    bod += '<input type="hidden" id="hi_acao" value="">';
    return cab_cab + cabgrid + bod + botao;
}
//Recupera item selecionado do Select (list box)
function ListSelected(idItem, idLista) {
    if (idLista !== undefined && idLista !== '') {
        var tt = '#hicab_' + idItem;
        $(tt).val(idLista);
        $(tt).text(idLista);
        $('#' + idItem).val(idLista);
        tt = '#divResultSelect_' + idItem;
        $(tt).hide();
    }
}
