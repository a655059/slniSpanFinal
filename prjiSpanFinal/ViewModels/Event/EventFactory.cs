using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventFactory
    {
        iSpanProjectContext _db;

        public EventFactory()
        {
            _db = new iSpanProjectContext();
        }
        public List<Coupon> listTheCoupons(int? memberid)
        {
            List<Coupon> res = new List<Coupon>();
            res = _db.Coupons.Where(p => p.CouponId != 1 &&p.MemberId== memberid).ToList();
            return res;
        }
        //取得EventID轉成ViewModel
        public EventViewModel fToEvent(int EventID,int memid)
        {
            EventViewModel res = new EventViewModel();
            //活動為非預設活動
            OfficialEventList Event = _db.OfficialEventLists.Where(e => e.OfficialEventListId != 1&&e.OfficialEventListId==EventID).FirstOrDefault();
            if (Event == null)
            {
                return res;
            }

            List<CShowCoupon> evtShowCoupon = new List<CShowCoupon>();
            //優惠券為 本次活動 可收券時間(早>晚)排序
            var Coupons = _db.Coupons.Where(c => c.OfficialEventListId == EventID).OrderBy(e => e.ReceiveStartDate);
            if (Coupons.Any())
                evtShowCoupon = fCouponToShowCoupon(Coupons.ToList(), memid);

            List<EventSubs> evtSubs = new List<EventSubs>();
            //子活動為本次活動 折價排序(低>高)
            var Subs = _db.SubOfficialEventLists.Where(s => s.OfficialEventListId == EventID).OrderByDescending(s=>s.Discount).ToList();
            if (Subs.Any()) { 
                foreach(var item in Subs)
                {
                    List<EventShowItem> PRODS =ftoEvtShowItem(_db.SubOfficialEventToProducts.Where(S => S.SubOfficialEventListId == item.SubOfficialEventListId && S.VerifyId == 2).Select(p => p.Product).ToList());                    
                    EventSubs es = new EventSubs()
                    {
                        SubEvent = item,
                        SubEventProducts=PRODS,
                    };
                    evtSubs.Add(es);
                }
            }

            res.Event = Event;
            res.EventSubs = evtSubs;
            res.EventCoupons = evtShowCoupon;
            return res;
        }
        //List<Coupon> 轉成List<CShowCoupon>
        public List<CShowCoupon> fCouponToShowCoupon(List<Coupon> coupon,int id)
        {
            List<CShowCoupon> res = new List<CShowCoupon>();
            if (coupon.Any())
            {
                foreach (Coupon coupons in coupon)
                {
                    CShowCoupon showCoupon = new CShowCoupon
                    {
                        coupon = coupons,
                    };
                    if (id > 0)
                        showCoupon.loggeduser = _db.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
                    res.Add(showCoupon);
                }

            }


            return res;
        }
        public List<EventShowItem> ftoEvtShowItem(List<Product> list)
        {
            List<EventShowItem> res = new List<EventShowItem>();
            if (list == null)
            {
                return res;
            }
            foreach (var item in list)
            {
                if (item.ProductStatusId !=0)
                {
                    continue;
                }
                var price = _db.ProductDetails.Where(p => p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice);
                decimal x = price.Min();
                decimal y = price.Max();
                byte[] pic = _db.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                decimal discount=1;
                bool isDeliveryFree = false;
                bool isStart = false;
                SubOfficialEventToProduct prodstatus = new SubOfficialEventToProduct();
                if (_db.SubOfficialEventToProducts.Where(p => p.ProductId == item.ProductId).Where(p => p.VerifyId == 2).FirstOrDefault()!=null)
                {
                    prodstatus = _db.SubOfficialEventToProducts.Where(p => p.ProductId == item.ProductId).Where(p => p.VerifyId == 2).FirstOrDefault();
                    var prodSublist = _db.SubOfficialEventLists.Where(e => e.SubOfficialEventListId == prodstatus.SubOfficialEventListId).FirstOrDefault();
                    var prodoflist = _db.SubOfficialEventLists.Where(e => e.SubOfficialEventListId == prodstatus.SubOfficialEventListId).Select(p => p.OfficialEventList).FirstOrDefault();
                    discount = Convert.ToDecimal(prodSublist.Discount);
                    isDeliveryFree = prodSublist.IsFreeDelivery;
                    if (DateTime.Now.CompareTo(prodoflist.StartDate) >= 0 && DateTime.Now.CompareTo(prodoflist.EndDate) <= 0)
                        isStart = true;
                }


                List<decimal> dlist = new List<decimal>();
                if (x == y)
                    dlist.Add(x);
                else
                {
                    dlist.Add(x);
                    dlist.Add(y);
                }
                EventShowItem a = new EventShowItem()
                {
                    product = item,
                    price = dlist,
                    discount= discount,
                    isDeliveryFree=isDeliveryFree,
                    isStart=isStart,                    
                };
                if (pic != null)
                    a.pic = pic;

                res.Add(a);
            }
            return res;
        }
        public List<FreshSalesShowItem> ftoFSShowItem(List<Product> list,int eventID) 
        {
            List<FreshSalesShowItem> res = new List<FreshSalesShowItem>();
            if (!list.Any())
            {
                return res;
            }
            foreach (var prod in list)
            {
                if (prod.ProductStatusId !=0)
                {
                    continue;
                }
                FreshSalesShowItem obj = new FreshSalesShowItem();
                var prodinfo = _db.SubOfficialEventToProducts.Where(e => e.SubOfficialEventList.OfficialEventListId == eventID).Where(e => e.ProductId == prod.ProductId && e.VerifyId == 2);
                decimal Discount = Convert.ToDecimal(prodinfo.Select(e => e.SubOfficialEventList.Discount).FirstOrDefault());
                //decimal Discount = Convert.ToDecimal(_db.SubOfficialEventToProducts.Where(e => e.ProductId == prod.ProductId && e.VerifyId == 2 && !e.SubOfficialEventList.IsFreeDelivery).Select(e => e.SubOfficialEventList.Discount).FirstOrDefault());
                DateTime StartDate = prodinfo.Select(e => e.SubOfficialEventList.OfficialEventList.StartDate).FirstOrDefault();
                //DateTime StartDate = _db.SubOfficialEventToProducts.Where(e => e.ProductId == prod.ProductId && e.VerifyId == 2).Select(e => e.SubOfficialEventList.OfficialEventList.StartDate).FirstOrDefault();
                int Sale = _db.OrderDetails.Where(o => o.ProductDetail.ProductId == prod.ProductId).Where(o => o.Order.StatusId == 7 || o.Order.StatusId == 6).Where(o => (o.Order.ReceiveDate).CompareTo(StartDate) > 0).Select(o => o.Quantity).Sum(o => o);
                byte[] Pic = _db.ProductPics.Where(p => p.ProductId == prod.ProductId).Select(p => p.Pic).FirstOrDefault();
                var Detail = _db.ProductDetails.Where(p => p.ProductId == prod.ProductId);
                var Price = Detail.Select(p => p.UnitPrice);
                List<decimal> PriceList = new List<decimal>();
                if (Price.Min() == Price.Max())
                {
                    PriceList.Add(Price.Max());
                }
                else
                {
                    PriceList.Add(Price.Max());
                    PriceList.Add(Price.Min());
                }
                int Stock = Detail.Select(p => p.Quantity).Sum(q => q);
                bool isStart = false;
                bool isOver = false;
                SubOfficialEventToProduct prodstatus = _db.SubOfficialEventToProducts.Where(p => p.ProductId == prod.ProductId).Where(p => p.VerifyId == 2).FirstOrDefault();
                

                    if (_db.SubOfficialEventToProducts.Where(p => p.ProductId == prod.ProductId).Where(p => p.VerifyId == 2).Where(e => DateTime.Now.CompareTo(e.SubOfficialEventList.OfficialEventList.StartDate) >= 0 && DateTime.Now.CompareTo(e.SubOfficialEventList.OfficialEventList.EndDate) <= 0).Any())
                        isStart = true;
                    if (_db.SubOfficialEventToProducts.Where(p => p.ProductId == prod.ProductId).Where(p => p.VerifyId == 2).Where(e => DateTime.Now.CompareTo(e.SubOfficialEventList.OfficialEventList.EndDate) >= 0).Any())
                        isOver = true;
                obj.product = prod;
                obj.price = PriceList;
                if (Pic != null)
                    obj.pic = Pic;
                obj.stock = Stock;
                obj.discount = Discount;
                obj.sales = Sale;
                obj.isStart = isStart;
                obj.isOver = isOver;

                res.Add(obj);
            }
            return res;
        }
    }
}
