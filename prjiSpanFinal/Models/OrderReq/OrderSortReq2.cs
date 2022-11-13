using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.OrderReq2
{
    public class OrderSortReq
    {
        public List<OrderListViewModel> SortTab(int sort, int tab, int id, int pages, int eachpage, string keyword, DateTime startdate, DateTime enddate) 
        {
            string connectionString = "Data Source=.;Initial Catalog=iSpanProject;Integrated Security=True";
            string queryString = $"SELECT a.OrderID,e.ProductID,f.MemberID as SellerId,f.MemberAcc as SellerAcc,a.MemberID as BuyerId,d.MemberAcc as BuyerAcc,g.IsFreeDelivery as IsFreeDelivery,OrderDatetime,h.OrderStatusName,StatusID as ShipperStatusId,i.Fee as ShipperFee,j.Fee as PaymentFee,b.Quantity,b.UnitPrice,c.Pic,c.Style,e.ProductName FROM Orders as a join OrderDetails as b on a.OrderID = b.OrderID join ProductDetail as c on b.ProductDetailID = c.ProductDetailID join MemberAccount as d on a.MemberID = d.MemberID join Product as e on c.ProductID = e.ProductID join MemberAccount as f on e.MemberID = f.MemberID join Coupons as g on a.CouponID = g.CouponID join OrderStatus as h on a.StatusID = h.OrderStatusID \r\n  join Shipper as i on a.ShipperID = i.ShipperID join Payment as j on a.PaymentID = j.PaymentID " +
                                 $"where a.MemberID = {id} and a.StatusID <> 1 and a.StatusID <> 9 and a.OrderDatetime > '{startdate.ToString("yyyy-MM-dd")}' and a.OrderDatetime < '{enddate.ToString("yyyy-MM-dd")}' order by a.OrderID asc";
            List<OrderListViewModel> q = new List<OrderListViewModel>();
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int thisoid = 0;
                    OrderListViewModel t = new OrderListViewModel();
                    List<int> qty = new List<int>();
                    List<decimal> up = new List<decimal>();
                    List<byte[]> pic = new List<byte[]>();
                    List<string> style = new List<string>();
                    List<string> name = new List<string>();
                    while (reader.Read())
                    {
                        if (thisoid == (int)reader[0])
                        {
                            qty.Add((int)reader[12]);
                            up.Add((decimal)reader[13]);
                            pic.Add((byte[])reader[14]);
                            style.Add((string)reader[15]);
                            name.Add((string)(reader[16]));
                        }
                        else
                        {

                            name = new List<string>();
                            qty = new List<int>();
                            up = new List<decimal>();
                            pic = new List<byte[]>();
                            style = new List<string>();
                            if (t.OrderId != 0)
                            {
                                q.Add(t);
                            }
                            t = new OrderListViewModel();
                            t.OrderId = (int)reader[0];
                            thisoid = t.OrderId;
                            t.ProductId = (int)reader[1];
                            t.SellerId = (int)reader[2];
                            t.SellerAcc = (string)reader[3];
                            t.BuyerId = (int)reader[4];
                            t.BuyerAcc = (string)reader[5];
                            t.IsFreeDelivery = (bool)reader[6];
                            t.OrderDatetime = (DateTime)reader[7];
                            t.OrderStatusName = (string)reader[8];
                            t.ShipperStatusId = (int)reader[9];
                            t.ShipperFee = (int)reader[10];
                            t.PaymentFee = (int)reader[11];
                            t.BuyerCommentId = dbcontext.Orders.Where(o => o.OrderId == t.OrderId).Select(p => p.OrderDetails.Select(o => o.Comments.Count)).FirstOrDefault().ToList();
                            t.SellerCommentId = dbcontext.Orders.Where(o => o.OrderId == t.OrderId).Select(p => p.CommentForCustomers.Count).FirstOrDefault();
                            t.SellerisComment = dbcontext.Orders.Where(o => o.OrderId == t.OrderId).Select(p => p.CommentForCustomers.Any()).FirstOrDefault();
                            qty.Add((int)reader[12]);
                            up.Add((decimal)reader[13]);
                            pic.Add((byte[])reader[14]);
                            style.Add((string)reader[15]);
                            name.Add((string)(reader[16]));
                        }
                        t.Quantity = qty;
                        t.Unitprice = up;
                        t.Pic = pic;
                        t.Style = style;
                        t.ProductName = name;
                    }
                    q.Add(t);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //iSpanProjectContext dbcontext = new iSpanProjectContext();
            //List<OrderListViewModel> q = dbcontext.Orders.Where(o => o.OrderDatetime >= startdate && o.OrderDatetime <= enddate && o.MemberId == id && o.StatusId != 1 && o.StatusId != 9).Select(o => new OrderListViewModel()
            //{
            //    OrderId = o.OrderId,
            //    SellerId = o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId,
            //    SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
            //    BuyerId = o.MemberId,
            //    BuyerAcc = o.Member.MemberAcc,
            //    OrderDatetime = o.OrderDatetime,
            //    //RecieveAdr = o.RecieveAdr,
            //    //FinishDate = o.FinishDate,
            //    //CouponName = o.Coupon.CouponName,
            //    //Discount = o.Coupon.Discount,
            //    IsFreeDelivery = o.Coupon.IsFreeDelivery,
            //    OrderStatusName = o.Status.OrderStatusName,
            //    ShipperStatusId = o.StatusId,
            //    //ShipperName = o.Shipper.ShipperName,
            //    ShipperFee = o.Shipper.Fee,
            //    //ShipperPhone = o.Shipper.Phone,
            //    //PaymentDate = o.PaymentDate,
            //    //ShippingDate = o.ShippingDate,
            //    //ReceiveDate = o.ReceiveDate,
            //    //PaymentName = o.Payment.PaymentName,
            //    PaymentFee = o.Payment.Fee,
            //    BuyerCommentId = o.OrderDetails.Select(o => o.Comments.Count).ToList(),
            //    SellerCommentId = o.CommentForCustomers.Count,
            //    //OrderMessage = o.OrderMessage,
            //    //OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
            //    //ProductDetailId = o.OrderDetails.Select(o => o.ProductDetailId).ToList(),
            //    Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
            //    //OrderDetailReceiveDate = o.OrderDetails.Select(o => o.ReceiveDate).ToList(),
            //    //ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
            //    Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
            //    ProductId = o.OrderDetails.FirstOrDefault().ProductDetail.ProductId,
            //    Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
            //    Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
            //    ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
            //}).ToList();
            if (tab == 0)
            {
            }
            else
            {
                q = q.Where(o => o.ShipperStatusId == tab).ToList();
            }
            if(sort == 0)
            {
                q = q.OrderByDescending(o => o.OrderDatetime).ToList();
            }
            else if(sort == 1)
            {
                q = q.OrderBy(o => o.OrderDatetime).ToList();
            }
            else if(sort == 2)
            {
                q = q.OrderByDescending(o => o.PaymentFee + o.ShipperFee + o.Quantity.Select((Value, index) => Value * Convert.ToInt32(o.Unitprice[index])).Sum()).ToList();
            }
            else if (sort == 3)
            {
                q = q.OrderBy(o => o.PaymentFee + o.ShipperFee + o.Quantity.Select((Value, index) => Value * Convert.ToInt32(o.Unitprice[index])).Sum()).ToList();
            }
            if (keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    q = q.Where(o => o.OrderStatusName.Contains(keys[i]) || o.ProductName.Any(str => str.Contains(keys[i])) || o.SellerAcc.Contains(keys[i]) || o.Style.Any(str => str.Contains(keys[i]))).Select(o => o).ToList();
                }
            }
            foreach (var item in q)
            {
                item.OrderCount = q.Count;
            }
            return q.Skip((pages - 1) * eachpage).Take(eachpage).ToList();
        }
    }
}
