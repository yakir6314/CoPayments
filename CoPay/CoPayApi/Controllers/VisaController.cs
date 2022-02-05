using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisaController : ControllerBase
    {

        string BaseVisaURL = @"https://sandbox.api.visa.com/dcas/accountservices/v1/accounts/{accountId}/cards/";

        // GET: api/<VisaController>
        [HttpGet("getCardTransaction")]
        public IEnumerable<string> GetCardTransactions(int cardId)
        {
            string TransactionURL = $"{cardId}/transactions";
            Get(BaseVisaURL, TransactionURL);


             string Get(string Baseurl, string Url)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Baseurl+Url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }


            return new string[] { "value1", "value2" };
        }
    }
}
