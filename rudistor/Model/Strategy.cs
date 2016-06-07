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
            this.limit = limit;
            this.lockNum = lockNum;
            this.vol =  vol;
        }
        public Strategy(Strategy s)
        {
            this.whichGrid = s.whichGrid;
            this.IsActivate = s.IsActivate;
            this.StageId = s.StageId;
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
                    RaisePropertyChanged("StageId");
                }
            }
        }
        //仓位限制
        public string limit { get; set; }
        //锁定
        public string lockNum { get; set; }
       
        //每笔
        public string vol { get; set; }
        
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
        //
        public string t1dd { get; set; }
        //
        public string t2cj { get; set; }
        //
        public string t2dd { get; set; }
        //
        public string t2cl { get; set; }
        //
        public string t2vol { get; set; }
        //
        public string cl { get; set; }


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
        #endregion // Fields
        public static Strategy FromDataRow(DataRow dataRow)
        {
            return new Strategy()
            {
                whichGrid = dataRow.Field<string>(Strategy.whichGridPropertyName),
                IsActivate = dataRow.Field<bool>(Strategy.IsActivatePropertyName),
                StageId = dataRow.Field<string>(Strategy.StageIdPropertyName),
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
                cl=dataRow.Field<string>(Strategy.clPropertyName)

            };
        }
    }
}
