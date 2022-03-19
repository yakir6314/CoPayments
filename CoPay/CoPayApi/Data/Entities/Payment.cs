using CoPayApi.Data.DTO;
using CoPayApi.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
        public CraditCard CraditCard { get; set; }
        public ErrorHandler sendPaymentToSap()
        {
            ErrorHandler err = new ErrorHandler() { isSuccess = false, error = string.Empty };
            string url = Consts.sapBaseUrl + Consts.addPaymentToSapUrl;
            string jsonObject = JsonConvert.SerializeObject(this);
            string stringResponse = "";
            dynamic dynamicResponse;
            try
            {

                using (WebClient wc = new WebClient())
                {
                    stringResponse = wc.UploadString(url, jsonObject);
                    dynamicResponse = JsonConvert.DeserializeObject<dynamic>(stringResponse);
                    if (dynamicResponse["success"] == true)
                    {
                        err.successFunction();
                    }
                    else
                    {
                        err.failFunction(dynamicResponse["error"]);
                    }
                }
            }
            catch(Exception e)
            {

            }

            return err;
        }
    }
}
