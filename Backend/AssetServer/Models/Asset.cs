using System.ComponentModel.DataAnnotations;

namespace AssetServerAPI.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ImageFileName { get; set; }
        public string? FbxFileName { get; set; }
    }
}