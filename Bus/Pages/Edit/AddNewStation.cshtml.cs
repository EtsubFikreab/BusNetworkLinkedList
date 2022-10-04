using Bus.Models;
using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Bus.Pages
{
    public class AddNewStationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty]
        public string? stationName { get; set; }
        [BindProperty]
        public string? stationId { get; set; }
        public AddNewStationModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            bService.st.addStation(int.Parse(stationId), stationName);
            return RedirectToPage("/editNetwork");
        }
        public void OnGet()
        {
        }
    }
}
