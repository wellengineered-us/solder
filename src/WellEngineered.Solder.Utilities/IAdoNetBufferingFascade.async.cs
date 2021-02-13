/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using __IRow = System.Collections.Generic.IDictionary<string, object>;
using __Row = System.Collections.Generic.Dictionary<string, object>;

namespace WellEngineered.Solder.Utilities
{
	public partial interface IAdoNetBufferingFascade
	{
		#region Methods/Operators

		IAsyncEnumerable<__IRow> ExecuteRecordsAsync(bool schemaOnly, Type connectionType, string connectionString, bool transactional, IsolationLevel isolationLevel, CommandType commandType, string commandText, IEnumerable<DbParameter> commandParameters, int? commandTimeout = null, bool commandPrepare = false, Action<long> resultCallback = null);

		#endregion
	}
}