namespace FeedbackAPI.Helpers
{
    //Holds all the parameters possible when submitting a Get for Feedback Entries
    public class FeedbackViewResourceParameters
    {
        const int _maxPageSize = 20;

        int _pageSize = 3;

        public int? FeedbackScore { get; set; }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > _maxPageSize ? _maxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;
    }
}
