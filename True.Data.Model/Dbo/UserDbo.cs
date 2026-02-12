using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace True.Data.Model.Dbo
{
    [Table("users", Schema = "true")]
    [PrimaryKey(nameof(Id))]
    public class UserDbo
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
