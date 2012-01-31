<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RebuildThumbnailCache.ascx.cs" Inherits="Controls_RebuildThumbnailCache" %>
<script type="text/javascript" src="../Scripts/jquery-ui-1.8.7.custom.min.js"></script>
<div id="BeginRebuild">Rebuild Thumbnails</div>
<div>
        <table>
            <tbody id="Log">
                <tr>
                    <td>PID</td>
                    <td>Status</td>
                </tr>
            </tbody>
        </table>
</div>
<script type="text/javascript">

    $(document).ready(function () {
        CreateWidgets();
    });
    var ServiceURL = '<%= Page.ResolveClientUrl("~/Users/RebuildThumbnailCache.asmx") %>';
    function CreateWidgets() {

        $("#BeginRebuild").button();
        $("#BeginRebuild").click(function () {

            $.ajax({
                url: ServiceURL + "/GetAllPIDS",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: null,
                async: false,
                success: function (res, status, jqXHR) {
                    //alert(JSON.stringify(res));
                    var pids = res.d;
                    for (var i = 0; i < pids.length; i++) {

                        var newentry = "<tr><td>" + pids[i] + "</td><td id='" + pids[i] + "'>" + "_" + "</td></tr>";
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
                            }
                        });



                    }
                }
            });

        });
    }


</script>
