using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightSimulator
{
    public class CEvent
    {
        private string m_Name;
        private uint m_Id;
        private EVT m_Evt;

        private bool m_bIsActive;

        public CEvent(EVT evt, uint id, string name)
        {
            m_Evt = evt;
            m_Id = id;
            m_Name = name;

            m_bIsActive = false;
        }

        public void Set() { m_bIsActive = true; }
        public void Reset() { m_bIsActive = false; }
        public bool IsActive() { return m_bIsActive; }
        public uint GetId() { return m_Id; }
        public string GetName() { return m_Name; }
        public EVT GetEvt() { return m_Evt; }
    }
}
