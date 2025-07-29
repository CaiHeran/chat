using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Services;
using WebServer.Areas.Identity.Data;
using WebServer.Models;

var builder = WebApplication.CreateBuilder(args);

// 配置内存数据库
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseInMemoryDatabase("ChatDatabase"));

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
    
    // 创建测试用户
    var testUser = new AppUser
    {
        UserName = "testuser@example.com",
        Email = "testuser@example.com",
        EmailConfirmed = true
    };
    
    var result = await userManager.CreateAsync(testUser, "Test123!");
    if (result.Succeeded)
    {
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
        
        Console.WriteLine("Seed data created successfully!");
        Console.WriteLine($"Test user: {testUser.Email} / Test123!");
        Console.WriteLine($"Room 1 ID: {room1.Id}");
        Console.WriteLine($"Room 2 ID: {room2.Id}");
    }
}
