using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebServer.Areas.Identity.Data;
using WebServer.Models;

namespace WebServer.Data;

public class ChatDbContext : IdentityDbContext<AppUser>
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    // 聊天相关的 DbSet
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RoomMember> RoomMembers { get; set; }
    public DbSet<UserRoomReadStatus> UserRoomReadStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // 配置 Room 实体
        builder.Entity<Room>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Description).HasMaxLength(500);
            
            // Room 与 Creator 的关系
            entity.HasOne(r => r.Creator)
                  .WithMany()
                  .HasForeignKey(r => r.CreatorId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
        
        // 配置 Message 实体
        builder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Content).IsRequired().HasMaxLength(1000);
            
            // Message 与 Sender 的关系
            entity.HasOne(m => m.Sender)
                  .WithMany()
                  .HasForeignKey(m => m.SenderId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // Message 与 Room 的关系
            entity.HasOne(m => m.Room)
                  .WithMany(r => r.Messages)
                  .HasForeignKey(m => m.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 RoomMember 实体
        builder.Entity<RoomMember>(entity =>
        {
            entity.HasKey(rm => rm.Id);
            
            // 确保用户在同一房间中只有一个活跃记录
            entity.HasIndex(rm => new { rm.RoomId, rm.UserId, rm.IsActive })
                  .IsUnique()
                  .HasFilter("[IsActive] = 1");
            
            // RoomMember 与 Room 的关系
            entity.HasOne(rm => rm.Room)
                  .WithMany(r => r.Members)
                  .HasForeignKey(rm => rm.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // RoomMember 与 User 的关系
            entity.HasOne(rm => rm.User)
                  .WithMany()
                  .HasForeignKey(rm => rm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // 配置 UserRoomReadStatus 实体
        builder.Entity<UserRoomReadStatus>(entity =>
        {
            entity.HasKey(urrs => urrs.Id);
            
            // 确保用户在同一房间只有一个阅读状态记录
            entity.HasIndex(urrs => new { urrs.UserId, urrs.RoomId })
                  .IsUnique();
            
            // UserRoomReadStatus 与 User 的关系
            entity.HasOne(urrs => urrs.User)
                  .WithMany()
                  .HasForeignKey(urrs => urrs.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // UserRoomReadStatus 与 Room 的关系
            entity.HasOne(urrs => urrs.Room)
                  .WithMany()
                  .HasForeignKey(urrs => urrs.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}