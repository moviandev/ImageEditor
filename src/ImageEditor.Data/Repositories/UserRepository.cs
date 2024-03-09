using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageEditor.Business.Interfaces;
using ImageEditor.Business.Models;
using ImageEditor.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImageEditor.Data.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ImageEditorContext db) : base(db)
    {
    }

    public async Task<IEnumerable<Image>> GetAllImagesAsync(Guid id)
    {
        var user = await DbSet
            .Include(u => u.Images)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user.Images;
    }
}
