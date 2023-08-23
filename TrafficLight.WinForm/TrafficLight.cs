using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightSimulator
{
    internal class TrafficLight: LightTowerBase
    {
        internal TrafficLight(OBJ_LOC loc, PictureBox red, PictureBox yellow, PictureBox green) :
            base(OBJ_TYPE.TRAFFIC_LIGHT, loc, red, yellow, green) { }

        internal bool IsNotInStopState()
        {
            return (STATE)GetCurrentStateMask() > STATE.STOP;
        }

        #region UI update
        internal override void EnforeceStateValueOnUi(uint state)
        {
            switch ((STATE)state)
            {
                case STATE.MOVE:
                    foreach (var lightObj in m_lightColor)
                    {
                        m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.GREEN) ? true : false;
                    }
                    break;

                case STATE.SLOW:
                    foreach (var lightObj in m_lightColor)
                    {
                        m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.YELLOW) ? true : false;
                    }
                    break;

                case STATE.STOP:
                default:
                    foreach (var lightObj in m_lightColor)
                    {
                        m_lightColor[lightObj.Key].Visible = (lightObj.Key == COLOR.RED) ? true : false;
                    }
                    break;
            }
        }
        #endregion
    }
}
