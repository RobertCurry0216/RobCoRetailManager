﻿using Microsoft.AspNet.Identity;
using RRMDataManager.Library.DataAccess;
using RRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {


        [HttpPost]
        public void Post(SaleModel sale)
        {
            var data = new SaleData();
            var userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);
        }
    }
}
