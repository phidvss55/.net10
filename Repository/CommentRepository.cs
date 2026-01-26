using Microsoft.EntityFrameworkCore;
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
    
    public async Task<List<Models.Comment>> GetAllAsync()
    {
        return await  _context.Comments.ToListAsync();
    }
    
    public async Task<Models.Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Models.Comment> CreateAsync(Models.Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }
    
    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await _context.Comments.FindAsync(id);
        if (existingComment == null)
        {
            return null;
        }

        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;
        existingComment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        return existingComment;
    }
    
    public async Task<Comment?> DeleteAsync(int id)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

        if (commentModel == null)
        {
            return null;
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }
}