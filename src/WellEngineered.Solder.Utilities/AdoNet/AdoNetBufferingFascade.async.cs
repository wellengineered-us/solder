/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Primitives;

using __IRow = System.Collections.Generic.IDictionary<string, object>;
using __Row = System.Collections.Generic.Dictionary<string, object>;

namespace WellEngineered.Solder.Utilities.AdoNet
{
	public sealed partial class AdoNetBufferingFacade
	{
		#region Methods/Operators

		public async ValueTask<DbParameter> CreateParameterAsync(Type connectionType, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue, CancellationToken cancellationToken = default)
		{
			DbParameter dbParameter;

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			await Task.CompletedTask;

			using (DbConnection dbConnection = (DbConnection)Activator.CreateInstance(connectionType))
			{
				using (DbCommand dbCommand = dbConnection.CreateCommand())
					dbParameter = dbCommand.CreateParameter();

				dbParameter.ParameterName = parameterName;
				dbParameter.Size = parameterSize;
				dbParameter.Value = parameterValue;
				dbParameter.Direction = parameterDirection;
				dbParameter.DbType = parameterDbType;
				dbParameter.IsNullable = parameterNullable;
				dbParameter.Precision = parameterPrecision;
				dbParameter.Scale = parameterScale;
				dbParameter.SourceColumn = sourceColumn;

				return dbParameter;
			}
		}

		public async IAsyncEnumerable<__IRow> ExecuteRecordsAsync(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, int? commandTimeout = null, bool commandPrepare = false, Func<long, ValueTask> resultCallbackAsync = null, CancellationToken cancellationToken = default)
		{
			DbTransaction dbTransaction;
			const bool OPEN = true;

			//IList<__Row> rows;

			CommandBehavior commandBehavior;
			long resultIndex = 0;

			ReadOnlyCollection<DbColumn> dbColumns;
			DbColumn dbColumn;
			PropertyInfo[] propertyInfos;
			PropertyInfo propertyInfo;

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			if ((object)connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			using (DbConnection dbConnection = (DbConnection)Activator.CreateInstance(connectionType))
			{
				if (OPEN)
				{
					dbConnection.ConnectionString = connectionString;
					await dbConnection.OpenAsync();

					if (transactional)
						dbTransaction = await dbConnection.BeginTransactionAsync(isolationLevel);
					else
						dbTransaction = null;
				}

				using (DbCommand dbCommand = dbConnection.CreateCommand())
				{
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = commandType;
					dbCommand.CommandText = commandText;

					if ((object)commandTimeout != null)
						dbCommand.CommandTimeout = (int)commandTimeout;

					// add parameters
					if ((object)commandParameters != null)
					{
						foreach (DbParameter commandParameter in commandParameters)
						{
							if ((object)commandParameter.Value == null)
								commandParameter.Value = DBNull.Value;

							dbCommand.Parameters.Add(commandParameter);
						}
					}

					if (commandPrepare)
						await dbCommand.PrepareAsync();

					//rows = new List<__Row>();

					commandBehavior = schemaOnly ? CommandBehavior.SchemaOnly : CommandBehavior.Default;

					using (DbDataReader dbDataReader = await dbCommand.ExecuteReaderAsync(commandBehavior))
					{
						__Row row;
						string key;
						object value;

						if (!schemaOnly)
						{
							do
							{
								if ((object)resultCallbackAsync != null)
									await resultCallbackAsync(resultIndex++);

								while (await dbDataReader.ReadAsync())
								{
									row = new __Row(StringComparer.OrdinalIgnoreCase);

									for (int fieldIndex = 0; fieldIndex < dbDataReader.FieldCount; fieldIndex++)
									{
										key = dbDataReader.GetName(fieldIndex);
										value = dbDataReader.GetValue(fieldIndex);
										value = value.ChangeType<object>();

										if (row.ContainsKey(key) || string.IsNullOrEmpty(key))
											key = string.Format("Field_{0:0000}", fieldIndex);

										row.Add(key, value);
									}

									//rows.Add(row);
									yield return row;
								}
							}
							while (await dbDataReader.NextResultAsync());
						}
						else
						{
							if (!dbDataReader.CanGetColumnSchema())
								throw new NotSupportedException(string.Format("The connection command type '{0}' does not support schema access.", dbDataReader.GetType().FullName));

							dbColumns = dbDataReader.GetColumnSchema();
							{
								if ((object)dbColumns != null)
								{
									for (long recordIndex = 0; recordIndex < dbColumns.Count; recordIndex++)
									{
										dbColumn = dbColumns[(int)recordIndex];

										propertyInfos = dbColumn.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

										row = new __Row(StringComparer.OrdinalIgnoreCase);
										row.Add(string.Empty, dbColumn);

										if ((object)propertyInfos != null)
										{
											for (int fieldIndex = 0; fieldIndex < propertyInfos.Length; fieldIndex++)
											{
												propertyInfo = propertyInfos[fieldIndex];

												if (propertyInfo.GetIndexParameters().Any())
													continue;

												key = propertyInfo.Name;
												value = propertyInfo.GetValue(dbColumn);
												value = value.ChangeType<object>();

												row.Add(key, value);
											}
										}

										yield return row;
									}
								}
							}
						}
					}

					//return rows;
				}
			}
		}

		#endregion
	}
}
#endif