using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisaController : ControllerBase
    {

        string BaseVisaURL = @"https://sandbox.api.visa.com/dcas/accountservices/v1/accounts/{accountId}/cards/";

        [HttpGet("getCardTransaction")]
        public string GetCardTransactions(int cardId)
        {
            string TransactionURL = $"{cardId}/transactions";
            string Tansactions =  Get(BaseVisaURL, TransactionURL);


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

            return Tansactions;
        }
    }
}
