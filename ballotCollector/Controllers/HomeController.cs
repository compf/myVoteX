using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using voteServer.Models;
using Model;

namespace voteServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index()
    {

        return null;
    }
    [HttpOptions]

    public void SubmitVote()
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "https://ballotprovider.compf.me:1997");
        Response.Headers.Add("Access-Control-Allow-Methods", "POST");
        Response.Headers.Add("Access-Control-Allow-Headers", "*");
    }

    [HttpPost]

    public string SubmitVote([FromBody] EncryptedBallot encrypted)
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "https://ballotprovider.compf.me:1997");
        Response.Headers.Add("Access-Control-Allow-Methods", "POST");
        Response.Headers.Add("Access-Control-Allow-Headers", "*");

        if (!ModelState.IsValid)
        {

            Console.WriteLine("Model state is invalid ");

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

        }

        Console.WriteLine("Received encrypted ballot: " + encrypted.EncryptedBallotData);
        Console.WriteLine("Received encrypted key: " + encrypted.EncryptedKey);
        Console.WriteLine("Received ivs: " + encrypted.Ivs);
        Console.WriteLine("Received public key: " + encrypted.PublicKey);
        Console.WriteLine("Received return address: " + encrypted.ReturnAddress);
        return "success";

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
