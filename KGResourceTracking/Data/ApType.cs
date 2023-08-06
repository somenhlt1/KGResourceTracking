using System.ComponentModel;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.Data;

[PrimaryKey(nameof(Id), nameof(Type))]
public class ApType
{
    public int Id { get; set; }
    public ApTypeEnum Type { get; set; }
    public int Count { get; set; }
}
public enum ApTypeEnum
{
    [Description("200")]
    TwoHundred,
    [Description("100")]
    OneHunred,
    [Description("50")]
    Fifty,
    [Description("20")]
    Twenty,
    [Description("10")]
    Tenth,
    [Description("5")]
    Five
}