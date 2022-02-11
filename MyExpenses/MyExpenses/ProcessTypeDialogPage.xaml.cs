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
    public partial class ProcessTypeDialogPage : PopupPage
    {
        public static string _UserId;
        public ProcessTypeDialogPage(string UserId)
        {
            InitializeComponent();
            _UserId = UserId;
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void Ok_Clicked(object sender, EventArgs e)
        {

        }

        private async void AddExpensesType_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AddExpensesPage(_UserId,"Insert"));
        }

        private async void AddExpensesTypeTransaction_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AddExpensesTransactionPage(_UserId));
        }

        private async void UpdateExpensesType_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AddExpensesPage(_UserId, "Update"));
        }
    }
}