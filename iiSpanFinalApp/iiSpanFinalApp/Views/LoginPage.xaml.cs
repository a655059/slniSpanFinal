using iiSpanFinalApp.ViewModels;
using Newtonsoft.Json;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iiSpanFinalApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        //MainPage mp;
        //CItemVM vm;
        int targetFlag = 0;
        //public Login(int tFlag)
        //{
        //    InitializeComponent();
        //    mp = Application.Current.Properties[CDictionary.mainPage] as MainPage;
        //    vm = mp.vm;
        //    targetFlag = tFlag;
        //}

        private async void btnLogin(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string pw = txtPW.Text;
            if (id == "" || pw == "")
            {
                await DisplayAlert("錯誤", "帳號密碼不可為空白。", "OK");
                return;
            }

            HttpClient client = new HttpClient();
            var result = await client.GetStringAsync($"https://localhost:5000/MsgApi?txtAccount={id}&txtPW={pw}");
            var mem = JsonConvert.DeserializeObject<CAppMember>(result);
            if (mem == null)
            {
                await DisplayAlert("錯誤", "帳號密碼有誤，請重新輸入。", "OK");
                txtID.Text = txtPW.Text = "";
            }
            else
            {
                Application.Current.Properties[CDictionary.SK_LOGINED_USER] = mem;
                mp.user = mem;
                Navigation.RemovePage(this);

                if (targetFlag == 1)
                    vm.ShowCartPage();
                else if (targetFlag == 2)
                    await Navigation.PushAsync(new Member());
                //else if (targetFlag == 0)
                //    await Navigation.PopAsync();
            }
        }

        private void btnDemo(object sender, EventArgs e)
        {
            txtID.Text = "0966666666";
            txtPW.Text = "0966666666";
        }
    }
}