using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePromotion.Models
{
    public class OrderCart
    {
        public long ProductId { get; set; }
        public int NumberofItem { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PromotionPrice { get; set; }
    }
}
