using EduTrackAcademics.DTO;
using EduTrackAcademics.Services;
using Microsoft.AspNetCore.Mvc;
using EduTrackAcademics.AuthFolder;
namespace EduTrackAcademics.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthenticationService _authService;

		public AuthController(IAuthenticationService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO dto)
		{
			var token = await _authService.LoginAsync(dto);
			if (token == null) return Unauthorized(new { Message = "Invalid email or password." });

			return Ok(new { Token = token });
		}

		[HttpPost("generate-otp")]
		public async Task<IActionResult> GenerateOtp([FromBody] EmailRequest request)
		{
			var otp = await _authService.GenerateOtpAsync(request.Email);
			if (otp == null) return NotFound(new { Message = "User not found." });

			return Ok(new { Message = "Verification code sent to your email." });
		}

		[HttpPost("verify-email")]
		public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
		{
			var result = await _authService.VerifyEmailAsync(dto);
			if (!result) return BadRequest(new { Message = "Invalid or expired OTP." });

			return Ok(new { Message = "Email verified successfully." });
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] EmailRequest request)
		{
			var token = await _authService.GenerateResetTokenAsync(request.Email);
			if (token == null) return NotFound(new { Message = "User not found." });

			return Ok(new { Message = "Reset token sent to your email." });
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var result = await _authService.ResetPasswordAsync(dto);
			if (!result) return BadRequest(new { Message = "Invalid token or expired." });

			return Ok(new { Message = "Password has been reset successfully." });
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromBody] EmailRequest request)
		{
			var result = await _authService.LogoutAsync(request.Email);
			if (!result) return NotFound(new { Message = "User not found." });

			return Ok(new { Message = "Logged out successfully." });
		}
	}
}