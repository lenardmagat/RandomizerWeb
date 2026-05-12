using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PracticeWeb.Model;
public class GroupMember
{
    [Key]
    public int GroupMemberId{get; set;}
    public required string Name{get; set;}
    public int GroupNumber{get; set;}
    public int GroupId{get; set;}
    [ForeignKey("GroupId")]
    public Group Owner{get;set;} = null!;

}