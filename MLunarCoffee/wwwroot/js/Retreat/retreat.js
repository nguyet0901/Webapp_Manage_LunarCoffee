function App_Retreat_Render_Data_Radio(_min_initi, _min, _max, _key) {
    let result = '';
    let _index = 0;
    while (_min <= _max) {
        let dis = _index * _distance_multi;

        let is_checked = (_min == _min_initi) ? 'checked="checked"' : '';
        //let is_disabled = (_min < _min_initi) ? 'disabled' : '';

        //or  set theo date_now
        let date_now = ConvertDT_To_ThousandNumber(new Date);
        let is_disabled = (_min < date_now) ? 'disabled' : '';

        let tr =
            '<span class="_text_radio ' + is_disabled +'" style="left:' + dis + 'px">' + ConvertThousandNumber_To_MY(_min) + '</span>'
            + '<div class="ui radio checkbox _check_radio" style="left:' + (dis + 9) + 'px">'
            + '<input type="radio" class="radio_choose" name="radio" ' + is_checked
            + ' data-value=' + _min
            + ' data-id=' + _key
            + ' data-default=' + _min_initi + ' ' + is_disabled +'>'
            + '<label class="coloring red"></label>'
            + '</div>';

        result = result + tr;
        _min = _min + 1;
        _index++;
    }
    return result;
}
function App_Retreat_Render_Data_TimeLine_Current(data, _min, _min_initi, _max_initi) {

    let dis = (_min_initi - _min) * _distance_multi;
    let _w = (_max_initi - _min_initi) * _distance_multi + 13;
    let contentHtml = '<div class="retreat_appointment_current_line" style="position:absolute;width:' + _w + 'px;left:' + (dis + 9) + 'px"></div>';
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let IsDisable = Number(data[i].IsDisable)

        let thousand_datefrom = ConvertDT_To_ThousandNumber(item.Date_From)
        let dis = (thousand_datefrom - _min) * _distance_multi;
        let tr = '';
        if (IsDisable == 1) {
            tr = '<div class="retreat_appointment_sticky_disable" style="position:absolute;left:' + (dis + 9) + 'px">'
                + '</div>';
        }
        else {
            tr = '<div class="retreat_appointment_sticky" style="position:absolute;left:' + (dis + 9) + 'px">'
                + '</div>';
        }
        contentHtml = contentHtml + tr;
    }
    return '<div class="retreat_appointment_sticky_container">'
        + contentHtml
        + '</div>'

}
function App_Retreat_Render_Data_TimeLine(data, _min_initi, _min, key) {
    let contentHtml = '';
    let data_active = data.filter(word => Number(word["IsDisable"]) == 0);
    if (data_active != undefined) {
        for (var i = 0; i < data_active.length; i++) {

            let item = data_active[i];
            let thousand_datefrom = ConvertDT_To_ThousandNumber(item.Date_From)
            if (i == 0) _min_initi = thousand_datefrom;
            let dis = (thousand_datefrom - _min) * _distance_multi;
            let tr = '<div class="retreat_appointment" style="position:absolute;left:' + (dis + 9) + 'px">'
                + '</div>';
            contentHtml = contentHtml + tr;
        }
        let _w = ConvertDT_To_ThousandNumber(data_active[data_active.length - 1].Date_From) - ConvertDT_To_ThousandNumber(data_active[0].Date_From);
        let _l = (_min_initi - _min) * _distance_multi;
        return '<div id="retreat_appointment_container_' + key + '" class="retreat_appointment_container" data-value=0>'
            + contentHtml
            + '<div id="retreat_appointment_new_line_' + key + '" class="retreat_appointment_new_line" '
            + 'style="width:' + (_w * _distance_multi) + 'px;left:' + (_l + 13) + 'px;"></div>'
            + '</div>'
    }
}
function App_Retreat_Render_Data(data, id, idMaster) {
    var myNode = document.getElementById(idMaster);
    if (myNode != null) {
        let contentHtml = '';

        if (data && data.length > 0) {
            let _min_initi = ConvertDT_To_ThousandNumber(data[0].Date_From)
            let _max_initi = ConvertDT_To_ThousandNumber(data[data.length - 1].Date_From)
            let _min = _min_initi - 3;
            let _max = _max_initi + 16
            let data_active = data.filter(word => Number(word["IsDisable"]) == 0);
            if (data_active != undefined && data_active.length != 0) {
                let _min_initi_active = ConvertDT_To_ThousandNumber(data_active[0].Date_From)
                contentHtml = App_Retreat_Render_Data_Radio(_min_initi_active, _min, _max, id);
                contentHtml = contentHtml + App_Retreat_Render_Data_TimeLine(data_active, _min_initi, _min, id);

                contentHtml = contentHtml + App_Retreat_Render_Data_TimeLine_Current(data, _min, _min_initi, _max_initi);
                document.getElementById(idMaster).innerHTML = document.getElementById(idMaster).innerHTML
                    + '<div class="ui segment app_retreat_list_content">' + contentHtml + '</div>';
            }
            
        }
    }
    App_Retreat_Event();
}