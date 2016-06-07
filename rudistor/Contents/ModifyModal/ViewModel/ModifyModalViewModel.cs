using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace rudistor.Contents.ModifyModal.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ModifyModalViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MofifyModalViewModel class.
        /// </summary>
        /// 

        private Dictionary<int,string> _stages;
        public Dictionary<int, string> Stages
        {
            get
            { return _stages; }
            set
            {
                if (value != _stages)
                {
                    _stages = value;
                }
            }
        }

        private string _selectedStageA;
        public string SelectedStageA
        {
            get
            { return _selectedStageA; }
            set
            {
                if (value != _selectedStageA)
                {
                    _selectedStageA = value;
                }
            }
        }

        private string _selectedStageB;
        public string SelectedStageB
        {
            get
            { return _selectedStageB; }
            set
            {
                if (value != _selectedStageB)
                {
                    _selectedStageB = value;
                }
            }
        }
       

        public ModifyModalViewModel()
        {
            this._stages = new Dictionary<int, string>();
            //this._selectedStageA = "1";
            //this._selectedStageB = "2";
        }
       
    }
}