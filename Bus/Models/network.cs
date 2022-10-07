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
            //number of lines passing through a station
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

        public List<int> LineThrStation(int stnID)
        {
            //The function returns the first line, the second line and so on passing through a station controlled by the order value.
            Node<stationData>? temp;
            List<int> linesThr = new List<int>();
            for (int i = 0; i < nbLines; i++)
            {
                temp = lines[i].stationsList.front;
                while (temp != null)
                {
                    if (stnID == temp.data.stnId)
                    {
                        linesThr.insertBack(lines[i].lineNb);
                        break;
                    }
                    temp = temp.next;
                }
            }
            return linesThr;
        }
        public bool directLine(int stnID1, int stnID2, int lineID)
        {
            //The function returns whether or not there is a direct line between two stations using a given line number/id
            bool flag1 = false;
            bool flag2 = false;
            Node<stationData>? temp = lines[lineIndex(lineID)].stationsList.front;
            while (temp != null)
            {
                if (stnID1 == temp.data.stnId)
                {
                    flag1 = true;
                }
                else if (stnID2 == temp.data.stnId)
                {
                    flag2 = true;
                }
                temp = temp.next;
            }
            return flag1 && flag2;
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
        public float distance(int stnID1, int stnID2, int lineID)
        {
            //distance between two stations in terms of actual distance
            float d1, d2, distance;
            d1 = d2 = 0; //initialized because of compiler warning
            Node<stationData>? temp = lines[lineIndex(lineID)].stationsList.front;
            while (temp != null)
            {
                if (stnID1 == temp.data.stnId)
                {
                    d1 = temp.data.distance;
                }
                else if (stnID2 == temp.data.stnId)
                {
                    d2 = temp.data.distance;
                }
                temp = temp.next;
            }
            if (d1 > d2)
                distance = d1 - d2;
            else
                distance = d2 - d1;
            return distance;
        }
        /*public List shortestPath()
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
            for (int i = lineIndex(lineID); i < nbLines;)
            {
                lines[i] = lines[++i];
            }
            nbLines--;
            changed = 1;
            return true;
        }
    }
}