using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RoomMate.Entities.Users;

namespace RoomMate.Common.SaveImageClients
{
    public class UserSaveImageClient : SaveImageClient
    {
        private UserImage userImage;
        public UserImage saveUserImageToDiskAndReturnCreatedObject(IEnumerable<HttpPostedFileBase> userImages, Guid userID)
        {
            string directoryPath = "~/Content/IMAGE/" + userID + "/userImage/";

            createDirectory(directoryPath);
            deleteFilesFromDirectory(directoryPath);
            try
            {

                foreach (HttpPostedFileBase file in userImages)
                {
                    if (file != null)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string pathToImage = directoryPath + Path.GetFileName(file.FileName);
                        file.SaveAs(HttpContext.Current.Server.MapPath(pathToImage));
                        createNewUserImage(pathToImage, fileName);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e.Message);
                throw e;
            }
            return userImage;
        }

        private void createNewUserImage(string pathToImage, string fileName)
        {
            userImage = new UserImage();
            userImage.UserImageID = Guid.NewGuid();
            userImage.ImagePath = pathToImage;
            userImage.ImageName = fileName;

        }

    }
}