namespace OperaHouseTheater.Services.Members
{

    public interface IMemberService
    {
        public void BecameMember(string memberName,string phoneNumber,string userId);

        public bool IsMember(string userId);
    }
}
