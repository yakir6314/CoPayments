using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        public ActionResult<string> GetCardTransactions(int cardId,bool isTest)
        {
            string TransactionURL = $"{cardId}/transactions";
            string Tansactions = isTest?getMoc(): Get(BaseVisaURL, TransactionURL);
            return Tansactions;
        }
        [HttpGet("a")]
        string Get(string Baseurl, string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Baseurl + Url);
            request.Headers.Add("app - id", appId);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        [HttpGet("b")]
        string getMoc()
        {
            StreamReader r = new StreamReader(@"C:\CoPayments\CoPay\CoPayApi\Controllers\VisaJson.json");
            string jsonString = r.ReadToEnd();
            return jsonString;
        }
    }
}
