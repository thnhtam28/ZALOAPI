using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class OAResponse : BaseResponse<OAData>
    {

    }

    public class OAData
    {
        public string description { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string display_name { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public string oa_id { get; set; }
        public bool is_verified { get; set; }
    }
}