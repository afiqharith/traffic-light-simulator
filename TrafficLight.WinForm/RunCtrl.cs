using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficLightSimulator
{
    internal class RunCtrl
    {
        #region STEP
        internal STEP iAutoNextStep;
        internal STEP iLastSwitchStep;
        #endregion

        #region RUNSTATE
        internal RUN_STATE iRunState;
        internal RUN_STATE iLastSwitchRunState;
        #endregion

        #region Object member
        internal EventManager evt;
        private TrafficLight[] runTrafficLight;
        internal double iSysCounter;
        private TrafficLight currentActiveLane;
        #endregion


        internal RunCtrl(EventManager eventManager)
        {
            evt = eventManager;
            PowerUpRestart();
        }

        #region Operation
        internal void Scheduler(int interval = 300, bool isDelay = false)
        {
            if (isDelay) { Thread.Sleep(interval); }

            switch (iRunState)
            {
                case RUN_STATE.IDLE:
                    if (!EventManager.IsEventActive(EVT.POWER_UP_STARTING))
                    {
                        EventManager.SetEvent(EVT.POWER_UP_STARTING);
                        iRunState = RUN_STATE.INIT;
                    }
                    break;

                case RUN_STATE.INIT:
                    if (!EventManager.IsEventActive(EVT.POWER_UP_COMPLETED))
                    {
                        PowerUpOperation();
                    }
                    break;

                case RUN_STATE.PAUSE:
                    Thread.Sleep(100);
                    break;

                case RUN_STATE.RUNNING:
                default:
                    AutoOperation();
                    break;
            }
        }

        internal void PowerUpRestart()
        {
            iRunState = RUN_STATE.IDLE;
        }

        private void PowerUpOperation()
        {
            if (EventManager.IsEventActive(EVT.POWER_UP_STARTING) && !EventManager.IsEventActive(EVT.POWER_UP_COMPLETED))
            {
                iRunState = RUN_STATE.INIT;
            }

            //runTrafficLight = new TrafficLight[]
            //{
            //    new TrafficLight(OBJ_LOC.AREA_LANE_1, red1, yellow1, green1),
            //    new TrafficLight(OBJ_LOC.AREA_LANE_2, red2, yellow2, green2),
            //    new TrafficLight(OBJ_LOC.AREA_LANE_3, red3, yellow3, green3),
            //    new TrafficLight(OBJ_LOC.AREA_LANE_4, red4, yellow4, green4)
            //};

            //Need to find a method to use evt (non-static)
            runTrafficLight = EventManager.GetWindowInstance().itemTrafficLight;

            switch (iAutoNextStep)
            {
                case STEP.PU_INIT_TRAFFICLIGHT:
                    for (int i = 0; i < runTrafficLight.Length; i++)
                    {
                        runTrafficLight[i].SetCurrentStateMask(0x1);
                    }
                    EventManager.SetEvent(EVT.POWER_UP_COMPLETED);
                    break;

                default:
                    break;
            }

            if (EventManager.IsEventActive(EVT.POWER_UP_COMPLETED))
            {
                iRunState = RUN_STATE.RUNNING;
                iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                EventManager.SetEvent(EVT.POST_POWER_UP_COMPLETED);
            }

        }

        private void AutoOperation()
        {
            switch (iAutoNextStep)
            {
                case STEP.AUTO_DETERMINE_NEXT_STEP:

                    // After power up completed, determine which lane to be activated (make it turn green, normally 1st index)
                    if (EventManager.IsEventActive(EVT.POWER_UP_COMPLETED) && EventManager.IsEventActive(EVT.POST_POWER_UP_COMPLETED))
                    {
                        for (int i = 0; i < runTrafficLight.Length; i++)
                        {
                            if (i == 0)
                            {
                                runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() << 2);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        EventManager.ResetEvent(EVT.POST_POWER_UP_COMPLETED);
                        iAutoNextStep = STEP.AUTO_VERIFY_UPDATE_SYS_TIMER;
                        break;
                    }
                    //Normal operation after post power up done aka normal running
                    else
                    {
                        // Determine current active lane after done update timer
                        if (EventManager.IsEventActive(EVT.UPDATE_TIMER_COMPLETED))
                        {
                            for (int i = 0; i < runTrafficLight.Length; i++)
                            {
                                if (runTrafficLight[i].IsNotInStopState())
                                {
                                    SetRequestLaneStateChangeInProg(runTrafficLight[i]);
                                    break;
                                }
                            }
                            EventManager.ResetEvent(EVT.UPDATE_TIMER_COMPLETED);
                            iAutoNextStep = STEP.AUTO_UPDATE_TIMEBASED_LANE_VAL;
                            break;
                        }
                    }

                    //Nothing to be done here, so, update system timer
                    iAutoNextStep = STEP.AUTO_VERIFY_UPDATE_SYS_TIMER;
                    break;

                case STEP.AUTO_UPDATE_TIMEBASED_LANE_VAL:


                    //for (int i = 0; i < runTrafficLight.Length; i++)
                    //{
                    //    if (IsLaneStateChangeInProg(runTrafficLight[i]))
                    //    {
                    //        // If runtask is green & already 10s, Change it to yellow
                    //        // Otherwise, wait until it reach 10s
                    //        if (runTrafficLight[i].GetCurrentStateMask() > (uint)STATE.SLOW &&
                    //            runTrafficLight[i].GetCurrentCounter() >= 9)
                    //        {
                    //            runTrafficLight[i].ResetCurrentCounter();
                    //            runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 1);
                    //        }

                    //        // If runtask is already yellow & already 3s, reset runtask counter
                    //        // Otherwise, wait until it reach 3s
                    //        if (runTrafficLight[i].GetCurrentStateMask() == (uint)STATE.SLOW &&
                    //            runTrafficLight[i].GetCurrentCounter() >= 2)
                    //        {
                    //            runTrafficLight[i].ResetCurrentCounter();
                    //        }
                    //    }

                    //    // Look for any yellow (still active lane) & update traffic light to red
                    //    if (runTrafficLight[i].IsNotInStopState())
                    //    {
                    //        // If the timer already reset, update the runtask to stopstate (red)
                    //        if (runTrafficLight[i].GetCurrentCounter() < 0)
                    //        {
                    //            // Change the runtask to red
                    //            runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 1);
                    //            // Get out from this loop 
                    //            iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    //            break;
                    //        }

                    //        //if (runTrafficLight[i].GetCurrentCounter() >= 10)
                    //        //{

                    //        //}
                    //    }
                    //    // And maintain other runtask to red
                    //    //else
                    //    //{
                    //    //    runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 0);
                    //    //}
                    //}

                    //Working code =======================
                    if (iSysCounter == 10.0)
                    {
                        iAutoNextStep = STEP.AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW;
                        break;
                    }
                    if (iSysCounter == 14.0)
                    {
                        iAutoNextStep = STEP.AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED;
                        break;
                    }
                    if (iSysCounter == 16.0)
                    {
                        iAutoNextStep = STEP.AUTO_ACTIVATE_NEXT_LANE_INDEX;
                        break;
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    //Working code =======================
                    break;

                case STEP.AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW:
                    for (int i = 0; i < runTrafficLight.Length; i++)
                    {
                        // Look for curent active lane & update traffic light to yellow
                        // & if other brotherhood lane is on red, ignore it
                        if (IsLaneStateChangeInProg(runTrafficLight[i]))
                        {
                            runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 1);
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;


                case STEP.AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED:
                    for (int i = 0; i < runTrafficLight.Length; i++)
                    {
                        // Look for curent active lane (yellow) & update traffic light to red
                        if (runTrafficLight[i].IsNotInStopState())
                        {
                            runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 1);
                        }
                        // Maintain brotherhood traffic light to red
                        else
                        {
                            runTrafficLight[i].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() >> 0);
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                case STEP.AUTO_ACTIVATE_NEXT_LANE_INDEX:
                    for (int i = 0; i < runTrafficLight.Length; i++)
                    {
                        // Look for any previously was an active lane
                        if (runTrafficLight[i].GetPrevState() != STATE.STOP)
                        {
                            // Look for next (right) brotherhood lane index
                            // If brotherhood reach maximum index, reset the next active lane to first lane (index 0)
                            int nextLane = ((i + 1) < runTrafficLight.Length) ? (i + 1) : 0;

                            // Update the new active lane to active mode (green) & start moving
                            runTrafficLight[nextLane].SetCurrentStateMask(runTrafficLight[i].GetCurrentStateMask() << 2);
                        }
                        else
                        {
                            // Ignore other brotherhood lane that was unactive & red
                            continue;
                        }
                    }
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                case STEP.AUTO_VERIFY_UPDATE_SYS_TIMER:
                    iSysCounter = iSysCounter == 16.0 ? 1 : (iSysCounter += 1);
                    for (int i = 0; i < runTrafficLight.Length; i++)
                    {
                        runTrafficLight[i].SetIncreaseCounter();
                    }
                    EventManager.SetEvent(EVT.UPDATE_TIMER_COMPLETED);
                    iAutoNextStep = STEP.AUTO_DETERMINE_NEXT_STEP;
                    break;

                default:
                    //AutoOperation();
                    break;
            }
        }
        #endregion

        #region Misc
        internal void CtrlPause()
        {
            iRunState = RUN_STATE.PAUSE;
        }

        internal void CtrlRunning()
        {
            iRunState = RUN_STATE.RUNNING;
        }

        private void SetRequestLaneStateChangeInProg(TrafficLight lane)
        {
            currentActiveLane = lane;
        }

        private bool IsLaneStateChangeInProg(TrafficLight lane)
        {
            return currentActiveLane == lane;
        }
        #endregion
    }
}
