using NLog;

namespace puka;

static class ConfigLogger
{

	public static void ToDirectory(string directoryPath)
	{
		var config = new NLog.Config.LoggingConfiguration();

		// Targets where to log to: File and Console
		var logInfo = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath,"info.log") };
		var logDebug = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath,"debug.log")  };
		var logWarn = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "warn.log") };
		var logError = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "error.log") };
		var logFatal = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "fatal.log") };
		var logTrace = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(directoryPath, "trace.log") };

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
