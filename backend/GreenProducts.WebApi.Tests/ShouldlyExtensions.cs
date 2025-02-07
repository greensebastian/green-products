using Shouldly;

namespace GreenProducts.WebApi.Tests;

public static class ShouldlyExtensions
{
    public static void ShouldBeEquivalentToUnordered<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
        var actualEnumerated = actual.ToList();
        var expectedEnumerated = expected.ToList();
        actualEnumerated.Count.ShouldBe(expectedEnumerated.Count);
        actualEnumerated.ShouldBeSubsetOf(expectedEnumerated);
    }
}