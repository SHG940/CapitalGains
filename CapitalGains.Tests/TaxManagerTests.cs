using CapitalGains.Abstractions.Manager;
using CapitalGains.Domain;
using CapitalGains.Domain.Enum;
using CapitalGains.Managers;
using FluentAssertions;

namespace CapitalGains.Tests;

public class TaxManagerTests
{
    private readonly ITaxManager _taxManager = new TaxManager();
    private readonly TaxRanges _defaultTaxRanges = new()
    {
        TaxPercentage = 0.2f,
        MinProfitToTax = 20000
    };

    [Fact]
    public void Case0()
    {
        // Act
        var taxSummary = _taxManager.CalculateTaxes([], _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNull();
        taxSummary.Should().BeEmpty();
    }

    [Fact]
    public void Case1_NoTaxes()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new  ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 100
            },
            new  ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 50
            },
            new  ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 50
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(3);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
    }

    [Fact]
    public void Case2()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 5.00f,
                Quantity = 5000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(3);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(10000);
        taxSummary[2].Tax.Should().Be(0);
    }

    [Fact]
    public void Case1Plus2()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 100
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 50
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 50
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 5.00f,
                Quantity = 5000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(6);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(0);
        taxSummary[4].Tax.Should().Be(10000);
        taxSummary[5].Tax.Should().Be(0);
    }

    [Fact]
    public void Case3()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 5.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 3000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(3);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(1000);
    }

    [Fact]
    public void Case4()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 25.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 10000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(3);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
    }

    [Fact]
    public void Case5()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 25.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 25.00f,
                Quantity = 5000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(4);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(10000);
    }

    [Fact]
    public void Case6()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 2.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 2000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 2000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 25.00f,
                Quantity = 1000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(5);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(0);
        taxSummary[4].Tax.Should().Be(3000);
    }

    [Fact]
    public void Case7()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 2.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 2000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20.00f,
                Quantity = 2000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 25.00f,
                Quantity = 1000
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 20.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15.00f,
                Quantity = 5000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 30.00f,
                Quantity = 4350
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 30.00f,
                Quantity = 650
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(9);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(0);
        taxSummary[4].Tax.Should().Be(3000);
        taxSummary[5].Tax.Should().Be(0);
        taxSummary[6].Tax.Should().Be(0);
        taxSummary[7].Tax.Should().Be(3700);
        taxSummary[8].Tax.Should().Be(0);
    }

    [Fact]
    public void Case8()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 10.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 50.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 20.00f,
                Quantity = 10000
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 50.00f,
                Quantity = 10000
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(4);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(80000);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(60000);
    }

    [Fact]
    public void Case9()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 5000.00f,
                Quantity = 10
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 4000.00f,
                Quantity = 5
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 15000.00f,
                Quantity = 5
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 4000.00f,
                Quantity = 2
            },
            new ()
            {
                Operation = Operation.Buy,
                UnitCost = 23000.00f,
                Quantity = 2
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 20000.00f,
                Quantity = 1
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 12000.00f,
                Quantity = 10
            },
            new ()
            {
                Operation = Operation.Sell,
                UnitCost = 15000.00f,
                Quantity = 3
            }
        };

        // Act
        var taxSummary = _taxManager.CalculateTaxes(transactions, _defaultTaxRanges).ToArray();

        // Assert
        taxSummary.Should().NotBeNullOrEmpty();
        taxSummary.Length.Should().Be(8);
        taxSummary[0].Tax.Should().Be(0);
        taxSummary[1].Tax.Should().Be(0);
        taxSummary[2].Tax.Should().Be(0);
        taxSummary[3].Tax.Should().Be(0);
        taxSummary[4].Tax.Should().Be(0);
        taxSummary[5].Tax.Should().Be(0);
        taxSummary[6].Tax.Should().Be(1000);
        taxSummary[7].Tax.Should().Be(2400);
    }
}