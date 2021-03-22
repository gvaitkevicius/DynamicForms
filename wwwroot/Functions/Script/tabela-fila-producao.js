
$(document).ready(function () {

    //#region Eventos

    $('[data-toggle="tooltip"]').tooltip();

    $(`[name=btnRadioVisualizacao]`).change(function () {
        TabelaFilaProducao.evtMudouRadioVisualizacao(event);
    });

    //eventos do otimizador
    $(`#btnAtualizarDadosOtimizador`).click(function () {
        DadosOtimizador.iniciar();
    });
    $("#execAgoraOtimizador").click(function () {
        DadosOtimizador.executarAgora();
    });
    $("#adiarOtimizador").click(function () {
        DadosOtimizador.adiar();
    })

    //eventos da interface
    $(`#btnAtualizarDadosInterface`).click(function () {
        DadosInterface.iniciar();
    });
    $("#execAgoraInterface").click(function () {
        DadosInterface.executarAgora();
    })
    $("#adiarInterface").click(function () {
        DadosInterface.adiar();
    })


    //#endregion

    //prepara os dados da pagina e depois realiza a primeira pesquisa
    prepararDadosIniciaisAPS('/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.CargasWeb').then(function () {
        //TabelaFilaExpedicao.eventoPesquisarPrincipalAPS();
    });

    //A cada 5 segundos atualiza os dados da interface e otimizador
    monitorOtimizador.iniciar();
    monitorInterface.iniciar();
});


function roundToTwo(num) {
    return +(Math.round(num + "e+2") + "e-2");
}


var monitorOtimizador = {
    timeout: null,
    iniciar: async function () {
        await DadosOtimizador.iniciar();
        monitorOtimizador.timeout = setTimeout(monitorOtimizador.iniciar, 5000);
    },
    cancelar: function () {
        if (monitorOtimizador.timeout != null)
            clearTimeout(monitorOtimizador.timeout);
    }
}

var monitorInterface = {
    ajax: null,
    timeout: null,
    iniciar: async function () {
        await DadosInterface.iniciar();
        monitorInterface.timeout = setTimeout(monitorInterface.iniciar, 5000);
    },
    cancelar: function () {
        if (monitorInterface.timeout != null)
            clearTimeout(monitorInterface.timeout);
    }
}


var DadosOtimizador = new function () {
    this.iniciar = async function () {
        return await obterMensagensOtimizador();
    }

    //obtem as mensagens do otimizador e faz o necessário com elas, como, exibir a barra de progresso da mensagem de progresso e etc...
    function obterMensagensOtimizador() {
        return $.ajax({
            type: 'GET',
            url: '/apischedule/APISchedule/ObterMensagensOtimizador',
            success: function (mensagens) {

                //Ordenando as mensagens por data descreceste
                mensagens.sort(function (a, b) {
                    return new Date(b.MEN_EMISSION) - new Date(a.MEN_EMISSION)
                })

                //Verifica se o otimizador esta rodando
                let msg_run = mensagens.find(msg => msg.MEN_TYPE === "RUN_OTIMIZADOR");
                if (msg_run != undefined && msg_run.MEN_SEND.toLowerCase() === "true") {
                    abrirIC();
                    let mensagens_de_progresso = mensagens.filter(msg => msg.MEN_TYPE.startsWith("%"));

                    //Adiciona a barra de progresso, o botão de cancelar e o botão de atualizar dados
                    if (mensagens_de_progresso != null && mensagens_de_progresso.length > 0) {

                        mensagens_de_progresso = mensagens_de_progresso.sort((a, b) => (a.MEN_SEND > b.MEN_SEND) ? -1 : 1);

                        $('#acoesOtimizador').html([
                            $('<div>', { style:"margin-top: 20px; margin-right: 5px;", class: "progresso_otimizador progress ", id: "btnAtualizarDadosOtimizador" }).html([
                                $('<div>', { class: "progress-bar", style: `color:black; width: ${mensagens_de_progresso[0].MEN_SEND}%;`, role: "progressbar", "aria-valuenow": `${mensagens_de_progresso[0].MEN_SEND}`, "aria-valuemin": "0", "aria-valuemax": "100" }).html(`${mensagens_de_progresso[0].MEN_TYPE.replace('%', '')}`)
                            ])
                        ]);
                    }
                    else {
                        $('#acoesOtimizador').html('Não foram encontradas informações de progresso.');
                    }
                }
                else
                    $('#acoesOtimizador').html('');

                //Muda o icone para erro se tiver erro no otimizador
                if(mensagens.find(msg => msg.MEN_TYPE === "ERRO_OTIMIZADOR")){
                    $(`#iconOtLoad`).attr('class', 'fa fa-exclamation-triangle');
                }
                else if(msg_run != undefined && msg_run.MEN_SEND.toLowerCase() === "false"){
                    fecharIC();
                }

                //Exibe a proxima execução
                let msg_prox_exec_otimizador = mensagens.find(msg => msg.MEN_TYPE === "PROX_EXEC_OTIMIZADOR");
                if(msg_prox_exec_otimizador !== undefined)
                    $('#prox_exec_otimizador').html(msg_prox_exec_otimizador.MEN_SEND);

                //Exibe a ultima execução
                let msg_ultima_exec_otimizador = mensagens.find(msg => msg.MEN_TYPE === "ULTIMA_EXEC_OTIMIZADOR");
                if(msg_ultima_exec_otimizador !== undefined)
                    $('#ultima_exec_otimizador').html(msg_ultima_exec_otimizador.MEN_SEND);

                //Preenche a tabela com todas as mensagens
                $('#tableMensagensOtimizador tbody').html('');
                mensagens.forEach(function(currentValue){
                    let men_send = $('<td>', { style:"width:15px; height: 80px;" }).html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_SEND)]);
                    let men_type = currentValue.MEN_TYPE === "ERRO_OTIMIZADOR" ? $('<td>').html([$('<div>', { class: "scrollable", style:"color:red;" }).html(currentValue.MEN_TYPE)]) : $('<td>').html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_TYPE)]);
                    let men_emission = $('<td>').html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_EMISSION)]);
                    
                    let linha = $(`<tr>`).html([
                        men_send,
                        men_type,
                        men_emission
                    ]);
                    $('#tableMensagensOtimizador tbody').append(linha);
                });
            },
            error: function () {
            },
            complete: function () {
            }
        });
    }

    function abrirIC() {
        $('#iconOtLoad').attr('class', 'fa fa-spinner fa-pulse fa-6x fa-fw')
        //$('#otLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Otimizador');
    }
    function fecharIC() {
        $('#iconOtLoad').attr('class', 'fa fa-check')
        //$('#iconOtLoad').html('<i class="fa fa-check"></i>Otimizador');
    }
    function adiar() {
        console.log("Adiando otimizador...");
    }
    this.executarAgora = function (type_otimizador) { //faz chamada para a ApiSchedule

        abrirIC();

        return $.ajax({
            type: 'GET',
            url: '/apischedule/APISchedule/ExecutarAgoraOtimizador',
            dataType: "json",
            data: { type_otimizador: type_otimizador },
            success: function (mensagens) {
                mensagens.forEach(function(men){
                    if(men.MEN_TYPE != "ERRO_OTIMIZADOR" && men.MEN_TYPE != "ERRO_SCHEDULE" && men.MEN_TYPE != "ERRO_INTERFACE"){
                        //mostraDialogo("O otimizador será executado imediatamente.", 'success', 3000);
                        //$(".exibir").append("O otimizador será executado imediatamente.");
                    }
                    else{
                        mostraDialogo(men.MEN_SEND, 'error', 3000);
                        $(".exibir").append(men.MEN_SEND);
                    }
                })
            },
            error: function () {
            },
            complete: function () {
                fecharIC();
            }
        });
    }
}

var DadosInterface = new function () {
    this.iniciar = async function () {
        await obterMensagensInterface();
    }

    //obtem as mensagens do otimizador e faz o necessário com elas, como, exibir a barra de progresso da mensagem de progresso e etc...
    async function obterMensagensInterface() {
        return $.ajax({
            type: 'GET',
            url: '/apischedule/APISchedule/ObterMensagensInterface',
            success: function (mensagens) {

                //Ordenando as mensagens por data decrescente
                mensagens.sort(function (a, b) {
                    return new Date(b.MEN_EMISSION) - new Date(a.MEN_EMISSION)
                })

                let msg_run = mensagens.find(msg => msg.MEN_TYPE === "RUN_INTERFACE");

                // Se a interface está rodando, adiciona a barra de progresso
                if (msg_run != undefined && msg_run.MEN_SEND.toLowerCase() === "true") {
                    abrirIC();
                    let mensagens_de_progresso = mensagens.filter(msg => msg.MEN_TYPE.startsWith("%"));

                    //Adiciona a barra de progreso, o botão de cancelar e o de atualizar
                    if (mensagens_de_progresso != null && mensagens_de_progresso.length > 0) {

                        mensagens_de_progresso = mensagens_de_progresso.sort((a, b) => (a.MEN_SEND > b.MEN_SEND) ? -1 : 1);
                        $('#acoesInterface').html([
                            $('<div>', { class: "progresso progress", style:"margin-top: 17px; margin-right: 5px;" }).html([
                                $('<div>', { class: "progress-bar", style: `color:black; width: ${mensagens_de_progresso[0].MEN_SEND}%`, role: "progressbar", "aria-valuenow": `${mensagens_de_progresso[0].MEN_SEND}`, "aria-valuemin": "0", "aria-valuemax": "100" }).html(mensagens_de_progresso[0].MEN_TYPE.replace('%', ''))
                            ])
                        ]);
                    }
                    else {
                        $('#acoesInterface').html('Não foram encontradas informações de progresso.');
                    }
                }
                else
                    $('#acoesInterface').html('');

                //Muda o icone para erro se tiver erro na interface
                if(mensagens.find(msg => msg.MEN_TYPE === "ERRO_INTERFACE")){
                    $(`#iconItLoad`).attr('class', 'fa fa-exclamation-triangle');
                }
                else if(msg_run != undefined && msg_run.MEN_SEND.toLowerCase() === "false"){
                    fecharIC();
                }

                //Exibe a proxima execução
                let msg_prox_exec_interface = mensagens.find(msg => msg.MEN_TYPE === "PROX_EXEC_INTERFACE");
                if(msg_prox_exec_interface != undefined)
                    $('#prox_exec_interface').html(msg_prox_exec_interface.MEN_SEND);

                //Exibe a ultima execução
                let msg_ultima_exec_interface = mensagens.find(msg => msg.MEN_TYPE === "ULTIMA_EXEC_INTERFACE");
                if(msg_ultima_exec_interface !== undefined)
                    $('#ultima_exec_interface').html(msg_ultima_exec_interface.MEN_SEND);

                //Preenche a tabela com todas as mensagens
                $('#tableMensagensInterface tbody').html('');
                mensagens.forEach(function(currentValue){
                    let men_send = $('<td>', { style:"width:15px; height: 80px;" }).html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_SEND)]);
                    let men_type = currentValue.MEN_TYPE === "ERRO_INTERFACE" ? $('<td>').html([$('<div>', { class: "scrollable", style:"color:red;" }).html(currentValue.MEN_TYPE)]) : $('<td>').html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_TYPE)]);
                    let men_emission = $('<td>').html([$('<div>', { class: "scrollable" }).html(currentValue.MEN_EMISSION)]);
                    let linha = $(`<tr>`).html([
                        men_send,
                        men_type,
                        men_emission
                    ]);
                    $('#tableMensagensInterface tbody').append(linha);
                });

            },
            error: function () {
            },
            complete: function () {
            }
        });
    }
    function abrirIC() {
        $('#iconItLoad').attr('class', 'fa fa-spinner fa-pulse fa-6x fa-fw')
        //$('#iconItLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Interface');
    }
    function fecharIC() {
        $('#iconItLoad').attr('class', 'fa fa-check');
        //$('#iconItLoad').html('<i class="fa fa-check"></i>Interface');
    }
    function adiar() {
        console.log('Adiando interface...');
    }
    this.executarAgora = function () { //faz chamada para a ApiSchedule

        abrirIC();

        return $.ajax({
            type: 'POST',
            url: '/apischedule/APISchedule/ExecutarAgoraInterface',
            success: function (mensagens) {
                
                mensagens.forEach(function(men){
                    if(men.MEN_TYPE != "ERRO_OTIMIZADOR" && men.MEN_TYPE != "ERRO_SCHEDULE" && men.MEN_TYPE != "ERRO_INTERFACE"){
                        //mostraDialogo("A interface será executada imediatamente.", 'success', 3000);
                    }
                    else{
                        mostraDialogo(men.MEN_SEND, 'error', 3000);
                    }
                })
            },
            error: function () {
            },
            complete: function () {
                fecharIC();
            }
        });
    }
}

var DadosSchedule = new function(){
    this.execProcedure = async function(){
        let mostrou_erro = false;

        return $.ajax({
            type: 'POST',
            url: '/apischedule/APISchedule/ExecProcedureInterface',
            success: function (mensagens) {

                //se não tem erro nas mensagens, mostra uma msg de sucesso
                if(!mensagens.find(x => x.MEN_TYPE == "ERRO_INTERFACE")){
                    mostraDialogo('Sucesso ao executar procedure.', 'success', 3000);
                }
                else{ //se tem, mostra todas mensagens
                    mensagens.forEach(function(men){
                        if(men.MEN_TYPE != "ERRO_INTERFACE"){
                            mostraDialogo(men.MEN_SEND, "info", 3000);
                        }
                        else if(men.MEN_TYPE == "ERRO_INTERFACE" && !mostrou_erro){
                            mostraDialogo(men.MEN_SEND, "error", 3000);
                            mostrou_erro = true;
                        }
                    });
                }
            },
            error: function () {
                mostraDialogo("Falha ao executar procedure.", "error", 3000);
            },
            complete: function () {
            }
        });
    }

    this.executarAgora = async function(rodarOtimizador, rodarInterface, type_otimizador){


        let msg = "Agendando execução imediata:";
        if(rodarOtimizador != ""){
            msg += " otimizador ";
        }
        if(rodarInterface != ""){
            msg += " interface ";
        }

        mostraDialogo(msg, 'info', 3000)

        if(rodarOtimizador != ""){
            await DadosOtimizador.executarAgora(type_otimizador);

        }
        if(rodarInterface != ""){
            await DadosInterface.executarAgora();
        }

    }

    this.cancelar = async function(){
        let mostrou_erro = false;

        return $.ajax({
            type: 'POST',
            url: '/apischedule/APISchedule/CancelarExecucao',
            success: function (mensagens) {

                //se não tem erro nas mensagens, mostra uma msg de sucesso
                if(!mensagens.find(x => x.MEN_TYPE.startsWith("ERRO"))){
                    mostraDialogo('Sucesso ao cancelar execuções.', 'success', 3000);
                }
                else{ //se tem, mostra todas mensagens
                    mensagens.forEach(function(men){
                        if(!men.MEN_TYPE.startsWith("ERRO")){
                            mostraDialogo(men.MEN_SEND, "info", 3000);
                        }
                        else if(men.MEN_TYPE.startsWith("ERRO") && !mostrou_erro){
                            mostraDialogo(men.MEN_SEND, "error", 3000);
                            mostrou_erro = true;
                        }
                    });
                }
            },
            error: function () {
                mostraDialogo("Falha ao cancelar interface/otimizador.", "error", 3000);
            },
            complete: function () {
            }
        });
    }
}

