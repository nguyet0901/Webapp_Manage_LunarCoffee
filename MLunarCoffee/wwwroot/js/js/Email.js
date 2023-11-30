
async function Email_SendTest (fromemail, toemail) {
    new Promise((resolove) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/Mail/SendMailTemplate"
                , data = JSON.stringify({
                    'FromEmail': fromemail.trim(),
                    'ToEmail': toemail.trim(),
                    'CCEmail': '',
                    'BCCEmail': '',
                    'Subject': 'Email testing',
                    'Body': '',
                    'Link': sys_HTTPImageRoot + '_Template/welcome.html',
                    'Attachments': [],
                    'TemplateValues': {
                        '[username]': sys_userName_Main,
                        '[email]': fromemail
                    }
                })
                , async = true
                , success = function (result) {
                }
            );
        }, 100)
    })
}

async function Email_CheckValid(mailtest)
{
    return new Promise((resolve) =>
    {
        setTimeout(() =>
        {
            AjaxJWT(url = "/api/Mail/CheckValid"
                , data = JSON.stringify({
                    'FromEmail': mailtest.trim(),
                    'ToEmail': '',
                    'CCEmail': '',
                    'BCCEmail': '',
                    'Subject': '',
                    'Body': '',
                    'Link': '',
                    'Attachments': [],
                    'TemplateValues': {}
                })
                , async = true
                , success = function (result)
                {
                    resolve(result);
                }
            );
        }, 100)
    })
}

function Email_SendNormal (fromemail, toemail, ccemail, bccemail, subject, body, filelist) {
    try {
        var formData = new FormData();
        if (filelist != undefined && filelist.length != 0) {
            for (let i = 0; i != filelist.length; i++) {
                let ext = filelist[i].name.split('.').pop().toLowerCase();
                let size = filelist[i].size;
                if (filelist.length > 3) return 0;
                if (size > Maxbyteattach) return 0;
                if (ext != "jpg" && ext != "png" && ext != "jpeg" && ext != "html" && ext != "htm"
                    && ext != "xlsx" && ext != "xls" && ext != "doc"
                    && ext != "docx" && ext != "pdf") {
                    return 0;
                }
                formData.append("files", filelist[i]);
            }
        }

        var objArr = {
            "FromEmail": fromemail
            , "ToEmail": toemail
            , "CCEmail": ccemail
            , "BCCEmail": bccemail
            , "Subject": subject
            , "Body": body
            , "Link": ''
            , "TemplateValues": ''

        };
        formData.append('objArr', JSON.stringify(objArr));
        AjaxSendEmail(url = "/api/Mail/Send"
            , formData = formData
            , async = true
        );
        return 1;
    }
    catch (ex) {
        return 0;
    }
}

// emailList = [{ emailFrom, emailTo, emailCC, emailBCC, subject, content }];
function Email_SendMulti(emailList, filelist) {
    try {

        let blockEmail = sliceIntoChunks(emailList, 50);
        if (blockEmail && blockEmail.length !== 0) {
            for (let i = 0; i < blockEmail.length; i++) {

                let formData = new FormData();
                if (filelist !== undefined && filelist.length !== 0) {
                    for (let j = 0; j !== filelist.length; j++) {
                        let ext = filelist[j].name.split('.').pop().toLowerCase();
                        let size = filelist[j].size;
                        if (filelist.length > 3) return 0;
                        if (size > Maxbyteattach) return 0;
                        if (ext !== "jpg" && ext !== "png" && ext !== "jpeg" && ext !== "html" && ext !== "htm"
                            && ext !== "xlsx" && ext !== "xls" && ext !== "doc"
                            && ext !== "docx" && ext !== "pdf") {
                            return 0;
                        }
                        formData.append("files", filelist[j]);
                    }
                }


                let emailListNew = [];
                for (let k = 0; k < blockEmail[i].length; k++) {
                    let item = blockEmail[i][k];
                    var objArr = {
                        "FromEmail": item.emailFrom
                        , "ToEmail": item.emailTo
                        , "CCEmail": item.emailCC
                        , "BCCEmail": item.emailBCC
                        , "Subject": item.subject
                        , "Body": item.content
                        , "Link": ''
                        , "TemplateValues": ''
                    };
                    emailListNew.push(objArr)
                }
                formData.append('objArr', JSON.stringify(emailListNew));
                emailListNew.push(formData);

                AjaxSendEmailMulti(url = "/api/Mail/SendMulti"
                    , formData = formData
                    , async = true
                );

            }
            return 1;
        }
        return 0;
    }
    catch (ex) {
        return 0;
    }
}



async function Email_SendTemplate (fromemail, toemail, ccemail, bccemail, subject, body, link, tempvalue) {
    new Promise((resolove) => {
        setTimeout(() => {
            AjaxJWT(url = "/api/Mail/SendMailTemplate"
                , data = JSON.stringify({
                    'FromEmail': fromemail.trim(),
                    'ToEmail': toemail.trim(),
                    'CCEmail': ccemail.trim(),
                    'BCCEmail': bccemail.trim(),
                    'Subject': subject.trim(),
                    'Body': body.trim(),
                    'Link': link.trim(),
                    'Attachments': [],
                    'TemplateValues': tempvalue
                })
                , async = true
                , success = function (result) { }
            );
        }, 100)
    })
}