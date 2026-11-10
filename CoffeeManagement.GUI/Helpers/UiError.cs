using System;
using System.Windows.Forms;
using CoffeeManagement.BUS.Exceptions;

namespace CoffeeManagement.GUI.Helpers
{
    internal static class UiError
    {
        public static void Show(Exception exception, IWin32Window owner = null)
        {
            AppLogger.Error(exception);
            string message = exception is BusinessException || exception is DatabaseConnectionException
                ? exception.Message + Environment.NewLine + Environment.NewLine + "The original exception details were saved to the local application log."
                :
                exception.GetType().Name == "DataAccessException" ? "The database operation failed. Verify that SQL Server is running, the CoffeeManagementDB database exists, and the connection settings in App.config are correct. See the application log for details." :
                "Something went wrong. Details were saved to the local application log.";
            MessageBox.Show(owner, message, "CoffeeManagement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
