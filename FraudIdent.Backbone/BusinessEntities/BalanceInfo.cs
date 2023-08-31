using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessEntities
{
    public class BalanceInfo
    {
        public int Id { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? DistanceScaleCam1 { get; set; }
        public decimal? DistanceScaleCam2 { get; set; }
    }
}
