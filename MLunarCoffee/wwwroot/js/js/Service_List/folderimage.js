function Treatment_Plant_Create_Folder() {
    $("#DetailModal_Content").html('');
    $("#DetailModal_Content").load("/Customer/ImageNewFolder?CustomerID=" + ser_MainCustomerID + "&TreatmentPlanID=" + ser_current_treatment_plant_id + "&ver=" + versionofWebApplication);
    $('#DetailModal').modal('show');
}

function Tab_List_Load_Folder() {
    if (ser_current_treatment_plant_id != 0) {
        $('#Treatment_Plan_Folder').show();
        AjaxLoad(url = "/Customer/Service/TabList/TabList_Service/?handler=LoadImgTreatPlan"
            , data = {
                'CustomerID': ser_MainCustomerID,
                'TreatPlanID': ser_current_treatment_plant_id }
            , async = true
            , error = function () { notiError_SW() }
            , success = function (result) {
                let dtFolder = JSON.parse(result);
                Render_Folder_TreatPlan(dtFolder, 'FolderTreatmentPlan');
                Event_Click_Folder_Cust_TreatPlan();
                TriggerClick_Folder();
            }
        );
    }
}
 function Render_Folder_TreatPlan(dtFolder, id) {
        var myNode = document.getElementById(id);
        if (myNode != null) {
            myNode.innerHTML = '';
            let stringContent = '';
             
            if (dtFolder && dtFolder.length > 0) {
                for (var i = 0; i < dtFolder.length; i++) {
                    let item = dtFolder[i]
                    stringContent = stringContent
                        + '<li class="nav-item"><a class="text-sm px-2 item ' + (i == 0 ? "active" : "") + ' text-sm p-1 ms-2" data_id="' + item.ID + '" data-foldername="' + item.FolderName + '">' + item.FolderName + '</a></li>'
                }

                //let headdiv = '<div class="ui horizontal secondary pointing fluid tabular menu" style="border-bottom: 2px solid transparent;">';
                //let contentdiv = '';
                //let contentFolder = '';
                //for (var i = 0; i < dtFolder.length; i++) {

                //    let item = dtFolder[i]
                 //contentdiv += '<a class=" item ' + (i == 0 ? "active" : "") + '" data_id="' + item.ID + '" data-foldername="' + item.FolderName + '">' + item.FolderName + '</a>'
                //}
                //let fooddiv = '</div>';
                //stringContent = headdiv + contentdiv + fooddiv + contentFolder;

                $("#ImgTreatmentPlan").show();
            } 
            $("#FolderTreatmentPlan").show();

            document.getElementById(id).innerHTML = stringContent;
        }
}
function TriggerClick_Folder() {
    if ($('#FolderTreatmentPlan a.item').get(0) != undefined)
        $('#FolderTreatmentPlan a.item').get(0).click();
}