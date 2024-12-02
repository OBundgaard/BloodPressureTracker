using Microsoft.AspNetCore.Mvc;
using Patient.Repositories;
using Patient.Models;
using FeatureHubSDK;


namespace Patient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientRepository patientRepository, IClientContext featurehub) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientModel>>> Get()
    {
        if (!featurehub["PatientGet"].IsEnabled)
        {
            return NoContent();
        }

        var patients = await patientRepository.GetPatientsAsync();
        return Ok(patients);
    }

    [HttpGet("{SSN}")]
    public async Task<ActionResult<PatientModel>> Get(string SSN)
    {
        if (!featurehub["PatientGetBySSN"].IsEnabled)
        {
            return NoContent();
        }

        var patient = await patientRepository.GetPatientBySSNAsync(SSN);
        if (patient == null)
        {
            return NotFound();
        }
        return Ok(patient);
    }

    [HttpPost("AddPatient")]
    public async Task<ActionResult<PatientModel>> Post([FromBody] PatientModel patient)
    {
        if (featurehub["PatientPost"].IsEnabled)
        {
            return NoContent();
        }

        var createdPatient = await patientRepository.AddPatientAsync(patient);
        return CreatedAtAction(nameof(Get), new { SSN = createdPatient.SSN }, createdPatient);
    }

    [HttpPut("{SSN}")]
    public async Task<IActionResult> Put(string SSN, [FromBody] PatientModel patient)
    {
        if (!featurehub["PatientPut"].IsEnabled)
        {
            return NoContent();
        }

        if (SSN != patient.SSN)
        {
            return BadRequest();
        }

        await patientRepository.UpdatePatientAsync(patient);
        return NoContent();
    }

    [HttpDelete("{SSN}")]
    public async Task<IActionResult> Delete(string SSN)
    {
        if (!featurehub["PatientDelete"].IsEnabled)
        {
            return NoContent();
        }

        var patient = await patientRepository.GetPatientBySSNAsync(SSN);
        if (patient == null)
        {
            return NotFound();
        }

        await patientRepository.DeletePatientAsync(patient);
        return NoContent();
    }
}
