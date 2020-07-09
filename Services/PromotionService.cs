using SamplePromotion.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplePromotion.Services
{
    public class PromotionService
    {
        MasterService masterservice = null;
        public PromotionService()
        {
            masterservice = new MasterService();
        }
        public decimal GetOrderValue(List<OrderCart> orderlist)
        {
            var productIds = orderlist.Select(p => p.ProductId).ToArray();
            decimal finalPrice = 0;
            List<decimal> pricelist = new List<decimal>();
            List<int> promotionIdTemp = new List<int>();
            IList<OrderCart> promotionlist = new List<OrderCart>();
            foreach (var item in orderlist)
            {
                IList<OrderCart> temp = new List<OrderCart>();
                temp.Add(item);
                var promotionId = GetMatchingPromotion(temp);
                if (promotionId > 0)
                {
                    //calculate its price
                    // remove item from list
                    var price = CalculatePromotionPrice(temp, promotionId);
                    pricelist.Add(price);
                    promotionIdTemp.Add(promotionId);
                }
                else
                {
                    promotionlist.Add(item);
                }
            }


            var promotionIds = promotionIdTemp.ToArray();
            var comboPromotionlist = (from p in masterservice.MasterPromotionList() where !promotionIds.Contains(p.PromotionId) select p).ToList();

            foreach (var item in comboPromotionlist)
            {
                bool validPromo = false;
                List<OrderCart> orderCarts = new List<OrderCart>();
                foreach (var i in item.PromotionItem)
                {
                    var validItem = promotionlist.Where(x => x.ProductId == i.ProductId).FirstOrDefault();
                    if (validItem != null)
                    {
                        if (validItem.NumberofItem >= i.NumberofProduct)
                        {
                            validPromo = true;
                            orderCarts.Add(new OrderCart { ProductId = i.ProductId, NumberofItem = validItem.NumberofItem });
                        }
                        else
                        {
                            validPromo = false;
                            orderCarts.Clear();
                            break;
                        }
                    }
                }
                if (validPromo)
                {
                    var price = CalculatePromotionPrice(orderCarts, item.PromotionId);
                    pricelist.Add(price);
                    foreach (var order in orderCarts)
                    {
                        promotionlist = promotionlist.Where(x => x.ProductId != order.ProductId).ToList();
                    }
                }
            }
            if (promotionlist.Any())
            {
                foreach (var itemlist in promotionlist)
                {
                    var itemPrice = masterservice.GetProductPrice(itemlist.ProductId);
                    var cartPrice = itemPrice * itemlist.NumberofItem;
                    pricelist.Add(cartPrice);
                }
            }

            foreach (var itemprice in pricelist)
            {
                finalPrice = finalPrice + itemprice;
            }
            return finalPrice;
        }

        public decimal CalculatePromotionPrice(IList<OrderCart> productkartList, int promotionId)
        {
            decimal promotionPrice = 0;
            long productId = 0;
            int productItemCount = 0;
            Promotion promotion = null;
            if (productkartList.Count == 1)
            {
                productId = productkartList[0].ProductId;
                productItemCount = productkartList[0].NumberofItem;
                promotion = masterservice.MasterPromotionList().Where(x => x.PromotionId == promotionId && x.IsActive == true).FirstOrDefault();
                if (promotion != null)
                {
                    int promotionItemConut = promotion.PromotionItem[0].NumberofProduct;
                    int temp = productItemCount % promotionItemConut;

                    switch (promotionId)
                    {
                        case 1:
                        case 2:

                            decimal productPrice = masterservice.MasterProductList().Where(x => x.Id == productId).Select(p => p.Price).FirstOrDefault();
                            promotionPrice = promotion.Value * (productItemCount / promotionItemConut) + (temp * productPrice);

                            break;
                        default:
                            // code block
                            break;
                    }
                }
            }
            else
            {
                decimal firstProductValue = masterservice.GetProductPrice(productkartList[0].ProductId);
                decimal secondProductValue = masterservice.GetProductPrice(productkartList[1].ProductId);
                int firstproductItemCount = productkartList[0].NumberofItem;
                int secondproductItemCount = productkartList[1].NumberofItem;
                int promoValue = firstproductItemCount;
                decimal items = 0;
                if (firstproductItemCount > secondproductItemCount)
                {
                    promoValue = secondproductItemCount;
                    items = (firstproductItemCount - secondproductItemCount) * firstProductValue;
                }
                else if (firstproductItemCount < secondproductItemCount)
                {
                    promoValue = firstproductItemCount;
                    items = (secondproductItemCount - firstproductItemCount) * secondProductValue;
                }
                promotion = masterservice.MasterPromotionList().Where(x => x.PromotionId == promotionId && x.IsActive == true).FirstOrDefault();
                if (promotion != null)
                {
                    switch (promotionId)
                    {
                        case 3:
                            promotionPrice = promoValue * promotion.Value + items;
                            break;
                        default:
                            // code block
                            break;
                    }
                }

            }
            return promotionPrice;
        }

        public int GetMatchingPromotion(IList<OrderCart> productList)
        {
            int resultPromotionId = 0;
            var productKeys = productList.Select(p => p.ProductId).ToArray();
            Array.Sort(productKeys);

            foreach (var item in masterservice.MasterPromotionList())
            {
                var promotionitemKeys = item.PromotionItem.Select(p => p.ProductId).ToArray();
                Array.Sort(promotionitemKeys);

                var isvalid = productKeys.SequenceEqual(promotionitemKeys);
                if (isvalid)
                {
                    resultPromotionId = item.PromotionId;
                    var promotionItem = masterservice.MasterPromotionList().Where(x => x.PromotionId == resultPromotionId).Select(x => x.PromotionItem).FirstOrDefault();
                    int validPromotionCount = 0;
                    foreach (var product in productList)
                    {
                        foreach (var promotion in promotionItem)
                        {
                            if (product.ProductId == promotion.ProductId)
                            {
                                if (product.NumberofItem >= promotion.NumberofProduct)
                                {
                                    validPromotionCount++;
                                }
                            }
                        }
                    }
                    if (validPromotionCount > 0 && productList.Count == validPromotionCount)
                    {
                        break;
                    }
                }
            }
            return resultPromotionId;
        }
    }
}
