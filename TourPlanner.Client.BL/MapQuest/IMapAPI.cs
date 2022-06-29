using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL.MapQuest
{
    public interface IMapAPI
    {
        Task<MapDirections?> GetDirections(string uri);
        Task<byte[]?> GetMapImage(string uri);
    }
}
