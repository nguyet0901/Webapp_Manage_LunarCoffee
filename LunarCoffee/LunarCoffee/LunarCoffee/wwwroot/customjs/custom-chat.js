
//chat page settings
(function () {
    'use strict'
    var chat = {
        messageToSend: '',
        messageResponses: [
            'Why did the web developer leave the restaurant? Because of the table layout.',
            'How do you comfort a JavaScript bug? You console it.',
            'An SQL query enters a bar, approaches two tables and asks: "May I join you?"',
            'What is the most used language in programming? Profanity.',
            'What is the object-oriented way to become wealthy? Inheritance.',
            'An SEO expert walks into a bar, bars, pub, tavern, public house, Irish pub, drinks, beer, alcohol'
        ],
        init: function () {
            this.cacheDOM();
            this.bindEvents();
            this.render();
        },
        cacheDOM: function () {
            this.$chatHistory = $('.chat-history');
            this.$button = $('#SendMessage');
            this.$textarea = $('#message-to-send');
            this.$chatHistoryList = this.$chatHistory.find('ul');
        },
        bindEvents: function () {
            this.$button.on('click', this.addMessage.bind(this));
            this.$textarea.on('keyup', this.addMessageEnter.bind(this));
        },
        render: function () {
            this.scrollToBottom();
            if (this.messageToSend.trim() !== '') { 
                if (this.sendFacebook(this.messageToSend)) {
                    var template = Handlebars.compile($('#template-send').html());
                    var context = {
                        messageOutput: this.messageToSend,
                        time: this.getCurrentTime(),
                        name: "Tôi"
                    };
                    this.$chatHistoryList.append(template(context));
                    this.scrollToBottom();
                }
                this.$textarea.val('');
                //var templateResponse = Handlebars.compile($('#message-response-template').html());
                //var contextResponse = {
                //    response: this.getRandomItem(this.messageResponses),
                //    time: this.getCurrentTime()
                //};
                //setTimeout(function () {
                //    this.$chatHistoryList.append(templateResponse(contextResponse));
                //    this.scrollToBottom();
                //}.bind(this), 1500);
            }
        },
        sendFacebook: function (mes) {
            MessageStatus(recipient, "typing_off");
            let elm = document.getElementById("fileupload");
            let file = elm.files[0];
            let retu = false;
            let temp = fanpages.filter(item => item.KeyPage == idPageSelected);
            if (file) {
                SendFile();
            }
            if (recipient != '') {
                $.ajax({
                    url: "https://graph.facebook.com/v4.0/me/messages",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        "messaging_type": "RESPONSE",
                        "recipient": JSON.stringify({
                            "id": recipient
                        }),
                        "message": JSON.stringify({
                            "text": mes
                        }),
                        "access_token": temp[0].accessToken

                    }),
                    contentType: 'application/json; charset=utf-8',
                    async: false,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        notiError(textStatus);
                    },
                    success: function (result) {
                        //ReloadMess(result.message_id, 'send');
                        UpdateListUI('listMember', 'itemsMenber', result.recipient_id, 'data-ret');
                        UpdateListUI('listFanPage', 'itemFanPage', idPageSelected , 'data-id');
                        retu = true;
                    }
                });
            } else {
                notiError('Người Nhận không xác định');
            }
            return retu;
        },
        addMessage: function () {
            this.messageToSend = this.$textarea.val();
            this.render();
        },
        addMessageEnter: function (event) {
            if (event.keyCode === 13) {
                this.addMessage();
            }
        },
        scrollToBottom: function () {
            this.$chatHistory.scrollTop(this.$chatHistory[0].scrollHeight);
        },
        getCurrentTime: function () {
            return new Date().toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, '$1$3');
        },
        getRandomItem: function (arr) {
            return arr[Math.floor(Math.random() * arr.length)];
        }
    };
    chat.init();
    var searchFilter = {
        options: { valueNames: ['name'] },
        init: function () {
            var userList = new List('people-list', this.options);
            var noItems = $('<li id="no-items-found">No items found</li>');
            userList.on('updated', function (list) {
                if (list.matchingItems.length === 0) {
                    $(list.list).append(noItems);
                } else {
                    noItems.detach();
                }
            });
        }
    };
    searchFilter.init();
}());