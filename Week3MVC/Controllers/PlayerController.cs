//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc;
//using Week3MVC.Models;

//namespace Week3MVC.Controllers
//{
//    public class PlayerController : Controller
//    {
//        // Static list of players (replace with database or data source)
//        private static List<Player> players = new List<Player>
//        {
//            new Player { Id = 1, Name = "Player 1", Category = "Category A", BiddingAmount = 100 },
//            new Player { Id = 2, Name = "Player 2", Category = "Category B", BiddingAmount = 150 },
//            new Player { Id = 3, Name = "Player 3", Category = "Category A", BiddingAmount = 120 },
//        };

//        // GET: Player
//        public IActionResult Index()
//        {
//            return View(players);
//        }

//        // GET: Player/Details/1
//        public IActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var player = players.FirstOrDefault(p => p.Id == id);
//            if (player == null)
//            {
//                return NotFound();
//            }

//            return View(player);
//        }

//        // GET: Player/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Player/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(Player player)
//        {
//            if (ModelState.IsValid)
//            {
//                // Generate a new unique ID (replace with database-generated ID)
//                int newId = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
//                player.Id = newId;

//                players.Add(player);

//                return RedirectToAction(nameof(Index));
//            }
//            return View(player);
//        }

//        // GET: Player/Edit/1
//        public IActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var player = players.FirstOrDefault(p => p.Id == id);
//            if (player == null)
//            {
//                return NotFound();
//            }

//            return View(player);
//        }

//        // POST: Player/Edit/1
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(int id, Player player)
//        {
//            if (id != player.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                var existingPlayer = players.FirstOrDefault(p => p.Id == id);
//                if (existingPlayer != null)
//                {
//                    existingPlayer.Name = player.Name;
//                    existingPlayer.Category = player.Category;
//                    existingPlayer.BiddingAmount = player.BiddingAmount;

//                    return RedirectToAction(nameof(Index));
//                }
//                else
//                {
//                    return NotFound();
//                }
//            }
//            return View(player);
//        }

//        // GET: Player/Delete/1
//        public IActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var player = players.FirstOrDefault(p => p.Id == id);
//            if (player == null)
//            {
//                return NotFound();
//            }

//            return View(player);
//        }

//        // POST: Player/Delete/1
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            Console.WriteLine("hai");
//            var player = players.FirstOrDefault(p => p.Id == id);
//            if (player != null)
//            {
//                players.Remove(player);
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week3MVC.Models;

namespace Week3MVC.Controllers
{
    [Route("players")]

    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Player
        [HttpGet]
        [Route("")] // Route for the Index action
        public IActionResult Index()
        {
            var players = _context.Players.ToList();
            return View(players);
        }

        // GET: Player/Details/1
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Player/Create
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Player/Create
        [HttpPost]
        [Route("create")]

        [ValidateAntiForgeryToken]
        public IActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Player/Edit/1
        [Route("edit/{id?}")] // Route for the Edit action

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Player/Edit/1
        [HttpPost]
        [Route("edit/{id?}")] // Route for the Edit action

        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Player/Delete/1
        [HttpGet]
        [Route("delete/{id?}")] // Route for the Delete action
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Player/Delete/1
        [HttpPost, ActionName("Delete")]
        [Route("delete/{id?}")] // Route for the Delete action
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var player = _context.Players.FirstOrDefault(p => p.Id == id);
            if (player != null)
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(p => p.Id == id);
        }
    }
}

