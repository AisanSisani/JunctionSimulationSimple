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
using System.Threading;
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

        // (TODO if you want add the synchronization handlers in this region)
        #region Federation Management Callbacks
        // FdAmb_OnSynchronizationPointRegistrationConfirmedHandler -> does nothing
        public override void FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(sender, data);

            #region User Code
            Report("Synchronization Confirmed", ConsoleColor.Blue);
            #endregion //User Code
        }
        // FdAmb_OnSynchronizationPointRegistrationFailedHandler -> does nothing
        public override void FdAmb_OnSynchronizationPointRegistrationFailedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationFailedHandler(sender, data);

            #region User Code
            Report("Synchronization Failed", ConsoleColor.Red);
            #endregion //User Code
        }
        // FdAmb_SynchronizationPointAnnounced
        public override void FdAmb_SynchronizationPointAnnounced(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_SynchronizationPointAnnounced(sender, data);

            #region User Code
            // do nothing
            manager.federate.SynchronizationPointAchieved(data.Label, true);
            #endregion //User Code
        }
        // FdAmb_FederationSynchronized
        public override void FdAmb_FederationSynchronized(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_FederationSynchronized(sender, data);

            #region User Code
            Report($"Synchronization ({data.Label}) is completed.", ConsoleColor.Blue);
            #endregion //User Code
        }
        #endregion

        #region Declaration Management Callbacks
        // registering the car you have created
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            Report("FdAmb_StartRegistrationForObjectClassAdvisedHandler", ConsoleColor.Blue);

            // Check that this is for the CarOC
            if (data.ObjectClassHandle == Som.TLightOC.Handle)
                RegisterObject(manager.TLightObject);

            // the timer starts here TIMER
            //Report("Timer start", ConsoleColor.Blue);
            //manager.timer.Start(); // move this to turn on attribute update callback
            #endregion //User Code
        }

        private void RegisterObject(HlaObject obj)
        {
            Report("RegisterObject", ConsoleColor.Blue);
            if (!RegisterHlaObject(obj)) Report("Light could not be registered", ConsoleColor.Red);

            //TODO read what are these
            //// DDM - register object with regions
            //// Create a list of attribute set and region set pairs
            //AttributeHandleSetRegionHandleSetPairVector pairs = new AttributeHandleSetRegionHandleSetPairVector();
            //// Construct the region set
            //List<HlaRegion> regions = new List<HlaRegion>();
            //regions.Add(aor1);
            //// Populate the pairs. Here we use all the attributes of the object
            //pairs.Pairs.Add(new KeyValuePair<List<HlaAttribute>, List<HlaRegion>>(obj.Attributes, regions));
            //// register object attributes with related regions
            //RegisterHlaObject(obj, pairs);
            //associateRegionsForUpdates(obj, pairs);
        }

        #region Object Management Callbacks
        
        // other federates asks you to update you values
        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

            #region User Code
            Report("FdAmb_AttributeValueUpdateRequestedHandler", ConsoleColor.Blue);

            // !!! If this federate is created only one object instance, then it is sufficient to check the handle of that object, otherwise we need to check all the collection
            if (data.ObjectInstance.Handle == manager.TLightObject.Handle)
            {
                UpdateAll(manager.TLightObject);
            }
            #endregion //User Code
        }
        #endregion // Object Management Callbacks

        //update the car position based on the timer, it is called in the simulation manager
        public void UpdateState(CTLightHlaObject tlight)
        {
            // Add Values
            Report("UpdateState", ConsoleColor.Blue);

            tlight.AddAttributeValue<TLState>(Som.TLightOC.state, tlight.tlight.state);
            if (!UpdateAttributeValues(tlight, "")) Report("Updates State not successfull", ConsoleColor.Red);
        }

        // Update attribute values
        public void UpdateAll(CTLightHlaObject tlight)
        {
            Report("UpdateAll", ConsoleColor.Blue);

            // Add Values
            tlight.AddAttributeValue(Som.TLightOC.tlight_id, tlight.tlight.tlight_id);
            tlight.AddAttributeValue(Som.TLightOC.belong_area, (uint)tlight.tlight.belong_area);
            tlight.AddAttributeValue(Som.TLightOC.duration_red, (Int64)tlight.tlight.duration_red);
            tlight.AddAttributeValue(Som.TLightOC.duration_green, (Int64)tlight.tlight.duration_green);
            tlight.AddAttributeValue(Som.TLightOC.state, (uint)tlight.tlight.state);
            if(!UpdateAttributeValues(tlight, "")) Report("Updates All not successfull", ConsoleColor.Red);
        }

        /*
        // Send TLightMIC.Message
        public bool SendMessage(string tlight_id, Area area, TLState state)
        {
            Report($"send message {tlight_id}, {area}, {state}", ConsoleColor.Blue);

            Racon.RtiLayer.HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.TLightMIC);


            // Add Values
            interaction.AddParameterValue(Som.TLightMIC.tlight_id, tlight_id); // String
            interaction.AddParameterValue(Som.TLightMIC.area, (uint)area);
            interaction.AddParameterValue(Som.TLightMIC.state, (uint)state);

            // Send interaction
            return (SendInteraction(interaction, ""));
        }
        */

        // report
        private void Report(string txt)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(txt);
        }

        // report
        private void Report(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
        }

        #endregion //Manually Added Code


    }
}
