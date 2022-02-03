
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
        static CTLight tlight = new CTLight();
        static bool Terminate = false; // exit switch for app

        // Communication layer related structures
        ///static public CTLightFdApp federate; //Application-specific federate 
       
        // Main Entrance for Application
        static void Main(string[] args)
        {
            // *************************************************
            // Program Initialization
            // *************************************************
            // Instantiation
            ///federate = new CTLightFdApp();// Initialize the application-specific federate
            // Local data
            ///tlight = new CTLightHlaObject(federate.Som.TLightOC);// local object

            // UI initialization -> ok
            PrintVersion();
            Thread ConsoleKeyListener = new Thread(new ThreadStart(ProcessKeyboard));
            ConsoleKeyListener.Name = "KeyListener";

            // Racon Initialization
            // Getting the information/debugging messages from RACoN
            manager.federate.StatusMessageChanged += Federate_StatusMessageChanged;
            manager.federate.LogLevel = LogLevel.ALL;

            // initialization of light Properties -> not ok
            Console.ForegroundColor = ConsoleColor.Yellow;
            setTLightConfiguration(); // get user input
            Console.Title = "TLightdApp: " + tlight.tlight_id; // set console title
            printConfiguration();// report to user
            ConsoleKeyListener.Start();// start keyboard event listener



            // *************************************************
            // RTI Initialization
            // *************************************************
            // Initialize the federation execution
            bool result = manager.federate.InitializeFederation(manager.federate.FederationExecution);

            // Initialize themes TODO
            // TM Initialization
            manager.federate.EnableAsynchronousDelivery();
            manager.federate.EnableTimeConstrained();

            // FM Test
            manager.federate.ListFederationExecutions();

            

            // *************************************************
            // Main Simulation Loop - loops until ESC is pressed
            // *************************************************

            do
            {
                // process rti events (callbacks) and tick
                if (manager.federate.FederateState.HasFlag(Racon.FederateStates.JOINED))
                    manager.federate.Run();

                if (tlight.state == TLState.red)
                {

                    Report($"State is red. Sleep for {(int)tlight.duration_red} ms.", ConsoleColor.Green);
                    Thread.Sleep((int)tlight.duration_red);
                    Report("Change state to green", ConsoleColor.Green);
                    tlight.state = TLState.green;
                }
                else
                {
                    Report($"State is green. Sleep for {(int)tlight.duration_green} ms.", ConsoleColor.Green);
                    Thread.Sleep((int)tlight.duration_green);
                    Report("Change state to red", ConsoleColor.Green);
                    tlight.state = TLState.red;
                }
            } while (!Terminate);

            // TM Tests
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

            // *************************************************
            // Shutdown
            // *************************************************
            // Finalize user interface
            ConsoleKeyListener.Interrupt();

            // Finalize Federation Execution
            // Remove objects
            manager.timer.Stop(); // stop reporting the ship position
            manager.federate.DeleteObjectInstance(manager.TLightObject);

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
            Console.ResetColor();
            Console.WriteLine("Racon Message: " + (sender as CTLightFdApp).StatusMessage);
        }

        // Set ship configuration
        private static void setTLightConfiguration()
        {
            // initialization with user input
            Console.ForegroundColor = ConsoleColor.Yellow;

            // tlight_id
            Console.WriteLine();
            Console.Write("Enter TLight ID: ");
            tlight.tlight_id = Console.ReadLine();

            // initial state
            tlight.state = TLState.red;

            // duration red
            int duration;
            Console.Write("Enter Red Duration: ");
            int.TryParse(Console.ReadLine(), out duration);
            tlight.duration_red = duration;

            // duration green
            Console.Write("Enter Green Duration: ");
            int.TryParse(Console.ReadLine(), out duration);
            tlight.duration_green = duration;

            // belong area
            int pos = 0;
            do
            {
                Console.Write("Enter Belong Area (north_down = 0, south_up = 3, east_left = 5, west_right = 6): ");
                int.TryParse(Console.ReadLine(), out pos);
            } while ((pos != 0) && (pos != 3) && (pos != 5) && (pos != 6));

            tlight.belong_area = (Area)(pos);

            // Encapsulate own tlight
            CTLightHlaObject encapsulatedShipObject = new CTLightHlaObject(manager.federate.Som.TLightOC);
            encapsulatedShipObject.tlight = tlight;
            //the light should be saved in the manager
            manager.TLightObject = encapsulatedShipObject;
        }
        private static void printConfiguration()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nThanks for input! The tlight configuration:");
            Console.WriteLine("ID: {0}", tlight.tlight_id);
            Console.WriteLine("State: {0}", tlight.state);
            Console.WriteLine("Area: {0}", tlight.belong_area);
            Console.WriteLine("Duration Red: {0}", tlight.duration_red);
            Console.WriteLine("Duration Green: {0}", tlight.duration_green);
        }

        // Print version
        private static void PrintVersion()
        {
            Console.WriteLine(
              "***************************************************************************\n"
              + "                        " + "TLightFdApp v1.0.0" + "\n"
              + "***************************************************************************");
        }
        // report
        private static void Report(string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
        }

    }
}