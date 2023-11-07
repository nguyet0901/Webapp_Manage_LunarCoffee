using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLunarCoffee.Models.Model.AlrmScheduler
{

    public class AlrmScheduler
    {
        public int id { get; set; }
        public int dayofweek { get; set; }
        public int type { get; set; }
        public string hour { get; set; }
        public string date { get; set; }
        public int day { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string createdName { get; set; }
        public string createdWorkName { get; set; }
        public DateTime dateCreated{ get; set; }
        public int status { get; set; }
        public int empWork { get; set; }
        public int empCreate { get; set; }
        public string checklist { get; set; }
        public int editButton { get; set; }
        public int deleteButton { get; set; }
    }

    public class AlrmScheduler_Extension
    {
        public int alarmID { get; set; }
        public int status { get; set; }
        public int isNew { get; set; }
        public int countComment { get; set; }
        public DateTime date { get; set; }
    }
    public class AlrmScheduler_List
    {
        public int type { get; set; }
        public string text { get; set; }
        public string title { get; set; }
    }

    public class AlrmScheduler_Item
    {
        public int dayofweek { get; set; }
        public int type { get; set; }
        public string hour { get; set; }
        public string date { get; set; }
        public int day { get; set; }    
    }
}