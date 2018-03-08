using FoodieBuddy.Domain.Models.MailingList;
using FoodieBuddy.Domain.Models.MenuItems;
using FoodieBuddy.Domain.Models.Reservations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Infrastructure.Persistence
{
    public class FoodieBuddyDbContext: DbContext, IFoodieBuddyDbContext
    {
        public DbSet<Mail> Mails { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public FoodieBuddyDbContext(DbContextOptions<FoodieBuddyDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mail>()
                .ToTable("MailingList")
                .HasKey(KeyExtensions => KeyExtensions.MailingId);

            modelBuilder.Entity<MenuItem>()
                .ToTable("MenuItem")
                .HasKey(KeyExtensions => KeyExtensions.FoodId);

            modelBuilder.Entity<Reservation>()
                .ToTable("Reservation")
                .HasKey(KeyExtensions => KeyExtensions.ReservationId);
        }
    }
}
