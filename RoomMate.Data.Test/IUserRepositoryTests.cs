using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RoomMate.Data.Repository;
using RoomMate.Entities.Users;

namespace RoomMate.Data.Test
{
    [TestClass]
    public class IUserRepositoryTests
    {
        public IUserRepositoryTests()
        {
            IList<User> users = new List<User>
            {
                new User
                {
                    UserID = new Guid("8fa4048f-8b13-441c-b25d-728129b19e84"),
                    Email = "SaraConor@gmail.com",
                    PasswordHash = "E16B2AB8D12314BF4EFBD6203906EA6C",
                    UserName = "SaraConor",
                    FirsName = "Sara",
                    LastName = "Conor",
                    IsEmailVerified = true,
                    CodeActivation = new Guid("a4815c97-2a27-44fe-89dc-c023cccf18e8"),
                    CodeResetPassword = Guid.Empty,
                },
            };

            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository
                .Setup(m => m.GetAll())
                .Returns(users);

            mockUserRepository.Setup(m => m.IsUserWithEmailExist(It.IsAny<String>())).Returns(
            (String email) =>
            {
                User user = users.FirstOrDefault(u => u.Email.ToLower().Equals(email.ToLower()));
                if (user!=null)
                    return true;
                else
                    return false;
            });

            this.mockUserRepository = mockUserRepository.Object;

        }
        public IUserRepository mockUserRepository { get; private set; }

        [TestMethod]
        public void IUserRepository_IsUserWithEmailExist_CheckIsExistUserWithGivenEmailWithDifferentSizeOfLetters_Success()
        {
            string testEmail = "SaRaCoNoR@gmail.com";
            IList<User> testUserList = this.mockUserRepository.GetAll().ToList();

            bool emailExistInData = this.mockUserRepository.IsUserWithEmailExist(testEmail);

            Assert.AreEqual(emailExistInData, true);

        }
    }
}
