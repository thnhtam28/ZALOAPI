using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZALOAPI.Models;

namespace ZALOAPI.Controllers
{
    public class ZaloController : Controller
    {
        // GET: Zalo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ZaloViewModel model)
        {
            return View();
        }

        public ActionResult CallZaloAPI(string access_token, int type)
        {
            var uri = GetUri(type);
            var lstUser = new List<User>();
            var lstMessage = new List<Message>();

            string strFollowersList = MakeHTTPRequest(uri, access_token, 1);
            var resFollowersList = JsonConvert.DeserializeObject<FollowersResponse>(strFollowersList);

            if (resFollowersList.data.followers != null && resFollowersList.data.followers.Count > 0)
            {
                foreach (var item in resFollowersList.data.followers)
                {
                    uri = GetUri(2);
                    string strProfile = MakeHTTPRequest(uri, access_token, 2, item.user_id);
                    var resProfile = JsonConvert.DeserializeObject<ProfileResponse>(strProfile);
                    var user = new User();

                    if (resProfile != null)
                    {
                        if (resProfile.data != null)
                        {
                            user.user_gender = resProfile.data.user_gender;
                            user.user_id = resProfile.data.user_id;
                            user.user_id_by_app = resProfile.data.user_id_by_app;
                            user.display_name = resProfile.data.display_name;
                            user.avatar = resProfile.data.avatar;
                            user.avatar_120 = resProfile.data.avatars.avatar_120;
                            user.avatar_240 = resProfile.data.avatars.avatar_240;

                            if (resProfile.data.shared_info != null)
                            {
                                user.address = resProfile.data.shared_info.address;
                                user.city = resProfile.data.shared_info.city;
                                user.district = resProfile.data.shared_info.district;
                                user.phone = resProfile.data.shared_info.phone + "";
                                user.full_name = resProfile.data.shared_info.full_name;
                            }
                            //user.tag_names = resProfile.data.tags_and_notes_info.tag_names.ToList().Join(',');
                        }
                    }
                    
                    lstUser.Add(user);
                }
            }

            uri = GetUri(4);
            string strRecentChatList = MakeHTTPRequest(uri, access_token, 4);
            var resRecentChatList = JsonConvert.DeserializeObject<ConversationResponse>(strRecentChatList);
            if (resRecentChatList.data != null)
            {
                foreach (var item in resRecentChatList.data)
                {
                    lstMessage.Add(ConvertConversationDataToMessage(item));
                }
            }

            uri = GetUri(0);
            var strOA = MakeHTTPRequest(uri, access_token, 0);
            var resOA = JsonConvert.DeserializeObject<OAResponse>(strOA);

            return Json(new { lstUser = lstUser, oa = resOA.data, recentChatList = lstMessage });
        }

        public ActionResult GetConversation(string access_token, string user_id)
        {
            var lstMessage = new List<Message>();
            var uri = GetUri(3);
            string strConversationList = MakeHTTPRequest(uri, access_token, 3, user_id);
            var resConversationList = JsonConvert.DeserializeObject<ConversationResponse>(strConversationList);
            if (resConversationList.data != null)
            {
                resConversationList.data = resConversationList.data.OrderBy(x => x.time).ToList();
                foreach(var item in resConversationList.data)
                {
                    lstMessage.Add(ConvertConversationDataToMessage(item));
                }
            }
            
            return Json(lstMessage);
        }

        public ActionResult SendMessage(string access_token, string user_id, string message)
        {
            var uri = GetUri(5);
            string strMessage = MakeHTTPRequest(uri, access_token, 5, user_id, message);
            var resMessage = JsonConvert.DeserializeObject<MessageResponse>(strMessage);
            var code = resMessage.error;
            return Json(new { code = code, str_date_time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
        }

        public Message ConvertConversationDataToMessage(ConversationData data)
        {
            var message = new Message();

            message.src = data.src;
            message.time = data.time;
            message.date_time = JavaTimeStampToDateTime(data.time);
            message.str_date_time = message.date_time.ToString("dd/MM/yyyy HH:mm:ss");
            message.type = data.type;
            message.message = data.message;
            message.message_id = data.message_id;
            message.from_id = data.from_id;
            message.from_display_name = data.from_display_name;
            message.from_avatar = data.from_avatar;
            message.to_id = data.to_id;
            message.to_display_name = data.to_display_name;
            message.to_avatar = data.to_avatar;

            return message;
        }

        public string MakeHTTPRequest(UriMethod uri, string access_token, int type, string user_id = "", string message = "")
        {
            WebRequest request = WebRequest.Create(uri.Uri);
            //var myHttpWebRequest = (HttpWebRequest)request;
            //myHttpWebRequest.PreAuthenticate = true;
            //myHttpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
            //myHttpWebRequest.Accept = "application/json";

            Stream dataStream = null;

            if (uri.IsPostData)
            {
                if (uri.Method == "POST")
                {
                    uri.Uri += "?access_token=" + access_token;
                    request = WebRequest.Create(uri.Uri);
                    request.Method = uri.Method;
                    request.ContentType = "application/json";
                    string postData = string.Empty;

                    if (type == 5)
                    {
                        postData = JsonConvert.SerializeObject(new MessageRequest(user_id, message)).ToString();
                    }

                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    request.ContentLength = byteArray.Length;
                    dataStream = request.GetRequestStream();

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                else if (uri.Method == "GET")
                {
                    if (type == 0)
                    {
                        uri.Uri += "?access_token=" + access_token;
                    }
                    else
                    {
                        string postData = string.Empty;
                        if (type == 1)
                        {
                            postData = JsonConvert.SerializeObject(new GetFollowersRequest()).ToString();
                        }
                        else if (type == 2)
                        {
                            postData = JsonConvert.SerializeObject(new GetProfileRequest(user_id)).ToString();
                        }
                        else if (type == 3)
                        {
                            postData = JsonConvert.SerializeObject(new ConversationRequest(user_id)).ToString();
                        }
                        else if (type == 4)
                        {
                            postData = JsonConvert.SerializeObject(new GetFollowersRequest()).ToString();
                        }
                        uri.Uri += "?data=" + postData;
                        uri.Uri += "&access_token=" + access_token;
                    }
                    request = WebRequest.Create(uri.Uri);
                    request.ContentType = "application/json";
                }
            }

            WebResponse response = request.GetResponse();
            var status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        public UriMethod GetUri(int type)
        {
            switch(type)
            {
                case 0:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/getoa",
                        Method = "GET",
                        IsPostData = true
                    };
                case 1:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/getfollowers",
                        Method = "GET",
                        IsPostData = true
                    };
                case 2:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/getprofile",
                        Method = "GET",
                        IsPostData = true
                    };
                case 3:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/conversation",
                        Method = "GET",
                        IsPostData = true
                    };
                case 4:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/listrecentchat",
                        Method = "GET",
                        IsPostData = true
                    };
                case 5:
                    return new UriMethod()
                    {
                        Uri = "https://openapi.zalo.me/v2.0/oa/message",
                        Method = "POST",
                        IsPostData = true
                    };
            }
            return null;
        }

        public static DateTime JavaTimeStampToDateTime(long javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}