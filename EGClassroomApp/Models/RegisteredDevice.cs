using EGClassroom.Helper;
using EGClassroom.Models.Interfaces;
using EGClassroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models
{
    public class RegisteredDevice : BaseModel, IRegisteredDevice
    {
        private string _name;
        private string _deviceID;
        private string _imagePath;
        private RoleEnum _role;
        public string DeviceID
        {
            get { return _deviceID;  }
            set {
                if (value != _deviceID)
                {
                    _deviceID = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {   if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public RoleEnum Role
        {
            get
            {
                return _role;
            }

            set
            {
                if (value != _role)
                {
                    _role = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }

            set
            {
                if (value != _imagePath)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return _deviceID + ": " + _name;
        }
    }
}
