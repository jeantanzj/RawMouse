using EGClassroom.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models
{
    public class RegisteredDevice : IRegisteredDevice
    {
        private string _studentName;
        private string _deviceID;
        public string DeviceID
        {
            get { return _deviceID;  }
            set { if (value != _deviceID) _deviceID = value; }
        }

        public string StudentName
        {
            get
            {
                return _studentName;
            }

            set
            {   if (value != _studentName) _studentName = value;
            }
        }

        public override string ToString()
        {
            return _deviceID + ": " + _studentName;
        }
    }
}
