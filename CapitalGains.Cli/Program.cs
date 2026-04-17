using CapitalGains.Abstractions.Manager;
using CapitalGains.Domain;
using CapitalGains.Managers;
using Newtonsoft.Json;

// TaxRanges allows to introduce changes without need to change base logic
TaxRanges taxRanges = new()
{
    TaxPercentage = 0.2f,
    MinProfitToTax = 20000
};
// ITaxManager allows to take the logic in any other app such as another CLI, ClassLibrary or API (SOLID principles)
ITaxManager taxManager = new TaxManager();
var lines = new List<string>();

while (true)
{
    var input = Console.ReadLine();

    // Context definition says the result has to be shown when empty line is enter.
    // Using Tasks here could show the results wrongly because a line, adding code to fix that just increase the complexity
    // I could, but just wanted to write here why I didn't do it
    if (!string.IsNullOrWhiteSpace(input))
    {
        lines.Add(input);
        continue;
    }

    foreach (var line in lines)
    {
        var transactions = JsonConvert.DeserializeObject<IEnumerable<Transaction>>(line);
        var taxSummary = taxManager.CalculateTaxes(transactions, taxRanges);

        Console.WriteLine(JsonConvert.SerializeObject(taxSummary));
    }

    break;
}