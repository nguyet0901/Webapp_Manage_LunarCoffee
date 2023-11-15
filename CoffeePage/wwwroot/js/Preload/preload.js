//#region // var
//var session_base = "BaseData";
//var session_setting = "BaseSetting";
//var indexedDBVer = 9; //Version is number table
var preload_circleAvatar = '/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCADIAMgDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD0yiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAoopNy8/MOOvPSgBaKKTI45HPTnrQAtFGQehooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAKdzIyX9sBgjsCOpJAOPcCqsuzyZFyjxCU5cD7xKk4PqQcVrUUAU5PtKQysWGwQ/KB1DY5qsp3PacLkYWMqODh8Ej8K1aKAKOm9JMf7O7/e5z+NXqKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooA//9k=';
var preload_pic = '/default.png';
const VDMINIFY = 'false';
var preload_compressdata = {
    // private property
    _f: String.fromCharCode,
    compress: function (uncompressed) {
        if (uncompressed == null) return "";
        var i, value,
            context_dictionary = {},
            context_dictionaryToCreate = {},
            context_c = "",
            context_wc = "",
            context_w = "",
            context_enlargeIn = 2, // Compensate for the first entry which should not count
            context_dictSize = 3,
            context_numBits = 2,
            context_data_string = "",
            context_data_val = 0,
            context_data_position = 0,
            ii,
            f = preload_compressdata._f;

        for (ii = 0; ii < uncompressed.length; ii += 1) {
            context_c = uncompressed.charAt(ii);
            if (!Object.prototype.hasOwnProperty.call(context_dictionary, context_c)) {
                context_dictionary[context_c] = context_dictSize++;
                context_dictionaryToCreate[context_c] = true;
            }

            context_wc = context_w + context_c;
            if (Object.prototype.hasOwnProperty.call(context_dictionary, context_wc)) {
                context_w = context_wc;
            } else {
                if (Object.prototype.hasOwnProperty.call(context_dictionaryToCreate, context_w)) {
                    if (context_w.charCodeAt(0) < 256) {
                        for (i = 0; i < context_numBits; i++) {
                            context_data_val = (context_data_val << 1);
                            if (context_data_position == 15) {
                                context_data_position = 0;
                                context_data_string += f(context_data_val);
                                context_data_val = 0;
                            } else {
                                context_data_position++;
                            }
                        }
                        value = context_w.charCodeAt(0);
                        for (i = 0; i < 8; i++) {
                            context_data_val = (context_data_val << 1) | (value & 1);
                            if (context_data_position == 15) {
                                context_data_position = 0;
                                context_data_string += f(context_data_val);
                                context_data_val = 0;
                            } else {
                                context_data_position++;
                            }
                            value = value >> 1;
                        }
                    } else {
                        value = 1;
                        for (i = 0; i < context_numBits; i++) {
                            context_data_val = (context_data_val << 1) | value;
                            if (context_data_position == 15) {
                                context_data_position = 0;
                                context_data_string += f(context_data_val);
                                context_data_val = 0;
                            } else {
                                context_data_position++;
                            }
                            value = 0;
                        }
                        value = context_w.charCodeAt(0);
                        for (i = 0; i < 16; i++) {
                            context_data_val = (context_data_val << 1) | (value & 1);
                            if (context_data_position == 15) {
                                context_data_position = 0;
                                context_data_string += f(context_data_val);
                                context_data_val = 0;
                            } else {
                                context_data_position++;
                            }
                            value = value >> 1;
                        }
                    }
                    context_enlargeIn--;
                    if (context_enlargeIn == 0) {
                        context_enlargeIn = Math.pow(2, context_numBits);
                        context_numBits++;
                    }
                    delete context_dictionaryToCreate[context_w];
                } else {
                    value = context_dictionary[context_w];
                    for (i = 0; i < context_numBits; i++) {
                        context_data_val = (context_data_val << 1) | (value & 1);
                        if (context_data_position == 15) {
                            context_data_position = 0;
                            context_data_string += f(context_data_val);
                            context_data_val = 0;
                        } else {
                            context_data_position++;
                        }
                        value = value >> 1;
                    }


                }
                context_enlargeIn--;
                if (context_enlargeIn == 0) {
                    context_enlargeIn = Math.pow(2, context_numBits);
                    context_numBits++;
                }
                // Add wc to the dictionary.
                context_dictionary[context_wc] = context_dictSize++;
                context_w = String(context_c);
            }
        }

        // Output the code for w.
        if (context_w !== "") {
            if (Object.prototype.hasOwnProperty.call(context_dictionaryToCreate, context_w)) {
                if (context_w.charCodeAt(0) < 256) {
                    for (i = 0; i < context_numBits; i++) {
                        context_data_val = (context_data_val << 1);
                        if (context_data_position == 15) {
                            context_data_position = 0;
                            context_data_string += f(context_data_val);
                            context_data_val = 0;
                        } else {
                            context_data_position++;
                        }
                    }
                    value = context_w.charCodeAt(0);
                    for (i = 0; i < 8; i++) {
                        context_data_val = (context_data_val << 1) | (value & 1);
                        if (context_data_position == 15) {
                            context_data_position = 0;
                            context_data_string += f(context_data_val);
                            context_data_val = 0;
                        } else {
                            context_data_position++;
                        }
                        value = value >> 1;
                    }
                } else {
                    value = 1;
                    for (i = 0; i < context_numBits; i++) {
                        context_data_val = (context_data_val << 1) | value;
                        if (context_data_position == 15) {
                            context_data_position = 0;
                            context_data_string += f(context_data_val);
                            context_data_val = 0;
                        } else {
                            context_data_position++;
                        }
                        value = 0;
                    }
                    value = context_w.charCodeAt(0);
                    for (i = 0; i < 16; i++) {
                        context_data_val = (context_data_val << 1) | (value & 1);
                        if (context_data_position == 15) {
                            context_data_position = 0;
                            context_data_string += f(context_data_val);
                            context_data_val = 0;
                        } else {
                            context_data_position++;
                        }
                        value = value >> 1;
                    }
                }
                context_enlargeIn--;
                if (context_enlargeIn == 0) {
                    context_enlargeIn = Math.pow(2, context_numBits);
                    context_numBits++;
                }
                delete context_dictionaryToCreate[context_w];
            } else {
                value = context_dictionary[context_w];
                for (i = 0; i < context_numBits; i++) {
                    context_data_val = (context_data_val << 1) | (value & 1);
                    if (context_data_position == 15) {
                        context_data_position = 0;
                        context_data_string += f(context_data_val);
                        context_data_val = 0;
                    } else {
                        context_data_position++;
                    }
                    value = value >> 1;
                }


            }
            context_enlargeIn--;
            if (context_enlargeIn == 0) {
                context_enlargeIn = Math.pow(2, context_numBits);
                context_numBits++;
            }
        }

        // Mark the end of the stream
        value = 2;
        for (i = 0; i < context_numBits; i++) {
            context_data_val = (context_data_val << 1) | (value & 1);
            if (context_data_position == 15) {
                context_data_position = 0;
                context_data_string += f(context_data_val);
                context_data_val = 0;
            } else {
                context_data_position++;
            }
            value = value >> 1;
        }

        // Flush the last char
        while (true) {
            context_data_val = (context_data_val << 1);
            if (context_data_position == 15) {
                context_data_string += f(context_data_val);
                break;
            }
            else context_data_position++;
        }
        return context_data_string;
    },
    decompress: function (compressed) {
        if (compressed == null) return "";
        if (compressed == "") return null;
        var dictionary = [],
            next,
            enlargeIn = 4,
            dictSize = 4,
            numBits = 3,
            entry = "",
            result = "",
            i,
            w,
            bits, resb, maxpower, power,
            c,
            f = preload_compressdata._f,
            data = { string: compressed, val: compressed.charCodeAt(0), position: 32768, index: 1 };

        for (i = 0; i < 3; i += 1) {
            dictionary[i] = i;
        }

        bits = 0;
        maxpower = Math.pow(2, 2);
        power = 1;
        while (power != maxpower) {
            resb = data.val & data.position;
            data.position >>= 1;
            if (data.position == 0) {
                data.position = 32768;
                data.val = data.string.charCodeAt(data.index++);
            }
            bits |= (resb > 0 ? 1 : 0) * power;
            power <<= 1;
        }

        switch (next = bits) {
            case 0:
                bits = 0;
                maxpower = Math.pow(2, 8);
                power = 1;
                while (power != maxpower) {
                    resb = data.val & data.position;
                    data.position >>= 1;
                    if (data.position == 0) {
                        data.position = 32768;
                        data.val = data.string.charCodeAt(data.index++);
                    }
                    bits |= (resb > 0 ? 1 : 0) * power;
                    power <<= 1;
                }
                c = f(bits);
                break;
            case 1:
                bits = 0;
                maxpower = Math.pow(2, 16);
                power = 1;
                while (power != maxpower) {
                    resb = data.val & data.position;
                    data.position >>= 1;
                    if (data.position == 0) {
                        data.position = 32768;
                        data.val = data.string.charCodeAt(data.index++);
                    }
                    bits |= (resb > 0 ? 1 : 0) * power;
                    power <<= 1;
                }
                c = f(bits);
                break;
            case 2:
                return "";
        }
        dictionary[3] = c;
        w = result = c;
        while (true) {
            if (data.index > data.string.length) {
                return "";
            }

            bits = 0;
            maxpower = Math.pow(2, numBits);
            power = 1;
            while (power != maxpower) {
                resb = data.val & data.position;
                data.position >>= 1;
                if (data.position == 0) {
                    data.position = 32768;
                    data.val = data.string.charCodeAt(data.index++);
                }
                bits |= (resb > 0 ? 1 : 0) * power;
                power <<= 1;
            }

            switch (c = bits) {
                case 0:
                    bits = 0;
                    maxpower = Math.pow(2, 8);
                    power = 1;
                    while (power != maxpower) {
                        resb = data.val & data.position;
                        data.position >>= 1;
                        if (data.position == 0) {
                            data.position = 32768;
                            data.val = data.string.charCodeAt(data.index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }

                    dictionary[dictSize++] = f(bits);
                    c = dictSize - 1;
                    enlargeIn--;
                    break;
                case 1:
                    bits = 0;
                    maxpower = Math.pow(2, 16);
                    power = 1;
                    while (power != maxpower) {
                        resb = data.val & data.position;
                        data.position >>= 1;
                        if (data.position == 0) {
                            data.position = 32768;
                            data.val = data.string.charCodeAt(data.index++);
                        }
                        bits |= (resb > 0 ? 1 : 0) * power;
                        power <<= 1;
                    }
                    dictionary[dictSize++] = f(bits);
                    c = dictSize - 1;
                    enlargeIn--;
                    break;
                case 2:
                    return result;
            }

            if (enlargeIn == 0) {
                enlargeIn = Math.pow(2, numBits);
                numBits++;
            }

            if (dictionary[c]) {
                entry = dictionary[c];
            } else {
                if (c === dictSize) {
                    entry = w + w.charAt(0);
                } else {
                    return null;
                }
            }
            result += entry;

            // Add w+entry[0] to the dictionary.
            dictionary[dictSize++] = w + entry.charAt(0);
            enlargeIn--;

            w = entry;

            if (enlargeIn == 0) {
                enlargeIn = Math.pow(2, numBits);
                numBits++;
            }

        }
    }
};

//#endregion
//#region // settings
function Master_Setting_Set (name, value) {
    let key = '[' + VDNAME + '][' + session_setting + ']' + name;
    localStorage.setItem(key, preload_compressdata.compress(value));
}
function Master_Setting_Get (name) {
    let key = '[' + VDNAME + '][' + session_setting + ']' + name;
    let value = preload_compressdata.decompress(localStorage.getItem(key));
    return value;
}
//#endregion

//#region // Data DB

function Master_Session_Data (session, setfun) {
    if (!localstorage_check(session)) {
        let e = {};
        e.time = new Date();
        let data = {};
        data["Time"] = e;
        localstorage_set(session, data)
        setfun();
    }
    return exist_preload('DB');
}
function Master_IndexDB_Checking () {
    let isAllow = true;
    //prefixes of implementation that we want to test
    window.indexedDB = window.indexedDB || window.mozIndexedDB ||
        window.webkitIndexedDB || window.msIndexedDB;

    //prefixes of window.IDB objects
    window.IDBTransaction = window.IDBTransaction ||
        window.webkitIDBTransaction || window.msIDBTransaction;
    window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange ||
        window.msIDBKeyRange

    if (!window.indexedDB) {
        isAllow = false;
        window.alert("Your browser doesn't support a stable version of IndexedDB.")
    }
    return isAllow;
}
function Master_IndexDB_Delete (dbName, callback) {
    if (Master_IndexDB_Checking()) {
        window.indexedDB.databases().then((r) => {
            for (let i = 0; i < r.length; i++) window.indexedDB.deleteDatabase(r[i].name);
        }).then(() => {
            if (typeof callback === 'function') callback()
        });
    }
}
function Master_IndexDB_IniTable (dbName, name, data, ver) {
    return new Promise(resolve => {
        dbName = '[' + VDNAME + '][' + VDVERSION + '][' + dbName + ']';
        let request = window.indexedDB.open(dbName, ver);
        request.onupgradeneeded = function (event) {
            let db = event.target.result;
            db.createObjectStore(name, {keyPath: "ID"});
        };
        request.onsuccess = function (event) {
            let database = request.result;
            let transaction = database.transaction([name], 'readwrite');
            let objectStore = transaction.objectStore(name);
            objectStore.add({ID: 1,Name: name,Value: preload_compressdata.compress(JSON.stringify(data))});
            database.close();
            resolve(true);
        };

    });


}
function Master_IndexDB_Reads (dbName, tablenames, callback) {
    try {
        let promises = [];
        for (i = 0; i < tablenames.length; i++) {
            promises.push(Master_IndexDB_Read(dbName, tablenames[i]))
        }
        Promise.all(promises).then((values) => {
            callback(values);
        });
    }
    catch (ex) {
        console.log('error 4: ' + dbName + ' - ' + tablenames)
    }
}
function Master_IndexDB_Read (dbName, name) {
    try {
        return new Promise(function (resolve) {
            dbName = '[' + VDNAME + '][' + VDVERSION + '][' + dbName + ']';
            let request = window.indexedDB.open(dbName, indexedDBVer);
            request.onsuccess = function (event) {
                let db = request.result;
                try {
                    var transaction = db.transaction([name], "readwrite");
                    var objectStore = transaction.objectStore(name);
                    var objectStoreRequest = objectStore.getAll();
                    objectStoreRequest.onsuccess = function (event) {
                        if (objectStoreRequest.result != undefined && objectStoreRequest.result[0] != undefined) {
                            var myRecord = JSON.parse(preload_compressdata.decompress(objectStoreRequest.result[0].Value));
                            var result = myRecord.reduce(function (r, e) {
                                r[e.ID] = e
                                return r;
                            }, {});
                            return resolve(result);
                        }
                        return resolve('');
                    };
                } catch (ex) {
                    console.log('error 1: ' + name +' : '+ ex)
                    return {};
                }
                db.close();
            };
            request.error = function () {
                console.log('error 2: ' + name)
            }

        });
    } catch (ex) {
    }
}
function Fun_GetTeeth_ByToken (data, tokenstring, type) {
    try {
        let result = "";
        if (data != undefined && Object.values(data).length > 0) {

            let _tokens = tokenstring.split(",");
            if (_tokens.length != 0) {
                for (let i = 0; i < _tokens.length; i++) {
                    if (data[_tokens[i]] != undefined) {
                        if (type == 0) {
                            result += (data[_tokens[i]].TeethName).trim() + ', ';
                        }
                        else if (type == 1) {
                            result += (data[_tokens[i]].TeethNameBaby).trim() + ', ';
                        }
                        else {
                            result += (data[_tokens[i]].TeethNameMerge).trim() + ', ';
                        }
                    }
                }
            }
            result = (result.trim()).slice(0, -1);
        }
        return result;
    }
    catch (ex) {
        return "";
    }
}
function Fun_GetString_ByToken (data, tokenstring) {
    try {
        let result = "";
        if (Object.values(data).length > 0) {

            let _tokens = tokenstring.split(",");
            if (_tokens.length != 0) {
                for (let i = 0; i < _tokens.length; i++) {
                    if (data[_tokens[i]] != undefined) {
                        result += (data[_tokens[i]].Name).trim() + ', ';
                    }
                }
            }
            result = (result.trim()).slice(0, -1);
        }
        return result;
    }
    catch (ex) {
        return "";
    }
}
function Fun_GetStringButStringID_ByToken (data, tokenstring, stringID) {
    try {
        let result = "";
        if (Object.values(data).length > 0) {

            let _tokens = tokenstring.split(",");
            if (_tokens.length != 0) {
                for (let i = 0; i < _tokens.length; i++) {
                    if (data[_tokens[i]] != undefined && Number(stringID) != Number(_tokens[i])) {
                        result += (data[_tokens[i]].Name).trim() + ', ';
                    }
                }
            }
            result = (result.trim()).slice(0, -1);
        }
        return result;
    }
    catch (ex) {
        return "";
    }
}
function Fun_GetName_ByID (data, id) {
    try {
        let result = "";
        if (Object.values(data).length > 0) {
            if (data[id] != undefined) {
                result = data[id].Name;
            }
        }
        return result;
    }
    catch (ex) {
        return "";
    }
}
function Fun_GetAvatar_ByID (data, id) {
    try {
        let result = preload_pic;
        if (Object.values(data).length > 0) {
            if (data[id] != undefined) {
                result = data[id].Avatar;
                if (result == '') result = preload_pic;
            }
        }
        return result;
    }
    catch (ex) {
        return preload_pic;
    }
}
function Fun_GetObject_ByID (data, id) {
    try {

        let result = null;
        if (id != 0) {

            if (Object.values(data).length > 0) {
                if (data[id] != undefined) {
                    if (typeof data[id].Avatar === 'object' && data[id].Avatar !== null && data[id].Avatar == '') data[id].Avatar = preload_pic;
                    result = data[id];
                    if (result.Avatar == "") result.Avatar = preload_pic;
                }
            }
        }
        if (result == null) {
            let e = {};
            e.ID = 0;
            e.Name = '-';
            e.Avatar = preload_pic;
            e.Code = '-';
            result = e;
        }
        return result;
    }
    catch (ex) {
        let e = {};
        e.ID = 0;
        e.Name = '-';
        e.Avatar = preload_pic;
        e.Code = '-';
        return e;
    }
}

function Fun_GetObject_ByValueID(data, field, valueID) {
    try {
        let result = null;        
        if (valueID != 0) {
            if (Object.values(data).length > 0) {
                let key = Object.keys(data).find(key => data[key][field] == valueID)
                result = data[key];                
                if (typeof result.Avatar === 'object' && result.Avatar !== undefined && result.Avatar == '') result.Avatar = preload_pic;
            }
        }
        if (result == null) {
            let e = {};
            e.ID = 0;
            e.Name = '-';
            e.Avatar = preload_pic;
            e.Code = '-';
            result = e;
        }
        return result;
    }
    catch (ex) {
        let e = {};
        e.ID = 0;
        e.Name = '-';
        e.Avatar = preload_pic;
        e.Code = '-';
        return e;
    }
}
function Fun_GetArray_ByJson (data, json) {
    try {
        let result = [];
        if (Object.values(data).length > 0) {

            let objjson = JSON.parse(json);
            if (Object.values(objjson).length > 0) {
                for ([key, value] of Object.entries(objjson)) {
                    if (data[key] != undefined) {
                        result.push((data[key].Name).trim());

                    }
                };

            }
        }
        return result;
    }
    catch (ex) {
        return [];
    }
}

//#endregion

//#region // Javascript and css
function js_require_notasync (url, loadnormal) {

    if (VDMINIFY == "true" && !url.includes('/assests/') && !url.includes('/assets/') && !url.includes('.min.js')) url = url.replace('.js', '.min.js');
    if (loadnormal != true) {
        try {
            let key = '[' + VDNAME + ']' + url;
            let isreload = 0;
            let _data;

            let value = preload_compressdata.decompress(localStorage.getItem(key));
            if (value != undefined && value != null && value != '') {
                let _el = JSON.parse(value);
                if (_el != undefined) {
                    let _u = _el.unique;
                    _data = _el.data;
                    if (_u != '' && _u == ver_get()) {
                        isreload = 1;
                    }
                }
            }
            if (isreload == 0) {
                jQuery.ajax({
                    url: url + '?' + ver_get(),
                    dataType: 'script',
                    async: false,
                    success: function (data) {
                        let e = {};
                        e.data = data;
                        e.unique = ver_get();
                        localStorage.setItem(key, preload_compressdata.compress(JSON.stringify(e)));
                    },
                    error: function () {
                        console.log(url + ' failure 2')
                    }
                });
            }
            else {
                let code = _data;
                let s = document.createElement("script");
                s.type = "text/javascript";
                try {
                    s.appendChild(document.createTextNode(code));
                    document.head.appendChild(s);
                } catch (ex) {
                    s.text = code;
                    document.head.appendChild(s);
                }
            }
        }
        catch (ex) {
            jQuery.ajax({
                url: url + '?' + ver_get(),
                dataType: 'script',
                async: false,
                success: function (data) {

                },
                error: function () {
                    console.log(url + ' failure 2')
                }
            });

            console.log(url + ' full storage')
        }
    }
    else {
        jQuery.ajax({
            url: url + '?' + ver_get(),
            dataType: 'script',
            async: false,
            cache: true,
            success: function (data) {
                //let s = document.createElement("script");
                //s.type = "text/javascript";
                //s.appendChild(document.createTextNode(data));
                //document.head.appendChild(s);
            },
            error: function () {
            }
        });
    }
}

function js_require (_url, callback, _skipCache) {
    try {
 
        if (VDMINIFY == "true" && !_url.includes('/assests/') && !_url.includes('/assets/') && !_url.includes('.min.js')) _url = _url.replace('.js', '.min.js');
        basket
            .require({
                url: _url
                , key: VDNAME + _url
                , skipCache: (_skipCache != undefined) ? false : _skipCache
                , unique: _url + ver_get()
            })
            .then(function () {
                if (typeof callback !== 'undefined' && $.isFunction(callback)) {
                    callback();
                }
            });
    }
    catch (ex) {}
}
function css_require (_url) {

    if (VDMINIFY == "true" && !_url.includes('/assests/') && !_url.includes('/assets/')) _url = _url.replace('.css', '.min.css');
    $('head').append($('<link rel="stylesheet" type="text/css" />')
        .attr('href', _url));

}
//#endregion

//#region // version
function ver_check (beforefunc, exefunc) {
    let setver = 0;
    let version = ver_get();
    if (version === '') {
        setver = 1;
    }
    else {
        let verapp = version.split(']')[0];
        verapp = verapp.replace('[', '')
        if (verapp !== VDVERSION) {
            setver = 1;
        }
    }
    if (setver === 1 || exist_preload('JS')) {
        if (typeof beforefunc === 'function') beforefunc();
        ver_reset();
        if (typeof exefunc === 'function') exefunc();
    }


}

function ver_reset () {
    let defaultver = '[' + VDVERSION + ']' + (new Date()).getTime();
    ver_set(defaultver);
}

function ver_set (ver) {
    let key = '[' + VDNAME + ']' + '-ver';
    localStorage.setItem(key, ver);
}

function ver_get () {
    let key = '[' + VDNAME + ']' + '-ver';
    let ver = localStorage.getItem(key);
    if (ver != null && ver != undefined) {
        return ver
    }
    else {
        return '';
    }
}
function exist_preload (_type) {
    let key = '[' + VDNAME + ']' + '[Exist][' + _type + ']';
    let ver = localStorage.getItem(key);
    if (ver != null && ver != undefined) {
        return false;
    }
    else {
        localStorage.setItem(key, 1);
        return true;
    }
}
//#endregion

//#region // local storage
function localstorage_set (key, value) {
    try {
        key = '[' + VDNAME + ']' + key;
        let e = {}; e.unique = ver_get(); e.data = value;
        localStorage.setItem(key, preload_compressdata.compress(JSON.stringify(e)));
    }
    catch (ex) {
        console.log("Local Storage is full, Please empty data");
        console.log(key)
    }
}
function localstorage_get (key) {
    key = '[' + VDNAME + ']' + key;
    let ver = preload_compressdata.decompress(localStorage.getItem(key));
    if (ver != null && ver != undefined) {
        return ver
    }
    else {
        return '';
    }
}
function localstorage_check (key) {
    let isExist = 0;
    let value = localstorage_get(key);
    if (value != '') {
        let _el = JSON.parse(value);
        if (_el != undefined) {
            let _u = _el.unique;
            _data = _el.data;
            if (_u != '' && _u == ver_get()) {
                isExist = 1;
            }
        }
    }
    return (isExist == 1);

}
//#endregion
//#region // author
function author_set (key, value) {
    try {
        key = '[' + VDNAME + ']' + key;
        localStorage.setItem(key, value);
    }
    catch (ex) {
    }
}
function author_get (key) {
    key = '[' + VDNAME + ']' + key;
    let ver = localStorage.getItem(key);
    if (ver != null && ver != undefined) {
        return ver
    }
    else {
        return '';
    }
}
//#endregion