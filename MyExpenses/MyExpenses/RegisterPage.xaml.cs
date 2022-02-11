using Plugin.DeviceInfo;
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
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();
			DeviceId.Text = CrossDeviceInfo.Current.Id;
			Images.Items.Clear();
			Images.Items.Add("Avatar1");
			Images.Items.Add("Avatar2");
			Images.Items.Add("Avatar3");
			Images.Items.Add("Avatar4");
			Images.Items.Add("Avatar5");
			Images.Items.Add("Avatar6");
			Images.Items.Add("Avatar7");
			Images.Items.Add("Avatar8");
			Images.Items.Add("Avatar9");
			Images.Items.Add("Avatar10");
			Images.Items.Add("Avatar11");
			Images.Items.Add("Avatar12");
		}

        private void Back_Clicked(object sender, EventArgs e)
        {
			Navigation.PopModalAsync();
        }

        private void Register_Clicked(object sender, EventArgs e)
        {
			try
			{
				Application.Current.Properties["Image"] = Images.SelectedItem.ToString();
				var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=InsertRegisterUsersDetails");
				client.Timeout = -1;
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "text/xml; charset=utf-8");
				var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +@"<soap:Body>" + "\n" +@"<InsertRegisterUsersDetails xmlns=""http://tempuri.org/"">" + "\n" +@"<UserName>"+UserName.Text+"</UserName>" + "\n" +@"<Password>"+Password.Text+"</Password>" + "\n" +@"<PasswordAgain>"+PasswordAgain.Text+"</PasswordAgain>" + "\n" +@"<Image>"+Images.SelectedItem.ToString()+"</Image>" + "\n" +@"<Salary>"+Salarys.Text+"</Salary>" + "\n" + @"<Status>0</Status>" + "\n" + @"<FamilyId>"+ FamilyId .Text+ "</FamilyId>" + "\n"+ @"<DeviceId>" + DeviceId.Text + "</DeviceId>" + "\n" + @"</InsertRegisterUsersDetails>" + "\n" +@"</soap:Body>" + "\n" +@"</soap:Envelope>";
				request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);
				Console.WriteLine(response.Content);
				Navigation.PopModalAsync();
			}
			catch(Exception)
            {

            }
		}

        private void Images_SelectedIndexChanged(object sender, EventArgs e)
        {
			Avatar.Source = Images.SelectedItem.ToString();
		}
    }
}