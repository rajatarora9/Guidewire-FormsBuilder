using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common.Contract;

namespace Common.Contract
{


    public static class HtmlHelperExtension
    {

        #region Public Methods

        public static Type GetCollectionType(IEnumerable collection)
        {

            Type type = collection.GetType();
            if (type.IsGenericType)
            {
                return type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Single(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
            }
            return null;
        }
        public static List<GridColumnMetaData> GetColumnsMetadata(IList collection)
        {
            //  var   collection = model as IEnumerable;

            var columnsMetadata = new List<GridColumnMetaData>();
            var parentColumnsMetadata = new List<GridColumnMetaData>();
            var subColumnsMetadata = new List<GridColumnMetaData>();

            Type modelType = GetCollectionType(collection);
            if (modelType != null)
            {
                PropertyInfo[] properties = modelType.GetProperties();

                foreach (var property in properties)
                {
                    var columnMetaData = new GridColumnMetaData();

                    var attribute = property.GetCustomAttribute(typeof(GridColumnAttributes)) as GridColumnAttributes;
                    columnMetaData.PropertyName = property.Name;
                    if(property.PropertyType == typeof(long) || property.PropertyType == typeof(int) || property.PropertyType == typeof(short))
                    {
                        columnMetaData.ColumnDataType = ColumnDataType.Integer;
                    }
                    else if(property.PropertyType == typeof(bool))
                    {
                        columnMetaData.ColumnDataType = ColumnDataType.Bool;
                    }
                    else
                    {
                        columnMetaData.ColumnDataType = ColumnDataType.None;
                    }
                    //var SecureObjectAttribute = property.GetCustomAttribute(typeof(SecureObjectAttribute)) as SecureObjectAttribute;
                    //if(SecureObjectAttribute != null)
                    //{
                    //    columnMetaData.SecureObjectKey = SecureObjectAttribute.SecurityObjectKey;
                    //}
                    if (attribute != null)
                    {

                        //ObjectToObjectMapper.TranslateObject(attribute, columnMetaData);
                        columnMetaData.AllowNegative = attribute.AllowNegative;
                        columnMetaData.ColumnType = attribute.ColumnType;
                        columnMetaData.DisplayName = attribute.DisplayName;
                        columnMetaData.SequenceNumber = attribute.SequenceNumber;
                        columnMetaData.IsVisible = attribute.IsVisible;
                        columnMetaData.ReferenceTableName = "";
                        columnMetaData.Type = attribute.GetType();
                        //columnMetaData.IsVisible = columnMetaData.ColumnType != ColumnType.None;
                        if(columnMetaData.SequenceNumber == 0)
                        {
                            columnsMetadata.Add(columnMetaData);
                        }
                        else if (property.DeclaringType.Name == "GridModelBase")
                        {
                            parentColumnsMetadata.Add(columnMetaData);
                        }
                        else
                        {
                            subColumnsMetadata.Add(columnMetaData);
                        }
                    }


                    //to check for the secure object attribute key
                    



                }
            }
            columnsMetadata.AddRange(parentColumnsMetadata.OrderBy(x=>x.SequenceNumber).ToList());
            columnsMetadata.AddRange(subColumnsMetadata.OrderBy(x => x.SequenceNumber).ToList());
            return columnsMetadata;
        }

        #endregion
    }
}