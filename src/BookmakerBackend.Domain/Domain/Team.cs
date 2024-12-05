using System;
using System.Collections.Generic;

namespace BookmakerBackend.Domain.Domain;

public partial class Team
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public string Sport { get; set; } = null!;

    public virtual ICollection<Event> EventTeam1s { get; set; } = new List<Event>();

    public virtual ICollection<Event> EventTeam2s { get; set; } = new List<Event>();
}
