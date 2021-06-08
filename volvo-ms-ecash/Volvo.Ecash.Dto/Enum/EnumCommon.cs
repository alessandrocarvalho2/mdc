using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Enum
{
    public static class EnumCommon
    {
        public enum SystemMessageTypeEnum
        {
            Success,
            Info,
            Error,
        };

        public enum DatabaseTypeEnum
        {
            SqlServer = 1,
            PostgreSql = 2,
            Oracle = 3,
        };

        public enum ApprovalCategory
        {
            PreAprovado = 0,
            AAprovar = 1,
            Aprovado = 2
        };

    }
}
