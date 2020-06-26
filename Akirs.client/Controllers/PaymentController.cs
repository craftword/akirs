using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
//using Newtonsoft.Json;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Json;
using Akirs.client.Models;

namespace Akirs.client.Controllers
{
    public class PaymentController : Controller
    {
        private const string secretKey = "XDETQTTDJLF6IDZ6OJNGQMAC3MIQR0";
        private const string url = "https://paywith.quickteller.com/api/v2/transaction/6164518076058662?isRequestRef=true";

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        [Route("Callback/Payment")]
        public ActionResult Callback()
        {

            string refr = "23424323";
           // string clientID = "http://62.173.32.98";
            string clientID = "localhost";

            string hashValue = SHA1(refr + secretKey);
            JsonValue json = Upload(url, clientID, hashValue).Result;
            //try
            //{
            //    JsonValue json = Upload(url, clientID, hashValue).Result;
            //    ErrorLogHandler.SaveLog("Success Error ************************" + json);
            //    return View(json);

            //}
            //catch (Exception ex)
            //{
            //    //SmartLogger.SaveLog("Calling RTGS/ACH Amount is greater than Threshold");
            //    ErrorLogHandler.SaveLog("Error ************************" + ex.InnerException.StackTrace);
            //}



            return View(json);
            //redirect to action
        }


        public ActionResult Message()
        {

            return View();
        }


        public static async Task<JsonValue> Upload(string url, string clientId, string hashValue)
        {
            try
            {
                 using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("clientid", clientId);
                client.DefaultRequestHeaders.Add("Hash", hashValue);

                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    return System.Json.JsonObject.Parse(data);
                }
            }
            }
            catch (Exception ex)
            {

                throw;
            }

           
        }

        public static string SHA1(string plainText)
        {
            SHA1 HashTool = new SHA1Managed();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(plainText));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        public static string toBase64(string plainText)
        {
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(plainText));
            return Convert.ToBase64String(PhraseAsByte);
        }


        //orrr better yet
        // [AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Result(string resp_code, string resp_desc, string tx_ref, string recharge_pin, string signature)
        //{
        //    /****this should be the amount set on the payment request page that has the PwQ script installed**/
        //    long amountRequested = Convert.ToInt64(Session["amount"]);

        //    /***the customer has not paid, till we verify****/
        //    bool paid = false;

        //    /****Quick check to avoid any catastophe*****/
        //    if (string.IsNullOrEmpty(tx_ref))
        //        throw new Exception("No transaction reference returned from Payment gateway!");

        //    /***these values would be supplied by Interswitch***/
        //    var clientId = "http://62.173.32.98";//this will be the domain of your web app
        //    var secretKey = "XDETQTTDJLF6IDZ6OJNGQMAC3MIQR0";//this is a key Interswitch will provide you
        //    var baseUrl = "https://paywith.quickteller.com/api/v2/transaction/6164518076058662?isRequestRef=true";//the base url of the REST service

        //   // var baseUrl = ConfigurationManager.AppSettings["pwqcallbackbaseurl"];//the base url of the REST service
        //    var fullUrl = string.Format("{0}/Transaction.json?transactionRef={1}", baseUrl, tx_ref);
        //    //var signature = 

        //    /**variable to hold the response data**/
        //    PwQTrxnResponse trxnResponse = PwQWrapper.GetTransaction(fullUrl, clientId, secretKey);

        //    /***test for validity of payment****/
        //    if (trxnResponse.ResponseCode == "00" && (Convert.ToInt64(trxnResponse.Amount) == amountRequested))
        //    {
        //        paid = true;
        //        //NEXT: deliver the goods!
        //    }

        //    /***prepare a response UI****/
        //    ViewBag.Paid = paid;
        //    ViewBag.ResponseCode = trxnResponse.ResponseCode;
        //    ViewBag.ResponseDecription = trxnResponse.ResponseDescription;
        //    ViewBag.PaymentRef = trxnResponse.PaymentReference;

        //    /***Serve the UI***/
        //    return View("AfterPayment");
        //}

               
    }
}