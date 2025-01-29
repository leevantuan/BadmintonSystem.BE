﻿namespace BadmintonSystem.Infrastructure.DependencyInjection.Options;

public class RedisOption
{
    public bool Enabled { get; set; }

    public string ConnectionString { get; set; } = string.Empty;

    public string InstanceName { get; set; } = "BMTSYS_";
}
