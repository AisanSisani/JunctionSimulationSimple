// **************************************************************************************************
//		CSimulationManager
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
/// The Simulation Manager manages the (multiple) federation execution(s) and the (multiple instances of) joined federate(s).
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
using System.ComponentModel;
using System.Timers;
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using JSSimge.Som;

namespace JSSimge
{
  public class CSimulationManager
  {
    #region Declarations
    // Communication layer related structures
    public CTLightFdApp federate; //Application-specific federate 
    // Local data structures
    // user-defined data structures are declared here
    public CTLightHlaObject TLightObject;
    #endregion //Declarations

    #region Constructor
    public CSimulationManager()
    {

            // Initialize the application-specific federate
            federate = new CTLightFdApp(this);
            // Initialize the federation execution
            federate.FederationExecution.Name = "JSFederation";
            federate.FederationExecution.FederateType = "TLightFd";
            federate.FederationExecution.ConnectionSettings = "rti://127.0.0.1";

            // Time management TIMER
            federate.Lookahead = 1;

            // Handle RTI type variation
            initialize();
    }
    #endregion //Constructor
    
    #region Methods
    // Handles naming variation according to HLA specification
    private void initialize()
    {
      switch (federate.RTILibrary)
      {
        case RTILibraryType.HLA13_DMSO: case RTILibraryType.HLA13_Portico: case RTILibraryType.HLA13_OpenRti:
                    Console.WriteLine("Used RTILibraryType.HLA13_Portico or RTILibraryType.HLA13_OpenRti");
                    federate.Som.TLightOC.Name = "objectRoot.TLight";
                    federate.Som.TLightOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    //federate.Som.TLightMIC.Name = "interactionRoot.TLightM";
                    //TODO make it relative
                    federate.FederationExecution.FDD = @"C:\Users\aisan\aisan_space\aisan_work\projects\JunctionSimulationSimple\JunctionSimulationVS\JunctionSimulationSimple\TLightFdApp\Som\JSFom.fed";
                break;
        case RTILibraryType.HLA1516e_Portico: case RTILibraryType.HLA1516e_OpenRti:
                    Console.WriteLine("Used RTILibraryType.HLA1516e_OpenRti");
                    federate.Som.TLightOC.Name = "HLAobjectRoot.TLight";
                    federate.Som.TLightOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    //federate.Som.TLightMIC.Name = "HLAinteractionRoot.TLightM";
                    //TODO make it relative
                    federate.FederationExecution.FDD = @"C:\Users\aisan\aisan_space\aisan_work\projects\JunctionSimulationSimple\JunctionSimulationVS\JunctionSimulationSimple\TLightFdApp\Som\JSFom.xml";
                break;
      }
    }

    // report
    private void Report(string txt, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(txt);
    }

    #endregion //Methods
  }
}
