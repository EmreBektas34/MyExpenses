using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace MyExpensesApi
{
    /// <summary>
    /// MyExpensesApiServices için özet açıklama
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Bu Web Hizmeti'nin, ASP.NET AJAX kullanılarak komut dosyasından çağrılmasına, aşağıdaki satırı açıklamadan kaldırmasına olanak vermek için.
    // [System.Web.Script.Services.ScriptService]
    public class MyExpensesApiServices : System.Web.Services.WebService
    {
        private static String conn = "Data Source=192.168.1.102; User Id=sa; Password=Emre1992; Database=MyExpenses;";
        SqlConnection con = new SqlConnection(conn);
        DataTable dt;
        SqlCommand cmd;
        SqlDataReader reader;


        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getMyExpensesTransactions(string UserId,string ExpensesId, DateTime StartDate,DateTime EndDate,string FamilyId)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                if(ExpensesId.ToString() != string.Empty && FamilyId.ToString()!=string.Empty)
                {
                    cmd = new SqlCommand("Select ExpensesList.ExpensesName,ExpensesList.ForeColor,ExpensesTransactions.Id,ExpensesTransactions.ExpensesId,ExpensesTransactions.ExpensesDescription,ExpensesTransactions.ExpensesAmount,substring(Convert(nvarchar,ExpensesTransactions.Date,102),0,11) as Date,ExpensesTransactions.Time,ExpensesTransactions.ExpensesTransactionType,ExpensesTransactions.MonthName,Users.UserName FROM ExpensesTransactions inner join ExpensesList on ExpensesList.Id=ExpensesTransactions.ExpensesId inner join Users on Users.Id=ExpensesTransactions.UserId where ExpensesTransactions.UserId='" + UserId + "' and ExpensesTransactions.FamilyId='" + FamilyId + "' and ExpensesTransactions.ExpensesId='" + ExpensesId + "' and Date>='" + StartDate.Date.ToString("yyyy-MM-dd") + "' and Date<='" + EndDate.Date.ToString("yyyy-MM-dd") + "'", con);
                }

                else if (ExpensesId.ToString() != string.Empty && FamilyId.ToString() == string.Empty)
                {
                    cmd = new SqlCommand("Select ExpensesList.ExpensesName,ExpensesList.ForeColor,ExpensesTransactions.Id,ExpensesTransactions.ExpensesId,ExpensesTransactions.ExpensesDescription,ExpensesTransactions.ExpensesAmount,substring(Convert(nvarchar,ExpensesTransactions.Date,102),0,11) as Date,ExpensesTransactions.Time,ExpensesTransactions.ExpensesTransactionType,ExpensesTransactions.MonthName,Users.UserName FROM ExpensesTransactions inner join ExpensesList on ExpensesList.Id=ExpensesTransactions.ExpensesId inner join Users on Users.Id=ExpensesTransactions.UserId where ExpensesTransactions.UserId='" + UserId + "' and Date>='" + StartDate.Date.ToString("yyyy-MM-dd") + "' and Date<='" + EndDate.Date.ToString("yyyy-MM-dd") + "'", con);
                }
                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getMyExpensesTransactionsAll(string UserId, string ExpensesType, DateTime StartDate, DateTime EndDate, string FamilyId)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                if (ExpensesType.ToString() =="Tümü" && FamilyId.ToString() != string.Empty)
                {
                    cmd = new SqlCommand("Select ExpensesList.ExpensesName,ExpensesList.ForeColor,ExpensesTransactions.Id,ExpensesTransactions.ExpensesId,ExpensesTransactions.ExpensesDescription,ExpensesTransactions.ExpensesAmount,substring(Convert(nvarchar,ExpensesTransactions.Date,102),0,11) as Date,ExpensesTransactions.Time,ExpensesTransactions.ExpensesTransactionType,ExpensesTransactions.MonthName,Users.UserName FROM ExpensesTransactions inner join ExpensesList on ExpensesList.Id=ExpensesTransactions.ExpensesId inner join Users on Users.Id=ExpensesTransactions.UserId where ExpensesTransactions.UserId='" + UserId + "' and ExpensesTransactions.FamilyId='" + FamilyId + "' and Date>='" + StartDate.Date.ToString("yyyy-MM-dd") + "' and Date<='" + EndDate.Date.ToString("yyyy-MM-dd") + "'", con);
                }

                else if (ExpensesType.ToString() == "Tümü" && FamilyId.ToString() == string.Empty)
                {
                    cmd = new SqlCommand("Select ExpensesList.ExpensesName,ExpensesList.ForeColor,ExpensesTransactions.Id,ExpensesTransactions.ExpensesId,ExpensesTransactions.ExpensesDescription,ExpensesTransactions.ExpensesAmount,substring(Convert(nvarchar,ExpensesTransactions.Date,102),0,11) as Date,ExpensesTransactions.Time,ExpensesTransactions.ExpensesTransactionType,ExpensesTransactions.MonthName,Users.UserName FROM ExpensesTransactions inner join ExpensesList on ExpensesList.Id=ExpensesTransactions.ExpensesId inner join Users on Users.Id=ExpensesTransactions.UserId where ExpensesTransactions.UserId='" + UserId + "' and Date>='" + StartDate.Date.ToString("yyyy-MM-dd") + "' and Date<='" + EndDate.Date.ToString("yyyy-MM-dd") + "'", con);
                }
                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getMyExpensesNameList(string UserId,string FamilyId)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                if(FamilyId!=string.Empty)
                {
                    cmd = new SqlCommand("Select * From ExpensesList where UserId='" + UserId + "' and FamilyId='"+FamilyId+"'", con);
                }
                else if(FamilyId == string.Empty)
                {
                    cmd = new SqlCommand("Select * From ExpensesList where UserId='" + UserId + "'", con);
                }
                else if (FamilyId == "4")
                {
                    cmd = new SqlCommand("Select * From ExpensesList where UserId='" + UserId + "'", con);
                }

                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getUsers(string UserName, string Password)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                cmd = new SqlCommand("Select Users.Id,Users.UserName,Users.Password,Users.PasswordAgain,Users.Image,Users.Salary,Users.Status,Users.FamilyId,Users.DeviceId,FamilyGroupName.FamilyName from Users inner join FamilyGroupName on Users.FamilyId=FamilyGroupName.Id where UserName='" + UserName+ "' and Password='" + Password + "'", con);
                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getExpensesId(string UserId, string ExpensesName)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                cmd = new SqlCommand("Select Id from ExpensesList where UserId='" + UserId + "' and ExpensesName='" + ExpensesName + "'", con);
                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getCurrentMonthTotals(string MonthName)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
            String resultJSON = "";
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                con.Open();
                cmd = new SqlCommand("Select Sum(ExpensesAmount) as ExpensesAmount from ExpensesTransactions where MonthName='"+MonthName+"'", con);
                reader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(reader);
                con.Close();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<String, Object>> tableRows = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    tableRows.Add(row);
                }
                resultJSON = serializer.Serialize(tableRows).ToString();
                if (resultJSON == "[]")
                {
                    Context.Response.Write("[{\"UserName\":\"*\"]");
                }
                else
                {
                    Context.Response.Write(resultJSON);
                }

            }
            catch (Exception)
            {
                resultJSON = "*";
                Context.Response.Write(resultJSON);
            }
        }


        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void InsertAddExpenses(string ExpensesName, string ForeColor, string UserId, string BackgroundColor)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "insert into ExpensesList(ExpensesName,ForeColor,UserId,BackgroundColor) values (@ExpensesName,@ForeColor,@UserId,@BackgroundColor)";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@ExpensesName", ExpensesName);
                komut.Parameters.AddWithValue("@ForeColor", ForeColor);
                komut.Parameters.AddWithValue("@UserId", UserId);
                komut.Parameters.AddWithValue("@BackgroundColor", BackgroundColor);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Insert To Expenses");
            }
            catch (Exception)
            {
                Context.Response.Write("Delete To Expenses is declined");
            }
        }


        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteAddExpenses(string ExpensesId,string UserId)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "Delete from ExpensesList where Id='"+ ExpensesId + "' and UserId='"+ UserId + "'";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Delete To Expenses");
            }
            catch (Exception)
            {
                Context.Response.Write("Delete To Expenses is declined");
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void UpdateAddExpenses(string ExpensesId, string ExpensesName, string ForeColor, string UserId, string BackgroundColor)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "Update ExpensesList set ExpensesName=@ExpensesName,ForeColor=@ForeColor,BackgroundColor=@BackgroundColor where Id='"+ ExpensesId + "' and UserId='"+ UserId + "'";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@ExpensesName", ExpensesName);
                komut.Parameters.AddWithValue("@ForeColor", ForeColor);
                komut.Parameters.AddWithValue("@BackgroundColor", BackgroundColor);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Update To Expenses");
            }
            catch (Exception)
            {
                Context.Response.Write("Update To Expenses is declined");
            }
        }


        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void InsertAddExpensesTransaction(string ExpensesId, string ExpensesDescription, string ExpensesAmount, DateTime Date, string Time, string ExpensesTransactionType, string UserId, string MonthName)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "insert into ExpensesTransactions(ExpensesId,ExpensesDescription,ExpensesAmount,Date,Time,ExpensesTransactionType,UserId,MonthName) values (@ExpensesId,@ExpensesDescription,@ExpensesAmount,@Date,@Time,@ExpensesTransactionType,@UserId,@MonthName)";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@ExpensesId", ExpensesId);
                komut.Parameters.AddWithValue("@ExpensesDescription", ExpensesDescription);
                komut.Parameters.AddWithValue("@ExpensesAmount", Convert.ToDouble(ExpensesAmount));
                komut.Parameters.AddWithValue("@Date", Date.Date.ToString("yyyy-MM-dd"));
                komut.Parameters.AddWithValue("@Time", Time);
                komut.Parameters.AddWithValue("@ExpensesTransactionType", ExpensesTransactionType);
                komut.Parameters.AddWithValue("@UserId", UserId);
                komut.Parameters.AddWithValue("@MonthName", MonthName);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Insert To ExpensesTransaction");
            }
            catch (Exception)
            {
                Context.Response.Write("Insert To ExpensesTransaction is declined");
            }
        }



        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void DeleteAddExpensesTransaction(string ExpensesTransacitonId,string UserId)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "Delete from ExpensesTransactions where Id='"+ ExpensesTransacitonId + "' and UserId='"+ UserId + "'";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Delete To ExpensesTransaction");
            }
            catch (Exception)
            {
                Context.Response.Write("Delete To ExpensesTransaction is declined");
            }
        }



        [WebMethod(EnableSession = false)]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void InsertRegisterUsersDetails(string UserName, string Password, string PasswordAgain, string Image, string Salary,string Status, string FamilyId,string DeviceId)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(conn);
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string kayit = "insert into Users(UserName,Password,PasswordAgain,Image,Salary,Status,FamilyId,DeviceId) values (@UserName,@Password,@PasswordAgain,@Image,@Salary,@Status,@FamilyId,@DeviceId)";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@UserName", UserName);
                komut.Parameters.AddWithValue("@Password", Password);
                komut.Parameters.AddWithValue("@PasswordAgain", PasswordAgain);
                komut.Parameters.AddWithValue("@Image", Image);
                komut.Parameters.AddWithValue("@Salary", Convert.ToDouble(Salary));
                komut.Parameters.AddWithValue("@Status", Status);
                komut.Parameters.AddWithValue("@FamilyId", FamilyId);
                komut.Parameters.AddWithValue("@DeviceId", DeviceId);
                komut.ExecuteNonQuery();
                baglanti.Close();
                Context.Response.Write("Insert To Register");
            }
            catch (Exception)
            {
                Context.Response.Write("Insert To Register is declined");
            }
        }

    }
}
