using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract
{
    public class GridColumnAttributes : Attribute
    {
        public string DisplayName { get; set; }
        public bool IsVisible { get; set; }
        public ColumnType ColumnType { get; set; }
        public int SequenceNumber { get; set; }
        public bool IsReadonly { get; set; }
        public string ReferenceTableName { get; set; }
        public Type Type { get; set; }
        public string MethodName { get; set; }

        public bool IsHistoryView { get; set; }

        public bool MandatorySymbolRequired { get; set; }
        public int MaxLength { get; set; }

        public string SecureObjectKey { get; set; }
        public bool IsSortable { get; set; } //TFS:290860 - propoerty added for sorting 'isSortable'

        public bool AllowNegative { get; set; } //Defect 294615 Allow negative values for Currency fields

        public GridColumnAttributes(string displayName, ColumnType columnType, int sequenceNumber = 0, bool isVislble = false, bool isReadonly = false,
            string referenceTableName = "", Type type = null, string methodName = "", bool isHistoryView = false, bool mandatorySymbolRequired = false, int maxLength = 0, string secureObjectKey = "", bool isSortable = false, bool allowNegative = false)
        //TFS:290860 - parameter and propoerty added for sorting 'isSortable', 
        {
            SequenceNumber = sequenceNumber;
            DisplayName = displayName;
            IsVisible = isVislble;
            ColumnType = columnType;
            IsReadonly = isReadonly;
            ReferenceTableName = referenceTableName;
            Type = type;
            MethodName = methodName;
            IsHistoryView = isHistoryView;
            MandatorySymbolRequired = mandatorySymbolRequired;
            MaxLength = maxLength;
            SecureObjectKey = secureObjectKey;
            IsSortable = isSortable;
            AllowNegative = allowNegative;
        }
    }

    public class GridColumnMetaData
    {
        public string DisplayName { get; set; }
        public bool IsVisible { get; set; }
        public ColumnType ColumnType { get; set; }
        public int SequenceNumber { get; set; }
        public bool IsReadonly { get; set; }
        public string PropertyName { get; set; }
        public string ReferenceTableName { get; set; }
        public Type Type { get; set; }
        public string MethodName { get; set; }
        public bool IsHistoryView { get; set; }
        public ColumnDataType ColumnDataType { get; set; }
        public string SecureObjectKey { get; set; }
        public bool MandatorySymbolRequired { get; set; }
        public int MaxLength { get; set; }
        public bool IsSortable { get; set; }  //TFS:290860 - propoerty added for sorting 'isSortable'

        public bool AllowNegative { get; set; } //Defect 294615 Allow negative values for Currency fields
    }

    public enum ColumnType
    {
        TextBox,
        DatePicker,
        DropDown,
        CheckBox,
        Label,
        ShortDateString,
        CheckBoxList,
        Button,
        None,
        CurrencyTextBox,
        HyperLink,
        ExpandIcon,
        TextArea

    }

    public enum ColumnDataType
    {
        Integer,
        DateTime,
        Bool,
        String,
        None
    }
}
