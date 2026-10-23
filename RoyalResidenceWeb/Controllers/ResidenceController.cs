using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoyalResidence.DTO;
using RoyalResidenceWeb.Models;
using RoyalResidenceWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalResidenceWeb.Controllers
{
    public class ResidenceController : Controller
    {
        private readonly IResidenceService _residenceService;
        private readonly IMapper _mapper;

        public ResidenceController(IResidenceService residenceService, IMapper mapper)
        {
            _mapper = mapper;
            _residenceService = residenceService;
        }

        public async Task<IActionResult> Index()
        {
            List<ResidenceDTO> residenceList = new();
            try
            {
                var response = await _residenceService.GetAllAsync<ApiResponse<List<ResidenceDTO>>>("");
                if(response != null && response.Success && response.Data != null)
                {
                    residenceList = response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return View(residenceList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResidenceCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(createDTO);
            }

            try
            {
                var response = await _residenceService.CreateAsync<ApiResponse<ResidenceDTO>>(createDTO, "");
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Residence created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return View(createDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid residence ID";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var response = await _residenceService.GetAsync<ApiResponse<ResidenceDTO>>(id, "");
                if (response != null && response.Success && response.Data != null)
                {
                    return View(_mapper.Map<ResidenceUpdateDTO>(response.Data));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Occurred an error: {ex.Message}";
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ResidenceUpdateDTO updateDTO)
        {
            try
            {
                var response = await _residenceService.UpdateAsync<ApiResponse<object>>(updateDTO, "");
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Residence updated successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Occurred an error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid residence ID";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var response = await _residenceService.GetAsync<ApiResponse<ResidenceDTO>>(id, "");
                if (response != null && response.Success && response.Data != null)
                {
                    return View(response.Data);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Occurred an error: {ex.Message}";
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ResidenceDTO residenceDTO)
        {
            try
            {
                var response = await _residenceService.DeleteAsync<ApiResponse<object>>(residenceDTO.Id, "");
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Residence deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Occurred an error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
