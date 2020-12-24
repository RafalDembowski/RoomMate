using RoomMate.Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMate.Common.SaveImageClients
{
    interface ISaveImageClient
    {
        void createDirectory(string directoryPath);
        void deleteFilesFromDirectory(string directoryPath);
    }
}
