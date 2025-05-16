using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>
opt.UseSqlServer(builder.Configuration.GetConnectionString("default"))
    
    
    
    );

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    "Admin",
    "{area:exists}/{controller=home}/{action=index}/{id?}"

 );


app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}/{id?}"

 );



app.Run();
