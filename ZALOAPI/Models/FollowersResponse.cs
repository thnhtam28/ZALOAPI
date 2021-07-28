using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZALOAPI.Models
{
    public class FollowersResponse : BaseResponse<FollowersData>
    {

    }
    public class FollowersData
    {
        public int total { get; set; }
        public List<followers> followers { get; set; }
    }

    public class followers
    {
        public string user_id { get; set; }
    }
}