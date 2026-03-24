using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

 
public class CoordinatorDTO
{
	[Required]
	public string CoordinatorName { get; set; }

	[Required, EmailAddress]
	public string CoordinatorEmail { get; set; }
	public long CoordinatorPhone { get; set; }
	public string CoordinatorQualification { get; set; }

	public int CoordinatorExperience { get; set; }

	public string CoordinatorGender { get; set; }
	public string CoordinatorPassword { get; set; }
	public IFormFile Resumepath { get; set; }
}