using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class ExtendedDataTable: DataTable
     {
       public  List<object> toList() {
            List<object> list = new List<object>();
            foreach (DataRow dr in this.Rows)
               {
                    list.Add(dr.Table);
               }
            return list;
        }

        public  List<T> toList<T>()
        {
               DataTable table = this;
                List<T> list = new List<T>();

                T item ;
                Type listItemType = typeof(T);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    item = (T) Activator.CreateInstance(listItemType);
                    mapRow(item, table, listItemType, i);
                    list.Add(item);
                }
            return list;
        }
        private static void mapRow(object vOb, System.Data.DataTable table, Type type, int row)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var columnName = table.Columns[col].ColumnName;
                var prop = type.GetProperty(columnName);
                object data = table.Rows[row][col];
                prop.SetValue(vOb, data+"", null);
            }
        }

     }
}