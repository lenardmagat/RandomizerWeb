using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticeWeb.Model;
public class User
{
    [Key]
    public int UserId {get; set;}
    [MaxLength(12)]
    public required string Name{get; set;}
    public required string HashedPassword{get;set;}
    public string status {get;set;} = null!;
    public List<Group> Groups{get; set;} = new();
    public List<Member> Members{get; set;} = new();
} 