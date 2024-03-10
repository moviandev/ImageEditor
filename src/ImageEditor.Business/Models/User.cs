namespace ImageEditor.Business.Models;
public class User : Entity
{
    protected User(string email)
    {
        Email = email;
        Images = new List<Image>();
    }

    public User()
    {
    }

    public string Email { get; protected set; }

    // Relationship with images
    public List<Image> Images { get; set; }

    public void AddImages(params Image[] images)
    {
        if (images is not null)
            Images.AddRange(images);
        else
            Images = new List<Image>(images);
    }

    public static class Builder
    {
        public static User Create(string email)
            => new(email);
    }
}
