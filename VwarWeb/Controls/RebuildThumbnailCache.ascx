<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RebuildThumbnailCache.ascx.cs" Inherits="Controls_RebuildThumbnailCache" %>

 <style type="text/css">
        .thead
        {
            color:White;
            background-color:#507CD1;
            font-weight:bold;
        }
        tr:nth-child(even) 
        {
            background-color:#EFF3FB;
            width:20%;
        }
        tr:nth-child(odd) 
        {
            background-color:#FFFFFF;
            width:20%;
        }
        .thumbpanel
        {
            font-size: .8em;
            font-family: Verdana, Helvetica, Arial;
            width: 650px;
        }
        .Thumbtable
        {
            border-right: outset 1px blue;
            border-left: outset 1px blue;
            border-top: outset 1px blue;
            border-bottom: outset 1px blue;
            width:100%;
        }
        .pid
        {
            border-right: outset 1px blue;
            border-left: outset 1px blue;
            border-top: outset 1px blue;
            border-bottom: outset 1px blue;
            width:10%;
            text-align:center;
        }
        .Scrollpane
        {
            max-height : 500px;
            overflow : scroll;
        }
    </style>

<script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
<div  class='thumbpanel'>
    <div id="BeginRebuild">Rebuild Thumbnails</div><div id="status">Idle...</div>
    <div id="scrollpane" class="Scrollpane">
            <table class="Thumbtable">
                <tbody id="Log">
                    <tr>
                        <td>PID</td>
                        <td>Status</td>
                    </tr>
                </tbody>
            </table>
    </div>
</div>
<script type="text/javascript">

    function goToByScroll(id) {
        $('#scrollpane').scrollTop($(document.getElementById(id)).position().top + $('#scrollpane').scrollTop() - $('#scrollpane').height());
        
    }


    $(document).ready(function () {
        CreateWidgets();
    });
    var ServiceURL = '<%= Page.ResolveClientUrl("~/Administrators/RebuildThumbnailCache.asmx") %>';
    function CreateWidgets() {

        $("#BeginRebuild").button();
        $("#BeginRebuild").click(function () {

            $("#status").html("Retrieving list");
            
            $.ajax({
                url: ServiceURL + "/GetAllPIDS",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                async: false,
                success: function (res, status, jqXHR) {
                    //alert(JSON.stringify(res));
                    $("#status").html("Processing");
                    var pids = res.d;
                    for (var i = 0; i < pids.length; i++) {

                        var newentry = "<tr><td class='pid'>" + pids[i] + "</td><td class='Thumbtable' id='" + pids[i] + "'>" + "_" + "</td></tr>";
                        $("#Log").html($("#Log").html() + newentry);
                    }
                    for (var i = 0; i < pids.length; i++) {

                        $.ajax({
                            url: ServiceURL + "/UpdateThumbnailCache",
                            type: "POST",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "pid": pids[i] }),
                            success: function (res, status, jqXHR) {
                                // alert(res.d);
                                document.getElementById(pids[i]).innerHTML = res.d;
                                if ($("#status").html() == "Processing")
                                    $("#status").html("Processing.");
                                else if ($("#status").html() == "Processing.")
                                    $("#status").html("Processing..");
                                else if ($("#status").html() == "Processing..")
                                    $("#status").html("Processing...");
                                else if ($("#status").html() == "Processing...")
                                    $("#status").html("Processing");

                                goToByScroll(pids[i])
                            }
                        });



                    }
                    $("#status").html("Finished");
                }
            });

        });
    }


</script>
