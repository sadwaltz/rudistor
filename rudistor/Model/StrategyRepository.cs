using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    class StrategyRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();  
        #region Fields
        private DataSet _data;
        /// <summary>
        /// The file path that we use to store XML file which holds all the app data we need
        /// </summary>
        public static readonly string PhysicalFilePath = Path.Combine(Environment.CurrentDirectory, "Strategy.rep");
        /// <summary>
        /// Name of [Broker] table
        /// </summary>
        private const string StrategyTableName = "strategy";
        private static StrategyRepository _instance;
        #endregion // Fields

        public static StrategyRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StrategyRepository();
            }
            return _instance;
        }
        private StrategyRepository()
        {
            this.CreateDataSource();

            if (!File.Exists(PhysicalFilePath))
            {
                this.AddDefaultStrategy();
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

        private void Save()
        {
            _data.AcceptChanges();
            _data.WriteXml(PhysicalFilePath);
        }

        private void CreateDataSource()
        {
            _data = new DataSet("StrategyData");

            DataTable strategies = new DataTable(StrategyTableName);

            strategies.Columns.Add(Strategy.whichGridPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.IsActivatePropertyName, typeof(bool));
            strategies.Columns.Add(Strategy.StageIdPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.limitPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.lockNumPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.volPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.kkjcPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.kpPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.dkjcPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.dpPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t1cjPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t1ddPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2cjPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2ddPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2clPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2volPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.clPropertyName, typeof(string));
            //新版本 added
            strategies.Columns.Add(Strategy.autoCallPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.jjkkPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.jjkpPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.jjdkPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.jjdpPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2WeightPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.t2MethodPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.zdjcPropertyName, typeof(string));
            strategies.Columns.Add(Strategy.zkjcPropertyName, typeof(string));
            
            strategies.PrimaryKey =new DataColumn[] { strategies.Columns[Strategy.whichGridPropertyName]};
            _data.Tables.Add(strategies);
        }
        private void AddDefaultStrategy()
        {
            int total = 32;
            for (int i = 1; i <= total; ++i)
            {
                String whichGrid;
                if (i < 10)
                {
                    whichGrid = "Grid0" + i;
                }
                else
                {
                    whichGrid = "Grid" + i;
                }
                bool isActivate = true;
                String stageId = "aaa-bbbb";
                String limit = "5";
                String lockNum = "3";
                String vol = "1";
                String kkjc = "-160.0";
                String kp = "-170.0";
                
                Strategy strategy = new Strategy() {whichGrid=whichGrid,IsActivate=isActivate,StageId=stageId,limit=limit,lockNum=lockNum,vol=vol,kkjc=kkjc,kp=kp,dkjc="-1000.0",dp="-201.0",t1cj="0",t1dd="0",t2cj="0",t2dd="0",t2cl="1",t2vol="2",cl="4",autoCall="0",jjkk="999",jjkp="999",jjdk="999",jjdp="999",t2Weight="1",t2Method="0",zdjc="999",zkjc="999" };

                this.AddStrategy(strategy);
            }
        }
        private void AddStrategy(Strategy strategy)
        {
            Strategies.Rows.Add(new object[]
                {
                    strategy.whichGrid,
                    strategy.IsActivate,
                    strategy.StageId,
                    strategy.limit,
                    strategy.lockNum,
                    strategy.vol,                    
                    strategy.kkjc,
                    strategy.kp,
                    strategy.dkjc,
                    strategy.dp,
                    strategy.t1cj,
                    strategy.t1dd,
                    strategy.t2cj,
                    strategy.t2dd,
                    strategy.t2cl,
                    strategy.t2vol,
                    strategy.cl,
                    //新版本
                    strategy.autoCall,
                    strategy.jjkk,
                    strategy.jjkp,
                    strategy.jjdk,
                    strategy.jjdp,
                    strategy.t2Weight,
                    strategy.t2Method,
                    strategy.zdjc,
                    strategy.zkjc
                });
        }
        private DataTable Strategies
        {
            get
            {
                return _data.Tables[StrategyTableName];
            }
        }

        public IEnumerable<Strategy> GetStrategies()
        {
            foreach (DataRow dataRow in Strategies.Rows)
            {
                yield return Strategy.FromDataRow(dataRow);
            }
        }
        
        public Strategy getStrategyByGridName(String gridName)
        {
            DataRow foundRow = Strategies.Rows.Find(gridName);
            Strategy temp =  Strategy.FromDataRow(foundRow);
            temp.IsNotActivate = !temp.IsActivate;
            return temp;
        }
        public void updateStrategy(Strategy strategy)
        {
            logger.Debug("开始更新生效参数："+strategy.whichGrid);
            try
            {
                DataRow rowToDelete = Strategies.Rows.Find(strategy.whichGrid);
                rowToDelete.Delete();
                this.AddStrategy(strategy);
                this.Save();
            }
            catch (Exception e)
            {
                logger.Debug("更新生效参数发生异常：" + e.ToString());
            }
        }
    }
}
