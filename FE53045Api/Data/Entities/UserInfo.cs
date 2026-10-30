using System;
using System.Collections.Generic;

namespace AcademyManagementApi.Data.Entities;

public partial class UserInfo
{
    public string UserName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
