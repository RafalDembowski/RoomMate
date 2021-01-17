using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMate.Data.Repository
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        string[] GetRandomDistinctCity();
    }
}
