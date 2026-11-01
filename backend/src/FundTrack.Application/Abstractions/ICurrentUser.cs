namespace FundTrack.Application.Abstractions;

/// <summary>Who is making the change. Defined here, implemented outside, so Application stays free of HTTP.</summary>
public interface ICurrentUser
{
    string Name { get; }
}
