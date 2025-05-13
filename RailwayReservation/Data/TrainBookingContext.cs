using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainBooking.Models;

namespace TrainBooking.Data
{
    public class TrainBookingContext : IdentityDbContext<User>
    {
        public TrainBookingContext(DbContextOptions<TrainBookingContext> options)
            : base(options)
        {
        }
     public DbSet<User> Users { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<TrainSchedule> TrainSchedules { get; set; }
    public DbSet<SeatAvailability> SeatAvailabilities { get; set; }
    public DbSet<ClassType> ClassTypes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<WaitingList> WaitingLists { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder
        );
        // Ticket → User
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u=>u.Tickets)
            .HasForeignKey(t => t.Username)
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket → Train
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Train)
            .WithMany(t=>t.Tickets)
            .HasForeignKey(t => t.TrainID)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket → ClassType
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.ClassType)
            .WithMany(c=>c.Tickets)
            .HasForeignKey(t => t.ClassTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        // TrainSchedule → Train
        modelBuilder.Entity<TrainSchedule>()
            .HasOne(ts => ts.Train)
            .WithMany(t=>t.TrainSchedules)
            .HasForeignKey(ts => ts.TrainID)
            .OnDelete(DeleteBehavior.Cascade);

        // TrainSchedule → Station
        modelBuilder.Entity<TrainSchedule>()
            .HasOne(ts => ts.Station)
            .WithMany(s=>s.TrainSchedules)
            .HasForeignKey(ts => ts.StationID)
            .OnDelete(DeleteBehavior.Cascade);

        // SeatAvailability → Train
        modelBuilder.Entity<SeatAvailability>()
            .HasOne(sa => sa.Train)
            .WithMany(t=>t.SeatAvailabilities)
            .HasForeignKey(sa => sa.TrainID)
            .OnDelete(DeleteBehavior.Restrict);

        // SeatAvailability → ClassType
        modelBuilder.Entity<SeatAvailability>()
            .HasOne(sa => sa.ClassType)
            .WithMany(c=>c.SeatAvailabilities)
            .HasForeignKey(sa => sa.ClassTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification → User
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u=>u.Notifications)
            .HasForeignKey(n => n.Username)
            .OnDelete(DeleteBehavior.Cascade);

        // Notification → Train
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Train)
            .WithMany(t=>t.Notifications)
            .HasForeignKey(n => n.TrainID)
            .OnDelete(DeleteBehavior.Cascade);

        // Passenger → Ticket
        modelBuilder.Entity<Passenger>()
            .HasOne(p => p.Ticket)
            .WithMany(t=>t.Passengers)
            .HasForeignKey(p => p.TicketID)
            .OnDelete(DeleteBehavior.Cascade);

        // Payment → Ticket
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Ticket)
            .WithMany()
            .HasForeignKey(p => p.TicketID)
            .OnDelete(DeleteBehavior.Cascade);

        // WaitingList → Ticket
        modelBuilder.Entity<WaitingList>()
            .HasOne(wl => wl.Ticket)
            .WithMany()
            .HasForeignKey(wl => wl.TicketID)
            .OnDelete(DeleteBehavior.Cascade);

        // WaitingList → Train
        modelBuilder.Entity<WaitingList>()
            .HasOne(wl => wl.Train)
            .WithMany()
            .HasForeignKey(wl => wl.TrainID)
            .OnDelete(DeleteBehavior.Restrict);

        // WaitingList → ClassType
        modelBuilder.Entity<WaitingList>()
            .HasOne(wl => wl.ClassType)
            .WithMany()
            .HasForeignKey(wl => wl.ClassTypeID)
            .OnDelete(DeleteBehavior.Restrict);
    
}
}
}
