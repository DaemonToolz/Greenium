using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GreeniumCore.API
{
    public class SessionWrapper
    {
        private String SavedUsername { get; set; }


        private String ConnectionToken { get; set; }

        public bool NotificationPooler { get; set; }


       
        public SessionWrapper()
        {
        }

        public SessionWrapper(String Username, String Password, String Token, bool Autolog = false)
        {
            ConnectionToken = Token;
            SavedUsername = Username;


        }

        public async System.Threading.Tasks.Task<bool> LogInAsync()
        {
            try
            {
                var jsonUser = await RequestHandler.Call("http://localhost:10856/Mobile.svc/Mobile/Login", "POST", null,
                    null,
                    new {Username = SavedUsername, Token = ConnectionToken});

                var userObj = JsonConvert.DeserializeObject<dynamic>(jsonUser);
                jsonUser = JsonConvert.SerializeObject(userObj.LogInResult);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

    }
}