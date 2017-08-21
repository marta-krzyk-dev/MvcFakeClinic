using MedicalClinic.Infrastructure.Abstract;
using System.Web.Security; // FormsAuthentication

namespace MedicalClinic.Infrastructure.Concrete
{
    //Klasa osłonowa dla metod statycznych klasy FormsAuthentication.
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);

            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }

            return result;
        }
    }
}