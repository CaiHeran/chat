using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Services;
using WebServer.Areas.Identity.Data;
using WebServer.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// 配置内存数据库
builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseInMemoryDatabase("ChatDatabase");
    
    // 在开发环境启用敏感数据日志记录
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        // 记录数据库操作到控制台
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});

// 配置身份验证
builder.Services.AddDefaultIdentity<WebServer.Areas.Identity.Data.AppUser>(options => 
{
    // 开发阶段简化验证要求
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ChatDbContext>();

// 配置认证 Cookie 选项
builder.Services.ConfigureApplicationCookie(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // 开发环境：更短的过期时间
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
    }
    
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// 添加服务
builder.Services.AddControllersWithViews(); 
builder.Services.AddSignalR();

// 注册聊天服务
builder.Services.AddScoped<ChatService>();

var app = builder.Build();

// 确保数据库创建并添加种子数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    
    context.Database.EnsureCreated();
    
    // 添加种子数据（仅在开发环境）
    if (app.Environment.IsDevelopment())
    {
        await SeedDataAsync(context, userManager);
    }
}

// 添加定期数据库状态日志（修复后的版本）
if (app.Environment.IsDevelopment())
{
    _ = Task.Run(async () =>
    {
        while (!app.Lifetime.ApplicationStopping.IsCancellationRequested)
        {
            try
            {
                // 每次都创建新的 scope 来避免已释放的 ServiceProvider 问题
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                await LogDatabaseStatusAsync(context);
                
                await Task.Delay(TimeSpan.FromMinutes(5), app.Lifetime.ApplicationStopping);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("📊 Database status logging stopped.");
                break;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("📊 Database status logging stopped due to application shutdown.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error logging database status: {ex.Message}");
                // 等待一段时间后重试，避免连续错误
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), app.Lifetime.ApplicationStopping);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 开发环境：添加用户验证中间件
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                var userExists = await dbContext.Users.AnyAsync(u => u.Id == userId);
                
                if (!userExists)
                {
                    Console.WriteLine($"⚠️ Invalidating stale authentication for user: {userId}");
                    // 清除认证状态
                    await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
            }
        }
        await next();
    });
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<WebServer.Hubs.ChatHub>("/chathub");
app.MapRazorPages();

app.Run();

// 种子数据方法
async Task SeedDataAsync(ChatDbContext context, UserManager<AppUser> userManager)
{
    // 检查是否已有数据
    if (context.Rooms.Any()) return;
    
    Console.WriteLine("🌱 Starting seed data creation...");

    // 创建测试用户
    var testUser = new AppUser {
        UserName = "1@test.com",
        Email = "1@test.com",
        EmailConfirmed = true
    };
    var testUser2 = new AppUser {
        UserName = "2@test.com",
        Email = "2@test.com",
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(testUser, "Test!234");
    await userManager.CreateAsync(testUser2, "Test!234");
    if (result.Succeeded)
    {
        Console.WriteLine($"✅ Test user created: {testUser.Email}");
        
        // 创建测试房间
        var room1 = new Room
        {
            Name = "General Chat",
            Description = "A general chat room for everyone",
            CreatorId = testUser.Id,
            CreatedAt = DateTime.UtcNow
        };
        
        var room2 = new Room
        {
            Name = "Tech Discussion",
            Description = "Discuss technology and programming",
            CreatorId = testUser.Id,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Rooms.AddRange(room1, room2);
        await context.SaveChangesAsync();
        Console.WriteLine($"✅ Rooms created: {room1.Id}, {room2.Id}");
        
        // 创建房间成员记录
        var member1 = new RoomMember
        {
            RoomId = room1.Id,
            UserId = testUser.Id,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        var member2 = new RoomMember
        {
            RoomId = room2.Id,
            UserId = testUser.Id,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        context.RoomMembers.AddRange(member1, member2);
        
        // 添加示例消息
        var message1 = new Message
        {
            RoomId = room1.Id,
            SenderId = testUser.Id,
            Content = "Welcome to the General Chat room!",
            Time = DateTime.UtcNow.AddMinutes(-10),
            Type = 0
        };
        
        var message2 = new Message
        {
            RoomId = room1.Id,
            SenderId = testUser.Id,
            Content = "Feel free to start chatting here.",
            Time = DateTime.UtcNow.AddMinutes(-5),
            Type = 0
        };
        
        context.Messages.AddRange(message1, message2);
        await context.SaveChangesAsync();
        
        Console.WriteLine("✅ Seed data created successfully!");
        Console.WriteLine($"📧 Test user 1: {testUser.Email} / Test!123");
        Console.WriteLine($"📧 Test user 1: {testUser2.Email} / Test!123");
        Console.WriteLine($"🏠 Room 1 ID: {room1.Id} - {room1.Name}");
        Console.WriteLine($"🏠 Room 2 ID: {room2.Id} - {room2.Name}");
        Console.WriteLine($"💬 Messages added: {context.Messages.Count()}");
        Console.WriteLine($"👥 Active members: {context.RoomMembers.Count(rm => rm.IsActive)}");
    }
    else
    {
        Console.WriteLine($"❌ Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
}

// 数据库状态日志方法
async Task LogDatabaseStatusAsync(ChatDbContext context)
{
    try
    {
        var stats = new
        {
            Users = await context.Users.CountAsync(),
            Rooms = await context.Rooms.CountAsync(),
            Messages = await context.Messages.CountAsync(),
            ActiveMembers = await context.RoomMembers.CountAsync(rm => rm.IsActive),
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        Console.WriteLine($"📊 [Database Status @ {stats.Timestamp}] Users: {stats.Users}, Rooms: {stats.Rooms}, Messages: {stats.Messages}, Active Members: {stats.ActiveMembers}");
        
        // 显示最近的活动
        var recentMessages = await context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Room)
            .OrderByDescending(m => m.Time)
            .Take(3)
            .ToListAsync();
            
        if (recentMessages.Any())
        {
            Console.WriteLine("💬 Recent Messages:");
            foreach (var msg in recentMessages)
            {
                Console.WriteLine($"   [{msg.Time:HH:mm:ss}] {msg.Sender?.UserName} in {msg.Room?.Name}: {msg.Content}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error getting database status: {ex.Message}");
    }
}
