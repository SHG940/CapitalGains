using CapitalGains.Abstractions.Manager;
using CapitalGains.Domain;
using CapitalGains.Domain.Enum;
using CapitalGains.Managers.InternalModels;

namespace CapitalGains.Managers;

public class TaxManager : ITaxManager
{
    public IEnumerable<TaxSummary> CalculateTaxes(IEnumerable<Transaction> transactions, TaxRanges taxRanges)
    {
        var taxSummary = new List<TaxSummary>();

        if (!transactions.Any()) return taxSummary;

        var stock = new StockStatus();

        taxSummary.AddRange(transactions.Select(transaction => transaction.Operation == Operation.Buy
            ? ApplyBuyTransaction(transaction, stock)
            : ApplySellTransaction(transaction, stock, taxRanges))
        );

        return taxSummary;
    }

    private static TaxSummary ApplyBuyTransaction(Transaction transaction, StockStatus currentStock)
    {
        currentStock.WeightedAverage = Math.Round((currentStock.Quantity * currentStock.WeightedAverage + transaction.Quantity * transaction.UnitCost)
                                                        / (currentStock.Quantity + transaction.Quantity), 2);

        currentStock.Quantity += transaction.Quantity;

        return new TaxSummary();
    }

    private static TaxSummary ApplySellTransaction(Transaction transaction, StockStatus currentStock, TaxRanges taxRanges)
    {
        // Based on rules this scenario do not supposed to happened
        // if (transaction.Quantity > currentStock.Quantity) throw new ArgumentException("Not stock enough");

        var transactionAmount = transaction.Quantity * transaction.UnitCost;
        var cost = transaction.Quantity * currentStock.WeightedAverage;
        var overallProfit = transactionAmount - cost;
        var applyTaxes = overallProfit > 0 && transactionAmount > taxRanges.MinProfitToTax;
        var profitAfterLosses = overallProfit + (applyTaxes ? currentStock.Losses : 0);

        currentStock.Quantity -= transaction.Quantity;

        if (profitAfterLosses < 0)
            currentStock.Losses = profitAfterLosses;
        else if (applyTaxes && currentStock.Losses < 0) // Loses stored in negative numbers simplify the operations in terms of code
        {
            if (profitAfterLosses < 0)
                currentStock.Losses += overallProfit;
            else
                currentStock.Losses = 0;
        }

        var tax = Math.Round(applyTaxes ? Math.Max(0, profitAfterLosses) * taxRanges.TaxPercentage : 0, 2);

        return new ()
        {
            Tax = tax
        };
    }
}