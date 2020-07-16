using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XLVII_Milica_Karetic
{
    class Program
    {
        public static Random rnd = new Random();
        //list of vehicles
        public static List<Thread> threads = new List<Thread>();
        //random generated number of vehicles
        public static int vehicleNum;
        //object to lock
        private static readonly object locker = new object();
        //counter for locking
        static int count = 0;

        /// <summary>
        /// Letting vehicles set the direction in case there was an vehicle on the opposite side
        /// </summary>
        private static EventWaitHandle waitingDirection = new AutoResetEvent(true);
        /// <summary>
        /// Vehicle can pass the road
        /// </summary>
        public static EventWaitHandle nextVehicle = new AutoResetEvent(true);

        //current direction
        public static string currentDirection = "";

        //counter for first vehicle to pass the road
        public static int cntFirst = 0;

        /// <summary>
        /// delegate for notification user
        /// </summary>
        /// <param name="name">Car name</param>
        /// <param name="direction">Car direction</param>
        public delegate void Notification(string name, string direction);

        /// <summary>
        /// event based on that delegate
        /// </summary>
        public static event Notification onNotification;

        /// <summary>
        /// Raise an event
        /// </summary>
        internal static void Notify(string name, string direction)
        {
            if (onNotification != null)
            {
                onNotification(name, direction);
            }
        }    

        /// <summary>
        /// Method that vehicles call when they start
        /// </summary>
        public static void Go()
        {
            string currentThread = Thread.CurrentThread.Name;

            int dir = rnd.Next(0, 2);
            string direction = "";

            //setting vehicles direction
            if (dir == 0)
                direction = "North";
            else
                direction = "South";

            Notify(currentThread, direction);

            //wait all vehicles to get directions
            lock (locker)
            {
                count++;
            }
            while (count < vehicleNum)
            {
                Thread.Sleep(0);
            }

            //let first vehicle to pass the road
            nextVehicle.WaitOne();

            //passing road
            PassingRoad(direction);
        }

        /// <summary>
        /// Method for passing road
        /// </summary>
        /// <param name="direction">Vehicle's direction</param>
        public static void PassingRoad(string direction)
        {          
            // just first vehicle can pass and set the initial direction
            waitingDirection.WaitOne();

            //first vehicle
            if (cntFirst == 0)
            {
                //first vehicle will increase counter and other vehicles will skip this if condition
                cntFirst++;
                //set initial direction to first vehicle's direcion
                currentDirection = direction;
                Console.WriteLine(Thread.CurrentThread.Name + " is going " + direction);
                //indicates that now other vehicles also can pass the road if their direction is ok
                waitingDirection.Set();
                //next vehicle can enter the bridge
                nextVehicle.Set();
                //passing the road
                Thread.Sleep(500);
            }
            else if (currentDirection == direction)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " is going " + direction);
                waitingDirection.Set();
                nextVehicle.Set();
                Thread.Sleep(500);
            }
            else
            {
                Console.WriteLine(Thread.CurrentThread.Name + " is waiting to pass the bridge from {0} side.", direction);
                //change direction in which vehicles can go to the current vehicle's direction
                currentDirection = direction;
                //passing the road
                Thread.Sleep(500);
                waitingDirection.Set();
                //this vehicle can pass the road when nextVehicle event is set
                PassingRoad(direction);
            }          
        }

        static void Main(string[] args)
        {
            Stopwatch s = new Stopwatch();
            //start stopwatch 
            s.Start();

            NotifyPassingVehicles not = new NotifyPassingVehicles();
            vehicleNum = rnd.Next(1, 16);

            //notify user about total number of created vehicles
            onNotification = not.NotifyUser;
            Notify("", "");

            //create vehicles
            for (int i = 0; i < vehicleNum; i++)
            {
                Thread t = new Thread(new ThreadStart(Go))
                {
                    Name = string.Format("Vehicle_{0}", i + 1)
                };
                threads.Add(t);
            }

            //start vehicles
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }
            //join them
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

            //end stopwatch after all threads finished their job
            s.Stop();

            //write time to console
            WriteApplicationTime(s);

            Console.ReadLine();
        }

        /// <summary>
        /// Write application duration
        /// </summary>
        /// <param name="s">Stopwatch</param>
        public static void WriteApplicationTime(Stopwatch s)
        {
            TimeSpan ts = s.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds);
            //write time to console
            Console.WriteLine("\nRunTime ---> " + elapsedTime);
        }
    }
}