var TabelaCargaMaquina = new function () {
    this.iniciar = function () {
        fecharIC();
    }
    this.carregarTabela = function (acao, data, grupo) {

        var _html = "";
        var id_href = "";

        abrirIC();

        if (acao == '') {
            $.ajax({
                type: 'GET',
                url: UrlBase + 'ObterCargaMaquina?nivel=n1',
                dataType: 'json',
                traditional: true,
                async: false,
                contenttype: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.cargaMaquinaDia2.length != null) {
                        _EAtraso = '';
                        _PAtraso = '';
                        _Adiantado = '';
                        _Reserva = '';
                        _NaData = '';
                        _M2MaquinaDia = '';
                        _M2AcabadoDia = '';
                        _PesoAcabadoDia = '';
                        dia = '';
                        _id = '';
                        _menu = '';
                        _menuFim = '';
                        _html = '';

                        result.cargaMaquinaDia2.forEach(function (f) {

                            if (dia != f.MED_DATA) {
                                _html += _menu + _EAtraso + _NaData + _Adiantado + _PAtraso + _Reserva + _M2AcabadoDia + _PesoAcabadoDia + _menuFim;

                                _EAtraso = '&emsp;<span class="label label-danger" title = "Entrega Atrasada"> 0 TL - Entrega Atrasada</span>';
                                _NaData = '&emsp;<span class="label label-success"   title = "Na data"> 0 TL - Na Data</span>';
                                _Adiantado = '&emsp;<span class="label label-primary" title = "Antecipado"> 0 TL - Adiantado</span>';
                                _PAtraso = '&emsp;<span class="label label-default" title = "Planejamento Atrasado"></i> 0 TL - Ocioso</a></span>';
                                //_Reserva = '&ensp;<span class="label label-primary" title = "Reserva"> 0 TL - Reserva</span>';

                                T_Adiantado = 0;
                                T_EAtraso = 0;
                                T_NaData = 0;
                                T_PAtraso = 0;
                                T_Reserva = 0;
                                T_M2MaquinaDia = 0;
                                T_M2AcabadoDia = 0;
                                T_PesoMaquinaDia = 0;
                                T_PesoAcabadoDia = 0;
                            }
                            dia = f.MED_DATA;
                            id_href = f.MED_DATA.substring(0, 4) + '_' + f.MED_DATA.substring(5, 7) + '_' + f.MED_DATA.substring(8, 10);
                            _id = '#c_' + id_href;


                            _menu = '<div class="panel panel-default">';
                            _menu += '<div class="panel-heading" role="tab">';
                            _menu += '<h4 class="panel-title">';
                            _menu += '<a style="text-decoration:none;" data-toggle="collapse" href="' + _id + '" aria-expanded="false" class="collapsed">';
                            _menu += '<i class="fa fa-check"></i>PRODUÇÃO ' + f.MED_DATA.substring(8, 10) + '/' + f.MED_DATA.substring(5, 7) + '/' + f.MED_DATA.substring(0, 4);

                            T_EAtraso += f.ATRASO;
                            _EAtraso = '&emsp;<span class="label label-danger label-aps-n1" title = "ENTREGA ATRASADA"> ' + roundToTwo(T_EAtraso) + ' HRS - ATRASADOS</span>';
                            T_NaData += f.NA_DATA;
                            _NaData = '&emsp;<span class="label label-success label-aps-n1"   title = "NA DATA"> ' + roundToTwo(T_NaData) + ' HRS - NA DATA</span>';
                            T_Adiantado += f.ADIANTADO;
                            _Adiantado = '&emsp;<span class="label label-primary label-aps-n1" title = "ANTECIPADO"> ' + roundToTwo(T_Adiantado) + ' HRS - ADIANTADOS</span>';
                            T_PAtraso += f.OCIOSO;
                            _PAtraso = '&emsp;<span class="label label-default label-aps-n1" title = "PLANEJAMENTO ATRASADO"> ' + roundToTwo(T_PAtraso) + ' HRS - OCIOSOS</span>';

                            T_Reserva += f.DISPONIVEL;
                            //_Reserva = '&emsp;<span class="label label-default label-aps-n1-m2" title = "RESERVA"> ' + roundToTwo(T_Reserva) + ' HRS - RESERVA</span>';

                            T_M2AcabadoDia += f.M2_ACABADO_DIA;
                            _M2AcabadoDia = '&emsp;<span class="label label-default label-aps-n1 label-aps-n1-m2" title = "Mil. M² ACABADO"> ' + roundToTwo(T_M2AcabadoDia/1000.0) + ' Mil. M² - ACABADO</span>';

                            T_PesoAcabadoDia += f.PESO_ACABADO_DIA;
                            _PesoAcabadoDia = '&emsp;<span class="label label-default label-aps-n1 label-aps-n1-m2" title = "TL ACABADO"> ' + roundToTwo(T_PesoAcabadoDia/1000000.0) + ' TL - ACABADO</span>';

                            _menuFim = '        </a></h4>';
                            _menuFim += '    </div>';

                            _menuFim += '    <div id="c_' + id_href + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne4" aria-expanded="false" style="height: 0px;">';
                            _menuFim += '        <div class="panel-body">';
                            //_menuFim += '          <div onclick="$(\'#c_n1_'+f.DIA+'\').html(\'tettttt\');"  id="c_'+f.DIA+'" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne3" aria-expanded="true" style="">';
                            _menuFim += '                   <div id="c_n1' + id_href + '"></div>';
                            //_menuFim += '          </div>';
                            _menuFim += '</div>';
                            //_menuFim += '</div>';
                            _menuFim += '</div>';
                            _menuFim += '</div>';
                        });
                        _html += _menu + _EAtraso + _NaData + _Adiantado + _PAtraso + _M2AcabadoDia + _PesoAcabadoDia + _menuFim;

                        $('#cargaMaquina').html(_html);
                        TabelaCargaMaquina.carregarTabela('n2', '', '');
                        fecharIC();

                    } else {
                        Modal.erro('Erro ao carregar a tabela de carga maquina.' + result);
                        fecharIC();
                    }

                },
                error: function () {
                    Modal.erro('Erro ao carregar a tabela de carga maquina.' + result);
                },
            });

        }
        if (acao == 'n2') {
            $.get(UrlBase + 'ObterCargaMaquina?nivel=n2').done(function (r) {
                if (r.cargaMaquinaDia2 != null) {
                    _EAtraso = '';
                    _PAtraso = '';
                    _Adiantado = '';
                    _NaData = '';
                    dia = '';
                    _id = '';
                    _menu = '';
                    _menuFim = '';
                    _html = '';
                    id_href = '';

                    r.cargaMaquinaDia2.forEach(function (f) {
                        id_href = f.MED_DATA.substring(0, 4) + '_' + f.MED_DATA.substring(5, 7) + '_' + f.MED_DATA.substring(8, 10);
                        _id = '#c_n1' + id_href;

                        _menu = '<div class="panel panel-default">';
                        _menu += '<div class="panel-heading" role="tab" id="headingOne5"></div>';
                        _menu += '<div class="panel-body" role="tab" onclick="javascript:TabelaCargaMaquina.carregarTabela(\'n3_' + id_href + '-' + f.DIM_ID + '\',\'' + id_href + '-' + f.DIM_ID + '-' + f.TIPO + '\',\'\')">';
                        _menu += '<a style="width:100%; text-decoration:none;" data-toggle="collapse" href="#' + id_href + '-' + f.DIM_ID + '" aria-expanded="false" class="collapsed">';
                        _menu += '<div style="display:flex;">';

                        _menu += '<div style="width:5%;"><i class="fa fa-check"></i>';
                        _menu += '</div>';

                        var tipo = f.TIPO == "E" ? "EQUIPE" :
                            f.TIPO == "M" ? "MÁQUINA" :
                                f.TIPO == "D" ? "DOCA" : " ";
                        _menu += '<div style="width:10%;">' + tipo;
                        _menu += '</div>';

                        _menu += '<div style="width:10%;">' + f.DIM_ID + '';
                        _menu += '</div>';

                        _menu += '<div style="width:20%;">' + f.MAQ_DESCRICAO + '';
                        _menu += '</div>';

                        _menu += '<div style="width:60%;">'; {
                            T_EAtraso = f.ATRASO;
                            _menu += '&emsp;<span class="label label-danger label-aps-n2" title = "Entrega Atrasada"> ' + roundToTwo(T_EAtraso) + '</span>';
                            T_NaData = f.NA_DATA;
                            _menu += '&emsp;<span class="label label-success label-aps-n2" title = "Na data"> ' + roundToTwo(T_NaData) + '</span>';
                            T_Adiantado = f.ADIANTADO;
                            _menu += '&emsp;<span class="label label-primary label-aps-n2" title = "Antecipado"> ' + roundToTwo(T_Adiantado) + '</span>';
                            T_PAtraso = f.OCIOSO;
                            _menu += '&emsp;<span class="label label-default label-aps-n2" title = "Ociosidade"></i> ' + roundToTwo(T_PAtraso) + '</span>';
                            T_Reserva = f.DISPONIVEL;
                            _menu += '&emsp;<span class="label label-default label-aps-n2" title = "Reserva"> ' + roundToTwo(T_Reserva) + '</span>';
                            //T_M2AcabadoDia = f.M2_ACABADO_DIA;
                            //_menu += '&emsp;<span class="label label-default label-aps-n2" title = "M³ Acabado Dia"></i> ' + T_M2AcabadoDia + '</span>';
                            T_M2MaquinaDia = f.M2_MAQUINA_DIA;
                            _menu += '&emsp;<span class="label label-default label-aps-n2 label-aps-n1-m2" title = "Mil. M² PRODUZIDO"></i> ' + roundToTwo(T_M2MaquinaDia/1000.0) + '</span>';

                            T_PesoMaquinaDia = f.PESO_MAQUINA_DIA;
                            _menu += '&emsp;<span class="label label-default label-aps-n2 label-aps-n1-m2" title = "TL PRODUZIDO"></i> ' + roundToTwo(T_PesoMaquinaDia / 1000000.0) + '</span>';
                        }
                        _menu += '</div>';
                        _menu += '</div></a>'; //Fecha aqui;

                        _menuFim = '    </div>';
                        _menuFim += '<div id="' + id_href + '-' + f.DIM_ID + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne5" aria-expanded="false" style="height: 0px;">';
                        _menuFim += '<div class="panel-body">';
                        _menuFim += '<div id="n3_' + id_href + '-' + f.DIM_ID + '" >  Carrega Pedidos    </div>';
                        _menuFim += '</div></div></div>';

                        $(_id).append(_menu + _menuFim);
                    });
                    fecharIC();

                    //TabelaCargaMaquina.carregarTabela('n3', '', '');
                } else {
                    Modal.erro('Erro ao carregar a tabela de carga maquina.' + r);
                    fecharIC();
                }
            }).fail(function () {
                Modal.erro('Erro ao carregar a tabela de carga maquina.' + r);
            }).always(function () {
                fecharIC();
            });

        }
        if (acao.substring(0, 2) == 'n3') {
            var url = UrlBase + 'ObterCargaMaquina?nivel=n3&_data=' + data;
            $.get(url).done(function (r) {
                if (r.cargaMaquinaPedidos != null) {
                    _html = '';
                    cor = '#F9F9F9';
                    //Header
                    _html += '<div class="flow_aps" style="display: flex; width: 100%; background-color:#EEE">';
                    {
                        _html += '<div style="width:5%; display:flex;" title="ORDEM DE PRODUÇÃO">OP</div>';
                        _html += '<div style="width:5%; display:flex;" title="PEDIDO">PED</div>';
                        _html += '<div style="width:20%; display:flex;">PEDIDO ID</div>';
                        _html += '<div style="width:20%; display:flex;">CLIENTE</div>';
                        _html += '<div style="width:15%; display:flex;">DATA ENTREGA</div>';
                        _html += '<div style="width:10%; display:flex;">TEMPO OP</div>';
                        _html += '<div style="width:15%; display:flex;">INICIO PREVISTO</div>';
                        _html += '<div style="width:15%; display:flex;">FIM PREVISTO</div>';
                        _html += '<div style="width:25%; display:flex;">PRODUTO</div>';
                        _html += '<div style="width:10%; display:flex; title="QUANTIDADE"">QTD</div>';
                        _html += '<div style="width:20%; display:flex;">M² TOTAL: <br>' + roundToTwo(r.somatorio/1000.0) + '</div>';
                        _html += '<div style="width:20%; display:flex; title="PESO (TL)">PESO (TL)</div>';
                    }

                    _html += '</div>';

                    r.cargaMaquinaPedidos.forEach(function (f) {

                        var list_id = f.FPR_DATA_FIM_PREVISTA.split('T')[0].split('-');
                        var id = list_id[0] + '_' + list_id[1] + '_' + list_id[2];

                        _html += '<div class="flow_aps" style="display: flex; width: 100%; background-color:' + cor + '">';
                        {
                            _html += '<div style="width:5%; display:flex;"><div style="width:20px; height:20px; border-radius:50%; margin-top:5px; background-color:' + f.FPR_COR_FILA + '"></div></div>';
                            _html += '<div style="width:5%; display:flex;"><div style="width:20px; height:20px; border-radius:50%; margin-top:5px; background-color:' + f.ORD_COR_FILA + '"></div></div>';
                            _html += '<div style="width:20%; display:flex;" title="IR PARA O PEDIDO"><a onclick="alterarPedidoCargaMaq(\'' + f.ORD_ID + '\', \'/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.Order\', \'n3_' + id + '-' + f.ROT_MAQ_ID + '\')">' + f.ORD_ID + '</a></div>';
                            _html += '<div style="width:20%; display:flex;">' + f.CLI_NOME + '</div>';
                            _html += '<div style="width:15%; display:flex;">' + formatarDataHora(f.ORD_DATA_ENTREGA_DE) + '</div>';
                            _html += '<div style="width:10%; display:flex;">' + Math.round(f.TEMPO_OP / 60) + '</div>';
                            _html += '<div style="width:15%; display:flex;">' + formatarDataHora(f.FPR_DATA_INICIO_PREVISTA) + '</div>';
                            _html += '<div style="width:15%; display:flex;">' + formatarDataHora(f.FPR_DATA_FIM_PREVISTA) + '</div>';
                            _html += '<div style="width:25%;" title="' + f.ORD_PRO_ID + '">' + f.ORD_PRO_DESCRICAO + '</div>';
                            _html += '<div style="width:10%; display:flex;">' + f.ORD_QUANTIDADE + '</div>';
                            _html += '<div style="width:20%; display:flex;">' + roundToTwo(f.ORD_M2_TOTAL/1000.0) + '</div>';
                            _html += '<div style="width:20%; display:flex;">' + roundToTwo(f.ORD_PESO_TOTAL / 1000000.0) + '</div>';
                        }
                        _html += '</div>';

                        cor = cor == '#FFFFFF' ? '#F9F9F9' : '#FFFFFF';
                    });
                    $('#' + acao).html(_html);

                    fecharIC();
                } else {
                    Modal.erro('Erro ao carregar a tabela de carga maquina 2.');
                    fecharIC();
                }
            }).fail(function () {
                Modal.erro('Erro ao carregar a tabela de carga maquina.');
            }).always(function () {
                fecharIC();
            });
        }
    }


    function abrirIC() {
        $('#cmLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Carga Maquina');
    }
    function fecharIC() {
        $('#cmLoad').html('<i class="fa fa-check">  </i>Carga Maquina');
    }
}

function btnSimularCM() {
    var str_html = '';
    str_html += '<div id="divSimularCM">';
    //INICIO - INPUT DE PESQUISA DE PRODUTOS;
    str_html += '<div class="col-sm-4 col-md-4 col-xs-12 padding">';
    str_html += '<p class="col-sm-12 col-md-12"> COD PRODUTO *</p>';
    str_html += '<div class="input-group" id="datalistprincipal1">'
    str_html += '<input data-column="PRO_ID" list="listaprincipal2" name="lista" value="" class="form-control inputprincipal2 pos_2" id="inputOrdIdCM" placeholder="COD PRODUTO *" >';
    str_html += '<datalist class="dataprincipal2" id="listaprincipal2"></datalist>';
    str_html += '<span class="input-group-addon" id="" aria-hidden="">';
    str_html += '<button type="" class="fa fa-search" id="" data-dismiss="" aria-label="" data-toggle="" data-target="" style="background:transparent;border:none">';
    str_html += '</button>';
    str_html += '</span>';
    str_html += '<span class="input-group-addon" id="" aria-hidden="">';
    str_html += '<button type="" class="fa fa-arrow-circle-right" style="background:transparent;border:none"  onclick="prepararAbrirNovaAba(event, DynamicForms.Areas.PlugAndPlay.Models.Produto, 0, PRO_ID, PRO_ID, true)">';
    str_html += '</button>';
    str_html += '</span>';
    str_html += '</div>';
    str_html += '</div>';
    //FIM - INPUT DE PESQUISA DE PRODUTOS;

    //INICIO - INPUT DE PESQUISA DE CLIENTES;
    str_html += '<div class="col-sm-4 col-md-4 col-xs-12 padding">';
    str_html += '<p class="col-sm-12 col-md-12"> COD CLIENTE *</p>';
    str_html += '<div class="input-group" id="datalistprincipal1">'
    str_html += '<input data-column="CLI_ID" list="listaprincipal2" name="lista" value="" class="form-control inputprincipal2 pos_2" id="inputCliIdCM" placeholder="COD CLIENTE *" >';
    str_html += '<datalist class="dataprincipal3" id="listaprincipal3"></datalist>';
    str_html += '<span class="input-group-addon" id="" aria-hidden="">';
    str_html += '<button type="" class="fa fa-search" id="" data-dismiss="" aria-label="" data-toggle="" data-target="" style="background:transparent;border:none">';
    str_html += '</button>';
    str_html += '</span>';
    str_html += '<span class="input-group-addon" id="" aria-hidden="">';
    str_html += '<button type="" class="fa fa-arrow-circle-right" style="background:transparent;border:none"  onclick="prepararAbrirNovaAba(event, DynamicForms.Areas.PlugAndPlay.Models.CLIENTE, 0, CLI_ID, CLI_ID, true)">';
    str_html += '</button>';
    str_html += '</span>';
    str_html += '</div>';
    str_html += '</div>';
    //FIM - INPUT DE PESQUISA DE CLIENTES;


    //INICIO - INPUT DATA DE SIMULAÇÃO
    str_html += '<div class="col-sm-4 col-md-4 col-xs-12 padding" aria-hidden="">';
    str_html += '<p class="col-sm-12 col-md-12" for="ORD_DATA_ENTREGA_DE">DATA DE ENTREGA *</p>';
    str_html += '<input type="date" class="form-control inputprincipal4" id="inputDataEntregaCM" placeholder="DATA DE ENTREGA *" title="DATA DE ENTREGA *">';
    str_html += '</div>';
    //FIM - INPUT DATA DE SIMULAÇÃO

    //INICIO - INPUT DE QUANTIDADE
    str_html += '<div class="col-sm-4 col-md-4 col-xs-12 padding">';
    str_html += '<p class="col-sm-12 col-md-12" for="ORD_QUANTIDADE">QUANTIDADE *</p>';
    str_html += '<input type="number" class="form-control inputprincipal3" id="inputQuantidadeCM" placeholder="QUANTIDADE *" title="QUANTIDADE *">';
    str_html += '</div>';
    //FIM - INPUT DE QUANTIDADE

    //INICIO - INPUT DE QUANTIDADE
    /*
    str_html += '<div class="col-sm-4 col-md-4 col-xs-12 padding">';
    str_html += '<p class="col-sm-12 col-md-12" for="ORD_STATUS">STATUS PEDIDO *</p>';
    str_html += '<select class="menu-right-click form-control col-sm-6 col-md-6" id="inputStatusCM" style="" data-column="inputStatusCM">';
    str_html += '<option value="" selected="selected">VAZIO</option>';
    str_html += '<option value="">ABERTO </option>';
    str_html += '<option value="SS">SUSPENSO PRODUÇÃO E EXPEDIÇÃO</option>';
    str_html += '<option value="RE">RESERVA PRODUÇÃO</option>';
    str_html += '<option value="SE">SUSPENSO EXPEDIÇÃO</option>';
    str_html += '<option value="E">ENCERRADO</option>';
    str_html += '<option value="EI">ENCERRADO PELA INTERFACE</option>';
    str_html += '</select>';
    str_html += '</div>';
    */
    //FIM - INPUT DE QUANTIDADE


    //INICIO - BOTÕES
    str_html += '<div class="col-sm-12 col-md-12 col-xs-12 padding">';
    str_html += '<div class="col-sm-2 col-md-2"><button type="submit" class="btn btn-success" id="btnSimularPedido" onclick="simularPedido()">Simular</button></div>';
    str_html += '<div class="col-sm-2 col-md-2"><button type="submit" class="btn btn-secondary" id="btnCancelarSimulacao" onclick="cancelarSimulacao()">Cancelar</button></div>';
    str_html += '</div>';
    //FIM BOTÕES

    str_html += '<div class="col-sm-12 col-md-12 col-xs-12 padding" id="msgSimulacaoCM"></div>';

    //headingOne4
    str_html += '</div>';

    $('#divSimularCM').remove()
    $('#menuexp').append(str_html);
}

