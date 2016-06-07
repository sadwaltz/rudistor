using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rudistor.Model
{
    public class ResetInfo
    {

        private string _whichGrid;
        private string _direction;
        private string _volume;
        
        public String Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public String Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }
        public String whichGrid
        {
            get { return _whichGrid; }
            set { _whichGrid = value; }
        }
        public ResetInfo(string whichGrid,string direction, string volume)
        {
            this.whichGrid = whichGrid;
            
            this.Direction = direction;
            this.Volume = volume;
        }

    }
}
