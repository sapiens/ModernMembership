namespace ModernMembership.Events
{
    public static class Extensions
    {
         public static LocalMemberAdded CreateNewMemberEvent(this LocalMember m)
         {
             var evnt = new LocalMemberAdded(m.Id)
                 {
                     DisplayName = m.DisplayName,
                     LoginId = m.LoginId.Value,
                     Email = m.Email.Value,
                     Status = m.Status
                 };
             return evnt;
         }
    }
}