using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Model.GrowHub.Settings;

public sealed class ManualSetting(Id<Setting> id, SettingValue value) : Setting(id)
{
    public SettingValue Value { get; } = value;
    
    public static ManualSetting New(SettingValue value) => new(new Id<Setting>(), value);
}