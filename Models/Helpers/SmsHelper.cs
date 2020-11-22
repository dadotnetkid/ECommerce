using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Helpers
{
    public class SmsHelper
    {
        public static void SendSms(string message, string address)
        {
            string accessToken = "kPwgAHTu-INMyd3IGQdwIxs__oAEMqMpkHpjf_LpK60";
            var shortCode = "5686";

            var subscriber = "9067701852";
            var url = $"https://devapi.globelabs.com.ph/smsmessaging/v1/outbound/{shortCode}/requests?access_token={accessToken}";
            var client = new RestClient(url);
            var request = new RestRequest();
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.Parameters.Clear();
            request.AddJsonBody(new
            {
                outboundSMSMessageRequest = new
                {
                    senderAddress = shortCode,
                    outboundSMSTextMessage = new { message = message },
                    address = address
                }
            });
            IRestResponse response = client.Execute(request);
            var content = response.Content;
        }
    }
}
