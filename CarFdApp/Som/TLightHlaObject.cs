// **************************************************************************************************
//		CTLightHlaObject
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.3
//			at		: 	Wednesday, January 19, 2022 6:50:09 PM
//		compatible with		: 	RACoN v.0.0.2.5
//
//		copyright		: 	(C) 
//		email			: 	
// **************************************************************************************************
/// <summary>
/// This is a wrapper class for local data structures. This class is extended from the object model of RACoN API
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using JSSimge.Som;


namespace JSSimge.Som
{
  public class CTLightHlaObject : HlaObject
  {
    #region Declarations
    // Declare your local object structure here
        public string tlight_id;
        public TLState state;
        public Int64 duration_red;
        public Int64 duration_green;
        public Area belong_area;
    #endregion //Declarations
    
    #region Constructor
    public CTLightHlaObject(HlaObjectClass _type) : base(_type)
    {
            // Instantiate local data here
            tlight_id = "north_down";
            state = TLState.red;
            duration_red = 0;
            duration_green = 0;
            belong_area = Area.north_down;
    }
    // Copy constructor - used in callbacks
    public CTLightHlaObject(HlaObject _obj) : base(_obj)
    {
            // Instantiate local data here
            tlight_id = "north_down";
            state = TLState.red;
            duration_red = 0;
            duration_green = 0;
            belong_area = Area.north_down;
    }
    #endregion //Constructor
  }
}
