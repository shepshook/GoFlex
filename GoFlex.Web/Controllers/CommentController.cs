using System;
using System.Threading.Tasks;
using GoFlex.Core;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> PostComment(Comment comment)
        {
            comment.UserId = Guid.Parse(User.FindFirst("userId").Value);

            await _unitOfWork.CommentRepository.InsertAsync(comment);

            return Ok();
        }
    }
}
