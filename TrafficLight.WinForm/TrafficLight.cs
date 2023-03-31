using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightSimulator
{
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
                default:
                    bLastState = bCurrentState;
                    bCurrentState = false;
                    lightColor[(int)COLOR.YELLOW].Visible = false;
                    lightColor[(int)COLOR.GREEN].Visible = false;
                    lightColor[(int)COLOR.RED].Visible = true;
                    break;
            }
        }

    }
}
