// **************************************************************************************************
//		CTLightOC
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
  public class CTLightOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute tlight_id;
    public HlaAttribute state;
    public HlaAttribute duration_red;
    public HlaAttribute duration_green;
    public HlaAttribute belong_area;
    #endregion //Declarations
    
    #region Constructor
    public CTLightOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.TLight";
      ClassPS = PSKind.Subscribe;
      
      // Create Attributes
      // tlight_id
      tlight_id = new HlaAttribute("tlight_id", PSKind.PublishSubscribe);
      Attributes.Add(tlight_id);
      // state
      state = new HlaAttribute("state", PSKind.Subscribe);
      Attributes.Add(state);
      // duration_red
      duration_red = new HlaAttribute("duration_red", PSKind.Subscribe);
      Attributes.Add(duration_red);
      // duration_green
      duration_green = new HlaAttribute("duration_green", PSKind.Subscribe);
      Attributes.Add(duration_green);
      // belong_area
      belong_area = new HlaAttribute("belong_area", PSKind.Subscribe);
      Attributes.Add(belong_area);
    }
    #endregion //Constructor
  }
}
