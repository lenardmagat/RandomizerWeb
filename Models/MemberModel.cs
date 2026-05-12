using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticeWeb.Model;
public class Member
{
    [Key]
    public int MemberId {get; set;}
    public required string Name {get; set;}
    public int? UserId = null;
    [ForeignKey("UserId")]
    public User? Owner{get; set;} = null;
}