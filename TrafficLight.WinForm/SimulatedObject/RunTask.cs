using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightSimulator
{
    internal class RunTask
    {
        private OBJ_TYPE m_objectType;
        private OBJ_LOC m_objectLoc;

        public RunTask(OBJ_TYPE type, OBJ_LOC loc)
        {
            SetType(type);
            SetLoc(loc);
        }

        public virtual void SetType(OBJ_TYPE type) { m_objectType = type; }

        public virtual OBJ_TYPE GetRunType() { return m_objectType; }

        public virtual void SetLoc(OBJ_LOC loc) { m_objectLoc = loc; }

        public virtual OBJ_LOC GetLoc() { return m_objectLoc; }
    }
}
