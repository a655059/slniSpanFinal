using iSpanFinalApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iSpanFinalApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Order : ContentPage
    {
        public CAppMember mem;
        public Order(CAppMember member)
        {
            InitializeComponent();
            mem = member;
            formload();


        }
        private void formload()
        {
            Stream stream = new MemoryStream(mem.MemberPic);
            var imageSource = ImageSource.FromStream(() => stream);
            Mempic.Source = imageSource;
            MemAcc.Text = mem.MemberAcc;
            Memname.Text = mem.MemberName;
        }
    }
}