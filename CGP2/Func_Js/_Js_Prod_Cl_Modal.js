function add_m_list(ccompany, cvendedor, cproducto, cnamefc, ccostobruto_u1, cprecioventamax, c_nomb_comer, cname) {
    alert("hola");
    $("#linea2").append("<tr><td>" + cproducto + "</th>" +
                            "<td>" + ccompany + "</th>" +
                            "<td>" + cvendedor + "</th>" +
                            "<td>" + cproducto + "</th>" +
                            "<td>" + cnamefc + "</th>" +
                            "<td>" + ccostobruto_u1 + "</th>" +
                            "<td>" + cprecioventamax + "</th>" +
                            "<td>" + c_nomb_comer + "</th>" +
                            "<td>" + cname + "</td></tr>");
}

//paginacion
function pag(pos) {
    var sk = ((pos - 1 ) * 10); 
    var ta = sk + 10;

    var result = @Html.Raw(JResult);
    var prin = "";
    for (var i = sk; i < ta; i++) {
        prin += '<tr>' +
        '<td><button type="button" class="btn btn-info btn-sm" onclick="add3(\'' + 
        result[i]['cproducto'] + '\', \'' +
        result[i]['ccompany'] + '\', \'' +
        result[i]['cvendedor'] + '\', \'' +
        result[i]['cproducto'] + '\', \'' +
        result[i]['cnamefc'] + '\', \'' +
        result[i]['ccostobruto_u1'] + '\', \'' +
        result[i]['cprecioventamax'] + '\', \'' +
        result[i]['c_nomb_comer'] + '\', \'' +
        result[i]['cname'] + '\')" >' +
        result[i]['cproducto'] + '</button></td>' +
                    '<td>' + result[i]['ccompany'] + '</td>' +
                    '<td>' + result[i]['cvendedor'] + '</td>' +
                    '<td>' + result[i]['cproducto'] + '</td>' +
                    '<td>' + result[i]['cnamefc'] + '</td>' +
                    '<td>' + result[i]['ccostobruto_u1'] + '</td>' +
                    '<td>' + result[i]['cprecioventamax'] + '</td>' +
                    '<td>' + result[i]['c_nomb_comer'] + '</td>' +
                    '<td>' + result[i]['cname'] + '</td></tr>';
    }
    $("#fuck_here tr").remove().ready($("#fuck_here").append(prin));
} 