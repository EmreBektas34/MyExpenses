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
    public partial class AddExpensesPage : PopupPage
    {
        public static string _UserId, ProcessTypes;
        public AddExpensesPage(string UserId,string ProcessType)
        {
            InitializeComponent();
            _UserId = UserId;
            ProcessTypes = ProcessType;
            BackColors.Items.Clear();
            BackColors.Items.Add("Mavi");
            BackColors.Items.Add("Kırmızı");
            BackColors.Items.Add("Turuncu");
            BackColors.Items.Add("Mor");
            BackColors.Items.Add("Beyaz");

            ForeColors.Items.Clear();
            ForeColors.Items.Add("Mavi");
            ForeColors.Items.Add("Kırmızı");
            ForeColors.Items.Add("Turuncu");
            ForeColors.Items.Add("Mor");
            ForeColors.Items.Add("Beyaz");

            if(ProcessTypes=="Insert")
            {
                Ok.IsVisible = true;
                Update.IsVisible = false;
                ProcessTypess.Text = "Gider Tipi Ekle";
                Lists.IsVisible = false;
                FrameProperties.HeightRequest = 350;
                FrameProperties.MinimumHeightRequest = 350;
       
            }
            else if(ProcessTypes == "Update")
            {
                Ok.IsVisible = false;
                Update.IsVisible = true;
                ProcessTypess.Text = "Gider Tipi Güncelle";
                Lists.IsVisible = true;
                FrameProperties.HeightRequest = 500;
                FrameProperties.MinimumHeightRequest = 500;
                getAddExpenseList();
            }
        }

        void getAddExpenseList()
        {
            ExpensesList.FlowItemsSource = null;
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
            ExpensesList.FlowItemsSource = AddExpenseListDataDetails;
        }
        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void Ok_Clicked(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=InsertAddExpenses");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" + @"<soap:Body>" + "\n" + @"<InsertAddExpenses xmlns=""http://tempuri.org/"">" + "\n" + @"<ExpensesName>" + ExpensesName.Text + "</ExpensesName>" + "\n" + @"<ForeColor>" + SelectedForeColorName.Text + "</ForeColor>" + "\n" + @"<UserId>" + _UserId + "</UserId>" + "\n" + @"<BackgroundColor>" + SelectedBackColorName.Text + "</BackgroundColor>" + "\n" + @"</InsertAddExpenses>" + "\n" + @"</soap:Body>" + "\n" + @"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch(Exception)
            {

            }
        }

        private void BackColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BackColors.SelectedItem.ToString() == "Mavi")
            {
                SelectedBackColorName.Text = "#0000FF";
            }
            else if (BackColors.SelectedItem.ToString() == "Kırmızı")
            {
                SelectedBackColorName.Text = "#FF0000";
            }
            else if (BackColors.SelectedItem.ToString() == "Turuncu")
            {
                SelectedBackColorName.Text = "#FF7F00";
            }
            else if (BackColors.SelectedItem.ToString() == "Mor")
            {
                SelectedBackColorName.Text = "#660099";
            }    
            else if (BackColors.SelectedItem.ToString() == "Beyaz")
            {
                SelectedBackColorName.Text = "#FFFFFF";
            }
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=UpdateAddExpenses");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +@"<soap:Body>" + "\n" +@"<UpdateAddExpenses xmlns=""http://tempuri.org/"">" + "\n" +@"<ExpensesId>"+ExpensesId.Text+"</ExpensesId>" + "\n" +@"<ExpensesName>"+ExpensesName.Text+"</ExpensesName>" + "\n" +@"<ForeColor>"+SelectedForeColorName.Text+"</ForeColor>" + "\n" +@"<UserId>"+_UserId+"</UserId>" + "\n" +@"<BackgroundColor>"+SelectedBackColorName.Text+"</BackgroundColor>" + "\n" +@"</UpdateAddExpenses>" + "\n" +@"</soap:Body>" + "\n" +@"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch(Exception)
            {

            }
        }

        private void ExpensesList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedExpenses = (getMyExpensesNameListClass)e.Item;
            ExpensesId.Text = SelectedExpenses.Id;
            ExpensesName.Text = SelectedExpenses.ExpensesName;
            SelectedBackColorName.Text = SelectedExpenses.BackgroundColor;
            SelectedForeColorName.Text = SelectedExpenses.ForeColor;
        }

        private void ForeColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ForeColors.SelectedItem.ToString() == "Mavi")
            {
                SelectedForeColorName.Text = "#0000FF";
            }
            else if (ForeColors.SelectedItem.ToString() == "Kırmızı")
            {
                SelectedForeColorName.Text = "#FF0000";
            }
            else if (ForeColors.SelectedItem.ToString() == "Turuncu")
            {
                SelectedForeColorName.Text = "#FF7F00";
            }
            else if (ForeColors.SelectedItem.ToString() == "Mor")
            {
                SelectedForeColorName.Text = "#660099";
            }
            else if (ForeColors.SelectedItem.ToString() == "Beyaz")
            {
                SelectedForeColorName.Text = "#FFFFFF";
            }
        }
    }
}