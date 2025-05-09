using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO
{
    public  class UpdateRoleDto
    {
        [Required(ErrorMessage = "Old Role is required.")]
        [DefaultValue("User")]
        public string OldRoleName { get; set; }

        [Required(ErrorMessage = "New Role is required.")]
        [DefaultValue("User")]
        public string NewRoleName { get; set; }

        public string UserId { get; set; }
    }
}
