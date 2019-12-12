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

        public DataTable GetEventsByDate(string fromDate)
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand("GoAdmin_GetEventsByDate");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                //dbmanager.AddParameter(cmd, "@FromDate", SqlDbType.DateTime);
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

        public DataTable GetEventExpensesLookup(int eventDetailId)
        {
            try
            {
                var cmd = dbmanager.GetSqlCommand($"SELECT *FROM  finance.EventExpensesEstimateLookup where EventID={eventDetailId}");
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

        public bool CreateStay(EventExpensesEstimateLookup eventExpenses)
        {
            try
            {
                var query = $"INSERT INTO  finance.EventExpensesEstimateLookup (EventID,ExpensesTypeid,ExpenseTypeSource,ExpenseAmount,ExpenseModeOfPayment,Notes) VALUES " +
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
            var selectQuery = $"select *from  finance.EventExpensesEstimateLookup where EventId=@EventId and ExpensesTypeid in ({ids})";
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
                        var updateQuery = $"update  finance.EventExpensesEstimateLookup  set ExpenseAmount={ts.Amount},ExpenseModeOfPayment='{ts.ModeOfPayment ?? "online"}', SlabRange ='{ts.Slab}' where EventID={eventId} and ExpensesTypeid={ts.ExpensesTypeid} ";
                        cmd.CommandText = updateQuery;
                        dbmanager.ExecuteNonQuery(cmd);
                    }
                }
                //insert new records
                else
                {
                    cmd.Connection.Open();

                    var insertQuery = $"insert into  finance.EventExpensesEstimateLookup " +
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
            var selectQuery = $"select *from  finance.EventExpensesEstimateLookup where EventId=@EventId and ExpensesTypeid = {guide.ExpensesTypeid}";
            var cmd = dbmanager.GetSqlCommand(selectQuery);
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventId", eventId);
                var dt = dbmanager.GetDataTableResult(cmd);

                cmd.Parameters.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmd.CommandText = $"update  finance.EventExpensesEstimateLookup  set ExpenseTypeSource='{guide.Name}', ExpenseAmount ={guide.Amount},ExpenseModeOfPayment='{guide.PaymentMode ?? "online"}' where EventID={eventId} and ExpensesTypeid={guide.ExpensesTypeid} ";
                    dbmanager.ExecuteNonQuery(cmd);
                }
                else
                {
                    var insertQuery = $"insert into  finance.EventExpensesEstimateLookup " +
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


		public bool UpdateReceipt(int eventDatesId,List<EventExpenseEstimate> eventExpenses)
		{
			var cmd = dbmanager.GetSqlCommand();
			try
			{
				cmd.Connection.Open();
				foreach (var item in eventExpenses)
				{
					var query = $"insert into Finance.EventExpensesEstimate (EventDatesID,EventExpensesEstimateLookupID,Notes,IsActive,CreatedBy,CreatedDate) " +
						$"values (@eventDatesID, @eventExpensesEstimateLookupID, @notes, @isActive, @createdBy, @createdDate)";
					cmd.CommandText = query;
					cmd.Parameters.AddWithValue("@eventDatesID", eventDatesId);
					cmd.Parameters.AddWithValue("@eventExpensesEstimateLookupID", item.EventExpensesEstimateLookupID);
					cmd.Parameters.AddWithValue("notes", string.IsNullOrEmpty(item.Notes) ? "" : item.Notes);
					cmd.Parameters.AddWithValue("@isActive", true);
					cmd.Parameters.AddWithValue("@createdBy", "Admin");
					cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
					cmd.ExecuteNonQuery();
					cmd.Parameters.Clear();
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

        public bool SaveEventTransaction(EventTransaction transaction)
        {
            try
            {
                var query = "insert into finance.ExpensesTransaction (EventDatesID,TransferedAmount,TransactionDate,TransactionID,ExpenseRecipientID,CreatedBy,CreatedDate,notes) " +
                    "values (@EventDatesID,@TransferedAmount,@TransactionDate,@TransactionID,@ExpenseRecipientID,@CreatedBy,@CreatedDate,@notes)";
                var cmd = dbmanager.GetSqlCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EventDatesID", transaction.EventDatesID);
                cmd.Parameters.AddWithValue("@TransferedAmount", transaction.TransferedAmount);
                cmd.Parameters.AddWithValue("@BalanceAmount", transaction.BalanceAmount);
                cmd.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                if (transaction.ExpenseRecipientID == 0)
                    cmd.Parameters.AddWithValue("@ExpenseRecipientID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ExpenseRecipientID", transaction.ExpenseRecipientID);
                cmd.Parameters.AddWithValue("@CreatedBy", "Admin");
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@notes", (transaction.notes)??"");
                var rows = dbmanager.ExecuteNonQuery(cmd);
                return rows >= 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public DataTable GetEventTransactions(int eventdatesId)
        {
            try
            {
                var query = $"select expt.*,expr.RecipientName as Recepient from finance.ExpensesTransaction expt left join finance.ExpenseRecipient expr on expt.ExpenseRecipientID = expr.ExpenseRecipientID where EventDatesID = {eventdatesId}";
                var cmd = dbmanager.GetSqlCommand(query);
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRecepients()
        {
            try
            {
                var query = $"select *from finance.ExpenseRecipient";
                var cmd = dbmanager.GetSqlCommand(query);
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEventEstimationSummary(int eventDatesId)
        {
            try
            {
                var query = $"select  lup.ExpenseModeOfPayment as ModeOfPayment, sum(lup.ExpenseAmount) as Amount from finance.EventExpensesEstimateLookup lup join Finance.EventExpensesEstimate est on lup.EventExpensesEstimateLookupID = est.EventExpensesEstimateLookupID where est.IsActive = 1 and est.EventDatesID=@EventDatesID group by lup.ExpenseModeOfPayment";
                var cmd = dbmanager.GetSqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@EventDatesID", eventDatesId);
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEventDetails(int eventDatesId)
        {
            try
            {
                var query = $"select dat.Id,dat.FromDate,dat.ToDate,dat.AvailableSlots,dat.BookedSlots,detail.Title,detail.EventType,detail.City,detail.NoOfSlots from EventDates dat join EventDetails detail on dat.id = detail.id where dat.id = {eventDatesId}";
                var cmd = dbmanager.GetSqlCommand();
                cmd.CommandText = query;
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEventEstimation(int eventDatesId)
        {
            try
            {
                var query = $"SELECT estlkp.ExpenseTypeSource,estlkp.SlabRange,estlkp.ExpenseAmount,estlkp.ExpenseModeOfPayment,estlkp.Notes,eexpt.ExpensesType,eexpt.ExpenseTypeCode FROM finance.EventExpensesEstimate eest JOIN finance.EventExpensesEstimateLookup estlkp on eest.EventExpensesEstimateLookupID = estlkp.EventExpensesEstimateLookupID JOIN finance.EventExpensesType eexpt on estlkp.ExpensesTypeid = eexpt.ExpensesTypeid WHERE EventDatesID = {eventDatesId} AND IsActive = 1";
                var cmd = dbmanager.GetSqlCommand();
                cmd.CommandText = query;
                var dt = dbmanager.GetDataTableResult(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
