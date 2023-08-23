using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightSimulator
{
    internal enum COLOR
    {
        RED,
        YELLOW,
        GREEN,
        NUMS_OF_COLOR,
    }

    internal enum STATE
    {
        STOP = 0b_0001,
        SLOW = 0b_0010,
        MOVE = 0b_0100,

        NUMS_OF_STATE,
    }

    internal class LightTowerBase : RunTask
    {
        protected Dictionary<COLOR, PictureBox> m_lightColor;

        private uint m_currentStateMask { get; set; }
        private STATE m_lastState { get; set; }
        protected int m_internalCounter { get; set; }


        public LightTowerBase(OBJ_TYPE type, OBJ_LOC loc, PictureBox redLight, PictureBox yellowLight, PictureBox greenLight) : base(type, loc)
        {
            m_internalCounter = -1;
            m_lightColor = new Dictionary<COLOR, PictureBox>()
            {
                { COLOR.RED, redLight },
                { COLOR.YELLOW , yellowLight },
                { COLOR.GREEN, greenLight }
            };
        }

        #region State mask manipulation
        internal void SetCurrentStateMask(uint mask)
        {
            SetPrevState((STATE)m_currentStateMask);
            m_currentStateMask = mask;
            EnforeceStateValueOnUi(mask);
        }

        public uint GetCurrentStateMask() { return m_currentStateMask; }
        #endregion

        #region State manipulation
        private void SetPrevState(STATE lastState) { m_lastState = lastState; }

        internal STATE GetPrevState() { return m_lastState; }
        #endregion

        #region Internal counter manipulation
        internal void SetIncreaseCounter(int inc = 1) { m_internalCounter += inc; }

        internal int GetCurrentCounter() { return m_internalCounter; }

        internal void ResetCurrentCounter(int restVal = -1) { m_internalCounter = restVal; }
        #endregion

        #region UI update
        internal virtual void EnforeceStateValueOnUi(uint state)
        {
            //switch ((STATE)state)
            //{
            //    case STATE.MOVE:
            //        foreach (var lightObj in m_lightColor)
            //        {
            //            m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.GREEN) ? true : false;
            //        }
            //        break;

            //    case STATE.SLOW:
            //        foreach (var lightObj in m_lightColor)
            //        {
            //            m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.YELLOW) ? true : false;
            //        }
            //        break;

            //    case STATE.STOP:
            //    default:
            //        foreach (var lightObj in m_lightColor)
            //        {
            //            m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.RED) ? true : false;
            //        }
            //        break;
            //}
        }
        #endregion
    }
}
