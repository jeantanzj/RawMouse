using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGClassroom.Models
{
    public class Record : BaseModel
    {
        private string _name;
        private string _resultsString;
        private string _score;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

       
        public String ResultsString
        {
            get
            {
                return _resultsString;
            }

            set
            {
                _resultsString = value;
                OnPropertyChanged("ResultsString");
            }
        }

        public String Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

    }

}
