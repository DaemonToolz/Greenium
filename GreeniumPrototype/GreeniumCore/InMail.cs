
using System.IO;
using System.Reflection;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;

using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Office.Interop.Outlook;

namespace GreeniumCore{
    public class InMail{

        private static string[] Scopes = { GmailService.Scope.GmailReadonly };
        private static string ApplicationName = "Greenium Gmail API";

        public long OutlookWeight(string username, string password)
        {
            var application = new Application();

           
            Microsoft.Office.Interop.Outlook.NameSpace outlook = application.GetNamespace("MAPI");

            var defaultFolder = outlook.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            defaultFolder.GetExplorer(false);

            outlook.Logon(username, password, Missing.Value, Missing.Value);

         
            long weight = defaultFolder.Items.Count;
            outlook.Logoff();
            return weight * 29;
        }

        public long GmailWeight(){
            UserCredential credential;

            using (var stream =
                new FileStream("./Auth/authorizer.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/greenium_api.json");
                
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
               
            }
            
            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer(){
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
       
            return service.Users.Messages.List("me").Execute().Messages.Count * 29;

        }

    }

}
