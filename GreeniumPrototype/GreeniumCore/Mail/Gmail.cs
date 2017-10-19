using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EAGetMail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace GreeniumCore.Mail
{
    public class Gmail : IGenericMailConnector
    {
        private MailClient myAccount;
        private MailServer imapServer;

        private void OAuth()
        {
    
        }

        public Gmail(String email, String password, String name)
        {
            myAccount = new MailClient(name);
            
            imapServer = new MailServer("imap.gmail.com",
                email, password, true,
                ServerAuthType.AuthXOAUTH2, ServerProtocol.Imap4){
                Port = 993
            };

            //imapServer = new MailServer("imap.gmail.com", email, password, ServerProtocol.Imap4);
        }

        public void Connect()
        {
            myAccount.Connect(imapServer);
        }

        public void Disconnect()
        {
            myAccount.Quit();
        }

        public string[] GetMails()
        {
            return null; //myAccount.GetMailInfos().Select;
        }

        public int GetWeight()
        {
            return myAccount.GetMailCount() * 29;
        }
        
    }
}
