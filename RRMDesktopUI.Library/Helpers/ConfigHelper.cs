using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["TaxRate"];
            return decimal.TryParse(rateText, out decimal output)
            ? output
            : throw new ConfigurationErrorsException("Config Error: No tax rate found, app.config is not set up correct.");
        }
    }
}
