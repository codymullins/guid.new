namespace Guid.Models;

public record SingleResult(System.Guid Value);
public record ManyResult(IEnumerable<SingleResult> Items);
public record Sorry(string Message);