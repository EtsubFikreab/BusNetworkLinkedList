using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages
{
    public class LineSelectModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [BindProperty(SupportsGet = true)]
        public string Choice { get; set; }
        public BusService bService { get; set; }
        public Dictionary<string, string>[] Choices { get; set; }
        public LineSelectModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        void initChoices()
        {
            bService.net.storeLines();
            Choices = new Dictionary<string, string>[bService.net.nbLines];
            for(int i=0; i< bService.net.nbLines; i++)
            {
                Choices[i] = new()
                {
                    {"Choice", Choice },
                    {"LineID", bService.net.lines[i].lineNb.ToString()}
                };
            }
        }
        public void OnGet()
        {
            initChoices();
        }
    }
}
