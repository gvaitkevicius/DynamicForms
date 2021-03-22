window.onload = function () {
    //se existe um campo de código de barras, adiciona um evento nele
    let cod = document.getElementById('codigo_de_barras');

    if (cod != null) {
        //foca o código de barras sempre que carregar a tela
        cod.focus();

        //se digitar os 29 digitos do código de barras, irá focar no próximo campo
        cod.addEventListener('change', function () {
            if (this.value.length >= 29) {
                let campos = document.getElementsByClassName('campo');

                if (campos.length >= 2) campos[1].focus();
            }
        });
    }

};