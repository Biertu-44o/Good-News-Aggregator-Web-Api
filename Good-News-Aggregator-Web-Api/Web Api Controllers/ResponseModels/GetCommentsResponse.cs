namespace Web_Api_Controllers.ResponseModels
{
    public class GetCommentsResponse
    {
        public Int32 Id { get; set; }
        public Int32 ArticleId { get; set; }
        public DateTime DateTime { get; set; }
        public String Text { get; set; }
        public String Name { get; set; }
        public String? ProfilePicture { get; set; }
    }
}
