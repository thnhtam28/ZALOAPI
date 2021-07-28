using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class ProfileResponse : BaseResponse<ProfileData>
    {

    }

    public class ProfileData
    {
        public int user_gender { get; set; }
        public string user_id { get; set; }
        public string user_id_by_app { get; set; }
        public string avatar { get; set; }
        public avatars avatars { get; set; }
        public string display_name { get; set; }
        public shared_info shared_info { get; set; }
        public tags_and_notes_info tags_and_notes_info { get; set; }
    }

    public class avatars
    {
        [JsonProperty(PropertyName = "120")]
        public string avatar_120 { get; set; }
        [JsonProperty(PropertyName = "240")]
        public string avatar_240 { get; set; }
    }

    public class shared_info
    {
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public int phone { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string full_name { get; set; }
    }

    public class tags_and_notes_info
    {
        public string[] tag_names { get; set; }
        public string[] notes { get; set; }
    }
}