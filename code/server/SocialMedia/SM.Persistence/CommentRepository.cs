using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SM.Persistence
{
    public class CommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsWithReplies(int? parentId = null)
        {
            return await _context.Comments
                .Where(c => c.ParentId == parentId)
                .Include(c => c.Replies)
                .ToListAsync();
        }
    }

}
