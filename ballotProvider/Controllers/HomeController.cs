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
        SimpleBallot ballot=new SimpleBallot(
            GetPublicKey(),
            "https://ballotcollector.compf.me:1998/Home/SubmitVote",
            "1234"
        );
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


        data=ballot;
    }

    public string GetPublicKey()
    {
        System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();
        rsa.ImportFromPem(System.IO.File.ReadAllText("../ballotCounter/pki/ballotcounter.pem"));
        var publicKey= rsa.ExportSubjectPublicKeyInfo();
        string key= Convert.ToBase64String(publicKey);
        Console.WriteLine("Public key: "+key);
        return key;
    }

    public IActionResult Index()
    {
        return View(data);
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
