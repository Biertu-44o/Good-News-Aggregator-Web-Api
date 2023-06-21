using MediatR;

namespace Data.CQS.Commands
{
    public class RemoveArticleCommand : IRequest
    {
        public Int32 Id { get; set; }

    }
}
