function js_foreach() {
    //res <=> JSON
    $.each(res, function (i, item) {
        alert(item.cproducto);
    });
    $.each(res, function (i, item) {
        $.each(item, function (b, item2) {
            alert(item2);
        });
    });
}

var JSON_fin = [];
function js_table_values_to_json() {
    //KEY3 => obtengo el valor de un input que el id sea el valor KEY1 <=> util para cuando es una tabla genera dinamicamente
    //KEY4 => obtengo el valor de un input cuando se el id
    $("table tbody").find('tr').each(function (i) {
        var $tds = $(this).find('td');
        var JSON_New = {
            "KEY1": $tds.eq(0).text().trim(),
            "KEY2": $tds.eq(1).text().trim(),
            "KEY3": $("#" + $tds.eq(0).text().trim()).val(),
            "KEY4": $("#id_input").val()
        };
        JSON_fin.push(JSON_New);
    });
}

function view_controller_massive_json() {
    var js1 = JSON.stringify(Json1);
    alert(js1);
    $.ajax({
        url: '/Test2/testjson/',
        data: { js: js1 },
        type: "POST",
    }).done(function () {
        alert("done");
    });
}


function findarr(arr, key) {
    for (var z = 0; z < arr.length; z++) {
        if (arr[z]["id"] == key) {
            return arr[z]["values"];
        }
    }
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
