using ProjectBusinessCustomer.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ProjectBusinessCustomer.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string email, string password)
        {
            bool result = FormsAuthentication.Authenticate(email, password);
            if (result)
                FormsAuthentication.SetAuthCookie(email, false);

            return result;
        }
    }
}