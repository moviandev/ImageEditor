namespace ImageEditor.Business.Models;
public class User : Entity
{
    protected User(string name, string email)
    {
        Name = name;
        Email = email;
        Images = new List<Image>();
    }

    public User()
    {
    }

    public string Name { get; protected set; }
    public string Email { get; protected set; }

    // Relationship with images
    public virtual List<Image> Images { get; protected set; }

    public static class Builder
    {
        public static User Create(string name, string email)
            => new(name, email);
    }
}
