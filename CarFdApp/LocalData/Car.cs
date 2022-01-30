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
                    position.Y = 21;
                    break;
                case Area.south_up:
                    position.X = 1;
                    position.Y = -21;
                    break;
                case Area.east_left:
                    position.X = 21;
                    position.Y = 1;
                    break;
                case Area.west_right:
                    position.X = -21;
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

    
        //checking if the initial positions are empty or not TODO
    }

}
