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
                    $"(@EventId,@ExpenseTypeId,@StayName,@Amount,@paymentMode,@Notes)";

                var cmd = dbmanager.GetSqlCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventId", eventExpenses.EventID);
                cmd.Parameters.AddWithValue("@ExpenseTypeId", eventExpenses.ExpensesTypeid);
                cmd.Parameters.AddWithValue("@StayName", eventExpenses.ExpenseTypeSource);
                cmd.Parameters.AddWithValue("@Amount", eventExpenses.ExpenseAmount);
                cmd.Parameters.AddWithValue("@paymentMode", eventExpenses.ExpenseModeOfPayment);
                cmd.Parameters.AddWithValue("@Notes", eventExpenses.Notes);
                var rows = dbmanager.ExecuteNonQuery(cmd);

                return (rows >= 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateTransport(int eventId, List<TransportSlab> transportSlabs)
        {
            var ids = string.Join<int>(",", transportSlabs.Select(x => x.ExpensesTypeid).ToList());
            var selectQuery = $"select *from finance.EventExpensesEstimate where EventId=@EventId and ExpensesTypeid in ({ids})";
            var cmd = dbmanager.GetSqlCommand(selectQuery);
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventId", eventId);
                //cmd.Parameters.AddWithValue("@ExpenseTypeIds", ids);

                var dt = dbmanager.GetDataTableResult(cmd);

                //clear above params
                cmd.Parameters.Clear();

                //update exissting records
                if (dt != null && dt.Rows.Count > 0)
                {
                    //cmd.Connection.Open();
                    foreach (DataRow row in dt.Rows)
                    {

                        var ts = transportSlabs.Where(x => x.ExpensesTypeid == int.Parse(row["ExpensesTypeid"].ToString())).First();
                        var updateQuery = $"update finance.EventExpensesEstimate  set ExpenseAmount={ts.Amount},ExpenseModeOfPayment='{ts.ModeOfPayment ?? "online"}', SlabRange ='{ts.Slab}' where EventID={eventId} and ExpensesTypeid={ts.ExpensesTypeid} ";
                        cmd.CommandText = updateQuery;
                        dbmanager.ExecuteNonQuery(cmd);
                    }
                }
                //insert new records
                else
                {
                    cmd.Connection.Open();

                    var insertQuery = $"insert into finance.EventExpensesEstimate " +
                                                $"(EventId,ExpensesTypeid,SlabRange,ExpenseAmount,ExpenseModeOfPayment,CreatedBy,CreatedDate) " +
                                                $"values (@EventId,@ExpensesTypeid,@SlabRange,@ExpenseAmount,@ExpenseModeOfPayment,@CreatedBy,@CreatedDate)";

                    cmd.CommandText = insertQuery;

                    for (int i = 0; i < transportSlabs.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@EventId", eventId);
                        cmd.Parameters.AddWithValue("@ExpensesTypeid", transportSlabs[i].ExpensesTypeid);
                        cmd.Parameters.AddWithValue("@SlabRange", transportSlabs[i].Slab);
                        cmd.Parameters.AddWithValue("@ExpenseAmount", transportSlabs[i].Amount);
                        cmd.Parameters.AddWithValue("@ExpenseModeOfPayment", (transportSlabs[i].ModeOfPayment) ?? "online");
                        //cmd.Parameters.AddWithValue("@Notes", transportSlabs[i].);
                        cmd.Parameters.AddWithValue("@CreatedBy", "Admin");
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        //dbmanager.ExecuteNonQuery(cmd);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public bool UpdateGuide(int eventId, GuideExpense guide)
        {
            var selectQuery = $"select *from finance.EventExpensesEstimate where EventId=@EventId and ExpensesTypeid = {guide.ExpensesTypeid}";
            var cmd = dbmanager.GetSqlCommand(selectQuery);
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventId", eventId);
                var dt = dbmanager.GetDataTableResult(cmd);

                cmd.Parameters.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmd.CommandText = $"update finance.EventExpensesEstimate  set ExpenseTypeSource='{guide.Name}', ExpenseAmount ={guide.Amount},ExpenseModeOfPayment='{guide.PaymentMode ?? "online"}' where EventID={eventId} and ExpensesTypeid={guide.ExpensesTypeid} ";
                    dbmanager.ExecuteNonQuery(cmd);
                }
                else
                {
                    var insertQuery = $"insert into finance.EventExpensesEstimate " +
                                                $"(EventId,ExpensesTypeid,ExpenseTypeSource,ExpenseAmount,ExpenseModeOfPayment,CreatedBy,CreatedDate) " +
                                                $"values (@EventId,@ExpensesTypeid,@GuideName,@ExpenseAmount,@ExpenseModeOfPayment,@CreatedBy,@CreatedDate)";

                    cmd.CommandText = insertQuery;
                    cmd.Parameters.AddWithValue("@EventId", eventId);
                    cmd.Parameters.AddWithValue("@ExpensesTypeid", guide.ExpensesTypeid);
                    cmd.Parameters.AddWithValue("@GuideName", guide.Name);
                    cmd.Parameters.AddWithValue("@ExpenseAmount", guide.Amount);
                    cmd.Parameters.AddWithValue("@ExpenseModeOfPayment", (guide.PaymentMode) ?? "online");
                    cmd.Parameters.AddWithValue("@CreatedBy", "Admin");
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    dbmanager.ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
