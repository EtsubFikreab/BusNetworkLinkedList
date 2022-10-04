using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages.SaveUndo
{
    public class SaveModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        public SaveModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        public IActionResult OnGet()
        {
            bService.st.updateStnFile(bService.stationsFileName);
            bService.net.updateNetFile(bService.networkFileName);
            return RedirectToPage("/adminDashboard");
        }
    }
}
