using NLog;

namespace puka.util;

static class ConfigLogger
{

	public static void Load()
	{
		string directoryPath = Path.GetTempPath();
		var config = new NLog.Config.LoggingConfiguration();

#if DEBUG
		var logDebug = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_debug.log") };
		var logTrace = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_trace.log") };
		config.AddRule(LogLevel.Debug, LogLevel.Debug, logDebug);
		config.AddRule(LogLevel.Trace, LogLevel.Trace, logTrace);
#endif

		var logInfo = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_info.log") };
		var logError = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_error.log") };
		var logWarn = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_warn.log") };
		var logFatal = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_fatal.log") };
		config.AddRule(LogLevel.Info, LogLevel.Info, logInfo);
		config.AddRule(LogLevel.Warn, LogLevel.Warn, logWarn);
		config.AddRule(LogLevel.Error, LogLevel.Error, logError);
		config.AddRule(LogLevel.Fatal, LogLevel.Fatal, logFatal);

		LogManager.Configuration = config;
	}
}
