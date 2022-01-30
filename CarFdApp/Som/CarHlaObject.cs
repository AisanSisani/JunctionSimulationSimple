// **************************************************************************************************
//		CCarHlaObject
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
  public class CCarHlaObject : HlaObject
  {
        #region Declarations
        // Declare your local object structure here
        public CCar car;
        #endregion //Declarations

        #region Constructor
        public CCarHlaObject(HlaObjectClass _type) : base(_type)
        {
            car = new CCar();
        }
        // Copy constructor - used in callbacks
        public CCarHlaObject(HlaObject _obj) : base(_obj)
        {
            car = new CCar();
        }
        #endregion //Constructor
  }
}
