using iSpanFinalApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iSpanFinalApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class Login : ContentPage
    {
        public Uri thiscompurl = new Uri("http://10.0.2.2:44455/");
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string pw = txtPW.Text;
            if (id == "" || pw == "")
            {
                await DisplayAlert("登入失敗", "帳號密碼不可為空白。", "OK");
                return;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.GetStringAsync($"/MsgApi/LoginCheck?txtAccount={id}&txtPW={pw}");
            var member = JsonConvert.DeserializeObject<CAppMember>(result);
            if (member == null)
            {
                await DisplayAlert("登入失敗", "帳號密碼錯誤，請重新輸入。", "OK");
                txtID.Text = txtPW.Text = "";
            }
            else
            {
                await DisplayAlert("登入成功", "帳號密碼正確，成功登入。", "OK");
                await Navigation.PushAsync(new Order(member));
            }
        }

        private void btnDemo1(object sender, EventArgs e)
        {
            txtID.Text = "peko9487";
            txtPW.Text = "peko9487";
        }
        private void btnDemo2(object sender, EventArgs e)
        {
            txtID.Text = "maririn";
            txtPW.Text = "maririn";
        }
        private void btnDemo3(object sender, EventArgs e)
        {
            txtID.Text = "subalove99";
            txtPW.Text = "subalove99";
        }
        private void btnDemo4(object sender, EventArgs e)
        {
            txtID.Text = "shirannfla";
            txtPW.Text = "shirannfla";
        }

    }
}