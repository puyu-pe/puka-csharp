using NLog;

namespace puka;

static class ConfigLogger
{

	public static void Load()
	{
		string directoryPath = Path.GetTempPath();
		var config = new NLog.Config.LoggingConfiguration();
		// Targets where to log to: File and Console
		var logInfo = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath,"puka_info.log") };
		var logDebug = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath,"puka_debug.log")  };
		var logWarn = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_warn.log") };
		var logError = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_error.log") };
		var logFatal = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_fatal.log") };
		var logTrace = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "puka_trace.log") };

		// Rules for mapping loggers to targets            
		config.AddRule(LogLevel.Info, LogLevel.Info, logInfo);
		config.AddRule(LogLevel.Debug, LogLevel.Debug, logDebug);
		config.AddRule(LogLevel.Warn, LogLevel.Warn, logWarn);
		config.AddRule(LogLevel.Error, LogLevel.Error, logError);
		config.AddRule(LogLevel.Fatal, LogLevel.Fatal, logFatal);
		config.AddRule(LogLevel.Trace, LogLevel.Trace, logTrace);

		// Apply config           
		LogManager.Configuration = config;
	}
}
