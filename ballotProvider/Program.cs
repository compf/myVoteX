using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;;
var builder = WebApplication.CreateBuilder(args);

var certPem = File.ReadAllText("../pki/ballot_provider.cert.pem");
var keyPem = File.ReadAllText("../pki/ballot_provider.key.pem");
var x509 = X509Certificate2.CreateFromPem(certPem, keyPem);

// Add services to the container.
builder.Services.AddControllersWithViews();
var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
{
    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
    ClientCertificateMode = ClientCertificateMode.AllowCertificate,
    ServerCertificate = x509

};
builder.WebHost.ConfigureKestrel(options =>
    options.ConfigureEndpointDefaults(listenOptions =>
        listenOptions.UseHttps(httpsConnectionAdapterOptions)));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
