using System;
using System.Text.Json;

namespace Bus.Models
{
    public class stationData
    {
        public int stnId { get; set; } // station number
        public float distance { get; set; }   // distance from the beginning
    }

    public class Line
    {
        public int lineNb { get; set; }       //Identification number for a line.
        public int nbStations { get; set; }   //Number of stations on a line
        public List<stationData>? stationsList; //Line of stations
        public Line()
        {
            lineNb = 0;
            nbStations = 0;
            stationsList = new List<stationData>();
        }
        //~Line(); not necessary since C# has a garbage collector
        public List<stationData>? getSubLine(stationData start, stationData end)
        {
            if (start == null)
                return null;
            List<stationData> subLine = new List<stationData>();

            //find the beginning
            Node<stationData>? temp = stationsList.find(start);
            while (temp != null)
            {
                subLine.insertBack(temp.data);
                temp = temp.next;
            }
            return subLine;
        }
        public void addStation(int pos, stationData stdata, int id = -1)
        {
            /*
                pos - position of insertion
                0 - before
                1 - after
                2 - first
                3 - last
            */

            //data for finding the node that is before or after 

            Node<stationData> prevStdata = stationsList.front;
            if (pos < 2 && id != -1)
            {
                while (prevStdata != null)
                {
                    if (prevStdata.data.stnId == id)
                        break;
                    prevStdata = prevStdata.next;
                }
            }
            //insertion
            if (pos == 0)
                stationsList.insertBefore(prevStdata, stdata);
            else if (pos == 1)
                stationsList.insertAfter(prevStdata, stdata);
            else if (pos == 2)
                stationsList.insertFirst(stdata);
            else
                stationsList.insertBack(stdata);
        }
        public void deleteStation(int pos, int id = -1)
        {
            /*
                pos - position of deletion
                0 - before
                1 - after
                2 - first
                3 - last
            */
            //data for finding the node that is before or after
            Node<stationData> prevStdata = stationsList.front;
            if (pos < 2 && id > 0)
            {
                while (prevStdata != null)
                {
                    if (prevStdata.data.stnId == id)
                        break;
                    prevStdata = prevStdata.next;
                }
                if (prevStdata == null)
                    return;
            }
            if (pos == 0)
                stationsList.removeBefore(prevStdata);
            else if (pos == 1)
                stationsList.removeAfter(prevStdata);
            else if (pos == 2)
                stationsList.removeFirst();
            else
                stationsList.removeLast();
        }
        public List<stationData> traverseForward()
        {
            //display handled in the front end
            return stationsList;
        }
        public List<stationData> traverseBackward()
        {
            List<stationData> reverse = new List<stationData>();
            Node<stationData> temp = stationsList.front;
            while (temp != null)
            {
                reverse.insertFirst(temp.data);
                temp = temp.next;
            }
            return reverse;
        }

    }
}