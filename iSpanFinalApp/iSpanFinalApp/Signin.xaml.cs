using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Resource;
using Newtonsoft.Json;

namespace iSpanFinalApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signin : ContentPage
	{
        public Uri thiscompurl = new Uri("http://10.0.2.2:44455/");
        public Signin ()
		{
			InitializeComponent ();
		}

		private async void signin_Clicked(object sender, EventArgs e)
		{
			if(txtPW.Text != txtconfirmPW.Text)
			{
                await DisplayAlert("錯誤", "你輸入的兩次密碼不同", "OK");
				return;
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var re = await client.GetStringAsync($"/MsgApi/Signin?txtName={txtName.Text}&txtAccount={txtID.Text}&txtPW={txtPW.Text}&txtPhone={txtPhone.Text}&txtEmail={txtEmail.Text}");
            var st = JsonConvert.DeserializeObject<string>(re);
            if (st == "0")
            {
                await DisplayAlert("成功", "註冊成功", "OK");
                await Navigation.PopAsync();
            }
            else if(st == "1")
            {
                await DisplayAlert("失敗", "帳號已註冊", "OK");
            }
            else if(st == "2")
            {
                await DisplayAlert("失敗", "電話號碼重複", "OK");
            }
            else if (st == "3")
            {
                await DisplayAlert("失敗", "Email重複", "OK");
            }
        }
        private void btnDemo1(object sender, EventArgs e)
        {
            txtName.Text = "王大明";
            txtID.Text = "Ming900103";
            txtPW.Text = "Wang900103";
            txtconfirmPW.Text = "Wang900103";
            txtPhone.Text = "0966076245";
            txtEmail.Text = "Wang20221101@gmail.com";
        }
        private void btnDemo2(object sender, EventArgs e)
        {
            txtName.Text = "王大明";
            txtID.Text = "Apple1234";
            txtPW.Text = "Apple1234";
            txtconfirmPW.Text = "Apple1234";
            txtPhone.Text = "0966076245";
            txtEmail.Text = "Wang20221101@gmail.com";
        }
        private void btnDemo3(object sender, EventArgs e)
        {
            txtName.Text = "王大明";
            txtID.Text = "Apple1234";
            txtPW.Text = "Apple1234";
            txtconfirmPW.Text = "Apple1234";
            txtPhone.Text = "0912456987";
            txtEmail.Text = "Wang20221101@gmail.com";
        }
        private void btnDemo4(object sender, EventArgs e)
		{
            txtName.Text = "王大明";
			txtID.Text = "Apple1234";
            txtPW.Text = "Apple1234";
            txtconfirmPW.Text = "Apple1234";
			txtPhone.Text = "0912456987";
            txtEmail.Text = "Apple1234@gmail.com";
        }
	}
}