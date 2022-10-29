using iiSpanFinalApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace iiSpanFinalApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}