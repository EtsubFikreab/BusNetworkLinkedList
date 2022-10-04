using Bus.Models;
using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages.Edit
{
    public class changeLineModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Choice { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? LineID { get; set; }//previous id
        [BindProperty]
        public Dictionary<string, string>[] Choices { get; set; }
        [BindProperty]
        public string? NewLineID { get; set; }
        [BindProperty]
        public string? NewStnID { get; set; }
        [BindProperty]
        public Dictionary<string, string>[] StaionChoices { get; set; }
        public changeLineModel(ILogger<IndexModel> logger, BusService busService)
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
            bService.net.lines[bService.net.lineIndex(int.Parse(LineID))].lineNb = int.Parse(NewLineID);
            return RedirectToPage("/editNetwork");
        }
        void initDict()
        {
            int index = bService.net.lineIndex(int.Parse(LineID));
            int numStations = bService.net.lines[index].nbStations;
            Choices = new Dictionary<string, string> [numStations];
            for (int i = 0; i < numStations; i++)
            {
                Choices[i] = new()
                {
                    { "Choice", Choice },
                    { "StnID", bService.net.stdata[index][i].stnId.ToString() },
                    {"LineID", LineID}
                };
            }
        }
        void initStcho()
        {
            //initialize station choices
            StaionChoices = new Dictionary<string, string>[bService.st.nbStations];
            for (int i = 0; i < bService.st.nbStations; i++)
            {
                StaionChoices[i] = new()
                {
                    {"Choice",Choice },
                    {"stnID", bService.st.stationList[i].stnID.ToString() },
                    {"LineID", LineID}
                };
            }
        }
        public void OnGet()
        {
            bService.net.storeLines();
            initDict();
            initStcho();
        }
    }
}
