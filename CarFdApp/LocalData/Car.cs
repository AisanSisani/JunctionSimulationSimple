using System;
using System.Collections.Generic;
using System.Text;

// Racon
using Racon;
using Racon.RtiLayer;
// App
using JSSimge.Som;

namespace JSSimge
{
    public class CCar
    {
        #region Declarations
        // Declare your local object structure here
        public string car_id;
        public Area belong_area;
        public Direction heading_direction;
        public Pace speed;
        public Coordinate position;

        public bool Exit = false; // is car exited the map?
        public double length_street = 20;

        public CSimulationManager manager;
        #endregion //Declarations

        public CCar()
        {
            // Instantiate local data here
            car_id = "1001";
            belong_area = Area.north_down;
            heading_direction = Direction.north;
            speed = Pace.medium;
            position.X = 1;
            position.Y = -100;
        }

        public Direction updateDirectionBasedOnArea(Area area)
        {
            Direction newDirection = Direction.east; // dummy
            switch (area)
            {
                case Area.north_down:
                case Area.south_down:
                    newDirection = Direction.south;
                    break;
                case Area.north_up:
                case Area.south_up:
                    newDirection = Direction.north;
                    break;
                case Area.west_left:
                case Area.east_left:
                    newDirection = Direction.west;
                    break;
                case Area.east_right:
                case Area.west_right:
                    newDirection = Direction.east;
                    break;
            }
            return newDirection;
        }

        public Coordinate updatePositionBasedOnArea(Area area)
        {
            Coordinate newPosition;
            newPosition.X = 0;
            newPosition.Y = 0;

            switch (area)
            {
                //initial positions
                case Area.north_down:
                    newPosition.X = -1;
                    newPosition.Y = length_street + 1;
                    break;
                case Area.south_up:
                    newPosition.X = 1;
                    newPosition.Y = -(length_street + 1);
                    break;
                case Area.east_left:
                    newPosition.X = length_street + 1;
                    newPosition.Y = 1;
                    break;
                case Area.west_right:
                    newPosition.X = -(length_street + 1);
                    newPosition.Y = -1;
                    break;

                //after junction initial positions
                case Area.north_up:
                    newPosition.X = 1;
                    newPosition.Y = 1;
                    break;
                case Area.south_down:
                    newPosition.X = -1;
                    newPosition.Y = -1;
                    break;
                case Area.east_right:
                    newPosition.X = 1;
                    newPosition.Y = -1;
                    break;
                case Area.west_left:
                    newPosition.X = -1;
                    newPosition.Y = +1;
                    break;
            }
            return newPosition;
        }

        //method for checking if the initial positions are empty or not TODO

