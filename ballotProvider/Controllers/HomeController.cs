using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using voteServer.Models;
using Model;

namespace voteServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public object data;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        Ballot ballot=new Ballot();
        var erstStimme=new BallotGroup();
        erstStimme.Votes.Add("CDU",new BooleanVote());
        erstStimme.Votes.Add("SPD",new BooleanVote());
        erstStimme.Votes.Add("Grüne",new BooleanVote());

        var zweitStimme=new BallotGroup();
        zweitStimme.Votes.Add("CDU",new BooleanVote());
        zweitStimme.Votes.Add("SPD",new BooleanVote());
        zweitStimme.Votes.Add("Grüne",new BooleanVote());
        ballot.Groups.Add("Erststimme",erstStimme);
        ballot.Groups.Add("Zweitstimme",zweitStimme);

        var envelope=new SimpleBallot();
        envelope.Ballot=ballot;
        envelope.BallotId="1234";
        envelope.PublicKey=GetPublicKey();
        envelope.ReturnAddress="https://ballotcollector.compf.me:1998/Home/SubmitVote";
        data=envelope;
    }

    public string GetPublicKey()
    {
        System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();
        var publicKey= rsa.ExportSubjectPublicKeyInfo();
        string key= Convert.ToBase64String(publicKey);
        Console.WriteLine("Public key: "+key);
        return key;
    }

    public IActionResult Index()
    {
        return View(data);
    }
    [HttpPost]

    public void SubmitVote([FromBody]EncryptedBallot encrypted)
    {
        if (!ModelState.IsValid) { 

                Console.WriteLine("Model state is invalid " );

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

         }
       
      Console.WriteLine("Received encrypted ballot: "+encrypted.EncryptedBallotData);
      Console.WriteLine("Received encrypted key: "+encrypted.EncryptedKey);
      Console.WriteLine("Received ivs: "+encrypted.Ivs);
        Console.WriteLine("Received public key: "+encrypted.PublicKey);
        Console.WriteLine("Received return address: "+encrypted.ReturnAddress);

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
