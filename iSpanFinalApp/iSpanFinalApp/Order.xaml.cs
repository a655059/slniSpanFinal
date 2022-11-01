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
using Plugin.LocalNotification;
using Xamarin.Forms.PlatformConfiguration;
using Java.Lang.Reflect;

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
            var ods = JsonConvert.DeserializeObject<List<OrderListViewModel>>(result);
            if (ods.Count != 0)
            {
                listItem.ItemsSource = ods;
                listItem.ItemTapped += btnOD;
            }
            
            var result2 = await client.GetStringAsync($"/MsgApi/GetNewNotificationbyID?id={mem.MemberId}");
            var nos = JsonConvert.DeserializeObject<List<CNotification>>(result2);
            if (nos.Count != 0)
            {
                
                string alertstr = "";
                foreach (var item in nos)
                {
                    DisplayNotification(item.NotificationId, item.Text, item.TextContent);
                    alertstr += $"{item.Text}\n時間: {item.NotiTime}\n內文: {item.TextContent}\n";
                }
                await DisplayAlert("在你離開的時候...", alertstr, "確認");
            }
        }

        public void DisplayNotification(int id, string subtitle , string message)
        {
            var notification = new NotificationRequest
            {
                NotificationId = id,
                Title = "蝦到爆商城",
                Subtitle = subtitle,
                Description = message,
                //ReturningData = filePath, // Returning data when tapped on notification.
            };

            LocalNotificationCenter.Current.Show(notification);
        }

        private async void btnOD(object sender, ItemTappedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.GetStringAsync($"/MsgApi/GetOrderDetail?id={(e.Item as OrderListViewModel).OrderId}");
            var ods = JsonConvert.DeserializeObject<OrderDetailViewModel>(result);
            if (ods != null)
            {
                await Navigation.PushAsync(new OrderDetail(ods));
            }
        }

        private async void btnbuyer_Clicked(object sender, EventArgs e)
        {
            btnbuyer.BackgroundColor = Color.Orange;
            btnseller.BackgroundColor = Color.LightGray;
            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.GetStringAsync($"/MsgApi/GetOrders?id={mem.MemberId}");
            var ods = JsonConvert.DeserializeObject<List<OrderListViewModel>>(result);
            if (ods.Count != 0)
            {
                listItem.ItemsSource = ods;
                listItem.ItemTapped += btnOD;
            }
            else
            {
                listItem.ItemsSource = ods;
            }
        }

        private async void btnseller_Clicked(object sender, EventArgs e)
        {
            btnbuyer.BackgroundColor = Color.LightGray;
            btnseller.BackgroundColor = Color.Orange;
            HttpClient client = new HttpClient();
            client.BaseAddress = thiscompurl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.GetStringAsync($"/MsgApi/GetOrdersSeller?id={mem.MemberId}");
            var ods = JsonConvert.DeserializeObject<List<OrderListViewModel>>(result);
            if (ods.Count != 0)
            {
                listItem.ItemsSource = ods;
                listItem.ItemTapped += btnOD;
            }
            else
            {
                listItem.ItemsSource = ods;
            }
        }

        private async void logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}