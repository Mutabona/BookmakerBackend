using System;
using System.Collections.Generic;
using BookmakerBackend.Domain.Enums;

namespace BookmakerBackend.Domain.Domain;

public partial class Event
{
    public Guid Id { get; set; }
    
    public ResultStatus Result { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public Guid? Team1Id { get; set; }

    public Guid? Team2Id { get; set; }

    public string Sport { get; set; } = null!;

    public decimal? Margin { get; set; }

    public virtual ICollection<Coefficient> Coefficients { get; set; } = new List<Coefficient>();

    public virtual Team? Team1 { get; set; }

    public virtual Team? Team2 { get; set; }
}
