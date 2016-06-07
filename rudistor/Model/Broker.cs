using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class Broker
    {
        public  String brokerName { get; set; }
        public  String brokerId { get; set; }
        public  String host { get; set; }
        public  int port { get; set; }
        #region Fields
        public const string brokerNamePropertyName = "brokerName";
        public const string brokerIdPropertyName = "brokerId";
        public const string hostPropertyName = "host";
        public const string portPropertyName = "port";

        #endregion // Fields
        public static Broker FromDataRow(DataRow dataRow)
        {
            return new Broker()
            {
                brokerName = dataRow.Field<string>(Broker.brokerNamePropertyName),
                brokerId = dataRow.Field<string>(Broker.brokerIdPropertyName),
                port = dataRow.Field<int>(Broker.portPropertyName),
                host = dataRow.Field<string>(Broker.hostPropertyName)
            };
        }
    }
}
