using CapitalGains.Domain.Enum;
using Newtonsoft.Json;

namespace CapitalGains.Domain;

public class Transaction
{
    [JsonProperty("operation")]
    public Operation Operation { get; set; }

    [JsonProperty("unit-cost")]
    public float UnitCost { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}