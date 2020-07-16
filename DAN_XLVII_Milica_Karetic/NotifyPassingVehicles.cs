using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLVII_Milica_Karetic
{
    /// <summary>
    /// Class for notification user
    /// </summary>
    class NotifyPassingVehicles
    {
        /// <summary>
        /// Notification message
        /// </summary>
        internal void NotifyUser(string name, string direction)
        {
            //at first time when there is no created vehicles write total number of vehicles
            //every other time write vehicles name and direction
            if (Program.threads.Any())
                Console.WriteLine(name + "'s direction is " + direction);
            else
                Console.WriteLine("There is " + Program.vehicleNum + " cars on the road.\n");
        }

    }
}
