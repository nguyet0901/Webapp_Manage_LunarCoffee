function ResetAllProperties_PrintDesign() {
    SetEmptyAllProperties_PrintDesign();
    DisableAllProperties_PrintDesign();
    ShowHideButtonExecute_PrintDesign();
}
function ShowHideButtonExecute_PrintDesign() {
    $('#btnDeleteAttribute').hide();
    $('#btnResetAttribute').hide();
    $('#btnSaveAttribute').hide();

}
function DisableAllProperties_PrintDesign() {
    var paras = document.getElementsByClassName('input_setting_attribute');
    for (i = 0; i < paras.length; i++) {
        paras[i].disabled = true;
    }
}
function SetEmptyAllProperties_PrintDesign() {
    var paras = document.getElementsByClassName('input_setting_attribute');
    for (i = 0; i < paras.length; i++) {
        paras[i].value = "";
    }
}
function Enable_Row_Properties_PrintDesign() {
    $("#s_margin").prop('disabled', false);
    $("#s_padding").prop('disabled', false);
    $("#s_classname").prop('disabled', false);
}
function getRandomInt(max) {
    min = 1;
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
function ResetAttribute_Row() {
    ResetAllProperties_PrintDesign();
    Enable_Row_Properties_PrintDesign();
    for (var i = 0; i < data_main_layout.length; i++) {
        if (data_main_layout[i].RowRandom == current_row_selected) {
            $("#s_margin").val(data_main_layout[i].Margin);
            $("#s_padding").val(data_main_layout[i].Padding);
            $("#s_classname").val(data_main_layout[i].ClassName);
            if (data_main_layout[i].IsTable == 1) {
                $("#s_border").prop('disabled', false);
                $("#s_bordertop").prop('disabled', false);
                $("#s_borderleft").prop('disabled', false);
                $("#s_borderbottom").prop('disabled', false);
                $("#s_borderright").prop('disabled', false);
                $("#s_border").val(data_main_layout[i].Border);
                $("#s_bordertop").val(data_main_layout[i].BorderTop);
                $("#s_borderleft").val(data_main_layout[i].BorderLeft);
                $("#s_borderbottom").val(data_main_layout[i].BorderBottom);
                $("#s_borderright").val(data_main_layout[i].BorderRight);
            }
        }
    }
    document.getElementById("type_main_element").innerHTML = "ROW";
}
function SaveAttribute_Row() {
    let margin = $("#s_margin").val();
    let padding = $("#s_padding").val();
    let border = $("#s_border").val();
    for (var i = 0; i < data_main_layout.length; i++) {
        if (data_main_layout[i].RowRandom == current_row_selected) {
            data_main_layout[i].Margin = margin;
            data_main_layout[i].Padding = padding;
            data_main_layout[i].Border = border;
            data_main_layout[i].BorderTop = $("#s_bordertop").val();
            data_main_layout[i].BorderLeft = $("#s_borderleft").val();
            data_main_layout[i].BorderBottom = $("#s_borderbottom").val();
            data_main_layout[i].BorderRight = $("#s_borderright").val();
            data_main_layout[i].ClassName = $("#s_classname").val();
        }
    }

}
function DeleteAttribute_Row() {
    for (var i = data_main_layout.length - 1; i >= 0; i--) {
        if (data_main_layout[i].RowRandom == current_row_selected) {
            data_main_layout.splice(i, 1);
        }
    }
    RemoveChosen_ROW_ELEMENT_CELL();
    ResetAllProperties_PrintDesign();

}
function Enable_Cell_Properties_PrintDesign() {
    $("#s_margin").prop('disabled', false);
    $("#s_padding").prop('disabled', false);
    $("#s_width").prop('disabled', false);
    $("#s_style").prop('disabled', false);
    $("#s_border").prop('disabled', false);
    $("#s_bordertop").prop('disabled', false);
    $("#s_borderleft").prop('disabled', false);
    $("#s_borderbottom").prop('disabled', false);
    $("#s_borderright").prop('disabled', false);

}
function ResetAttribute_Cell() {
    ResetAllProperties_PrintDesign();
    Enable_Cell_Properties_PrintDesign();
    for (var i = 0; i < data_main_layout.length; i++) {
        for (k = 0; k < data_main_layout[i].Cells.length; k++) {
            if (data_main_layout[i].Cells[k].Random == current_cell_selected) {
                $("#s_margin").val(data_main_layout[i].Cells[k].Margin);
                $("#s_padding").val(data_main_layout[i].Cells[k].Padding);
                $("#s_width").val(data_main_layout[i].Cells[k].Width);
                $("#s_style").val(data_main_layout[i].Cells[k].Style);
                $("#s_bordertop").val(data_main_layout[i].Cells[k].BorderTop);
                $("#s_borderleft").val(data_main_layout[i].Cells[k].BorderLeft);
                $("#s_borderbottom").val(data_main_layout[i].Cells[k].BorderBottom);
                $("#s_borderright").val(data_main_layout[i].Cells[k].BorderRight);

            }
        }
    }
    document.getElementById("type_main_element").innerHTML = "CELL";
}
function SaveAttribute_Cell() {
    let width = $("#s_width").val();
    let style = $("#s_style").val();
    let margin = $("#s_margin").val();
    let padding = $("#s_padding").val();
    for (var i = 0; i < data_main_layout.length; i++) {
        for (k = 0; k < data_main_layout[i].Cells.length; k++) {
            if (data_main_layout[i].Cells[k].Random == current_cell_selected) {
                data_main_layout[i].Cells[k].Width = width;
                data_main_layout[i].Cells[k].Style = style;
                data_main_layout[i].Cells[k].Margin = margin;
                data_main_layout[i].Cells[k].Padding = padding;
                data_main_layout[i].Cells[k].BorderTop = $("#s_bordertop").val();
                data_main_layout[i].Cells[k].BorderLeft = $("#s_borderleft").val();
                data_main_layout[i].Cells[k].BorderBottom = $("#s_borderbottom").val();
                data_main_layout[i].Cells[k].BorderRight = $("#s_borderright").val();

            }
        }
    }
}
function Enable_Element_Properties_PrintDesign() {
    $("#s_margin").prop('disabled', false);
    $("#s_font_size").prop('disabled', false);
    $("#s_width").prop('disabled', false);
    $("#s_height").prop('disabled', false);
    $("#s_textalign").prop('disabled', false);
    $("#s_texttransform").prop('disabled', false);
    $("#s_fontweight").prop('disabled', false);
    $("#s_padding").prop('disabled', false);
    $("#s_border").prop('disabled', false);
    $("#s_bordertop").prop('disabled', false);
    $("#s_borderleft").prop('disabled', false);
    $("#s_borderbottom").prop('disabled', false);
    $("#s_borderright").prop('disabled', false);
    $("#s_display").prop('disabled', false);
    $("#s_verticalalign").prop('disabled', false);
    $("#s_fontweight").prop('disabled', false);
    $("#s_colspan").prop('disabled', false);
    $("#s_formcontainer").prop('disabled', false);
    $("#s_text").prop('disabled', false);
    $("#s_font_style").prop('disabled', false);
    

    //$("#s_width").prop('disabled', true);
    //$("#s_height").prop('disabled', true);
    //$("#s_textalign").prop('disabled', true);
    //$("#s_font_size").prop('disabled', true);
    //$("#s_texttransform").prop('disabled', true);
    //$("#s_fontweight").prop('disabled', true);
    //$("#s_margin").prop('disabled', true);
    //$("#s_padding").prop('disabled', true);
    //$("#s_border").prop('disabled', true);
    //$("#s_display").prop('disabled', true);
    //$("#s_verticalalign").prop('disabled', true);
    //$("#s_colspan").prop('disabled', true);
    //$("#s_style").prop('disabled', true);



}
function ResetAttribute_Element() {
    ResetAllProperties_PrintDesign();
    Enable_Element_Properties_PrintDesign();
    let type;
    let IsTable;
    let IsText;
    for (var i = 0; i < data_main_layout.length; i++) {
        for (k = 0; k < data_main_layout[i].Cells.length; k++) {
            for (z = 0; z < data_main_layout[i].Cells[k].Attribute.length; z++) {
                if (data_main_layout[i].Cells[k].Attribute[z].AttributeRandom == current_element_selected) {
                    type = data_main_layout[i].Cells[k].Attribute[z].Type;
                    
                    IsTable = data_main_layout[i].Cells[k].IsTable;
                    IsText = data_main_layout[i].Cells[k].Attribute[z].IsText;
                    $("#s_margin").val(data_main_layout[i].Cells[k].Attribute[z].Margin);
                    $("#s_font_size").val(data_main_layout[i].Cells[k].Attribute[z].FontSize);
                    $("#s_font_style").val(data_main_layout[i].Cells[k].Attribute[z].FontStyle);
                    $("#s_texttransform").val(data_main_layout[i].Cells[k].Attribute[z].TextTransform);
                    $("#s_height").val(data_main_layout[i].Cells[k].Attribute[z].Height);
                    $("#s_width").val(data_main_layout[i].Cells[k].Attribute[z].Width);
                    $("#s_textalign").val(data_main_layout[i].Cells[k].Attribute[z].TextAlign);
                    $("#s_border").val(data_main_layout[i].Cells[k].Attribute[z].Border);
                    $("#s_bordertop").val(data_main_layout[i].Cells[k].Attribute[z].BorderTop);
                    $("#s_borderleft").val(data_main_layout[i].Cells[k].Attribute[z].BorderLeft);
                    $("#s_borderbottom").val(data_main_layout[i].Cells[k].Attribute[z].BorderBottom);
                    $("#s_borderright").val(data_main_layout[i].Cells[k].Attribute[z].BorderRight);
                    $("#s_padding").val(data_main_layout[i].Cells[k].Attribute[z].Padding);
                    $("#s_display").val(data_main_layout[i].Cells[k].Attribute[z].Display);
                    $("#s_verticalalign").val(data_main_layout[i].Cells[k].Attribute[z].VerticalAlign);
                    $("#s_fontweight").val(data_main_layout[i].Cells[k].Attribute[z].FontWeight);
                    $("#s_formcontainer").val(data_main_layout[i].Cells[k].Attribute[z].FormContainer);
                    $("#s_text").val(data_main_layout[i].Cells[k].Attribute[z].Text);

                    if (Number(IsTable) == 0) {
                        $("#s_colspan").prop('disabled', true);
                        $("#s_colspan").val('');
                    }
                    else {
                        $("#s_colspan").prop('disabled', false);
                        $("#s_colspan").val(data_main_layout[i].Cells[k].Attribute[z].Colspan);
                    }
                    if (Number(IsTable) == 1) {
                        $("#s_text").prop('disabled', false);
                    }
                    else if (Number(IsText) == 0) {
                        $("#s_text").prop('disabled', true);
                    }
                    else {
                        $("#s_text").prop('disabled', false);
                    }
                }
            }
        }
    }
    document.getElementById("type_main_element").innerHTML = type;
}
function Enable_Main_Properties_PrintDesign() {
    $("#s_padding").prop('disabled', false);
    $("#s_style").prop('disabled', false);
}
function ResetAttribute_MAIN() {
    ResetAllProperties_PrintDesign();
    Enable_Main_Properties_PrintDesign();
    $("#s_padding").val(data_main_padding);
    $("#s_style").val(data_main_style);
    document.getElementById("type_main_element").innerHTML = "FORM";
}
function SaveAttribute_MAIN() {

    data_main_padding = $("#s_padding").val();
    data_main_style = $("#s_style").val();
}
function SaveAttribute_Element() {

    for (var i = 0; i < data_main_layout.length; i++) {
        for (k = 0; k < data_main_layout[i].Cells.length; k++) {
            for (z = 0; z < data_main_layout[i].Cells[k].Attribute.length; z++) {
                if (data_main_layout[i].Cells[k].Attribute[z].AttributeRandom == current_element_selected) {
                    data_main_layout[i].Cells[k].Attribute[z].Margin = $("#s_margin").val();
                    data_main_layout[i].Cells[k].Attribute[z].FontSize = $("#s_font_size").val();
                    data_main_layout[i].Cells[k].Attribute[z].FontStyle = $("#s_font_style").val();
                    data_main_layout[i].Cells[k].Attribute[z].TextTransform = $("#s_texttransform").val();
                    data_main_layout[i].Cells[k].Attribute[z].Height = $("#s_height").val();
                    data_main_layout[i].Cells[k].Attribute[z].Width = $("#s_width").val();
                    data_main_layout[i].Cells[k].Attribute[z].TextAlign = $("#s_textalign").val();
                    data_main_layout[i].Cells[k].Attribute[z].Border = $("#s_border").val();
                    data_main_layout[i].Cells[k].Attribute[z].BorderTop = $("#s_bordertop").val();
                    data_main_layout[i].Cells[k].Attribute[z].BorderLeft = $("#s_borderleft").val();
                    data_main_layout[i].Cells[k].Attribute[z].BorderBottom = $("#s_borderbottom").val();
                    data_main_layout[i].Cells[k].Attribute[z].BorderRight = $("#s_borderright").val();
                    data_main_layout[i].Cells[k].Attribute[z].Padding = $("#s_padding").val();
                    data_main_layout[i].Cells[k].Attribute[z].Display = $("#s_display").val();
                    data_main_layout[i].Cells[k].Attribute[z].VerticalAlign = $("#s_verticalalign").val();
                    data_main_layout[i].Cells[k].Attribute[z].FontWeight = $("#s_fontweight").val();
                    data_main_layout[i].Cells[k].Attribute[z].Colspan = $("#s_colspan").val();
                    data_main_layout[i].Cells[k].Attribute[z].FormContainer = $("#s_formcontainer").val();
                    data_main_layout[i].Cells[k].Attribute[z].Text = $("#s_text").val();
                }
            }


        }

    }
}
function DeleleAttribute_Element() {
    for (var i = 0; i < data_main_layout.length; i++) {
        for (k = 0; k < data_main_layout[i].Cells.length; k++) {
            for (z = data_main_layout[i].Cells[k].Attribute.length - 1; z >= 0; z--) {

                if (data_main_layout[i].Cells[k].Attribute[z].AttributeRandom == current_element_selected) {
                    data_main_layout[i].Cells[k].Attribute.splice(z, 1);
                }
            }
        }
    }
    RemoveChosen_ROW_ELEMENT_CELL();
    ResetAllProperties_PrintDesign();
}
//#region SELECTE COLOR 
function RemoveChosen_ROW_ELEMENT_CELL() {

    let x = document.getElementsByClassName("mainrow");
    if (x != undefined) {
        for (i = 0; i < x.length; i++) {
            if (x[i].className.includes("boderclick")) {
                x[i].className = x[i].className.replace("boderclick", "");
            }
        }
    }
    x = document.getElementsByClassName("div_row_main");
    if (x != undefined) {
        for (i = 0; i < x.length; i++) {
            if (x[i].className.includes("boderclick")) {
                x[i].className = x[i].className.replace("boderclick", "");
            }
        }
    }
    x = document.getElementsByClassName("attribute_in_cell");
    if (x != undefined) {
        for (i = 0; i < x.length; i++) {
            if (x[i].className.includes("boderclick")) {
                x[i].className = x[i].className.replace("boderclick", "");
            }
        }
    }
    if ($('#divMainPrintDesign').hasClass("boderclick")) {
        $('#divMainPrintDesign').removeClass("boderclick");
    }

}
//#endregion
//#region EXECUTE ARRAY 
function Change_Index_OF_Main_Layout() {
    var new_lay = [];
    var datarow = document.getElementsByClassName('mainrow');
    for (i = 0; i < datarow.length; i++) {
        let ran = datarow[i].dataset.random;
        for (k = 0; k < data_main_layout.length; k++) {
            if (data_main_layout[k].RowRandom == ran) {
                let element = JSON.parse(JSON.stringify(data_main_layout[k]));
                new_lay.push(element);
            }
        }
    }
    data_main_layout = JSON.parse(JSON.stringify(new_lay));
    Add_State_Array_Design();
    Draw_Design_Row_Main("divMainPrintDesign");
}
function Add_State_Array_Design() {
    for (var i = data_main_array.length - 1; i >= 0; i--) {
        if (data_main_array[i].State == 0) {
            data_main_array.splice(i, 1);
        }
    }
    let ele = {};
    ele.State = 1;
    ele.Data = JSON.parse(JSON.stringify(data_main_layout));
    data_main_array.push(ele);
    data_main_array_index = data_main_array.length - 1;
    Show_Hide_Next_Back_State();
}
function Next_State_Array_Design() {
    if (data_main_array.length > data_main_array_index + 1) {
        data_main_array_index = data_main_array_index + 1;
        data_main_array[data_main_array_index].State = 1;
        data_main_layout = JSON.parse(JSON.stringify(data_main_array[data_main_array_index].Data));

        Draw_Design_Row_Main("divMainPrintDesign");
    }
    Show_Hide_Next_Back_State();
}
function Back_State_Array_Design() {
    if (data_main_array_index > 0) {
        data_main_array[data_main_array_index].State = 0;
        data_main_array_index = data_main_array_index - 1;
        data_main_layout = JSON.parse(JSON.stringify(data_main_array[data_main_array_index].Data));
        Draw_Design_Row_Main("divMainPrintDesign");
    }
    Show_Hide_Next_Back_State();
}
function Show_Hide_Next_Back_State() {
    if (data_main_array_index == 0) {
        $("#back_state_ex").prop("disabled", true);
    }
    else {
        $("#back_state_ex").prop("disabled", false);
    }
    if (data_main_array_index == data_main_array.length - 1) {
        $("#next_state_ex").prop("disabled", true);
    }
    else {
        $("#next_state_ex").prop("disabled", false);
    }



}
//#endregion
