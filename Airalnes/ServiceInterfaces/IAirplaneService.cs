using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.Interfaces
{
    public interface IAirplaneService
    {
        List<Airplane> GetAllAirplanes();
        List<Airport> GetAllAirports();
    }
}
