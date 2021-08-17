namespace OperaHouseTheater.Test.Services
{
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Services.Members;
    using OperaHouseTheater.Test.Mocks;
    using Xunit;
    using System.Linq;

    using static OperaHouseTheater.Data.DataConstants;

    public class MemberServiceTest
    {
        const string testUserId = "TestUserId";
        const string testMemberName = "memberName";
        const string testPhoneNumber = "0888888888";


        [Fact]
        public void UserIsMemberShouldReturnTrueWhenUserIsMember()
        {
            const string testUserId = "TestUserId";

            using var data = DatabaseMock.Instance;

            data.Members.Add(new Member { UserId = testUserId });
            data.SaveChanges();

            var memberService = new MemberService(data);

            var result = memberService.UserIsMember(testUserId);

            Assert.True(result);
        }

        [Fact]
        public void UserIsMemberShouldReturnFalseWhenUserIsNotMember()
        {
            using var data = DatabaseMock.Instance;

            data.Members.Add(new Member { UserId = testUserId });
            data.SaveChanges();

            var memberService = new MemberService(data);

            var result = memberService.UserIsMember("AnotherUserId");

            Assert.False(result);
        }

        [Fact]
        public void BecameMemberShouldAddMember()
        {
            using var data = DatabaseMock.Instance;

            var memberService = new MemberService(data);

            memberService.BecameMember(testMemberName, testPhoneNumber, testUserId);

            var memberIsAdded = data.Members
                .Any(m => m.MemberName == testMemberName &&
                            m.PhoneNumber == testPhoneNumber &&
                            m.UserId == testUserId);

            Assert.True(memberIsAdded);
        }

        [Fact]
        public void GetMemberIdShouldReturnCorrectMemberIdIfExists() 
        {
            using var data = DatabaseMock.Instance;

            var memberService = new MemberService(data);

            memberService.BecameMember(testMemberName, testPhoneNumber, testUserId);

            var memberId = memberService.GetMemberId(testUserId);

            Assert.Equal(1, memberId);
        }

        [Fact]
        public void GetMemberIdShouldReturnZeroIfMemberNotExists()
        {
            using var data = DatabaseMock.Instance;

            var memberService = new MemberService(data);

            var memberId = memberService.GetMemberId(testUserId);

            Assert.Equal(0, memberId);
        }
    }
}
