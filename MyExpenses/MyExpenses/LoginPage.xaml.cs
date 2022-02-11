using MyExpenses.Class;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace MyExpenses
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static string Images;
        public LoginPage()
        {
            InitializeComponent();
            getUsersImage();
            getLocalStrogeUserName();
        }

        void getLocalStrogeUserName()
        {
            try
            {
                UserName.Text = Application.Current.Properties["UserName"].ToString();
                if (UserName.Text!=string.Empty)
                {
                    UserName.Text = Application.Current.Properties["UserName"].ToString();
                    RememberMe.IsToggled = true;
                }
                else
                {
                    UserName.Text = "";
                    RememberMe.IsToggled = false;
                }
            }
            catch (Exception)
            {
                UserName.Text = "";
            }
        }
        void getUsersImage()
        {
            try
            {
                Images = Application.Current.Properties["Image"].ToString();
                ProfileImage.Source = Images;
            }
            catch(Exception)
            {
                ProfileImage.Source = "https://bsic.it/wp-content/uploads/2020/08/Noprofilepic.png";
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            Waiting.IsVisible = true;
            Waiting.IsRunning = true;
            Login.IsEnabled = false;
            Register.IsEnabled = false;
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            try
            {
                var client = new RestClient("http://192.168.1.102:14335/MyExpensesApiServices.asmx?op=getUsers");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml; charset=utf-8");
                var body = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" +@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">" + "\n" +@"<soap:Body>" + "\n" +@"<getUsers xmlns=""http://tempuri.org/"">" + "\n" +@"<UserName>"+UserName.Text+"</UserName>" + "\n" +@"<Password>"+Password.Text+"</Password>" + "\n" +@"</getUsers>" + "\n" +@"</soap:Body>" + "\n" +@"</soap:Envelope>";
                request.AddParameter("text/xml; charset=utf-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string UserData = response.Content.Substring(0, response.Content.Length - 294);

                var UserDetails = JsonConvert.DeserializeObject<List<getUsersClass>>(UserData);        

                if (UserName.Text == UserDetails[0].UserName)
                {
                    if(UserDetails[0].Status=="1")//Kullancı Onaylanmış
                    {
                        Application.Current.Properties["Image"] = UserDetails[0].Image;
                        Application.Current.Properties["Salary"] = UserDetails[0].Salary;
                        Application.Current.Properties["FamilyName"] = UserDetails[0].FamilyName;
                        Application.Current.Properties["FamilyId"] = UserDetails[0].FamilyId;
                        await Navigation.PushModalAsync(new MainPage(UserDetails[0].UserName, UserDetails[0].Id, UserDetails[0].Image));

                        Login.IsEnabled = true;
                        Register.IsEnabled = true;

                        Waiting.IsVisible = false;
                        Waiting.IsRunning = false;
                    }
                    else if(UserDetails[0].Status == "0")//Kullanıcı Onay Aşamasında
                    {
                        await DisplayAlert("Bilgi", "Kullanıcı bilgileriniz onay aşamasındadır en yakın zamanda onaylanacaktır.", "Tamam");
                        Waiting.IsVisible = false;
                        Waiting.IsRunning = false;
                        Login.IsEnabled = true;
                        Register.IsEnabled = true;
                    }
                    else if (UserDetails[0].Status == "2")//Kullanıcı İptal Edilmiş
                    {
                        await DisplayAlert("Bilgi", "Kullanıcı bilgileriniz iptal edilmiş.", "Tamam");
                        Waiting.IsVisible = false;
                        Waiting.IsRunning = false;
                        Login.IsEnabled = true;
                        Register.IsEnabled = true;
                    }
                }
            }
            catch(Exception)
            {
                await DisplayAlert("Bilgi", "Kullanıcı bilgileri hatalı !!", "Tamam");
                Waiting.IsVisible = false;
                Waiting.IsRunning = false;
                Login.IsEnabled = true;
                Register.IsEnabled = true;
            }
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["Image"] ="";
           await Navigation.PushModalAsync(new RegisterPage());
        }

 
        private void RememberMe_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
          
        }

        private void RememberMe_Toggled(object sender, ToggledEventArgs e)
        {
            if (RememberMe.IsToggled == true)
            {
                Application.Current.Properties["UserName"] = UserName.Text;
            }
            else
            {
                Application.Current.Properties["UserName"] = "";
            }
        }
    }
}