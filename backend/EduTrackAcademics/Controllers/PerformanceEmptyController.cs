using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace EduTrackAcademics.Controllers
{
    [ApiController]

    [Route("api/[controller]")]

    public class PerformanceController : ControllerBase

    {

        private readonly IPerformanceService _service;

        public PerformanceController(IPerformanceService service)

        {

            _service = service;

        }

        // ✅ Average Score

       // [Authorize(Roles = "Instructor,Admin")]

        [HttpGet("average/{enrollmentId}")]

        public async Task<IActionResult> GetAverageScore(string enrollmentId)

        {

            var result = await _service.GetAverageScoreAsync(enrollmentId);

            return Ok(result);

        }

        // ✅ Last Updated

      //  [Authorize(Roles = "Instructor,Admin")]

        [HttpGet("lastupdated/{enrollmentId}")]

        public async Task<IActionResult> GetLastUpdated(string enrollmentId)

        {

            var result = await _service.GetLastModifiedDateAsync(enrollmentId);

            return Ok(result);

        }

        // ✅ Instructor batches

      //  [Authorize(Roles = "Instructor,Admin")]

        [HttpGet("instructor-batches/{instructorId}")]

        public async Task<IActionResult> GetInstructorBatches(string instructorId)

        {

            var result = await _service.GetInstructorBatchesAsync(instructorId);

            return Ok(result);

        }

        // ✅ Batch Performance (Coordinator + Admin also allowed)

     //   [Authorize(Roles = "Instructor,Admin,Coordinator")]

        [HttpGet("batch-performance/{batchId}")]

        public async Task<IActionResult> GetBatchPerformance(string batchId)

        {

            var result = await _service.GetBatchPerformanceAsync(batchId);

            return Ok(result);

        }

    }
}


