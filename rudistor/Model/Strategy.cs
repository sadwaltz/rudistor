using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class Strategy : ViewModelBase
    {
        public Strategy(string whichGrid,bool IsActivate,string StageId,string limit,string lockNum,string vol)
        {
            this.whichGrid = whichGrid;
            this.IsActivate = IsActivate;
            this.IsNotActivate = !IsActivate;
            this.StageId = StageId;
            this.t1 = StageId.Split('-').ToArray()[0];
            this.t2 = StageId.Split('-').ToArray()[1];
            this.limit = limit;
            this.lockNum = lockNum;
            this.vol =  vol;
        }
        public Strategy(Strategy s)
        {
            this.whichGrid = s.whichGrid;
            this.IsActivate = s.IsActivate;
            this.StageId = s.StageId;
            this.t1 = StageId.Split('-').ToArray()[0];
            this.t2 = StageId.Split('-').ToArray()[1];
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
            this.t2Weight = s.t2Weight;
            this.t2Method = s.t2Method;
            this.zdjc = s.zdjc;
            this.zkjc = s.zkjc;
        }
        public Strategy()
        {
            // TODO: Complete member initialization
        }
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
                    RaisePropertyChanged("StageId");
                    RaisePropertyChanged("t1");
                    RaisePropertyChanged("t2");
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
        public string t1 { get; set; }
        //T2合约
        public string t2 { get; set; }
        //空开价差
        public string kkjc { get; set; }
        //空平
        public string kp { get; set; }
        //多开价差
        public string dkjc { get; set; }
        //多平
        public string dp { get; set; }
        //腿1超价
        public string t1cj { get; set; }
        //腿1等待
        public string t1dd { get; set; }
        //腿2超价
        public string t2cj { get; set; }
        //腿2等待
        public string t2dd { get; set; }
        //腿2策略，取值：中间价、对手价
        public string t2cl { get; set; }
        //permitVol
        public string t2vol { get; set; }
        //策略
        public string cl { get; set; }
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
        //腿2配比
        public string t2Weight { get; set; }
        //减、除
        public string t2Method { get; set; }
        //做多价差--实际多开价差
        public string zdjc { get; set; }
        //做空价差--实际空开价差
        public string zkjc { get; set; }
        #region Fields
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
        public const string t2WeightPropertyName = "t2Weight";
        public const string t2MethodPropertyName = "t2Method";
        public const string zdjcPropertyName = "zdjc";
        public const string zkjcPropertyName = "zkjc";

        #endregion // Fields
        public static Strategy FromDataRow(DataRow dataRow)
        {
            return new Strategy()
            {
                whichGrid = dataRow.Field<string>(Strategy.whichGridPropertyName),
                IsActivate = dataRow.Field<bool>(Strategy.IsActivatePropertyName),
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
                autoCall=dataRow.Field<string>(Strategy.autoCallPropertyName),
                jjkk=dataRow.Field<string>(Strategy.jjkkPropertyName),
                jjkp = dataRow.Field<string>(Strategy.jjkpPropertyName),
                jjdk = dataRow.Field<string>(Strategy.jjdkPropertyName),
                jjdp = dataRow.Field<string>(Strategy.jjdpPropertyName),
                t2Weight = dataRow.Field<string>(Strategy.t2WeightPropertyName),
                t2Method = dataRow.Field<string>(Strategy.t2MethodPropertyName),
                zdjc = dataRow.Field<string>(Strategy.zdjcPropertyName),
                zkjc = dataRow.Field<string>(Strategy.zkjcPropertyName)
            };
        }
    }
}
