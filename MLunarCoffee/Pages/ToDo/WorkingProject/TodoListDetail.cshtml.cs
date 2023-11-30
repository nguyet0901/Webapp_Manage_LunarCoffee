using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using MLunarCoffee.Comon;
using MLunarCoffee.Comon.Session;
using MLunarCoffee.Models.ToDoModel;

namespace MLunarCoffee.Pages.ToDo.WorkingProject
{
    public class TodoListDetailModel : PageModel
    {
        public string _CurrentID { get; set; }
        public void OnGet()
        {
            string curr = Request.Query["CurrentID"];
            if (curr != null)
            {
                _CurrentID = curr.ToString();
            }
            else
            {
                _CurrentID = "0";
            }
        }

        
         public async Task<IActionResult> OnPostExcuteData(string ListName, string ListDescription)
        {
            try
            {
                TodoList[] toDoList;
                string _Todo_List_Detail = "";
                toDoList = GetArrayProject_Detail_Empty(ListName, ListDescription);
                _Todo_List_Detail = JsonConvert.SerializeObject(toDoList);
                return Content(_Todo_List_Detail);
            }
            catch (Exception ex)
            {
                return Content("0");
            }
        }

        public static TodoList[] GetArrayProject_Detail_Empty(string name, string description)
        {
            try
            {
                TodoList l = new TodoList()
                {
                    id = 0,
                    name = name,
                    description = description,
                    tasks = new TodoTask[] { },
                    state = 1
                };
                TodoList[] ls = new TodoList[] { l };
                return ls;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
