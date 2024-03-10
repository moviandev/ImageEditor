using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageEditor.Business.Interfaces;
using ImageEditor.Business.Models;
using ImageEditor.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ImageEditor.Data.Repositories;
public class ImageRepository : Repository<Image>, IImageRepository
{
  public ImageRepository(ImageEditorContext db) : base(db)
  {
  }
}
