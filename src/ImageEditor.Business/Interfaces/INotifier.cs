using System.Collections.Generic;
using ImageEditor.Business.Notifications;

namespace ImageEditor.Business.Interfaces;
public interface INotifier
{
    public bool HasNotifications();
    public IEnumerable<Notification> GetNotifications();
    public void Handle(Notification notification);
}