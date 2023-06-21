namespace Core.DTOs.Account
{
    public class userSettingsDTO
    {
        public string Name { get; set; }
        public string Theme { get; set; }
        public List<string> AllThemes { get; set; }
        public int PositiveRate { get; set; }
        public int PositiveRateFilter { get; set; }
        public byte[]? ProfilePictureByteArray { get; set; }
    }
}