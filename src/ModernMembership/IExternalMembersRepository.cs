namespace ModernMembership
{
    public interface IExternalMembersRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateExternalIdException"></exception>
        /// <param name="member"></param>
        void Add(ExternalMember member);

        ExternalMember GetMember(ExternalMemberId id);
    }
}