using MyExpenses.Class;
using Newtonsoft.Json;
using RestSharp;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyExpenses
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddExpensesTransactionPage : PopupPage
	{
        public static string _UserId;
		public AddExpensesTransactionPage (string UserId)
		{
			InitializeComponent ();
            _UserId = UserId;
            getAddExpenseList();
        }
        void getAddExpenseList()
        {
            try
            { 
            ExpensesList.Items.Clear();
            var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesNameList");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/xml; charset=utf-8");
            var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" + @"<soap:Body>" + "\n" + @"<getMyExpensesNameList xmlns=""http://tempuri.org/"">" + "\n" + @"<UserId>" + _UserId + "</UserId>" + "\n" + @"</getMyExpensesNameList>" + "\n" + @"</soap:Body>" + "\n" + @"</soap:Envelope>";
            request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string AddExpenseListData = response.Content.Substring(0, response.Content.Length - 307);

            //DisplayAlert("", AddExpenseListData, "Tamam");
            var AddExpenseListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesNameListClass>>(AddExpenseListData);
            for(int a=0; a< AddExpenseListDataDetails.Count; a++)
            {
                ExpensesList.Items.Add(AddExpenseListDataDetails[a].ExpensesName);
            }
            }
            catch (Exception)
            {

            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BackColors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ForeColors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void Ok_Clicked(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=InsertAddExpensesTransaction");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" + @"  <soap:Body>" + "\n" + @"<InsertAddExpensesTransaction xmlns=""http://tempuri.org/"">" + "\n" + @"<ExpensesId>" + SelectedExpensesNameId.Text + "</ExpensesId>" + "\n" + @"<ExpensesDescription>" + Description.Text + "</ExpensesDescription>" + "\n" + @"<ExpensesAmount>" + Convert.ToDouble(TotalAmount.Text) + "</ExpensesAmount>" + "\n" + @"<Date>" + DateTime.Now.ToString("yyyy-MM-dd") + "</Date>" + "\n" + @"<Time>" + DateTime.Now.ToShortTimeString() + "</Time>" + "\n" + @"<ExpensesTransactionType>OutComing</ExpensesTransactionType>" + "\n" + @"<UserId>" + _UserId + "</UserId>" + "\n" + @"<MonthName>" + DateTime.Now.ToString("MM") + "</MonthName>" + "\n" + @"</InsertAddExpensesTransaction>" + "\n" + @"</soap:Body>" + "\n" + @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch(Exception)
            {

            }
        }

        private void ExpensesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
            var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getExpensesId");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/xml; charset=utf-8");
            var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +@"<soap:Body>" + "\n" +@"<getExpensesId xmlns=""http://tempuri.org/"">" + "\n" +@"<UserId>"+_UserId+"</UserId>" + "\n" +@"<ExpensesName>"+ ExpensesList.SelectedItem.ToString() + "</ExpensesName>" + "\n" +@"</getExpensesId>" + "\n" +@"</soap:Body>" + "\n" +@"</soap:Envelope>";
            request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string AddExpenseListData = response.Content.Substring(0, response.Content.Length - 299);

            var AddExpenseListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesNameListClass>>(AddExpenseListData);
            SelectedExpensesNameId.Text = AddExpenseListDataDetails[0].Id;
            }
            catch (Exception)
            {

            }
        }
    }
}