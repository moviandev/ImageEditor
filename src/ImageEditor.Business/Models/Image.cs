namespace ImageEditor.Business.Models;
public class Image : Entity
{
    protected Image(string s3OriginalImage, string s3EditedImage, Guid userId)
    {
        S3OriginalImage = s3OriginalImage;
        S3EditedImage = s3EditedImage;
        UserId = userId;
    }

    public string S3OriginalImage { get; protected set; }
    public string S3EditedImage { get; protected set; }
    public Guid UserId { get; protected set; }

    // Relationship with user
    public virtual User User { get; protected set; }

    public static class Builder
    {
        public static Image Create(string s3OriginalImage, string s3EditedImage, Guid userId)
            => new(s3OriginalImage, s3EditedImage, userId);
    }
}
