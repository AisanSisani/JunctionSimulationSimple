// **************************************************************************************************
//		CCarOC
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
  public class CCarOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute car_id;
    public HlaAttribute belong_area;
    public HlaAttribute heading_direction;
    public HlaAttribute speed;
    public HlaAttribute position;
    #endregion //Declarations
    
    #region Constructor
    public CCarOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.Car";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Attributes
      // car_id
      car_id = new HlaAttribute("car_id", PSKind.PublishSubscribe);
      Attributes.Add(car_id);
      // belong_area
      belong_area = new HlaAttribute("belong_area", PSKind.PublishSubscribe);
      Attributes.Add(belong_area);
      // heading_direction
      heading_direction = new HlaAttribute("heading_direction", PSKind.PublishSubscribe);
      Attributes.Add(heading_direction);
      // speed
      speed = new HlaAttribute("speed", PSKind.PublishSubscribe);
      Attributes.Add(speed);
      // position
      position = new HlaAttribute("position", PSKind.PublishSubscribe);
      Attributes.Add(position);
    }
    #endregion //Constructor
  }
}
