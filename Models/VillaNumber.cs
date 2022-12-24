using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApp_WebAPI.Models
{
    public class VillaNumber
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }

        public string SpecialDetails { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

    }
}
