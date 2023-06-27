namespace Web_Api_Controllers.RequestModels
{
    public class GetArticlesRequest
    {
        /// <summary>
        /// Page number. Greater than 0.
        /// </summary>
        public Int32 Page { get; set; }
        /// <summary>
        /// Number of article items per page. Greater than 0.
        /// </summary>
        public Int32 PageSize { get; set; }
        /// <summary>
        /// Articles with a rating higher than the specified value. Greater than or equal to 0 and less than or equal to 10.
        /// </summary>
        public Int32 UserFilter { get; set; }
    }
}
