using EGClassroom.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EGClassroom.ViewModels.Interfaces
{
    public interface IRegisteredDeviceViewModel
    {
        ObservableCollection<RegisteredDevice> GetRegisteredDevices();
    }
}
