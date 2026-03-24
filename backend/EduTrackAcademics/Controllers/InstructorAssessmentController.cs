using EduTrackAcademics.Data;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Exceptions;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackAcademics.Controllers
{
	[Route("api/instructorAssessmentQuestion")]
	[ApiController]
	public class InstructorAssessmentController : ControllerBase
	{
		private readonly IInstructorService _service;
		private readonly EduTrackAcademicsContext _context;

		public InstructorAssessmentController(IInstructorService service, EduTrackAcademicsContext context)
		{
			_service = service;
			_context = context;
		}
		// ASSESSMENT

		[HttpPost("assessment")]
		public async Task<IActionResult> CreateAssessment([FromBody] AssessmentDTO dto)
		{
			if (dto == null)
			{
				return BadRequest("Invalid data");
			}
			var result=await _service.CreateAssessmentAsync(dto);
			return Ok(new
			{
				Message = result
			});
		}

		[HttpGet("assessmentDetails/{id}")]
		public async Task<IActionResult> GetAssessment(string id)
			=> Ok(await _service.GetAssessmentByIdAsync(id));

		[HttpGet("assessmentQuestions/{id}")]
		public async Task<IActionResult> GetAllQuestions(string id)
		{
			var questions = await _context.Questions
				.Where(q => q.AssessmentId == id)
				.ToListAsync();

			if (questions == null || !questions.Any())
				return NotFound($"No questions found for assessment {id}");

			return Ok(questions);
		}



		[HttpGet("assessments/course/{courseId}")]
		public async Task<IActionResult> GetAssessmentsByCourse(string courseId)
			=> Ok(await _service.GetAssessmentsByCourseAsync(courseId));

		[HttpPut("assessment/{id}")]
		public async Task<IActionResult> UpdateAssessment(string id, AssessmentDTO dto)
			=> Ok(await _service.UpdateAssessmentAsync(id, dto));

		[HttpDelete("assessment/{id}")]
		public async Task<IActionResult> DeleteAssessment(string id)
			=> Ok(await _service.DeleteAssessmentAsync(id));

		[HttpPut("UpdateFeedback/Score")]
		public async Task<IActionResult> UpdateSubmission(UpdateSubmissionDto dto)
		{
			var result = await _service.AddFeedbackAsync(dto);

			if (!result.IsSubmitted)
			{
				return BadRequest(new
				{
					Message = "Student has not submitted the assessment."
				});
			}

			return Ok(new
			{
				Score = result.Score,
				Percentage = result.Percentage,
				Message = "Submission updated successfully."
			});
		}

		// QUESTIONS

		[HttpPost("question")]
		public async Task<IActionResult> AddQuestion(QuestionDTO dto)
	   => Ok(await _service.AddQuestionAsync(dto));

		[HttpGet("question/{id}")]
		public async Task<IActionResult> GetQuestion(string id)
			=> Ok(await _service.GetQuestionByIdAsync(id));

		[HttpGet("questions/assessment/{assessmentId}")]
		public async Task<IActionResult> GetQuestionsByAssessment(string assessmentId)
			=> Ok(await _service.GetQuestionsByAssessmentAsync(assessmentId));

		[HttpPut("question/{id}")]
		public async Task<IActionResult> UpdateQuestion(string id, QuestionDTO dto)
			=> Ok(await _service.UpdateQuestionAsync(id, dto));

		[HttpDelete("question/{id}")]
		public async Task<IActionResult> DeleteQuestion(string id)
			=> Ok(await _service.DeleteQuestionAsync(id));
	}
}
