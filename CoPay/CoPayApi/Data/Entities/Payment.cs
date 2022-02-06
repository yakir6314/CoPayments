using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Data.Entities
{
    public class Payment
    {
        [Key]
        public int id { get; set; }
        public decimal sum { get; set; }
        public User user { get; set; }
        public DateTime dateAdded { get; set; }
        public string comment { get; set; }
        public string business { get; set; }
        public bool isApproved { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
