using System.Configuration;

namespace RRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            // TODO: Move this to the Api
            string rateText = ConfigurationManager.AppSettings["TaxRate"];
            return decimal.TryParse(rateText, out decimal output)
            ? output
            : throw new ConfigurationErrorsException("Config Error: No tax rate found, app.config is not set up correct.");
        }
    }
}