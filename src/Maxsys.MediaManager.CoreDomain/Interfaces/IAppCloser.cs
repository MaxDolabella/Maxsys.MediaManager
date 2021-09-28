namespace Maxsys.MediaManager.CoreDomain.Interfaces
{
    /// <summary>
    /// Provides an interface to close the application.
    /// </summary>
    public interface IAppCloser
    {
        /// <summary>
        /// Closes the application.
        /// </summary>
        void CloseApp();
    }
}