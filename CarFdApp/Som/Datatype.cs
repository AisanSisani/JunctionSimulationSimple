// **************************************************************************************************
//		Data Types
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
/// This file includes the enumerated and fixed record data types.
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
  #region Enumerated Datatypes
  // west, east, north, west
  public enum Direction {  west = 0, east = 1, north = 2, south = 3 };
  // north_down, ...
  public enum Area {  north_down = 0, north_up = 1, south_down = 2, south_up = 3, east_right = 4, east_left = 5, west_right = 6, west_left = 7 };
  public enum TLState {  red = 0, green = 1 };
  // speed
  public enum Pace {  very_slow = 0, slow = 1, medium = 2, fast = 3, very_fast = 4 };
  #endregion
  
  #region Fixed Record Datatypes
  public struct Coordinate
  {
    public double X; // Datatype defined in SOM: HLAfloat64Time
    public double Y; // Datatype defined in SOM: HLAfloat64Time
  }
  #endregion
  #region Variant Record Datatypes
  #endregion
  
}
