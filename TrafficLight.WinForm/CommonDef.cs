using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public enum EVT
    {
        FIRST_LANE_INDEX_ACTIVATED,
        ONE_CYCLE_COMPLETED,
        WAIT_STOP_TIMER,
        NUMS_OF_EVENT,
    }

    public enum LANE_ID
    {
        LANE1,
        LANE2,
        LANE3,
        LANE4,
        NUMS_OF_LANE
    }
}
