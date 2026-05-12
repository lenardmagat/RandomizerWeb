using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticeWeb.Model;
public class Group
{
    [Key]
    public int GroupId{get; set;}
    [MaxLength(12)]
    public required string Name {get; set;}
    public required string Status {get; set;}
    public int? UserId{get; set;} = null;
    [ForeignKey("UserId")]
    public User? Owner{get; set;} = null;
    [MinLength(1)]
    public required int NoOfGroups {get;set;}
}