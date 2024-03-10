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
    public virtual List<Image> Images { get; protected set; }

    public static class Builder
    {
        public static User Create(string email)
            => new(email);
    }
}
