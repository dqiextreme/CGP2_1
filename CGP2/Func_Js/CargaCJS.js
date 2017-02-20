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
        re_ver();
    }
//Modal----------------------------------------//----------------------------------------//---------------------------------
//Genero Json Productos Cargados----------------//----------------------------------------//---------------------------------
    var res = [];
    function Generar_lst() {
        $("#loader img").remove().ready($("#loader").append('<img src="../../Content/Images/Loader.gif" alt="" style="width:10%;height:10%;" />'));
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
                $("#Prod_Tbody > #" + ele.ProductoID).find('td').eq(0).replaceWith("<td><a class=\"glyphicon glyphicon-remove\" onclick=\"Remove('" + ele.ProductoID + "')\"></a></td>");
            });

            if (data.Observaciones !== null) {
                $("#Tbl_Result").replaceWith("<h4 id=\"Tbl_Result\" style=\"text-align:center; width:100%;\">" + data.Observaciones + "</h4>");
            }
        });
        Vali2();
    }
    //validacion nueva
    function Vali2() {
        var Prod_Carg = $("#Prod_Tbody tr").each(function () { return this; });

        var fin = $.grep(Prod_Carg, function (ele, ind) {
            if ($(ele).find('td').eq(9).text().trim().length > 6) { return this; }
        });

        $(fin).each(function (ind, ele) { alert($(ele).find('td').eq(1).text().trim() + " - " + $(ele).find('td').eq(9).text().trim()); });
        $("#loader img").remove();
    }
//Genero Json Productos Cargados----------------//----------------------------------------//---------------------------------

//Elimino Productos Cargados--------------------//----------------------------------------//---------------------------------
    function Remove(prod) {
        $("#Prod_Tbody > #" + prod).remove();
    }
//Elimino Productos Cargados--------------------//----------------------------------------//---------------------------------
//Edito Productos Cargados----------------------//----------------------------------------//---------------------------------
    function Edit(prod) {
        //$("#Prod_Tbody > #" + prod).remove();
        var Cajas = $("#Prod_Tbody > #" + prod).find('td').eq(4).text().trim();
        var Unidad = $("#Prod_Tbody > #" + prod).find('td').eq(5).text().trim();
        $("#Prod_Tbody > #" + prod).find('td').eq(4).replaceWith("<td><input id='" + prod + "_Caj' class=\"col-sm-12 pad\" value=\"" + Cajas + "\")/></td>");
        $("#Prod_Tbody > #" + prod).find('td').eq(5).replaceWith("<td><input id='" + prod + "_Uni' class=\"col-sm-12 pad\" value=\"" + Unidad + "\")/></td>");
        //'<td><input class="col-sm-12 pad" id="' + result[i]["res"][0]["cproducto"] + '_unidad' + '")" /></td>'
    }

    function Ok(prod) {
        //$("#Prod_Tbody > #" + prod).remove();
        var Cajas = $("#" + prod + "_Caj").val();
        var Unidad = $("#" + prod + "_Uni").val();
        
        $("#Prod_Tbody > #" + prod).find('td').eq(4).replaceWith("<td>" + Cajas + "</td>");
        $("#Prod_Tbody > #" + prod).find('td').eq(5).replaceWith("<td>" + Unidad + "</td>");
        //'<td><input class="col-sm-12 pad" id="' + result[i]["res"][0]["cproducto"] + '_unidad' + '")" /></td>'
    }

//Edito Productos Cargados----------------------//----------------------------------------//---------------------------------

    
    

