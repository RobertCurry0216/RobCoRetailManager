using RRMDataManager.Library.Internal.DataAccess;
using RRMDataManager.Library.Models;
using System.Collections.Generic;

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