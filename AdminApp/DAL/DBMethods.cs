using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Models.Admin;

namespace AdminApp.DAL
{
    public class DBMethods
    {
        private App.DAL.StoredProcs.Admin DAL;

        public DBMethods()
        {
            DAL = new App.DAL.StoredProcs.Admin();
        }

        public List<EventDetails> GetEventsByDate(DateTime fromDate)
        {
            var result = DAL.GetEventsByDate(DateTime.Now);

            if (result != null)
            {
                var events = Utils.ConvertDataTable<EventDetails>(result);
                return events;
            }
            return null;
        }

        public List<LuEventType> GetEventTypes()
        {
            var result = DAL.GetluEventTypes();

            if (result != null)
            {
                var data = Utils.ConvertDataTable<LuEventType>(result);
                return data;
            }
            return null;
        }

        public List<LuEvents> GetAllLuEvents()
        {
            var result = DAL.GetluEvents();

            if (result != null)
            {
                var data = Utils.ConvertDataTable<LuEvents>(result);
                return data;
            }
            return null;
        }


        public List<LuEventExpenseType> GetLuExpenseTypes()
        {
            var result = DAL.GetluExpenseTypes();
            if (result != null)
            {
                var data = Utils.ConvertDataTable<LuEventExpenseType>(result);
                return data;
            }
            return null;
        }

        public List<EventExpensesEstimate> GetEventExpenses(int eventDetailId)
        {
            var result = DAL.GetEventExpenses(eventDetailId);
            if (result != null)
            {
                var data = Utils.ConvertDataTable<EventExpensesEstimate>(result);
                return data;
            }
            return null;
        }
    }
}