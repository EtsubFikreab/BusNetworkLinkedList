using Bus.Models;
using System;
using System.Text.Json;

namespace Bus.Services
{
    public class BusService
    {
        public BusService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            st=new stations();
            st.buildStations(stationsFileName);

            net = new network();
            net.buildNetwork(networkFileName);
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public string networkFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "network.json"); }
        }
        public string stationsFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "stations.json"); }
        }
        public network net;
        public stations st;
    }
}
