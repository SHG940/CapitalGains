using Newtonsoft.Json;

namespace CapitalGains.Domain;

public class TaxSummary
{
    [JsonProperty("tax")]
    public double Tax { get; set; }
}