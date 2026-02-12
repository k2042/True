using System.ComponentModel.DataAnnotations;

namespace True.Contracts.Entities
{
    public partial class Currency
    {
        [Required, StringLength(3)] public string Id { get; set; } = null!;
        [Required] public string Name { get; set; } = null!;
        [Required] public decimal Rate { get; set; }
    }
}
