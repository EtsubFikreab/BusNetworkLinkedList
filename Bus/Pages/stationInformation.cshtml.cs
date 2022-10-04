using Bus.Models;
using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages
{
    public class AllNetworkModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        public AllNetworkModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        public void OnGet()
        {
        }
    }
}
