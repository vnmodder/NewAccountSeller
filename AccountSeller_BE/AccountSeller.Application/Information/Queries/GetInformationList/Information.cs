using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Application.Information.Queries.GetInformationList
{
    public class Information
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the total money.
        /// </summary>
        public decimal TotalMoney { get; set; }
    }
}
