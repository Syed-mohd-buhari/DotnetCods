using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models
{
    public class UserInfoModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public List<int> RoleId { get; set; }
        public List<string> RolesName { get; set; }
        public bool IsExist { get; set; }

        public string ErrorMessage { get; set; }
    }
}
