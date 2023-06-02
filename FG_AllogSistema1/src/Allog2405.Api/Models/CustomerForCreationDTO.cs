using System.ComponentModel.DataAnnotations;
namespace Allog2405.Api.Models;

public class CustomerForCreationDTO
{
    [Required(ErrorMessage = "Identify yourself if you do not want to die a gruesome death")]
    [MaxLength(100, ErrorMessage = "Nice try boyo")]
    public string FirstName {get; set;} = string.Empty;
    
    [Required(ErrorMessage = "Your last name, honey?")]
    [MaxLength(100, ErrorMessage = "Nice try boyo")]
    public string LastName {get; set;} = string.Empty;
    
    [Required(ErrorMessage = "Fill out your CPF lol")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid CPF, retard")]
    public string Cpf {get; set;} = string.Empty;

}