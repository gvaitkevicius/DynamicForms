    var _itensCal = [];
    var ds = "";
    var calendario;
    newLine = true;
    var matTab = []; while (matTab.push([]) < 7);
    for (var i = 0; i < matTab.length; i++) {
        matTab[i] = 0;
    }
    var Lin = 0;

    function inserirItens(linha) {
        let campos_invalidos_na_linha_inteira = []; //array contendo todos os campos vazios de todas colunas dessa linha
        for (var coluna = 0; coluna < 9; coluna++) {

            //Recuperando dados das colunas
            var dataDe = $('#DataDe_' + linha + '_' + coluna).val();
            var dataAte = $('#DataAte_' + linha + '_' + coluna).val();
            var Turn = $('#TurnoId_' + linha + '_' + coluna).val();
            var Turm = $('#TurmaId_' + linha + '_' + coluna).val();
            var Tipo = $('#Tipo_' + linha + '_' + coluna).val();
            var Limp = $('#LimpesaMaquinas_' + linha + '_' + coluna).val();

            if(dataDe == null || dataDe == "" || dataAte == null || dataAte == null)
                continue;

            //valida os campos 
            let campos_invalidos_na_coluna_atual = validaDados(Tipo,Turn, Turm, Limp, linha, coluna);
            campos_invalidos_na_coluna_atual.forEach(function(currentValue){
                campos_invalidos_na_linha_inteira.push(currentValue);
            });

            //se essa os itens dessa linha,coluna estiverem consistente, prossegue para inserir os itens
            if(campos_invalidos_na_coluna_atual.length == 0){

                for (var i = 0; i < 7; i++) {
                    ds = "";
                    var st = "ds_" + i + "_" + linha;

                    //se o dia da semana atual estiver marcado, gera um item de calendário para ele
                    var item = document.getElementById(st);
                    if (item != null && item.checked == true) {
                        ds += item.value;

                        if (ds != null || ds != "") {
                            addItemCalendario(ds, dataDe, dataAte, Tipo,Turn, Turm, Limp, linha, coluna);
                        }
                        else{
                            let objErro = { identificador: `Dia da Semana, linha: ${linha}, coluna: ${coluna}`, campoId: `ds_${linha}_${coluna}` };
                            camposVazios.push(objErro);
                        }
                        
                        ds = "";
                    }
                }
            }

        }

        return campos_invalidos_na_linha_inteira;
    }

    function addItemCalendario(diaSemana, hIni, hFim, tipo, turno, turma, limp) {
        var item = { 'diaSem': diaSemana, 'hInicio': hIni, 'hFim': hFim, 'tipo': tipo, 'turno': turno, 'turma': turma, 'limp': limp };
        _itensCal.push(item);
    }

    function validaDados(tipo, turno, turma, limp, linha, coluna) {
        let camposVazios = [];

        if (tipo == null || tipo == "") {

            let objErro = { identificador: `Tipo, linha: ${linha + 1}, coluna: ${coluna + 1}`, campoId: `Tipo_${linha}_${coluna}` };
            camposVazios.push(objErro);
        }
        if (turno == null || turno == "") {

            let objErro = { identificador: `Turno, linha: ${linha + 1}, coluna: ${coluna + 1}`, campoId: `TurnoId_${linha}_${coluna}` };
            camposVazios.push(objErro);
        }
        if (turma == null || turma == "") {

            let objErro = { identificador: `Turma, linha: ${linha + 1}, coluna: ${coluna + 1}`, campoId: `TurmaId_${linha}_${coluna}` };
            camposVazios.push(objErro);
        }
        if (limp == null || limp == "") {

            let objErro = { identificador: `Limpeza, linha: ${linha + 1}, coluna: ${coluna + 1}`, campoId: `LimpesaMaquinas_${linha}_${coluna}` };
            camposVazios.push(objErro);
        }

        //valida a consistência dos horários
        if(!validaHoras(linha, coluna)){
            let objErro = { identificador: `Horários da linha ${linha + 1}, coluna: ${coluna + 1}`, campoId: null }; //é preciso refazer a função validarHoras para retornar o id do campo errado
            camposVazios.push(objErro);
        }

        return camposVazios;
    }

    function adicionarLina() {
        Lin++;
        newLine = true;
        var bgTag = ' <tr id="colFields_' + Lin + '">';
        var eoTag = '</tr>'
        var tdCab = ['<td><input class= "form-control col-md-2" id = "DataDe_' + Lin + '_' + matTab[Lin] + '" value = "" name = "DataDe_' + Lin + '_' + matTab[Lin] + '" type = "time" data-format="h:mm TT" ></td>', '<td><input class="form-control col-md-2" id ="DataAte_' + Lin + '_' + matTab[Lin] + '" value ="" name="DataAte_' + Lin + '_' + matTab[Lin] + '" type="time" data-format="h:mm TT"> </td>', '<td><select class="form-control col-md-2" id = "TurnoId_' + Lin + '_' + matTab[Lin] + '" name = "TurnoId_' + Lin + '_' + matTab[Lin] + '" ></select ></td >', '<td><select class="form-control col-md-2" id = "TurmaId_' + Lin + '_' + matTab[Lin] + '" name = "TurmaId_' + Lin + '_' + matTab[Lin] + '" ></select ></td >', '<td><select class="form-control col-md-2" id="Tipo_' + Lin + '_' + matTab[Lin] + '" name="Tipo_' + Lin + '_' + matTab[Lin] + '"> <option value="1">EXPEDIENTE</option> <option value="2">IMPRODUTIVO</option></select> </td>', '<td><select class="form-control col-md-1" id="LimpesaMaquinas_' + Lin + '_' + matTab[Lin] + '" name="LimpesaMaquinas_' + Lin + '_' + matTab[Lin] + '"><option value="0">SIM</option> <option value="1">NÃO</option></select></td>'];
        var tdCabDay = [];

        if (Lin < 6) {

            for (var i = 0; i < 7; i++) {
                tdCabDay.push('<td> <input type="checkbox" id="ds_' + i + '_' + Lin + '" value="' + i + '">   </td>');
            }
            for (var i = 0; i < tdCabDay.length; i++) {
                bgTag += tdCabDay[i];
            }
            for (var i = 0; i < tdCab.length; i++) {
                bgTag += tdCab[i];
            }
            bgTag += eoTag;
            $('#tabBody').append(bgTag);
            if (matTab[Lin] >= 1) {
                addSelect('TurnoId_' + Lin + '_' + matTab[Lin], 'ObterTurnos');
                addSelect('TurmaId_' + Lin + '_' + matTab[Lin], 'ObterTurmas');
            } else {
                addSelect('TurnoId_' + Lin + '_0', 'ObterTurnos');
                addSelect('TurmaId_' + Lin + '_0', 'ObterTurmas');
            }
        }
    }
    function maxCol() {
        var max = 0;
        for (var i = 0; i < Lin; i++) {
            max = (matTab[i] > max) ? matTab[i] : max;
        }
        return max;
    }
    function addCab(nCol) {
        var thCab = ["<th>Hora de Início</th>", "<th>Hora de Termino</th>"];
        while (nCol--) {
            for (var i = 0; i < thCab.length; i++) {
                $('#colCab').append(thCab[i]);
            }
        }
    }
    function adicionarColuna() {
        //Adicionando nova coluna a tela
        matTab[Lin]++
        var thCab = ["<th>Hora de Início</th>", "<th>Hora de Termino</th>", "<th>Turno</th>", "<th>Turma</th>", "<th>Tipo</th>", "<th>Limpeza de Máquina</th>"];
        var tdCab = ['<td><input class= "form-control col-md-2" id = "DataDe_' + Lin + '_' + matTab[Lin] + '" value = "" name = "DataDe_' + Lin + '_' + matTab[Lin] + '" type = "time" data-format="h:mm TT" ></td>', '<td><input class="form-control col-md-2" id ="DataAte_' + Lin + '_' + matTab[Lin] + '" value ="" name="DataAte_' + Lin + '_' + matTab[Lin] + '" type="time" data-format="h:mm TT"> </td>', '<td><select class="form-control col-md-2" id = "TurnoId_' + Lin + '_' + matTab[Lin] + '" name = "TurnoId_' + Lin + '_' + matTab[Lin] + '" ></select ></td >', '<td><select class="form-control col-md-2" id = "TurmaId_' + Lin + '_' + matTab[Lin] + '" name = "TurmaId_' + Lin + '_' + matTab[Lin] + '" ></select ></td >', '<td><select class="form-control col-md-2" id="Tipo_' + Lin + '_' + matTab[Lin] + '" name="Tipo_' + Lin + '_' + matTab[Lin] + '"> <option value="1">EXPEDIENTE</option> <option value="2">IMPRODUTIVO</option> <option value="3">MANUTENÇÃO</option></select> </td>', '<td><select class="form-control col-md-1" id="LimpesaMaquinas_' + Lin + '_' + matTab[Lin] + '" name="LimpesaMaquinas_' + Lin + '_' + matTab[Lin] + '"><option value="0">SIM</option> <option value="1">NÃO</option></select></td>'];
        if (matTab[Lin] >= maxCol()) {
            for (var i = 0; i < thCab.length; i++) {
                $('#colCab').append(thCab[i]);
            }
        }
        for (var i = 0; i < tdCab.length; i++) {
            $('#colFields_' + Lin).append(tdCab[i]);
        }
        addSelect('TurnoId_' + Lin + '_' + matTab[Lin], 'ObterTurnos');
        addSelect('TurmaId_' + Lin + '_' + matTab[Lin], 'ObterTurmas');
    }
    function addSelect(id, obj) {   
        if (obj != null) {
            var data = obj;
            var selectbox = $('#' + id);
            selectbox.find('option').remove();
            $.each(data, function (i, d) {
                $('<option>').val(d.Id).text(d.Descricao).appendTo(selectbox);
            });
        }
    }

    function ObterTurnos(){
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "get",
                url: 'ObterTurnos',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (obj) {
                    resolve(obj);
                },
                error: function (XMLHttpRequest, txtStatus, errorThrown) {
                    var mensagem = `<strong>Erro ao obter turnos!</strong><br>ERRO ao tentar obter os turnos, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                    mostraDialogo(mensagem, 'danger', 3000);     
                    
                    reject();
                }
            });
        });
    }

    function ObterTurmas(){
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "get",
                url: 'ObterTurmas',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (obj) {
                    resolve(obj);
                },
                error: function (XMLHttpRequest, txtStatus, errorThrown) {
                    var mensagem = `<strong>Erro ao obter turmas!</strong><br>ERRO ao tentar obter as turmas, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                    mostraDialogo(mensagem, 'danger', 3000);     
                    
                    reject();
                }
            });
        });
    }

    /**
     * Adiciona eventos nos horarios
     */
    function addEventHorario(row, column) {
        //passa os valores de coluna e linha do elemento
        $("#DataAte_" + row + "_" + column).on("focusout", { _row: row, _column: column }, function (event) {
            if (validaHoras(event.data._row, event.data._column) && !document.getElementById('manutencao').checked)
                autoCompleteHorario(event.data._row, event.data._column);
        });

        $("#DataDe_" + row + "_" + column).on("focusout", { _row: row, _column: column }, function (event) {
            if (validaHoras(event.data._row, event.data._column) && !document.getElementById('manutencao').checked)
                autoCompleteHorario(event.data._row, event.data._column);
        });
    }

    //adiciona os eventos de click no checkbox dos presets. 
    //cada caixa selecionada tem um comportamento
    function addEventPresets() {
        $('#periodoNormal').click(function (e) {
            $('#descricao input, select').prop("disabled", false);
            $("[id^='DataDe']").prop("disabled", true);	//desabilita todos horarios de inicio
            $("[id^='DataDe'][id$='0']").prop("disabled", false); //habilita todos os horarios de inicio da primeira coluna

            clear();
        });
        $('#feriados').click(function (e) {
            $('#descricao input, select').prop("disabled", true);

            clear();

            $('#LimpesaMaquinas_0_0').val('1');
            $('[id^="Tipo').val('0'); //tipo improdutivo
            $("[id^=ds][id$=_0]").prop("checked", true);
            $("#DataDe_0_0").val("06:00");
            $("#DataAte_0_0").val("06:00");
        });
        $('#manutencao').click(function (e) {
            clear();
            $('#descricao input, select').prop("disabled", false);
            $('[id^="LimpesaMaquinas"').val('0'); //limpesa nao
            $('[id^="Tipo').val('2'); //tipo manutencao
            $("[id^='DataDe']").prop("disabled", false);	
        });
    }

    /**
     * Adiciona feature de auto completar nos horarios
     */
    function autoCompleteHorario(row, column) {

        //se a data de inicio e termino forem a mesmas, a linha fechou 24hrs.
        if ($("#DataDe_" + row + "_0").val() == $("#DataAte_" + row + "_" + column).val()) {

            //limpa os campos a frente
            var horariosInicioLinha = $("[id^=DataDe_" + row + "]").toArray();
            var horariosTerminoLinha = $("[id^=DataAte_" + row + "]").toArray();

            horariosInicioLinha.forEach(function (currentValue, index) {
                //verifica se o campo está a frente do horario atual
                if (index > column) {
                    $("#DataDe_" + row + "_" + index)[0].value = "";
                }
            });
            horariosTerminoLinha.forEach(function (currentValue, index) {
                //verifica se o campo está a frente do horario atual
                if (index > column) {
                    $("#DataAte_" + row + "_" + index)[0].value = "";
                }
            });
            return;
        }

        //seta o proximo horario de inicio
        if ($("#DataDe_" + row + "_" + (column + 1)).length) {
            $("#DataDe_" + row + "_" + (column + 1)).val(
                $("#DataAte_" + row + "_" + column).val()
            );
        }

        //seta o proximo horario de termino
        if ($("#DataAte_" + row + "_" + (column + 1)).length) {
            $("#DataAte_" + row + "_" + (column + 1)).val(
                $("#DataDe_" + row + "_0").val()
            );
        }
    }

    /**
     * Chama as funções de validações dos horários
     **/
    function validaHoras(row, column) {

        if (!hrInicioLinha_igual_hrTerminoLinhaAnterior(row, column)) {
            return false;
        }
        else if (!hrTerminoAnterior_igual_hrInicioAtual(row, column)) {
            return false;
        }
        else if (!hrTermino_igual_hrInicio(row, column)) {
            return false;
        }
        else if(!hrInicio_e_hrTermino_preenchidos(row, column)){
            return false;
        }
        else {
            return true;
        }
    }

    /**
     * Valida se a hora de ínicio atual ou a hora de térmmino são diferentes de vazios, se forem, valida se as duas estão preenchidas
     * @param {*} row 
     * @param {*} column 
     */
    function hrInicio_e_hrTermino_preenchidos(row, column){
        let hora_inicio_atual = $(`#DataDe_${row}_${column}`).val();
        let hora_termino_atual = $(`#DataAte_${row}_${column}`).val();

        //se um dos campos esta preenchido, entao tanto a hora de inicio quanto de término devem estar preenchidos
        if(hora_inicio_atual !== null || hora_inicio_atual !== "" || hora_termino_atual !== null || hora_termino_atual !== ""){

            if(hora_termino_atual === "" || hora_termino_atual === "" || hora_inicio_atual === "" || hora_inicio_atual === null)
                return false;
        }

        return true;
    }

    /**
     * Checa se o último horario de termino da linha anterior é igual ao primeiro horário de inicio da linha atual
     */
    function hrInicioLinha_igual_hrTerminoLinhaAnterior(row, column) {
        var horariosTermino_linhaAnterior = $("[id^=DataAte_" + (row - 1) + "]").toArray();
        var filteredHorariosTermino_linhaAnterior = horariosTermino_linhaAnterior.filter(notEmpty);

        if (filteredHorariosTermino_linhaAnterior.length > 0) {
            var ultimoHorarioTermino_linhaAnterior = filteredHorariosTermino_linhaAnterior[filteredHorariosTermino_linhaAnterior.length - 1].value;
            var primeiroHorarioInicio_linhaAtual = $("#DataDe_" + row + "_0")[0].value;

            //se o ultimo horario de termino na linha anterior for diferente do primeiro horario de inicio da linha atual\
            if (ultimoHorarioTermino_linhaAnterior != primeiroHorarioInicio_linhaAtual &&
                primeiroHorarioInicio_linhaAtual !== "") {
                mostraDialogo("O horario de inicio da linha deve ser igual ao horario de termino da linha anterior. <br>Linha: " + (row + 1) + "<br>Coluna de Horário: " + (column + 1), "danger", 5000);
                $("#DataDe_" + row + "_0")[0].value = ultimoHorarioTermino_linhaAnterior;
                return false;
            }
        }

        return true;
    }

    /**
     * Checa se o horário de termino anterior é igual ao horário de inicio atual
     */
    function hrTerminoAnterior_igual_hrInicioAtual(row, column) {
        var hrTerminoAnterior = $("#DataAte_" + row + "_" + (column - 1))[0];
        var hrInicioAtual = $("#DataDe_" + row + "_" + column)[0];

        if (hrTerminoAnterior !== undefined && hrInicioAtual !== undefined &&
            hrInicioAtual.value !== "" && column > 0) {
            if (hrTerminoAnterior.value !== hrInicioAtual.value) {
                mostraDialogo("O horario de inicio deve ser igual ao horario de termino anterior.  <br>Linha: " + (row + 1) + "<br>Coluna de Horário: " + (column + 1), "danger", 5000);
                $("#DataDe_" + row + "_" + column)[0].value = hrTerminoAnterior.value;

                return false;
            }
        }

        return true;
    }

    /**
     * Pega o primeiro horario de inicio e o ultimo horario de termino, e checa se eles são iguais
     */
    function hrTermino_igual_hrInicio(row, column) {

        var horariosInicio = $("[id^=DataDe_" + row + "]").toArray();
        var horariosTermino = $("[id^=DataAte_" + row + "]").toArray();

        var filteredHorariosInicio = horariosInicio.filter(notEmpty);

        if (horariosTermino[horariosTermino.length - 1] !== undefined && horariosTermino[horariosTermino.length - 1].value !== "") {
            if (filteredHorariosInicio[0].value !== horariosTermino[horariosTermino.length - 1].value) {

                $("#DataAte_" + row + "_" + column)[0].value = horariosInicio[0].value;
                mostraDialogo("O ultimo horario de termino da linha deve ser igual ao primeiro horario de inicio.  <br>Linha: " + (row + 1) + "<br>Coluna de Horário: " + (column + 1), "danger", 5000);

                return false;
            }
        }

        return true;
    }

    /**
     * Pega o campo de horario inicio e termino da linha e coluna atual, e checa se o horário inicio não é maior que o horário de termino.
     */
    function hrInicio_maior_hrTermino(row, column) {
        var horariosInicio = $("[id^=DataDe_" + row + "]").toArray();
        var filteredHorariosInicio = horariosInicio.filter(notEmpty);

        if ($("#DataDe_" + row + "_" + column).val() >
            $("#DataAte_" + row + "_" + column).val() &&
            filteredHorariosInicio[0].value !== $("#DataAte_" + row + "_" + column)[0].value) {
            mostraDialogo("O horario de inicio nao pode ser maior que o horario de termino e deve haver pelomenos um horario de inicio e um horario de termino.  <br>Linha: " + (row + 1) + "<br>Coluna de Horário: " + (column + 1), "danger", 5000);
            return false;
        }

        return true;
    }

    function desabilitarHorariosInicio(row, column) {
        if (column > 0) {
            $("#DataDe_" + row + "_" + column).attr("disabled", "disabled");
        }

    }

    function popularTabelaMaquinas(urlData) {
        $.ajax({
            type: "get",
            url: urlData,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (obj) {
                if (obj !== null) {
                    var data = obj;
                    data.forEach(function (currentValue) {
                        let hiddenCalId = currentValue.CAL_ID != null ? 
                            `<input type="hidden" name="hdnCalIdMaq${currentValue.MAQ_ID}" id="hdnCalIdMaq${currentValue.MAQ_ID}" value="${currentValue.CAL_ID}"></input>`
                            :
                            `<input type="hidden" name="hdnMaquinaSemCal${currentValue.MAQ_ID}" id="hdnMaquinaSemCal${currentValue.MAQ_ID}" value="${currentValue.MAQ_ID}"></input>`;

                        var tr = `
                                    <tr>
                                        <td><input type="text" value="${currentValue.MAQ_ID}" id="txtMaquinaMaqId" name="txtMaquinaMaqId" disabled="disabled"/></td>
                                        <td><input type="text" value="${currentValue.CAL_ID}" id="txtMaquinaCalId" name="txtMaquinaCalId" disabled="disabled" style='max-width: 100px'/></td>
                                        <td><input type="text" value="${currentValue.MAQ_DESCRICAO}" id="txtMaquinaMaqDesc" name="txtMaquinaMaqDesc" disabled="disabled" style='max-width: 100px'/></td>
                                        <td><input type="checkbox" id="checkboxIncluirMaq${currentValue.MAQ_ID}" name="checkboxIncluirMaq" value="${currentValue.MAQ_ID}" /> </td>
                                        <td><i class="fa fa-search-plus circle" onclick="popularTabelaHorarios('${currentValue.CAL_ID}', 0)"></i> </td>

                                    </tr>`;

                        $("#tbMaquinas tbody").append(tr);
                        $("#tbMaquinas thead").append(hiddenCalId);
                    })
                }
                else {
                    mostraDialogo("Não foi encontrado nenhuma máquina cadastrada no sistema.", "warning");
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao popular tabela de máquinas!</strong><br>ERRO ao tentar popular a tabela de máquinas, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);     

            }
        })
    }

    function popularTabelaEquipes(urlData){
        $.ajax({
            type: "get",
            url: urlData,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (obj) {
                if (obj !== null) {
                    var data = obj;
                    data.forEach(function (currentValue) {
                        let hiddenCalId = currentValue.CAL_ID != null ? 
                        `<input type="hidden" name="hdnCalIdEqu${currentValue.EQU_ID}" id="hdnCalIdEqu${currentValue.EQU_ID}" value="${currentValue.CAL_ID}"></input>`
                        :
                        `<input type="hidden" name="hdnEquipeSemCal${currentValue.EQU_ID}" id="hdnEquipeSemCal${currentValue.EQU_ID}" value="${currentValue.EQU_ID}"></input>`;

                        var tr = `
                                    <tr>
                                        <td><input type="text" value="${currentValue.EQU_ID}" id="txtEquipeEquId" name="txtEquipeEquId" disabled="disabled" /></td>
                                        <td><input type="text" value="${currentValue.CAL_ID}" id="txtEquipeCalId" name="txtEquipeCalId" disabled="disabled" style='max-width: 100px'/></td>
                                        <td><input type="checkbox" id="checkboxIncluirEqu${currentValue.EQU_ID}" name="checkboxIncluirEqu" value="${currentValue.EQU_ID}" onchange="obterCalIdEquipe('${currentValue.EQU_ID}')" /> </td>
                                        <td><i class="fa fa-search-plus circle" onclick="popularTabelaHorarios('${currentValue.CAL_ID}', 1)"></i> </td>

                                    </tr>`;

                        $("#tbEquipes tbody").append(tr);
                        $("#tbEquipes thead").append(hiddenCalId);
                    });
                }
                else {
                    mostraDialogo("Não foi encontrado nenhuma máquina cadastrada no sistema.", "warning");
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao popular tabela de equipes!</strong><br>ERRO ao tentar popular a tabela de equipes, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);     
                                
            }
        })
    }

    /**
     * 
     * @param {*} calId 
     * @param {*} maquina_ou_equipe maquina == 0 equipe == 1 
     */
    function popularTabelaHorarios(calId, maquina_ou_equipe){
        //limpar a tabela
        clearTbHorarios(maquina_ou_equipe);

        //pega a data de termino especificada no calendario para trazer a ultima semana baseada nesta data
        var dTermino = null;
        if ($('#DataFim').val() !== "" && $('#DataFim').val() !== null) {
            dTermino = $('#DataFim').val();
        }

        let urlData = "ObterItensCalendario";
        $.ajax({
            type: "get",
            url: urlData,
            dataType: 'json',
            data: {
                CalId: calId !== 'null' ? calId: null,
                dataReferencia: dTermino
            },
            contentType: "application/json; charset=utf-8",
            success: function (obj) {
                if (obj != null) {
                    var data = obj;
                    preencherTabelaHorariosHTML(data, maquina_ou_equipe);
                }

                if (obj == null) {
                    mostraDialogo('A maquina/equipe selecionada não está vinculada a nenhum calendário.', 'warning');
                }
                else if (obj.length <= 0) {
                    mostraDialogo('Não foi encontrado nenhum registro para a maquina/equipe e período pesquisado(' + dTermino + ').');
                }
            },
            error: function (XMLHttpRequest, txtStatus, errorThrown) {
                var mensagem = `<strong>Erro ao obter horários de maquina/equipe!</strong><br>ERRO ao tentar obter os horários de máquina/equipe, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                mostraDialogo(mensagem, 'danger', 3000);     
            }
        })
    }

    /**
     * Gera o body da tabela de horário para maquina ou equipe
     * @param {*} data 
     * @param {*} maquina_ou_equipe 0 para maquina 1 para equipe
     */
    function preencherTabelaHorariosHTML(data, maquina_ou_equipe) {
        let html_completo = ``;

        //visita cada dia da semana
        data.forEach(function (currentDayOfWeek, index) {

            let inicio_dia = '<tr>';
            let horarios_no_dia = '';

            //variavel que sera colocada como um dos atributtos do input type checkbox
            //se for o dia da semana, estara como checked=true
            var dInicio = new Date(currentDayOfWeek[0].ICA_DATA_DE);
            var dayOfWeek = dInicio.getDay();
            var domingo = (dayOfWeek == '0') ? "checked" : "";
            var segunda = (dayOfWeek == '1') ? "checked" : "";
            var terca = (dayOfWeek == '2') ? "checked" : "";
            var quarta = (dayOfWeek == '3') ? "checked" : "";
            var quinta = (dayOfWeek == '4') ? "checked" : "";
            var sexta = (dayOfWeek == '5') ? "checked" : "";
            var sabado = (dayOfWeek == '6') ? "checked" : "";

            //inicia o tr e preenche os dias da semana
            var day_of_week = `
                            <td><input type='checkbox' ${domingo} disabled='true'></td>
                            <td><input type='checkbox' ${segunda} disabled='true'></td>
                            <td><input type='checkbox' ${terca} disabled='true'></td>
                            <td><input type='checkbox' ${quarta} disabled='true'></td>
                            <td><input type='checkbox' ${quinta} disabled='true'></td>
                            <td><input type='checkbox' ${sexta} disabled='true'></td>
                            <td><input type='checkbox' ${sabado} disabled='true'></td>`;

            //horarios dentro do dia
            currentDayOfWeek.forEach(function (_currentHourInDay, index) {
                var dInicio = new Date(_currentHourInDay.ICA_DATA_DE);
                var dTermino = new Date(_currentHourInDay.ICA_DATA_ATE);
                var hrInicio = dInicio.toLocaleTimeString(undefined, {
                    hour: '2-digit',
                    minute: '2-digit'
                });
                var hrTermino = dTermino.toLocaleTimeString(undefined, {
                    hour: '2-digit',
                    minute: '2-digit'
                });

                var tableLimpesaTipo = `<table class='table'>
                                            <thead>
                                                <tr>
                                                    <th>Tipo</th>
                                                    <th>Limpeza Maquina</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <input type='text' size='10'disabled='true' value='${_currentHourInDay.ICA_TIPO == 0 ? 'IMPRODUTIVO' : _currentHourInDay.ICA_TIPO == 1 ? 'EXPEDIENTE' : _currentHourInDay.ICA_TIPO == 2 ? 'MANUTENCAO' : 'TIPO NÃO IDENTIFICADO'}'>
                                                    </td>

                                                    <td>
                                                        <input type='text' class='form-control' disabled='true' value='${_currentHourInDay.ICA_LIMPESA_MAQUINA == 1 ? 'SIM' : 'NÃO'}'>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>`;
                var tableHorarioTermino = `<table class="table">
                                            <thead>
                                                <tr>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td colspan="2">
                                                        <input type='text' class='form-control' value='${hrTermino}'' disabled='true' style='max-width: 100px'>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        ${tableLimpesaTipo}
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>`;

                var tableTurnoTurma = `<table class='table'>
                                            <thead>
                                                <tr>
                                                    <th>Turma</th>
                                                    <th>Turno</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <input type='text' class='form-control' disabled='true' value='${_currentHourInDay.URM_ID}'>
                                                    </td>

                                                    <td>
                                                        <input type='text' class='form-control' disabled='true' value='${_currentHourInDay.URN_ID}'>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>`;

                var tableHorarioInicio = `<table class="table">
                        <thead>
                            <tr>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="2" >
                                    <input type='text' class='form-control' value='${hrInicio}' disabled='true' style='max-width: 100px'>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ${tableTurnoTurma}
                                </td>
                            </tr>
                        </tbody>
                    </table>`;

                //incrementa a quantidade de horarios
                horarios_no_dia += `
                        <td>
                            <div class="table-responsive">
                                ${tableHorarioInicio}
                            </div>
                        </td>
                        <td>
                            <div class='table-responsive'>
                                ${tableHorarioTermino}
                            </div>
                        </td>`;

            });

            let fim_dia = '</tr>';

            html_completo += inicio_dia + //começa o <tr>
                day_of_week + //preenche os td de checkbox do dia da semana
                horarios_no_dia + //preenche os td contendo os horarios da linha
                fim_dia; //fecha o </tr>
        });

        //maquina_ou_equipe == maquina
        if(maquina_ou_equipe == 0) $("#tabBodyHorariosMaquina").append(html_completo);

        //maquina_ou_equipe == equipe
        if(maquina_ou_equipe == 1) $("#tabBodyHorariosEquipe").append(html_completo);
    }

    // function obterCalIdMaquina(maqId) {
    //     if ($(`[id='checkboxIncluirMaq${maqId}']`).is(':checked') && !$(`[id='hdnCalId${maqId}']`).length) {
            
    //         $.ajax({
    //             type: "get",
    //             data: {
    //                 MaqId: maqId
    //             },
    //             url: `ObterCalId`,
    //             dataType: 'json',
    //             contentType: "application/json; charset=utf-8",
    //             success: function (calId) {
    //                 if (calId !== null) {
    //                     //armazenar id em um hidden
    //                     $("#tbMaquinas").append(`<input type="hidden" name="hdnCalId${maqId}" id="hdnCalId${maqId}" value="${calId}">`);
    //                 }
    //                 else {
    //                     $("#tbMaquinas").append(`<input type="hidden" name="hdnMaquinaSemCal${maqId}" id="hdnMaquinaSemCal${maqId}" value="${maqId}">`);
    //                     mostraDialogo("Esta máquina não está vinculada à nenhum calendário.", "warning");
    //                 }
    //             },
    //             error: function (XMLHttpRequest, txtStatus, errorThrown) {
    //                 var mensagem = `<strong>Erro ao obter calendário da máquina!</strong><br>ERRO ao tentar obter o calendário da máquina, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
    //                 mostraDialogo(mensagem, 'danger', 3000);     
    //             }
    //         })
    //     }
    //     else if (!$(`[id='checkboxIncluirMaq${maqId}']`).is(':checked')) { //o usuario deselecionou o calendario

    //         //remove o respectivo hidden
    //         if ($(`[id='hdnCalId${maqId}']`).length) {
    //             $(`[id='hdnCalId${maqId}']`).remove();
    //         }
    //         if ($(`[id='hdnMaquinaSemCal${maqId}']`).length) {
    //             $(`[id='hdnMaquinaSemCal${maqId}']`).remove();
    //         }
    //     }
    // }

    // function obterCalIdEquipe(equId){
    //     if ($(`[id='checkboxIncluirEqu${equId}']`).is(':checked') && !$(`[id='hdnCalId${equId}']`).length) {

    //         $.ajax({
    //             type: "get",
    //             data: {
    //                 EquId: equId
    //             },
    //             url: `ObterCalId`,
    //             dataType: 'json',
    //             contentType: "application/json; charset=utf-8",
    //             success: function (calId) {
    //                 if (calId !== null) {
    //                     //armazenar id em um hidden
    //                     $("#tbEquipes").append(`<input type="hidden" name="hdnCalId${equId}" id="hdnCalId${equId}" value="${calId}">`);
    //                 }
    //                 else {
    //                     $("#tbEquipes").append(`<input type="hidden" name="hdnEquipeSemCal${equId}" id="hdnEquipeSemCal${equId}" value="${equId}">`);
    //                     mostraDialogo("Esta máquina não está vinculada à nenhum calendário.", "warning");
    //                 }
    //             },
    //             error: function (XMLHttpRequest, txtStatus, errorThrown) {
    //                 var mensagem = `<strong>Erro ao obter calendário de equipe!</strong><br>ERRO ao tentar obter o calendário de equipe, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
    //                 mostraDialogo(mensagem, 'danger', 3000);                     
    //             }
    //         })
    //     }
    //     else if (!$(`[id='checkboxIncluirEqu${equId}']`).is(':checked')) { //o usuario deselecionou o calendario

    //         //remove o respectivo hidden
    //         if ($(`[id='hdnCalId${equId}']`).length) {
    //             $(`[id='hdnCalId${equId}']`).remove();
    //         }
    //         if ($(`[id='hdnEquipeSemCal${equId}']`).length) {
    //             $(`[id='hdnEquipeSemCal${equId}']`).remove();
    //         }
    //     }
    // }

    function gerarColuna(linha, coluna) {
        if (coluna == 0) {
            var checkbox = `
                        <td>
                            <input type="checkbox" id="ds_0_${linha}" value="0">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_1_${linha}" value="1">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_2_${linha}" value="2">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_3_${linha}" value="3">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_4_${linha}" value="4">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_5_${linha}" value="5">
                        </td>
                        <td>
                            <input type="checkbox" id="ds_6_${linha}" value="6">
                        </td>
                    `;

            $(`#colFields_${linha}`).append(checkbox);
        }


        var html = `
                        <td style='margin:0; padding: 0;'>
                            <div class='table-responsive'>
                            <table class="table">
                                <thead>
                                    <tr>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="2"><input class="form-control col-md-5" id="DataDe_${linha}_${coluna}" value="" name="DataDe_${linha}_${coluna}" type="time" data-format="h:mm TT "></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th>Turma</th>
                                                        <th>Turno</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <select class="form-control col-md-2" id="TurmaId_${linha}_${coluna}" name="TurmaId_${linha}_${coluna}"></select>
                                                        </td>
                                                        <td>
                                                            <select class="form-control col-md-2" id="TurnoId_${linha}_${coluna}" name="TurnoId_${linha}_${coluna}"></select>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </td>

                        <td>
                            <table class="table">
                                <thead>
                                    <tr>

                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="2"><input class="form-control col-md-5" id="DataAte_${linha}_${coluna}" value="" name="DataAte_${linha}_${coluna}" type="time" data-format="h:mm TT "></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th>Tipo</th>
                                                        <th>Limpeza</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <select class="form-control col-md-2" id="Tipo_${linha}_${coluna}" name="Tipo_${linha}_${coluna}">
                                                                <option value="1">EXPEDIENTE</option>
                                                                <option value="0">IMPRODUTIVO</option>
                                                                <option value="2">MANUTENÇÃO</option>
                                                            </select>
                                                        </td>
                                                        <td>
                                                            <select class="form-control col-md-1" id="LimpesaMaquinas_${linha}_${coluna}" name="LimpesaMaquinas_${linha}_${coluna}">
                                                                <option value="1">SIM</option>
                                                                <option value="0">NÃO</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                `;

        $(`#colFields_${linha}`).append(html);
    }

    function gerarLinha(linha, coluna) {
        var html = `
                    <tr id="colFields_${linha}" style='margin: 0; padding: 0;'>
                    </tr>`;

        $("#tabBody").append(html);
    }

    function clear() {
        var horariosInicio = $("[id^=DataDe").toArray();
        var horariosTermino = $("[id^=DataAte").toArray();

        horariosInicio.forEach(function (currentValue, index) {
            //verifica se o campo está a frente do horario atual
            currentValue.value = "";
        });
        horariosTermino.forEach(function (currentValue, index) {
            //verifica se o campo está a frente do horario atual
            currentValue.value = "";
        });

        var diaSem = $("[id^=ds]").toArray();

        diaSem.forEach(function (currentValue) {
            currentValue.checked = '';
        });
    }

    function clearTbHorarios(maquina_ou_equipe) {
        if(maquina_ou_equipe == 0) $('#tbHorariosMaquinas tbody').html('');
        if(maquina_ou_equipe == 1) $('#tbHorariosEquipes tbody').html('');
    }

    function notEmpty(input) {
        return input.value !== ""
    }

    function abrirIC() {
        $('#itensLoad').html('<i class="fa fa-spinner fa-pulse fa-6x fa-fw"></i>Gerando Calendário');
    }

    function fecharIC() {
        $('#itensLoad').html('<i class="fa fa-check">  </i>Calendário de Maquina Gerado');
    }

    var ApApp = {

        clickBtnGerar: async function () {
            _itensCal = [];
            let mensagem_erro = ``;
            let todos_campos_invalidos = [];

            //percorre todas as linhas
            for (var i = 0; i < 8; i++) {

                //a função inserirItens percorre todas colunas, verifica os dados e insere no itens cal
                let campos_vazios_na_linha_atual = inserirItens(i, 9);
                campos_vazios_na_linha_atual.forEach(function(currentValue){

                    todos_campos_invalidos.push(currentValue);
                });
            }

            //constrói a mensagem de erro
            todos_campos_invalidos.forEach(function(currentValue){
                mensagem_erro += `<p>O(s) campo(s) ${currentValue.identificador} está inconsistente.</p>`;
            });

            //validad se selecionou algum dia da semana
            if (!$("[id^=ds_]:checkbox:checked").length) {
                mensagem_erro += `<p>Nenhum dia da semana selecionado.</p>`;
            }

            //valida se a data de inicio e fim do calendario estao preenchidas
            var dtInicio = $('#DataIni').val();
            var dtFim = $('#DataFim').val();
            if (dtInicio == null || dtInicio == '' || dtFim == null || dtFim == '') {
                mensagem_erro += `<p>Data do calendário não preenchida.</p>`;
            }

            //valida se existe pelomenos um horario de inicio e um horario de termino
            var hrsInicio = $('[id^=DataDe_').filter(function () { return !!this.value });
            var hrsTermino = $('[id^=DataAte_').filter(function () { return !!this.value });

            if(!hrsInicio.length)
                mensagem_erro += `<p>Horários de ínicio não preenchidos</p>`;
            if(!hrsTermino.length)
                mensagem_erro += `<p>Horários de término não preenchidos</p>`;

            //cria uma lista das maquinas selecionadas
            lstMaquinas = [];
            $("[id^=checkboxIncluirMaq]:checkbox:checked").each(function (index, element) {

                //cria uma maquina com calId = 0
                let maquina = { maqId: element.value, calId: 0 };

                //se achar um código do calendário adiciona, se nao, cria um novo calendario
                if ($(`[id='hdnCalIdMaq${element.value}']`).length) {
                    var value = $(`[id='hdnCalIdMaq${element.value}']`).val();
                    maquina.calId = value;
                }

                lstMaquinas.push(maquina);
            });

            //cria uma lista das equipes selecionadas
            lstEquipes = [];
            $("[id^=checkboxIncluirEqu]:checkbox:checked").each(function (index, element) {

                //cria uma maquina com calId = 0
                let equipe = { equId: element.value, calId: 0 };

                //se achar um código do calendário adiciona, se nao, cria um novo calendario
                if ($(`[id='hdnCalIdEqu${element.value}']`).length) {
                    var value = $(`[id='hdnCalIdEqu${element.value}']`).val();
                    equipe.calId = value;
                }

                lstEquipes.push(equipe);
            });

            calendario = { 'DataInicio': dtInicio, 'DataFim': dtFim };
            preset = $("input[name='preset']:checked").val();


            if (mensagem_erro.length == 0) {
                abrirIC();

                $.ajax({
                    type: 'POST',
                    url: '/PlugAndPlay/ItensCalendario/CadastrarItensCalendario',
                    cache: false,
                    async: true,
                    data: { Calendario: JSON.stringify(calendario), ItensDoCalendario: JSON.stringify(_itensCal), jsonMaquinas: JSON.stringify(lstMaquinas), jsonEquipes: JSON.stringify(lstEquipes), Preset: preset },
                    datatype: 'JSON',
                    success: function (data) {
                        alert("Calendário Gerado com sucesso!");
                        fecharIC();
                        document.location.reload(true);
                        //$('#btnGerar').attr("disabled", true
                    },
                    error: function (XMLHttpRequest, txtStatus, errorThrown) {
                        var mensagem = `<strong>Erro ao inserir calendário!</strong><br>ERRO ao tentar inserir o calendário, a mensagem de erro é: ${XMLHttpRequest.status} ${errorThrown}.`;
                        mostraDialogo(mensagem, 'danger', 3000);                    
                    }
                });
            }
            else {
                mostraDialogo(mensagem_erro, 'danger', 3000);
            }
        }
    };

    $(document).ready(function () {
        let turnos;
        let turmas;

        //obtem e armazena os turnos
        ObterTurnos().then(function(result){
            turnos = result;

            //quando terminar obtem e armazena as turmas
            ObterTurmas().then(function(result){
                turmas = result;

                //prossegue para criar a tabela
                for (var i = 0; i < 7; i++) {
                    gerarLinha(i);
        
                    for (var j = 0; j < 9; j++) {
                        gerarColuna(i, j);
                        addEventHorario(i, j);
                        desabilitarHorariosInicio(i, j);
                        addSelect('TurnoId_' + i + '_' + j, turnos);
                        addSelect('TurmaId_' + i + '_' + j, turmas);
                    }
                }
        
                popularTabelaMaquinas('ObterMaquinas');
                popularTabelaEquipes('ObterEquipes');
                addEventPresets();
                addCab(8);
                $('#btnGerar').click(ApApp.clickBtnGerar);
            })
        });

        
    });
