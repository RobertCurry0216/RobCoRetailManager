using RRMDataManager.Library.Internal.DataAccess;
using RRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<Models.UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { Id = id };

            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "RRMData");

            return output;
        }
    }
}
