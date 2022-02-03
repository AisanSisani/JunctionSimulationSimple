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
        public CCarFdApp federate; //Application-specific federate 
        // Local data structures
        public BindingList<CCarHlaObject> CarObjects; // Keeps the ships in the environment
        public BindingList<CTLightHlaObject> TLightObjects; // Keeps the stations in the environment
        public System.Timers.Timer timer = new System.Timers.Timer(5000); // Timer to report the position periodically, TIMER
        #endregion //Declarations

        #region Constructor
        public CSimulationManager()
        {
            // Initialize the application-specific federate
            federate = new CCarFdApp(this);
            // Initialize the federation execution
            federate.FederationExecution.Name = "JSFederation";
            federate.FederationExecution.FederateType = "CarFd";
            federate.FederationExecution.ConnectionSettings = "rti://127.0.0.1";

            // Time management TIMER
            federate.Lookahead = 1;

            // Handle RTI type variation
            initialize();

            // Populate the local ship list
            CarObjects = new BindingList<CCarHlaObject>();
            TLightObjects = new BindingList<CTLightHlaObject>();
        }
        #endregion //Constructor
    
        #region Methods
        // Handles naming variation according to HLA specification
        private void initialize()
        {
            switch (federate.RTILibrary)
            {
            case RTILibraryType.HLA13_DMSO: case RTILibraryType.HLA13_Portico: case RTILibraryType.HLA13_OpenRti:
                    federate.Som.CarOC.Name = "objectRoot.Car";
                    federate.Som.CarOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    federate.Som.TLightOC.Name = "objectRoot.TLight";
                    federate.Som.TLightOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    //TODO make it relative
                    federate.FederationExecution.FDD = @"C:\Users\aisan\aisan_space\aisan_work\projects\JunctionSimulationSimple\JunctionSimulationVS\JunctionSimulationSimple\CarFdApp\Som\JSFom.fed";
                    break;
            case RTILibraryType.HLA1516e_Portico: case RTILibraryType.HLA1516e_OpenRti:
                    federate.Som.CarOC.Name = "HLAobjectRoot.Car";
                    federate.Som.CarOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    federate.Som.TLightOC.Name = "HLAobjectRoot.TLight";
                    federate.Som.TLightOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    //TODO make it relative
                    federate.FederationExecution.FDD = @"C:\Users\aisan\aisan_space\aisan_work\projects\JunctionSimulationSimple\JunctionSimulationVS\JunctionSimulationSimple\CarFdApp\Som\JSFom.xml";
            break;
            }
        }

        // Update Ship Position TODO: create the timer and all the shit TIMER
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Update all the attributes of the car
            federate.UpdateAll(CarObjects[0]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Timer Elapsed - {CarObjects[0].car.car_id}: {CarObjects[0].car.belong_area}, {CarObjects[0].car.heading_direction}, {CarObjects[0].car.speed}, ({CarObjects[0].car.position.X}, {CarObjects[0].car.position.Y})");

            //// report all ships
            //foreach (var item in ShipObjects)
            //{
            //  Console.WriteLine($"{item.Ship.Callsign}: ({item.Ship.Position.X}, {item.Ship.Position.Y}), {item.Ship.Heading}, {item.Ship.Speed}");
            //}

            // Force a garbage collection to occur.
            GC.Collect();
        }
        #endregion //Methods
    }
}
