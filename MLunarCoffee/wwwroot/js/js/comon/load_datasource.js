function GetDataComboServiceCustomer(link, fn) {
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {

            fn(JSON.parse(result.d)["Table1"]);
 
        }

    })
};

function GetDataComboServiceCustomerDiscount(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table2"]);
        }
    })
    return x;
}

function GetDataComboTypeStatus(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}
function GetDataComboMethod(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}

//// Load list customer status,
//function GetDentistDescription(link, fn) {
//	var x = "";
//	$.ajax({
//		url: link,
//		dataType: "json",
//		type: "POST",
//		contentType: 'application/json; charset=utf-8',
//		data: JSON.stringify({}),
//		async: true,
//		error: function(XMLHttpRequest, textStatus, errorThrown) {

//			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
//		},
//		success: function(data) {

//			fn(JSON.parse(data.d));
//		}
//	});
//	return x;
//}

//GetCombo History
function GetDataComboTypeHistory(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"]);
        }
    })
    return x;
}

//GetCombo Labo customer
function GetDataComboLaboCus(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
        type: "POST",
		data: JSON.stringify({ "CustomerID": CustomerID }),
		contentType: 'application/json; charset=utf-8',
		async: false,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"], JSON.parse(data.d)["Table4"], JSON.parse(data.d)["Table5"], JSON.parse(data.d)["Table6"]);
		}
	})
	return x;
}

// Load list Labo customer ,
function GetDataSourceLabocus(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {
			fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#dtContentLaboCus").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#dtContentLaboCus").show();
            $("#div_customer_waiting").hide();           
        }
	})
	return x;
}

// Load list Customer Instay ,
function GetDataSourceCustomerInStay(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}


// Load list customer History,
function GetDataSourceHistory(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#dtContentHistory").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#dtContentHistory").show();
            $("#div_customer_waiting").hide();
        }
    })
    return x;
}
// Load list customer Tab,

function GetDataSourceTab(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#group_table_tabcust").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#group_table_tabcust").show();
            $("#div_customer_waiting").hide();
        }
    })
    return x;
}
// Load list customer Payment,
function GetDataSourcePayment(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#group_table_payment").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#group_table_payment").show();
            $("#div_customer_waiting").hide();            
        }
    })
    return x;
}

// Load list customer Payment,
function GetDataSourceTreatment(link, CustomerID, PatientRecordID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID, 'PatientRecordID': PatientRecordID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#dtContentTreatment").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#dtContentTreatment").show();
            $("#div_customer_waiting").hide();
        }
    })
    return x;
}

// Load list customer Broker,
function GetDataSourceBroker(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#group_table_payment").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#group_table_payment").show();
            $("#div_customer_waiting").hide();
        }
	})
	return x;
}
// Load list customer care on Treatment,
function GetDataSourceCustomerCareTreatment(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}


//LoadData CustomerCare AfterTreatment
function GetDataSourceCustomerCareAfterTreatment(link, DateFrom, DateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({"DateFrom": DateFrom, "DateTo": DateTo }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//GetCombo CustomerCare AfterTreatment
function GetDataComboBranch(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}

function GetDataComboUnitVTTH(link,productID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        data: JSON.stringify({ "productID": productID }),
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetInfoCustomer(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"], JSON.parse(data.d)["Table4"]);
        }
    })
    return x;
}
//GetCombo Customer
function GetDataComboCustomer(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"]);
        }
    })
    return x;
}
//GetCombo Appointment
function GetDataComboAppointment(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"]
                , JSON.parse(data.d)["Table4"]
                , JSON.parse(data.d)["Table5"]);
        }
    })
    return x;
}
//GetCombo Appointment    List
function GetDataComboAppointmentList(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"],JSON.parse(data.d)["Table4"]);
        }
    })
    return x;
}
// Load Image Folder Tree
function GetDataSourceImageFolder(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Image Folder Tree
function GetImageByFolder(link, ser_MainCustomerID, folderName, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": ser_MainCustomerID,"folderName": folderName }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}



// Load list customer Care,
function GetDataSourceCustomerCare(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({"CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load list Appointment In Day,
function GetDataSourceAppointmentList(link,  fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
	        fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}


// Load list Appointment By Dat,
function GetDataSourceAppointmentListByDay(link,  dateFrom, dateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}

// Load Searching,
function GetDataSourceSerchCustomer(link, keyword, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "keyword": keyword }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load Searching,
function GetLaboSearchingByKeyword(link, keyword, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "keyword": keyword }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDeleteMergeCusSearchingByKeyword(link, keyword, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "keyword": keyword }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDetinationMergeCusSearchingByKeyword(link, keyword, customerFrom, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "keyword": keyword, "customerFrom": customerFrom }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load Searching,
function GetAppCancelSearchingByKeyword(link, keyword, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "keyword": keyword}),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load list WareHouse,
function GetDataSourceWareHouse(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load list Level
function GetDataSourceLevel(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}

// Load list manu in stay room
function GetDataSourceInStayRoom(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}

// Load list Labo Supplier,
function GetDataSourceLaboSupplier(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

            fn(JSON.parse(data.d).Table, JSON.parse(data.d).Table1);
		}
	})
	return x;
}
// Load list Manu in stay bed list
function GetDataSourceManuInStayBed(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
// Load list Supplier,
function GetDataSourceSupplier(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load list Supplier,
function GetDataSourceProductType(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataSourceUnit(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

///////////// DISCOUNT //////////////////
// Load list Discount
function GetDataDiscount(link, date, branchID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "date": date, "branchID": branchID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
        
            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"]);
        }
    })
    return x;
}


// Execute DataTable Left and right
function ExecuteDataTableLeftAndRight(link, type, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "type": type }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Execute DataTable Left and right
function LoadDiscountDetailByID(link, id, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "id": id }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load Combo Discount
function GetDataComboDiscount(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"]);
        }
    })
    return x;
}
///////////////////////


