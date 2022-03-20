using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoPayApi.Data.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrivateCompanyID { get; set; }
        public string customerCode { get; set; }
        public List<User> UsersInTheCompany { get; set; }
    }
}
