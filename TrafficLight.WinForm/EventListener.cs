using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightSimulator
{
    public enum EVT
    {
        FIRST_LANE_INDEX_ACTIVATED,
        ONE_CYCLE_COMPLETED,
        WAIT_STOP_TIMER,
        NUMS_OF_EVENT,
    }

    public class EventListener
    {
        public static List<bool> EventList = new List<bool>();

        public EventListener()
        {
            //Intialize event power up event

            for(int i = 0; i < (int)EVT.NUMS_OF_EVENT; i++)
            {
                EventList.Insert(i, false);
            }

        }

        public static void SetEvent(EVT clientEvent)
        {
            EventList[(int)clientEvent] = true;
        }

        public static void ResetEvent(EVT clientEvent)
        {
            EventList[(int)clientEvent] = false;
        }

        public static bool IsEventActive(EVT clientEvent)
        {
            return EventList[(int)clientEvent];
        }
    }

}
