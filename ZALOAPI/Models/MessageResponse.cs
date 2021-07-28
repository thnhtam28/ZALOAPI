using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class MessageResponse : BaseResponse<MessageData>
    {

    }
    public class MessageData
    {
        public string message_id { get; set; }
        public string user_id { get; set; }
    }
}