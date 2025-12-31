using System;
using System.Collections.Generic;
using System.Text;

namespace Aban.Domain.Configuration
{
    public class PathsConfiguration
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ProjectName { get; set; }
        public string ViewDirectoryName { get; set; }
        public string HomePageUrl { get; set; }
        public string LoginURL { get; set; }
        public string WebsiteURL { get; set; }
        public string HostingEnvironmentPath { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
