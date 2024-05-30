using Microsoft.EntityFrameworkCore;

namespace loginDemo.Models;

public partial class WebAppDataBaseContext : DbContext
{
    public WebAppDataBaseContext()
    {
    }

    public WebAppDataBaseContext(DbContextOptions<WebAppDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<LogInformation> LogInformations { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress; Database=WebAppDataBase; Trusted_Connection=True; TrustServerCertificate=True;");*/
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = WebApplication.CreateBuilder();
        var connectionString = builder.Configuration.GetConnectionString ("MyConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_Reservations_RoomId");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations).HasForeignKey(d => d.RoomId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
