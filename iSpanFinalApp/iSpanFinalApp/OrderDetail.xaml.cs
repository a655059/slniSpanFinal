using iSpanFinalApp.ViewModels;
using System;
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
	public partial class OrderDetail : ContentPage
	{
		OrderDetailViewModel od;
        public OrderDetail (OrderDetailViewModel orderdetail)
		{
			InitializeComponent ();
			od = orderdetail;
            outerstack.BindingContext = od;

            for (int i=0;i< od.Quantity.Count;i++)
			{
				StackLayout st = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
				
				};
				Image im = new Image()
				{
					Source = od.PicCool[i],
					WidthRequest = 100,
					HeightRequest = 100
				};
                st.Children.Add(im);
				StackLayout st2 = new StackLayout();
				StackLayout st31 = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
				};
                Label lb311 = new Label()
                {
					TextColor = Color.Black,
                    FontSize = 24,
                    Text = od.ProductName[i]+" "
                };
                Label lb312 = new Label()
				{
                    TextColor = Color.Black,
                    FontSize = 24,
                    Text = od.Style[i]
				};
                st31.Children.Add(lb311);
                st31.Children.Add(lb312);
                st2.Children.Add(st31);
                StackLayout st32 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                };
				Label lb321 = new Label()
				{
					Margin = new Thickness(20, 0, 0, 0),
                    FontSize = 16,
                    Text = "價格: "
                };
				Label lb322 = new Label()
				{
					Margin = new Thickness(20, 0, 0, 0),
					FontSize = 16,
					Text = od.Unitprice[i].ToString()
                };
                st32.Children.Add(lb321);
                st32.Children.Add(lb322);
                st2.Children.Add(st32);
                StackLayout st33 = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                };
                Label lb331 = new Label()
                {
                    Margin = new Thickness(20, 0, 0, 0),
                    FontSize = 16,
                    Text = "數量: "
                };
                Label lb332 = new Label()
                {
                    Margin = new Thickness(20, 0, 0, 0),
                    FontSize = 16,
                    Text = od.Quantity[i].ToString()
                };
                st33.Children.Add(lb331);
                st33.Children.Add(lb332);
                st2.Children.Add(st33);
                st.Children.Add(st2);
                layoutsl.Children.Add(st);
            }

        }
	}
}