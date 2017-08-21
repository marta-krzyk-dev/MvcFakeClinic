using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace MvcApplication1.Models
{
   
        public class Profile : ProfileBase
        {
            [Key]
            public int ID;

            [SettingsAllowAnonymous(false)]
            public string Name
            {
                get { return base["Name"] as string; }
                set { base["Name"] = value; }
            }

            [SettingsAllowAnonymous(false)]
            public string Surname
            {
                get { return base["Surname"] as string; }
                set { base["Surname"] = value; }
            }

            private Profile(ProfileBase pb)
            {

            }

            public static Profile GetProfile(string username)
            {
                var p = new Profile(Create(username));
                return p;
            }

            public static Profile GetProfile()
            {
                var p = new Profile(Create(Membership.GetUser().UserName));
                return p;
            }
        }
    
}