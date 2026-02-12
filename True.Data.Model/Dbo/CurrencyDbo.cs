using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace True.Data.Model.Dbo
{
    [Table("currencies", Schema = "true")]
    [PrimaryKey(nameof(Id))]
    public class CurrencyDbo
    {
        [Required, StringLength(3)] public string Id { get; set; } = null!;
        [Required] public string Name { get; set; } = null!;
        [Required] public decimal Rate { get; set; }
    }
}
