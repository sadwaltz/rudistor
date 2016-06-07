using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace rudistor.Contents.ResetModal.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ResetModalViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MofifyModalViewModel class.
        /// </summary>
        /// 

        private string _whichGrid;
        public string WhichGrid
        {
            get
            { return _whichGrid; }
            set
            {
                if (value != _whichGrid)
                {
                    _whichGrid = value;
                }
            }
        }
        private string _stageId;

        public string StageId
        {
            get { return _stageId; }
            set 
            {
                if (value != _stageId)
                {
                    _stageId = value;
                }
            }
        }

        private string _direction;
        public string Direction
        {
            get
            { return _direction; }
            set
            {
                if (value != _direction)
                {
                    _direction = value;
                }
            }
        }

        private string _volume;
        public string Volume
        {
            get
            { return _volume; }
            set
            {
                if (value != _volume)
                {
                    _volume = value;
                }
            }
        }
       

        public ResetModalViewModel()
        {
            
        }
       
    }
}