var pedidoCargaMaquina;
function simularPedido() {

    var pro_id = $('#inputOrdIdCM').val();
    var data_entrega = $('#inputDataEntregaCM').val();
    var quantidade = $('#inputQuantidadeCM').val();
    var cli_id = $('#inputCliIdCM').val();
    var ord_status = $('#inputStatusCM').val();
    var str_html = '';
    if (quantidade <= 1000000) {
        $.ajax({
            type: 'GET',
            url: UrlBase + 'SimularPedidoCargaMaquina?pro_id=' + pro_id + '&data_entrega_str=' + data_entrega + '&quantidade=' + quantidade + '&cli_id=' + cli_id,
            dataType: 'json',
            traditional: true,
            async: false,
            contenttype: "application/json; charset=utf-8",
            success: function (result) {
                if (result.status == "OK") {
                    str_html += '<div id="msgSimulacaoCMRes" class="col-sm-12 col-md-12 col-xs-12 padding sucesso">';
                    str_html += '<p>O Pedido pode ser agendado para esta data! Deseja incluir na fila de produção? </p>';

                    str_html += '<div class="col-sm-12 col-md-12 col-xs-12 padding">';
                    str_html += '<div class="col-sm-2 col-md-2"><button type="submit" class="btn btn-success" id="salvar0" onclick="incluirPedidoFila()">Sim</button></div>';//'++', '++', '++'
                    str_html += '<div class="col-sm-2 col-md-2"><button type="button" class="btn btn-secondary" id="cancelar0" onclick="CloseDiv();">Não</button></div>';

                    str_html += '</div>';
                    str_html += '</div>';
                    console.log(result.prazoMinimo);


                    pedidoCargaMaquina = {
                        pro_id: pro_id,
                        data_entrega: data_entrega,
                        quantidade: quantidade,
                        cli_id: cli_id,
                        ord_status: ord_status,
                        data_entrega: result.prazoMinimo,
                        fila: result.logs
                    };
                }
                else if (result.status == "ADIAR") {
                    str_html += '<div id="msgSimulacaoCMRes" class="col-sm-12 col-md-12 col-xs-12 padding erro">';
                    str_html += '<p>Impossível entregar o pedido esta data! A primeira data disponível é: </p>';
                    str_html += '<p>' + result.prazoMinimo.substring(8, 10) + '/' + result.prazoMinimo.substring(5, 7) + '</p>';
                    str_html += '<p>Deseja realizar a venda para esta data?</p>';

                    str_html += '<div class="col-sm-12 col-md-12 col-xs-12 padding">';
                    str_html += '<div class="col-sm-2 col-md-2"><button type="submit" class="btn btn-success" id="salvar0" onclick="">Sim</button></div>';
                    str_html += '<div class="col-sm-2 col-md-2"><button type="button" class="btn btn-secondary" id="cancelar0" onclick="CloseDiv();">Não</button></div>';

                    str_html += '</div>';
                    str_html += '</div>';
                }
                else if (result.status == "ERRO") {
                    alert('STATUS: ' + result.status + '\nMENSAGEM: ' + result.msgRetorno);
                }

                if (result.logs.length > 0) {
                    str_html += '<div id="msgSimulacaoCMLogs"><div>LOGS: </div>';
                    str_html += '<div>';

                    for (var i = 0; i < result.logs.length; i++) {
                        var data_inicio = result.logs[i].DataInicioPrevista;
                        data_inicio = data_inicio.substring(8, 10) + '/' + data_inicio.substring(5, 7) + '/' + data_inicio.substring(0, 4);

                        var data_fim = result.logs[i].DataFimPrevista;
                        data_fim = data_fim.substring(8, 10) + '/' + data_fim.substring(5, 7) + '/' + data_fim.substring(0, 4);

                        str_html += '<p style="LINE-HEIGHT:5px;">MAQ_ID: ' + result.logs[i].MaquinaId + ' </p>';
                        str_html += '<p style="LINE-HEIGHT:5px;">SEQ. TRANS: ' + result.logs[i].SequenciaTransformacao + ' </p>';
                        str_html += '<p style="LINE-HEIGHT:5px;">DATA INICIO: ' + data_inicio + ' </p>';
                        str_html += '<p style="LINE-HEIGHT:5px;">DATA FIM: ' + data_fim + ' </p>';

                        str_html += '<hr>';
                    }

                    str_html += '<p style="LINE-HEIGHT:5px;">INICIO JANELA EMBARQUE: ' + result.inicioJanelaEmbarque + ' </p>';
                    str_html += '<p style="LINE-HEIGHT:5px;">FIM JANELA EMBARQUE: ' + result.fimJanelaEmbarque + ' </p>';
                    str_html += '<p style="LINE-HEIGHT:5px;">EMBARQUE ALVO: ' + result.embarqueAlvo + ' </p>';
                    str_html += '<hr>';

                    str_html += '</div></div>';
                }

                console.log('LOGS Simulação de pedido');
                console.log(result.logs);
                $('#msgSimulacaoCMRes').remove();
                $('#msgSimulacaoCMLogs').remove();
                $('#msgSimulacaoCM').append(str_html);

            }

        });
    } else { alert(`Quantidade acima do limite de 1000000`); }

}

function CloseDiv() { $('#msgSimulacaoCMRes').remove(); $('#msgSimulacaoCMLogs').remove(); }

function incluirPedidoFila() {
    console.log(pedidoCargaMaquina);
    var str_fila = JSON.stringify(pedidoCargaMaquina.fila);

    $.ajax({
        type: 'POST',
        url: UrlBase + 'IncluirPedidoFila',
        data: {
            cli_id: pedidoCargaMaquina.cli_id,
            ord_status: pedidoCargaMaquina.ord_status,
            pro_id: pedidoCargaMaquina.pro_id,
            quantidade: pedidoCargaMaquina.quantidade,
            data_entrega: pedidoCargaMaquina.data_entrega,
            fila: str_fila
        },
        dataType: 'json',
        traditional: true,
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) { }
    });
}

var htmlData = "";
function consultaFilaTotal(rot_maq_id = "") {
    var dados = "";
    var namespace = "DynamicForms.Areas.PlugAndPlay.Models.Consultas";
    var nameMethod = 'IniciarConsulta';
    var parametros = '';
    if (rot_maq_id == "") {
        dados = '[{"CON_ID":44,"CON_DESCRICAO":"FILA_PRODUCAO","CON_GRUPO":"","CON_COMAND":"SELECT * FROM V_FILA_PRODUCAO","CON_CONEXAO":"","CON_TITULO":"","CON_TIPO":"","PlayAction":"","PlayMsgErroValidacao":"","IndexClone":null}]';
        parametros = 'ROT_MAQ_ID_0=;ROT_MAQ_ID_1=z';
        //dados = '[{"CON_ID":45,"CON_DESCRICAO":"FILA_PRODUCAO","CON_GRUPO":"","CON_COMAND":"SELECT * FROM V_FILA_PRODUCAO","CON_CONEXAO":"","CON_TITULO":"","CON_TIPO":"","PlayAction":"","PlayMsgErroValidacao":"","IndexClone":null}]';
    }
    else {
        dados = '[{"CON_ID":44,"CON_DESCRICAO":"FILA_PRODUCAO","CON_GRUPO":"","CON_COMAND":"SELECT * FROM V_FILA_PRODUCAO","CON_CONEXAO":"","CON_TITULO":"","CON_TIPO":"","PlayAction":"","PlayMsgErroValidacao":"","IndexClone":null}]';
        parametros = 'ROT_MAQ_ID_0=' + rot_maq_id + ';ROT_MAQ_ID_1=' + rot_maq_id;
    }


    $.ajax({
        type: 'POST',
        url: "/DynamicWeb/ExecuteMethod",
        data: { list_obj: dados, class_name: namespace, name_method: nameMethod, parametros: parametros },
        dataType: "json",
        traditional: true,
        async: false,
        contenttype: "application/json; charset=utf-8",
        success: function (result) {
            executarProtocolos(result.log);

            for (i = 0; i < result.log.length; i++) {

                if (result.log[i].Status === "ERRO") {
                    mostraDialogo(result.log[i].MsgErro, 'danger', 3000);
                    $(".exibir").append(result.log[i].MsgErro);
                }
                if (result.log[i].Status === "OK") {
                    mostraDialogo(result.log[i].MsgErro, 'success', 3000);
                    $(".exibir").append(result.log[i].MsgErro);
                }
            }
        },
        error: OnError,
        complete: onComplete
    })

}

function cancelarSimulacao() {
    $('#divSimularCM').remove();
}

