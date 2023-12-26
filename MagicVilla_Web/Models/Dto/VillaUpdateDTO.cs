using System.ComponentModel.DataAnnotations;

namespace VillaApp_WebAPI.Models.Dto
{
    //things you want to expose 
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupany { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
