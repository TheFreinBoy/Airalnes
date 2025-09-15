using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airalnes.Models;
using Airalnes.Helpers;
using Airalnes.Interfaces;
using Airalnes.RepoInterfaces;

namespace Airalnes.Services
{
    public class AirplaneService : IAirplaneService
    {
        private readonly IAirplaneRepository _airplaneRepository;

        public AirplaneService(IAirplaneRepository airplaneRepository)
        {
            _airplaneRepository = airplaneRepository;
        }

        public List<Airplane> GetAllAirplanes()
        {
            return _airplaneRepository.GetAirplanes();
        }

        public List<Airport> GetAllAirports()
        {
            return _airplaneRepository.GetAirports();
        }
    }
}
