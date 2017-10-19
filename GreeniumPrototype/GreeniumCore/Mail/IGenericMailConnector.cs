using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeniumCore.Mail{
    public interface IGenericMailConnector{
        void Connect();
        void Disconnect();
        string[] GetMails();
        int GetWeight();
    }
}
