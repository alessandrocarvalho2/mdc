using System;

namespace Volvo.Ecash.Domain.Entities {
    public class BaseEntity {
        public int Id { get;  set; }

        private DateTime? _createAt;

        public DateTime? CreateAt {
            get { return _createAt; }
            set { _createAt = (value == null ? DateTime.Now : value); }
        }

        public DateTime? UpdateAt { get; set; }

    } 
}