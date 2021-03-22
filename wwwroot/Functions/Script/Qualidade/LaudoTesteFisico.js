
function LaudoTestesFisicos(ROT_MAQ_ID = "", ROT_PRO_ID = "", ORD_ID = "", FPR_SEQ_REPETICAO = "", ROT_SEQ_TRANSFORMACAO = "") {

    var form = document.createElement("form");

    form.method = "post";
    form.action = "/plugandplay/ReportLaudotesteFisico/LaudoTestesFisicos";
    form.setAttribute("target", "LaudoTestesFisicos");


    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("type", "hidden");
    hiddenField.setAttribute("name", "ROT_MAQ_ID");
    hiddenField.setAttribute("value", ROT_MAQ_ID);
    //--
    var hiddenField1 = document.createElement("input");
    hiddenField1.setAttribute("type", "hidden");
    hiddenField1.setAttribute("name", "ORD_ID");
    hiddenField1.setAttribute("value", ORD_ID);
    //--
    var hiddenField2 = document.createElement("input");
    hiddenField2.setAttribute("type", "hidden");
    hiddenField2.setAttribute("name", "ROT_PRO_ID");
    hiddenField2.setAttribute("value", ROT_PRO_ID);
    //--
    var hiddenField3 = document.createElement("input");
    hiddenField3.setAttribute("type", "hidden");
    hiddenField3.setAttribute("name", "FPR_SEQ_REPETICAO");
    hiddenField3.setAttribute("value", FPR_SEQ_REPETICAO);
    //--
    var hiddenField4 = document.createElement("input");
    hiddenField4.setAttribute("type", "hidden");
    hiddenField4.setAttribute("name", "ROT_SEQ_TRANSFORMACAO");
    hiddenField4.setAttribute("value", ROT_SEQ_TRANSFORMACAO);
    form.setAttribute("enctype", "multipart/form-data");
    //--
    form.appendChild(hiddenField);
    form.appendChild(hiddenField1);
    form.appendChild(hiddenField2);
    form.appendChild(hiddenField3);
    form.appendChild(hiddenField4);
    document.body.appendChild(form);
    window.open('', 'LaudoTestesFisicos');

    form.submit();

}
