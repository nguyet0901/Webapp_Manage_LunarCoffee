

function Calendar_Fill_Data_OldNew(data, datasercare,careid) {

    if (data != undefined && data.length != 0) {
        for (i = 0; i < data.length; i++) {
            let e = data[i];
            let firstcare = e.ServiceCare_ID.substr(0, e.ServiceCare_ID.indexOf(','));
            if (firstcare !== "") {
                let va = datasercare[firstcare];
                if (va != undefined) {
                    //data[i].ColorText =  va.Name;
                    data[i].ColorCode = va.Color;
                    data[i].SerCareID = va.ID;
                }
            }
        }
        return data;
    }
    else {
        return [];
    }
}


