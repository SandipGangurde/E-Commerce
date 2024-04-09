using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Common
{
    public class ServiceJsonReader
    {
        public ServiceConfigurations ServiceConfigurations { get; set; }
    }
    public class MicroService
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
    }

    public class ServiceConfigurations
    {
        public string BaseUri { get; set; }
        public string AuthUri { get; set; }
        public List<MicroService> MicroServices { get; set; }
        public int PDALokingDayRange { get; set; }
    }
}
