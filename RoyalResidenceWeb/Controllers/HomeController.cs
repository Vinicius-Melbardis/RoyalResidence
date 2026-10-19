using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalResidence.DTO;
using RoyalResidenceWeb.Models;
using RoyalResidenceWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalResidenceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IResidenceService _residenceService;
        private readonly IMapper _mapper;

        public HomeController(IResidenceService residenceService, IMapper mapper)
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
                TempData["error"] = $"Occurred an error: {ex.Message}";
            }

            return View(residenceList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
