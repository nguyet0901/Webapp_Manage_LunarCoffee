 
function formatNumber(num) {
    if (num != undefined && num != 'null' && num != '') {
        let numtemp = roundToNumber(num, '2');
        if (numtemp != null)
            num = numtemp;
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }
    else return '0';
}

function formatNumberToMilion(number) {
    return formatNumber(number);
    let num = number;
    if (Math.abs(num) >= 1000000) {
        if ((num % 1000000) == 0) {
            num = (num / 1000000).toFixed(0);
        } else if ((num % 1000000 % 100000) / 10000 == 0) {
            num = (num / 1000000).toFixed(1);
        } else {
            num = (num / 1000000).toFixed(2);
        }
        num = formatNumber(num) + ` M`;
    }
    else {
        num = formatNumber(num)
    }
    let value = num.toString();
    //let length = value.length;
    //let tail = value.substring(length - 2, length);
    //let ret = value.substring(0, length - 2);
    //return formatNumber(ret) + tail;
    return value;
};

function formatNumberPrice(number) {
    let num = number;
    if (num && num > 1000) {
        if (num >= 1000000) {
            if ((num % 1000000) == 0) {
                num = (num / 1000000).toFixed(0);
            } else if ((num % 1000000 % 100000) / 10000 == 0) {
                num = (num / 1000000).toFixed(1);
            } else {
                num = (num / 1000000).toFixed(2);
            }
            num = formatNumber(num) + ` <span class="text-xs text-muted mb-0 ">${Outlang["Trieu"] ?? 'Triệu'}</span>`;
        }
        else if (num >= 1000) {
            num = formatNumber((num / 1000).toFixed(0)) + ` <span class="text-xs text-muted mb-0 ">${Outlang["Nghin"] ?? 'Nghìn'}</span>`;
        }
    } else {
        return formatNumber(num)
    }
    let value = num.toString();
    //let length = value.length;
    //let tail = value.substring(length - 2, length);
    //let ret = value.substring(0, length - 2);
    //return formatNumber(ret) + tail;
    return value;
};
function formatNumberPrice_NumberOnly(number) {
    let num = number;
    if (num && num > 1000) {
        if (num >= 1000000) {
            if ((num % 1000000) == 0) {
                num = (num / 1000000).toFixed(0);
            } else if ((num % 1000000 % 100000) / 10000 == 0) {
                num = (num / 1000000).toFixed(1);
            } else {
                num = (num / 1000000).toFixed(2);
            }
            num = num ;
        }
        else if (num >= 1000) {
            num = (num / 1000).toFixed(0) ;
        }
    } else {
        return num
    }
    let value = num.toString();
    let length = value.length;
    let tail = value.substring(length - 2, length);
    let ret = value.substring(0, length - 2);
    return formatNumber(ret) + tail;
};
//#endregion
// làm tròn số với số thập phân
function roundToNumber(num, roundTo) {
    try {
        if (roundTo != undefined && parseInt(roundTo) >= 10) {
            let roundValue = parseInt(roundTo);
            return Math.ceil(num / roundValue) * roundValue;
        } else if (parseInt(roundTo) < 10) {
            return +(Math.ceil(num + "e+" + roundTo) + "e-" + roundTo); 
        }
    }
    catch (err) {
        return null;
    }
}
(function () {
    /**
     * Tinh chỉ số thập phân của một con số.
     *
     * @param {String}  type  Loại điều chỉnh.
     * @param {Number}  value Số liệu.
     * @param {Integer} exp   Số mũ (the 10 logarithm of the adjustment base).
     * @returns {Number} Giá trị đã chỉnh sửa.
     */
    function decimalAdjust(type, value, exp) {
        // Nếu exp có giá trị undefined hoặc bằng không thì...
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // Nếu value không phải là số hoặc exp không phải là số nguyên thì...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return NaN;
        }
        // Shift
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }
    // Làm tròn số thập phân (theo mốc số 5)
    if (!Math.roundfloat) {
        Math.roundfloat = function (value, exp) {
            value = decimalAdjust('round', value, exp);
            value = value.toString().split('e');
            value = value[1] ? (+(value[0] + 'e' + value[1])).toFixed(Math.abs(value[1]) + (value[0] > 1 ? 1 : 0)) : value[0];
            return value;
        };
    }
    // Làm tròn số thập phân (về gần giá trị 0)
    if (!Math.floorfloat) {
        Math.floorfloat = function (value, exp) {
            value = decimalAdjust('floor', value, exp);
            value = value.toString().split('e');
            value = value[1] ? (+(value[0] + 'e' + value[1])).toFixed(Math.abs(value[1]) + (value[0] > 1 ? 1 : 0)) : value[0];
            return value;
        };
    }
    // Làm tròn số thập phân (ra xa giá trị 0)
    if (!Math.ceilfloat) {
        Math.ceilfloat = function (value, exp) {
            value = decimalAdjust('ceil', value, exp);
            value = value.toString().split('e');
            value = value[1] ? (+(value[0] + 'e' + value[1])).toFixed(Math.abs(value[1]) + (value[0] > 1 ? 1 : 0)) : value[0];
            return value;
        };
    }
})();


