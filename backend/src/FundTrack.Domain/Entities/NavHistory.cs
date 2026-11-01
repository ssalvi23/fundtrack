using FundTrack.Domain.Common;

namespace FundTrack.Domain.Entities;

public sealed class NavHistory : Entity
{
    private NavHistory()
    {
    }

    public Guid SchemeId { get; private set; }
    public DateOnly NavDate { get; private set; }
    public decimal Nav { get; private set; }

    public static NavHistory Record(Guid schemeId, DateOnly navDate, decimal nav)
    {
        if (nav <= 0)
        {
            throw new DomainException("NAV must be greater than zero.");
        }

        return new NavHistory { SchemeId = schemeId, NavDate = navDate, Nav = nav };
    }
}
