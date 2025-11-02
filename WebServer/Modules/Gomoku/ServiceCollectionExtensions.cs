using WebServer.Modules.Gomoku.Services;

namespace WebServer.Modules.Gomoku;

/// <summary>
/// 五子棋模块 DI 注册扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加五子棋模块到 DI 容器
    /// </summary>
    public static IServiceCollection AddGomokuModule(this IServiceCollection services)
    {
        // 注册服务
        services.AddScoped<IGomokuService, GomokuService>();
        services.AddSingleton<GomokuRuleEngine>();

        // 不要在这里重复 AddSignalR，统一在 Program.cs 配置
        return services;
    }
}
