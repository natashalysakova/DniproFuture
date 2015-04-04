using System.Web.Security;
using DniproFuture.Models.Abstract;
using DniproFuture.Models.Repository;

namespace DniproFuture.Models.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        private readonly DniproFutureModelRepository _repository = new DniproFutureModelRepository();

        public bool Authenticate(string username, string password)
        {
            var result = _repository.IsUserExist(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}