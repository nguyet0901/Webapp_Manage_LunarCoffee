# VTTECH.github.io


----1. Get data Icon
var dataicon = [];
var icon = document.getElementsByClassName('icon')
for(let i = 0 ; i < icon.length ;i++){
    
    let e = {};
    e.ClassName = icon[i].classList.value;
    e.Name = icon[i].classList.value;
    dataicon.push(e)
}


for(let i = 0 ; i < dataicon.length ;i++){
   	dataicon[i].ClassName = ((dataicon[i].ClassName).replace("icon",'vtt-icon')).trim()
   	dataicon[i].Name = ((dataicon[i].Name).replaceAll("icon vttech-icon-",'')).trim()
 	dataicon[i].Name = ((dataicon[i].Name).replaceAll("-",' ')).trim()
}

copy(dataicon)
Có data copy vào file js/icon.js => thay vào data "data_Icon_List"




----2: Copy file fonts => đè vào file fonts 
----3: Copy css của file style.css(anh Lộc gửi) đè vào file dist/css/font.css 