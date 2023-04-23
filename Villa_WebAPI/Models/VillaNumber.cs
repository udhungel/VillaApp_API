using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApp_WebAPI.Models
{
    public class VillaNumber
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }

        public string SpecialDetails { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [ForeignKey("Villa")] // here Villa is the name of the associated Navigation Property 
        public int VillaID { get; set; }

        public Villa Villa { get; set; }

    }
}
