using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLVII_Milica_Karetic
{
    class NotifyPassingVehicles
    {
        /// <summary>
        /// Notification message
        /// </summary>
        internal void NotifyUser(string name, string direction)
        {
            if (Program.threads.Any())
                Console.WriteLine(name + "'s direction is " + direction);
            else
                Console.WriteLine("There is " + Program.vehicleNum + " cars on the road.");
        }

    }
}
