using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace TrafficLightSimulator
{
    internal class Car
    {
        private Label _carSkeleton;
        private int _initXPos;
        private int _initYPos;

        private int _PrevXPos;
        private int _PrevYPos;

        private int _CurrentXPos;
        private int _CurrentYPos;


        public Car(double speed, Color color, int xPos, int yPos)
        {
            _carSkeleton = new Label();
            _carSkeleton.BackColor = Color.Transparent;
            _carSkeleton.ForeColor = color;
            _carSkeleton.Text = "[==]";
        }

        public Label GetInstance()
        {
            return _carSkeleton;
        }
    }
}
