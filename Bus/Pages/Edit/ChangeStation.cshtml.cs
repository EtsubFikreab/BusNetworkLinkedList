using Bus.Models;
using Bus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bus.Pages.Edit
{
    public class ChangeStationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public BusService bService { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Choice { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? StnID { get; set; }//previous id
        [BindProperty(SupportsGet = true)]
        public string? LineID { get; set; }
        [BindProperty]
        public string? StnName { get; set; }
        [BindProperty]
        public string NewStnID { get; set; }
        [BindProperty]
        public string Distance { get; set; }
        public ChangeStationModel(ILogger<IndexModel> logger, BusService busService)
        {
            _logger = logger;
            bService = busService;
        }
        public bool delStn()
        {
            bService.st.deleteStation(int.Parse(StnID), bService.st.mapStation(int.Parse(StnID)));
            return true;
        }
        public bool delStnFromLine()
        {
            int index = bService.net.lineIndex(int.Parse(LineID));
            Node<stationData>? temp = bService.net.lines[index].stationsList.front;
            while(temp!= null)
            {
                if (temp.data.stnId == int.Parse(StnID))
                    break;
                temp = temp.next;
            }
            if (temp != null)
            {
                bService.net.lines[index].stationsList.deleteNode(temp);
                bService.net.lines[index].nbStations--;
                return true;
            }
            return false;
        }
        public void insertStationOnLine()
        {
            int index = bService.net.lineIndex(int.Parse(LineID));
            stationData st = new()
            {
                stnId = int.Parse(StnID),
                distance = int.Parse(Distance)
            };
            bService.net.lines[index].stationsList.insertBack(st);
            bService.net.lines[index].nbStations++;
            bService.net.changed = 1;
        }
        public IActionResult OnPost()
        {
            if(Choice=="1")
                bService.st.updateStation(int.Parse(StnID), int.Parse(NewStnID), StnName);
            if(Choice=="3")
                insertStationOnLine();
            return RedirectToPage("/editNetwork");
        }
        public void OnGet()
        {
            bService.net.storeLines();
        }
    }
}
