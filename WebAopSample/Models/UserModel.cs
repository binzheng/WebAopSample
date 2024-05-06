
using Dapper.Contrib.Extensions;

namespace WebAopSample.Models;


[Table("MUser")]
public class UserModel
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }   
}
