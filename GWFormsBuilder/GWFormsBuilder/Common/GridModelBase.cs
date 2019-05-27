
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contract;

namespace Common.Contract
{
    public class GridModelBase
    {
        ////Propeties for Dyna Grid

        /// <summary>
        ///     Gets or sets the create date.
        /// </summary>
        /// <value>
        ///     The create date.
        /// </value>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Gets or sets the create user id.
        /// </summary>
        /// <value>
        ///     The create user id.
        /// </value>
        public string CreateUserId { get; set; }

        /// <summary>
        ///     Gets or sets the action.
        /// </summary>
        /// <value>
        ///     The action.
        /// </value>
        [GridColumnAttributes("Remove", ColumnType.Label, 1, false, true, "", null, "", true)]
        public string Action { get; set; }

        /// <summary>
        ///     Gets or sets the update date.
        /// </summary>
        /// <value>
        ///     The update date.
        /// </value>
        [GridColumnAttributes("Action Date", ColumnType.ShortDateString, 2, false, true, "", null, "", true)]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        ///     Gets or sets the update user id.
        /// </summary>
        /// <value>
        ///     The update user id.
        /// </value>
        [GridColumnAttributes("Action By", ColumnType.Label, 3, false, true, "", null, "", true)]
        public string UpdateUserId { get; set; }

        [GridColumnAttributes("", ColumnType.None)]
        public bool IsDeleted { get; set; }

        [GridColumnAttributes("", ColumnType.None)]
        public bool IsExisting
        {
            get { return true; }
        }


        /// <summary>
        ///     Gets or sets the verification value id.
        /// </summary>
        /// <value>
        ///     The verification value.
        /// </value>

        [GridColumnAttributes("", ColumnType.None)]
        public string OriginalValueDictionary { get; set; }
    }
}