var href_id = '';
function alterarPedidoCargaMaq(ord_id, namespace, _href_id) {
    var str = global_modal(ord_id, namespace, 1);
    if (str != null) {
        href_id = _href_id;
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

function atualizarTabelaCargaMaquina() {
    TabelaCargaMaquina.carregarTabela(href_id, href_id.split('n3_')[1], '');
}

function validarData(data_ini) {
    var bissexto = false;
    if ((data_ini.ano % 4 == 0) && ((data_ini.ano % 100 != 0) || (data_ini.ano % 400 == 0)))
        bissexto = true;

    if (data_ini.dia == 29 && data_ini.mes == 2 && !bissexto) {
        data_ini.dia = 1;
        data_ini.mes++;
    }
    if (data_ini.dia == 30 && data_ini.mes == 2 && bissexto) {
        data_ini.dia = 1;
        data_ini.mes++;
    }
    else if (data_ini.dia == 31 && (data_ini.mes == 4 || data_ini.mes == 6 || data_ini.mes == 9 || data_ini.mes == 11)) {
        data_ini.dia = 1;
        data_ini.mes++;
    }
    else if (data_ini.dia == 32 && (data_ini.mes == 1 || data_ini.mes == 3 || data_ini.mes == 5 || data_ini.mes == 7 || data_ini.mes == 8 || data_ini.mes == 10 || data_ini.mes == 12)) {
        data_ini.dia = '01';
        data_ini.mes++;
    }

    if (data_ini.dia < 10) {
        data_ini.dia = '0' + parseInt(data_ini.dia);
    }

    if (data_ini.mes < 10) {
        data_ini.mes = '0' + parseInt(data_ini.mes);
    }

    if (data_ini.mes == 13)
        data_ini.mes = 1;

    return data_ini;
}

/*Modelo com paginação
var qtd_paginas_fila_producao = 0;
function eventoKeyPressFilaProducao(event) {
    var tecla = event.which || event.keyCode;
    var acao;
    if (tecla == 13) //13 Corresponde a tecla Enter.
    {
        var num_digitado = $('#semanaFilaProducaoInput').val();
        if (num_digitado >= 1 && num_digitado <= qtd_paginas_fila_producao) {
            TabelaFilaProducao.carregarTabela(acao, num_digitado - 1);

            //$('html, body').animate({ scrollTop: $("#ancora" + tab).offset().top }, 600);
            $('html, body').animate({ scrollTop: $("#collapseThree-4 .panel-body .content-body").offset().top - 60 }, 600);
        }
        else {
            alert('Semana inválida!');
        }
    }
}*/

/* Modelo com paginação
function montarPaginasFilaProducao(qtd_paginas, ativo, acao) {
    var str = "";

    str = '<div id="paginacao_fila" class="ctrlPag" align="center">';
    str += '<ul class="pagination">';
    str += ativo == 1 ?
        '<li class="page-item pg disabled"><a href="#")" disabled>Anterior</a></li>' :
        '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + (ativo - 1) + ',' + acao + ')">Anterior</a></li>';

    if (qtd_paginas > 9) { //INICIO
        if (ativo < 5) {
            for (let i = 1; i <= ativo + 2; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>';
            }
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            str += '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + qtd_paginas + ',' + acao + ')">' + qtd_paginas + '</a></li>';
        }
        else if (ativo > qtd_paginas - 4) { //FIM
            str += '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(1,' + acao + ') ">1</a></li>';
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            for (let i = ativo - 2; i <= qtd_paginas; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>';
            }
        }
        else { //MEIO
            str += '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(1,' + acao + ') ">1</a></li>';
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            for (let i = ativo - 2; i <= ativo + 2; i++) {
                str += ativo == i ?
                    '<li class="page-item active pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>' :
                    '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>';
            }
            str += '<li class="page-item pg disabled"><a>...</a></li>';
            str += '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + qtd_paginas + ',' + acao + ')">' + qtd_paginas + '</a></li>';
        }
    }
    else {
        for (let i = 1; i <= qtd_paginas; i++) {
            str += ativo == i ?
                '<li class="page-item active pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>' :
                '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + i + ',' + acao + ')">' + i + '</a></li>';
        }
    }
    str += ativo == qtd_paginas ?
        '<li class="page-item pg disabled"><a href="#">Proximo</a></li>' :
        '<li class="page-item pg"><a href="#" onclick="evtMudarPaginaFilaProducao(' + (ativo + 1) + ',' + acao + ')">Proximo</a></li>';
    //str += '<li class="page-item pg"><a href="">Proximo</a></li>';
    str += '</ul></div>';
    return str;
}

function evtMudarPaginaFilaProducao(ativo, acao) {
    TabelaFilaProducao.carregarTabela(acao, ativo - 1);
}*/

var _escala = 1;
var _ativo;
function evtMenosEscala() {
    if (_escala > 1) {
        _escala -= 3;
        if (_escala <= 1)
            border_width_fila_producao = '2px';
        $('.sticky-fila-maquinas .ped-fila').remove();

        var acao;
        TabelaFilaProducao.carregarTabela(acao, _ativo);
        $('html, body').animate({ scrollTop: $("#collapseThree-4 .panel-body .content-body").offset().top - 60 }, 600);
    }
    else {

        alert('A escala já está no valor mínimo!');
    }
}

function evtMaisEscala() {
    _escala += 3;
    border_width_fila_producao = '3px';

    $('.sticky-fila-maquinas .ped-fila').remove();

    var acao;
    TabelaFilaProducao.carregarTabela(acao, _ativo);
    $('html, body').animate({ scrollTop: $("#collapseThree-4 .panel-body .content-body").offset().top - 60 }, 600);
}


var classe_atual;
var _cor_antiga = [];
function pintarPedidos(classe) {
    var antigo = document.getElementsByClassName(classe_atual);
    for (let i = 0; i < _cor_antiga.length; i++) {
        antigo[i].style.backgroundColor = _cor_antiga[i];
    }

    //_cor_antiga = cor_antiga;
    _cor_antiga = [];
    classe_atual = classe;

    var atual = document.getElementsByClassName(classe_atual);
    for (let i = 0; i < atual.length; i++) {
        _cor_antiga.push(atual[i].style.backgroundColor);
        atual[i].style.backgroundColor = "#c4c4c4";
    }
}

function pintarPadrao() {
    var antigo = document.getElementsByClassName(classe_atual);
    for (let i = 0; i < _cor_antiga.length; i++) {
        antigo[i].style.backgroundColor = _cor_antiga[i];
    }
}


var horario_inicial_total;
var border_width_fila_producao = '2px';
var TabelaFilaProducao = new function () {
    //(1) variaveis globais ==>
    // 1.1 - 
    //(1) publicos ===>

    var _html = '';

    this.iniciar = function () {
        $(document).on('click', '#tabFilaProducao .btnEditar', clickBtnEditar);
        $(document).on('click', '#tabFilaProducao .btnExcluir', clickBtnExluir);
        $(document).on('click', '#btnRecalcFila', recalculaFila);

        fecharIC();

        //fecha indicador de atrazo(carregando) na resposta de todas as solicotacoes ajax
    }
    this.carregarTabela = function (acao, ativo = 0) {
        //monta tabela fila de produção
        abrirIC();
        var i = 0;
        var MinutoFimAnterior = 0;
        var Maq = '';
        let visualizacao_maquina_equipe_checked = $('#visualizacaoMaquinaEquipe').is(':checked');

        removePedidos();

        $.get(UrlBase + 'ObterFila?recalculaFila=' + acao + '&pagina=' + ativo + '&visualizacaoMaquinaEquipe=' + visualizacao_maquina_equipe_checked).done(function (fila) {
            if (fila.fila2 != null && fila.fila2.length > 0) {
                console.log('O usuário logado é: ' + fila.usuario_logado);

                var data_ini = {
                    dia: fila.DtIni.split('-')[2].split('T')[0],
                    mes: fila.DtIni.split('-')[1],
                    ano: fila.DtIni.split('-')[0]
                };

                for (var x = 0; x <= fila.QtdHorasLinhaTempo; x++) {
                    //if (x <= 144) {// mostra apenas 6 dias em horas 

                    if (x == 0) {
                        dia = '<div rel="popover" data-animate=" animated fadeIn " data-container="body" data-toggle="popover"  data-placement="top" data-content="Linha do tempo" data-title="Top popover" data-trigger="hover" data-html="true" data-original-title="" title="">';
                        dia2 = '</div>';

                        if (fila.MinutoIni > 30) {

                            var mini = 60 - fila.MinutoIni;
                            _html = '<div id="linhaTempo' + data_ini.dia + '-' + fila.horaIni + '" class="linhaTempo"></div>';
                            $('#contemLinhaDoTempo').append(_html);
                        } else {
                            // exemplo 16:25   vai dar uma primeira parte de 5 minutos e outra parte de meia hora
                            mini = 30 - fila.MinutoIni;
                            //_html = '<div id="linhaTempo' + data_ini.dia + '-' + fila.horaIni + '" class="linhaTempo"></div>';
                            //$('#contemLinhaDoTempo').append(_html);
                            _html = '<div id="linhaTempo' + data_ini.dia + '-' + fila.horaIni + '" class="linhaTempo">' + data_ini.dia + '/' + data_ini.mes + '<br>' + fila.horaIni + '</div>';
                            $('#contemLinhaDoTempo').append(_html);
                        }
                    }

                    if (fila.horaIni == 23) {
                        fila.horaIni = -1;
                        data_ini.dia++;
                        data_ini = validarData(data_ini);
                    }
                    fila.horaIni++;
                    _html = '<div id="linhaTempo' + data_ini.dia + '-' + fila.horaIni + '" class="linhaTempo">' + data_ini.dia + '/' + data_ini.mes + '<br>' + fila.horaIni + ' </div > ';
                    $('#contemLinhaDoTempo').append(_html);

                    //_html = '<div class="linhaTempo">' + fila.horaIni + '</div>';
                    //$('#contemLinhaDoTempo').append(_html);

                    //}
                }

                var width = $('.linhaTempo').width();
                $('.linhaTempo').css('width', width * _escala);

                //Espaço em branco inicial
                horario_inicial_total = fila.DtIni;
                var tamanho = fila.MinutoIni * _escala;
                var _html = '<div onclick="pintarPadrao()" class="ped-fila"  style="background-color: 0a000e; width:' + (tamanho) + 'px;  display:block" >_</div>';
                //var _html_ond = '<div onclick="pintarPadrao()" class="ped-fila"  style="background-color: #fff; height:100px;  width:' + (tamanho + fila.minIniOndu) + 'px;  display:block" >';
                var _html_ond = '<div>TESTE';
                _html_ond += '<div title="Facão 1" style="background-color:#ccc; border-right: 2px double #000; width:20px; height:50px"><p style="color:#000; padding-top:15px;">F1</p></div>';
                _html_ond += '<div title="Facão 2" style="background-color:#ccc; border-top: 2px solid #000; border-right: 2px double #000; width:20px; height:50px"><p style="color:#000; padding-top:15px;">F2</p></div>';

                _html_ond += '</div>'
                $('.sticky-fila-maquinas div').html(_html);
                $('.maquina-fila-onduladeira').prepend(_html_ond);

                fila.fila2.forEach(function (f) {
                    //gera as linhas da tabela
                    //o primeiro registro é escala de tempo 
                    //alimenta variavel length do vetor por o javascripit nao implementou essa variavel no array nao sei pq
                    var destino_op = (f.estado_op == 1 && visualizacao_maquina_equipe_checked) ? f.equipeId : f.maquinaId;

                    if (f.estado_op == 1 && visualizacao_maquina_equipe_checked) {
                        if (Maq != f.equipeId) {
                            aMaquinas[i] = f.equipeId;
                            i++;
                            aMaquinas.length = i;
                            Maq = f.equipeId;
                            MinutoFimAnterior = 0; // primeiro registro da maquina 
                            MinutoIniAnterior = 0;
                        }

                        //esconde a maquina vinculada a op
                        $(`[id='${f.maquina}_${f.maquinaId}']`).hide();
                    }
                    else if (Maq != f.maquinaId) {
                        aMaquinas[i] = f.maquinaId;
                        i++;
                        aMaquinas.length = i;
                        Maq = f.maquinaId;
                        MinutoFimAnterior = 0; // primeiro registro da maquina 
                        MinutoIniAnterior = 0;

                        $(`[id='${f.maquina}_${f.maquinaId}']`).show();
                    }

                    //IniTruncado
                    //inielse truncado kkkkk
                    // verifica se tem espaços entre os horarios 
                    tamanho = f.MinutoIni - MinutoFimAnterior;
                    if (tamanho > 0) { // tem espaco em branco 
                        tamanho *= _escala;
                        _html = '<div class="ped-fila" onclick="pintarPadrao()"  style="background-color: 0a000e; width:' + (tamanho) + 'px;  display:block" ></div>';

                        $(`#maq_${destino_op}`).append(_html);
                    }

                    var temp_id = f.pedidoId.replaceAll('/', '_');
                    let cor_somente_producao = f.OrdTipo == 2 ? "black" : "white";

                    if ((f.MinutoIniTrunc > '0' && f.MinutoFimTrunc > '0') && (f.MinutoIniTrunc > f.MinutoIni && f.MinutoFimTrunc < f.MinutoFim)) {
                        var borderColor = f.CorOrd;
                        if (f.Status == 'E1' || f.Status == 'R1' || f.Status == 'R2') {
                            borderColor = '#81d6b5';
                        }

                        tamanho1 = f.MinutoIniTrunc - f.MinutoIni;
                        tamanho1 *= _escala;
                        _html = '<div onmouseout="javascript:deselPedidos(\'' + f.pedidoId + '\', \'' + f.CorOrd + '\');" onmouseover="javascript:selPedidos(\'' + f.pedidoId + '\');"onclick="javascript:pintarPedidos(\'pedId_' + temp_id + '\', \'' + f.CorFila + '\')" ondblclick="javascript:dadosPed(\'' + f.equipeId + '\',\'' + f.grpDescricao + '\',\'' + f.hieranquiaSeqTransf + '\',\'' + f.GrupoProdutivo + '\',\'' + f.CorBico5 + '\',\'' + f.CorBico4 + '\',\'' + f.CorBico3 + '\',\'' + f.CorBico2 + '\',\'' + f.CorBico1 + '\',\'' + f.grupoMaquinaId + '\',\'' + f.TempoSetupAString + '\',\'' + f.TempoSetupString + '\',\'' + f.PerformanceString + '\',\'' + f.M2Total + '\',\'' + f.M2Unitario + '\',\'' + f.PecasPorPulso + '\',\'' + f.CliNome + '\',\'' + f.CliId + '\',\'' + f.maquina + '\',\'' + f.maquinaId + '\',\'' + f.produtoId + '\',\'' + f.pedidoId + '\',\'' + f.produto + '\',\'' + moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.seqTransform + '\',\'' + f.seqRepet + '\',\'' + (f.MinutoFim - f.MinutoIni) + '\',\'' + f.Id + '\',\'' + f.CongelaFila + '\',\'' + f.OrdemDaFila + '\',\'' + moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.qtd + '\',\'' + moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.OrdOpIntegracao + '\',\'' + f.OrdTipo + '\',\'' + f.Status + '\');" class="ped-fila pedId_' + temp_id + '" id="pedId_' + f.pedidoId + '" style="background-color: ' + f.CorFila + '; width:' + tamanho1 + 'px; border-style: dashed; border-width: ' + border_width_fila_producao + '; border-color: ' + borderColor + '; color:' + cor_somente_producao + ';" data-toggle="tooltip" title="' + f.pedidoId + '">' + f.OrdOpIntegracao + '</div>';
                        $(`#maq_${destino_op}`).append(_html);

                        tamanho2 = f.MinutoFimTrunc - f.MinutoIniTrunc;
                        tamanho2 *= _escala;
                        _html = '<div onmouseout="javascript:deselPedidos(\'' + f.pedidoId + '\', \'' + f.CorOrd + '\');" onmouseover="javascript:selPedidos(\'' + f.pedidoId + '\');"onclick="javascript:pintarPedidos(\'pedId_' + temp_id + '\', \'' + f.CorFila + '\')" ondblclick="javascript:dadosPed(\'' + f.equipeId + '\',\'' + f.grpDescricao + '\',\'' + f.hieranquiaSeqTransf + '\',\'' + f.GrupoProdutivo + '\',\'' + f.CorBico5 + '\',\'' + f.CorBico4 + '\',\'' + f.CorBico3 + '\',\'' + f.CorBico2 + '\',\'' + f.CorBico1 + '\',\'' + f.grupoMaquinaId + '\',\'' + f.TempoSetupAString + '\',\'' + f.TempoSetupString + '\',\'' + f.PerformanceString + '\',\'' + f.M2Total + '\',\'' + f.M2Unitario + '\',\'' + f.PecasPorPulso + '\',\'' + f.CliNome + '\',\'' + f.CliId + '\',\'' + f.maquina + '\',\'' + f.maquinaId + '\',\'' + f.produtoId + '\',\'' + f.pedidoId + '\',\'' + f.produto + '\',\'' + moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.seqTransform + '\',\'' + f.seqRepet + '\',\'' + (f.MinutoFim - f.MinutoIni) + '\',\'' + f.Id + '\',\'' + f.CongelaFila + '\',\'' + f.OrdemDaFila + '\',\'' + moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.qtd + '\',\'' + moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.OrdOpIntegracao + '\',\'' + f.OrdTipo + '\',\'' + f.Status + '\');" class="ped-fila pedId_' + temp_id + '" id="pedId_' + f.pedidoId + '" style="background-color: ' + f.CorFila + '; width:' + tamanho2 + 'px; background-repeat: repeat-x; background-image: url(/images/fundo_op_truncado.png); border-style: dashed; border-width: ' + border_width_fila_producao + '; border-color: ' + borderColor + '; color:' + cor_somente_producao + ';" data-toggle="tooltip" title="' + f.pedidoId + '"></div>';
                        $(`#maq_${destino_op}`).append(_html);

                        tamanho3 = f.MinutoFim - f.MinutoFimTrunc;
                        tamanho3 *= _escala;
                        _html = '<div onmouseout="javascript:deselPedidos(\'' + f.pedidoId + '\', \'' + f.CorOrd + '\');" onmouseover="javascript:selPedidos(\'' + f.pedidoId + '\');"onclick="javascript:pintarPedidos(\'pedId_' + temp_id + '\', \'' + f.CorFila + '\')" ondblclick="javascript:dadosPed(\'' + f.equipeId + '\',\'' + f.grpDescricao + '\',\'' + f.hieranquiaSeqTransf + '\',\'' + f.GrupoProdutivo + '\',\'' + f.CorBico5 + '\',\'' + f.CorBico4 + '\',\'' + f.CorBico3 + '\',\'' + f.CorBico2 + '\',\'' + f.CorBico1 + '\',\'' + f.grupoMaquinaId + '\',\'' + f.TempoSetupAString + '\',\'' + f.TempoSetupString + '\',\'' + f.PerformanceString + '\',\'' + f.M2Total + '\',\'' + f.M2Unitario + '\',\'' + f.PecasPorPulso + '\',\'' + f.CliNome + '\',\'' + f.CliId + '\',\'' + f.maquina + '\',\'' + f.maquinaId + '\',\'' + f.produtoId + '\',\'' + f.pedidoId + '\',\'' + f.produto + '\',\'' + moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.seqTransform + '\',\'' + f.seqRepet + '\',\'' + (f.MinutoFim - f.MinutoIni) + '\',\'' + f.Id + '\',\'' + f.CongelaFila + '\',\'' + f.OrdemDaFila + '\',\'' + moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.qtd + '\',\'' + moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.OrdOpIntegracao + '\',\'' + f.OrdTipo + '\',\'' + f.Status + '\');" class="ped-fila pedId_' + temp_id + '" id="pedId_' + f.pedidoId + '" style="background-color: ' + f.CorFila + '; width:' + tamanho3 + 'px; border-style: dashed; border-width: ' + border_width_fila_producao + '; border-color: ' + borderColor + '; color:' + cor_somente_producao + ';" data-toggle="tooltip" title="' + f.pedidoId + '">' + f.OrdOpIntegracao + '</div>';
                        $(`#maq_${destino_op}`).append(_html);

                    }
                    else {
                        tamanho = f.MinutoFim - f.MinutoIni;
                        tamanho *= _escala; //O código de escala fica bem aqui 

                        if (f.Status == 'E1' || f.Status == 'R1' || f.Status == 'R2') {
                            //Referente a cargas simuladas
                            //E1: Simulação
                            //R1: Reserva forte
                            //R2: Reserva fraca
                            _html = '<div onmouseout="javascript:deselPedidos(\'' + f.pedidoId + '\', \'' + f.CorOrd + '\', \'' + f.Status + '\');" onmouseover="javascript:selPedidos(\'' + f.pedidoId + '\');"onclick="javascript:pintarPedidos(\'pedId_' + temp_id + '\',\'' + f.CorFila + '\')" ondblclick="javascript:dadosPed(\'' + f.equipeId + '\',\'' + f.grpDescricao + '\',\'' + f.hieranquiaSeqTransf + '\',\'' + f.GrupoProdutivo + '\',\'' + f.CorBico5 + '\',\'' + f.CorBico4 + '\',\'' + f.CorBico3 + '\',\'' + f.CorBico2 + '\',\'' + f.CorBico1 + '\',\'' + f.grupoMaquinaId + '\',\'' + f.TempoSetupAString + '\',\'' + f.TempoSetupString + '\',\'' + f.PerformanceString + '\',\'' + f.M2Total + '\',\'' + f.M2Unitario + '\',\'' + f.PecasPorPulso + '\',\'' + f.CliNome + '\',\'' + f.CliId + '\',\'' + f.maquina + '\',\'' + f.maquinaId + '\',\'' + f.produtoId + '\',\'' + f.pedidoId + '\',\'' + f.produto + '\',\'' + moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.seqTransform + '\',\'' + f.seqRepet + '\',\'' + (f.MinutoFim - f.MinutoIni) + '\',\'' + f.Id + '\',\'' + f.CongelaFila + '\',\'' + f.OrdemDaFila + '\',\'' + moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.qtd + '\',\'' + moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.OrdOpIntegracao + '\',\'' + f.OrdTipo + '\',\'' + f.Status + '\');" class="ped-fila pedId_' + temp_id + '" id="pedId_' + f.pedidoId + '" style="background-color: ' + f.CorFila + '; width:' + tamanho + 'px; border-style: dashed; border-color: #81d6b5; border-width: ' + border_width_fila_producao + '; color: ' + cor_somente_producao + ';" data-toggle="tooltip" title="' + f.pedidoId + '">' + f.OrdOpIntegracao + '</div>';
                        }
                        else {
                            _html = '<div onmouseout="javascript:deselPedidos(\'' + f.pedidoId + '\', \'' + f.CorOrd + '\');" onmouseover="javascript:selPedidos(\'' + f.pedidoId + '\');"onclick="javascript:pintarPedidos(\'pedId_' + temp_id + '\', \'' + f.CorFila + '\')" ondblclick="javascript:dadosPed(\'' + f.equipeId + '\',\'' + f.grpDescricao + '\',\'' + f.hieranquiaSeqTransf + '\',\'' + f.GrupoProdutivo + '\',\'' + f.CorBico5 + '\',\'' + f.CorBico4 + '\',\'' + f.CorBico3 + '\',\'' + f.CorBico2 + '\',\'' + f.CorBico1 + '\',\'' + f.grupoMaquinaId + '\',\'' + f.TempoSetupAString + '\',\'' + f.TempoSetupString + '\',\'' + f.PerformanceString + '\',\'' + f.M2Total + '\',\'' + f.M2Unitario + '\',\'' + f.PecasPorPulso + '\',\'' + f.CliNome + '\',\'' + f.CliId + '\',\'' + f.maquina + '\',\'' + f.maquinaId + '\',\'' + f.produtoId + '\',\'' + f.pedidoId + '\',\'' + f.produto + '\',\'' + moment(f.inicioPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.fimPrevisto).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.seqTransform + '\',\'' + f.seqRepet + '\',\'' + (f.MinutoFim - f.MinutoIni) + '\',\'' + f.Id + '\',\'' + f.CongelaFila + '\',\'' + f.OrdemDaFila + '\',\'' + moment(f.PrevisaoMateriaPrima).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.qtd + '\',\'' + moment(f.EntregaDe).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + moment(f.EntregaAte).format("DD/MM/YYYY HH:mm:ss") + '\',\'' + f.OrdOpIntegracao + '\',\'' + f.OrdTipo + '\',\'' + f.Status + '\');" class="ped-fila pedId_' + temp_id + '" id="pedId_' + f.pedidoId + '" style="background-color: ' + f.CorFila + '; width:' + tamanho + 'px; border-style: dashed; border-width: ' + border_width_fila_producao + '; border-color: ' + f.CorOrd + '; color:' + cor_somente_producao + ';" data-toggle="tooltip" title="' + f.pedidoId + '">' + f.OrdOpIntegracao + '</div>';
                        }

                        if (f.OrdOpIntegracao == '122578' && f.maquina == 'IMP 04 TOM')
                            console.log('Tamanho: ' + tamanho);

                        $(`#maq_${destino_op}`).append(_html);

                        _html = '';
                    }

                    //endelse truncado_kkkkk

                    MinutoFimAnterior = f.MinutoFim;
                    MinutoIniAnterior = f.MinutoIni;

                });

                for (var y = 0; y < aMaquinas.length; y++) {
                    $("#maq_" + aMaquinas[y]).sortable();
                    $("#maq_" + aMaquinas[y]).disableSelection();
                }
                if (acao == 'sim') {
                    ListaPendenciasCadastrais.carregarTabela();
                }

            } else {
                Modal.erro('Erro ao carregar a fila de produção.' + fila);
                fecharIC();
            }
        }).fail(function () {
            Modal.erro('Erro ao carregar a fila de produção.' + fila);
        }).always(function () {
            fecharIC();
        });
    }
    this.evtMudouRadioVisualizacao = function (event) {
        //ao mudar o valor do radio, deve recarregar a tabela trazendo ou nao as equipes
        TabelaFilaProducao.carregarTabela();

        //esconde ou mostra as linhas de equipe da tabela
        if (event.target.value == 1) {
            $('.equipes').show();
        }
        else {
            $('.equipes').hide();
        }
    }

    function recalculaFila() {//botao editar
        TabelaFilaProducao.carregarTabela('Nao');
    }

    //(2)Eventos de botoes ===>
    function clickBtnEditar() {//botao editar
        var tr = $(this).parents('tr');
        ModalAlterarFila.abrir(tr);
    }
    function clickBtnExluir() {//botao excluir
        var btn = $(this);
        var tr = btn.parents('tr');
        Modal.confExlusao([
            { t: 'Máquina', d: tr.find('.maquina').text() },
            { t: 'Pedido', d: tr.find('.pedido').text() },
            { t: 'Produto', d: tr.find('.produto').text() },
            { t: 'Quantidade', d: tr.find('.qtd').text() },
            { t: 'Inicio Previsto', d: tr.find('.inicioPrevisto').text() },
            { t: 'Fim Previsto', d: tr.find('.fimPrevisto').text() },
        ], function () {
            Carregando.abrir('Excluindo');
            $.post('/PlugAndPlay/FilaProducao/Delete', {
                ord: tr.attr('data-ord-id'),
                prod: tr.attr('data-pro-id'),
                seqTran: tr.attr('data-seq-tran'),
                seqRep: tr.attr('data-seq-rep'),
                maq: tr.attr('data-maq-id'),
            }).done(function (result) {
                alert('te2');
                TabelaFilaProducao.carregarTabela();
            }).fail(function () {
                Modal.erro('Não foi possivel excluir a ordem de producao.');
            }).always(function () {
                Carregando.fechar();
            });
        });
    }
    //(3) uteis ===>
    function abrirIC() {
        $('#fpLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Fila Produção');
    }
    function fecharIC() {
        $('#fpLoad').html('<i class="fa fa-check">  </i>Fila Produção');
    }

}


var TabelaEstoque = new function () {
    this.iniciar = function () {
        fecharIC();
    }
    this.carregarTabela = function () {

        abrirIC();
        fecharIC();

    }

    function abrirIC() {
        $('#EstoqueLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Estoque');
    }
    function fecharIC() {
        $('#EstoqueLoad').html('<i class="fa fa-check">  </i>Estoque');
    }
}

function exibirCaminhao(embarque) {
    $('#panel-caminhao' + embarque).css('display', '');
    $('#panel-planilha' + embarque).css('display', 'none');
}

function exibirPlanilha(embarque) {
    $('#panel-caminhao' + embarque).css('display', 'none');
    $('#panel-planilha' + embarque).css('display', '');
}

function cliqueClientePlanilha(cli_id) {
    var namespace = '/DynamicWeb/GetClassForm?nome_classe=DynamicForms.Areas.PlugAndPlay.Models.Cliente';
    var str = global_modal(cli_id, namespace);
    if (str != null) {
        $('.modal_slave').html(str);
        $('#modalGlobalDynamic').modal('show');
    }
}

async function cliqueColarClipBoard(cargaId, pedidoId) {
    const text = await navigator.clipboard.readText();

    var tr_id = '#ant_' + cargaId.replaceAll('/', '_').replaceAll('.', '_') + '_' + pedidoId.replaceAll('/', '_').replaceAll('.', '_');
    $(tr_id + ' .new_carga').text(text);


}

function cliqueClipBoard(text) {
    var textArea = document.createElement("textarea");

    // Place in top-left corner of screen regardless of scroll position.
    textArea.style.position = 'fixed';
    textArea.style.top = 0;
    textArea.style.left = 0;

    // Ensure it has a small width and height. Setting to 1px / 1em
    // doesn't work as this gives a negative w/h on some browsers.
    textArea.style.width = '2em';
    textArea.style.height = '2em';

    // We don't need padding, reducing the size if it does flash render.
    textArea.style.padding = 0;

    // Clean up any borders.
    textArea.style.border = 'none';
    textArea.style.outline = 'none';
    textArea.style.boxShadow = 'none';

    // Avoid flash of white box if rendered for any reason.
    textArea.style.background = 'transparent';


    textArea.value = text;

    document.body.appendChild(textArea);

    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
    } catch (err) {
        console.log('Oops, unable to copy');
    }

    document.body.removeChild(textArea);
}


function pegarCorOTIF(cor) {
    if (cor == 'VERDE')
        return '#6acd6d'; //'#4CAF50';
    else if (cor == 'VERMELHO')
        return '#ff6254'; //'#f44336';
    else if (cor == 'AZUL')
        return '#5d6fc9'; //'#3F51B5';

    else
        return '#FFFFFF';
}

function pegarCorFila(cor) {
    /*
     #4CAF50
    #f44336
    #3F51B5k     
     */
    return cor == '#4CAF50' ? '#6acd6d' : //Verde
        cor == '#f44336' ? '#ff6254' : //Vermelho
            cor == '#3F51B5k' ? '#5d6fc9' : //Azul
                '#FFF';
}


var cliente_antigo = "";
var carId_antigo = "";
function gerarTbodyExpedicao(f) {

    var data_emb = f.EMBARQUE.substring(6, 8) + '/' + f.EMBARQUE.substring(4, 6) + '/' + f.EMBARQUE.substring(0, 4);

    var str = "";
    if (f.CLI_ID != cliente_antigo || f.CAR_ID != carId_antigo) {
        cliente_antigo = f.CLI_ID;
        carId_antigo = f.CAR_ID;

        str += '<tr>';
        str += '<td class="cab_carga" style="text-align:left" colspan="23" >CARGA ATUAL: {0}. DOCA: {8}. TIPO DE CAMINHÃO: {1}. OCUPAÇÃO {2}%. DATA INICIO PREVISTO: {3}. DATA FIM PREVISTO: {4}. CARGA STATUS: {5} PLACA: {6}. TRANSPORTADORA: {7}</td>'
            .replace('{0}', f.CAR_ID)
            .replace('{1}', f.TIP_ID)
            .replace('{2}', f.OCUPACAO)
            .replace('{3}', f.CAR_DATA_INICIO_PREVISTO)
            .replace('{4}', f.CAR_DATA_FIM_PREVISTO)
            .replace('{5}', pegarStatus(f.CAR_STATUS))
            .replace('{6}', f.VEI_PLACA)
            .replace('{7}', f.TRA_NOME)
            .replace('{8}', f.CAR_ID_DOCA)
            ;
        str += '</tr>';

        str += '<tr>';
        str += '<td onclick="cliqueClientePlanilha(\'' + f.CLI_ID + '\')" style="text-align:left"  colspan="23"> EMBARQUE: ' + data_emb + '. CLIENTE: ' + f.CLI_NOME + '. ' + f.MUN + ', ' + f.UF + ', DATA ENTREGA: {99}</td>';
        str += '</tr>';
    }

    var cor_otif = pegarCorOTIF(f.COR_OTIF);
    var cor_fila = pegarCorFila(f.FPR_COR_FILA);

    var aux_id = 'ant_' + f.CAR_ID.replaceAll('/', '_').replaceAll('.', '_') + '_' + f.ORD_ID.replaceAll('/', '_').replaceAll('.', '_');
    str += '<tr id="' + aux_id + '">';
    //Dados editáveis
    str += '<td><span><button type="button" class="btn btn-primary btn-rounded btn-sm my-0" onclick="eventoAlterarItemCarga(\'' + f.CAR_ID + '\', \'' + f.ORD_ID + '\', \'' + f.DTEMBARQUE + '\')">Alterar</button ></span ></td>'; //Botão salvar
    str += '<td class="td_exped new_carga" contenteditable="true" style="border: 1px solid #999;" onclick="cliqueColarClipBoard(\'' + f.CAR_ID + '\', \'' + f.ORD_ID + '\')">' + '' + '</td>'; //Nova carga
    str += '<td class="td_exped new_itc_qtd_plan" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova qtd planejada
    str += '<td class="td_exped new_ord_entrega" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova ordem entrega
    str += '<td class="td_exped new_tipo_caminhao" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //novo tipo caminhão

    //Dados fixos
    str += '<td style="color:#000; background-color:' + cor_fila + '" class="old_carga" id="old_carga_' + f.CAR_ID + '" onclick=cliqueClipBoard(\'' + f.CAR_ID + '\')>' + f.CAR_ID + '</td>'; //Carga atual
    str += '<td class="old_itc_qtd_plan">' + f.ITC_QTD_PLANEJADA + '</td>'; //Quantidade planejada atual
    str += '<td class="old_ord_entrega">' + f.ITC_ORDEM_ENTREGA + '</td>'; //Ordem de entrega
    str += '<td style="display:none;" class="old_tipo_caminhao">' + f.TIP_ID + '</td>'; //Tipo caminhão
    str += '<td style="color:#000; background-color:' + cor_otif + '">' + f.ORD_ID + '</td>'; //Número do pedido
    //str += '<td>' + f.ORD_ID + '</td>'; //Número do pedido
    str += '<td>' + f.ORD_QUANTIDADE + '</td>'; //Produto
    str += '<td>' + f.PRO_ID + '</td>'; //Produto
    str += '<td>' + f.PRO_DESCRICAO + '</td>'; //Pro descrição
    str += '<td>' + f.SALDO_ESTOQUE + '</td>'; //Saldo estoque
    var peso_total = f.ORD_QUANTIDADE * f.ORD_PESO_UNITARIO;
    str += '<td>' + peso_total + '</td>'; //Peso total
    str += '<td>' + f.M3_PLANEJADO + '</td>'; //M³
    //str += '<td>' + f.OCUPACAO + '</td>'; //Percentual ocupação
    str += '<td>' + f.PECENT_ESTOQUE_PRONTO + '</td>'; //Percentual estoque pronto
    str += '<td>' + f.PRO_LARGURA_EMBALADA + ' x ' + f.PRO_ALTURA_EMBALADA + ' x ' + f.PRO_COMPRIMENTO_EMBALADA + '</td>'; //Dimensões do pallet
    str += '<td>' + formatarData(f.ORD_DATA_ENTREGA_DE) + '</td>'; //Data de entrega

    str += '<td>' + formatarDataHora(f.FIMPREVISTO) + '</td>'; //Fim previsto produção
    //str += '<td>' + f.CAR_DATA_INICIO_PREVISTO + '</td>'; //Carga data inicio previsto
    //str += '<td>' + f.CAR_DATA_FIM_PREVISTO + '</td>'; //Carga data fim previsto

    //str += '<td>' + pegarStatus(f.CAR_STATUS) + '</td>'; //Carga status
    str += '<td>' + f.CAR_ID_JUNTADA + '</td>'; //Carga id juntada
    //str += '<td>' + f.VEI_PLACA + '</td>'; //Veículo placa
    //str += '<td>' + f.TRA_NOME + '</td>'; //Transportadora nome


    str += '</tr>'
    return str;
}

function gerarTbodyExpedicaoNovaCarga(car_id, embarque) {
    var data_emb = embarque.substring(8, 10) + '/' + embarque.substring(5, 7) + '/' + embarque.substring(0, 4);
    var ord_id = "TESTE";

    var str = "";
    str += '<tr>';
    str += '<td class="cab_carga" onclick="cliqueClientePlanilha()" style="text-align:left"  colspan="23"> EMBARQUE: ' + data_emb + '. CARGA GERADA MANUALMENTE: ' + car_id + '</td>';
    str += '</tr>';

    var aux_id = 'ant_' + car_id.replaceAll('/', '_').replaceAll('.', '_') + '_' + ord_id.replaceAll('/', '_').replaceAll('.', '_');
    str += '<tr id="' + aux_id + '">';
    //Dados editáveis
    str += '<td><span><button type="button" class="btn btn-primary btn-rounded btn-sm my-0" onclick="eventoAlterarItemCarga(\'' + car_id + '\', \'' + ord_id + '\')">Alterar</button ></span ></td>'; //Botão salvar
    str += '<td class="td_exped new_carga" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova carga
    str += '<td class="td_exped new_itc_qtd_plan" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova qtd planejada
    str += '<td class="td_exped new_ord_entrega" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //Nova ordem entrega
    str += '<td class="td_exped new_tipo_caminhao" contenteditable="true" style="border: 1px solid #999;">' + '' + '</td>'; //novo tipo caminhão

    //Dados fixos
    str += '<td class="old_carga" id="old_carga_' + car_id + '" onclick=cliqueClipBoard(\'' + car_id + '\')>' + car_id + '</td>'; //Carga atual
    str += '<td class="old_itc_qtd_plan"></td>'; //Quantidade planejada atual
    str += '<td class="old_ord_entrega"></td>'; //Ordem de entrega
    str += '<td class="old_tipo_caminhao"></td>'; //Tipo caminhão
    str += '<td></td>'; //Número do pedido
    str += '<td></td>'; //Quantidade pedido
    str += '<td></td>'; //Produto
    str += '<td></td>'; //Produto descrição
    str += '<td></td>'; //Saldo estoque
    str += '<td></td>'; //Pesos total pedido
    str += '<td></td>'; //M³
    str += '<td></td>'; //Percentual ocupação
    str += '<td></td>'; //Percentual estoque pronto

    str += '<td></td>'; //Dimensões do pallet
    str += '<td></td>'; //Data de entrega

    str += '<td></td>'; //Fim previsto produção
    str += '<td></td>'; //Carga data inicio previsto
    str += '<td></td>'; //Carga data fim previsto

    str += '<td></td>'; //Carga status
    str += '<td></td>'; //Carga id juntada
    str += '<td></td>'; //Veículo placa
    str += '<td></td>'; //Transportadora nome


    str += '</tr>'
    return str;
}

function eventoAlterarItemCarga(cargaId, pedidoId, embarque) {
    var tr_id = '#ant_' + cargaId.replaceAll('/', '_').replaceAll('.', '_') + '_' + pedidoId.replaceAll('/', '_').replaceAll('.', '_');

    var old_carga = $(tr_id + ' .old_carga').text();
    var old_ord_entrega = $(tr_id + ' .old_ord_entrega').text();
    var old_itc_qtd_plan = $(tr_id + ' .old_itc_qtd_plan').text();
    var old_tipo_caminhao = $(tr_id + ' .old_tipo_caminhao').text();


    var new_carga = $(tr_id + ' .new_carga').text() != '' ? $(tr_id + ' .new_carga').text() : $(tr_id + ' .old_carga').text();
    var new_ord_entrega = $(tr_id + ' .new_ord_entrega').text() != '' ? $(tr_id + ' .new_ord_entrega').text() : $(tr_id + ' .old_ord_entrega').text();
    var new_itc_qtd_plan = $(tr_id + ' .new_itc_qtd_plan').text() != '' ? $(tr_id + ' .new_itc_qtd_plan').text() : $(tr_id + ' .old_itc_qtd_plan').text();
    var new_tipo_caminhao = $(tr_id + ' .new_tipo_caminhao').text() != '' ? $(tr_id + ' .new_tipo_caminhao').text() : $(tr_id + ' .old_tipo_caminhao').text();

    if (parseInt(new_itc_qtd_plan)) {
        var aux_num = parseInt(new_itc_qtd_plan);
        if (aux_num > 0 && aux_num <= old_itc_qtd_plan) {
            new_carga = new_carga.toUpperCase();
            if (new_carga == "NOVO" || new_carga == "NOVA" || new_carga == "NEW") {
                var today = new Date();
                var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
                var time = today.getHours() + ":" + today.getMinutes();
                var dateTime = date + 'T' + time;

                var novaCarga_str = '[{"CAR_STATUS":"","CAR_EMBARQUE_ALVO":"' + embarque + '","TIP_ID":"","VEI_PLACA":"","TRA_ID":"","CAR_OBSERVACAO_DE_TRANSPORTE":"","CAR_ID_DOCA":"","CAR_PREVISAO_MATERIA_PRIMA":"","CAR_DATA_INICIO_REALIZADO":"","CAR_DATA_FIM_REALIZADO":"","CAR_PESO_TEORICO":"","CAR_VOLUME_TEORICO":"","CAR_PESO_REAL":"","CAR_VOLUME_REAL":"","CAR_PESO_EMBALAGEM":"","CAR_PESO_ENTRADA":"","CAR_PESO_SAIDA":"","CAR_GRUPO_PRODUTIVO":"","ROT_ID":"","CAR_JUSTIFICATIVA_DE_CARREGAMENTO":"","PlayAction":"CARGA_APS","MovimentoEstoqueProducao":[],"MovimentoEstoqueReservaDeEstoque":[],"MovimentoEstoqueVendas":[],"MovimentoEstoqueSaidaInventario":[],"MovimentoEstoqueEntradaInventario":[],"MovimentoEstoqueTransferenciaSimples":[],"MovimentoEstoqueConsumoMateriaPrima":[],"MovimentoEstoquePerdas":[],"ItensCarga":[]}]';
                var novaCarga = JSON.parse(novaCarga_str);
                var namespace = 'DynamicForms.Areas.PlugAndPlay.Models.Carga';

                insertBanco(novaCarga, namespace, 0);

                new_carga = result_log[0].PrimaryKey.split(':')[1];
            }

            $.ajax({
                type: "POST",
                url: UrlBase + "GerenciarCargas",
                data: {
                    pedidoId: pedidoId,
                    old_carga: old_carga,
                    new_carga: new_carga,
                    old_ord_entrega: old_ord_entrega,
                    new_ord_entrega: new_ord_entrega,
                    old_itc_qtd_plan: old_itc_qtd_plan,
                    new_itc_qtd_plan: new_itc_qtd_plan,
                    old_tipo_caminhao: old_tipo_caminhao,
                    new_tipo_caminhao: new_tipo_caminhao
                },
                dataType: "json",
                traditional: true,
                async: false,
                contenttype: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.msg != "OK") {
                        //Deu algum erro esperado. Erro causado pelo usuário.
                        alert(result.msg);
                    }
                    else {
                        alert("Carga movimentada com sucesso!");
                        $(tr_id + ' .old_carga').text(new_carga); //Só vai setar o texto de nova carga quando for sucesso na transação;
                        $(tr_id + ' .new_carga').text("");
                    }
                },
                error: function (erro) {
                    alert('Erro inesperado:' + erro);

                }
            });
        }
        else {
            alert('A nova quantidade planejada deve ser maior que zero e menor igual à ' + old_itc_qtd_plan);
        }
    }
    else {
        alert('O valor digitado no campo nova quantidade planejada deve ser em branco ou um valor inteiro maior que zero e menor igual à ' + old_itc_qtd_plan);
    }

    //Atualizar a tabela.
}

