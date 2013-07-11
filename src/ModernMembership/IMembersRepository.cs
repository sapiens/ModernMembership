namespace ModernMembership
{
    public interface IMembersRepository
    {
        /// <summary>
        ///  </summary>
        /// <exception cref="DuplicateMemberIdException"></exception>
        /// <exception cref="DuplicateLoginIdException"></exception>
        /// <exception cref="DuplicateEmailException"></exception>
        /// <param name="member"></param>
        void Add(LocalMember member);
    }
}