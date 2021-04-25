using System;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Views.Event.Components.Comments
{
    public class CommentsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId, Guid? parentId)
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync();
            if (parentId.HasValue)
            {
                var children = comments.Where(c => c.ParentId == parentId).ToList();
                return View("Default", children);
            }
            
            var rootComments = comments.Where(c => c.EventId == eventId && !c.ParentId.HasValue).ToList();
            return View("Default", rootComments);
        }
    }
}