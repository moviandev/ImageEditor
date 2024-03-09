using ImageEditor.Business.Interfaces;

namespace ImageEditor.Business.Notifications;
public class Notifier : INotifier
{
    private List<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public IEnumerable<Notification> GetNotifications()
        => _notifications;

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public bool HasNotifications()
        => _notifications.Any();
}