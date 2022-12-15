using System.ComponentModel.DataAnnotations;

namespace VillaApp_WebAPI.Models.Dto
{
    //things you want to expose 
    public class VillaDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }
        public int Occupany { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
