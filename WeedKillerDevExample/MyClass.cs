using System;
using System.Collections.Generic;
using System.Text;
using BOG.WeedKiller;  // Assembly: 

//  Copyright (c) 2009-2016, John J Schultz, all rights reserved.

namespace WeedKillerDevExample
{
    // This is an example of how to implement the Weed Killer within your own class.

    public class MyClass
    {
        bool WantEvents = true;

        // Implements the WeedKillerEventHandler, which gives progress reports as files and folders are processed.
        // The WeedKiller class' worker method will always complete, even when it is denied access to files/folders.  
        // This event handler is completely optional in your application or service. If you need to log all or part 
        // of Weed Killer's activity, this is how you would capture it.
        static void WeedKillerEventProcessor(object sender, WeedKillerEventArgs e)
        {
            // this just demonstrates a way of grouping particular action types, so that you
            // can dial-up or dial-down the chatter in what your code log.  In this example,
            // 0 is the quietest, 3 is the noisiest.
            int RequiredLoggingLevel = -1;
            switch (e.Action)
            {
                case WeedKillerEventArgs.WeedKillerActionType.AgedFileRemoved:
                case WeedKillerEventArgs.WeedKillerActionType.MaxCountFileRemoved:
                    RequiredLoggingLevel = 0;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.MinCountFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.ZeroLengthFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.FreshFileSpared:
                case WeedKillerEventArgs.WeedKillerActionType.ReadOnlyFileSpared:
                    RequiredLoggingLevel = 1;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.ReadOnlyDirectorySpared:
                case WeedKillerEventArgs.WeedKillerActionType.RootDirectoryExcluded:
                case WeedKillerEventArgs.WeedKillerActionType.EmptyDirectoryRemoved:
                    RequiredLoggingLevel = 2;
                    break;

                case WeedKillerEventArgs.WeedKillerActionType.DirectoryNoMatch:
                case WeedKillerEventArgs.WeedKillerActionType.FileNoMatch:
                case WeedKillerEventArgs.WeedKillerActionType.AccessDenied:
                case WeedKillerEventArgs.WeedKillerActionType.UnhandledError:
                    RequiredLoggingLevel = 3;
                    break;
            }
            if (RequiredLoggingLevel > 2)
            {
                // Where do you want your properties to go today?  You decide, but here is what you have.
                string s;
                s = Enum.GetName(typeof(WeedKillerEventArgs.WeedKillerActionType), e.Action);
                s = e.Success ? "Success" : "FAIL";
                s = e.TestMode ? "Testing" : "Actual";
                s = e.Message;
                s = e.Path;
                s = e.FileName;
                s = e.TimeStamp.ToString();
                s = e.Size.ToString();
                s = e.Size_Recovered.ToString();
            }
        }

        public void DoWork()
        {
            WeedKillerConfig config = new WeedKillerConfig();

            config.Description = "not needed in local applications";
            config.Enabled = true;
            config.RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);

            // for regular expressions
            config.FilePattern = @"domain_.+\.cookie";  // would match the pattern domain_*.cookie
            config.FilePattern_Evaluation = WeedKillerConfig.ExpressionEvaluation.RegularExpression;
            // for legacy wildcard patterns
            config.FilePattern = @"domain_*.cookie";  // would match the pattern domain_*.cookie
            config.FilePattern_Evaluation = WeedKillerConfig.ExpressionEvaluation.Wildcards;

            // for regular expressions
            config.SubFolderPattern = ".+";
            config.FilePattern_Evaluation = WeedKillerConfig.ExpressionEvaluation.RegularExpression;
            // for legacy wildcard patterns
            config.SubFolderPattern = "*.*";
            config.FilePattern_Evaluation = WeedKillerConfig.ExpressionEvaluation.Wildcards;

            config.FileEval = WeedKillerConfig.FileDateEvaluation.Created;
            config.AgeMetric = 30;
            config.AgeMeasureUnit = WeedKillerConfig.AgeUnitOfMeasure.Days;
            config.ServerList = string.Empty;

            config.Aggressive = true;
            config.IgnoreZeroLength = false;
            config.MinimumRetentionCount = 1;
            config.MaximumRetentionCount = 2000;
            config.RecurseSubFolders = true;
            config.SubFoldersOnly = false;
            config.RemoveEmptyFolders = false;
            config.TestOnly = true;

            WeedKiller worker = new WeedKiller();

            try
            {
                if (WantEvents)
                {
                    worker.WeedKillerEvent += new WeedKillerEventHandler(WeedKillerEventProcessor);
                }
                worker.KillWeeds(config);
            }
            catch
            {
                // handle your exception here.
            }
            finally
            {
                try
                {
                    if (WantEvents)
                    {
                        worker.WeedKillerEvent -= new WeedKillerEventHandler(WeedKillerEventProcessor);
                    }
                }
                finally
                {
                }
            }
        }
    }
}
