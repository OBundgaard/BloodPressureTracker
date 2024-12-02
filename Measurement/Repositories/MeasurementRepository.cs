namespace Measurement.Repositories;

using Measurement.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using Polly;
using Polly.Retry;

public interface IMeasurementRepository
{
    Task<IEnumerable<MeasurementModel>> GetMeasurementsAsync();
    Task<MeasurementModel> GetMeasurementByIdAsync(int id);
    Task<MeasurementModel> AddMeasurementAsync(MeasurementModel measurement);
    Task<MeasurementModel> UpdateMeasurementAsync(MeasurementModel measurement);
    Task DeleteMeasurementAsync(MeasurementModel measurement);
}

public class MeasurementRepository : IMeasurementRepository
{
    private readonly MeasurementDbContext _context;
    private readonly AsyncRetryPolicy _retryPolicy;

    public MeasurementRepository(MeasurementDbContext context)
    {
        _context = context;
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<MeasurementModel> AddMeasurementAsync(MeasurementModel measurement)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await _context.Measurements.AddAsync(measurement);
            await _context.SaveChangesAsync();
            return measurement;
        });
    }

    public async Task<MeasurementModel> GetMeasurementByIdAsync(int id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                throw new KeyNotFoundException($"Measurement with ID {id} not found.");
            }
            return measurement;
        });
    }

    public async Task<IEnumerable<MeasurementModel>> GetMeasurementsAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            return await _context.Measurements.ToListAsync();
        });
    }

    public async Task<MeasurementModel> UpdateMeasurementAsync(MeasurementModel measurement)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _context.Measurements.Update(measurement);
            await _context.SaveChangesAsync();
            return measurement;
        });
    }

    public async Task DeleteMeasurementAsync(MeasurementModel measurement)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();
        });
    }
}
