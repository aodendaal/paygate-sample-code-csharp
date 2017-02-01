namespace PaygateTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    public class PaygateController : ApiController
    {
        public async Task<IHttpActionResult> GetRequest()
        {
            var client = new HttpClient();

            var request = new Dictionary<string, string>();

            request.Add("PAYGATE_ID", "10011072130");
            request.Add("REFERENCE", "PayGate Test");
            request.Add("AMOUNT", "3299");
            request.Add("CURRENCY", "ZAR");
            request.Add("RETURN_URL", "https://www.paygate.co.za/thankyou");
            request.Add("TRANSACTION_DATE", "2016-03-10 10:49:16");
            request.Add("LOCALE", "en-za");
            request.Add("COUNTRY", "ZAF");
            request.Add("EMAIL", "customer@paygate.co.za");

            request.Add("CHECKSUM", GetMd5Hash(request, "secret"));

            var requestString = ToUrlEncodedString(request);

            var content = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("https://secure.paygate.co.za/payweb3/initiate.trans", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var results = ToDictionary(responseContent);

            if (results.Keys.Contains("ERROR"))
            {
                return InternalServerError(new Exception(results["ERROR"]));
            }

            return Ok(results);
        }

        public IHttpActionResult PostResult([FromBody]string body)
        {
            return Ok(body);
        }

        private string ToUrlEncodedString(Dictionary<string, string> request)
        {
            var builder = new StringBuilder();

            foreach (string key in request.Keys)
            {
                builder.Append("&");
                builder.Append(key);
                builder.Append("=");
                //builder.Append(request[key]);
                builder.Append(HttpUtility.UrlEncode(request[key]));
            }

            var result = builder.ToString().TrimStart('&');

            return result;
        }

        private Dictionary<string, string> ToDictionary(string response)
        {
            var result = new Dictionary<string, string>();

            var valuePairs = response.Split('&');
            foreach (string valuePair in valuePairs)
            {
                var values = valuePair.Split('=');
                result.Add(values[0], values[1]);
            }

            return result;
        }

        private string GetMd5Hash(Dictionary<string, string> request, string key)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var input = new StringBuilder();
                foreach (string value in request.Values)
                {
                    input.Append(value);
                }

                input.Append(key);

                var test = input.ToString();

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }


    }
}
