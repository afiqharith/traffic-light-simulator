using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace TrafficLightSimulator
{
    public partial class MainWindow : Form
    {
        private int _interval;

        private double _counter;
        private double m_wndCounter
        {
            get
            {
                return _counter;
            }

            set
            {
                _counter = value;

                Action safeThread = delegate
                {
                    labelMainTimer.Text = String.Format("{0:f1}s", value);
                    //timer1.Text = String.Format("{0:f1}", value);
                    //timer2.Text = String.Format("{0:f1}", value);
                    //timer3.Text = String.Format("{0:f1}", value);
                    //timer4.Text = String.Format("{0:f1}", value);
                };

                if (labelMainTimer.InvokeRequired)
                {
                    labelMainTimer.Invoke(safeThread);
                }
                else
                {
                    labelMainTimer.Text = String.Format("{0:f1}s", value);
                }


                //timer1.Text = String.Format("{0:f1}", value);
                //timer2.Text = String.Format("{0:f1}", value);
                //timer3.Text = String.Format("{0:f1}", value);
                //timer4.Text = String.Format("{0:f1}", value);
            }
        }

        //Last AutoStep before TL_STATE change to pause
        //private STEP iLastSwitchStep;

        //private STEP iAutoNextStep;

        //private RUN_STATE iLastSwitchRunState;

        //private RUN_STATE iRunState;

        internal TrafficLight[] itemTrafficLight;

        //private TrafficLight currentActiveLane;

        //Event manager
        EventManager evt;

        RunCtrl m_runCtrl;
        public MainWindow()
        {
            InitializeComponent();

            itemTrafficLight = new TrafficLight[]
            {
                new TrafficLight(OBJ_LOC.AREA_LANE_1, red1, yellow1, green1),
                new TrafficLight(OBJ_LOC.AREA_LANE_2, red2, yellow2, green2),
                new TrafficLight(OBJ_LOC.AREA_LANE_3, red3, yellow3, green3),
                new TrafficLight(OBJ_LOC.AREA_LANE_4, red4, yellow4, green4)
            };

            //Init event listener
            evt = new EventManager(this);
            m_runCtrl = new RunCtrl(evt);

            textboxInterval.Text = "500";
            System.Windows.Forms.Timer systemTimer = new System.Windows.Forms.Timer();
            systemTimer.Enabled = true;
            systemTimer.Interval = 1;
            systemTimer.Tick += new EventHandler(SystemTimer_Tick);
            systemTimer.Start();
        }

        private void SystemTimer_Tick(object sender, EventArgs e)
        {
            ChangeAutoStep(m_runCtrl.iAutoNextStep);
            ChangeRunStep(m_runCtrl.iRunState);
            m_runCtrl.Scheduler(_interval, true);
            if (m_runCtrl.iRunState != RUN_STATE.PAUSE)
            {
                m_wndCounter = m_runCtrl.iSysCounter;
            }
        }
        private void buttonStopStart_Click(object sender, EventArgs e)
        {

            if (m_runCtrl.iRunState == RUN_STATE.RUNNING)
            {
                m_runCtrl.CtrlPause();
                buttonStopStart.Text = "Start";
                labelMainTimer.Text = "STOP";
                labelMainTimer.ForeColor = Color.Red;
            }
            else if (m_runCtrl.iRunState == RUN_STATE.PAUSE)
            {
                m_runCtrl.CtrlRunning();
                buttonStopStart.Text = "Stop";
                labelMainTimer.Text = "+++";
                labelMainTimer.ForeColor = Color.Green;
            }
        }
        private void textboxInterval_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textboxInterval.Text))
            {
                int.TryParse(textboxInterval.Text, out _interval);
            }
        }
        private void ChangeAutoStep(STEP nextStep)
        {
            if (nextStep != m_runCtrl.iLastSwitchStep)
            {
                m_runCtrl.iLastSwitchStep = nextStep;
                AppendRTBText(String.Format("State change = ({0}) {1}", (int)nextStep, nextStep), Color.Black);
            }
        }
        private void ChangeRunStep(RUN_STATE nextStep)
        {
            if (nextStep != m_runCtrl.iLastSwitchRunState)
            {
                m_runCtrl.iLastSwitchRunState = nextStep;
                AppendRTBText(String.Format("Runstep change = ({0}) {1}", (int)nextStep, nextStep), Color.DimGray);
            }
        }
        internal void AppendRTBText(string text, Color color)
        {
            if (richTextBox1.InvokeRequired)
            {
                Action safeThread = delegate { AppendRTBText(text, color); };
                richTextBox1.Invoke(safeThread);
            }
            else
            {
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;

                richTextBox1.SelectionColor = color;
                richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss,fff") + String.Format(" {0}", text) + Environment.NewLine);
                richTextBox1.SelectionColor = richTextBox1.ForeColor;
                richTextBox1.ScrollToCaret();
            }
        }


    }





}