//////////////EmployeeGroup
//LoadList
function GetDataSourceGroupEmployee(link, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//////////////
//////////////Employee
//LoadList
function GetDataSourceEmployee(link, EmpGroupID,  fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "EmpGroupID": EmpGroupID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataComboEmployee(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"], JSON.parse(data.d)["Table4"], JSON.parse(data.d)["Table5"]);
        }
    })
    return x;
}
///////////////

function GetDataComboInStayRoom(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: false,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {
			fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"]);
		}
	})
	return x;
}
///////////////
function GetDataSourceShift(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
function GetDataSourceWorkSchedule(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
/////////////User
function GetDataSourceGroupUser(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

function GetDataSourceUserList(link, UserBranchID, UserGroupID,  fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "UserBranchID": UserBranchID, "UserGroupID": UserGroupID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataComboUser(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"], JSON.parse(data.d)["Table3"], JSON.parse(data.d)["Table4"]);
        }
    })
    return x;
}
/////////


/////////////Product//////////////
function GetDataSourceProduct(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

function GetDataSourceInputList(link, branch_ID,dateFrom, dateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branch_ID": branch_ID, "datefrom": dateFrom, "dateto": dateTo }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataWareHouseLockList(link, WareID, date, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "WareID": WareID, "date": date }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

function GetDataSourceoutList(link, branch_ID, dateFrom, dateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branch_ID": branch_ID, "datefrom": dateFrom, "dateto": dateTo }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//////////////ServiceType
function GetDataSourceServiceType(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Lock Detail New, and Not New

function GetDataLockDetailNew(link,wareID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "wareID": wareID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

function GetDataLockDetail(link, idLock, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "idLock": idLock}),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

function GetDataReviewWareHouse(link, branchID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchID": branchID}),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataReviewWareHouseEdit(link, ID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "ID": ID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}


function GetDataWareHouseExImNor(link, wareID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "wareID": wareID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// load Data Source Service
function GetDataSourceService(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// load Data Ticket Reason Delete
function GetDataTicketReasonDelete(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

 
//.... Martketing Load TIcket List
function GetDataTicketList(link, UserID, date, statusID, sourceID, statusDataID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "UserID": UserID, "date": date,  "statusID": statusID, "sourceID": sourceID, "statusDataID": statusDataID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

//.... Martketing Load TIcket Import
function GetDataTicketListImport(link, date,  fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "date": date }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//.... Martketing Load TIcket Import
function GetDataTicketMove(link, dateFrom, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//.... Martketing Load TIcket Import
function GetDataTicketListImportView(link, ImportID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
        type: "POST",
        data: JSON.stringify({ "ImportID": ImportID }),
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}

//.... Martketing Load TIcket List
function GetDataTicketListDelete(link, dateFrom, dateTo, groupTeleID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "groupTeleID": groupTeleID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Ticket Source

function GetDataTicketSource(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Ticket Color Code

function GetDataScheduleColorCode(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Schedule Status Color Code

function GetDataScheduleStatusColor(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}


//Load Data Ticket Main By Day
function GetDataTicketMainByDay(link, dateFrom,dateTo,fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}


/////////////////////Permission////////////

//function GetDataPermissionPageList(link, groupid, fn) {
//    var x = "";
//    $.ajax({
//        url: link,
//        dataType: "json",
//        type: "POST",
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({ "groupid": groupid }),
//        async: false,
//        error: function (XMLHttpRequest, textStatus, errorThrown) {

//            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
//        },
//        success: function (data) {

//            fn(JSON.parse(data.d));
//        }
//    })
//    return x;
//}
function GetDataPermissionControlList(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        data: JSON.stringify({  }),
        contentType: 'application/json; charset=utf-8',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataPermissionPageList(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		data: JSON.stringify({}),
		contentType: 'application/json; charset=utf-8',
		async: false,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
function GetDataPermissionGroupPageList(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataPermissionGroupControlList(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

////////////////////////REPORT
//....Revenue
function GetRevenueBranch(link,  branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//....Customer Source
function GetRevenueCustomerSource(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//....Service New Old
function GetRevenueServiceNewOld(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
//....OverView
function GetReportOverView(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"], JSON.parse(data.d)["Table2"],JSON.parse(data.d)["Table3"]);
        }
    })
    return x;
}
/// .... Doanh THu TU Van
function GetRevenueConsultant(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

/// .... Doanh THu Dieu Tri
function GetRevenueTreatment(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

/// .... Doanh THu Dieu Tri
function GetReportTeleSale(link, dateFrom, dateTo, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

////////////////////////////Ticket
function LoadDatasourceticket(link, CustomerID, TicketID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID, "TicketID": TicketID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}


// Load list Discount
function GetDataLiability(link, branchid, dateFrom, dateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "dateFrom": dateFrom, "dateTo": dateTo }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}


// Load list Current InStay
function GetCurrentInStay(link, branchid, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid}),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load List Labo
function GetDataLaboList(link, branchid, dateFilter, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "date": dateFilter }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataDeleteMergeCusList(link, branchid, dateFilter, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "dateFilter": dateFilter }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Appointment Cancel
function GetAppointmentCancelList(link, branchid, dateFrom, dateTo, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "dateFrom": dateFrom, "dateTo": dateTo }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load User INfo
function GetDataUserInfo(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load list customer Schedule,
function GetDataSourceSchedule(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#dtContentScheduler").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#dtContentScheduler").show();
            $("#div_customer_waiting").hide();
        }
    })
    return x;
}
// Danh muc
function GetDataTypeHistory(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
function GetDataTypeAccount(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load list Customer
function GetListCustomer(link, date, branchID, groupID, offset, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "date": date, "branchID": branchID, 'groupID': groupID, "offset": offset}),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}
function GetReportONewCustomer(link, dateFrom, dateTo, branchID, fn) {

    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo, "branchID": branchID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d)["Table"], JSON.parse(data.d)["Table1"]);
        }
    })
    return x;
}


// Load Treatment Material

function GetDataTreatmentMaterial(link, stageID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "stageID": stageID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Treatment Stage By Service

function GetDataStageByService(link, tabID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "tabID": tabID }),
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load Get Data Status Dental Detail,
function GetDataStatusDentalDetail(link, CurrentID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CurrentID": CurrentID }),
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
// GetDataDentistList
function GetDataDentistList(link,  fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ }),
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}

// Get Teeth Description

//function GetTeethDescriptionByDisease(link, dataStatus, fn) {

//	var x = "";
//	$.ajax({
//		url: link,
//		dataType: "json",
//		type: "POST",
//		contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({ "dataStatus": dataStatus }),
//		async: false,
//		error: function (XMLHttpRequest, textStatus, errorThrown) {

//			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
//		},
//		success: function (data) {

//			fn(JSON.parse(data.d));
//		}
//	})
//	return x;
//}
// Load Ticket Tele Group

function GetDataTicketTeleGroup(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
// Load Ticket Group

function GetDataTicketGroup(link,typeid, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
        type: "POST",
        data: JSON.stringify({ 'typeid': typeid }),
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
// Load Get Data SMS Option
function GetDataSMSOption(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}
function GetDataEmailOption(link, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {

			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
		}
	})
	return x;
}



// Load Ticket Status Call

function GetDataTicketStatusCall(link, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        }
    })
    return x;
}
// Load list customer Tab Commission,

function GetDataSourceTabCommission(link, CustomerID, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "CustomerID": CustomerID }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {

            fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#group_table_payment").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#group_table_payment").show();
            $("#div_customer_waiting").hide();
        }
    })
    return x;
}
function GetDataSourceTabDesposit(link, CustomerID, fn) {
	var x = "";
	$.ajax({
		url: link,
		dataType: "json",
		type: "POST",
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify({ "CustomerID": CustomerID }),
		async: true,
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
		},
		success: function (data) {

			fn(JSON.parse(data.d));
        },
        beforeSend: function () {
            $("#group_table_payment").hide();
            $("#div_customer_waiting").show();
        },
        complete: function () {
            $("#group_table_payment").show();
            $("#div_customer_waiting").hide();
        }
	})
	return x;
}
///////////////////////////////////////////////////////////
// Load List Instay
function GetDataInStayList(link, branchid, dateFilter, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "dateFilter": dateFilter }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}

// Load List Broker
function GetDataBrokerValueList(link, branchid, date, fn) {
    var x = "";
    $.ajax({
        url: link,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "branchid": branchid, "date": date }),
        async: true,
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (data) {
            fn(JSON.parse(data.d));
        }
    })
    return x;
}