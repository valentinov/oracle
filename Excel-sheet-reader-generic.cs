        public List<T> ReadExcelSheet<T>(string filePath) where T : new()
        {
            List<T> result = new List<T>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Assuming the first row contains headers
                        }
                    });

                    var dataTable = dataSet.Tables[0];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        T item = new T();

                        // Map properties based on column index or name
                        PropertyInfo[] properties = typeof(T).GetProperties();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            PropertyInfo property = properties[i];
                            string columnName = property.Name; // Assume column name matches property name

                            if (dataTable.Columns.Contains(columnName))
                            {
                                object value = row[columnName];
                                property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                            }
                        }

                        result.Add(item);
                    }
                }
            }

            return result;
        }
