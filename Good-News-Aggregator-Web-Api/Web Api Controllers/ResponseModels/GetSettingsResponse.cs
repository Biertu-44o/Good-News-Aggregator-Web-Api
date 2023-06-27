namespace Web_Api_Controllers.ResponseModels
{
    public class UserSettingsResponse
    {
        public string Name { get; set;}

        public string Theme { get; set; }

        public List<String> AllThemes { get; set; }

        public int PositiveRate { get; set; }

        public int PositiveRateFilter { get; set; }

        public byte[]? ProfilePicture { get; set; }
    }
}
