//Modal----------------------------------------//----------------------------------------//---------------------------------
    var modal1 = 0;
    function Modal_Prod() {
        if (modal1 === 0) {
            $.ajax({
                url: '/CargaC/_Product_Mst/',
                data: { id: "" },
                type: "GET",
            }).done(function (result) { $("#Modal_Prod_Id div").remove().ready($("#Modal_Prod_Id").append(result)); });
        }
        modal1 = 1;
    }
//Modal----------------------------------------//----------------------------------------//---------------------------------
//Genero Json Productos Cargados----------------//----------------------------------------//---------------------------------
    var res = [];
    function Generar_lst() {
        res = [];
        $("#Prod_Tbody").find('tr').each(function (i) {
            var $tds = $(this).find('td');
            var res2 = {
                "cproducto": $tds.eq(1).text().trim(),
                "cnamefc": $tds.eq(2).text().trim(),
                "cprecioventamax": $tds.eq(3).text().trim(),
                "cajas": $tds.eq(4).text().trim(),
                "unidad": $tds.eq(5).text().trim()
            };
            res.push(res2);
        });
        //alert(JSON.stringify(res));
        Vali();
    }
    function Vali() {
        var Json1 = res;
        var js1 = JSON.stringify(Json1);
        //alert(js1);
        $.ajax({
            url: '/CargaC/prodtest2/',
            data: { js: js1 },
            type: "POST",
        }).done(function (data) {
            var prod = data.Productos;
            $(prod).each(function (ind, ele) {
                 $("#Prod_Tbody > #" + ele.ProductoID).find('td').eq(9).replaceWith("<td>" + ele.MensajeError + "</td>");
            });

            if (data.Observaciones !== null) {
                $("#Tbl_Result").replaceWith("<h4 id=\"Tbl_Result\" style=\"text-align:center; width:100%;\">" + data.Observaciones + "</h4>");
            }

        });
    }
//Genero Json Productos Cargados----------------//----------------------------------------//---------------------------------
//Elimino Productos Cargados--------------------//----------------------------------------//---------------------------------
    function Remove(prod) {
        $("#Prod_Tbody > #" + prod).remove();
    }
    function Edit(prod) {
        //$("#Prod_Tbody > #" + prod).remove();
        var Cajas = $("#Prod_Tbody > #" + prod).find('td').eq(4).text().trim();
        var Unidad = $("#Prod_Tbody > #" + prod).find('td').eq(5).text().trim();
        $("#Prod_Tbody > #" + prod).find('td').eq(4).replaceWith("<td><input class=\"col-sm-12 pad\")/></td>");
        $("#Prod_Tbody > #" + prod).find('td').eq(5).replaceWith("<td><input class=\"col-sm-12 pad\" value=\""+ Unidad +"\")/></td>");
        //'<td><input class="col-sm-12 pad" id="' + result[i]["res"][0]["cproducto"] + '_unidad' + '")" /></td>'
    }
//Elimino Productos Cargados--------------------//----------------------------------------//---------------------------------
