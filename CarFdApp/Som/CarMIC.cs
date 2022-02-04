// **************************************************************************************************
//		CCarMIC
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.3
//			at		: 	Friday, February 4, 2022 6:57:22 AM
//		compatible with		: 	RACoN v.0.0.2.5
//
//		copyright		: 	(C) 
//		email			: 	
// **************************************************************************************************
/// <summary>
/// This class is extended from the object model of RACoN API
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
  public class CCarMIC : HlaInteractionClass
  {
    #region Declarations
    public HlaParameter position;
    public HlaParameter area;
    public HlaParameter car_id;
    #endregion //Declarations
    
    #region Constructor
    public CCarMIC() : base()
    {
      // Initialize Class Properties
      Name = "HLAinteractionRoot.CarM";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Parameters
      // position
      position = new HlaParameter("position");
      Parameters.Add(position);
      // area
      area = new HlaParameter("area");
      Parameters.Add(area);
      // car_id
      car_id = new HlaParameter("car_id");
      Parameters.Add(car_id);
    }
    #endregion //Constructor
  }
}
