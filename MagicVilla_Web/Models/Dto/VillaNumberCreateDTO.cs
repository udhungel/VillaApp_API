﻿using System.ComponentModel.DataAnnotations;

namespace VillaApp_WebAPI.Models.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaID  {get; set;}
       

        public string SpecialDetails { get; set; }
    }
}
