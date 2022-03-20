using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Data.Entities
{
    public class CraditCard
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public string Mask { get; set; }
        public List<Payment> payments { get; set; }
    }
}
