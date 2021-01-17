using RoomMate.Data.Context;
using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomMate.Data.Repository
{
    public class AddressRepository : GenericRepository<Address> , IAddressRepository
    {
        public AddressRepository(RoomMateDbContext mainContext) : base(mainContext) { }
        public string[] GetRandomDistinctCity()
        {
            var addressesDistinct = _context
                                           .Addresses
                                           .Select(a => a.City)
                                           .Distinct()
                                           .OrderBy(a => Guid.NewGuid())
                                           .ToArray();

            return addressesDistinct;
        }
    }
}