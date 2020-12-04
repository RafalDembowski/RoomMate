using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RoomMate.Entities.Users;
using RoomMate.Data.Repository;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace RoomMate.Data.Test
{
    [TestClass]
    public class IGenericRepositoryTests
    {
      public IGenericRepositoryTests()
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
                new User
                {
                    UserID = new Guid("e64fc991-63ac-42d5-94e6-a3345d4b28ac"),
                    Email = "TomJson@gmail.com",
                    PasswordHash = "E16B2AB8D12314BF4EFBD6203906EA6C",
                    UserName = "TomJson",
                    FirsName = "Tom",
                    LastName = "Json",
                    IsEmailVerified = true,
                    CodeActivation = new Guid("59ecdf59-4afe-43b9-8af9-e7ce96dadac6"),
                    CodeResetPassword = Guid.Empty,
                }
            };


            Mock<IGenericRepository<Object>> mockUserRepository = new Mock<IGenericRepository<Object>>();

            //Get all users from data
            mockUserRepository
                .Setup(m => m.GetAll())
                .Returns(users);
            //Get user by id from data
            mockUserRepository
                .Setup(m => m.GetById(
                It.IsAny<Guid>())).Returns((Guid g) => users.Where(
                x => x.UserID == g).Single());
            //Insert user
            mockUserRepository
                .Setup(m => m.Insert(
                It.IsAny<User>()))
                .Callback((object user) => users.Add((User)user));
            //Update user
            mockUserRepository
              .Setup(m => m.Update(
              It.IsAny<User>()))
              .Callback(
              (object obj) =>
              {
                  User updateUser = (User)obj;

                  var existUser = users.Where(u => u.UserID == updateUser.UserID).Single();
                  if (existUser != null)
                  {
                      existUser.Email = updateUser.Email;
                      existUser.PasswordHash = updateUser.PasswordHash;
                      existUser.UserName = updateUser.UserName;
                      existUser.FirsName = updateUser.FirsName;
                      existUser.LastName = updateUser.LastName;
                      existUser.IsEmailVerified = updateUser.IsEmailVerified;
                      existUser.CodeActivation = updateUser.CodeActivation;
                      existUser.CodeResetPassword = updateUser.CodeResetPassword;

                  }
              });
            //Delete user
            mockUserRepository
                .Setup(m => m.Delete(It.IsAny<Guid>()))
                .Callback(
                (object g) =>
                {
                    Guid id = (Guid)g;
                    User userToDelete = (User)this.mockUserRepository.GetById(id);
                    users.Remove(userToDelete);
                });

            this.mockUserRepository = mockUserRepository.Object;

        }

        public IGenericRepository<Object> mockUserRepository { get; private set; }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void IGenericRepository_GettAll_TakeAllRecordsFromData_Success()
        {
            IList testUsers = this.mockUserRepository.GetAll().ToList();

            Assert.IsNotNull(testUsers);
            Assert.AreEqual(2 , testUsers.Count);

        }
        [TestMethod]
        public void IGenericRepository_GetById_TakeUserFromData_Success()
        {
            Guid id = new Guid("8fa4048f-8b13-441c-b25d-728129b19e84");
            User testUser = (User)this.mockUserRepository.GetById(id);

            Assert.IsNotNull(testUser);
            Assert.IsInstanceOfType(testUser, typeof(User));
            Assert.AreEqual("Conor", testUser.LastName);
        }
        [TestMethod]
        public void IGenericRepository_InsertUser_InsertNewUserToData_Success()
        {
            User newTestUser = new User
            {
                UserID = new Guid("4edc8fa9-32df-45c7-bfe9-f61d49c0d95e"),
                Email = "MarekKonrad@gmail.com",
                PasswordHash = "E16B2AB8D12314BF4EFBD6203906EA6C",
                UserName = "MarekKonrad",
                FirsName = "Marek",
                LastName = "Konrad",
                IsEmailVerified = true,
                CodeActivation = new Guid("94bfa942-e729-4d34-8cbf-801009cda6b5"),
                CodeResetPassword = Guid.Empty,
            };

            int userCount = this.mockUserRepository.GetAll().ToList().Count();
            Assert.AreEqual(2, userCount);

            this.mockUserRepository.Insert(newTestUser);
            //verify that count has been increased
            userCount = this.mockUserRepository.GetAll().ToList().Count();
            Assert.AreEqual(3, userCount);
        }
        [TestMethod]
        public void IGenericRepository_UpdateUser_UpdateNewUserToData_Success()
        {
            Guid id = new Guid("8fa4048f-8b13-441c-b25d-728129b19e84");
            User testUser = (User)this.mockUserRepository.GetById(id);

            testUser.LastName = "Nowak";
            this.mockUserRepository.Update(testUser);
            testUser = (User)this.mockUserRepository.GetById(id);

            Assert.AreEqual("Nowak", testUser.LastName);

        }
        [TestMethod]
        public void IGenericRepository_DeleteUser_Success()
        {
           
            Guid id = new Guid("8fa4048f-8b13-441c-b25d-728129b19e84");

            int userCount = this.mockUserRepository.GetAll().ToList().Count();
            Assert.AreEqual(2, userCount);

            this.mockUserRepository.Delete(id);
            //verify that count has been increased
            userCount = this.mockUserRepository.GetAll().ToList().Count();
            Assert.AreEqual(1, userCount);
        }
    }
}
