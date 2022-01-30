
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
        static CTLightHlaObject tlight;
        static bool Terminate = false; // exit switch for app

        // Communication layer related structures
        static public CTLightFdApp federate; //Application-specific federate 
       
        // Main Entrance for Application
        static void Main(string[] args)
        {
            // *************************************************
            // Initialization
            // *************************************************
            // Instantiation
            federate = new CTLightFdApp();// Initialize the application-specific federate
             // Local data
            tlight = new CTLightHlaObject(federate.Som.TLightOC);// local object

            // UI initialization
            PrintVersion();
            Thread ConsoleKeyListener = new Thread(new ThreadStart(ProcessKeyboard));
            ConsoleKeyListener.Name = "KeyListener";

            // Racon Initialization
            // Getting the information/debugging messages from RACoN
            federate.StatusMessageChanged += Federate_StatusMessageChanged;
            federate.LogLevel = LogLevel.ALL;

            // initialization of station Properties
            Console.ForegroundColor = ConsoleColor.Yellow;
            setTLightConfiguration(); // get user input
            Console.Title = "StationFdApp: " + tlight.tlight_id; // set console title
            printConfiguration();// report to user
            ConsoleKeyListener.Start();// start keyboard event listener

            // *************************************************
            // RTI Initialization
            // *************************************************
            // Initialize the federation execution
            federate.FederationExecution.FederateName = tlight.tlight_id; // set federate name
            federate.FederationExecution.Name = "Dardanelles";
            federate.FederationExecution.FederateType = "TLightFederate";
            federate.FederationExecution.ConnectionSettings = "rti://127.0.0.1";
            federate.FederationExecution.FDD = @"C:\Users\aisan\aisan_space\aisan_work\projects\JunctionSimulationSimple\JunctionSimulationVS\JunctionSimulationSimple\TLightFdApp\Som\JSFom.xml";
            // Connect
            federate.Connect(CallbackModel.EVOKED, federate.FederationExecution.ConnectionSettings);
            // Create federation execution
            federate.CreateFederationExecution(federate.FederationExecution.Name, federate.FederationExecution.FomModules);
            // Join federation execution
            federate.JoinFederationExecution(federate.FederationExecution.FederateName, federate.FederationExecution.FederateType, federate.FederationExecution.Name, federate.FederationExecution.FomModules);
            // Declare Capability
            federate.DeclareCapability();

            // *************************************************
            // Main Simulation Loop - loops until ESC is pressed
            // *************************************************

            //initializing the light to red at the beginning
            tlight.state = TLState.red;
            do
            {
                // process rti events (callbacks) and tick
                if (federate.FederateState.HasFlag(FederateStates.JOINED))
                    federate.Run();

                if(tlight.state == TLState.red)
                {
                    Console.WriteLine("State is red. Sleep for {0} ms.", (int)tlight.duration_red);
                    Thread.Sleep((int)tlight.duration_red);
                    Console.WriteLine("Change state to green");
                    tlight.state = TLState.green;
                }
                else
                {
                    Console.WriteLine("State is green. Sleep for {0} ms.", (int)tlight.duration_green);
                    Thread.Sleep((int)tlight.duration_green);
                    Console.WriteLine("Change state to red");
                    tlight.state = TLState.red;
                }

            } while (!Terminate);


            // *************************************************
            // Shutdown
            // *************************************************
            // Finalize user interface
            ConsoleKeyListener.Interrupt();
            // Finalize Federation Execution
            bool result2 = federate.FinalizeFederation(federate.FederationExecution, ResignAction.NO_ACTION);
            
            // Dumb trace log
            System.IO.StreamWriter file = new System.IO.StreamWriter(@".\TraceLog.txt");
            file.WriteLine(federate.TraceLog);
            file.Close();
            //Console.WriteLine(federate.TraceLog.ToString());

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
            Console.WriteLine((sender as CTLightFdApp).StatusMessage);
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

            // state
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
    }
}