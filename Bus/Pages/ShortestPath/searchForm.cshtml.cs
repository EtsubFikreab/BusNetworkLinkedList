using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages.ShortestPath
{
    public class searchFormModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty]
        public string? stationName1 { get; set; }
        [BindProperty]
        public string? stationName2 { get; set; }
        public searchFormModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        /*public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //return RedirectToPage("/");
        }*/
        public void OnGet()
        {
        }
    }
}
