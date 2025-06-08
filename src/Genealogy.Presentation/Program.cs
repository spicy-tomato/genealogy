using FirebaseAdmin;
using Genealogy.Infrastructure.Postgres;
using Google.Apis.Auth.OAuth2;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddPostgres();

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("keys/firebase-admin-sdk.json"),
});

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();