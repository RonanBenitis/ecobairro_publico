using ecobairroServer.Source.Core.Models;
using ecobairroServer.Source.Core.Models.Marcacao;
using ecobairroServer.Source.Core.Models.Pessoa;
using Microsoft.EntityFrameworkCore;

namespace ecobairroServer.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // Tabelas BD
    public DbSet<User> Users { get; set; }
    public DbSet<Fiscal> Fiscais{ get; set; }
    public DbSet<Municipe> Municipes{ get; set; }
    public DbSet<Pin> Pins{ get; set; }
    public DbSet<ChamadasPin> ChamadasPin{ get; set; }
    public DbSet<Bairro> Bairros{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações de índices únicos
        modelBuilder
            .Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder
            .Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder
            .Entity<Municipe>()
            .HasIndex(m => m.Cpf)
            .IsUnique();

        modelBuilder
            .Entity<Fiscal>()
            .HasIndex(f => f.Rgf)
            .IsUnique();

        modelBuilder
            .Entity<Bairro>()
            .HasIndex(m => m.Nome)
            .IsUnique();
    }
}
