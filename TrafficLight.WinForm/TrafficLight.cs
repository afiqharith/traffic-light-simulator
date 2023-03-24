using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightSimulator
{
    internal enum STEP
    {
        PU_INIT_TRAFFICLIGHT,

        AUTO_UPDATE_FIRST_LANE_INDEX_TO_ACTIVE,
        AUTO_STORE_CURRENT_ACTIVE_LANE_OBJECT,
        AUTO_UPDATE_BROTHERHOOD_TRAFFICLIGHT,

        AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW,
        AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED,
        AUTO_ACTIVATE_NEXT_LANE_INDEX,


        AUTO_DETERMINE_NEXT_STEP,
        AUTO_VERIFY_CURRENT_TIMER,
        AUTO_STOP_CURRENT_TIMER,
        AUTO_RECOVERY_AFTER_STOP_TIMER,
    }

    public enum LANE_ID
    {
        LANE1,
        LANE2,
        LANE3,
        LANE4,
        NUMS_OF_LANE
    }

    public class TrafficLight
    {
        private enum COLOR
        {
            RED,
            YELLOW,
            GREEN,
            NUMS_OF_COLOR,
        }

        private PictureBox[] lightColor = new PictureBox[(int)COLOR.NUMS_OF_COLOR];

        public int counter = 0;
        public bool bCurrentState;
        public bool bLastState;
        public LANE_ID id;

        private uint _currentStateMask;
        public uint currentStateMask
        {
            get
            {
                return _currentStateMask;
            }

            set
            {
                _currentStateMask = value;
                SetCurrentState(value);
            }
        }

        public TrafficLight(LANE_ID id, PictureBox red, PictureBox yellow, PictureBox green)
        {
            this.lightColor[(int)COLOR.RED] = red;
            this.lightColor[(int)COLOR.YELLOW] = yellow;
            this.lightColor[(int)COLOR.GREEN] = green;

            this.id = id;
        }

        private void SetCurrentState(uint state)
        {
            switch (state)
            {
                case 0b_0100:
                    bCurrentState = true;
                    lightColor[(int)COLOR.RED].Visible = false;
                    lightColor[(int)COLOR.YELLOW].Visible = false;
                    lightColor[(int)COLOR.GREEN].Visible = true;
                    break;

                case 0b_0010:
                    bCurrentState = true;
                    lightColor[(int)COLOR.RED].Visible = false;
                    lightColor[(int)COLOR.GREEN].Visible = false;
                    lightColor[(int)COLOR.YELLOW].Visible = true;
                    break;

                case 0b_0001:
                    bLastState = bCurrentState;
                    bCurrentState = false;
                    lightColor[(int)COLOR.YELLOW].Visible = false;
                    lightColor[(int)COLOR.GREEN].Visible = false;
                    lightColor[(int)COLOR.RED].Visible = true;
                    break;

                default:
                    bCurrentState = false;
                    lightColor[(int)COLOR.RED].Visible = false;
                    lightColor[(int)COLOR.YELLOW].Visible = false;
                    lightColor[(int)COLOR.GREEN].Visible = false;
                    break;
            }
        }

    }
}
