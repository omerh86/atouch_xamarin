using LinphoneXamarin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinphoneXamarin.Util
{


    public sealed class MySendRequestHelper
    {

        private static MySendRequestHelper instance = null;
        private static readonly object padlock = new object();


        MySendRequestHelper()
        {

        }
        public static MySendRequestHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MySendRequestHelper();
                    }
                    return instance;
                }
            }
        }


        public string getFcmRequest(FcmProp fcmProp)
        {
            FcmRequest fcmRequest = new FcmRequest(fcmProp);
            return objToJson<FcmRequest>(fcmRequest);
        }

        public string getCreateAccountRequest(CreateAccountProp createAccountProp)
        {
            CreateAccountRequest createAccountRequest = new CreateAccountRequest(createAccountProp);
            return objToJson<CreateAccountRequest>(createAccountRequest);
        }

        public string GetRsUserRequest(UserNameProp userNameProp)
        {
            RsUserRequest rsUserRequest = new RsUserRequest(userNameProp);
            return objToJson<RsUserRequest>(rsUserRequest);
        }

        public string GetPictureRequest(UserNameProp userNameProp)
        {
            PicturerRequest picturerRequest = new PicturerRequest(userNameProp);
            return objToJson<PicturerRequest>(picturerRequest);
        }

        public string GetNextResponseRequest()
        {
            NextResponseRequest nextResponseRequest = new NextResponseRequest(new object());
            return objToJson<NextResponseRequest>(nextResponseRequest);
        }


        public struct UserNameProp
        {
            public string userName;

            public UserNameProp(string userName)
            {
                this.userName = userName;
            }
        }

        public struct FcmProp
        {
            public string userName;
            public string token;

            public FcmProp(string userName, string token)
            {
                this.userName = userName;
                this.token = token;
            }
        }


        public struct ConnectionProp
        {
            public string userName;
            public string deviceId;

            public ConnectionProp(string userName, string deviceId)
            {
                this.userName = userName;
                this.deviceId = deviceId;

            }
        }

        public struct AITSettingsProp
        {
            public string userName;
            public string epID;

            public AITSettingsProp(string userName, string epID)
            {
                this.userName = userName;
                this.epID = epID;
            }
        }

        public struct CreateAccountProp
        {
            public string userLoginName;
            public string productName;
            public bool hasSoftPhone;
            public string deviceID;
            public bool isMobile;

            public CreateAccountProp(string userLoginName, string productName, bool hasSoftPhone, string deviceID, bool isMobile)
            {
                this.userLoginName = userLoginName;
                this.productName = productName;
                this.hasSoftPhone = hasSoftPhone;
                this.deviceID = deviceID;
                this.isMobile = isMobile;
            }
        }

        private struct FcmRequest
        {
            public FcmProp FcmToken;

            public FcmRequest(FcmProp p)
            {
                FcmToken = p;
            }
        }

        private struct CreateAccountRequest
        {
            public CreateAccountProp CreateAccount;

            public CreateAccountRequest(CreateAccountProp p)
            {
                CreateAccount = p;
            }
        }

        private struct RsUserRequest
        {
            public UserNameProp GetRsUser;

            public RsUserRequest(UserNameProp p)
            {
                GetRsUser = p;
            }
        }


        private struct PicturerRequest
        {
            public UserNameProp GetPicture;

            public PicturerRequest(UserNameProp p)
            {
                GetPicture = p;
            }
        }

        private struct NextResponseRequest
        {
            public object GetNextResponse;

            public NextResponseRequest(object GetNextResponse)
            {
                this.GetNextResponse = GetNextResponse;
            }
        }


        private T jsonToObj<T>(string val)
        {
            T loginInfo = JsonConvert.DeserializeObject<T>(val);
            return loginInfo;
        }

        private string objToJson<T>(T obj)
        {
            string res = JsonConvert.SerializeObject(obj);
            return res;
        }

    }
}
