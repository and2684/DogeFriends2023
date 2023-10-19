using Microsoft.AspNetCore.Mvc;

namespace SettingsService.Dto
{
    public class GetSettingDto
    {
        [FromHeader]
        public string Key { get; set; } = string.Empty;
        [FromHeader]
        public string EncryptionKey { get; set; } = string.Empty;
    }
}
