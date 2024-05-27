using System.ComponentModel.DataAnnotations.Schema;
using ResourceManager.Domain.SeedWork;

namespace ResourceManager.Domain.Entities;

[Table("file")]
public class File : PersonalizedEntity
{
    public string FileName { get; set; }
    
    public string OriginalFileName { get; set; }

    public string FileExtension { get; set; }

    public long Size { get; set; }

    public Guid? DirectoryId { get; set; }
    
    #region Navigations
    
    public virtual Directory? Directory { get; set; }
    
    #endregion
}