//recebe um array contendo os ids dos status e retorna um array contendo a descricao dos status
function pegarValoresStatus(lista_ids) {
    let status = [];
    lista_ids.forEach(function (currentValue, index) {
        status.push(pegarStatus(currentValue));
    });

    return status;
}

function pegarStatus(num) {
    var status = "nulo";
    switch (num) {
        case -1:
            status = "Deletada";
            break;
        case 1:
            status = "Aberta";
            break;
        case 1.1:
            status = "Reotimizar";
            break;
        case 1.2:
            status = "Antecipar alguém";
            break;
        case 2:
            status = "Aprovada";
            break;
        case 3:
            status = "Agenciada";
            break;
        case 4:
            status = "Piking";
            break;
        case 5:
            status = "Carregando";
            break;
        case 6:
            status = "Despachada (Consolidada)";
            break;
        case 7:
            status = "Faturada";
            break;
        case 8:
            status = "Etregue Parcial";
            break;
        case 9:
            status = "Entregue";
            break;
        case 99:
            status = "Estornada";
            break;
    }

    return status;
}

function novaCargaPlanilha(embarque, emb2) {
    var novaCarga_str = '[{"CAR_STATUS":"","CAR_EMBARQUE_ALVO":"' + embarque + '","TIP_ID":"","VEI_PLACA":"","TRA_ID":"","CAR_OBSERVACAO_DE_TRANSPORTE":"","CAR_ID_DOCA":"","CAR_PREVISAO_MATERIA_PRIMA":"","CAR_DATA_INICIO_REALIZADO":"","CAR_DATA_FIM_REALIZADO":"","CAR_PESO_TEORICO":"","CAR_VOLUME_TEORICO":"","CAR_PESO_REAL":"","CAR_VOLUME_REAL":"","CAR_PESO_EMBALAGEM":"","CAR_PESO_ENTRADA":"","CAR_PESO_SAIDA":"","CAR_GRUPO_PRODUTIVO":"","ROT_ID":"","CAR_JUSTIFICATIVA_DE_CARREGAMENTO":"","PlayAction":"CARGA_APS","MovimentoEstoqueProducao":[],"MovimentoEstoqueReservaDeEstoque":[],"MovimentoEstoqueVendas":[],"MovimentoEstoqueSaidaInventario":[],"MovimentoEstoqueEntradaInventario":[],"MovimentoEstoqueTransferenciaSimples":[],"MovimentoEstoqueConsumoMateriaPrima":[],"MovimentoEstoquePerdas":[],"ItensCarga":[]}]';
    var novaCarga = JSON.parse(novaCarga_str);
    var namespace = 'DynamicForms.Areas.PlugAndPlay.Models.Carga';

    insertBanco(novaCarga, namespace, 0);

    var car_id = result_log[0].PrimaryKey.split(':')[1];

    var tr = gerarTbodyExpedicaoNovaCarga(car_id, embarque);
    console.log(tr);

    var tbody_id = '#tbody' + emb2;

    $(tbody_id).prepend(tr);
}

