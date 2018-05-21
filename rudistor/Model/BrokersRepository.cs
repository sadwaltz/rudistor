using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    class BrokersRepository
    {
        #region Fields
        private DataSet _data;
        /// <summary>
        /// The file path that we use to store XML file which holds all the app data we need
        /// </summary>
        public static readonly string PhysicalFilePath = Path.Combine(Environment.CurrentDirectory, "Config", "Brokers.rep");
        /// <summary>
        /// Name of [Broker] table
        /// </summary>
        private const string BrokerTableName = "brokers";
        private static BrokersRepository _instance;
        #endregion // Fields

        public static BrokersRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BrokersRepository();
            }
            return _instance;
        }
        private BrokersRepository()
        {
            this.CreateDataSource();

            if (!File.Exists(PhysicalFilePath))
            {
                this.AddDefaultBrokers();
                this.Save();
            }
            else
            {
                this.Load();
            }
        }
        private void Load()
        {
            _data.ReadXml(PhysicalFilePath);
        }
        private void CreateDataSource()
        {
            _data = new DataSet("BrokerData");

            DataTable brokers = new DataTable(BrokerTableName);
            
            brokers.Columns.Add(Broker.brokerNamePropertyName, typeof(string));
            brokers.Columns.Add(Broker.brokerIdPropertyName, typeof(string));
            brokers.Columns.Add(Broker.hostPropertyName, typeof(string));
            brokers.Columns.Add(Broker.portPropertyName, typeof(int));

            _data.Tables.Add(brokers);
        }
        private DataTable Brokers
        {
            get
            {
                return _data.Tables[BrokerTableName];
            }
        }
        public IEnumerable<Broker> GetBrokers()
        {
            foreach (DataRow dataRow in Brokers.Rows)
            {
                yield return Broker.FromDataRow(dataRow);
            }
        }
        private void AddDefaultBrokers()
        {
            int total = 5;
            for (int i = 0; i < total; ++i)
            {
                String brokerName = "Name " + i;
                String brokerId = i.ToString();
                String host = "192.168.0.1";
                int port = 60000+i;
                Broker broker = new Broker() { brokerName = brokerName ,brokerId=brokerId,host=host,port=port};
                this.AddBroker(broker);
            }
        }
        private void AddBroker(Broker broker)
        {
            Brokers.Rows.Add(new object[]
                {
                    broker.brokerName,
                    broker.brokerId,
                    broker.host,
                    broker.port
                });
        }
        private void Save()
        {
            _data.AcceptChanges();
            _data.WriteXml(PhysicalFilePath);
        }
    }
}
