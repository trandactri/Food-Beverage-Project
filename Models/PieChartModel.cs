using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Pie Chart Model
    /// </summary>
    public class PieChartModel
    {
        /// <summary>
        /// Product 
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int? Quantity { get; set; }
    }
}