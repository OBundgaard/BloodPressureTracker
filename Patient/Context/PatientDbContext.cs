namespace Patient.Context;

using Microsoft.EntityFrameworkCore;
using Models;

public class PatientDbContext : DbContext
{
    public DbSet<PatientModel> Patients { get; set; }

    public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientModel>().HasKey(p => p.SSN);
        modelBuilder.Entity<PatientModel>().Property(p => p.SSN).ValueGeneratedNever();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while saving changes to the database.", ex);
        }
    }
}
