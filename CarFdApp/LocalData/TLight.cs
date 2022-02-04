using System;
using System.Collections.Generic;
using System.Text;

// Racon
using Racon;
using Racon.RtiLayer;
// App
using JSSimge.Som;

namespace JSSimge
{
    public class CTLight
    {
        #region Declarations
        // Declare your local object structure here
        public string tlight_id;
        public TLState state;
        public Int64 duration_red;
        public Int64 duration_green;
        public Area belong_area;
        #endregion //Declarations

        public CTLight()
        {
            tlight_id = "north_down";
            state = TLState.green;
            duration_red = 0;
            duration_green = 0;
            belong_area = Area.north_down;
        }

    }
    
}
