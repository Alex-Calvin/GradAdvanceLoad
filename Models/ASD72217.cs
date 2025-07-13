using System;
using System.Collections.Generic;

#nullable disable

namespace GradAdvanceLoad.Models
{
    public partial class GALOAD
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ExternalId { get; set; }
        public string Loaded_Ind { get; set; }
    }
}
