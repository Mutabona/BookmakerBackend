using System;
using System.Collections.Generic;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Domain.Domain;

public partial class Bet
{
    public Guid? Id { get; set; }

    public Guid CoefficientId { get; set; }
    
    public BetStatus? Status { get; set; }

    public string Username { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual Coefficient Coefficient { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;
}
