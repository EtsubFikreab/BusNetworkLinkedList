using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages
{
    public class LineInputModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty]
        public string? LineNumber { get; set; }
        public LineInputModel(ILogger<IndexModel> logger, BusService busService)
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
            bService.net.addLine(new Models.Line() { lineNb=int.Parse(LineNumber)});
            return RedirectToPage("./editNetwork");
        }
        public void OnGet()
        {
        }
    }
}
