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
        private string _image;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public string Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
    }

}
