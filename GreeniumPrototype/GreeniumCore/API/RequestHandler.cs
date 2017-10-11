using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GreeniumCore.API{
    public class RequestHandler{
        internal static readonly HttpClient Client = new HttpClient();

        public static async Task<String> Call(String address, String method, Dictionary<String, String> headers, Dictionary<String, String> getParams, Object postParams) {
            if (method.ToLower().Equals("get")){

                var finalAddress = address + "?";
                finalAddress = getParams.Aggregate(finalAddress, (current, pair) => current + (pair.Key + "=" + pair.Value + "&"));
                finalAddress.Remove(finalAddress.LastIndexOf("&"));

                var response = await Client.GetAsync(finalAddress);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var response = await Client.PostAsync(address,
                    new StringContent(JsonConvert.SerializeObject(postParams), Encoding.UTF8, "application/json"));
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
