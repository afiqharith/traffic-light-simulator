using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightSimulator
{

    internal enum RUN_STATE
    {
        IDLE,
        INIT,
        RUNNING,
        PAUSE,
        JAM,

        NUM_OF_TL_STATE,
    }


    internal enum STEP
    {
        PU_INIT_TRAFFICLIGHT,

        AUTO_DETERMINE_NEXT_STEP,
        AUTO_UPDATE_TIMEBASED_LANE_VAL,

        AUTO_UPDATE_ACTIVE_LANE_TO_YELLOW,
        AUTO_UPDATE_ACTIVE_LANE_TO_BUFFER_RED,
        AUTO_ACTIVATE_NEXT_LANE_INDEX,

        AUTO_VERIFY_UPDATE_SYS_TIMER,
    }

    public enum EVT
    {
        POWER_UP_STARTING,
        POWER_UP_COMPLETED,
        POST_POWER_UP_COMPLETED,

        FIRST_LANE_INDEX_ACTIVATED,
        ONE_CYCLE_COMPLETED,

        REQUIRED_TO_UPDATE_TIMER,
        UPDATE_TIMER_COMPLETED,

        WAIT_STOP_TIMER,
        STOP_TIMER_RESUME,
        NUMS_OF_EVENT,
    }

    //public enum STATE
    //{
    //    MOVE,
    //    SLOW,
    //    STOP,

    //    NUMS_OF_STATE,
    //}

    public enum LANE_ID
    {
        LANE1,
        LANE2,
        LANE3,
        LANE4,
        NUMS_OF_LANE
    }


    public enum OBJ_TYPE
    {
        TRAFFIC_LIGHT,
        LANE = TRAFFIC_LIGHT,
        CAR,
        NUMS_OF_OBJ,
    }

    public enum OBJ_LOC
    {
        AREA_LANE_1,
        AREA_LANE_2,
        AREA_LANE_3,
        AREA_LANE_4,

        NUMS_OF_LOC,
    }
}
