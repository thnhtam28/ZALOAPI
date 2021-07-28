﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class ConversationResponse : BaseResponse<List<ConversationData>>
    {

    }

    public class ConversationData
    {
        public int src { get; set; }
        public long time { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string message_id { get; set; }
        public string from_id { get; set; }
        public string from_display_name { get; set; }
        public string from_avatar { get; set; }
        public string to_id { get; set; }
        public string to_display_name { get; set; }
        public string to_avatar { get; set; }
    }
}