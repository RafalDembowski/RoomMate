using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RoomMate.Common.SaveImageClients
{
    public class SaveImageClient : ISaveImageClient
    {
        public void createDirectory(string directoryPath)
        {
            string fullDirectoryPath = HttpContext.Current.Server.MapPath(directoryPath);

            if (!Directory.Exists(fullDirectoryPath))
                Directory.CreateDirectory(fullDirectoryPath);
        }
        public void deleteFilesFromDirectory(string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(directoryPath));

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

        }
    }
}