using GreenProducts.Core.Products;

namespace GreenProducts.WebApi.Infrastructure;

/// <summary>
/// Special class just to seed some classifications (colurs, product types)
/// </summary>
public static class ProductClassificationsSeed
{
    public static class Ids
    {
        public const string Chair = "1d4fe195-622e-46c6-ae47-39e7d83154a9";
        public const string Plant = "b5c64c7f-7055-4aa0-b8a4-14568fcff4fd";
        public const string Decoration = "71b707ab-2ab3-42b6-8e29-ba91f19181f9";
        public const string Birch = "a6d1a6fe-73fa-4eef-88d0-c1cd58f87f6e";
        public const string Navy = "ac5d2362-6e12-4ff7-9b21-c2dc164b8d14";
        public const string Yellow = "0f2b7812-766a-41f9-a928-b6c259706d7f";
    }

    public static List<ProductClassification> Classifications { get; } = new()
    {
        new()
        {
            Id = Guid.Parse(Ids.Chair),
            Type = ProductClassification.Types.ProductType,
            Value = "CHAIR",
            DisplayName = "Chair",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        },
        new()
        {
            Id = Guid.Parse(Ids.Plant),
            Type = ProductClassification.Types.ProductType,
            Value = "PLANT",
            DisplayName = "Plant",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        },
        new()
        {
            Id = Guid.Parse(Ids.Decoration),
            Type = ProductClassification.Types.ProductType,
            Value = "DECORATION",
            DisplayName = "Decoration",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        },
        new()
        {
            Id = Guid.Parse(Ids.Birch),
            Type = ProductClassification.Types.Colour,
            Value = "BIRCH",
            DisplayName = "Birch tree",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        },
        new()
        {
            Id = Guid.Parse(Ids.Navy),
            Type = ProductClassification.Types.Colour,
            Value = "NAVY",
            DisplayName = "Navy blue",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        },
        new()
        {
            Id = Guid.Parse(Ids.Yellow),
            Type = ProductClassification.Types.Colour,
            Value = "YELLOW",
            DisplayName = "Yellow",
            CreatedOn = DateTimeOffset.Parse("2025-02-07T11:00:00Z")
        }
    };
}