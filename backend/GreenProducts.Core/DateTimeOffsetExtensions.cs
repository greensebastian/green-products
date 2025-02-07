namespace GreenProducts.Core;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset Truncate(this DateTimeOffset dateTimeOffset, long roundTicks) => new(dateTimeOffset.Ticks - dateTimeOffset.Ticks % roundTicks, dateTimeOffset.Offset);
}