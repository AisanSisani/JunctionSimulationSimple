
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
    public class Program
    {
        // Globals
        static CSimulationManager manager = new CSimulationManager();

        // Local object
        static CCar car = new CCar();
        static bool Terminate = false; // exit switch for app
        static double old_time = DateTime.Now.TimeOfDay.TotalSeconds; //TIMER

        // Communication layer related structures
        ///static public CTLightFdApp federate; //Application-specific federate 

        // Main Entrance for Application
        static void Main(string[] args)
        {
            // *************************************************
            // Program Initialization
            // *************************************************

            // UI initialization -> ok
            PrintVersion();
            Thread ConsoleKeyListener = new Thread(new ThreadStart(ProcessKeyboard));
            ConsoleKeyListener.Name = "KeyListener";

            // Racon Initialization -> ok
            // Getting the information/debugging messages from RACoN
            manager.federate.StatusMessageChanged += Federate_StatusMessageChanged;
            manager.federate.LogLevel = LogLevel.ALL;

            // initialization of light Properties -> ok
            Console.ForegroundColor = ConsoleColor.Yellow;
            setCarConfiguration(); // get user input
            Console.Title = "CarFdApp: " + car.car_id; // set console title
            manager.federate.FederationExecution.FederateName = car.car_id; // set feedrate name
            printConfiguration();// report to user
            ConsoleKeyListener.Start();// start keyboard event listener



            // *************************************************
            // RTI Initialization
            // *************************************************
            // Initialize the federation execution
            bool result = manager.federate.InitializeFederation(manager.federate.FederationExecution);

            // Initialize themes TODO
            // TM Initialization
            ///manager.federate.EnableAsynchronousDelivery();
            ///manager.federate.EnableTimeConstrained();

            // FM Test
            manager.federate.ListFederationExecutions();


            // *************************************************
            // Main Simulation Loop - loops until ESC is pressed
            // *************************************************
            old_time = DateTime.Now.TimeOfDay.TotalSeconds;
            int iteration = 0;
            do
            {
                //Report($"Simulation Iteration {iteration++}", ConsoleColor.Cyan);
                //printStatus();
                // process rti events (callbacks) and tick
                if (manager.federate.FederateState.HasFlag(Racon.FederateStates.JOINED))
                    manager.federate.Run();

                // Move our local ship
                //car.Move(GetTimeStep());
                //Report($"position: ({car.position.X},{car.position.Y})", ConsoleColor.Green);
                // send the updates
                //manager.federate.SendMessage(car.car_id, car.belong_area, car.position); 

            } while (!Terminate && !car.Exit);

            // TM Tests
            /*
            manager.federate.DisableAsynchronousDelivery();
            manager.federate.DisableTimeConstrained();
            manager.federate.QueryLogicalTime();
            manager.federate.QueryLookahead(); // generates exception as this federate is TC.
            double galt;
            bool res = manager.federate.queryGALT(out galt);
            bool res2 = manager.federate.queryGALT();
            double lits;
            bool res4 = manager.federate.QueryLITS(out lits);
            bool res3 = manager.federate.QueryLITS();
            */

            // *************************************************
            // Shutdown
            // *************************************************
            // Finalize user interface
            ConsoleKeyListener.Interrupt();

            

            // Finalize Federation Execution
            // Remove objects
            manager.timer.Stop(); // stop reporting the ship position
            manager.federate.DeleteObjectInstance(manager.CarObjects[0]); //destroy the car you registred

            // Leave and destroy federation execution
            bool result2 = manager.federate.FinalizeFederation(manager.federate.FederationExecution);

            // Dumb trace log
            System.IO.StreamWriter file = new System.IO.StreamWriter(@".\TraceLog.txt");
            file.WriteLine(manager.federate.TraceLog);
            file.Close();

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        // Get time step
        private static double GetTimeStep()
        {
            // Calculate simulation step time
            double curr_time, dt;

            curr_time = DateTime.Now.TimeOfDay.TotalSeconds;
            dt = curr_time - old_time;
            old_time = curr_time;

            return dt;
        }

        // Process KB events
        private static void ProcessKeyboard()
        {
            do
            {
                // input processing
                switch (Console.ReadKey(true).Key)
                {

                    case ConsoleKey.T:
                        Terminate = true;
                        break;
                }
            } while (true);
        }
        // Racon Information received
        private static void Federate_StatusMessageChanged(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Racon Message: " + (sender as CCarFdApp).StatusMessage);
        }

        // Set Car configuration
        private static void setCarConfiguration()
        {
            /*
            // initialization with user input
            Console.ForegroundColor = ConsoleColor.Yellow;

            // car_id
            Console.WriteLine();
            Console.Write("Enter Car ID: ");
            car.car_id = Console.ReadLine();

            //public Area belong_area;
            int pos = 0;
            do
            {
                Console.Write("Enter Initial Belong Area (north_down = 0, south_up = 3, east_left = 5, west_right = 6): ");
                int.TryParse(Console.ReadLine(), out pos);
            } while ((pos != 0) && (pos != 3) && (pos != 5) && (pos != 6));

            car.belong_area = (Area)(pos); 

            //public Direction heading_direction;
            //based on belong area
            car.heading_direction = car.updateDirectionBasedOnArea(car.belong_area);

            //public Coordinate position;
            //based on belong area
            car.position = car.updatePositionBasedOnArea(car.belong_area);

            //public Pace speed;
            pos = 0;
            do
            {
                Console.Write("Enter Speed (very_slow(+1)=0, slow(+2)=1, medium(+3)=2, fast(+4)=3), very_fast(+5)=4: ");
                int.TryParse(Console.ReadLine(), out pos);
            } while ((pos != 0) && (pos != 1) && (pos != 2) && (pos != 3) && (pos != 4));

            car.speed = (Pace)(pos);
            */
            car.car_id = "first_car";
            car.belong_area = Area.north_down;
            car.heading_direction = car.updateDirectionBasedOnArea(car.belong_area);
            car.position = car.updatePositionBasedOnArea(car.belong_area);
            car.speed = Pace.very_fast;

            car.manager = manager;
       
            // Encapsulate own car
            CCarHlaObject encapsulatedCarObject = new CCarHlaObject(manager.federate.Som.CarOC);
            encapsulatedCarObject.car = car;
            // add the car created to the simulation manager it will be index 0
            manager.CarObjects.Add(encapsulatedCarObject);
        }
        private static void printConfiguration()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nThanks for input! The car configuration:");
            Console.WriteLine("ID: {0}", car.car_id);
            Console.WriteLine("Belong Area: {0}", car.belong_area);
            Console.WriteLine("Heading Direction: {0}", car.heading_direction);
            Console.WriteLine("Speed: {0}", car.speed);
            Console.WriteLine("Position: ({0}, {1})", car.position.X, car.position.Y);
        }

        // Print status TODO: fill them with more data, also check where it is called
        public static void printStatus()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*************** Status ***************");
            Console.WriteLine("Cars:");
            foreach (var item in manager.CarObjects)
            {
                Console.WriteLine($"{item.car.car_id}: {item.car.belong_area}, ({item.car.position.X}, {item.car.position.Y})");
            }
            Console.WriteLine("\nTLights:");
            foreach (var item in manager.TLightObjects)
            {
                Console.WriteLine($"{item.tlight.tlight_id}: {item.tlight.belong_area} {item.tlight.state}");
            }
            Console.WriteLine("**************************************");
        }


        // Print version
        private static void PrintVersion()
        {
            Console.WriteLine(
              "***************************************************************************\n"
              + "                        " + "CarFdApp v1.0.0" + "\n"
              + "***************************************************************************");
        }

        // report
        public static void Report(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
        }

    }
}