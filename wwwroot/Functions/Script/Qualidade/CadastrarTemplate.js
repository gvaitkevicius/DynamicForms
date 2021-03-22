$(document).ready(function () {
    addSelect('TiposTestes', 'TestesSelecionados', '/plugandplay/Qualidade/RetornarTiposTestes');
    addSelect('TiposInspecao', 'InspecaoSelecionados', '/plugandplay/Qualidade/RetornarTiposInspecao');

    $('#alterar').click(function () {
        $("#btnAdd").attr("disabled", false);
        $("#btnRemove").attr("disabled", false);
        $("#btnClear").attr("disabled", false);

        $("#btnAddInspecao").attr("disabled", false);
        $("#btnRemoveInspecao").attr("disabled", false);
        $("#btnClearInspecao").attr("disabled", false);

        $("#cancelar").attr("disabled", false);
        $("#salvar").attr("disabled", false);
        $("#alterar").attr("disabled", true);
    });

    $('#cancelar').click(function () {
        $("#btnAdd").attr("disabled", true);
        $("#btnRemove").attr("disabled", true);
        $("#btnClear").attr("disabled", true);

        $("#btnAddInspecao").attr("disabled", true);
        $("#btnRemoveInspecao").attr("disabled", true);
        $("#btnClearInspecao").attr("disabled", true);

        $("#cancelar").attr("disabled", true);
        $("#salvar").attr("disabled", true);
        $("#alterar").attr("disabled", false);

        addSelect('TiposTestes', 'TestesSelecionados', '/plugandplay/Qualidade/RetornarTiposTestes');
        addSelect('TiposInspecao', 'InspecaoSelecionados', '/plugandplay/Qualidade/RetornarTiposInspecao');
    });

    $('#salvar').click(function () {
        $("#btnAdd").attr("disabled", true);
        $("#btnRemove").attr("disabled", true);
        $("#btnClear").attr("disabled", true);

        $("#btnAddInspecao").attr("disabled", true);
        $("#btnRemoveInspecao").attr("disabled", true);
        $("#btnClearInspecao").attr("disabled", true);

        $("#cancelar").attr("disabled", true);
        $("#salvar").attr("disabled", true);
        $("#alterar").attr("disabled", false);

        Salvar();
    });
});


function addSelect(id, idSelecionado, urlData) {

    var idTemplate = $('#TemplateID').text();

    $.ajax({
        type: "get",
        url: urlData,
        data: { TemId: idTemplate },
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

                var TestesNoTemplate = obj.TestesNoTemplate;
                var selectbox = $('#' + idSelecionado);
                selectbox.find('option').remove();
                $.each(TestesNoTemplate, function (i, d) {
                    $('<option>').val(d.Id).text(d.Descricao).appendTo(selectbox);
                });
            }
        }
    });
}

function addTestes() {

    if ($('#TiposTestes').find(":selected")) {
        $('#TiposTestes option:selected').each(function () {
            var valor = $(this).val();
            var texto = $(this).text();
            if (texto != null && valor != null) {
                $('#TestesSelecionados').append('<option value="' + valor + '">' + texto + '</option>');
                $('#TiposTestes').find(":selected").remove();
            }
        });
    }
}

function RetirarTeste() {

    if ($('#TestesSelecionados').find(":selected")) {
        $('#TestesSelecionados option:selected').each(function () {
            var valor = $(this).val();
            var texto = $(this).text();
            if (texto != null && valor != null) {
                $('#TiposTestes').append('<option value="' + valor + '">' + texto + '</option>');
                $('#TestesSelecionados').find(":selected").remove();
            }
        });
    }
}

function Limpar() {

    $('#TestesSelecionados').find('option').each(function () {
        var valor = $(this).val();
        var texto = $(this).text();
        $('#TiposTestes').append('<option value="' + valor + '">' + texto + '</option>');
        $(this).remove();
    });
}

function addInspecao() {

    if ($('#TiposInspecao').find(":selected")) {
        $('#TiposInspecao option:selected').each(function () {
            var valor = $(this).val();
            var texto = $(this).text();
            if (texto != null && valor != null) {
                $('#InspecaoSelecionados').append('<option value="' + valor + '">' + texto + '</option>');
                $('#TiposInspecao').find(":selected").remove();
            }
        });
    }
}

function RetirarInspecao() {

    if ($('#InspecaoSelecionados').find(":selected")) {
        $('#InspecaoSelecionados option:selected').each(function () {
            var valor = $(this).val();
            var texto = $(this).text();
            if (texto != null && valor != null) {
                $('#TiposInspecao').append('<option value="' + valor + '">' + texto + '</option>');
                $('#InspecaoSelecionados').find(":selected").remove();
            }
        });
    }
}

function LimparInspecao() {

    $('#InspecaoSelecionados').find('option').each(function () {
        var valor = $(this).val();
        var texto = $(this).text();
        $('#TiposInspecao').append('<option value="' + valor + '">' + texto + '</option>');
        $(this).remove();
    });
}

function Salvar() {
    var valores = [];
    var inspecoes = [];

    var id = $('#TemplateID').text();
    $('#TestesSelecionados').find('option').each(function () {
        var valor = $(this).val();
        if (valor != "")
            valores.push(valor);
    });

    $('#InspecaoSelecionados').find('option').each(function () {
        var valor = $(this).val();
        if (valor != "")
            inspecoes.push(valor);
    });

    $.ajax({
        type: "get",
        url: '/plugandplay/Qualidade/AddTiposDeTestesNoTemplate',
        traditional: true,
        data: { TemId: id, ListaTipos: valores, ListaTiposInspecao: inspecoes },
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (obj) {
            if (obj == true) {
                mostraDialogo("Testes Salvos", "success");
            }
        }
    });
}

function selectSearch(id_grup, id_Item, label, controllerPath) {
    var input = $('#' + id_grup + '');
    input.find('input').remove();
    var html = '';
    html += '<label for="' + id_Item + '">' + label + '</label>';
    html += '<select class="form-control" id="' + id_Item + '">';
    html += '<option>Procurar</option>';
    html += '</select>';
    input.html(html);
    addSelect('' + id_Item + '', controllerPath,);
}

function mostraDialogo(mensagem, tipo) {

    // se houver outro alert desse sendo exibido, cancela essa requisição
    if ($("#message").is(":visible"))
        return false;

    // se não setar o tempo, o padrão é 3 segundos
    if (!tempo)
        var tempo = 3000;

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
    }, tempo); // milliseconds
}