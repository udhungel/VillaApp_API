﻿using System.ComponentModel.DataAnnotations;

namespace VillaApp_WebAPI.Models.Dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }

        public string SpecialDetails { get; set; }
       
    }
}
