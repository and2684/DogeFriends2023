namespace SettingsService.Dto
{
    public class SetSettingDto
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string EncryptionKey { get; set; } = string.Empty;

    }
}
