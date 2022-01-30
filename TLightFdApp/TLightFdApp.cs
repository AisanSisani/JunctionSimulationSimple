// **************************************************************************************************
//		CTLightFdApp
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
/// The application specific federate that is extended from the Generic Federate Class of RACoN API. This file is intended for manual code operations.
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using JSSimge.Som;

namespace JSSimge
{
  public partial class CTLightFdApp : Racon.CGenericFederate
  {
    #region Manually Added Code
    
    // Local Data
    private CSimulationManager manager;
    
    #region Constructor
    public CTLightFdApp(CSimulationManager parent) : this()
    {
      manager = parent; // Set simulation manager
      // Create regions manually
    }
    #endregion //Constructor

    // Cut and paste the callbacks that you want to modify from the Generated file (ShipFdApp.simge.cs)



    #endregion //Manually Added Code


    }
}
