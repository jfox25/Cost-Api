using System.Collections.Generic;

namespace Api.Dto
{
    public class UserDto
    {
        public string Username { get; set; }
        public string NickName { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}