// **************************************************************************************************
//		FederateSom
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
  public class FederateSom : Racon.ObjectModel.CObjectModel
  {
        #region Declarations
        #region SOM Declaration
        public JSSimge.Som.CTLightOC TLightOC;
        //public JSSimge.Som.CTLightMIC TLightMIC;
        #endregion
        #endregion //Declarations

        #region Constructor
        public FederateSom() : base()
        {
              // Construct SOM
              TLightOC = new JSSimge.Som.CTLightOC();
              AddToObjectModel(TLightOC);
              //TLightMIC = new JSSimge.Som.CTLightMIC();
              //AddToObjectModel(TLightMIC);
        }
        #endregion //Constructor
  }
}
