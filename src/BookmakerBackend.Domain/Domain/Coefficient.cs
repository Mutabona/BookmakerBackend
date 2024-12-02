using System;
using System.Collections.Generic;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Domain.Domain;

public partial class Coefficient
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }
    
    public CoefficientType Type { get; set; }

    public decimal Value { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();

    public virtual Event Event { get; set; } = null!;
}