var listaCargaCidades = [];
function checkCargaCidade(ck, car_id, _cidade, _cidadeId) {

    var cidade = _cidade.split("-")[0];
    if (cidade[cidade.length - 1] == " ")
        cidade = cidade.substring(0, cidade.length - 1);

    if (ck.checked === true) {
        var item = {
            car_id: car_id,
            cidade: cidade,
            cidadeId: _cidadeId,
        };

        listaCargaCidades.push(item);


        $('.entrega_' + car_id).hide();
        for (var i = 0; i < listaCargaCidades.length; i++) {
            var id = '.entrega_' + listaCargaCidades[i].car_id + '_' + listaCargaCidades[i].cidadeId;
            $(id).show();

        }

    }
    else {
        var i = 0;
        while (i < listaCargaCidades.length && !(car_id == listaCargaCidades[i].car_id && cidade == listaCargaCidades[i].cidade))
            i++;

        if (i < listaCargaCidades.length) {
            item = listaCargaCidades[i]; //Item que vai ser retirado da lista;
            listaCargaCidades.splice(i, 1); //Retirando item da lista;

            //Verifica se possui outro item com o mesmo car_id
            var j = 0;
            while (j < listaCargaCidades.length && listaCargaCidades[j].car_id != item.car_id)
                j++;


            if (j < listaCargaCidades.length) {
                //Se possui, ocultar somente o item que foi removido
                var id = '.entrega_' + item.car_id + '_' + item.cidadeId;
                $(id).hide();
            }
            else {
                //Se não, mostra todos
                $('.entrega_' + car_id).show();
            }
        }
    }

    var _class = "";
    var ocup_tot = 0;
    var vol_liv_tot = 0;
    var peso_tot = 0;
    var vol_car_tot = 0;
    var flag = false;
    var fim_prev = '00/00 00:00';

    for (var i = 0; i < listaCargaCidades.length; i++) {
        var id = '.entrega_' + listaCargaCidades[i].car_id + '_' + listaCargaCidades[i].cidadeId;
        if (listaCargaCidades[i].car_id == car_id) {
            flag = true;
            _class = $(id).attr('class');
            _class = _class.split(" ");
            ocup_tot += parseFloat(_class[2].split('_')[2]);
            vol_car_tot += parseFloat(_class[3].split('_')[2]);
            vol_liv_tot = parseFloat(_class[4].split('_')[2]);
            peso_tot += parseFloat(_class[5].split('_')[2]);

            var temp = _class[6].split('_')[2] + ' ' + _class[7];
            fim_prev = compararDataHora(fim_prev, temp) == 1 ? fim_prev : temp;
        }

    }

    if (!flag) {
        var class_ocup = $('#ocup_' + car_id).attr('class');
        var class_vol_car = $('#vol_car_' + car_id).attr('class');
        var class_vol_liv = $('#vol_liv_' + car_id).attr('class');
        var class_peso_tot = $('#peso_tot_' + car_id).attr('class');
        var class_fim_prev = $('#data_fimprev_' + car_id).attr('class');

        ocup_tot = class_ocup.split("_")[1];
        vol_car_tot = class_vol_car.split("_")[2];
        vol_liv_tot = class_vol_liv.split("_")[2];
        peso_tot = class_peso_tot.split("_")[2];
        fim_prev = class_fim_prev.split("_")[2];

    } else {
        ocup_tot = ocup_tot.toFixed(2);
        vol_liv_tot = vol_liv_tot - vol_car_tot;
        vol_liv_tot = vol_liv_tot.toFixed(2);
        vol_car_tot = vol_car_tot.toFixed(2);
        peso_tot = peso_tot;
        fim_prev = fim_prev;
    }
    //peso_tot = roundToTwo(parseFloat(peso_tot) / 1000);

    $('#ocup_' + car_id).text(ocup_tot + '%');
    $('#vol_car_' + car_id).text(vol_car_tot);
    $('#vol_liv_' + car_id).text(vol_liv_tot);
    $('#peso_tot_' + car_id).text(peso_tot);
    $('#data_fimprev_' + car_id).text(fim_prev);
}

function alterarStatusCarga(status) {
    var cargas = "";

    if (_listaUniaoCargas.length > 0) {

        for (var i = 0; i < _listaUniaoCargas.length - 1; i++) {
            cargas += _listaUniaoCargas[i].idCarga + ',';
        }
        cargas += _listaUniaoCargas[_listaUniaoCargas.length - 1].idCarga;

        $.ajax({
            url: UrlBase + "/AlterarStatusCarga",
            data: { status: status, cargas: cargas },
            type: "post",
            dataType: "json",
            success: function (result) {
                var cargas_lista = cargas.split(',');
                var text;
                var temp;
                for (var i = 0; i < cargas_lista.length; i++) {
                    text = $('#titulo_carga_' + cargas_lista[i]).text();

                    temp = text.split('; ');
                    text = temp[0] + '; ' + pegarStatus(parseFloat(status)) + '; ' + temp[2];
                    console.log(text);
                    $('#titulo_carga_' + cargas_lista[i]).text('');
                    $('#titulo_carga_' + cargas_lista[i]).append('<br><i class="fa fa-check" style="color:#f44336;"></i>' + text);
                }

                alert('Alteração realizada com sucesso');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Houve erro no banco de dados!');
            },
            complete: function () { }
        })
    }
    else {
        alert('Selecione no mínimo uma Carga para fazer a alteração de status!');
    }


}

