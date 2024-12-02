using Measurement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Measurement.Models;
using FeatureHubSDK;

namespace Measurement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeasurementController(IMeasurementRepository measurementRepository, IClientContext featurehub) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementModel>>> Get()
    {
        if (!featurehub["MeasurementGet"].IsEnabled)
        {
            return NoContent();
        }

        var measurements = await measurementRepository.GetMeasurementsAsync();
        return Ok(measurements);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeasurementModel>> Get(int id)
    {
        if (!featurehub["MeasurementGetById"].IsEnabled)
        {
            return NoContent();
        }

        var measurement = await measurementRepository.GetMeasurementByIdAsync(id);
        if (measurement == null)
        {
            return NotFound();
        }
        return Ok(measurement);
    }

    [HttpPost]
    public async Task<ActionResult<MeasurementModel>> Post([FromBody] MeasurementModel measurement)
    {
        if (featurehub["MeasurementPost"].IsEnabled)
        {
            return NoContent();
        }

        var createdMeasurement = await measurementRepository.AddMeasurementAsync(measurement);
        return CreatedAtAction(nameof(Get), new { id = createdMeasurement.Id }, createdMeasurement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] MeasurementModel measurement)
    {
        if (!featurehub["MeasurementPut"].IsEnabled)
        {
            return NoContent();
        }

        if (id != measurement.Id)
        {
            return BadRequest();
        }

        await measurementRepository.UpdateMeasurementAsync(measurement);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!featurehub["MeasurementDelete"].IsEnabled)
        {
            return NoContent();
        }

        var measurement = await measurementRepository.GetMeasurementByIdAsync(id);
        if (measurement == null)
        {
            return NotFound();
        }

        await measurementRepository.DeleteMeasurementAsync(measurement);
        return NoContent();
    }
}
