﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;

namespace SportsStore.UnitTest
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            LoginViewModel model = new LoginViewModel
                                                    {
                                                        UserName = "admin",
                                                        Password = "secret"
                                                    };
            AccountController target = new AccountController(mock.Object);
            ActionResult result = target.Login(model, "/MyURL");
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }
        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);
            LoginViewModel model = new LoginViewModel
                                                    {
                                                        UserName = "badUser",
                                                        Password = "badPass"
                                                    };
            AccountController target = new AccountController(mock.Object);
            ActionResult result = target.Login(model, "/MyURL");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }

    }
}
