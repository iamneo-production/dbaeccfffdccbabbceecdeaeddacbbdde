using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Week3MVC.Controllers;
using Week3MVC.Models;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace TestProject
{
    public class Tests
    {
        private PlayerController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name
                .Options;

            _context = new ApplicationDbContext(options);

            _controller = new PlayerController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the database context to release resources
            _context.Dispose();
        }
        [Test]
        public void Week2_day5_Player_ClassExists()
        {
            string assemblyName = "Week3MVC";
            string typeName = "Week3MVC.Models.Player";
            Assembly assembly = Assembly.Load(assemblyName);
            Type playerType = assembly.GetType(typeName);
            Assert.IsNotNull(playerType);
        }
        [Test]
        public void Week2_Day5_Player_Properties_Id_ReturnExpectedDataTypes_int()
        {
            string assemblyName = "Week3MVC";
            string typeName = "Week3MVC.Models.Player";
            Assembly assembly = Assembly.Load(assemblyName);
            Type playerType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = playerType.GetProperty("Id");
            Assert.IsNotNull(propertyInfo, "The property 'Id' was not found on the Player class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(int), propertyType, "The data type of 'Id' property is not as expected (int).");
        }

        [Test]
        public void Week2_Day5_Player_Properties_BiddingAmount_ReturnExpectedDataTypes_decimal()
        {
            string assemblyName = "Week3MVC";
            string typeName = "Week3MVC.Models.Player";
            Assembly assembly = Assembly.Load(assemblyName);
            Type playerType = assembly.GetType(typeName);
            PropertyInfo propertyInfo = playerType.GetProperty("BiddingAmount");
            Assert.IsNotNull(propertyInfo, "The property 'BiddingAmount' was not found on the Player class.");
            Type propertyType = propertyInfo.PropertyType;
            Assert.AreEqual(typeof(decimal), propertyType, "The data type of 'BiddingAmount' property is not as expected (decimal).");
        }

        [Test]
        public void Week2_Day6_IndexMethod_PlayerController_Exists()
        {            

            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);

            MethodInfo method = controllerType.GetMethod("Index");

            var result = method.Invoke(controller, null) as IActionResult;

            Assert.IsNotNull(result);            
        }

        [Test]
        public void Week2_Day6_IndexMethod_PlayerController_DisplaysAllPlayers()
        {
            var playersData = new List<Player>
            {
                new Player { Id = 1, Name = "Player 1", Category = "bowler", BiddingAmount = 100m },
                new Player { Id = 2, Name = "Player 2", Category = "bowler", BiddingAmount = 100m }
            };

            _context.Players.AddRange(playersData);
            _context.SaveChanges();

            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);

            MethodInfo method = controllerType.GetMethod("Index");

            var result = method.Invoke(controller, null) as IActionResult;

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<Player>;
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Week3_Day2_Test_IndexViewFile_Exists()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/Week3MVC/Views/Player", "Index.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Index.cshtml view file does not exist.");
        }

        [Test]
        public void Week3_Day2_VerifyIndexRoute()
        {
            VerifyRouteForAction<PlayerController>("Index", "");
        }

        [Test]
        public void Week3_Day2_VerifyCreateRoute()
        {
            VerifyRouteForAction<PlayerController>("Create", "create");
        }

        [Test]
        public void Week3_Day2_Player_NameIsRequired()
        {
            // Arrange
            var player = new Player
            {
                // Missing 'Name' property
                Category = "bowler",
                BiddingAmount = 100m
            };

            // Act
            var results = ValidateModel(player);

            // Assert
            var nameRequiredResult = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(Player.Name)));
            Assert.NotNull(nameRequiredResult);
            Assert.AreEqual("Name is required.", nameRequiredResult.ErrorMessage);
        }

        [Test]
        public void Week3_Day2_Player_BiddingAmountMustBeGreaterThanZero()
        {
            // Arrange
            var player = new Player
            {
                Name = "Player 1",
                Category = "bowler",
                BiddingAmount = 0
            };

            // Act
            var results = ValidateModel(player);

            // Assert
            var biddingAmountRangeResult = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(Player.BiddingAmount)));
            Assert.NotNull(biddingAmountRangeResult);
            Assert.AreEqual("Bidding amount must be greater than 0.", biddingAmountRangeResult.ErrorMessage);
        }

        [Test]
        public void Week3_Day3_Test_LoginViewFile_Exists()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/Week3MVC/Views/Account", "Login.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Login.cshtml view file does not exist.");
        }

        [Test]
        public void Week3_Day3_Test_RegisterViewFile_Exists()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/Week3MVC/Views/Account", "Register.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Register.cshtml view file does not exist.");
        }

        [Test]
        public void Week3_Day3_CreateMethod_PlayerController_Add_Player_to_DB()
        {
            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);

            MethodInfo method = controllerType.GetMethod("Create", new[] { typeof(Player) });
            var player = new Player
            {
                Name = "John Doe",
                Category = "Bowler",
                BiddingAmount = 1000m,
            };
            var result = method.Invoke(controller, new object[] { player });
            Console.WriteLine(result);
            Assert.IsNotNull(result);

            var ride = _context.Players.FirstOrDefault(r => r.Id == 1);
            Console.WriteLine(ride.Name);
            Assert.IsNotNull(ride);
            Assert.AreEqual(1, ride.Id);
            Assert.AreEqual("John Doe", ride.Name);
            Assert.AreEqual("Bowler", ride.Category);
            Assert.AreEqual(1000m, ride.BiddingAmount);
        }

        [Test]
        public void Week3_Day5_EditMethod_PlayerController_Exists()
        {
            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);

            MethodInfo method = controllerType.GetMethod("Edit", new[] { typeof(int) });
            var result = method.Invoke(controller, new object[] { 1 });
            Assert.IsNotNull(result); 
        }

        [Test]
        public void Week3_Day5_DeleteMethod_PlayerController_Exists()
        {
            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);

            MethodInfo method = controllerType.GetMethod("Delete", new[] { typeof(int) });
            var result = method.Invoke(controller, new object[] { 1 });
            Assert.IsNotNull(result);
        }

        [Test]
        public void Week3_Day5_Test_CreateViewFile_Exists()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/Week3MVC/Views/Player", "Create.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Create.cshtml view file does not exist.");
        }

        [Test]
        public void Week3_Day5_Test_EditViewFile_Exists()
        {
            string indexPath = Path.Combine(@"/home/coder/project/workspace/Week3MVC/Views/Player", "Edit.cshtml");
            bool indexViewExists = File.Exists(indexPath);

            Assert.IsTrue(indexViewExists, "Edit.cshtml view file does not exist.");
        }

        [Test]
        public void Week3_Day5_DeleteMethod_PlayerController_Delete_Player_from_DB()
        {
            var controllerType = typeof(PlayerController);
            var controller = Activator.CreateInstance(controllerType, _context);
            var player = new Player
            {
                Name = "John Doe",
                Category = "Bowler",
                BiddingAmount = 1000m,
            };
            _context.Players.Add(player);
            _context.SaveChanges();

            var playerToDelete = _context.Players.FirstOrDefault(p => p.Name == "John Doe");

            // Invoke the DeleteConfirmed action to delete the player
            var methodInfo = controller.GetType().GetMethod("DeleteConfirmed", new[] { typeof(int) });
            var result = methodInfo.Invoke(controller, new object[] { playerToDelete.Id });

            // Ensure that the player has been removed from the database
            var deletedPlayer = _context.Players.FirstOrDefault(p => p.Id == playerToDelete.Id);

            Assert.IsNull(deletedPlayer);
        }

        //[Test]
        //public void Week3_Day5_EditMethod_PlayerController_Edit_Player_in_DB()
        //{
        //    var controllerType = typeof(PlayerController);
        //    var controller = Activator.CreateInstance(controllerType, _context);
        //    var player = new Player
        //    {
        //        Name = "John Doe",
        //        Category = "Bowler",
        //        BiddingAmount = 1000m,
        //    };
        //    _context.Players.Add(player);
        //    _context.SaveChanges();

        //    var playerToEdit = _context.Players.FirstOrDefault(p => p.Name == "John Doe");

        //    // Create a new player with updated details
        //    var updatedPlayer = new Player
        //    {
        //        //Id = playerToEdit.Id, // Make sure to set the player's ID
        //        Name = "Updated Name",
        //        Category = "Updated Category",
        //        BiddingAmount = 2000m,
        //    };

        //    // Invoke the Edit action to update the player
        //    var methodInfo = controller.GetType().GetMethod("Edit", new[] { typeof(int), typeof(Player) });
        //    var result = methodInfo.Invoke(controller, new object[] { playerToEdit.Id, updatedPlayer });

        //    // Ensure that the player has been updated in the database
        //    var editedPlayer = _context.Players.FirstOrDefault(p => p.Id == playerToEdit.Id);

        //    Assert.AreEqual("Updated Name", editedPlayer.Name);
        //    Assert.AreEqual("Updated Category", editedPlayer.Category);
        //    Assert.AreEqual(2000m, editedPlayer.BiddingAmount);
        //}















        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }

        private void VerifyRouteForAction<TController>(string actionName, string expectedRouteTemplate)
            where TController : Controller
        {
            var controllerType = typeof(TController);
            var methodInfo = controllerType.GetMethod(actionName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

            //if (methodInfo == null)
            //{
            //    Assert.Inconclusive($"Action '{actionName}' not found in controller '{controllerType.Name}'");
            //    return;
            //}

            var routeAttributes = methodInfo.GetCustomAttributes<RouteAttribute>(inherit: false).ToArray();

            Assert.AreEqual(1, routeAttributes.Length);
            var routeTemplate = routeAttributes[0].Template;
            var controllerName = controllerType.Name.Replace("Controller", "");
            Console.WriteLine($"Controller: {controllerName}, Action: {actionName}, Route: {routeTemplate}");

            Assert.AreEqual(expectedRouteTemplate, routeTemplate);
        }



    }

}