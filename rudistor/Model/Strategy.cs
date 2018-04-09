using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class Strategy : ViewModelBase
    {
        #region constructors
        public Strategy(string whichGrid,bool IsActivate,string StageId,string limit,string lockNum,string vol)
        {
            this.whichGrid = whichGrid;
            this.IsActivate = IsActivate;
            this.IsNotActivate = !IsActivate;
            this.StageId = StageId;
            this.t1 = StageId.Split('-').ToArray()[0];
            this.t2 = StageId.Split('-').ToArray()[1];
            this.incre = getIncre(t1);
            this.limit = limit;
            this.lockNum = lockNum;
            this.vol =  vol;
            this.autoCall = "Close";
            this.formatString = FormatStringGet(this.incre);
        }
        public Strategy(Strategy s)
        {
            this.whichGrid = s.whichGrid;
            this.IsActivate = s.IsActivate;
            this.IsNotActivate = !s.IsActivate;
            this.StageId = s.StageId;
            this.t1 = s.t1;
            this.t2 = s.t2;
            this.incre = s.incre;
            this.limit = s.limit;
            this.lockNum = s.lockNum;
            this.vol = s.vol;
            this.t1cj = s.t1cj;
            this.t1dd = s.t1dd;
            this.t2cj = s.t2cj;
            this.t2dd = s.t2dd;
            this.kkjc = s.kkjc;
            this.kp = s.kp;
            this.dkjc = s.dkjc;
            this.dp = s.dp;
            this.t2cl = s.t2cl;
            this.t2vol = s.t2vol;
            this.cl = s.cl;
            this.autoCall = s.autoCall;
            this.jjkk = s.jjkk;
            this.jjkp = s.jjkp;
            this.jjdk = s.jjdk;
            this.jjdp = s.jjdp;
            this.t1Weight = s.t1Weight;
            this.t2Weight = s.t2Weight;
            this.t2Ratio = s.t2Ratio;
            this.zdjc = s.zdjc;
            this.zkjc = s.zkjc;
            this.formatString = s.formatString;
        }
        public Strategy()
        {
            // TODO: Complete member initialization
        }
        #endregion
        //所属grid
        public string whichGrid { get; set; }
        //策略状态 生效与否
        private bool _IsActivate;
        public bool IsActivate 
        {
            get
            {
                return _IsActivate;
            }
            set 
            {
                if (value != _IsActivate)
                {
                    _IsActivate = value;
                    IsNotActivate = !IsActivate;
                    RaisePropertyChanged("IsNotActivate");
                    RaisePropertyChanged("IsActivate");
                }
            }
        }
        public bool IsNotActivate { get; set; }
        //策略种类 xxxx-xxxx
        private string _stageId;
        public string StageId 
        {
            get
            {
                return _stageId;
            }
            set 
            {
                if (value != _stageId)
                {
                    _stageId = value;
                    t1 = _stageId.Split('-').ToArray()[0];
                    t2 = _stageId.Split('-').ToArray()[1];
                    incre = getIncre(t1);
                    formatString = FormatStringGet(incre);
                    RaisePropertyChanged("StageId");
                    RaisePropertyChanged("t1");
                    RaisePropertyChanged("t2");
                    RaisePropertyChanged("incre");
                    RaisePropertyChanged("formatString");
                }
            }
        }
        //仓位限制
        public string limit { get; set; }
        //锁定
        public string lockNum { get; set; }       
        //每笔
        public string vol { get; set; }    
        //T1合约
        private string _t1;
        public string t1
        {
            get
            {
                return _t1;
            }
            set
            {
                if (value != _t1)
                {
                    _t1 = value;                    
                    StageId = _t1 + "-" + _t2;
                    
                   
                    
                }
            }
        }
        //T2合约
        private string _t2;
        public string t2 {
            get
            {
                return _t2;
            }
            set
            {
                if (value != _t2)
                {
                    _t2 = value;
                    StageId = _t1 + "-" + _t2;
                    
                }
            }
        }
        //步进值
        public string incre { get; set; }
        //空开价差
        private string _kkjc;
        public string kkjc
        {
            get
            {
                return _kkjc;
            }
            set
            {
                if (value != _kkjc)
                {
                    _kkjc = value;
                    RaisePropertyChanged("kkjc");
                }
            }
        }
        //空平
        private string _kp;
        public string kp
        {
            get
            {
                return _kp;
            }
            set
            {
                if (value != _kp)
                {
                    _kp = value;
                    RaisePropertyChanged("kp");
                }
            }
        }
        //多开价差
        private string _dkjc;
        public string dkjc
        {
            get
            {
                return _dkjc;
            }
            set
            {
                if (value != _dkjc)
                {
                    _dkjc = value;
                    RaisePropertyChanged("dkjc");
                }
            }
        }
        //多平
        private string _dp;
        public string dp
        {
            get
            {
                return _dp;
            }
            set
            {
                if (value != _dp)
                {
                    _dp = value;
                    RaisePropertyChanged("dp");
                }
            }
        }
        //腿1超价
        private string _t1cj;
        public string t1cj
        {
            get
            {
                return _t1cj;
            }
            set
            {
                if (value != _t1cj)
                {
                    _t1cj = value;
                    RaisePropertyChanged("t1cj");
                }
            }
        }
        //腿1等待
        private string _t1dd;
        public string t1dd
        {
            get
            {
                return _t1dd;
            }
            set
            {
                if (value != _t1dd)
                {
                    _t1dd = value;
                    RaisePropertyChanged("t1dd");
                }
            }
        }
        //腿2超价
        private string _t2cj;
        public string t2cj
        {
            get
            {
                return _t2cj;
            }
            set
            {
                if (value != _t2cj)
                {
                    _t2cj = value;
                    RaisePropertyChanged("t2cj");
                }
            }
        }
        //腿2等待
        private string _t2dd;
        public string t2dd
        {
            get
            {
                return _t2dd;
            }
            set
            {
                if (value != _t2dd)
                {
                    _t2dd = value;
                    RaisePropertyChanged("t2dd");
                }
            }
        }
        //腿2策略，取值：中间价、对手价
        private string _t2cl;
        public string t2cl 
        {
            get 
            {
                return _t2cl;
            }
            set
            {
                if (value != _t2cl)
                {
                    _t2cl = value;
                    RaisePropertyChanged("t2cl");
                }
            }
        }
        //permitVol
        private string _t2vol;
        public string t2vol
        {
            get
            {
                return _t2vol;
            }
            set
            {
                if (value != _t2vol)
                {
                    _t2vol = value;
                    RaisePropertyChanged("t2vol");
                }
            }
        }
        //策略
        private string _cl;
        public string cl 
        {
            get
            {
                return _cl;
            }
            set
            {
                if (value != _cl)
                {
                    _cl = value;
                    RaisePropertyChanged("cl");
                }
            }
        }
        //自动报单
        public string autoCall { get; set; }
        //间距空开
        public string jjkk { get; set; }
        //间距空平
        public string jjkp { get; set; }
        //间距多开
        public string jjdk { get; set; }
        //间距多平
        public string jjdp { get; set; }
        //腿1配比
        public string t1Weight { get; set; }
        //腿2配比
        public string t2Weight { get; set; }
        //腿2系数
        public string t2Ratio { get; set; }
        //做多价差--实际多开价差
        public string zdjc { get; set; }
        //做空价差--实际空开价差
        public string zkjc { get; set; }
        //Format String 
        public string formatString { get; set; }
        #region Config file fields
        public const string whichGridPropertyName = "whichGrid";
        public const string IsActivatePropertyName = "IsActivate";
        public const string StageIdPropertyName = "StageId";
        public const string limitPropertyName = "limit";
        public const string lockNumPropertyName = "lockNum";
        public const string volPropertyName = "vol";
        public const string kkjcPropertyName = "kkjc";
        public const string kpPropertyName = "kp";
        public const string dkjcPropertyName = "dkjc";
        public const string dpPropertyName = "dp";
        public const string t1cjPropertyName = "t1cj";
        public const string t1ddPropertyName = "t1dd";
        public const string t2cjPropertyName = "t2cj";
        public const string t2ddPropertyName = "t2dd";
        public const string t2clPropertyName = "t2cl";
        public const string t2volPropertyName = "t2vol";
        public const string clPropertyName = "cl";
        //新版本 added
        public const string autoCallPropertyName = "autoCall";
        public const string jjkkPropertyName = "jjkk";
        public const string jjkpPropertyName = "jjkp";
        public const string jjdkPropertyName = "jjdk";
        public const string jjdpPropertyName = "jjdp";
        public const string t1WeightPropertyName = "t1Weight";
        public const string t2WeightPropertyName = "t2Weight";
        public const string t2RatioPropertyName = "t2Ratio";
        public const string zdjcPropertyName = "zdjc";
        public const string zkjcPropertyName = "zkjc";

        #endregion // Fields
        #region create from config file
        public static Strategy FromDataRow(DataRow dataRow)
        {
            Strategy temp =  new Strategy()
            {
                whichGrid = dataRow.Field<string>(Strategy.whichGridPropertyName),
                //IsActivate = dataRow.Field<bool>(Strategy.IsActivatePropertyName),
                StageId = dataRow.Field<string>(Strategy.StageIdPropertyName),
                t1 = dataRow.Field<string>(Strategy.StageIdPropertyName).Split('-').ToArray()[0],
                t2 = dataRow.Field<string>(Strategy.StageIdPropertyName).Split('-').ToArray()[1],               
                limit = dataRow.Field<string>(Strategy.limitPropertyName),
                lockNum = dataRow.Field<string>(Strategy.lockNumPropertyName),
                vol = dataRow.Field<string>(Strategy.volPropertyName),
                kkjc = dataRow.Field<string>(Strategy.kkjcPropertyName),
                kp = dataRow.Field<string>(Strategy.kpPropertyName),
                dkjc = dataRow.Field<string>(Strategy.dkjcPropertyName),
                dp = dataRow.Field<string>(Strategy.dpPropertyName),
                t1cj=dataRow.Field<string>(Strategy.t1cjPropertyName),
                t1dd=dataRow.Field<string>(Strategy.t1ddPropertyName),
                t2cj=dataRow.Field<string>(Strategy.t2cjPropertyName),
                t2dd=dataRow.Field<string>(Strategy.t2ddPropertyName),
                t2cl=dataRow.Field<string>(Strategy.t2clPropertyName),
                t2vol=dataRow.Field<string>(Strategy.t2volPropertyName),
                cl=dataRow.Field<string>(Strategy.clPropertyName),
                //新版本 added
                //autoCall=dataRow.Field<string>(Strategy.autoCallPropertyName),
                jjkk=dataRow.Field<string>(Strategy.jjkkPropertyName),
                jjkp = dataRow.Field<string>(Strategy.jjkpPropertyName),
                jjdk = dataRow.Field<string>(Strategy.jjdkPropertyName),
                jjdp = dataRow.Field<string>(Strategy.jjdpPropertyName),
                t1Weight = dataRow.Field<string>(Strategy.t1WeightPropertyName),
                t2Weight = dataRow.Field<string>(Strategy.t2WeightPropertyName),
                t2Ratio = dataRow.Field<string>(Strategy.t2RatioPropertyName),
                zdjc = dataRow.Field<string>(Strategy.zdjcPropertyName),
                zkjc = dataRow.Field<string>(Strategy.zkjcPropertyName)
            };
            temp.IsActivate = false;
            temp.IsNotActivate = !temp.IsActivate;
            //获得品种步进值
            temp.incre = temp.getIncre(temp.t1);
            temp.formatString = temp.FormatStringGet(temp.incre);
            //默认关闭自动报单
            temp.autoCall = "Close";
            return temp;
        }
        #endregion
        #region private method
        private string getIncre(string stage)
        {

            if (stage == null)
            {
                return ConfigurationManager.AppSettings["StockIncre"];
            }

            string header = stage.Substring(0, 2).ToUpper();
            string index = header + "Incre";
            try
            {
                if (null != ConfigurationManager.AppSettings[index])
                {
                    return ConfigurationManager.AppSettings[index];
                }
                return ConfigurationManager.AppSettings["StockIncre"];
            }
            catch (ConfigurationErrorsException)
            {

                return ConfigurationManager.AppSettings[index];
            }




        }
        private string FormatStringGet(string incr)
        {
            //增强稳定性 18/01/2018
            if (incr == null)
            {
                return "F0";
            }
            int pos;
            pos = incr.IndexOf('.');

            if (pos < 0)
            {
                return "F0";
            }

            return string.Format("F{0}", incr.Length - pos - 1);
        }
        #endregion
    }
}