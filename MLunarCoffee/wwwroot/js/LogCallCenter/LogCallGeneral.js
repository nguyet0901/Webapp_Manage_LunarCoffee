function LoadLogCall_ByPhoneEx(dtConnect, dateFrom, dateTo, extension, page, phone_ext, callback)
{
    try {
        let dtLog = [];
        if (dtConnect.Type != undefined) {
            switch (dtConnect.Type) {
                case 2:
                    dtLog = AllLog_CMC_V1(dtConnect.Host, dtConnect.Port, dtConnect.Domain, dtConnect.Limit, dateFrom, dateTo, extension, page, dtConnect.UserName, dtConnect.Password, dtConnect.URLRecord, callback);
                    break;
                case 3:
                    dtLog = AllLog_CMC_V2(dtConnect.APIbaseurl, dtConnect.Api_Key, dtConnect.DomainAPI, dateFrom, dateTo, extension, page);
                    break;
                case 4:
                    dtLog = AllLog_ThienKhue(dtConnect.APIbaseurl, dtConnect.Api_Key, dtConnect.DomainAPI, dateFrom, dateTo, extension, page, dtConnect.UserName, dtConnect.Password);
                    break;
                case 5:
                    dtLog = AllLog_TACA(dtConnect.APIbaseurl, dtConnect.Api_Key, dtConnect.DomainAPI, dateFrom, dateTo, extension, page, dtConnect.URLRecord);
                    break;
                case 6:
                    dtLog = AllLog_Voicecloud(dateFrom, dateTo, extension);
                    break;
                case 7:
                    dtLog = AllLog_CloudFone(dtConnect.APIbaseurl, dtConnect.DomainAPI, dtConnect.UserName, dtConnect.Password, dateFrom, dateTo, extension, page);
                    break;
                case 8:
                    dtLog = AllLog_Voip24h(dtConnect.APIbaseurl, dtConnect.UserName, dtConnect.Password, dateFrom, dateTo, extension, page, phone_ext);
                    break;
                case 10:
                    dtLog = AllLog_Voicecloud(dtConnect.APIbaseurl, dtConnect.URLRecord, dtConnect.LinkDownloadAudio, dtConnect.UserName, dtConnect.Password, dateFrom, dateTo, extension, dtConnect.Limit, phone_ext);
                    break;
                case 11:
                    dtLog = AllLog_ETelecom(dtConnect.APIbaseurl, dtConnect.Api_Key, dateFrom, dateTo, extension, phone_ext);
                    break;
                case 12:
                    dtLog = AllLog_Omicall(dtConnect.APIbaseurl, dtConnect.Api_Key, dateFrom, dateTo, extension, phone_ext, page);
                    break;
                case 13:
                    dtLog = AllLog_Incall(dateFrom, dateTo, extension);
                    break;
                case 14:
                    dtLog = AllLog_Callio(dtConnect.APIbaseurl, dtConnect.Api_Key, dateFrom, dateTo, extension, phone_ext, page);
                    break;
                case 15:
                    dtLog = AllLog_3CX(dateFrom, dateTo, extension);
                    break;
                case 16:
                    dtLog = AllLog_YCall(dateFrom, dateTo, extension);
                    break;
                default:
                    console.log("Not setting log");
            }
            return dtLog;
        }
        else {
            return [];
        }
    }
    catch (ex) {
        return [];
    }
}

function LogCallGeneral_Author(dtConnect)
{
    try {
        let strToken = "";
        if (dtConnect.Type != undefined) {
            switch (dtConnect.Type) {
                case 2: //CMC_V1
                    return "";
                    break;
                case 3: //AllLog_CMC_V2
                    return "";
                    break;
                case 4: //ThienKhue
                    return "";
                    break;
                case 5://TACA
                    return "";
                    break;
                case 6://VoiceCloud
                    return "";
                    break;
                case 7://CloudFone
                    return "";
                    break;
                case 8://Voip24h
                    return "";
                    break;
                case 10://FPT_Oncall
                    strToken = LogCall_FTP_Author(dtConnect.APIbaseurl, dtConnect.UserName, dtConnect.Password);
                    break;
                case 11://ETelecom
                    return "";
                    break;
                case 12://Omicall
                    return "";
                    break;
                case 13://Incall
                    return "";
                    break;
                default:
                    console.log("Not setting log");
            }
            return strToken;
        }
        else {
            return "";
        }
    }
    catch (ex) {
        return "";
    }
}

function LogCallGeneral_LoadAudio(dtConnect, dateFrom, dateTo, extension)
{
    try {
        let dtAudio = [];
        if (dtConnect.Type != undefined) {
            switch (dtConnect.Type) {
                case 2: //CMC_V1
                    return "";
                    break;
                case 3: //AllLog_CMC_V2
                    return "";
                    break;
                case 4: //ThienKhue
                    return "";
                    break;
                case 5://TACA
                    return "";
                    break;
                case 7://CloudFone
                    return "";
                    break;
                case 8://Voip24h
                    return "";
                    break;
                case 10://FPT_Oncall
                    let dtext = LogCall_FTP_LoadAudio(dtConnect.APIbaseurl, dtConnect.UserName, dtConnect.Password, dtConnect.URLRecord, extension, dateFrom, dateTo, dtConnect.Limit);
                    let dt2013 = LogCall_FTP_LoadAudio(dtConnect.APIbaseurl, dtConnect.UserName, dtConnect.Password, dtConnect.URLRecord, 2013, dateFrom, dateTo, dtConnect.Limit);
                    let dt50000 = LogCall_FTP_LoadAudio(dtConnect.APIbaseurl, dtConnect.UserName, dtConnect.Password, dtConnect.URLRecord, 50000, dateFrom, dateTo, dtConnect.Limit);
                    dtAudio = [...dtext, ...dt2013, ...dt50000];
                    break;
                case 11://ETelecom
                    return "";
                    break;
                case 12://Omicall
                    return "";
                    break;
                case 13://Incall
                    return "";
                    break;
                default:
                    console.log("Not setting log");
            }
            return dtAudio;
        }
        else {
            return [];
        }
    }
    catch (ex) {
        return [];
    }
}