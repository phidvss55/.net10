using webapi.Commons;
using webapi.Models;

namespace webapi.Contracts;

public interface ICommentRepository
{
    Task<Result<List<Comment>>> GetAllAsync(CommentQueryObject queryObject, CancellationToken ct = default);
    Task<Result<Comment>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<Comment>> CreateAsync(Comment commentModel, CancellationToken ct = default);
    Task<Result<Comment>> UpdateAsync(int id, Comment commentModel, CancellationToken ct = default);
    Task<Result<Comment>> DeleteAsync(int id, CancellationToken ct = default);
}
