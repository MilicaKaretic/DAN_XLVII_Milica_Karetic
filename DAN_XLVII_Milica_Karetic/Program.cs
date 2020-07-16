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

        public static void Go()
        {
            string currentThread = Thread.CurrentThread.Name;

            int dir = rnd.Next(0, 2);
            string direction = "";

            if (dir == 0)
                direction = "North";
            else
                direction = "South";

            Notify(currentThread, direction);
            
        }

        static void Main(string[] args)
        {          
            NotifyPassingCars not = new NotifyPassingCars();
            carNum = rnd.Next(1, 16);

            onNotification = not.NotifyUser;
            Notify("", "");

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