var TabelaFilaExpedicao = new function (MaxDiasAntecipacao, diasAfrente, acao, carga_id) {
    //(1) variaveis globais ==>
    // 1.1 - 
    //(1) publicos ===>

    var _html = '';

    this.iniciar = function () {
        $(document).on('click', '#btnRecalcFilaExpedicao', recalculaFilaExpedicao);
        $(document).on('click', '#btnRecalcFilaExpedicaoFULL', recalculaFilaExpedicaoFULL);
        fecharIC();
        //fecha indicador de atrazo(carregando) na resposta de todas as solicotacoes ajax
    }
    this.carregarTabela = function (MaxDiasAntecipacao, diasAfrente, acao, carga_id, tipo_alteracao, pos_alteracao, embarque = undefined, dtEmbarque = undefined, status = undefined, query_json = undefined) {
        //monta tabela fila de produção
        abrirIC();

        //modal carregando
        Carregando.abrir('Processando ...');

        $.ajax({
            url: UrlBase + "/ObterFilaExpedicao",
            data: { MaxDiasAntecipacao: MaxDiasAntecipacao, diasAFrente: diasAfrente, recalculaFila: acao, cargaId: carga_id, dtEmbarque: dtEmbarque, status: status, query_json: query_json },
            type: "post",
            dataType: "json",
            success: function (result) {
                montarTabela(result.cargasPlanilha, result.cargasWhere, result.st, MaxDiasAntecipacao, diasAfrente, acao, carga_id, tipo_alteracao, pos_alteracao, embarque, dtEmbarque, status, query_json);
                Carregando.fechar();
                fecharIC();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Erro ao carregar cargas.' + xhr.status + ' ' + thrownError);
            },
            complete: function () {
                fecharIC();
                Carregando.fechar();
            }
        })
    }
    this.eventoPesquisarPrincipalAPS = function (posEmbarque = undefined, embarque = undefined, dtEmbarque = undefined, status = undefined, tipo_alteracao = 2) { //essa função gera a string de psequisa antes de fazer a consulta


        //insere um novo filtro e retorna a string de pesquisa e as colunas selecionadas
        var input_pesquisa_e_selecionados = prepararNovoFiltro("update", "", "AUTO", "aps", false); //retorna um array contendo o string_pesquisa gerado e as colunas selecionadas na pesquisa;
        var string_pesquisa = input_pesquisa_e_selecionados[0];
        var selecionados_local = input_pesquisa_e_selecionados[1];

        //retira os ';' que separa cada linha no input pesquisa, para poder criar o json corretamente
        let input_pesquisa_formatado = string_pesquisa;
        var count = (string_pesquisa.match(/;/g) || []).length;
        for (let i = 0; i < count; i++) {
            input_pesquisa_formatado = input_pesquisa_formatado.replace(";", "");
        }

        //cria o json de pesquisa
        var filtros = criarJsonPesquisa(input_pesquisa_formatado, "DynamicForms.Areas.PlugAndPlay.Models.CargasWeb", selecionados_local); //Cria o filtro no formato JSON que a parte back aceita para pesquisa

        TabelaFilaExpedicao.carregarTabela(undefined, 'FULL', 'sim', undefined, tipo_alteracao, posEmbarque, embarque, dtEmbarque, status, filtros);
    }
    this.gerarCamposPesquisa = function () {
        var div_pesquisa = gerarCamposPesquisaInicialAPS(0);

        div_pesquisa += '<style>.modal{overflow-y:auto}.colocapica{position:relative;top:50%;transform:translateY(25%)}.nopadding{padding:0;margin:0}.divselect{height:34px}#sub1{height:100%}#sub2{height:100%}#sel1{height:100%}.selectBox select{width:100%}.overSelect{position:absolute;left:0;right:0;top:0;bottom:0}#checkboxes{display:none;border:1px #dadada solid}#checkboxes label{display:block;background-color:#fff}#checkboxes label:hover{background-color:#1e90ff}</style>';
        $("#divPaiLinhas").html(div_pesquisa);
    }

    function recalculaFilaExpedicao() {//botao editar
        TabelaFilaExpedicao.carregarTabela('', 'standard', 'sim');
    }
    function recalculaFilaExpedicaoFULL() {//botao editar
        TabelaFilaExpedicao.carregarTabela('', 'FULL', 'sim');
    }
    function unirCargas(cargaSelecionada) {//botao Unir Cargas
        _linkUnirCargas(_listaUniaoCargas, cargaSelecionada);
    }
    function abrirIC() {
        $('#exLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i> Expedição');
    }
    function fecharIC() {
        $('#exLoad').html('<i class="fa fa-check">  </i>Expedição');
        // pendencia  evitar de carregar duas vesses ao dar f5 ou atualizar a pagina toda
    }
    function ajustarValores() { //ajusta os valores de atributos, ids e etc

        //ajusta a classe exped das div de embarque
        $(`[class^='panel panel-default exped']`).each(function (index) {
            $(this).attr("class", "panel panel-default exped" + index);
        });

    }

    function montarTabela(cargasPlanilha, cargasWhere, st, MaxDiasAntecipacao, diasAfrente, acao, carga_id, tipo_alteracao, pos_alteracao, embarque = undefined, dtEmbarque = undefined, status = undefined, query) {

        if (st == "OK") {

            var trs = [];
            var cab_tab = "";
            var body_tab = "";
            _c = "";
            _ite = "";
            _Pedido = "";
            _Cli = "";
            _Mun = "";
            _top = "";
            _sec = "";
            _cab = "";
            _rod = "";
            _rod1 = "";
            _html = "";

            total = 0;
            cor = "ca";
            _menu = "";
            let todas_cargas = ""; //essa variável contem o html de todas as cargas da consulta, excluindo a div de embarque
            var _itemMenu = '';
            var _Embarque = "";
            var indice = 0;
            var cargasPlanilha = cargasPlanilha;
            var index_plan = 0;
            var menorData
            var cargasNotInArray = []; //contem os ids das cargas trazidas do back-end separadas por ,
            var indexEmbarques = 0;
            var idsCargas = [];
            var cidade_entrega = "";
            var cidade_atual = "";
            var cliente_atual = [];
            var _Table = "";

            id = "";
            //id = cargasWhere[0].CAR_ID;
            var tip_capacidade_m3 = cargasWhere.length > 0 ? cargasWhere[0].TIP_CAPACIDADE_M3 : 0;
            var lista_objetos = [];
            var lista_objetos_full = [];
            var lista_datas_entrega = [];
            var lista_datas_aps = [];
            var volume_cidade = 0;
            var volume_total = 0;
            var peso_total = 0;
            var peso_cidade = 0;
            var temp_fim_prev = "";
            var temp_fim_prev_tot = [];
            var temp_fim_carg = "";
            var temp_fim_carg_tot = [];
            var temp_data_str = "";

            //O Trecho abaixo é fruto de uma gambiarra para contornar mais um problema estrutural que está essa função.
            var gambiarra1 = Object.assign({}, cargasWhere[0]);
            var gambiarra2 = Object.assign({}, cargasWhere[cargasWhere.length - 1]);
            gambiarra1.CAR_ID = "G00000";
            gambiarra2.CAR_ID = "G99999";

            cargasWhere.unshift(gambiarra1);
            cargasWhere.push(gambiarra2);

            cargasPlanilha.unshift(gambiarra1);
            cargasPlanilha.push(gambiarra2);
            //Fim da gambiarra

            cargasWhere.forEach(function (f) {//gera as linhas da tabela

                if (cidade_atual != f.PON_ID || id != f.CAR_ID) {
                    var temp_id = id.length == 0 ? cargasWhere[0].CAR_ID : id;
                    var temp_class = '.entrega_' + temp_id + '_' + cidade_atual;


                    var ocupacao_cidade = volume_cidade * 100 / tip_capacidade_m3;
                    ocupacao_cidade = ocupacao_cidade.toFixed(2);
                    var new_class_cid_ocup = 'ocup_cid_' + ocupacao_cidade;
                    volume_cidade = volume_cidade.toFixed(2);

                    var volume_livre = tip_capacidade_m3.toFixed(2);
                    //peso_cidade = roundToTwo(peso_cidade / 1000);

                    volume_cidade = 'vol_car_' + volume_cidade;
                    volume_livre = 'vol_liv_' + volume_livre;
                    var new_class_peso_tot = 'peso_tot_' + peso_cidade;
                    var class_fim_prev = 'data_fimprev_' + temp_fim_prev;

                    var objeto = {
                        class: temp_class,
                        class_cidade_ocup: new_class_cid_ocup,
                        class_cidade_vol_car: volume_cidade,
                        class_cidade_vol_liv: volume_livre,
                        class_peso_tot: new_class_peso_tot,
                        class_fim_prev: class_fim_prev
                    };

                    lista_objetos.push(objeto);

                    volume_cidade = 0;
                    peso_cidade = 0;
                }



                if (id != f.CAR_ID) {

                    //_html = _html.replace('{11}', temp_data_str);

                    var temp_class = '.entrega_' + id + '_' + cidade_atual;
                    var ocupacao_total = volume_total * 100 / tip_capacidade_m3;
                    ocupacao_total = ocupacao_total.toFixed(2);
                    volume_total = volume_total.toFixed(2);
                    //Pega todas as datas Fim produção previsto
                    var tfpt = '';
                    for (var i = 0; i < temp_fim_prev_tot.length; i++) {
                        tfpt += temp_fim_prev_tot[i] + ';';
                    }
                    //Pega todas as datas car_fim_previsto
                    var tfptCarga = '';
                    for (var i = 0; i < temp_fim_carg_tot.length; i++) {
                        tfptCarga += temp_fim_carg_tot[i] + ';';
                    }
                    //peso_total = roundToTwo(peso_total / 1000, 2);
                    var objeto = {
                        id: id,
                        class: temp_class,
                        ocupacao_total: ocupacao_total,
                        volume_total: volume_total,
                        volume_livre: tip_capacidade_m3,
                        peso_total: peso_total,
                        fim_prev: tfpt,
                        fim_prev_carga: tfptCarga
                    };
                    lista_objetos_full.push(objeto);

                    peso_total = 0;
                    peso_cidade = 0;
                    volume_cidade = 0;
                    volume_total = 0;
                    temp_fim_prev = "";
                    temp_fim_prev_tot = [];
                    temp_fim_carg = "";
                    temp_fim_carg_tot = [];


                    if (total > 0 && total < 100) {
                        _ite += '<td width="' + (100 - total) + '%" ></td>';
                    }

                    if (_Embarque != f.EMBARQUE) {
                        if (_top != "")
                            _top = _top.replace('tbody_caminhao', body_tab);
                        else
                            _html = _html.replace('tbody_caminhao', body_tab);

                        _menu += '</div></div></div>';

                        /*Parte da tabela*/
                        cab_tab = "";

                        cab_tab += '<div class="card" id="panel-planilha' + f.EMBARQUE + '" style="display:none; overflow:auto; height:600px;">';
                        cab_tab += '<div class="card-body">';
                        cab_tab += '<div id="table_expedicao" class="table-editable">';
                        cab_tab += '<span class="table-add float-right mb-3 mr-2"><a onclick="cliqueClipBoard(\'NOVO\')"><i class="fa fa-truck"></i> NOVA CARGA</a><p>Obs: Digite NOVO para inserir uma nova carga no campo NOVA CARGA na planilha abaixo</p></span>';
                        cab_tab += '<table class="table table-bordered table-striped text-center table_expedicao">';
                        //thead
                        cab_tab += '<thead><tr>';
                        //Dados editáveis
                        cab_tab += '<th>BOTÃO SALVAR</th>';
                        cab_tab += '<th>NOVA CARGA</th>';
                        cab_tab += '<th>NOVA QTD. PLANEJADA</th>';
                        cab_tab += '<th>NOVA ORDEM ENTREGA</th>';
                        cab_tab += '<th>NOVO TIPO CAMINHÃO</th>';

                        //Dados fixos
                        cab_tab += '<th>CARGA ATUAL</th>';
                        cab_tab += '<th data-toggle="tooltip" title="QUANTIDADE PLANEJADA ATUAL">QTD. PLAN.</th>';
                        cab_tab += '<th data-toggle="tooltip" title="ORDEM ENTREGA ATUAL">ORD. ENTREGA</th>';
                        //cab_tab += '<th>TIPO CAMINHÃO ATUAL</th>';

                        cab_tab += '<th data-toggle="tooltip" title="NÚMERO DO PEDIDO">N°. PEDIDO</th>';
                        cab_tab += '<th data-toggle="tooltip" title="PEDIDO QUANTIDADE">PEDIDO QTD.</th>';
                        cab_tab += '<th>PRODUTO ID</th>';
                        cab_tab += '<th>PRODUTO DESCRIÇÃO</th>';
                        cab_tab += '<th>SALDO ESTOQUE</th>';
                        cab_tab += '<th>PESO TOTAL</th>';
                        cab_tab += '<th>M³</th>';
                        //cab_tab += '<th data-toggle="tooltip" title="PERCENTUAL OCUPAÇÃO">% OCUPAÇÃO</th>';
                        cab_tab += '<th data-toggle="tooltip" title="PERCENTUAL ESTOQUE PRONTO">% ESTOQUE PRONTO</th>';
                        cab_tab += '<th data-toggle="tooltip" title="DIMENSÕES DO PALLET (LARGURA x ALTURA x COMPRIMENTO)">DIMENSÕES DO PALLET (LxAxC)</th>';
                        cab_tab += '<th data-toggle="tooltip" title="DATA DE ENTREGA">DATA DE ENTREGA</th>';
                        cab_tab += '<th data-toggle="tooltip" title="FIM PREVISTO PRODUÇÃO">FIM PREV. PROD.</th>';
                        //cab_tab += '<th data-toggle="tooltip" title="DATA INÍCIO PREVISTO">DT. INI. PREV.</th>';
                        //cab_tab += '<th data-toggle="tooltip" title="DATA FIM PREVISTO">DT. FIM. PREV.</th>';
                        //cab_tab += '<th>CARGA STATUS</th>';
                        cab_tab += '<th>CARGA ID JUNTADA</th>';
                        //cab_tab += '<th data-toggle="tooltip" title="VEÍCULO PLACA">VEI. PLACA</th>';
                        //cab_tab += '<th data-toggle="tooltip" title="TRANSPORTADORA NOME">TRA. NOME</th>';

                        cab_tab += '</tr></thead>';
                        //tbody
                        cab_tab += '<tbody id="tbody' + f.EMBARQUE + '">';
                        cab_tab += 'tbody_caminhao';
                        cab_tab += '</tbody>';
                        cab_tab += '</table>';
                        cab_tab += '</div></div></div>';
                        cab_tab += '';
                        /*Fim parte da tabela*/
                        //Teste Henrique Krupck

                        var dataAtual = f.EMBARQUE.substring(6, 8) + '/' + f.EMBARQUE.substring(4, 6) + '/' + f.EMBARQUE.substring(0, 4);
                        let lista_ids = [-1, 1, 2, 3, 4, 5];
                        let lista_dados = pegarValoresStatus(lista_ids);
                        let filtroStatus = elementoCombobox(lista_ids, "form-control selectstatus", "selectstatus0" + indice, "", "Selecione o status", "", "", lista_dados, 00, "", false, "margin-top: 10px;", ["data-embarque", "data-dtembarque", "data-pos"], [f.EMBARQUE, f.DTEMBARQUE, indexEmbarques]);
                        _menu += '<div class="panel panel-default exped' + indice + '" id="divexpedicao' + f.EMBARQUE + '">';
                        _menu += '    <div class="panel-heading" role="tab" id="headingOne3">';
                        _menu += '        <div class="panel-title" style="display:flex">';
                        _menu += '            <div style="width:70%" class="row">';
                        _menu += '              <a data-toggle="collapse" data-parent="#accordExpedicao" href="#collap' + f.EMBARQUE + '" aria-expanded="false" aria-controls="collapseOne-3" class="col-md-10 collapsed">';
                        _menu += '                <i class="fa fa-check"></i> Embarque ' + f.EMBARQUE.substring(6, 8) + '/' + f.EMBARQUE.substring(4, 6) + '/' + f.EMBARQUE.substring(0, 4);
                        _menu += '       &emsp;<span title = "OTIF"><i class="fa fa-handshake-o"></i> ' + Math.floor((Math.random() * 100) + 1) + '%</span>  &emsp;<span title = "Ocupação"><i class="fa fa-truck"></i>' + Math.floor((Math.random() * 100) + 1) + '% </span>&emsp;<span title = "Frete Reais/M3 "><i class="fa fa-dollar"></i>R$ ' + Math.floor((Math.random() * 100) + 1) + '</span>&emsp;';

                        _menu += '<span title = "Calendário de disponibilidade de veículos"><i class="fa fa-calendar" onclick="javascript:CalendarioDisponibilidade(\'' + dataAtual + '\')">&emsp;</i></span><span title = "Mapa dos Pontos de Entrega"><i class="fa fa-map" onclick="window.open(\'/PlugAndPlay/PontoMapa/PontosEntrega?data=' + dataAtual + '&carga=' + f.CAR_ID + '&modo=1 \', \'_blank\', \'toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=800,height=550\')"></i></span></a></a>';
                        _menu += `<div class='col-md-2'>${filtroStatus} </div>`;
                        _menu += '</div>';
                        _menu += `<div style="width:30%;"><div class="pull-right" style="margin-top: 15px; "><i onclick="TabelaFilaExpedicao.eventoPesquisarPrincipalAPS('${indexEmbarques}','${f.EMBARQUE}', '${f.DTEMBARQUE}', 'undefined', 4)"class="fa fa-refresh btn-atualizar-carga-individual" data-toggle="tooltip" title="Atualizar carga individual"></i></div><div style="display:flex">`;
                        _menu += ' <div style="width:50%; text-align:right"><div style="position:relative; top:50%; margin-right:15px">Visualização:</div></div>';
                        _menu += ' <div style="width:20%"><div style="position:relative; top:50%" class="radio' + f.EMBARQUE + '"><label onclick="exibirCaminhao(\'' + f.EMBARQUE + '\')"><input type="radio" name="optradio' + f.EMBARQUE + '" checked><span title = "Visual"> <i class="fa fa-truck"></i></span></label></div></div>';
                        _menu += ' <div style="width:20%"><div style="position:relative; top:50%" class="radio' + f.EMBARQUE + '"><label onclick="exibirPlanilha(\'' + f.EMBARQUE + '\')"><input type="radio" name="optradio' + f.EMBARQUE + '"><span title = "Planilha"> <i class="fa fa-file-text-o"></i></span></label></div></div>';
                        _menu += ' </div></div>';

                        _menu += '        </div>';
                        _menu += '    </div>';
                        _menu += '    <div id="collap' + f.EMBARQUE + '" class="panel-collapse collapse expedarea' + indice + '" role="tabpanel" aria-labelledby="headingOne3" aria-expanded="false" style="height: 0px;">';
                        _menu += cab_tab;
                        _menu += '        <div class="panel-body" id="panel-caminhao' + f.EMBARQUE + '">';

                        indice++; //Variável para pegar o índice do forEach atual.

                        body_tab = "";
                        indexEmbarques++;
                    }

                    let data = f.EMBARQUE.substring(6, 8) + '/' + f.EMBARQUE.substring(4, 6) + '/' + f.EMBARQUE.substring(0, 4);

                    cliente_atual = [];

                    //cliente_antigo = "";
                    //lista_datas_entrega = [];

                    _Embarque = f.EMBARQUE;

                    _Mun = '<p  style="font-size:11px;">' + _Mun + '</p>';

                    _html += _top + _sec + _cab + _ite + _rod + _Mun + _Table + _rod1; //Chega aqui para cada caminhão
                    todas_cargas += _sec + _cab + _ite + _rod + _Mun + _Table + _rod1;

                    _Table = "";
                    total = 0;
                    //cor = "ca";
                    id = f.CAR_ID;
                    tip_capacidade_m3 = f.TIP_CAPACIDADE_M3;
                    _top = _menu;
                    _menu = '';
                    _sec = '	<div class="col-xs-12 col-md-6" id="carga' + f.CAR_ID + '">';
                    _cab = '	   <section class="box ">';
                    _cab += '            <header class="panel_header">';
                    _cab += '<div id="titulo_carga_' + f.CAR_ID + '" class="col-xs-12" style="font-size:10px;" >';
                    _cab += '<br><i class="fa fa-check" style="color:#f44336;"></i>' + f.CAR_ID + '; ' + pegarStatus(f.CAR_STATUS) + '; ' + f.TIP_DESCRICAO + ', ' + f.VEI_PLACA + ', ' + f.TRA_NOME;
                    _cab += '</div>';
                    _cab += '               <div class="actions panel_actions pull-right">';
                    //_cab += '                    <a class="box_toggle fa fa-chevron-down" data-toggle="modal"  onclick="teste(\'' + cargas.cargas + '\');" />';
                    var js = "onchange=\"$('#UNC_" + f.CAR_ID + "_" + f.VEI_PLACA + "');javascript:checkCarga(this,'" + f.CAR_ID + "','" + f.VEI_PLACA + "');\"";
                    _cab += '                    <input style="margin-right:5px;" data-toggle="tooltip" title="Selecionar carga" type="checkbox"  id="UNC_' + f.CAR_ID + '_' + f.VEI_PLACA + '" ' + js + ' value="">';

                    _cab += '                    <a class="box_setting fa fa-truck" data-toggle="modal" title="Unir cargas selecionadas"  onclick="unirCargasMarcadas(\'' + f.CAR_ID + '\');"\ />';

                    _cab += '                    <a class="box_setting fa fa-cube" data-toggle="modal" title="3D"  onclick="Show3d(\'' + f.CAR_ID + '\');"\ />';

                    _cab += '                    <span title="Editar carga"><a class="box_setting fa fa-edit" data-toggle="modal"  onclick="alterarCarga(\'' + f.CAR_ID + '\');" href=""\ /> </span>';
                    _cab += '                    <a class="box_setting fa fa-print" data-toggle="modal" title="Relatório plano de carga" onclick="gerarRelatorio(\'' + f.CAR_ID + '\');" />';
                    _cab += '                    <a class="box_setting fa fa-map" data-toggle="modal" title = "Mapa Pontos de entrega do veículo" onclick="window.open(\'/PlugAndPlay/PontoMapa/PontosEntrega?data=' + dataAtual + '&carga=' + f.CAR_ID + '&modo=2 \', \'_blank\', \'toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=800,height=550\')" />';
                    _cab += '                    <a class="box_setting fa fa-bolt" data-toggle="modal" title = "Otimizar carga" onclick="window.open(\'/PlugAndPlay/APS/OtimizarCarga?strCargas=' + f.CAR_ID + '&carIdPrincipal=' + f.CAR_ID + '\')"/>';
                    //_cab += '                    <a class="box_setting fa fa-map" data-toggle="modal" title = "Mapa Pontos de entrega do veículo" onclick="eventOpenMap(\'' + dataAtual + '\', \'' + f.CAR_ID + '\', 2)"/>'; //Sexual Hands - Krupck (MAPAS)
                    _cab += '                </div>';
                    _cab += '            </header>';
                    _cab += '            <div class="content-body" style="display:flex">';
                    _cab += '                <div class="row" style="width:85%;">';
                    _cab += '                    <div class="col-xs-12">';
                    _cab += '                        <table border="0">';
                    _cab += '                            <tr>';
                    _cab += '                                <td valign="bottom">';
                    _cab += '                                    <img width="48px" src="/images/cabine' + f.ORD_COR_FILA + '.png" />';
                    _cab += '                                </td>';
                    _cab += '                                <td style="background-color:#1c1c1c" valign="bottom">';
                    _cab += '                                    <table width="200px" height="42px" style="border-collapse: collapse; border: 1px solid black;">';
                    _cab += '                                        <tr>';
                    _cab += `                                           <input type="hidden" class="${f.EMBARQUE}" value="${f.CAR_ID}"/>` //input hidden para ser acessado pela data, e pegar o valor que é o id da carga
                    _rod = '                                        </tr>';
                    _rod += '                                    </table>';
                    _rod += '                                    <img width="200px" src="/images/carroceria' + f.ORD_COR_FILA + '.png" />';
                    _rod += '                                </td>';
                    _rod += '                            </tr>';
                    _rod += '                            <tr><p id="iniFimPrev_' + f.CAR_ID + '" title="Início e Fim previsto" style="font-size:10px;">Ini: ' + formatarDataHora(f.CAR_DATA_INICIO_PREVISTO) + ' Fim: </p></tr>';
                    //_rod += '                            <tr><p>Status: ' + pegarStatus(f.CAR_STATUS) + '</p></tr>';
                    //_rod += '                            <tr><p data-toggle="tooltip" title="Quantidade de entregas">Qtd de desembarques: ' + f.QTD_DESCARGAS + '</p></tr>';
                    _rod += '                        </table>';
                    _rod1 = '                    </div>';
                    _rod1 += '                </div>';

                    _rod1 += '            <div class="p_caminhao_exped_carga' + f.CAR_ID + '" style="width:33%;"><table>{00}</table>';

                    _rod += `            <input type="hidden" id="hdnEmbarqueCarga${f.CAR_ID}" value='${f.EMBARQUE}'/>`;
                    _rod1 += '            </div>';

                    _rod1 += '            </div>';
                    _rod1 += '        </section>';
                    _rod1 += '    </div>';
                    _Pedido = "";
                    _Cli = "";

                    _Mun = "";
                    _ite = "";

                    //Zika
                    _Table += '<table style="font-size:10px;">';
                    _Table += '<tr align="center">';
                    _Table += '<th style="padding-left:5px" title="Ocupação">Ocupação</th>';
                    _Table += '<th style="padding-left:5px" title="Volume carregado m³">Vol carregado</th>';
                    _Table += '<th style="padding-left:5px" title="Volume livre m³">Vol livre</th>';
                    _Table += '<th style="padding-left:5px" title="Peso pedido">Peso pedido</th>';
                    _Table += '<th style="padding-left:5px" title="Fim produção">Fim prod</th>';
                    _Table += '</tr>'

                    _Table += '<tr align="center">';
                    //_Table += '<td>' + f.CAR_ID + '%</td>';
                    _Table += '<td style="padding-left:5px" id="ocup_' + f.CAR_ID + '" title="Ocupação"></td>'; //Ocupação
                    _Table += '<td style="padding-left:5px" id="vol_car_' + f.CAR_ID + '" title="Volume carregado m³"></td>'; //Volume carregado m³
                    _Table += '<td style="padding-left:5px" id="vol_liv_' + f.CAR_ID + '" title="Volume livre m³"></td>'; //Volume livre m³
                    _Table += '<td style="padding-left:5px" id="peso_tot_' + f.CAR_ID + '" title="Peso pedido"></td>'; //Peso pedido pronto
                    _Table += '<td style="padding-left:5px" id="data_fimprev_' + f.CAR_ID + '" title="Fim produção">' + formatarDataHora(f.FIMPREVISTO) + '**' + f.CAR_ID + '</td > '; //Fim produção

                    _Table += '</tr>';
                    _Table += '</table >';

                    cargasNotInArray.push(`#carga${f.CAR_ID}`);

                    var date = (f.CAR_EMBARQUE_ALVO).toString().slice(0, 10).replaceAll('-', '');

                    date == embarque ? idsCargas.push(f.CAR_ID) : '';

                }

                //Trecho de código de calcula a ocupação; volume carregado, volume livre, percentual pronto, fim previsto
                volume_cidade += f.M3_UE * f.QTD_UE;
                volume_total += f.M3_UE * f.QTD_UE;
                peso_cidade += f.ITC_QTD_PLANEJADA * f.ORD_PESO_UNITARIO;
                peso_total += f.ITC_QTD_PLANEJADA * f.ORD_PESO_UNITARIO;
                cidade_atual = f.PON_ID;
                //Maior fim produção
                temp_fim_prev = formatarDataHora(f.FIMPREVISTO);
                var tfpt = formatarDataHora(f.FIMPREVISTO);
                if (!temp_fim_prev_tot.includes(tfpt))
                    temp_fim_prev_tot.push(tfpt);

                //Maior fim carga
                temp_fim_carg = formatarDataHora(f.CAR_DATA_FIM_PREVISTO);
                tfpt = formatarDataHora(f.CAR_DATA_FIM_PREVISTO);
                if (!temp_fim_carg_tot.includes(tfpt))
                    temp_fim_carg_tot.push(tfpt);


                //Trecho de código que insere a cidade de entrega no lado direito do caminhão
                if (!cliente_atual.includes(f.CLI_NOME)) {
                    cidade_entrega = '<tr class="entrega_' + f.CAR_ID + ' entrega_' + f.CAR_ID + '_' + f.PON_ID + '">';
                    cidade_entrega += '<td><p style="font-size:9px;line-height:8px;">' + f.CLI_NOME + '</p></td>';
                    cidade_entrega += '<td><p style="font-size:9px;line-height:8px;padding-left:5px">{11}</p></td>';
                    cidade_entrega += '<tr>{00}';

                    _rod1 = _rod1.replace("{00}", cidade_entrega);
                    cliente_atual.push(f.CLI_NOME);
                }


                if ((cliente_antigo != "" && cliente_antigo != f.CLI_ID) || (carId_antigo != "" && f.CAR_ID != carId_antigo)) {

                    lista_datas_entrega.sort();
                    var data_str = "";
                    var w = 0;
                    for (; w < lista_datas_entrega.length - 1; w++) {
                        data_str += lista_datas_entrega[w] + ', ';
                    }
                    data_str += lista_datas_entrega[w];

                    lista_datas_entrega = [];
                    body_tab = body_tab.replace('{99}', data_str);

                    lista_datas_aps.push(data_str);
                }
                body_tab += gerarTbodyExpedicao(cargasPlanilha[index_plan]);
                index_plan++;


                var temp_dt = formatarData(f.ORD_DATA_ENTREGA_DE);
                if (!lista_datas_entrega.includes(temp_dt)) {
                    lista_datas_entrega.push(temp_dt);
                }

                if (f.COR_OTIF == "VERDE") {
                    if (cor == "verde1") {
                        cor = "verde2";
                    } else {
                        cor = "verde1";
                    }
                }
                if (f.COR_OTIF == "VERMELHO") {
                    if (cor == "vermelho1") {
                        cor = "vermelho2";
                    } else {
                        cor = "vermelho1";
                    }
                }
                if (f.COR_OTIF == "AMARELO") {
                    if (cor == "amarelo1") {
                        cor = "amarelo2";
                    } else {
                        cor = "amarelo1";
                    }
                }
                if (f.COR_OTIF == "AZUL") {
                    if (cor == "azul1") {
                        cor = "azul2";
                    } else {
                        cor = "azul1";
                    }
                }


                PM3 = f.M3_UE * f.QTD_UE;

                if (_Pedido != "") {
                    _Pedido += ',';
                }
                _Pedido += f.ORD_ID;

                if (_Cli != "") {
                    _Cli += ',';
                }
                _Cli += f.CLI_NOME;

                _m = f.UF + '-' + f.PON_DESCRICAO;
                if (_Mun.indexOf(_m) == -1) {
                    if (_Mun != "") {
                        _Mun += ',';
                    }
                    _Mun += '<input onclick="checkCargaCidade(this, \'' + f.CAR_ID + '\', \'' + f.MUN + '\', \'' + f.PON_ID + '\')" id="' + f.CAR_ID + '_' + f.MUN + '"  class="check_' + f.CAR_ID + '" type="checkbox">' + f.UF + '-' + f.PON_DESCRICAO;
                }

                var tamanho = (PM3 * 100) / f.CAPACIDADE;
                tamanho = tamanho.toFixed(2);
                _ite += '<td width="' + tamanho + '%" class="' + cor + '"></td>';
                total += (PM3 * 100) / f.CAPACIDADE;

            });



            if (total > 0 && total < 100) {
                _ite += '<td width="' + (100 - total) + '%" ></td>';
            }

            _Mun = '<p style="font-size:10px">' + _Mun + '</p>';
            _html += _top + _sec + _cab + _ite + _rod + _Mun + _rod1 + '</div>';
            _html = _html.replace('tbody_caminhao', body_tab);
            _html = _html.replaceAll("{00}", "");

            for (var w = 0; w < lista_datas_aps.length; w++) {
                _html = _html.replace("{11}", lista_datas_aps[w]);
            }
            todas_cargas += _sec + _cab + _ite + _rod + _Mun + _rod1;

            //Atualização dos caminhoezinhos.
            if (carga_id == undefined && dtEmbarque == undefined)
                $('#Cargas').html(_html);
            else if (tipo_alteracao == 1) { //TipoAlteração == 1. Significa que só mudou os valores, mas não mudou a data.
                $('#carga' + carga_id).html(_cab + _ite + _rod + _Mun + _rod1);
            } else if (tipo_alteracao == 2) { //TipoAlteração == 2. Mudou a data, porém para uma data já existente.
                if ($('#carga' + carga_id).parent().children().length == 1) {
                    $('#carga' + carga_id).parent().parent().parent().remove();
                } else {
                    $('#carga' + carga_id).remove();
                }

                $('.expedarea' + pos_alteracao + ' .panel-body').append(_sec + _cab + _ite + _rod + _Mun + _rod1);
            }
            else if (tipo_alteracao == 4) { //TipoAlteração == 4. Atualização do embarque inteiro
                let cargas_na_div = $(`#panel-caminhao${embarque} > [id^='carga']`).length;
                let cargas_no_back = idsCargas.length;
                //se tiver alguma carga que não veio do embarque, quer dizer que houve uma alteração na data
                if (cargas_na_div != cargas_no_back) {
                    tipo_alteracao = 3;
                }
                else {
                    let embarq = '#collap' + (dtEmbarque).slice(0, 10).replaceAll('-', '');
                    $(embarq).collapse('toggle');
                    TabelaFilaExpedicao.carregarTabela(undefined, diasAfrente, acao, undefined, 2, undefined, undefined, undefined, undefined, query);
                }
            }
            if (tipo_alteracao == 3) { //TipoAlteração == 3. Mudou a data, mas para uma data que ainda não existe.
                //TabelaFilaExpedicao.carregarTabela('undefined','undefined','undefined','undefined','undefined','undefined','undefined','undefined','undefined');
                TabelaFilaExpedicao.carregarTabela(undefined, diasAfrente, acao, undefined, 2, undefined, undefined, undefined, undefined, query);
            }
            if (tipo_alteracao == 5) { //Filtrou por status
                //substitui o conteudo de cargas pelas novas cargas, sem alterar a div de embarque
                $('.expedarea' + pos_alteracao + ' .panel-body').html(todas_cargas);
                ajustarValores();
            }

            for (var w = 0; w < lista_objetos.length; w++) {
                $(lista_objetos[w].class).addClass(lista_objetos[w].class_cidade_ocup);
                $(lista_objetos[w].class).addClass(lista_objetos[w].class_cidade_vol_car);
                $(lista_objetos[w].class).addClass(lista_objetos[w].class_cidade_vol_liv);
                $(lista_objetos[w].class).addClass(lista_objetos[w].class_peso_tot);
                $(lista_objetos[w].class).addClass(lista_objetos[w].class_fim_prev);
            }

            for (var w = 0; w < lista_objetos_full.length; w++) {
                $('#ocup_' + lista_objetos_full[w].id).addClass('ocup_' + lista_objetos_full[w].ocupacao_total);
                $('#vol_car_' + lista_objetos_full[w].id).addClass('vol_car_' + lista_objetos_full[w].volume_total);

                var vol_livre = lista_objetos_full[w].volume_livre - lista_objetos_full[w].volume_total;
                vol_livre = vol_livre.toFixed(2);

                $('#vol_liv_' + lista_objetos_full[w].id).addClass('vol_liv_' + vol_livre);

                $('#peso_tot_' + lista_objetos_full[w].id).addClass('peso_tot_' + lista_objetos_full[w].peso_total);

                //Parte de fim produção previsto
                var tfpt = lista_objetos_full[w].fim_prev.split(';');
                var temp = '00/00 00:00';
                for (var j = 0; j < tfpt.length && tfpt[j] != ""; j++) {
                    temp = compararDataHora(temp, tfpt[j]) == 1 ? temp : tfpt[j];
                }
                $('#data_fimprev_' + lista_objetos_full[w].id).addClass('fimprev_tot_' + temp);
                $('#data_fimprev_' + lista_objetos_full[w].id).text(temp);


                //Parte de fim carga previsto
                tfpt = lista_objetos_full[w].fim_prev_carga.split(';');
                var temp = '00/00 00:00';
                for (var j = 0; j < tfpt.length && tfpt[j] != ""; j++) {
                    temp = compararDataHora(temp, tfpt[j]) == 1 ? temp : tfpt[j];
                }

                str = $('#iniFimPrev_' + lista_objetos_full[w].id).text();
                $('#iniFimPrev_' + lista_objetos_full[w].id).text(str + temp);

                //Preenchendo os campos;
                $('#ocup_' + lista_objetos_full[w].id).text(lista_objetos_full[w].ocupacao_total + '%');
                $('#vol_car_' + lista_objetos_full[w].id).text(lista_objetos_full[w].volume_total);
                $('#vol_liv_' + lista_objetos_full[w].id).text(vol_livre);
                $('#peso_tot_' + lista_objetos_full[w].id).text(lista_objetos_full[w].peso_total);

            }

            //Gambiarra
            $('#cargaG00000').remove();
            $('#cargaG99999').remove();

        } else {
            Modal.erro('Erro ao carregar cargas.' + st);
        }

        //adiciona eventos no sel de status
        changeSelStatus();
    }
}
function obterRoteirospossiveis() {
    $.get(UrlBase + 'obterRoteirospossiveis?p=' + $('#d_produtoId').text() + '&s=' + $('#d_seqTransform').text()).done(function (maq) {
        if (maq != null && maq.length > 0) {
            $('#altmaq').html('');
            _html = '<select id= "rotManual" class="form-control input-sm m-bot15"> <option> </option>';
            maq.forEach(function (m) {
                _html += '<option value="' + m.MAQ_ID + '"> ' + m.MAQ_DESCRICAO + '</option>';
            });
            $('#altmaq').append(_html + '</select>');

        } else {
            $('#altmaq').append('Não foram encontrados roteiros para este produto.');
        }
    }).fail(function () {
        $('#altmaq').html(' <span onclick="javascript: obterRoteirospossiveis();">Alterar maquina</span>');
        $('#altmaq').append('Erro ao pesquisar roteiros para este produto.');
    }).always(function () {

    });
}

