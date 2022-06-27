using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.ViewModels
{
    internal interface ICloseWindow
    {
        public Action? Close { get; set; }
    }
}
