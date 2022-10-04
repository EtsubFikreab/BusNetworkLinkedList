using System;
using System.IO;
using System.Text.Json;
namespace Bus.Models
{
    public class station
    {
        //identification number of the station
        public int stnID { get; set; }
        //Name of station
        public string? name { get; set; }
    }
    public class stations
    {
        public int nbStations { get; set; } //no of stations in the network.
        public station[]? stationList { get; set; } //dynamic array to hold the stations in the network
        int avail;  //no of slot available to add new slot
        public int changed;//0 - not changed  1 - changed
        public stations()
        {
            nbStations = 0;
            avail = 10;
            stationList = new station[avail];
            changed = 0;
        }
        // ~ stations(); destructor not required because C# has garbage collector
        public int buildStations(string stnFileName)
        {
            if (!File.Exists(stnFileName))
                return 0;
            string jsonString = File.ReadAllText(stnFileName);
            stations? temp = JsonSerializer.Deserialize<stations>(jsonString);
            if (temp == null)
                return 0;
            nbStations = temp.nbStations;
            stationList = temp.stationList;
            avail = 0;
            changed = 0;
            return 1;
        }
        public void addStation(int id, string name)
        {
            if (avail == 0)
            {
                //resize list
                station[]? temp = new station[nbStations + 10];
                if (temp != null)
                {
                    avail = 10;
                    for (int i = 0; i < nbStations; i++)
                        temp[i] = stationList[i];
                    stationList = temp;
                }
                else return;

            }
            station data = new station();
            data.stnID = id;
            data.name = name;
            stationList[nbStations++] = data;
            avail--;
            changed = 1;

        }
        public void updateStnFile(string stnFileName)
        {
            if (changed == 0)
                return;
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(stnFileName, jsonString);
            changed = 0;
        }
        public void deleteStation(int id = -1, string name = "")
        {
            //id is -1 if only name is inserted
            int i = 0;
            if (id != -1)
            {
                for (; i < nbStations && stationList[i].stnID != id; i++)
                    ;
            }
            else
            {
                for (; i < nbStations && stationList[i].name != name; i++)
                    ;
            }
            while (i < nbStations - 1)
            {
                stationList[i].stnID = stationList[i + 1].stnID;
                stationList[i].name = stationList[++i].name;
            }
            nbStations--;
            avail++;
            int changed = 1;

        }
        public string displayStations()
        {
            return JsonSerializer.Serialize(stationList);
        }
        public string mapStation(int stNo)
        {
            for (int i = 0; i < nbStations; i++)
                if (stationList[i].stnID == stNo)
                    return stationList[i].name;
            return "[Not Found]";
        }
        public int mapStation(string stName)
        {
            for (int i = 0; i < nbStations; i++)
                if (stationList[i].name.Equals(stName, StringComparison.CurrentCultureIgnoreCase))
                    return stationList[i].stnID;
            return -1;
        }
        public void updateStation(int prevStnNo, int stnNo, string name)
        {
            for (int i = 0; i < nbStations; i++)
                if (stationList[i].stnID == prevStnNo)
                {
                    stationList[i].stnID = stnNo;
                    stationList[i].name = name;
                    changed = 1;
                }
        }
    }
}