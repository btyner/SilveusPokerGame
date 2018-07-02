using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilveusPokerGame.Models;
using SilveusPokerGame.Services;

namespace SilveusPokerGame.Controllers
{
    [Route("api/[controller]")]
    public class PokerController : Controller
    {

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        // POST api/poker
        [HttpPost]
        public IActionResult Start([FromBody]PlayersDTO value)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }

            PlayerHandsDTO playersHands = PokerService.StartGame(value);

            return Ok(new PlayerHandsDTO(){ Player1Hand = playersHands.Player1Hand, Player2Hand = playersHands.Player2Hand });
        }

        [HttpGet("[action]")]
        public IActionResult GetWinner()
        {
            WinnerDTO winner = PokerService.GetWinner();

            return Ok(winner);
        }

    }
}
