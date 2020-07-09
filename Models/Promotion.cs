using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePromotion.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public long Value { get; set; }
        public IList<PromotionSet> PromotionItem { get; set; }
        public bool IsActive { get; set; }
    }

    public class PromotionSet
    {
        public long ProductId { get; set; }
        public int NumberofProduct { get; set; }
    }
}
