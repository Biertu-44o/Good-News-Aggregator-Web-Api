namespace Web_Api_Controllers.RequestModels
{
    public class PutSettingsRequest
    {

        public string Name { get; set; }

        public string Theme { get; set; }

        public int PositiveRateFilter { get; set; }
    }
}
