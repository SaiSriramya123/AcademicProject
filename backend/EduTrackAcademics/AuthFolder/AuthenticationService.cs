using System.Security.Cryptography;
using  EduTrackAcademics.AuthFolder;
using EduTrackAcademics.DTO;
using EduTrackAcademics.Model;
using EduTrackAcademics.Services;

namespace EduTrackAcademics.AuthFolder
{
	public interface IAuthenticationService
	{
		    Task<string> LoginAsync(LoginDTO dto);
		    Task<string> GenerateResetTokenAsync(string email);	
			Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
			Task<string> GenerateOtpAsync(string email);
			Task<bool> VerifyEmailAsync(VerifyEmailDto dto);
		    
		    Task<bool> LogoutAsync(string email);
	}

		public class AuthenticationService : IAuthenticationService
		{
			private readonly IAuthenticationRepository _repo;
		    private readonly IEmailService _emailService;
		    private readonly JWTTokenGenerator _jwtService;

			public AuthenticationService(IAuthenticationRepository repo, IEmailService emailService, JWTTokenGenerator jwtService)
			{
				_repo = repo;
			    _emailService = emailService;
				_jwtService = jwtService;
			}

			// 1. LOGIN
			public async Task<string> LoginAsync(LoginDTO dto)
			{
				var user = await _repo.GetUserByEmailAsync(dto.Email);
				if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
					return null;

				return _jwtService.GenerateToken(user);
			}

			// 2. GENERATE OTP (Email Verification)
			public async Task<string> GenerateOtpAsync(string email)
			{
				var user = await _repo.GetUserByEmailAsync(email);
				if (user == null) return null;

				var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
				user.VerificationOtp = otp;
				user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

				await _repo.UpdateUserAsync(user);
				await _emailService.SendEmailAsync(email, "EduTrack Verification Code", $"Your OTP is: <b>{otp}</b>");
				return otp;
			}

			// 3. VERIFY EMAIL
			public async Task<bool> VerifyEmailAsync(VerifyEmailDto dto)
			{
				var user = await _repo.GetUserByEmailAsync(dto.Email);

				if (user == null || user.VerificationOtp != dto.Otp || user.OtpExpiry < DateTime.UtcNow)
					return false;

				user.IsEmailVerified = true;
				user.VerificationOtp = null; // Clear OTP after use
				user.OtpExpiry = null;

				await _repo.UpdateUserAsync(user);
				return true;
			}

			// 4. GENERATE RESET TOKEN (Forgot Password)
			public async Task<string> GenerateResetTokenAsync(string email)
			{
				var user = await _repo.GetUserByEmailAsync(email);
				if (user == null) return null;

				// Generate a secure unique token
				var token = Guid.NewGuid().ToString();
				user.ResetToken = token;
				user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

				await _repo.UpdateUserAsync(user);
				await _emailService.SendEmailAsync(email, "Password Reset Request",
					$"Use this token to reset your password: <b>{token}</b>. Valid for 30 minutes.");

				return token;
			}

			// 5. RESET PASSWORD
			public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
			{
				var user = await _repo.GetUserByEmailAsync(dto.Email);

				if (user == null || user.ResetToken != dto.Token || user.ResetTokenExpiry < DateTime.UtcNow)
					return false;

				// Hash the new password and clear the reset token
				user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
				user.ResetToken = null;
				user.ResetTokenExpiry = null;

				await _repo.UpdateUserAsync(user);
				return true;
			}

			// 6. LOGOUT
			public async Task<bool> LogoutAsync(string email)
			{
				var user = await _repo.GetUserByEmailAsync(email);
				if (user == null) return false;

				// For stateless JWT, logout usually clears server-side session markers 
				// or Refresh Tokens if you have them implemented.
				user.ResetToken = null;
				user.VerificationOtp = null;

				await _repo.UpdateUserAsync(user);
				return true;
			}
		}
}





