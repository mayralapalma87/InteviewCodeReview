using System;
using System.Linq;
using System.Text;

namespace InterviewCodeReview
{
    ///<summary>
    ///1) One of the specification it's missing, in the example code the ability to log in the file, console or database it's not opcional, the code log in the tree items, this should be implemented,
    ///Othewwise the ability to choose which type of log be able to selected it's not implemented either. 
    ///I added some comments in the lines to best practice and compilation errors.
    ///</summary>
    //2)
    public class JobLogger
    {
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logToDatabase; //It's a best practice follow the line in names of the properties and parameters, using '_' to private variables.
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static int _loggedOption = 0; //I add this parameter to set errors (value 1) only or errors and warnings (value 2), the default value it´s 0 to log the 3 options. 

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError , int loggedOption)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            _logToDatabase = logToDatabase; 
            _logToFile = logToFile;
            _logToConsole = logToConsole;
            _loggedOption = loggedOption;
        }

        public static void LogMessage(string message, bool messageFlag, bool warning, bool error, int loggedOption)  //The class can't have more than once parameter with the same name, so string message and bool message can't named like that.  
        {
            message.Trim();
            //Verified if the data it's complete.
            if (message == null || message.Length == 0)
            {
                throw new Exception("Message must have a description.");
            }
            if (!_logToConsole && !_logToFile && !_logToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if ((!_logError && !_logMessage && !_logWarning) || (!messageFlag && !warning && !error))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }
            if (!(loggedOption == 0 || loggedOption == 1 || loggedOption == 2))
            {
                throw new Exception("Invalid configuration to log opcion");
            }
            //Set the type of message.
            int type = 0;
            if (messageFlag && _logMessage)
            {
                type = 1;
            }
            if (error && _logError)
            {
                type = 2;
            }
            if (warning && _logWarning)
            {
                type = 3;
            }
            //Create a log string to set the complete message with the date.
            string log = "";
            switch (loggedOption){
                case 0 : 
                    if (error && _logError) log += DateTime.Now.ToShortDateString() + message;
                    if (warning && _logWarning) log += DateTime.Now.ToShortDateString() + message;
                    if (messageFlag && _logMessage) log += DateTime.Now.ToShortDateString() + message;
                    break;
                case 1:
                    if (error && _logError) log += DateTime.Now.ToShortDateString() + message;
                    break;
                case 2:
                     if (error && _logError) log += DateTime.Now.ToShortDateString() + message;
                     if (warning && _logWarning) log += DateTime.Now.ToShortDateString() + message;
                    break;
            }
            //Set where should be shown the message.
            if (_logToDatabase)
            {//Write in the Database
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                connection.Open();
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + log + "', '" + type.ToString() + "')"); //this  will only save the last type of log
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (_logToFile)
            {//Write in a File                
                if (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
                {
                    log += System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
                }
                System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", log);               
            }
            if (_logToConsole)
            { 
                //Set the color
                if (error && _logError) 
                    Console.ForegroundColor = ConsoleColor.Red;
                if (warning && _logWarning) 
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (messageFlag && _logMessage) 
                    Console.ForegroundColor = ConsoleColor.White;
                //Write in the console
                Console.WriteLine(DateTime.Now.ToShortDateString() + log);
            }
        }
        static void Main()
        {
        }
    }
}
