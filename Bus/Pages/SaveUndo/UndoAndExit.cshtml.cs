using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages.SaveUndo
{
    public class UndoAndExitModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        public UndoAndExitModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        public IActionResult OnGet()
        {
            bService.st.buildStations(bService.stationsFileName);
            bService.net.buildNetwork(bService.networkFileName);
            return RedirectToPage("/Index");
        }
    }
}
