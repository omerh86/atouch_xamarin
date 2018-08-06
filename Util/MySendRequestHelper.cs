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

        public string getFavoritesRequest(UserNameProp userNameProp)
        {
            FavoritesRequest favoritesRequest = new FavoritesRequest(userNameProp);
            return objToJson<FavoritesRequest>(favoritesRequest);
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

        public string getContactListRequest(ContactListProp contactListProp)
        {
            ContactListRequest contactListRequest = new ContactListRequest(contactListProp);
            return objToJson<ContactListRequest>(contactListRequest);
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


        public struct ContactListProp
        {
            public int startIndex;
            public int endIndex;
            public string sortingMethod;
            public string sortingOrder;
            public string filter;
            public bool filterAliasOnly;

            public ContactListProp(int startIndex, int endIndex, string filter)
            {
                this.startIndex = startIndex;
                this.endIndex = endIndex;
                this.sortingMethod = "LAST_NAME";
                this.sortingOrder = "ASCENDING";
                this.filter = filter;
                this.filterAliasOnly = false;
            }
        }

        private struct ContactListRequest
        {
            public ContactListProp GetContactList;

            public ContactListRequest(ContactListProp p)
            {
                GetContactList = p;
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

        private struct FavoritesRequest
        {
            public UserNameProp GetFavorites;

            public FavoritesRequest(UserNameProp p)
            {
                GetFavorites = p;
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

        public FavRootobjectResponse getServerFavRootObject(string json)
        {
            return jsonToObj<FavRootobjectResponse>(json);
        }

        public ContactlistRootobjectResponse getServerContactListRootObject(string json)
        {
            return jsonToObj<ContactlistRootobjectResponse>(json);
        }

        public class FavRootobjectResponse
        {
            public Containerinfo containerInfo { get; set; }
        }

        public class Containerinfo
        {
            public string type { get; set; }
            public int errCode { get; set; }
            public string errDesc { get; set; }
            public int totalNumber { get; set; }
            public Favorit[] favorits { get; set; }
        }

        public class ContactlistRootobjectResponse
        {
            public Contactlistresponse ContactListResponse { get; set; }
        }

        public class Contactlistresponse
        {
            public int errCode { get; set; }
            public string errDesc { get; set; }
            public int totalNumber { get; set; }
            public Contactspresence[] contactsPresence { get; set; }
        }

        public class Contactspresence
        {
            public Contact contact { get; set; }
            public Presense presense { get; set; }
        }

        public class Contact
        {
            public Alias[] aliases { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
            public object[] extAliases { get; set; }
            public string firstName { get; set; }
            public int imageSignature { get; set; }
            public string lastName { get; set; }
            public string serviceType { get; set; }
            public string userName { get; set; }
         
        }

        public class Alias
        {
            public string completeAliasName { get; set; }
            public bool completeInterGroupAccess { get; set; }
            public string configuration { get; set; }
        }

     

        public class Presense
        {
            public Presence presence { get; set; }
        }

        public class Presence
        {
            public string explicitPresence { get; set; }
            public string implicitPresence { get; set; }
        }

        public class Favorit
        {
            public Alias[] aliases { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
            public string[] extAliases { get; set; }
            public string firstName { get; set; }
            public int imageSignature { get; set; }
            public string lastName { get; set; }
            public string serviceType { get; set; }
            public string userName { get; set; }
        }

      


    }
}
