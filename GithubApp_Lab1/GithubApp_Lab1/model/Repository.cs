using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubApp_Lab1.model
{
    internal class Repository
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }
}
