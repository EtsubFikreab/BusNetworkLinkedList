using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages
{
    public class selectStationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Choice { get; set; }
        [BindProperty]
        public Dictionary<string, string>[] Choices { get; set; }
        public selectStationModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
            initChoices();
        }
        void initChoices()
        {
            Choices = new Dictionary<string, string>[bService.st.nbStations];
            for(int i=0; i < bService.st.nbStations; i++)
            {
                Choices[i] = new()
                {
                    {"Choice",Choice },
                    {"stnID", bService.st.stationList[i].stnID.ToString() }
                };
            }
        }
        public void OnGet()
        {
            initChoices();
        }
    }
}
