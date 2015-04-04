using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DniproFuture.Models.Abstract;
using DniproFuture.Models.Repository;

namespace DniproFuture.Models.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        DniproFutureModelRepository _repository = new DniproFutureModelRepository();
        public bool Authenticate(string username, string password)
        {
            bool result = _repository.IsUserExist(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}