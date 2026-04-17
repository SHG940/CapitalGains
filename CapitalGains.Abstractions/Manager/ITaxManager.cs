using CapitalGains.Domain;

namespace CapitalGains.Abstractions.Manager;

public interface ITaxManager
{
    IEnumerable<TaxSummary> CalculateTaxes(IEnumerable<Transaction> transactions, TaxRanges taxRanges);
}