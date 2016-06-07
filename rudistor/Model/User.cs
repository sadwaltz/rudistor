using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class User
    {
        private String userId;

        public String UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private String pass;

        public String Passwd
        {
            get { return pass; }
            set { pass = value; }
        }
        private String brokerId;

        public String BrokerId
        {
            get { return brokerId; }
            set { brokerId = value; }
        }
        private String investorId;

        public String InvestorId
        {
            get { return investorId; }
            set { investorId = value; }
        }
    }
}
