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

        public void updateDirectionBasedOnArea()
        {
            switch (belong_area)
            {
                case Area.north_down:
                    heading_direction = Direction.south;
                    break;
                case Area.south_up:
                    heading_direction = Direction.north;
                    break;
                case Area.east_left:
                    heading_direction = Direction.west;
                    break;
                case Area.west_right:
                    heading_direction = Direction.east;
                    break;
            }
        }

        public void updatePositionBasedOnArea()
        {
            switch (belong_area)
            {
                //initial positions
                case Area.north_down:
                    position.X = -1;
                    position.Y = length_street + 1;
                    break;
                case Area.south_up:
                    position.X = 1;
                    position.Y = -(length_street + 1);
                    break;
                case Area.east_left:
                    position.X = length_street + 1;
                    position.Y = 1;
                    break;
                case Area.west_right:
                    position.X = -(length_street + 1);
                    position.Y = -1;
                    break;

                //after junction initial positions
                case Area.north_up:
                    position.X = 1;
                    position.Y = 1;
                    break;
                case Area.south_down:
                    position.X = -1;
                    position.Y = -1;
                    break;
                case Area.east_right:
                    position.X = 1;
                    position.Y = -1;
                    break;
                case Area.west_left:
                    position.X = -1;
                    position.Y = +1;
                    break;
            }
        }

        //method for checking if the initial positions are empty or not TODO

        // Computational Model
        public void Move(double dt)
        {
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
                position = newPostion;
                return;
            }

            //if position is not out of map
            //check if the new position is crossing the juction and correct the new position
            Boolean cross = false;
            switch (belong_area)
            {
                // before junction
                case Area.north_down:
                    if (newPostion.Y < 1)
                    {
                        cross = true;
                        newPostion.Y = 1;
                    }

                    break;
                case Area.south_up:
                    if (newPostion.Y > -1)
                    {
                        cross = true;
                        newPostion.Y = -1;
                    }
                    break;
                case Area.west_right:
                    if (newPostion.X > -1) 
                    {
                        cross = true;
                        newPostion.X = -1;
                    }
                    break;
                case Area.east_left:
                    if (newPostion.X < 1) 
                    {
                        cross = true;
                        newPostion.X = 1;
                    }
                    break;

            }

            //check if the new position is 

            if (cross)
            {
                Area newArea = chooseNextArea();
                //TODO: check the light for the area that it is in
                // if the light is red
                position = newPostion;
                return;

                //if light is green

            }



            switch (belong_area)
            {
                // after junction
                case Area.east_right:

                    break;
                case Area.north_up:
                    break;
                case Area.south_down:
                    break;
                case Area.west_left:
                    break;

                // before junction
                case Area.north_down:
                    break;
                case Area.south_up:
                    break;
                case Area.west_right:
                    break;
                case Area.east_left:
                    break;

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
    }

}
