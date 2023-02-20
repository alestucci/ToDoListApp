using EntityFrameworkClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using ToDoListApp;

namespace EntityFrameworkClassLibrary
{
    public class ApplicationDbContext:  DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Todo>(entity =>
            //{
            //    entity.ToTable("Todo");
            //    entity.Property(e => e.TaskDescription).IsRequired().HasMaxLength(100);
            //});
            modelBuilder.Entity<Todo>().HasData(
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskDescription = "Comprare il pane",
                    IsCompleted = false
                },
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskDescription = "Pagare le bollette",
                    IsCompleted = false
                },
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskDescription = "Andare in banca",
                    IsCompleted = false
                }
                );
        }

        //public override int SaveChanges()
        //{
        //    var entries = ChangeTracker
        //    .Entries()
        //    .Where(e => e.Entity is Timestamps && (
        //            e.State == EntityState.Added
        //            || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        ((Timestamps)entityEntry.Entity).Created = DateTime.Now;

        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            ((Timestamps)entityEntry.Entity).Updated = DateTime.Now;
        //        }
        //    }

        //    return base.SaveChanges();
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Timestamps;
                if (auditableEntity != null)
                {
                    auditableEntity.Created = DateTime.Now;
                    auditableEntity.Updated = DateTime.Now;
                }
            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as Timestamps;
                if (auditableEntity != null)
                {
                    auditableEntity.Updated = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
