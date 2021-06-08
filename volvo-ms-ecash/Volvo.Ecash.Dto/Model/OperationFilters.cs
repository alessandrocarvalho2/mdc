﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volvo.Ecash.Dto.Enum;

namespace Volvo.Ecash.Dto.Model
{
    public class OperationFilters
    {
        [Required]
        public int BankAccountId { get; set; }

        public string InOut { get; set; }

        public int? CategoryId { get; set; }
    }
}
