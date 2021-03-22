function CalendarioDisponibilidade(dataSelecionada) {
    var strNamespace = 'DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaCalendarioDisponibilidadeVeiculos';
    var teste = dataSelecionada.toString();
    var arrayDeValoresDefault = { DataAtual: teste };
    var strArrayDeValoresDefault = JSON.stringify(arrayDeValoresDefault);
    var ahref = '/DynamicWeb/LinkSGI?str_namespace=' + strNamespace + '&ArrayDeValoresDefault=' + strArrayDeValoresDefault;
    window.open(ahref.toString());
}