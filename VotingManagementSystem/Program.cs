using Microsoft.EntityFrameworkCore;
using VotingManagementSystem.Data;
using VotingManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure SQLite and register VotingContext
var sqlitePath = builder.Configuration.GetValue<string>("VotingSqlite") ?? "VotingMS.db";
var connectionString = $"Data Source={sqlitePath}";
builder.Services.AddDbContext<VotingContext>(options => options.UseSqlite(connectionString));

// Register voting service (EF Core backed)
builder.Services.AddScoped<IVotingService, VotingService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Allow frontend served from wwwroot or other origins during development.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VotingContext>();
    db.Database.EnsureCreated();

    // Seed admin user
    if (!db.Users.Any(u => u.Role == "Admin"))
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin123"));
        var hash = string.Concat(bytes.Select(b => b.ToString("x2")));
        var admin = new VotingManagementSystem.Models.User
        {
            Username = "admin",
            Email = "admin@votems.com",
            PasswordHash = hash,
            Role = "Admin",
            Status = "Active"
        };
        db.Users.Add(admin);
        db.SaveChanges();
    }

    // Seed elections if empty
    if (!db.Elections.Any())
    {
        db.Elections.AddRange(new[] {
            new VotingManagementSystem.Models.Election { Title = "Student Council President 2025", Description = "Annual election for student council president", StartDate = DateTime.UtcNow.AddDays(-10), EndDate = DateTime.UtcNow.AddDays(10), Status = "Open" }
        });
        db.SaveChanges();
    }

    // Seed positions if empty
    if (!db.Positions.Any())
    {
        db.Positions.AddRange(new[] {
            new VotingManagementSystem.Models.Position { Name = "President", Description = "Student Council President" },
            new VotingManagementSystem.Models.Position { Name = "Vice President", Description = "Student Council Vice President" },
            new VotingManagementSystem.Models.Position { Name = "Secretary", Description = "Student Council Secretary" }
        });
        db.SaveChanges();
    }

    // Seed candidates if empty
    if (!db.Candidates.Any())
    {
        var electionId = db.Elections.First().Id;
        db.Candidates.AddRange(new[] {
            new VotingManagementSystem.Models.Candidate { Name = "Tech Innovation Club", Party = "Technology & Engineering", Bio = "Organized workshops and hackathons.", ElectionId = electionId, Status = "Approved" },
            new VotingManagementSystem.Models.Candidate { Name = "Drama & Arts Society", Party = "Arts & Culture", Bio = "Produced full-scale productions.", ElectionId = electionId, Status = "Approved" },
            new VotingManagementSystem.Models.Candidate { Name = "Environmental Green Club", Party = "Sustainability", Bio = "Led recycling and tree planting.", ElectionId = electionId, Status = "Approved" }
        });
        db.SaveChanges();
    }
}

// Serve files from wwwroot (your frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
