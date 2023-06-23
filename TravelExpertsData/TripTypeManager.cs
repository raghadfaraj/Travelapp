using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Data;

namespace TravelExpertsData
{
    public class TripTypeManager
    {
        public static List<TripType> GetTripTypes(TravelExpertsContext context) 
        {
            List<TripType> tripTypes = null;
            tripTypes = context.TripTypes.ToList();
            return tripTypes;
        }
        
    }
}
