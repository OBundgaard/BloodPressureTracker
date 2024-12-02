namespace Measurement.Context;
using Microsoft.EntityFrameworkCore;
using Measurement.Models;


public class MeasurementDbContext : DbContext
{
    public DbSet<MeasurementModel> Measurements { get; set; }
    public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeasurementModel>().HasKey(p => p.Id);
        modelBuilder.Entity<MeasurementModel>().Property(p => p.Id).ValueGeneratedNever();
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