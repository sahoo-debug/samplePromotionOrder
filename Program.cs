using SamplePromotion.Models;
using SamplePromotion.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplePromotion
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter item name (A, B, C, D) and count: eg: A 2");
            Console.WriteLine("To calculate price press 'exit' ");
            var orderlist = new List<OrderCart>();
            Console.WriteLine("Enter input:");
            while (true)
            {
                
                string line = Console.ReadLine();
                string[] items = line.Split(' ');
                if(items.Length == 2)
                {
                    var productId = new MasterService().MasterProductList().Where(x => x.Name == items[0]).Select(p => p.Id).FirstOrDefault();
                    if (productId > 0)
                    {
                        orderlist.Add(new OrderCart { ProductId = productId, NumberofItem = Convert.ToInt32(items[1]) });
                    }
                }
                Console.WriteLine("Enter input or Calculate price by press 'exit' ");
                if (line == "exit") // Check string
                {
                    break;
                }  
            }
            var finalorderist = new MasterService().MergeListByProduct(orderlist);
            var obj = new PromotionService();
            var price = obj.GetOrderValue(finalorderist);
            Console.WriteLine("Total price : {0}", price);
        }
    }   
}
