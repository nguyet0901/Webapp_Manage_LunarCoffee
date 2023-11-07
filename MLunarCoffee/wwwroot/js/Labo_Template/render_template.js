

function Render_Template_Labo(data, id) {
    var myNode = document.getElementById(id);
    if (myNode != null) {
        myNode.innerHTML = '';
        let stringContent = '';
        if (data && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let listQuiz = item.Quiz;
                let tr =  '';
 
                for (var k = 0; k < listQuiz.length; k++) {
                    itemQuiz = listQuiz[k];
                   // tr = '<span class="w-100 text-dark text-sm font-weight-bold">' + itemQuiz.title + '</span>';
                   
                     tr = tr + Render_Labo_Questions(itemQuiz);
                }
  
                stringContent = stringContent + tr;
            }
        }
        document.getElementById(id).innerHTML = stringContent;
    }

}
function Render_Labo_Questions(item) {

    let tr =
        '<div class="row row-cols-1 row-cols-sm-2 row-cols-md-4 row-cols-lg-5 text-sm text-dark  px-3 py-2">'
        + '<span class="w-100">' + item.title + '</span>';
    let dataListItems = item.listitems;
    if (dataListItems && dataListItems.length > 0) {
        for (var k = 0; k < dataListItems.length; k++) {
            let item = dataListItems[k];
            tr = tr + Render_Item_Check_List(item);
        }
    }
    tr = tr + '</div>';
    return tr;
 

}
function Render_Item_Check_List(item) {
    let tr = "";
    switch (item.type) {
        case "checkbox":
            tr = '<div class="form-check">'
                + '<input class="checkQA form-check-input pr-2" value="small" type="checkbox" id="' + item.id + '" data-id="' + item.id + '">'
                + '<label class="text-dark text-sm font-weight-normal">' + item.label + '</label>'
                + '</div>';
            break;
        case "checkboxOther":
            tr = '<div class="input-group mb-3">'
                + '<div class="input-group-text">'
                + '<div class="form-check pt-1">'
                + '<input class="form-check-input mt-0 checkQA" value="small" type="checkbox" id="' + item.id + '" data-id="' + item.id + '" >'
                + '</div>'
                + '</div>'
                + '<input type="text" class="form-control" placeholder=eg .other answer" id="other' + item.id + '" onchange="getValueCheckboxOther(' + item.id + ')">'

                + '</div>';
            break;


        case "checkbox_fillnumber": //checkboxfillnumber
            tr = '<label class="text-dark text-sm font-weight-normal px-0">' + item.label + '</label>'
                +'<div class="input-group mb-3">'
                + '<div class="input-group-text">'
                + '<div class="form-check pt-1">'
                + '<input class="form-check-input mt-0 checkQA" value="small" type="checkbox" id="' + item.id + '" data-id="' + item.id + '" >'
                + '</div>'
                + '</div>'
                + '<input type="text" class="form-control" placeholder=eg .other answer" id="other' + item.id + '" onchange="getValueCheckboxOther(' + item.id + ')">'
                + item.unit
                + '</div>';

 
            break;
 
        case "header":
            tr = '<div class="d-block"><h6 class="text-dark text-sm font-weight-normal px-0">' + item.text + '</h6></div>'
            break;
        case "text":
            tr = '<label class="text-dark text-sm font-weight-normal px-0">' + item.text + '</label>'
            break;

        case "checkbox_img":
            tr = tr + '<div class="column" style="position: relative;padding:10px 10px 30px 10px"><img src="/assests/img/pontic/' + item.link + '" class="ui image">'
                + '<label>' + item.title + '</label><br/>'
                + '<div class="ui checkbox" style="position: absolute;bottom: 0;left: 50%;transform: translateX(-50%)"><input type="checkbox" name="small" id="' + item.id + '" class="checkQA" data-id="' + item.id + '" />'
                + '<label></label>'
                + '</div></div>'
            break;

        case "lable_note":
            tr = '<label class="text-dark text-sm font-weight-normal px-0">' + item.label + '</label>'
                + '<input class="form-control" id="' + item.id + '" onchange="getAnswerDataType_Note(' + item.id + ')"   />'
            break;
        case "textarea":
            tr =  '<textarea class="form-control" id="' + item.id + '" onchange="getAnswerDataType_Note(' + item.id + ')" name="content"></textarea>'
            break;

        default: break;
    }
    return tr;
}

function EventLinkClick_CheckBoxList(data) {
    $(".checkQA").on('click', function (event) {
        var id = $(this).attr("data-id");
        if (this.checked == false) {
            for (let i = data.length - 1; i >= 0; i--) {
                if (data[i].id == id) {
                    data.splice(i, 1);
                    break;
                }
            }
        }
        else {
            var myEle = document.getElementById("other" + id);
            if (myEle) {
                var answer = $('#other' + id).val() ? $('#other' + id).val() : "";
                if (answer != "") {
                    let itemCheckedOther = { type: "checkother", id: id, text: answer };
                    data.push(itemCheckedOther);
                }
            }
            else {
                let itemChecked = { type: "checkbox", id: id };
                data.push(itemChecked);
            }
        }
    });
}