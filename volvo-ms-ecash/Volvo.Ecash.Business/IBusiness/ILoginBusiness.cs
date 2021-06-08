using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Business.IBusiness
{
    public interface ILoginBusiness
    {
        dynamic Find(string userID, string accessKey);
    }
}