        // Computational Model
        public void Move(double dt)
        {
            //Program.Report($"Move called position:({position.X},{position.Y})", ConsoleColor.Green);
            
            double d = 0.1; // distance taken in delta time.

            switch (speed)
            {
                case Pace.very_slow:
                    d = 1.0 * dt;
                    break;
                case Pace.slow:
                    d = 2.0 * dt;
                    break;
                case Pace.medium:
                    d = 3.0 * dt;
                    break;
                case Pace.fast:
                    d = 4.0 * dt;
                    break;
                case Pace.very_fast:
                    d = 5.0 * dt;
                    break;
            }
            //Program.Report($"--- d:{d}", ConsoleColor.Green);

            //calculate the new position
            Coordinate newPostion;
            newPostion.X = position.X;
            newPostion.Y = position.Y;

            switch (heading_direction)
            {
                case Direction.north:
                    newPostion.Y = position.Y + d;
                    break;
                case Direction.south:
                    newPostion.Y = position.Y - d;
                    break;
                case Direction.east:
                    newPostion.X = position.X + d;
                    break;
                case Direction.west:
                    newPostion.X = position.X - d;
                    break;
            }
            //Program.Report($"--- newPosition:({newPostion.X},{newPostion.Y})", ConsoleColor.Green);

            //check if the new position is out of map
            switch (belong_area)
            {
                // after junction
                case Area.east_right:
                    if (newPostion.X > length_street + 1)
                        Exit = true;
                    break;
                case Area.north_up:
                    if (newPostion.Y > length_street + 1)
                        Exit = true;
                    break;
                case Area.south_down:
                    if (newPostion.Y < -(length_street + 1))
                        Exit = true;
                    break;
                case Area.west_left:
                    if (newPostion.X < -(length_street + 1))
                        Exit = true;
                    break;
            }

            if (Exit)
            {
                //Program.Report($"--- Out of map", ConsoleColor.Green);
                position = newPostion;
                //TODO send the new position to the RTI
                //Program.Report($"--- position changed:({position.X},{position.Y})", ConsoleColor.Green);
                return;
            }

               

            //check if the new position is crossing  or at the juction and correct the new position to junction
            Boolean cross = false;
            switch (belong_area)
            {
                // before junction
                case Area.north_down:
                    if (newPostion.Y <= 1)
                    {
                        cross = true;
                        newPostion.Y = 1;
                    }

                    break;
                case Area.south_up:
                    if (newPostion.Y >= -1)
                    {
                        cross = true;
                        newPostion.Y = -1;
                    }
                    break;
                case Area.west_right:
                    if (newPostion.X >= -1) 
                    {
                        cross = true;
                        newPostion.X = -1;
                    }
                    break;
                case Area.east_left:
                    if (newPostion.X <= 1) 
                    {
                        cross = true;
                        newPostion.X = 1;
                    }
                    break;

            }
            
            Area newArea = belong_area;
            Direction newDirection = heading_direction;

            if (cross)
            {
                //Program.Report($"--- At or after jucntion newPostion: ({newPostion.X},{newPostion.Y})", ConsoleColor.Green);
                // check the light
                Boolean isGreen = isTLightGreen(newArea);
                if(isGreen)
                {
                    Program.Report($"--- Green light", ConsoleColor.Green);
                    newArea = chooseNextArea();
                    
                    newPostion = updatePositionBasedOnArea(newArea);
                    newDirection = updateDirectionBasedOnArea(newArea);
                    Program.Report($"--- New Area {newArea}", ConsoleColor.Red);
                    Program.Report($"--- New Position ({newPostion.X},{newPostion.Y})", ConsoleColor.Red);
                    Program.Report($"--- New Direction {newDirection}", ConsoleColor.Red);
                }
                else
                {
                    //Program.Report($"--- Red light", ConsoleColor.Green);
                    position = newPostion;
                    // send position to the RTI
                    //Program.Report($"--- position changed:({position.X},{position.Y})", ConsoleColor.Green);
                    return;

                }
                
            }

            // check if the new position empty
            Boolean empty = isPositionEmpty(newArea, newPostion);
            if (empty)
            {
                //Program.Report($"--- Empty Position", ConsoleColor.Green);
                position = newPostion;
                belong_area = newArea;
                heading_direction = newDirection;
                // TODO send the new position to the RTI
                //Program.Report($"--- position changed:({position.X},{position.Y})", ConsoleColor.Green);
                return;
            }
            else
            {
                //Program.Report($"--- Not Empty Position", ConsoleColor.Green);
                return;
            }
        }

        public Area chooseNextArea()
        {
            Area newArea;
            List<Area> list = new List<Area>();
            Random rnd = new Random();
            int index = rnd.Next(3);

            switch (belong_area)
            {
                // before junction
                case Area.north_down:
                    list = new List<Area>() {Area.west_left, Area.south_down, Area.east_right };
                    break;
                case Area.south_up:
                    list = new List<Area>() { Area.west_left, Area.north_up, Area.east_right }; 
                    break;
                case Area.west_right:
                    list = new List<Area>() { Area.north_up, Area.south_down, Area.east_right }; 
                    break;
                case Area.east_left:
                    list = new List<Area>() { Area.west_left, Area.south_down, Area.north_up }; 
                    break;
            }

            newArea = list[index];
            return newArea;
        }

        public Boolean isPositionEmpty(Area area, Coordinate position)
        {
            //not checking the current car
            for (int i = 1; i < manager.CarObjects.Count; i++)            {
                CCar car = manager.CarObjects[i].car;
                if ((car.belong_area == area) && (car.position.X == position.X) && (car.position.Y == position.Y))
                    return false;
            }

            return true;
        }

        public Boolean isPositionEmpty(Coordinate position)
        {
            //not checking the current car
            for (int i = 1; i < manager.CarObjects.Count; i++)
            {
                CCar car = manager.CarObjects[i].car;
                if ((car.position.X == position.X) && (car.position.Y == position.Y))
                    return false;
            }

            return true;
        }

        public Boolean isTLightGreen(Area area)
        {
            foreach (var item in manager.TLightObjects)
            {
                if (item.tlight.belong_area == area)
                {
                   
                    //manager.federate.askForUpdateTLight(item);

                    if (item.tlight.state == TLState.red)
                        return false;
                    else
                        return true;
                }
            }

            //Report($"Error: The traffic light in {area} has not been found", ConsoleColor.Red);
            return false;
        }

        private void Report(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
        }


        private void Report(string txt)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(txt);
        }




    }

}
