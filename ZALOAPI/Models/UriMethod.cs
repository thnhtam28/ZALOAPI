using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class UriMethod
    {
        public UriMethod()
        {
            IsPostData = false;
        }

        public string Uri { get; set; }
        public string Method { get; set; }
        public bool IsPostData { get; set; }
    }

    public class GetFollowersRequest
    {
        public GetFollowersRequest()
        {
            offset = 0;
            count = 10;
        }
        public int offset { get; set; }
        public int count { get; set; }
    }

    public class GetProfileRequest
    {
        public GetProfileRequest(string user_id)
        {
            this.user_id = user_id;
        }
        public string user_id { get; set; }
    }

    public class ConversationRequest : GetFollowersRequest
    {
        public ConversationRequest(string user_id)
        {
            offset = 0;
            count = 10;
            this.user_id = Convert.ToInt64(user_id);
        }
        public long user_id { get; set; }
    }

    [Serializable]
    public class MessageRequest
    {
        public MessageRequest(string user_id, string message)
        {
            this.recipient = new recipient();
            this.message = new message();
            this.recipient.user_id = user_id;
            this.message.text = message;
        }
        public recipient recipient { get; set; }
        public message message { get; set; }
    }

    public class recipient
    {
        public string user_id { get; set; }
    }

    public class message
    {
        public string text { get; set; }
    }
}