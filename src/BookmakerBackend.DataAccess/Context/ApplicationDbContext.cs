using BookmakerBackend.Domain.Domain;
using BookmakerBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bet> Bets { get; set; }

    public virtual DbSet<Coefficient> Coefficients { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=password;Port=5432");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("bet_status", new[] { "win", "lost", "in_progress" })
            .HasPostgresEnum("coefficient_type", new[] { "win_team1", "win_team2", "draw" })
            .HasPostgresEnum("event_status", new[] { "scheduled", "in_progress", "completed" })
            .HasPostgresEnum("result_status", new[] { "win_team1", "win_team2", "draw", "in_progress" })
            .HasPostgresEnum("transaction_type", new[] { "deposit", "withdrawal" })
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Bet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bet_pkey");

            entity.ToTable("bet");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CoefficientId).HasColumnName("coefficient_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Coefficient).WithMany(p => p.Bets)
                .HasForeignKey(d => d.CoefficientId)
                .HasConstraintName("bet_coefficient_id_fkey");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Bets)
                .HasForeignKey(d => d.Username)
                .HasConstraintName("bet_username_fkey");

            entity
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (BetStatus)Enum.Parse(typeof(BetStatus), v)
                    );
        });

        modelBuilder.Entity<Coefficient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("coefficient_pkey");

            entity.ToTable("coefficient");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Value)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("1")
                .HasColumnName("value");

            entity.HasOne(d => d.Event).WithMany(p => p.Coefficients)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("coefficient_event_id_fkey");

            entity
                .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<CoefficientType>(v));
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.Margin)
                .HasPrecision(5, 2)
                .HasColumnName("margin");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Sport)
                .HasMaxLength(50)
                .HasColumnName("sport");
            entity.Property(e => e.Team1Id).HasColumnName("team1_id");
            entity.Property(e => e.Team2Id).HasColumnName("team2_id");

            entity.HasOne(d => d.Team1).WithMany(p => p.EventTeam1s)
                .HasForeignKey(d => d.Team1Id)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("event_team1_id_fkey");

            entity.HasOne(d => d.Team2).WithMany(p => p.EventTeam2s)
                .HasForeignKey(d => d.Team2Id)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("event_team2_id_fkey");

            entity
                .Property(e => e.Result)
                .HasConversion(
                    e => e.ToString(),
                    e => (ResultStatus)Enum.Parse(typeof(ResultStatus), e));
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Sport)
                .HasMaxLength(50)
                .HasColumnName("sport");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pkey");

            entity.ToTable("transaction");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Username)
                .HasConstraintName("transaction_username_fkey");
            
            entity
                .Property(e => e.Type)
                .HasConversion(
                    e => e.ToString(),
                    e => (TransactionType)Enum.Parse(typeof(TransactionType), e));
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("customer_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
