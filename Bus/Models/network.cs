using System;
using System.Text.Json;
namespace Bus.Models
{
    public class network
    {
        public int nbLines { get; set; } //no of lines.
        public Line[]? lines { get; set; } //dynamic array to hold lines in network
        public stationData[][]? stdata { get; set; } //used for saving data
        int avail;
        public int changed; //flag to control save status of network
        public network()
        {
            nbLines = 0;
            avail = 10;
            lines = new Line[avail];
            changed = 0;
        }
        public int lineIndex(int LineID)
        {
            for (int i = 0; i < nbLines; i++)
            {
                if (lines[i].lineNb == LineID)
                    return i;
            }

            return -1;//if not found
        }
        //~network(); not required because of c# garbage collector
        public int buildNetwork(string networkFileName)
        {
            if (!File.Exists(networkFileName))
                return 0;
            network? temp = JsonSerializer.Deserialize<network>(File.ReadAllText(networkFileName));
            if (temp == null)
                return 0;
            for (int i = 0; i < temp.nbLines; i++)
            {
                //temp.lines[i].stationsList = new List<stationData>();
                for (int j = 0; j < temp.lines[i].nbStations && temp != null; j++)
                {
                    temp.addStation(temp.stdata[i][j], i);
                }
            }
            lines = temp.lines;
            nbLines = temp.nbLines;
            avail = 0;
            changed = 0;
            return 1;
        }
        public void updateNetFile(string networkFileName)
        {
            stdata = new stationData[nbLines][];
            for (int i = 0; i < nbLines; i++)
            {
                stdata[i] = new stationData[lines[i].nbStations];
                Node<stationData> temp = lines[i].stationsList.front;
                for (int j = 0; j < lines[i].nbStations && temp != null; j++)
                {
                    stdata[i][j] = temp.data;
                    temp = temp.next;
                }
            }
            File.WriteAllText(networkFileName, JsonSerializer.Serialize(this));
            changed = 0;
        }
        public int noLinesPerStation(int stnID)
        {
            int counter = 0;
            Node<stationData>? temp;
            for (int i = 0; i < nbLines; i++)
            {
                temp = lines[i].stationsList.front;
                while (temp != null)
                {
                    if (stnID == temp.data.stnId)
                    {
                        counter++;
                        break;
                    }
                    temp = temp.next;
                }
            }
            return counter;
        }

        public int LineThrStation()
        {
            //The function returns the first line, the second line and so on passing through a station controlled by the order value.
            //The function returns -1 if there is no line passing through the station.
            int order = 0;
            int stnID = 0;
            int counter = 0;
            Node<stationData>? temp;
            Console.WriteLine("Enter the station ID: ");
            stnID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the order: ");
            order = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < nbLines; i++)
            {
                temp = lines[i].stationsList.front;
                while (temp != null)
                {
                    if (stnID == temp.data.stnId)
                    {
                        counter++;
                        if (counter == order)
                            return lines[i].lineNb;
                        break;
                    }
                    temp = temp.next;
                }
            }
            return -1;
        }
        public int directLine(int stnID)
        {
            //The function returns whether or not there is a direct line between two stations using a given line number/id
            int lineID = 0;
            int counter = 0;
            Node<stationData>? temp;
            Console.WriteLine("Enter the line ID: ");
            lineID = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < nbLines; i++)
            {
                if (lines[i].lineNb == lineID)
                {
                    temp = lines[i].stationsList.front;
                    while (temp != null)
                    {
                        if (stnID == temp.data.stnId)
                        {
                            counter++;
                            if (counter == 2)
                                return 1;
                        }
                        temp = temp.next;
                    }
                }
            }
            return -1;
        }
        public List<stationData>[] displayLinesForward()
        {
            //displays all lines in forward direction
            List<stationData>[] forward = new List<stationData>[nbLines];
            for (int i = 0; i < nbLines; i++)
            {
                forward[i] = lines[i].traverseForward();
            }
            return forward;
        }
        public List<stationData>[] displayLinesBackward()
        {
            List<stationData>[] backward = new List<stationData>[nbLines];
            for (int i = 0; i < nbLines; i++)
            {
                backward[i] = lines[i].traverseBackward();
            }
            return backward;
        }
        /*public float distance()
        {

        }
        public List shortestPath()
        {

        }*/
        public void addLine(Line l)
        {
            if (avail == 0)
            {
                avail = 10;
                Line[] temp = new Line[avail + nbLines];
                if (temp != null)
                {
                    for (int i = 0; i < nbLines; i++)
                        temp[i] = lines[i];
                    lines = temp;
                }
                else return;
            }
            lines[nbLines++] = l;
            avail--;
            changed = 1;
        }
        public void addStation(stationData stdata, int index, int pos = 3, int prevID = -1)
        {
            //pos 3 - last
            if (index < nbLines)
                lines[index].addStation(pos, stdata, prevID);
            changed = 1;
        }
        public void storeLines()
        {
            stdata = new stationData[nbLines][];
            for (int i = 0; i < nbLines; i++)
            {
                stdata[i] = new stationData[lines[i].nbStations];
                Node<stationData> temp = lines[i].stationsList.front;
                for (int j = 0; j < lines[i].nbStations && temp != null; j++)
                {
                    stdata[i][j] = temp.data;
                    temp = temp.next;
                }
            }
        }
        public bool deleteLine(int lineID)
        {
            for (int i = lineIndex(lineID); i < nbLines; )
            { 
                lines[i] = lines[++i];
            }
            nbLines--;
            changed = 1;
            return true;
        }
    }
}