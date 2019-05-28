using Common.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWFormsBuilder.Models
{

    public class GWControlModel
    {
        public GWControlModel()
        {
            List<GWControl> GWControlList = new List<GWControl>();
        }

        public Dictionary<string, string> InputType;

        public List<string> LevelZeroInputType;

        public List<string> LevelOneInputType;


        [UIHint("Grid")]
        public List<LevelZeroControls> LevelZeroControlGrid;

        [UIHint("Grid")]
        public List<GWControl> GWControlList;

        [UIHint("Grid")]
        public List<LevelOneGWControl> LevelOneGWControl;

        public List<string> YesNo { get; set; }
        public List<string> ValueType { get; set; }

        public string FileName { get; set; }
    }
    public class GWControl : GridModelBase
    {

        [GridColumnAttributes("Input Type", ColumnType.TextBox, 1, true, false)]
        public string ControlType { get; set; }

        [GridColumnAttributes("Is Editable", ColumnType.TextBox, 4, true, false)]
        public string IsEditable { get; set; }

        [GridColumnAttributes("ID", ColumnType.TextBox, 2, true, false)]
        public string ID { get; set; }

        [GridColumnAttributes("Label", ColumnType.TextBox, 3, true, false)]
        public string Label { get; set; }

        [GridColumnAttributes("Value", ColumnType.TextBox, 7, true, false)]
        public string Value { get; set; }

        [GridColumnAttributes("Is Visible", ColumnType.TextBox, 5, true, false)]
        public string IsVisible { get; set; }

        [GridColumnAttributes("Value Type", ColumnType.TextBox, 8, true, false)]
        public string ValueType { get; set; }

        [GridColumnAttributes("Custom Value Type", ColumnType.TextBox, 9, true, false)]
        public string CustomValueType { get; set; }

        [GridColumnAttributes("Is Required", ColumnType.TextBox, 6, true, false)]
        public string IsRequired { get; set; }

        [GridColumnAttributes("Is Parent", ColumnType.TextBox, 10, true, false)]
        public bool IsParent { get; set; }

    }

    public class LevelOneGWControl : GridModelBase
    {
        [GridColumnAttributes("Control Type", ColumnType.DropDown, 1, true, false)]
        public string LevelOneControl { get; set; }

        [GridColumnAttributes("Actions", ColumnType.TextBox, 4, true, false)]
        public string Actions { get; set; }

        [GridColumnAttributes("ID", ColumnType.TextBox, 2, true, false)]
        public string ID { get; set; }

        [GridColumnAttributes("Label", ColumnType.TextBox, 3, true, false)]
        public string Label { get; set; }

        [GridColumnAttributes("Trigger IDs", ColumnType.TextBox, 7, true, false)]
        public string TriggerIds { get; set; }

        [GridColumnAttributes("Is Visible", ColumnType.DropDown, 5, true, false)]
        public string IsVisible { get; set; }

        [GridColumnAttributes("Value Type", ColumnType.DropDown, 8, true, false)]
        public string ValueType { get; set; }

        [GridColumnAttributes("Custom Value Type", ColumnType.TextBox, 9, true, false)]
        public string CustomValueType { get; set; }

        [GridColumnAttributes("Is Required", ColumnType.DropDown, 6, true, false)]
        public string IsRequired { get; set; }

        [GridColumnAttributes("Available", ColumnType.TextBox, 10, true, false)]
        public string Available { get; set; }

        [GridColumnAttributes("Value", ColumnType.TextBox, 11, true, false)]
        public string Value { get; set; }

        [GridColumnAttributes("Bold Value", ColumnType.TextBox, 11, true, false)]
        public string BoldValue { get; set; }

        [GridColumnAttributes("On Change", ColumnType.TextBox, 12, true, false)]
        public string OnChange { get; set; }

        [GridColumnAttributes("Parent ID", ColumnType.TextBox, 12, true, false)]
        public string ParentID { get; set; }

    }

    public class LevelZeroControls : GridModelBase
    {
        [GridColumnAttributes("Control Type", ColumnType.DropDown, 1, true, false)]
        public string LevelZeroControl { get; set; }
    }
}
