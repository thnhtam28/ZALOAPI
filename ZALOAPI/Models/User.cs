using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class User
    {
        public string user_id { get; set; }
        public string user_id_by_app { get; set; }
        public string display_name { get; set; }
        public string full_name { get; set; }
        public int user_gender { get; set; }
        public string avatar { get; set; }
        public string avatar_120 { get; set; }
        public string avatar_240 { get; set; }
        public string birth_date { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string phone { get; set; }
        public string tag_names { get; set; }
        public string notes { get; set; }
    }
}