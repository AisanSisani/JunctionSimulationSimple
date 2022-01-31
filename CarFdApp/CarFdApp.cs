// **************************************************************************************************
//		CCarFdApp
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
  public partial class CCarFdApp : Racon.CGenericFederate
  {
        #region Manually Added Code
    
        // Local Data
        private CSimulationManager manager;
        private object thisLock = new object(); //used when removing object

        #region Constructor
        public CCarFdApp(CSimulationManager parent) : this()
        {
          manager = parent; // Set simulation manager
          // Create regions manually
        }
        #endregion //Constructor

        // Cut and paste the callbacks that you want to modify from the Generated file (ShipFdApp.simge.cs)
        #region Federation Management Callbacks
        #endregion

        #region Declaration Management Callbacks
        // FdAmb_StartRegistrationForObjectClassAdvisedHandler
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            // Check that this is for the ShipOC
            if (data.ObjectClassHandle == Som.CarOC.Handle)
                RegisterObject(manager.CarObjects[0]);

            //TODO the timer starts here
            #endregion //User Code
        }
        #endregion

        #region Object Management Callbacks
        // FdAmb_ObjectDiscoveredHandler
        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            // if the object is a car
            if (data.ClassHandle == Som.CarOC.Handle)
            {
                // TODO: just save the cars that are in the same region (maybe)

                // Create and add a new car to the list
                CCarHlaObject newCar = new CCarHlaObject(data.ObjectInstance);
                newCar.Type = Som.CarOC;// if user forgets to set type of the object, then an exception generated
                manager.CarObjects.Add(newCar);

                // (3) Request Update Values for specific attributes only (in here all)
                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.CarOC.car_id);
                attributes.Add(Som.CarOC.belong_area);
                attributes.Add(Som.CarOC.heading_direction);
                attributes.Add(Som.CarOC.speed);
                attributes.Add(Som.CarOC.position);
                RequestAttributeValueUpdate(newCar, attributes);
            }
            // if the obejct is a tlight
            else if (data.ClassHandle == Som.TLightOC.Handle)
            {
                // Create and add a new tlight to the list
                CTLightHlaObject newTLight = new CTLightHlaObject(data.ObjectInstance);
                newTLight.Type = Som.TLightOC;// if user forgets to set type of the object, then an exception generated
                manager.TLightObjects.Add(newTLight);

                // (3) Request Update Values for specific attributes only (in here all)
                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.TLightOC.tlight_id);
                attributes.Add(Som.TLightOC.state);
                attributes.Add(Som.TLightOC.duration_green);
                attributes.Add(Som.TLightOC.duration_red);
                attributes.Add(Som.TLightOC.belong_area);
                RequestAttributeValueUpdate(newTLight, attributes);

            }
            #endregion //User Code
        }

        // FdAmb_AttributeValueUpdateRequestedHandler
        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

            #region User Code
            // !!! If this federate is created only one object instance, then it is sufficient to check the handle of that object, otherwise we need to check all the collection
            if (data.ObjectInstance.Handle == manager.CarObjects[0].Handle)
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
                UpdateAll(manager.CarObjects[0]);
                //UpdateName(manager.Ships[0]);
                //UpdatePosition(manager.Ships[0]);
                //UpdateHeading(manager.Ships[0]);
                //UpdateSpeed(manager.Ships[0]);
            }
            #endregion //User Code
        }

        // An Object is Removed
        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {   
            ///only the cars can be removed so hte handler is not checked
            ///TODO: maybe I should also check for the traffic lights
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            // Lock while taking a snapshot - to avoid foreach loop enumeration exception
            CCarHlaObject[] snap = new CCarHlaObject[manager.CarObjects.Count];
            lock (thisLock)
            {
                manager.CarObjects.CopyTo(snap, 0);
            }
            foreach (CCarHlaObject car in snap)
            {
                if (data.ObjectInstance.Handle == car.Handle)// Find the Object
                {
                    manager.CarObjects.Remove(car);
                    Report($"Ship: {car.car.car_id} left. Number of Ships Now: {manager.CarObjects.Count}" + Environment.NewLine);
                }
            }
            #endregion //User Code
        }
        #endregion



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

        // Update attribute values
        private void UpdateAll(CCarHlaObject car)
        {
            // Add Values
            car.AddAttributeValue(Som.CarOC.car_id, car.car.car_id);
            car.AddAttributeValue(Som.CarOC.belong_area, (uint)car.car.belong_area);
            car.AddAttributeValue(Som.CarOC.heading_direction, (uint)car.car.heading_direction);
            car.AddAttributeValue(Som.CarOC.speed, (uint)car.car.speed);
            car.AddAttributeValue<Coordinate>(Som.CarOC.position, car.car.position); 
            UpdateAttributeValues(car);
        }

        // report
        private void Report(string txt)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(txt);
        }

        //update the car position based on the timer, it is called in the simulation manager
        public void UpdatePosition(CCarHlaObject car)
        {
            // Add Values
            car.AddAttributeValue<Coordinate>(Som.CarOC.position, car.car.position);
            UpdateAttributeValues(car);
        }

        #endregion //Manually Added Code
    }
}
