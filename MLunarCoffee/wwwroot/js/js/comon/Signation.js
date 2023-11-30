 
var Signature_ResultSignation = "";
var Signature_IsStopTimeout = 0;
var Signature_ImageView = '';
var Signature_MySelf = 0;

function Signature_Stop_Timeout() {
    Signature_IsStopTimeout = 1;
   
}
function Signature_Action (signdata, ismyself) {
 
    Signature_ImageView = (signdata != undefined) ? signdata : "";
    Signature_MySelf = (ismyself != undefined) ? ismyself : 0;
    return new Promise((resolve, reject) => {
        setTimeout(
            () => {
                Signature_InitSignation();
                Signature_IsStopTimeout = 0;
                Signature_ResultSignation = "";
                var timeout = setInterval(change, 500);
                function change() {
                    if (Signature_IsStopTimeout == 1) {
                        clearInterval(timeout);
                        resolve(true)
                    }
                }
                
            },
        )
    })
}
function Signature_InitSignation() { 
    $("#DetailModalSign_Content").html('');
    $("#DetailModalSign_Content").load("/Signature/Signature");
    $('#DetailModalSign').modal('show');
    
} 