//function convert_block_three(number) {
//    if (number == '000') return '';
//    var _a = number + ''; //Convert biến 'number' thành kiểu string

//    //Kiểm tra độ dài của khối
//    switch (_a.length) {
//        case 0: return '';
//        case 1: return chuHangDonVi[_a];
//        case 2: return convert_block_two(_a);
//        case 3:
//            var chuc_dv = '';
//            if (_a.slice(1, 3) != '00') {
//                chuc_dv = convert_block_two(_a.slice(1, 3));
//            }
//            var tram = chuHangTram[_a[0]] + ' trăm';
//            return tram + ' ' + chuc_dv;
//    }
//}

//function convert_block_two(number) {
//    var dv = chuHangDonVi[number[1]];
//    var chuc = chuHangChuc[number[0]];
//    var append = '';

//    // Nếu chữ số hàng đơn vị là 5
//    if (number[0] > 0 && number[1] == 5) {
//        dv = 'lăm'
//    }

//    // Nếu số hàng chục lớn hơn 1
//    if (number[0] > 1) {
//        append = ' mươi';

//        if (number[1] == 1) {
//            dv = ' mốt';
//        }
//    }

//    return chuc + '' + append + ' ' + dv;
//}


function ConvertNumberToText_Vietnamese(number) {
    var defaultNumbers = ' hai ba bốn năm sáu bảy tám chín';
    var chuHangDonVi = ('1 một' + defaultNumbers).split(' ');
    var chuHangChuc = ('lẻ mười' + defaultNumbers).split(' ');
    var chuHangTram = ('không một' + defaultNumbers).split(' ');
    var dvBlock = '1 nghìn triệu tỷ'.split(' ');

    var convert_block_three = (number) => { 
        if (number == '000') return '';
        var _a = number + ''; //Convert biến 'number' thành kiểu string

        //Kiểm tra độ dài của khối
        switch (_a.length) {
            case 0: return '';
            case 1: return chuHangDonVi[_a];
            case 2: return convert_block_two(_a);
            case 3:
                var chuc_dv = '';
                if (_a.slice(1, 3) != '00') {
                    chuc_dv = convert_block_two(_a.slice(1, 3));
                }
                var tram = chuHangTram[_a[0]] + ' trăm';
                return tram + ' ' + chuc_dv;
        }
    }

    var convert_block_two = (number) => {
        var dv = chuHangDonVi[number[1]];
        var chuc = chuHangChuc[number[0]];
        var append = '';

        // Nếu chữ số hàng đơn vị là 5
        if (number[0] > 0 && number[1] == 5) {
            dv = 'lăm'
        }

        // Nếu số hàng chục lớn hơn 1
        if (number[0] > 1) {
            append = ' mươi';

            if (number[1] == 1) {
                dv = ' mốt';
            }
        }

        return chuc + '' + append + ' ' + dv;
    }

    var str = parseInt(number) + '';
    var i = 0;
    var arr = [];
    var index = str.length;
    var result = [];
    var rsString = '';

    if (index == 0 || str == 'NaN' || str == 0) {
        return 'Không';
    }

    // Chia chuỗi số thành một mảng từng khối có 3 chữ số
    while (index >= 0) {
        arr.push(str.substring(index, Math.max(index - 3, 0)));
        index -= 3;
    }

    // Lặp từng khối trong mảng trên và convert từng khối đấy ra chữ Việt Nam
    for (i = arr.length - 1; i >= 0; i--) {
        if (arr[i] != '' && arr[i] != '000') {
            result.push(convert_block_three(arr[i]));

            // Thêm đuôi của mỗi khối
            if (dvBlock[i]) {
                result.push(dvBlock[i]);
            }
        }
    }

    // Join mảng kết quả lại thành chuỗi string
    rsString = result.join(' ');

    // Trả về kết quả kèm xóa những ký tự thừa
    return rsString.replace(/[0-9]/g, '').replace(/ /g, ' ').replace(/ $/, '').replace('  ', ' ');
}