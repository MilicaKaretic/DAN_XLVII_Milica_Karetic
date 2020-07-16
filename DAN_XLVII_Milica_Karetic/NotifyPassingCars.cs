using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLVII_Milica_Karetic
{
    class NotifyPassingCars
    {
        /// <summary>
        /// Notification message
        /// </summary>
        internal void NotifyUser(string name, string direction)
        {
            if (Program.threads.Any())
                Console.WriteLine(name + " is going " + direction);
            else
                Console.WriteLine("There is " + Program.carNum + " cars on the road.");
        }

    }
}
