/*
	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.Solder.Executive
{
	public enum Signal
	{
		SignalNone = 0,
		SignalKill = 1,
		SignalInt = 2,
		SignalQuit = 3,
		SignalTerm = 4,
		SignalEnvExit = 999,

		ControlC = SignalInt,
		ControlBreak = SignalQuit,
		ControlZ = SignalKill,
		DockerStop = SignalTerm,
		EnvExit = SignalEnvExit,
	}
}