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
        private double mainSystemCounter
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

        private STEP iLastSwitchStep;

        private STEP iAutoNextStep;


        private TrafficLight[] lane = new TrafficLight[(int)LANE_ID.NUMS_OF_LANE];

        private TrafficLight currentActiveLane;
        public MainWindow()
        {
            InitializeComponent();

            textboxInterval.Text = "1000";

            System.Windows.Forms.Timer systemTimer = new System.Windows.Forms.Timer();
            systemTimer.Enabled = true;
            systemTimer.Interval = 1;
            systemTimer.Tick += new EventHandler(SystemTimer_Tick);
            systemTimer.Start();
        }



        private Thread T;
        private void SystemTimer_Tick(object sender, EventArgs e)
        {
            ChangeStep(iAutoNextStep);
            if (T == null)
            {
                PowerUp();
            }
            Run();

            //mainSystemCounter = mainSystemCounter == 16.0 ? 1 : (mainSystemCounter += 1);
            //Thread.Sleep(_interval);

        }

        private void ChangeStep(STEP nextStep)
        {
            if (nextStep != iLastSwitchStep)
            {
                iLastSwitchStep = nextStep;
                AppendRTBText(String.Format("State change = ({0}) {1}", (int)nextStep, nextStep), Color.Black);
            }
        }


        private void PowerUp()
        {
            //Init event listener
            EventListener evt = new EventListener();

            //Thread timerThread = new Thread(new ThreadStart(() =>
            //{
            //    while (true)
            //    {
            //        Thread.Sleep(1000);
            //        if (EventListener.IsEventActive(EVT.WAIT_STOP_TIMER))
            //        {
            //            Thread.Sleep(100);
            //            //break;
            //        }
            //        else
            //        {
            //            mainSystemCounter++;
            //        }
            //    }

            //}));
            //timerThread.Start();

            // Create a traffic light object lane mapping
            T = new Thread(new ThreadStart(() =>
            {
                lane = new TrafficLight[]
                {
                new TrafficLight(LANE_ID.LANE1, red1, yellow1, green1),
                new TrafficLight(LANE_ID.LANE2, red2, yellow2, green2),
                new TrafficLight(LANE_ID.LANE3, red3, yellow3, green3),
                new TrafficLight(LANE_ID.LANE4, red4, yellow4, green4)
                };
            }));
            T.Start();
            Thread.Sleep(10);
        }


        private void Run()
        {
            switch (iAutoNextStep)
            {
                case STEP.PU_INIT_TRAFFICLIGHT:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        lane[i].currentStateMask = 0x1;
                    }
                    iAutoNextStep = STEP.AUTO_UPDATE_FIRST_LANE_INDEX_TO_ACTIVE;
                    break;

                case STEP.AUTO_UPDATE_FIRST_LANE_INDEX_TO_ACTIVE:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        if (i == 0)
                        {
                            lane[i].currentStateMask = lane[i].currentStateMask << 2;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    iAutoNextStep = STEP.AUTO_VERIFY_CURRENT_TIMER;
                    break;

                case STEP.AUTO_STORE_CURRENT_ACTIVE_LANE_OBJECT:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        // Determine current active lane
                        if (lane[i].bCurrentState)
                        {
                            currentActiveLane = lane[i];
                        }
                    }
                    iAutoNextStep = STEP.AUTO_UPDATE_BROTHERHOOD_TRAFFICLIGHT;
                    break;

                case STEP.AUTO_UPDATE_BROTHERHOOD_TRAFFICLIGHT:
                    if (mainSystemCounter == 10.0)
                    {
                        iAutoNextStep = STEP.AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW;
                        break;
                    }
                    else if (mainSystemCounter == 14.0)
                    {
                        iAutoNextStep = STEP.AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED;
                        break;
                    }
                    else if (mainSystemCounter == 16.0)
                    {
                        iAutoNextStep = STEP.AUTO_ACTIVATE_NEXT_LANE_INDEX;
                        break;
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                case STEP.AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        // Look for curent active lane & update traffic light to yellow
                        // & if other brotherhood lane is on red, ignore it
                        if (currentActiveLane == lane[i])
                        {
                            lane[i].currentStateMask = lane[i].currentStateMask >> 1;
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;


                case STEP.AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        // Look for curent active lane (yellow) & update traffic light to red
                        if (lane[i].bCurrentState)
                        {
                            lane[i].currentStateMask = lane[i].currentStateMask >> 1;
                        }
                        // Maintain brotherhood traffic light to red
                        else
                        {
                            lane[i].currentStateMask = lane[i].currentStateMask >> 0;
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                case STEP.AUTO_ACTIVATE_NEXT_LANE_INDEX:
                    for (int i = 0; i < lane.Length; i++)
                    {
                        // Look for any previously was an active lane
                        if (lane[i].bLastState)
                        {
                            // Look for next (right) brotherhood lane index
                            // If brotherhood reach maximum index, reset the next active lane to first lane (index 0)
                            int nextLane = ((i + 1) < lane.Length) ? (i + 1) : 0;

                            // Update the new active lane to active mode (green) & start moving
                            lane[nextLane].currentStateMask = lane[i].currentStateMask << 2;
                        }
                        else
                        {
                            // Ignore other brotherhood lane that was unactive & red
                            continue;
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                case STEP.AUTO_DETERMINE_NEXT_STEP:
                    if (EventListener.IsEventActive(EVT.WAIT_STOP_TIMER))
                    {
                        iAutoNextStep = STEP.AUTO_STOP_CURRENT_TIMER;
                        break;
                    }
                    else
                    {
                        iAutoNextStep = STEP.AUTO_VERIFY_CURRENT_TIMER;
                        break;
                    }

                case STEP.AUTO_STOP_CURRENT_TIMER:
                    if (EventListener.IsEventActive(EVT.WAIT_STOP_TIMER))
                    {
                        Thread.Sleep(100);
                        break;
                    }
                    iAutoNextStep = STEP.AUTO_RECOVERY_AFTER_STOP_TIMER;
                    break;

                case STEP.AUTO_RECOVERY_AFTER_STOP_TIMER:
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                default:
                case STEP.AUTO_VERIFY_CURRENT_TIMER:
                    mainSystemCounter = mainSystemCounter == 16.0 ? 1 : (mainSystemCounter += 1);
                    //if(mainSystemCounter == 16.0)
                    //{
                    //    mainSystemCounter = 0;
                    //}
                    iAutoNextStep = STEP.AUTO_STORE_CURRENT_ACTIVE_LANE_OBJECT;
                    Thread.Sleep(_interval);
                    break;
            }
        }

        private void textboxInterval_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textboxInterval.Text))
            {
                int.TryParse(textboxInterval.Text, out _interval);
            }
        }

        private void AppendRTBText(string text, Color color)
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

        private void buttonStopStart_Click(object sender, EventArgs e)
        {
            if (buttonStopStart.Text == "Stop")
            {
                EventListener.SetEvent(EVT.WAIT_STOP_TIMER);
                AppendRTBText(String.Format("Event set = ({0}) {1}", (int)EVT.WAIT_STOP_TIMER, EVT.WAIT_STOP_TIMER), Color.Red);
                buttonStopStart.Text = "Start";
                labelMainTimer.Text = "STOP";
                labelMainTimer.ForeColor = Color.Red;
                iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
            }
            else
            {
                EventListener.ResetEvent(EVT.WAIT_STOP_TIMER);
                AppendRTBText(String.Format("Event reset = ({0}) {1}", (int)EVT.WAIT_STOP_TIMER, EVT.WAIT_STOP_TIMER), Color.Green);
                buttonStopStart.Text = "Stop";
                labelMainTimer.ForeColor = Color.Green;
                iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
            }

        }

        private void DetermineNextIterMask(ref uint mask)
        {
            switch (mask)
            {
                case 0x4:
                case 0x2:
                    mask = mask >> 1;
                    break;

                default:
                case 0x1:
                    mask = mask << 2;
                    break;
            }
        }


    }





}
