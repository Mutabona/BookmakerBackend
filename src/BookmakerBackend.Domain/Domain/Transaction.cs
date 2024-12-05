using System;
using System.Collections.Generic;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Domain.Domain;

public partial class Transaction
{
    public Guid? Id { get; set; }
    
    public TransactionType Type { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? DateTime { get; set; }

    public decimal Amount { get; set; }

    public virtual User UsernameNavigation { get; set; } = null!;
}
