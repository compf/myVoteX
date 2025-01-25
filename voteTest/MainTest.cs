namespace voteTest;
using Model;
using NUnit.Framework;
using System.Collections;
using System.Security.Cryptography;
using System.Text.Json;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    class TestCryptoGenerator : ICryptoGenerator
    {
        public RSA CreateRSA(string publicKey)
        {
            var rsa = RSA.Create();
            string inCode = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAv0FSVWXjUGVzwqLbLo4V8m+l3gMJxQdboE2facaPfDfWJ7gK
VcT4vg8HnMPdtH0f1gPMyc8maYeym/WUNIt2zFoKf6qCwCsW9NCBHYjIqa7xQoH0
YwRY4F3ctawuxdzqZSHrolCGvFtoo7dDWTi88nLOYzOh2ZsTZUBAF+twAS0h75Ij
cbwH/IYt1olK4jxNqBzY4OkJcc4rF/oGexeoJUHaUaZvcYV7q5o2jIfE8b+riaeC
AtIvAJgbxpqkg7iuDOeBfTpSrUnPdr4HfZwUr2i59nfXYgfZmLRXha9bklx607th
qrsToyYHw2qgLyrMm3mjJdd9DSsHE/QFGWVoyQIDAQABAoIBAHvbh8B6TW3hZchk
w2Ew7xGkMFzIxujsTPBRlK1hw8aEOpJaY4cMGrKq9RkW11tttNJaf6MqHgw1rvvF
XIdy0iqhHS90c1yUCzfcV+GSlbEd9GxH0MbXJ9+VqbuVmGzXFo0MHJdvYvJUmD1+
D4WWcvboVrRz2ZsdDMCDjX5wIIcBXroQiHQoHjEqgYJJP2apoScsPhVVaqsqfMnV
7kSeyvoxX+Qi4rpwFr2dYN3DgWBmxyg9RL8yYyBEavSMiEo6S9jdK8q2WSQosiS7
iOwLtaBKGGBLnNgk5ckK79Ly0HtiK5rEkRgmMKuKNYcI5HyNPPWN7qzyHBG4/WWt
5JNO7KECgYEA5K36nbmdrIicjhmDIyDNEsnLPCsaLQzHCZ5woT3LKMY5Rpri1+Rz
HLjpy9iNxQn0vCpHT7kq/aJETEvvA8jPTdGIWSw+vRcDfREvnfXNlin0CGkuTeLe
l4aC9NbW9Ci+YEeelZhMevjf6lT6dFqHOgnGjfGRdRxKwgUv3ly9i60CgYEA1hq9
X+JD3FHUNQgggQCIVdNYOjnlAnCBXuZ3SZk9lua1tlCnWJh6mswDRCQg/n6cJK/r
3Kh0Fv5ge1q6s8MDyFIwk4aLP+u+6DFiLWiO6+OjLq841ZUz6R1xnlZ55tm1OTgc
94U+r+nFXdrsQm3Pp9UUvFOxvXY1MLC/ViCUtQ0CgYAaCKiAblJCAyd3kfX4+NH/
8pM9nVaUjGDYen4uDR/k23RH0nhCxdJJEdAkEdpJ9VE0XsfRjq4TQ/bsjLSARMs8
+76/ECdwVX9jLKK0I/iswMf0cS0BVvOqYnjkMNU15LGPuneWZklGsrCjN9tvhaLh
e51sKXU61Oa54edKNFqr3QKBgQCxeBqvBss/LWjbyG/A3mMj2PpR0Tlda0ohEqGg
FPBzS6slgvcjvcgq1Z6DdfsPWH0u/89e6RbJFHN+7DbD2IsjGdWAA08bXdqxdnWt
s/R73QWfJcjTaUhNG4XZPE7xJlZMJpIELaNHh1t8r4GUkpdEw5/bdvhmVjxCrs76
oikBZQKBgFzmL7x64v1bks8enSEND6YvGB3AKbU52i635BK8CoUMmZwMEGSpifOl
6D9Z0MCXT8kf+Ls6eeKXzjrBxEQnOv78N8zxwUdCfhil3+v4Iiq1vH0GyjYuZaj6
vbjk0rVPbTA2m6c8DG+Rtfo5njktXjR6bjitM5ZrkQ1YtR5ynkTy
-----END RSA PRIVATE KEY-----

";
            rsa.ImportFromPem(inCode);


            return rsa;
        }

    }
    private EncryptedBallot TestBallot()
    {


        return new EncryptedBallot(
            "cWasy/lce+f7XiG/",
            "Q9wYKIqdOsbFpY5y3n18wMZnlwW0y4EqiYjFTlmVqTT5WiP9ZniXO6qZYoXEUEizd9ucHvOzcssGQRVh6cAFgJFUsI1Tp4tDau5pQ6dPIzY90kcjfFCna6tnsJ1IPzu6qJbezgiAM/bjI76k+fElzO0qDwDhyN5YGg==",
            "vERhnCBaLbBCdy9ChmdLEq2RS98+TI0oeGwFn0aHxY5zhBBNtEv0VnMib8Pw0X5WZmCwOud3I6qacc3zW5xtn0AFo8absS9vU0VGcxr1/CufqJ3gHzUIfIAjbvannPhk6JYkhI0pZDqhVr2OZBOQFpXNtFM6cd02u27drVrANKSmtF+XhNOZ8YEIomxj2FyVYUNXcIRurCw2WD03DSIyBX0+GpNmm0y6ebAgt/aTtYCF5q8wfxBZ3yIfn+X7mkJsTfb0DlLPwWr5eKqQTcIX3HF+d7/xYiG7+86SxDlQczcbSoAodRUvkulZt7s0EMw87hqer5Y3AupahFr9KUWmog==",
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAv0FSVWXjUGVzwqLbLo4V8m+l3gMJxQdboE2facaPfDfWJ7gKVcT4vg8HnMPdtH0f1gPMyc8maYeym/WUNIt2zFoKf6qCwCsW9NCBHYjIqa7xQoH0YwRY4F3ctawuxdzqZSHrolCGvFtoo7dDWTi88nLOYzOh2ZsTZUBAF+twAS0h75IjcbwH/IYt1olK4jxNqBzY4OkJcc4rF/oGexeoJUHaUaZvcYV7q5o2jIfE8b+riaeCAtIvAJgbxpqkg7iuDOeBfTpSrUnPdr4HfZwUr2i59nfXYgfZmLRXha9bklx607thqrsToyYHw2qgLyrMm3mjJdd9DSsHE/QFGWVoyQIDAQAB",
            "https://ballotcollector.compf.me:1998/Home/SubmitVote",
            "1234"
        );
    }

    [Test]
    public void TestBallotDecryption()
    {
        var ballot = TestBallot();
        var decrypted = ballot.Decrypt(new TestCryptoGenerator());
        Console.WriteLine(decrypted);
        
        Assert.That(decrypted.Groups.Count, Is.EqualTo(2));

        Assert.That(decrypted.Groups.ContainsKey("Erststimme"));
        Assert.That(decrypted.Groups.ContainsKey("Zweitstimme"));

        var group1=decrypted.Groups["Erststimme"];
        Assert.That(group1.Votes.ContainsKey("SPD"));
        Assert.That(group1.Votes["SPD"].CheckValidity(null));

        var group2=decrypted.Groups["Zweitstimme"];
        Assert.That(group2.Votes.ContainsKey("SPD"));
        Assert.That(group2.Votes["SPD"].CheckValidity(null));
        
    }
}
