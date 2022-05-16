/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using __IRow = System.Collections.Generic.IDictionary<string, object>;

namespace WellEngineered.Solder.Utilities.AdoNet
{
	public partial interface IAdoNetBufferingFacade
	{
		#region Methods/Operators

		DbParameter CreateParameter(Type connectionType, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue);

		IEnumerable<__IRow> ExecuteRecords(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, int? commandTimeout = null, bool commandPrepare = false, Action<long> resultCallback = null);

		#endregion
	}
}