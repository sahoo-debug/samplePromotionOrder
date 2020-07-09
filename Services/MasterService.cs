using SamplePromotion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePromotion.Services
{
   public class MasterService
    {
        public IList<Product> MasterProductList()
        {
            var productlist = new List<Product>
            {
                new Product{ Id = 1, Name= "A", Price= 50  },
                new Product{ Id = 2, Name= "B", Price= 30  },
                new Product{ Id = 3, Name= "C", Price= 20  },
                new Product{ Id = 4, Name= "D", Price= 15  },
            };
            return productlist;
        }

        public decimal GetProductPrice(long productId)
        {
            var productPrice = MasterProductList().Where(x => x.Id == productId).Select(c => c.Price).FirstOrDefault();
            return productPrice;
        }

        public IList<Promotion> MasterPromotionList()
        {
            var promotionlist = new List<Promotion>
            {
                new Promotion { PromotionId = 1, IsActive= true, Value = 130, PromotionItem = new List<PromotionSet>(){ new PromotionSet{ ProductId= 1, NumberofProduct = 3 } } },      // three A's
                new Promotion { PromotionId = 2, IsActive= true, Value = 45, PromotionItem = new List<PromotionSet>(){ new PromotionSet{ ProductId= 2, NumberofProduct = 2 } } },       // two B's
                new Promotion { PromotionId = 3, IsActive= true, Value = 30, PromotionItem = new List<PromotionSet>() { new PromotionSet { ProductId = 3, NumberofProduct = 1 },
                                                                                                                        new PromotionSet { ProductId = 4, NumberofProduct = 1 }, } }  // one C & One D
            };
            return promotionlist;
        }

        public List<OrderCart> MergeListByProduct(List<OrderCart> list)
        {
            var orderlist = new List<OrderCart>();
            foreach(var item in list)
            {
                var product = orderlist.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                if(product != null)
                {
                    int itemCount = product.NumberofItem + item.NumberofItem;
                    orderlist.Where(x => x.ProductId == item.ProductId).ToList().ForEach(i => i.NumberofItem = itemCount); 
                }
                else
                {
                    orderlist.Add(item);
                }
            }
            return orderlist;
        }
    }
}
