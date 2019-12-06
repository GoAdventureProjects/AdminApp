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

        public List<EventExpensesEstimateLookup> GetEventExpensesLookup(int eventDetailId)
        {
            var result = DAL.GetEventExpensesLookup(eventDetailId);
            if (result != null)
            {
                var data = Utils.ConvertDataTable<EventExpensesEstimateLookup>(result);
                return data;
            }
            return null;
        }

        public bool CreateNewStay(EventExpensesEstimateLookup eventExpenses)
        {
            try
            {
                return DAL.CreateStay(eventExpenses);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool UpdateTransport(int eventId, List<TransportSlab> transportSlabs)
        {
            try
            {
                return DAL.UpdateTransport(eventId, transportSlabs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateGuide(int eventId, GuideExpense guide)
        {
            try
            {
                return DAL.UpdateGuide(eventId, guide);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveEventExpenses(int eventDatesId, List<EventExpenseEstimate> eventExpenses)
        {
            try
            {
                return DAL.UpdateReceipt(eventDatesId, eventExpenses);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EventTransaction> GetEventTransactions(int eventDatesId)
        {
            try
            {
                var dt = DAL.GetEventTransactions(eventDatesId);
                if (dt != null)
                {
                    return Utils.ConvertDataTable<EventTransaction>(dt);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Recepient> GetRecepients()
        {
            try
            {
                var dt = DAL.GetRecepients();
                if (dt != null)
                {
                    return Utils.ConvertDataTable<Recepient>(dt);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveEventTransaction(EventTransaction transaction)
        {
            try
            {
                return DAL.SaveEventTransaction(transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EventEstimateAgg> GetEventEstimationAmount(int eventDatesId)
        {
            try
            {
                var dt = DAL.GetEventEstimationAmount(eventDatesId);
                if (dt != null)
                {
                    return Utils.ConvertDataTable<EventEstimateAgg>(dt);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}