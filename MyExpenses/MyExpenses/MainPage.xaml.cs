using MyExpenses.Class;
using Newtonsoft.Json;
using RestSharp;
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
    public partial class MainPage : ContentPage
    {
        public static string _UserId, _FamilyName, _FamilyId;
        public MainPage(string UserName,string UserId,string ProfileImage)
        {
            InitializeComponent();
            try
            {
                _UserId = UserId;
                _FamilyId = Application.Current.Properties["FamilyId"].ToString();
                _FamilyName = Application.Current.Properties["FamilyName"].ToString();
                if (_FamilyName != string.Empty)
                {
                    FamilyNames.Text = _FamilyName;
                    FamilyNames.FontSize = 20;
                }
                else
                {
                    FamilyNames.Text = "Giderlerim";
                    FamilyNames.FontSize = 30;
                }
                Salary.Text = Application.Current.Properties["Salary"].ToString() + " ₺";
                Avatar.Source = ProfileImage;
                getMyExpensesNameList();
                AllDataFistRun();
            }
            catch(Exception)
            {

            }
        }

        void AllDataFistRun()
        {
            try
            {
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesTransactionsAll");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8;");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +
                @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +
                @"  <soap:Body>" + "\n" +
                @"    <getMyExpensesTransactionsAll xmlns=""http://tempuri.org/"">" + "\n" +
                @"      <UserId>" + _UserId + "</UserId>" + "\n" +
                @"      <ExpensesType>Tümü</ExpensesType>" + "\n" +
                @"      <StartDate>" + StartDate.Date.ToString("yyyy-MM-dd") + "</StartDate>" + "\n" +
                @"      <EndDate>" + EndDate.Date.ToString("yyyy-MM-dd") + "</EndDate>" + "\n" +
                @"      <FamilyId>" + _FamilyId + "</FamilyId>" + "\n" +
                @"    </getMyExpensesTransactionsAll>" + "\n" +
                @"  </soap:Body>" + "\n" +
                @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8;", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                string MyExpensesNameTransactionListData = response.Content.Substring(0, response.Content.Length - 314);
                //DisplayAlert("", MyExpensesNameTransactionListData, "Tamam");
                var MyExpensesNameListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesTransactionsClass>>(MyExpensesNameTransactionListData);
                TransactionList.FlowItemsSource = MyExpensesNameListDataDetails;

                double total = 0;
                for (int a = 0; a < MyExpensesNameListDataDetails.Count; a++)
                {
                    total += Convert.ToDouble(MyExpensesNameListDataDetails[a].ExpensesAmount);
                }
                CurrentMonthExpenses.Text = total.ToString("0.00") + " ₺";

                double CurrentMonthSalary, CurrentMonthExpense, CurrentMonthBalances;
                CurrentMonthSalary = Convert.ToDouble(Application.Current.Properties["Salary"].ToString());
                CurrentMonthExpense = Convert.ToDouble(total);
                CurrentMonthBalances = CurrentMonthSalary - CurrentMonthExpense;
                CurrentMonthBalance.Text = CurrentMonthBalances.ToString("0.00") + " ₺";
            }
            catch (Exception)
            {
                TransactionList.FlowItemsSource = null;
            }
        }


        private void Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        void getMyExpensesNameList()
        {
            try
            {
                ExpensesItemList.Children.Clear();
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesNameList");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" + @"<soap:Body>" + "\n" + @"<getMyExpensesNameList xmlns=""http://tempuri.org/"">" + "\n" + @"<UserId>" + _UserId + "</UserId>" + "\n" + @"<FamilyId>" + _FamilyId + "</FamilyId>" + "\n" + @"</getMyExpensesNameList>" + "\n" + @"</soap:Body>" + "\n" + @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string MyExpensesNameListData = response.Content.Substring(0, response.Content.Length - 307);
                var MyExpensesNameListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesNameListClass>>(MyExpensesNameListData);

                for (int a = 0; a < MyExpensesNameListDataDetails.Count; a++)
                {
                    Button btn = new Button();
                    btn.Text = MyExpensesNameListDataDetails[a].ExpensesName;
                    btn.StyleId = MyExpensesNameListDataDetails[a].Id;
                    btn.CornerRadius = 15;
                    btn.BackgroundColor = Color.FromHex(MyExpensesNameListDataDetails[a].BackgroundColor);
                    btn.TextColor = Color.FromHex(MyExpensesNameListDataDetails[a].ForeColor);
                    btn.FontFamily = "Antipasto.otf#Antipasto";
                    btn.FontAttributes = FontAttributes.Bold;
                    btn.FontSize = 15;
                    btn.TextTransform = TextTransform.None;
                    btn.Clicked += Btn_Clicked;
                    ExpensesItemList.Children.Add(btn);
                }
            }
            catch(Exception)
            {

            }
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            try
            {
                string ExpensesType = ((Button)sender).Text;
                string ExpensesId = ((Button)sender).StyleId;
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesTransactions");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8;");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +
                @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +
                @"  <soap:Body>" + "\n" +
                @"    <getMyExpensesTransactions xmlns=""http://tempuri.org/"">" + "\n" +
                @"      <UserId>"+_UserId+"</UserId>" + "\n" +
                @"      <ExpensesId>"+ ExpensesId + "</ExpensesId>" + "\n" +
                @"      <StartDate>"+StartDate.Date.ToString("yyyy-MM-dd")+"</StartDate>" + "\n" +
                @"      <EndDate>" + EndDate.Date.ToString("yyyy-MM-dd") + "</EndDate>" + "\n" +
                @"      <FamilyId>"+_FamilyId+"</FamilyId>" + "\n" +
                @"    </getMyExpensesTransactions>" + "\n" +
                @"  </soap:Body>" + "\n" +
                @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8;", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                string MyExpensesNameTransactionListData = response.Content.Substring(0, response.Content.Length - 311);
                //DisplayAlert("", MyExpensesNameTransactionListData,"Tamam");
                var MyExpensesNameListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesTransactionsClass>>(MyExpensesNameTransactionListData);
                TransactionList.FlowItemsSource = MyExpensesNameListDataDetails;

                double total = 0;
                for (int a = 0; a < MyExpensesNameListDataDetails.Count; a++)
                {
                    total += Convert.ToDouble(MyExpensesNameListDataDetails[a].ExpensesAmount);
                }
                CurrentMonthExpenses.Text = total.ToString("0.00") + " ₺";

                double CurrentMonthSalary, CurrentMonthExpense, CurrentMonthBalances;
                CurrentMonthSalary = Convert.ToDouble(Application.Current.Properties["Salary"].ToString());
                CurrentMonthExpense = Convert.ToDouble(total);
                CurrentMonthBalances = CurrentMonthSalary - CurrentMonthExpense;
                CurrentMonthBalance.Text = CurrentMonthBalances.ToString("0.00") + " ₺";
            }
            catch(Exception ex)
            {
                TransactionList.FlowItemsSource = null;
            }
        }

        private async void AddExpense_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new ProcessTypeDialogPage(_UserId));
        }
        private void TransactionList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void Avatar_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("","Resim","Tamam");
        }

        private void AllData_Clicked(object sender, EventArgs e)
        {
            try
            {
                string ExpensesType = ((Button)sender).Text;
                string ExpensesId = ((Button)sender).StyleId;
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesTransactionsAll");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8;");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +
                @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +
                @"  <soap:Body>" + "\n" +
                @"    <getMyExpensesTransactionsAll xmlns=""http://tempuri.org/"">" + "\n" +
                @"      <UserId>"+_UserId+"</UserId>" + "\n" +
                @"      <ExpensesType>Tümü</ExpensesType>" + "\n" +
                @"      <StartDate>" + StartDate.Date.ToString("yyyy-MM-dd") + "</StartDate>" + "\n" +
                @"      <EndDate>" + EndDate.Date.ToString("yyyy-MM-dd") + "</EndDate>" + "\n" +
                @"      <FamilyId>"+_FamilyId+"</FamilyId>" + "\n" +
                @"    </getMyExpensesTransactionsAll>" + "\n" +
                @"  </soap:Body>" + "\n" +
                @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8;", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                string MyExpensesNameTransactionListData = response.Content.Substring(0, response.Content.Length - 314);
                //DisplayAlert("", MyExpensesNameTransactionListData, "Tamam");
                var MyExpensesNameListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesTransactionsClass>>(MyExpensesNameTransactionListData);
                TransactionList.FlowItemsSource = MyExpensesNameListDataDetails;

                double total = 0;
                for (int a = 0; a < MyExpensesNameListDataDetails.Count; a++)
                {
                    total += Convert.ToDouble(MyExpensesNameListDataDetails[a].ExpensesAmount);
                }
                CurrentMonthExpenses.Text = total.ToString("0.00") + " ₺";

                double CurrentMonthSalary, CurrentMonthExpense, CurrentMonthBalances;
                CurrentMonthSalary = Convert.ToDouble(Application.Current.Properties["Salary"].ToString());
                CurrentMonthExpense = Convert.ToDouble(total);
                CurrentMonthBalances = CurrentMonthSalary - CurrentMonthExpense;
                CurrentMonthBalance.Text = CurrentMonthBalances.ToString("0.00") + " ₺";
            }
            catch (Exception)
            {
                TransactionList.FlowItemsSource = null;
            }
        }

        private void SwipeItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                string SelectedExpenses = ((SwipeItem)sender).AutomationId;
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=DeleteAddExpensesTransaction");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +@"  <soap:Body>" + "\n" +@"<DeleteAddExpensesTransaction xmlns=""http://tempuri.org/"">" + "\n" +@"<ExpensesTransacitonId>"+ SelectedExpenses + "</ExpensesTransacitonId>" + "\n" +@"<UserId>"+_UserId+"</UserId>" + "\n" +@"</DeleteAddExpensesTransaction>" + "\n" +@"</soap:Body>" + "\n" +@"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                var client1 = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getMyExpensesTransactions");
                client1.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body1 = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" + @" <soap:Body>" + "\n" + @"<getMyExpensesTransactions xmlns=""http://tempuri.org/"">" + "\n" + @"<UserId>" + _UserId + "</UserId>" + "\n" + @"<ExpensesType>Tümü</ExpensesType>" + "\n" + @"<ExpensesId></ExpensesId>" + "\n" + @"<StartDate>" + StartDate.Date.ToString("yyyy-MM-dd") + "</StartDate>" + "\n" + @"<EndDate>" + EndDate.Date.ToString("yyyy-MM-dd") + "</EndDate>" + "\n" + @"</getMyExpensesTransactions>" + "\n" + @"</soap:Body>" + "\n" + @"</soap:Envelope>";
                request1.AddParameter("text/xml; charset=utf-8", body1, ParameterType.RequestBody);
                IRestResponse response1 = client1.Execute(request1);

                string MyExpensesNameTransactionListData = response1.Content.Substring(0, response1.Content.Length - 311);
                //DisplayAlert("", MyExpensesNameTransactionListData,"Tamam");
                var MyExpensesNameListDataDetails = JsonConvert.DeserializeObject<List<getMyExpensesTransactionsClass>>(MyExpensesNameTransactionListData);
                TransactionList.FlowItemsSource = MyExpensesNameListDataDetails;

                double total = 0;
                for (int a = 0; a < MyExpensesNameListDataDetails.Count; a++)
                {
                    total += Convert.ToDouble(MyExpensesNameListDataDetails[a].ExpensesAmount);
                }
                CurrentMonthExpenses.Text = total.ToString("0.00") + " ₺";

                double CurrentMonthSalary, CurrentMonthExpense, CurrentMonthBalances;
                CurrentMonthSalary = Convert.ToDouble(Application.Current.Properties["Salary"].ToString());
                CurrentMonthExpense = Convert.ToDouble(total);
                CurrentMonthBalances = CurrentMonthSalary - CurrentMonthExpense;
                CurrentMonthBalance.Text = CurrentMonthBalances.ToString("0.00") + " ₺";
            }
            catch (Exception ex)
            {
                TransactionList.FlowItemsSource = null;
            }
        }
    }
}