using System;
using System.Collections.Generic;
using System.Data;
using GDMInterfaces;

namespace GDMCore
{
    /// <summary>
    /// Implements the IModel interface, the API to the plug-in developers. Provides methods to update and manipulate the dataset containing the data tables and to get information of the current state.
    /// </summary>
    public class Model : IModel
    {
        private DataTable _selectedTable;
        private string _selectedColumnName;
        private Type _selectedColumnType;
        private DataSet _dataset;

        public DataSet DataSet
        {
            get { return _dataset; }
            set { _dataset = value; }
        }

        public Model()
        {
            _dataset = new DataSet("GDMTool");
        }

        public DataTable SelectedTable
        {
            get { return _selectedTable; }
            set { _selectedTable = value; }
        }
        public ISet<string> GetAllTableNames()
        {
            var tables = new SortedSet<String>();
            foreach (DataTable t in DataSet.Tables)
            {
                tables.Add(t.TableName);
            }
            return tables;
        }

        public string SelectedColumnName
        {
            get { return _selectedColumnName; }
            set { _selectedColumnName = value; }
        }

        public Type SelectedColumnType
        {
            get { return _selectedColumnType; }
            set { _selectedColumnType = value; }
        }

        public void DropTable(string table)
        {
            _dataset.Tables.Remove(table);
            if (SelectedTable==null || SelectedTable.TableName == table)
                SelectedTable = null;
        }

        public DataTable CreateTable()
        {
            string tableName = AllocateTableName();
            DataTable datatable = new DataTable(tableName);
            _dataset.Tables.Add(datatable);
            return datatable;
        }

        public List<DataColumn> GetColumns(string table)
        {
            List<DataColumn> columns = new List<DataColumn>();

            if (_dataset.Tables.Contains(table))
            {
                foreach (DataColumn d in _dataset.Tables[table].Columns)
                {
                    columns.Add(d);
                }
            }

            return columns;
        }

        public DataTable GetTable(string table)
        {
            return _dataset.Tables[table];
        }

        public List<DataTable> GetTables()
        {
            List<DataTable> list = new List<DataTable>();

            if (_dataset.Tables.Count > 0)
            {
                foreach (DataTable dt in _dataset.Tables)
                {
                    list.Add(dt);
                }
                
            }

            return list;
        }

        public void AddTable(DataTable table)
        {
            if (_dataset.Tables.Contains(table.TableName))
            {
                table.TableName = AllocateTableName();
            }

            _dataset.Tables.Add(table);
        }

        // Returns the lowest available table name in order to avoid name collisions when adding a table to the set
        private string AllocateTableName()
        {
            string pre = "Table"; int i = 1;

            while (_dataset.Tables.Contains(pre + i)) i++;

            return pre + i;

            //while(true)
            //{
            //    if (!this.usedTableNames.Contains(pre+i))
            //    {
            //        this.usedTableNames.Add(pre+i);
            //        return pre+i;
            //    }
            //    i++;
            //}
        }

        public void AddRelation(DataRelation relation)
        {
            _dataset.Relations.Add(relation);
        }

        internal void ResetData()
        {
            _dataset.Relations.Clear();
            foreach (DataTable d in _dataset.Tables) d.Constraints.Clear();
            _dataset.Tables.Clear();
            SelectedColumnType = null;
            SelectedColumnName = null;
            SelectedTable = null;
            _dataset.Reset();
        }
    }
}