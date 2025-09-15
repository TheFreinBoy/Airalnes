using Airalnes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airalnes.RepoInterfaces
{
    public interface IAirplaneRepository
    {
        List<Airplane> GetAirplanes();
        List<Airport> GetAirports();
    }
}
