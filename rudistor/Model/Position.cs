using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class Position
    {
        public String StageID
        {
            get { return stageID; }
            set { stageID = beautifiy(value); }
        }
        public String T1
        {
            get { return t1; }
            set { t1 = value; }
        }
        public String T2
        {
            get { return t2; }
            set { t2 = value; }
        }
        public String Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public String RealPrice
        {
            get { return realPrice; }
            set { realPrice = value; }
        }
        public String ExpPrice
        {
            get { return expPrice; }
            set { expPrice = value; }
        }
        public String Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        public ObservableCollection<Canceled> Canceled
        {
            get;
            set;
        }
        private string stageID;
        private string t1;
        private string t2;
        private string direction;
        private string realPrice;
        private string expPrice;
        private string volume;
        

        public Position(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
        {
            // TODO: Complete member initialization
            this.stageID = p1;
            this.t1 = p2;
            this.t2 = p3;
            this.direction = p4;
            this.realPrice = p5;
            this.expPrice = p6;
            this.volume = p7;
            
        }

        private string beautifiy(string stageID)
        {
            if (stageID == null)
                return null;
            int pos = int.Parse(stageID);
            return string.Format("{0}-{1}", pos / 8 + 1, pos % 8 + 1);
        }
    }
    public class Canceled
    {
        public string name{get;set;}
        public int cnt {get;set;}
        public Canceled(String p1, int p2)
        {
            this.name = p1;
            this.cnt = p2;
        }
    }
}
