using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace GreeniumCore.Mail
{
    public class Outlook : IGenericMailConnector
    {
        private Application outlook;
        private NameSpace   @namespace;
        private MAPIFolder  folder;

        private string email, password;

        public Outlook(String email, String password, String name){
            outlook = new Application();
            @namespace = outlook.GetNamespace("MAPI");
            folder = @namespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            folder.GetExplorer(false);
            this.email = email;
            this.password = password;
        }

        public void Connect()
        {
            @namespace.Logon(email, password, Missing.Value, Missing.Value);
        }

        public void Disconnect()
        {
            @namespace.Logoff();
        }

        public string[] GetMails()
        {
            throw new NotImplementedException();
        }

        public int GetWeight()
        {
            return folder.Items.Count * 29;
        }
    }
}
