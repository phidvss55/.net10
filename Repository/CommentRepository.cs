using Microsoft.EntityFrameworkCore;
using webapi.Commons;
using webapi.Contracts;
using webapi.Data;
using webapi.Models;

namespace webapi.Repository;

public class CommentRepository: ICommentRepository
{
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Comment>>> GetAllAsync(CommentQueryObject queryObject, CancellationToken ct = default)
    {
        var comments = _context.Comments
            .Include(a => a.AppUser)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
        {
            comments = comments.Where(s => s.Stock != null && s.Stock.Symbol == queryObject.Symbol);
        };
        if (queryObject.IsDecsending == true)
        {
            comments = comments.OrderByDescending(c => c.CreatedAt);
        }
        var result = await comments.ToListAsync(ct);
        return Result<List<Comment>>.Success(result);
    }

    public async Task<Result<Comment>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var comment = await _context.Comments
            .Include(a => a.AppUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return comment == null 
            ? Result<Comment>.Failure("Comment not found.") 
            : Result<Comment>.Success(comment);
    }
    
    public async Task<Result<Comment>> CreateAsync(Comment commentModel, CancellationToken ct = default)
    {
        await _context.Comments.AddAsync(commentModel, ct);
        await _context.SaveChangesAsync(ct);
        return Result<Comment>.Success(commentModel);
    }
    
    public async Task<Result<Comment>> UpdateAsync(int id, Comment commentModel, CancellationToken ct = default)
    {
        var existingComment = await _context.Comments.FindAsync(new object[] { id }, ct);
        if (existingComment == null)
        {
            return Result<Comment>.Failure("Comment not found.");
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;
        existingComment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        
        return Result<Comment>.Success(existingComment);
    }
    
    public async Task<Result<Comment>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (commentModel == null)
        {
            return Result<Comment>.Failure("Comment not found.");
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync(ct);
        return Result<Comment>.Success(commentModel);
    }
}