function buscaAjax() {
    $.ajax({
        url: "/home/busca/",
        data: { termo: $("#txtBusca").val() },
        type: "post",
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            $("#resultado").empty();
            $("#resultado").append("inicio");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#resultado").empty();
            $("#resultado").append("erro");
        },
        success: function (data, textStatus, XMLHttpRequest) {
            $("#resultado").empty();
            $(data).each(function () {
                $("#resultado").append("Nome: " + this.Nome + " Sobrenome:"
                    + this.Sobrenome + " <br/>");
            });
        }
    });
}

function prepararDadosIniciaisAPS(_url) {
    return new Promise((resolve, reject) => {
        limparDados();
        url = _url;
        funcao_pesquisa = _url.indexOf('?') != -1 ? _url.substring(0, _url.indexOf('?')) : "";
        Carregando.abrir('Processando ...');
        $.ajax({
            type: "GET",
            url: _url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.st === "OK") {
                    separarDados(result);
                    pegarCamposPesquisa(lista_estrutura);
                    pegarListaIndicesEstrutura(lista_estrutura);
                    tipo_de_tela = 1;
                    gerarModalFiltros(result.prefencias, "aps");
                    TabelaFilaExpedicao.gerarCamposPesquisa();


                    //se encontrar uma preferencia com tipo === buffer, desmonta ela e exibe na tela
                    let ultima_pesquisa = result.prefencias.filter(function (value) { return value.PRE_TIPO === "BUFFER" })[0];
                    if (ultima_pesquisa != "" && ultima_pesquisa != null && ultima_pesquisa != undefined) {
                        desmontarStringPesquisa(ultima_pesquisa.PRE_VALOR, "aps");
                    }

                    resolve();

                } else {//entrou no try catch do controler 
                    mostraDialogo(result.st, 'danger', 3000);
                    $("#insert" + num).append(result.st);
                    $(".exibir").append(result.st);

                    reject();
                }
            },
            error: OnError,
            complete: function () {
                Carregando.fechar();
            }
        });
    });
}

//adiciona o evento de on change em todos selects de status
function changeSelStatus() {
    $(".selectstatus").unbind('change');
    $('.selectstatus').change(function (event) {
        event.preventDefault()
        let embarque = $(this).attr("data-embarque");
        let dtembarque = $(this).attr("data-dtembarque");
        let pos = $(this).attr("data-pos"); //posição do embarque
        let status = $(this).val();

        TabelaFilaExpedicao.eventoPesquisarPrincipalAPS(pos, embarque, dtembarque, status, 5);
    });
}
function atualizarPorDataEmbarque(posEmbarque, embarque, dtEmbarque) {
    TabelaFilaExpedicao.carregarTabela($('#MaxDiasAntecipacao').val(), 'FULL', 'sim', undefined, 4, posEmbarque, embarque, dtEmbarque)
}