using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Dto
{
    public class PaymentDto
    {
        public decimal sum { get; set; }
        public string comment { get; set; }
        public string business { get; set; }
        public int cardId { get; set; }
    }

}
