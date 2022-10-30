using iSpanFinalApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iSpanFinalApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Order : ContentPage
    {
        public Uri thiscompurl = new Uri("http://10.0.2.2:44455/");
        public CAppMember mem;
        public Order(CAppMember member)
        {
            InitializeComponent();
            mem = member;
            formload();


        }
        private async void formload()
        {
            Stream stream = new MemoryStream(mem.MemberPic);
            var imageSource = ImageSource.FromStream(() => stream);
            Mempic.Source = imageSource;
            MemAcc.Text = mem.MemberAcc;
            Memname.Text = mem.MemberName;

            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.GetStringAsync($"/MsgApi/GetOrders?id={mem.MemberId}");
            var ods = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(result);
            listItem.ItemsSource = ods;

        }
    }
}