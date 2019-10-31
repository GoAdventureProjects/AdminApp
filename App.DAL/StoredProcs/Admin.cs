using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using App.Models.Admin;

namespace App.DAL.StoredProcs
{
    public class Admin
    {
        private DBManager dbmanager = new DBManager();

        public Admin()
        {

        }

        public DataTable GetluEventTypes()
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand("SELECT DISTINCT EventType AS Title FROM EventDetails  WHERE EventType IS NOT NULL");
                cmd.CommandType = CommandType.Text;

                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetluEvents()
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand("select distinct Id as EventDetailsId,Title as EventTitle,EventType from EventDetails ");
                cmd.CommandType = CommandType.Text;

                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEventsByDate(DateTime fromDate)
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand("GoAdmin_GetEventsByDate");

                cmd.CommandType = CommandType.StoredProcedure;
                dbmanager.AddParameter(cmd, "@FromDate", SqlDbType.DateTime);
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetluExpenseTypes()
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand("SELECT *FROM finance.EventExpensesType");
                cmd.CommandType = CommandType.Text;

                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEventExpenses(int eventDetailId)
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand($"SELECT *FROM finance.EventExpensesEstimate where EventID={eventDetailId}");
                cmd.CommandType = CommandType.Text;

                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region    create/update

        public bool CreateStay(EventExpensesEstimate eventExpenses)
        {
            try
            {
                var query = $"INSERT INTO finance.EventExpensesEstimate (EventID,ExpensesTypeid,ExpenseTypeSource,ExpenseAmount,Notes) VALUES " +
                    $"(@EventId,@ExpenseTypeId,@StayName,@Amount,@Notes)";

                var cmd = dbmanager.GetSqlCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventId", eventExpenses.EventID);
                cmd.Parameters.AddWithValue("@ExpenseTypeId", eventExpenses.ExpensesTypeid);
                cmd.Parameters.AddWithValue("@StayName", eventExpenses.ExpenseTypeSource);
                cmd.Parameters.AddWithValue("@Amount", eventExpenses.ExpenseAmount);
                cmd.Parameters.AddWithValue("@Notes", eventExpenses.Notes);
                var rows = dbmanager.ExecuteNonQuery(cmd);

                return (rows >= 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
