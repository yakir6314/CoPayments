using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace CoPayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisaController : ControllerBase
    {
            //need to add the app id in the header

        string BaseVisaURL = @"https://sandbox.api.visa.com/dcas/cardservices/v1/cards/prepaid/";
        string appId = "";

        [HttpGet("getCardTransaction")]
        public string GetCardTransactions(int cardId)
        {
            string TransactionURL = $"{cardId}/transactions";
            string Tansactions =  Get(BaseVisaURL, TransactionURL);


             string Get(string Baseurl, string Url)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Baseurl+Url);
                request.Headers.Add("app - id", appId);
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
