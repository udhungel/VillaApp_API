using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberservice, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberservice;
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateViewModel VillaNumberVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSucess)
            {
                VillaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x=> new SelectListItem 
                                                                                       { Text = x.Name                                                                                        
                                                                                        ,Value= x.Id.ToString()
                                                                                       });
            }
            return View(VillaNumberVM);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.villaNumber);
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = "Villa Number created Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessage.Count>0)
                    {                        
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());

                    }
                }
            }
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSucess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name
                                                                                        ,
                    Value = x.Id.ToString()
                });
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateViewModel villaNumberVM = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>(villaNo);
            if (response != null && response.IsSucess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.villaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);

            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSucess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name
                                                                                        ,
                    Value = x.Id.ToString()
                });

                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.villaNumber);
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = "Villa Number updated Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessage.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());

                    }
                }
            }
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSucess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name
                                                                                        ,
                    Value = x.Id.ToString()
                });
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {

            VillaNumberDeleteViewModel villaNumberVM = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>(villaNo);
            if (response != null && response.IsSucess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.villaNumber =model;

            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSucess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)).Select(x => new SelectListItem
                {
                    Text = x.Name
                                                                                        ,
                    Value = x.Id.ToString()
                });

                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteViewModel model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.villaNumber.VillaNo);
            if (response != null && response.IsSucess)
            {
                TempData["success"] = "Villa Number deleted Successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }


    }
}
