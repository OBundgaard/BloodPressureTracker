namespace Patient.Repositories;
using System.Diagnostics.Metrics;

using Models;
using Polly;
using Polly.Retry;
using Microsoft.EntityFrameworkCore;
using Patient.Context;

public interface IPatientRepository
{
    Task<IEnumerable<PatientModel>> GetPatientsAsync();
    Task<PatientModel> GetPatientBySSNAsync(string SSN);
    Task<PatientModel> AddPatientAsync(PatientModel patient);
    Task<PatientModel> UpdatePatientAsync(PatientModel patient);
    Task DeletePatientAsync(PatientModel patient);
}

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;
    private readonly AsyncRetryPolicy _retryPolicy;

    public PatientRepository(PatientDbContext context)
    {
        _context = context;
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<PatientModel> AddPatientAsync(PatientModel patient)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        });
    }

    public async Task<PatientModel> GetPatientBySSNAsync(string SSN)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var patient = await _context.Patients.FindAsync(SSN);
            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with SSN {SSN} not found.");
            }
            return patient;
        });
    }

    public async Task<IEnumerable<PatientModel>> GetPatientsAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            return await _context.Patients.ToListAsync();
        });
    }

    public async Task<PatientModel> UpdatePatientAsync(PatientModel patient)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return patient;
        });
    }

    public async Task DeletePatientAsync(PatientModel patient)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        });
    }
}
