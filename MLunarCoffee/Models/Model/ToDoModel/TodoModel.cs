using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.ToDoModel
{
    public class TodoCheckList
    {
        public Int64 id { get; set; }        
        public string name { get; set; }
        public int state { get; set; }

    }
    public class TodoTask
    {
        public Int64 id { get; set; } 
        public string name { get; set; }
        public string description { get; set; }      
        public int staffid { get; set; }
        public DateTime created { get; set; }
        public int state { get; set; }
        public TodoCheckList[] checklists { get; set; }
    }
    public class TodoList
    {
        public Int64 id { get; set; } 
        public string name { get; set; }
        public string description { get; set; }
        public int state { get; set; }
        public TodoTask [] tasks { get; set; }
    }
    public class TodoProject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string team_id { get; set; }
        public string status_id { get; set; }

        public TodoList[] lists { get; set; }
    }
}