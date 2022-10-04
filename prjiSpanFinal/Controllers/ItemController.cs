using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
                CProductViewModel cProduct = new CProductViewModel();
                cProduct.product = product;
                

                var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i);
                List<CProductDetailViewModel> cProductDetails = new List<CProductDetailViewModel>();
                foreach (var i in productDetails)
                {
                    CProductDetailViewModel cProductDetail = new CProductDetailViewModel();
                    cProductDetail.productDetail = i;
                    cProductDetails.Add(cProductDetail);
                }

                var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i);
                List<CProductPicViewModel> cProductPics = new List<CProductPicViewModel>();
                if (productPics.Count() != 0)
                {
                    foreach (var i in productPics)
                    {
                        CProductPicViewModel cProductPic = new CProductPicViewModel();
                        cProductPic.productPic = i;
                        cProductPics.Add(cProductPic);
                    }
                }
                
                var smallType = dbContext.SmallTypes.Where(i => i.SmallTypeId == product.SmallTypeId).Select(i => i).FirstOrDefault();
                CSmallTypeViewModel cSmallType = new CSmallTypeViewModel();
                cSmallType.smallType = smallType;

                var bigType = dbContext.BigTypes.Where(i => i.BigTypeId == smallType.BigTypeId).Select(i => i).FirstOrDefault();
                CBigTypeViewModel cBigType = new CBigTypeViewModel();
                cBigType.bigType = bigType;

                var comments = dbContext.Comments.Where(i => i.ProductId == product.ProductId).Select(i => i);
                double commentAvgScore = 0;
                int commentCount = 0;
                if (comments.Count() != 0)
                {
                    commentAvgScore = comments.Average(i => (double)(byte)i.Star);
                    commentCount = comments.Count();
                }

                int sellCount = 0;
                var orderDetails = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == product.ProductId && i.Order.StatusId == 6).Select(i => i.Quantity).ToList();
                if (orderDetails.Count != 0)
                {
                    sellCount = orderDetails.Sum();
                }

                var sellerProducts = dbContext.Products.Where(i => i.MemberId == product.MemberId && i.ProductId != product.ProductId).Select(i => i).ToList();
                List<CProductAllInfoViewModel> sellerProductList = new List<CProductAllInfoViewModel>();
                foreach (var i in sellerProducts)
                {
                    
                    CProductViewModel cProductViewModel = new CProductViewModel();
                    cProductViewModel.product = i;
                    var details = dbContext.ProductDetails.Where(a => a.ProductId == i.ProductId).Select(a => a).ToList();
                    List<CProductDetailViewModel> cProductDetailList = new List<CProductDetailViewModel>();
                    foreach (var j in details)
                    {
                        CProductDetailViewModel cProductDetail = new CProductDetailViewModel();
                        cProductDetail.productDetail = j;
                        cProductDetailList.Add(cProductDetail);
                    }
                    var photos = dbContext.ProductPics.Where(a => a.ProductId == i.ProductId).Select(a => a).ToList();
                    List<CProductPicViewModel> cProductPicList = new List<CProductPicViewModel>();
                    foreach (var j in photos)
                    {
                        CProductPicViewModel cProductPic = new CProductPicViewModel();
                        cProductPic.productPic = j;
                        cProductPicList.Add(cProductPic);
                    }
                    var commentStar = dbContext.Comments.Where(a => a.ProductId == i.ProductId).Select(a => a.Star).ToList();
                    double avgStar = 0;
                    if (commentStar.Count != 0)
                    {
                        avgStar = commentStar.Average(a=>a);
                    }
                    int sellCount1 = dbContext.OrderDetails.Where(a => a.Order.StatusId == 6 && a.ProductDetail.ProductId == i.ProductId).Select(a => a.Quantity).Sum();
                    
                    CProductAllInfoViewModel cProductAllInfo = new CProductAllInfoViewModel();
                    cProductAllInfo.CProduct = cProductViewModel;
                    cProductAllInfo.CProductDetails = cProductDetailList;
                    cProductAllInfo.CProductPics = cProductPicList;
                    cProductAllInfo.CommentAvgScore = avgStar;
                    cProductAllInfo.SellCount = sellCount1;
                    sellerProductList.Add(cProductAllInfo);
                }
                sellerProductList = sellerProductList.OrderByDescending(i => i.CommentAvgScore).Select(i => i).ToList();
                CItemIndexViewModel cItemIndex = new CItemIndexViewModel();
                cItemIndex.CProduct = cProduct;
                cItemIndex.CProductDetails = cProductDetails;
                cItemIndex.CProductPics = cProductPics;
                cItemIndex.CSmallType = cSmallType;
                cItemIndex.CBigType = cBigType;
                cItemIndex.CommentAvgScore = commentAvgScore;
                cItemIndex.CommentCount = commentCount;
                cItemIndex.SellCount = sellCount;
                cItemIndex.SellerProducts = sellerProductList;
                return View(cItemIndex);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Description(int? id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
            CProductViewModel cProduct = new CProductViewModel();
            cProduct.product = product;

            var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i).ToList();
            List<CProductDetailViewModel> cProductDetailsList = new List<CProductDetailViewModel>();
            foreach (var i in productDetails)
            {
                CProductDetailViewModel cProductDetail = new CProductDetailViewModel();
                cProductDetail.productDetail = i;
                cProductDetailsList.Add(cProductDetail);
            }

            var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i).ToList();
            List<CProductPicViewModel> cProductPicsList = new List<CProductPicViewModel>();
            if (productPics.Count != 0)
            {
                foreach (var i in productPics)
                {
                    CProductPicViewModel cProductPic = new CProductPicViewModel();
                    cProductPic.productPic = i;
                    cProductPicsList.Add(cProductPic);
                }
            }
            var seller = dbContext.MemberAccounts.Where(i => i.MemberId == product.MemberId).Select(i => i).FirstOrDefault();
            CMemberAccountViewModel cMemberAccount = new CMemberAccountViewModel();
            cMemberAccount.memberAccount = seller;


            CItemIndexViewModel cItemIndex = new CItemIndexViewModel();
            cItemIndex.CProduct = cProduct;
            cItemIndex.CProductDetails = cProductDetailsList;
            cItemIndex.CProductPics = cProductPicsList;
            cItemIndex.Seller = cMemberAccount;

            return PartialView("/Views/Item/_ItemDescriptionPartial.cshtml", cItemIndex);
        }
        public IActionResult Comment(int? id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var comments = dbContext.Comments.Where(i => i.ProductId == id).Select(i => i).ToList();
            List<CCommentAllInfoViewModel> cCommentAllInfosList = new List<CCommentAllInfoViewModel>();
            if (comments.Count != 0)
            {
                foreach (var i in comments)
                {
                    CCommentViewModel cComment = new CCommentViewModel();
                    cComment.comment = i;

                    var buyer = dbContext.MemberAccounts.Where(a => a.MemberId == i.MemberId).Select(a => a).FirstOrDefault();
                    CMemberAccountViewModel cMemberAccount = new CMemberAccountViewModel();
                    cMemberAccount.memberAccount = buyer;

                    var productStyles = dbContext.OrderDetails.Where(a => a.ProductDetail.ProductId == id && a.Order.MemberId == buyer.MemberId).Select(a => a.ProductDetail.Style);
                    List<string> productStyleList = new List<string>();
                    foreach (var j in productStyles)
                    {
                        productStyleList.Add(j);
                    }

                    var commentPics = dbContext.CommentPics.Where(a => a.CommentId == i.CommentId).Select(a => a).ToList();
                    List<CommentPic> commentPicList = new List<CommentPic>();
                    if (commentPics.Count != 0)
                    {
                        foreach (var j in commentPics)
                        {
                            commentPicList.Add(j);
                        }
                    }

                    CCommentAllInfoViewModel cCommentAllInfo = new CCommentAllInfoViewModel();
                    cCommentAllInfo.CComment = cComment;
                    cCommentAllInfo.CommentPics = commentPicList;
                    cCommentAllInfo.CMemberAccount = cMemberAccount;
                    cCommentAllInfo.ProductStyles = productStyleList;
                    cCommentAllInfosList.Add(cCommentAllInfo);
                }
            }
            CItemCommentViewModel cItemComment = new CItemCommentViewModel();
            cItemComment.CCommentAllInfos = cCommentAllInfosList;
            return PartialView("/Views/Item/_ItemCommentPartial.cshtml", cItemComment);
        }
        public IActionResult BuyerCount(int? id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var orderDetails = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == id && i.Order.StatusId == 6).Select(i => i);
            List<CBuyerCountAllInfoViewModel> cBuyerCountAllInfosList = new List<CBuyerCountAllInfoViewModel>();
            foreach (var i in orderDetails)
            {
                
            }
            return PartialView("/Views/Item/_ItemBuyerCountPartial.cshtml");
        }
    }
}
