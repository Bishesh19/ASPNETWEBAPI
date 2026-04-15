using Microsoft.EntityFrameworkCore; 
using ProductRecordSystem.Data; 
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args); 
// Add DbContext with PostgreSQL connection 
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")) 
); 
// Add Controllers 
builder.Services.AddControllers(); 
// Optional: Swagger for testing 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit           = true;
    options.Password.RequireLowercase       = true;
    options.Password.RequireUppercase       = true;
    options.Password.RequiredLength         = 6;
    options.Password.RequireNonAlphanumeric = false;
});

var app = builder.Build(); 

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Customer", "Vendor" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
// Use Swagger 
if (app.Environment.IsDevelopment()) 
{ 
app.UseSwagger(); 
app.UseSwaggerUI(); 
} 
app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();
app.Run();