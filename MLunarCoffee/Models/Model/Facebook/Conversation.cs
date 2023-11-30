using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.Facebook
{
    public class Conversation
    {
        public string id { get; set; } // id nguoi gui hoac nguoi nhan
        public string conver_id { get; set; } // id cuoc tro chuyen
        public string unread_count { get; set; }
        public string updated_time { get; set; }
        public string snippet { get; set; }
        public string name { get; set; }
        public string timespanread { get; set; }
        public string idfrom { get; set; }// id nguoi gui
        public string idcomment { get; set; }// id comment
        public string idpost { get; set; }// id post
        public string type { get; set; }// 0 :message ,1: comment
        public string ticketid { get; set; }
        public string tag { get; set; }// 0 :message ,1: comment
        public string block { get; set; }
        public string star { get; set; }
        public string currentsend_id { get; set; }
        public string staff { get; set; }
        public string staffname { get; set; }
        public string IsSearch_Filter { get; set; }

        public string IsCallBack { get; set; }
        public string IsTakeCareCallBack { get; set; }
        public DateTime CallBackTime { get; set; }
    }
    public class ObjectSender
    {
        public string id { get; set; } // id nguoi gui hoac nguoi nhan
        public string name { get; set; }
    }
    public class ObjectAttachment
    {
        public string name { get; set; }
        public string mime_type { get; set; }
        public string url { get; set; }
    }
    public class ConversationDetail
    {
        public ObjectSender objfrom { get; set; }
        public ObjectSender objto { get; set; }
        public string tag_inbox { get; set; }
        public string tag_read { get; set; }
        public string tag_sent { get; set; }
        public string tag_source { get; set; }
        public string message { get; set; }
        public string created_time { get; set; }
        public string sticker { get; set; }
        public ObjectAttachment [] attachments { get; set; }
        public string read { get; set; }
        public string updated_time { get; set; }
    }
    public class ConversationDetail_Search
    {
        public ObjectSender objfrom { get; set; }
        public string message { get; set; }
        public string created_time { get; set; }
    }

    public class CommentDetail
    {
        public ObjectSender objfrom { get; set; }
        public ObjectSender objto { get; set; }
        public string tag_inbox { get; set; }
        public string tag_read { get; set; }
        public string tag_sent { get; set; }
        public string tag_source { get; set; }
        public string message { get; set; }
        public string created_time { get; set; }
        public string sticker { get; set; }
        public ObjectAttachment[] attachments { get; set; }
        public string read { get; set; }
        public string updated_time { get; set; }
        public string inboxfromcomment { get; set; }
        public string isDelete { get; set; } // first comment is Delete
        public string isHidden { get; set; }
        public string isLike { get; set; }
        public string comment_id { get; set; }
        public string permalink_url { get; set; }
        public string isFirstComment { get; set; }
    }
}