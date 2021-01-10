using System.Configuration;

namespace RRMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            // TODO: Move this to the Api
            string rateText = ConfigurationManager.AppSettings["TaxRate"];
            return decimal.TryParse(rateText, out decimal output)
            ? output
            : throw new ConfigurationErrorsException("Config Error: No tax rate found, app.config is not set up correct.");
        }
    }
}