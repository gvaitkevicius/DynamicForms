// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function preencherCampos(_url) {
    $.ajax({
        type: "GET",
        url: _url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            exibirDados(result);
        },
        error: OnError,
        complete: onComplete 
    });
}

function exibirDados(result) {
    var obj = JSON.parse(result);
    var str_html = montarHtml(obj);
    $(".exibir").html(str_html);
}

function montarHtml(obj) {
    var str_html = "";
    str_html += '<div class="col-xs-12">';
    str_html += '<section class="box">';
    str_html += '<div class="content-body">';
    str_html += '<div class="row">';
    str_html += '<div class="col-md-8 col-sm-9 col-xs-10">';

    for (var i = 0; i < obj.length; i += 3) {
        //Tratar tipo;
        //Tratar nome_var;
        
        str_html += getFormulario(obj[i], obj[i + 1]);
        //str_html += getComponent(obj[i], obj[i + 1], obj[i + 2]);

        //For para percorrer e tratar atributos;
        //str = obj[i] + " -- " + obj[i + 1] + " -- " + obj[i + 2];
        //console.log(str);
    }

    str_html += '</div>';
    str_html += '</div>';
    str_html += '</div>';
    str_html += '</section>';
    str_html += '</div>';
    
    return str_html;
}

function getFormulario(tipo, identificador) {
    var str = "";
    str += '<div class="form-group">';
    str += '<label class="form-label">' + identificador + '</label>'
    str += '<div class="controls">';

    str += '<input ';
    str_prop = '';
    if (tipo.toLowerCase().includes("int")) {
        str_prop += 'type="text" placeholder="' + identificador + '" class="form-control" ';
        /*
        if (identificador.toLowerCase().includes("id")) {
            str_prop += 'disabled="disabled"';
        }
        */
    } else if (tipo.toLowerCase().includes("string")) {
        str_prop += 'type="text" placeholder="' + identificador + '" class="form-control" ';

        if (identificador.toLowerCase().includes("email")) {
            str_prop = str_prop.replace('type="text"', 'type="email"');
        }

    } else if (tipo.toLowerCase().includes("date")) {
        str_prop += 'type="text" placeholder="' + identificador +
            '" class="form-control datepicker" data-format="D, dd MM yyyy"';

    } else if (tipo.toLowerCase().includes("char")) {
        str_prop += 'type="text" placeholder="' + identificador + '" class="form-control" ';

    }
    str += str_prop;
    str += ' />';
    str += '</div>';
    str += '</div>';
    return str;
}

function getComponent(tipo, identificador, atributos) {
    //Não levar a sério essa função, só esotu testando
    var str = "";
    if (tipo.toLowerCase().includes("string")) {
        alert("String");
    }
    else if (tipo.toLowerCase().includes("date")) {
        str = "<input type=\"date\" placeholder=\"" + identificador + "\" name = \"" + identificador + "\">";
    }
    else if (tipo.toLowerCase().includes("int")) {
        str = "<input type=\"numeric\" placeholder=\"" + identificador + "\" name = \"" + identificador + "\">";
    }
    else if (tipo.toLowerCase().includes("char")) {
        alert("Char");
    }
    else {
        alert("Else");
    }
    return str;
}

//###########################################################################################//
function OnError() {
    alert("Error");
}

function onComplete() {
    //alert("Complete");
}