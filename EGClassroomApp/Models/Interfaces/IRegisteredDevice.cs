using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGClassroom.Models.Interfaces
{
    interface IRegisteredDevice
    {
        string DeviceID { get; set; }
        string StudentName { get; set; }
    }
}
