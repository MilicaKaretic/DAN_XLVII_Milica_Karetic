using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XLVII_Milica_Karetic
{
    class Program
    {
        public static Random rnd = new Random();
        public static List<Thread> threads = new List<Thread>();
        public static int carNum;

        //delegate for notification user
        public  delegate void Notification(int cars, string name, string direction);
        //event based on that delegate
        public static event Notification onNotification;
        public static event Notification onNotificationNum;

        /// <summary>
        /// Raise an event
        /// </summary>
        internal static void Notify(int cars, string name, string direction)
        {
            if (onNotification != null)
            {
                onNotification(cars, name, direction);
            }

        }

        /// <summary>
        /// Raise an event
        /// </summary>
        internal static void NotifyNum(int cars, string name, string direction)
        {
            if (onNotificationNum != null)
            {
                onNotificationNum(cars, name, direction);
            }

        }
        /// <summary>
        /// Notification message
        /// </summary>
        internal static void NotifyUser(int cars, string name, string direction)
        {
            Console.WriteLine(name + " is going " + direction);
        }

        /// <summary>
        /// Notification message
        /// </summary>
        internal static void NotifyCarsNum(int cars, string name, string direction)
        {
            Console.WriteLine("There is " + cars + " cars on the road.");
        }


        public static void Go()
        {
            string currentThread = Thread.CurrentThread.Name;
            int dir = rnd.Next(0, 2);
            string direction = "";

            if (dir == 0)
                direction = "North";
            else
                direction = "South";
            onNotification = NotifyUser;
            Notify(carNum, currentThread, direction);
            
        }

        static void Main(string[] args)
        {
            carNum = rnd.Next(1, 16);
            onNotificationNum = NotifyCarsNum;
            NotifyNum(carNum, "", "");

            for (int i = 0; i < carNum; i++)
            {
                Thread t = new Thread(new ThreadStart(Go))
                {
                    Name = string.Format("Car_{0}", i + 1)
                };
                threads.Add(t);
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
