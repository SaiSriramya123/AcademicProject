using System.Text;
using EduTrackAcademics.Aspects;

using EduTrackAcademics.AuthFolder;

using EduTrackAcademics.Data;
using EduTrackAcademics.Dummy;
using EduTrackAcademics.Model;
using EduTrackAcademics.Repository;
using EduTrackAcademics.Service;
using EduTrackAcademics.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;





var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<EduTrackAcademicsContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("EduTrackAcademicsContext")
		?? throw new InvalidOperationException("Connection string not found")
	));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<ICoordinatorDashboardRepo, CoordinatorDashboardRepo>();
builder.Services.AddScoped<ICoordinatorDashboardService, CoordinatorDashboardService>();

builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
builder.Services.AddSingleton<DummyInstructorData>();

builder.Services.AddSingleton<DummyInstructor>();
builder.Services.AddSingleton<DummyStudent>();
builder.Services.AddSingleton<DummyInstructorReg>();

builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IdService>();
builder.Services.AddScoped<PasswordService>();
//builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<IPerformanceService, PerformanceService>();

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

//builder.Services.AddScoped<IStudentProgressesRepository, StudentProgressesRepository>();
//builder.Services.AddScoped<IStudentProgressesService, StudentProgressesService>();

// Student Profile
builder.Services.AddScoped<IStudentProfileService, StudentProfileService>();
builder.Services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();


// Auth
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<JWTTokenGenerator>();

//for authentication
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();





// =======================
// JWT Authentication
// =======================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])
			),
			ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
			ValidAudience = builder.Configuration["JwtSettings:Audience"]
		};
	});

builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();

builder.Services.AddScoped<IAcademicReportRepository, AcademicReportRepository>();
builder.Services.AddScoped<IAcademicReportService, AcademicReportService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();



builder.Services.AddCors(options =>
{
	options.AddPolicy("MyCorsPolicy", builder => builder
		.WithOrigins("http://localhost:5173")
		.AllowAnyMethod()
		.AllowCredentials()
		.WithHeaders("Accept", "Content-Type", "Origin", "X-My-Header"));

});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<EduTrackAcademicsContext>();
	var admin = context.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");

	if (admin != null)
	{
		admin.Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"); // Recompute fresh hash
		context.SaveChanges();
	}
}

// =======================
// Middleware
// =======================
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API v1");
	c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("MyCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
 

app.Run();
