using System.Web.Security;

namespace FeedbackSafe
{
    public class FetchUser
    {
        // 
        public static string UserID()
        {
            // Get UserID from ASP.NET Membership
            MembershipUser currentUser = Membership.GetUser();
            string UserID = currentUser.ProviderUserKey.ToString();
            return UserID;
        }

        // 
        public static string UserName()
        {
            // Get UserID from ASP.NET Membership
            MembershipUser currentUser = Membership.GetUser();
            string UserName = currentUser.UserName;
            return UserName;
        }

        // 
        public static string UserEmail()
        {
            // Get UserID from ASP.NET Membership
            MembershipUser currentUser = Membership.GetUser();
            string UserEmail = currentUser.Email;
            return UserEmail;
        }
    }
}