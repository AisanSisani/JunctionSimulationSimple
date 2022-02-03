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
                if (data.ObjectClassHandle == Som.CarOC.Handle)
                    RegisterObject(manager.CarObjects[0]);

                //TODO the timer starts here
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
            // RTI has discovered objects you may like
            public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
            {
                // Call the base class handler
                base.FdAmb_ObjectDiscoveredHandler(sender, data);

                #region User Code
                Report("FdAmb_ObjectDiscoveredHandler", ConsoleColor.Blue);
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

            // other federates asks you to update you values
            public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
            {
                // Call the base class handler
                base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

                #region User Code
                Report("FdAmb_AttributeValueUpdateRequestedHandler", ConsoleColor.Blue);

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

            // RTI has sent you some updates regarding other objects you may like
            public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
            {
                // Call the base class handler
                base.FdAmb_ObjectAttributesReflectedHandler(sender, data);

                #region User Code

                Report("", ConsoleColor.Blue);
                foreach (var item in manager.CarObjects)
                {
                    if (data.ObjectInstance.Handle == item.Handle)
                    {

                        if (data.IsValueUpdated(Som.CarOC.car_id))
                            item.car.car_id = data.GetAttributeValue<string>(Som.CarOC.car_id);
                        
                        if (data.IsValueUpdated(Som.CarOC.belong_area))
                            item.car.belong_area = (Area)data.GetAttributeValue<uint>(Som.CarOC.belong_area);
                        
                        if (data.IsValueUpdated(Som.CarOC.heading_direction))
                            item.car.heading_direction = (Direction)data.GetAttributeValue<uint>(Som.CarOC.heading_direction);
                        
                        if (data.IsValueUpdated(Som.CarOC.speed))
                            item.car.speed = (Pace)data.GetAttributeValue<uint>(Som.CarOC.speed);

                        if (data.IsValueUpdated(Som.CarOC.position))
                            item.car.position = data.GetAttributeValue<Coordinate>(Som.CarOC.position);
                            
                        // report to the user
                        Report($"Foreign Car updated car_id:{item.car.car_id} area:{item.car.belong_area} position:({item.car.position.X},{item.car.position.Y})" + Environment.NewLine);

                    }
                }
                foreach (var item in manager.TLightObjects)
                {
                    if (data.ObjectInstance.Handle == item.Handle)
                    {
                        // Get parameter values - 1st method
                        // First check whether  the attr is updated or not. Result returns 0/null if the updated attribute set does not contain the attr and its value 
                        if (data.IsValueUpdated(Som.TLightOC.tlight_id))
                            item.tlight.tlight_id = data.GetAttributeValue<string>(Som.TLightOC.tlight_id);
                        if (data.IsValueUpdated(Som.TLightOC.state))
                            item.tlight.state = (TLState)data.GetAttributeValue<uint>(Som.TLightOC.state);
                        if (data.IsValueUpdated(Som.TLightOC.duration_red))
                            item.tlight.duration_red = data.GetAttributeValue<Int64>(Som.TLightOC.duration_red);
                        if (data.IsValueUpdated(Som.TLightOC.duration_green))
                            item.tlight.duration_green = data.GetAttributeValue<Int64>(Som.TLightOC.duration_green);
                        if (data.IsValueUpdated(Som.TLightOC.belong_area))
                            item.tlight.belong_area = (Area)data.GetAttributeValue<uint>(Som.TLightOC.belong_area);
                        // report to the user
                        Report($"Foreign TLight update tlight_id{item.tlight.tlight_id} area:{item.tlight.belong_area} state:{item.tlight.state}");
                    }
                }
                #endregion //User Code
            }

            // RTI says an Object is Removed, remove it yourself too if you have saved it
            public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
            {   
                ///only the cars (cars that you did not created) can be removed so the handler is not checked
                ///TODO: maybe I should also check for the traffic lights
                // Call the base class handler
                base.FdAmb_ObjectRemovedHandler(sender, data);

                #region User Code
                Report("FdAmb_ObjectRemovedHandler", ConsoleColor.Blue);
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
        public void UpdatePosition(CCarHlaObject car)
        {
            Report("UpdatePosition", ConsoleColor.Blue);

            // Add Values
            car.AddAttributeValue<Coordinate>(Som.CarOC.position, car.car.position);
            UpdateAttributeValues(car);
        }

        // Update attribute values
        public void UpdateAll(CCarHlaObject car)
        {
            Report("UpdateAll", ConsoleColor.Blue);

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
        private void Report(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
        }

        #endregion //Manually Added Code
    }
}
