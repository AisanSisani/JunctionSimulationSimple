// **************************************************************************************************
//		CTLightMIC
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
    public class CTLightMIC : HlaInteractionClass
    {
        #region Declarations
        public HlaParameter area;
        public HlaParameter tlight_id;
        public HlaParameter state;
        #endregion //Declarations

        #region Constructor
        public CTLightMIC() : base()
        {
            // Initialize Class Properties
            Name = "HLAinteractionRoot.TLightM";
            ClassPS = PSKind.PublishSubscribe;

            // Create Parameters
            // area
            area = new HlaParameter("area");
            Parameters.Add(area);
            // tlight_id
            tlight_id = new HlaParameter("tlight_id");
            Parameters.Add(tlight_id);
            // state
            state = new HlaParameter("state");
            Parameters.Add(state);
        }
        #endregion //Constructor
    }
}
