using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class VillaNumberDeleteViewModel
    {
        public VillaNumberDTO villaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }  
        public VillaNumberDeleteViewModel()
        {
            villaNumber = new VillaNumberDTO();
        }

    }
}
