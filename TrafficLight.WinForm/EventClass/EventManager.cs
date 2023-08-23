#define useDict
#define NewFormat

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightSimulator
{

#if NewFormat
    public class EventManager
    {
        public static Dictionary<EVT, CEvent> EVENT = new Dictionary<EVT, CEvent>();
        private static MainWindow m_mainWindow;

        public EventManager(MainWindow mainWindow)
        {
            InitializeEvt();
            m_mainWindow = mainWindow;  
        }

        private bool InitializeEvt()
        {
            foreach (EVT evt in Enum.GetValues(typeof(EVT)))
            {
                EVENT.Add(evt, new CEvent(evt, (uint)evt, Enum.GetName(typeof(EVT), evt)));
            }
            return true;
        }

        public static void SetEvent(EVT clientEvent)
        {
            EVENT[clientEvent].Set();
            m_mainWindow.AppendRTBText(String.Format("Set event = ({0}) {1}", (int)clientEvent, clientEvent), Color.Green);
        }

        public static void ResetEvent(EVT clientEvent)
        {
            EVENT[clientEvent].Reset();
            m_mainWindow.AppendRTBText(String.Format("Reset event = ({0}) {1}", (int)clientEvent, clientEvent), Color.Red);
        }

        public static bool IsEventActive(EVT clientEvent)
        {
            return EVENT[clientEvent].IsActive();
        }

        #region Main window method
        public static MainWindow GetWindowInstance() { return m_mainWindow; }
        #endregion
    }
#else


    public class EventManager
    {

#if useDict
        public static Dictionary<EVT, bool> EventList = new Dictionary<EVT, bool>();
#else
        public static List<bool> EventList = new List<bool>();
#endif

        private Form m_MainWindow;

        public EventManager()
        {
            //Intialize event power up event
#if useDict
            foreach (EVT evt in Enum.GetValues(typeof(EVT)))
            {
                EventList.Add(evt, false);
            }
#else
            for (int i = 0; i < (int)EVT.NUMS_OF_EVENT; i++)
            {
                EventList.Insert(i, false);
            }
#endif
        }

        public EventManager(Form mainWindow)
        {
            m_MainWindow = mainWindow;
            //Intialize event power up event
#if useDict
            foreach (EVT evt in Enum.GetValues(typeof(EVT)))
            {
                EventList.Add(evt, false);
            }
#else
            for (int i = 0; i < (int)EVT.NUMS_OF_EVENT; i++)
            {
                EventList.Insert(i, false);
            }
#endif
        }

        public static void SetEvent(EVT clientEvent)
        {
#if useDict
            EventList[clientEvent] = true;
#else

            EventList[(int)clientEvent] = true;
#endif

            //if(m_MainWindow != null)
            //{

            //}

        }

        public static void ResetEvent(EVT clientEvent)
        {
#if useDict
            EventList[clientEvent] = false;
#else

            EventList[(int)clientEvent] = false;
#endif
        }

        public static bool IsEventActive(EVT clientEvent)
        {
#if useDict
            return EventList[clientEvent];
#else
            return EventList[(int)clientEvent];
#endif
        }

#if useDict
        public static void CreateEvent()
        {

        }
#endif
    }
#endif
}
