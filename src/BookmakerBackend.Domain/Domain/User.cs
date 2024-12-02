using System;
using System.Collections.Generic;

namespace BookmakerBackend.Domain.Domain;

public partial class User
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal? Balance { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
