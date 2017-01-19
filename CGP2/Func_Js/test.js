/*
//------
elimino un row especifico de una tabla
'tbl_body' => id del tr en el tbody
onclick="javascript:document.getElementById('tbl_body').deleteRow(this);"
//------
habilitar el scroll
style="max-height: 240px ;overflow-y: scroll; display: none;"
//------
recargo el area
onclick="javascript:location.reload();"
//------
deshabilito pegar texto
onpaste="return false"
//------
al soltar la tecla lo pasa a mayus.
onkeyup="javascript:this.value=this.value.toUpperCase();"
//------
solo numeros
boton => onkeypress="return onlyNumbers(event)"
funcion => 
            function onlyNumbers(e){
            var result; var keynum; var keychar;
            var numcheck = /\d/;
            keynum = (window.event) ? e.keyCode : e.which;
            //alert(keynum);
            keychar = String.fromCharCode(keynum);
            //if (keynum == 13 || keynum == 8 || keynum == 0 || keynum == 44 || keynum == 46) {result = true;}
            if (keynum == 13 || keynum == 8 || keynum == 0) {result = true;}
            else{result =  numcheck.test(keychar);}
            if (!result){alert("Ingrese solamente numeros");}
            return result;
            }
*/



//var res = [];
function t2()
{ //$("table tbody").find('tr').remove('010114080');  
    $("table tbody").find('tr').each(function (i) {
        var $tds = $(this).find('td');
        var res2 = { 
            "cproducto" : $tds.eq(0).text().trim(),
            "ccompany" : $tds.eq(1).text().trim(),
            "cvendedor" : $tds.eq(2).text().trim(),
            "cproducto" : $tds.eq(3).text().trim(),
            "cnamefc" : $tds.eq(4).text().trim(),
            "ccostobruto_u1" : $tds.eq(5).text().trim(),
            "cprecioventamax" : $tds.eq(6).text().trim(),
            "c_nomb_comer" : $("#"+$tds.eq(0).text().trim()+"_c_nomb_comer").val()
        };
        res.push(res2);
    });
    //var a = $("#here").val();
    //alert(a);
    alert(JSON.stringify(res));
}

var res = [];
function t1(a,b,c,d,e,f,g,h)
{
    for (var i = 0; i < 1500; i++) {
        var res2 = { 
            "ccompany" : a,
            "cvendedor" : b,
            "cproducto" : c,
            "cnamefc" : d,
            "ccostobruto_u1" : e,
            "cprecioventamax" : f,
            "c_nomb_comer" : g,
            "cname" : i.toString()
        };
        res.push(res2);
    }
        

    //$.each(res, function(i, item) {
    //    alert(item.cproducto);
    //});

    //$.each(res, function(i, item){
    //    $.each(item, function(b, item2){
    //        alert(item2);
    //    });
    //});
    retrieve2();
}


function ret(a,b,c,d,e,f,g,h)
{
    for (var i = 0; i < 1050; i++) {
        var res2 = { 
            "cproducto" : c,
            "ccostobruto_u1" : "5"
        };
        res.push(res2);
    }
    retrieve2();
}
function retrieve()
{
    var js1 = JSON.stringify(res);
    alert(js1);
    $.ajax({
        url: '/Test2/testjson/',
        data:  JSON.stringify(res),
        contentType: 'application/json; charset=utf-8;',
        type: "GET",
    }).done(function (){ alert("done");
    });
}
    

function retrieve2()
{
    var js1 = JSON.stringify(res);
    alert(js1);
    $.ajax({
        url: '/Test2/testjson/',
        data:  { js: js1 },
        type: "POST",
    }).done(function (){ alert("done");
    });
}

//verifica duplicados en una tabla y elimina solo los duplicados
function Ver() {
    //$(":input[id^=field]").length
    alert($("#linea2").find('tr').length);

    var a1 = [];
    $("#linea2").find('tr').each(function (a) {
        var a2 = $(this).find('td').eq(0).text();
        if ($.inArray(a2, a1) == -1) {
            a1.push(a2);
        } else {
            $(this).remove();
        }
    });
}
  
//-------------------

function findarr(arr, key){
    for (var z = 0; z < arr.length; z++) { 
        if (arr[z]["id"] == key) {
            return arr[z]["values"];
        }
    }
}

function pag(pos) {
    var sk = ((pos - 1 ) * 10); 
    var ta = sk + 10;

    var result = @Html.Raw(JResult);
    var prin = "";
    for (var i = sk; i < ta; i++) {
        prin += '<tr>' +
        '<td><button type="button" class="btn btn-info btn-sm" onclick="add3(\'' + 
        result[i].cproducto + '\', \'' +
        result[i].ccompany + '\', \'' +
        result[i].cvendedor + '\', \'' +
        result[i].cproducto + '\', \'' +
        result[i].cnamefc + '\', \'' +
        result[i].ccostobruto_u1 + '\', \'' +
        result[i].cprecioventamax + '\', \'' +
        result[i].c_nomb_comer + '\', \'' +
        result[i].cname + '\')" >' +
        result[i].cproducto + '</button></td>' +
                    '<td>' + result[i].ccompany + '</td>' +
                    '<td>' + result[i].cvendedor + '</td>' +
                    '<td>' + result[i].cproducto + '</td>' +
                    '<td>' + result[i].cnamefc + '</td>' +
                    '<td>' + result[i].ccostobruto_u1 + '</td>' +
                    '<td>' + result[i].cprecioventamax + '</td>' +
                    '<td>' + result[i].c_nomb_comer + '</td>' +
                    '<td>' + result[i].cname + '</td></tr>';
    }
    $("#fuck_here tr").remove().ready($("#fuck_here").append(prin));
} 
    
function add3(ccompany,cvendedor,cproducto,cnamefc,ccostobruto_u1,cprecioventamax,c_nomb_comer,cname) {
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