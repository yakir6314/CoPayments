using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoPayApi.Data.Entities
{
    public class CraditCard
    {
        public string Id { get; set; }
        public int User { get; set; }
        public int Token { get; set; }
        public int Mask { get; set; }
    }
}
