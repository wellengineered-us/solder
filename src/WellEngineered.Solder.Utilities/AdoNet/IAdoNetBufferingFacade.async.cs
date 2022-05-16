/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

#if ASYNC_ALL_THE_WAY_DOWN
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

using __IRow = System.Collections.Generic.IDictionary<string, object>;

namespace WellEngineered.Solder.Utilities.AdoNet
{
	public partial interface IAdoNetBufferingFacade
	{
		#region Methods/Operators

		ValueTask<DbParameter> CreateParameterAsync(Type connectionType, string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue, CancellationToken cancellationToken = default);

		IAsyncEnumerable<__IRow> ExecuteRecordsAsync(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, int? commandTimeout = null, bool commandPrepare = false, Func<long, ValueTask> resultCallbackAsync = null, CancellationToken cancellationToken = default);

		#endregion
	}
}
#endif