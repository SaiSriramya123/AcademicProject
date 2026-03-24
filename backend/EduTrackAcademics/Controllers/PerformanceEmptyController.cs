using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace EduTrackAcademics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceEmptyController : ControllerBase
    {
        //this part is done to connect this controller to the database to fetch the data.
        private readonly IPerformanceService _service;
        public PerformanceEmptyController(IPerformanceService service)
        {
            _service = service;
        }

      
        [HttpGet("average/{enrollmentId}")]
        public IActionResult GetAverageScore(string enrollmentId)
        {
            var result= _service.GetAverageScore(enrollmentId);
            return Ok(result);
        }


        [HttpGet("lastupdated/{enrollmentId}")]
        public IActionResult GetLastUpdated(string enrollmentId)
        {
            var result = _service.GetLastModifiedDate(enrollmentId);
            return Ok(result);
        }



        [HttpGet("instructor-batches/{instructorId}")]
        public IActionResult GetInstructorBatches(string instructorId)

        {
          var result = _service.GetInstructorBatches(instructorId);

            return Ok(result);
        }



        [HttpGet("batch-performance/{batchId}")]

        public IActionResult GetBatchPerformance(string batchId)

        {

            var result = _service.GetBatchPerformance(batchId);

            return Ok(result);

        }

        

        }

    }


   