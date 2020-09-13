using Microsoft.AspNet.Identity;
using RRMDataManager.Library.DataAccess;
using RRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        public List<UserModel> GetById()
        {
            var data = new UserData();
            string id = RequestContext.Principal.Identity.GetUserId();

            return data.GetUserById(id);
        }
    }
}
