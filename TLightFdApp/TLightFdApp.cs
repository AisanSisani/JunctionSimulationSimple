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

        // (TODO if you want add the synchronization handlers in this region)
        #region Federation Management Callbacks
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
            manager.timer.Start(); // move this to turn on attribute update callback
            #endregion //User Code
        }

        private void RegisterObject(HlaObject obj)
        {
            RegisterHlaObject(obj);

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

        // Stop Registration
        public override void FdAmb_StopRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StopRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            Report("FdAmb_StopRegistrationForObjectClassAdvisedHandler", ConsoleColor.Blue);

            manager.timer.Stop(); // move this to turn off attribute update callback
            #endregion //User Code
        }
        #endregion // Declaration Management Callbacks



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
                // We can further try to figure out the attributes for which update is requested.
                //foreach (var item in data.ObjectInstance.Attributes)
                //{
                //  if (item.Handle == Som.ShipOC.Callsign.Handle) UpdateName(manager.Ships[0]);
                //  else if (item.Handle == Som.ShipOC.Heading.Handle) UpdateHeading(manager.Ships[0]);
                //  else if (item.Handle == Som.ShipOC.Position.Handle) UpdatePosition(manager.Ships[0]);
                //  else if (item.Handle == Som.ShipOC.Speed.Handle) UpdateSpeed(manager.Ships[0]);
                //}

                // We can update all attributes if we dont want to check every attribute.
                UpdateAll(manager.TLightObject);
                //UpdateName(manager.Ships[0]);
                //UpdatePosition(manager.Ships[0]);
                //UpdateHeading(manager.Ships[0]);
                //UpdateSpeed(manager.Ships[0]);
            }
            #endregion //User Code
        }

        public override void FdAmb_TurnUpdatesOnForObjectInstanceAdvisedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TurnUpdatesOnForObjectInstanceAdvisedHandler(sender, data);

            #region User Code
            // Start to update the position periodically TIMER
            manager.timer.Start(); // OpenRti does not support this callback
            #endregion //User Code
        }

        // -> idk
        public override void FdAmb_TurnUpdatesOffForObjectInstanceAdvisedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TurnUpdatesOffForObjectInstanceAdvisedHandler(sender, data);

            #region User Code
            // Stop to update the position TIMER
            manager.timer.Stop();
            #endregion //User Code
        }

        #endregion // Object Management Callbacks


        #region Time Management Callbacks
        // FdAmb_TimeRegulationEnabled
        public override void FdAmb_TimeRegulationEnabled(object sender, HlaTimeManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TimeRegulationEnabled(sender, data);

            #region User Code
            Report("FdAmb_TimeRegulationEnabled", ConsoleColor.Blue);

            Time = data.Time; //  Current logical time of the joined federate set by RTI
            Report("Logical time set by RTI TR: " + Time);
            #endregion //User Code
        }

        // FdAmb_TimeConstrainedEnabled
        public override void FdAmb_TimeConstrainedEnabled(object sender, HlaTimeManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TimeConstrainedEnabled(sender, data);

            #region User Code
            Report("FdAmb_TimeConstrainedEnabled", ConsoleColor.Blue);

            Time = data.Time; //  Current logical time of the joined federate set by RTI
            Report("Logical time set by RTI TC: " + Time);
            #endregion //User Code
        }

        // FdAmb_TimeAdvanceGrant
        public override void FdAmb_TimeAdvanceGrant(object sender, HlaTimeManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TimeAdvanceGrant(sender, data);

            #region User Code
            Report("FdAmb_TimeAdvanceGrant", ConsoleColor.Blue);

            Time = data.Time; //  Current logical time of the joined federate set by RTI
            Report("Logical time set by RTI: " + Time);
            #endregion //User Code
        }
        // FdAmb_RequestRetraction
        public override void FdAmb_RequestRetraction(object sender, HlaTimeManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_RequestRetraction(sender, data);

            #region User Code
            Report("FdAmb_RequestRetraction", ConsoleColor.Blue);

            throw new NotImplementedException("FdAmb_RequestRetraction");
            #endregion //User Code
        }
        #endregion //Time Management Callbacks


        //update the car position based on the timer, it is called in the simulation manager
        public void UpdateState(CTLightHlaObject tlight)
        {
            // Add Values
            Report("UpdateState", ConsoleColor.Blue);

            tlight.AddAttributeValue<TLState>(Som.TLightOC.state, tlight.tlight.state);
            UpdateAttributeValues(tlight);
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
            UpdateAttributeValues(tlight);
        }

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
