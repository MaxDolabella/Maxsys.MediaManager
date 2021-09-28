namespace Maxsys.MediaManager.CoreDomain.Interfaces
{
    /// <summary>
    /// Provides an interface to set and close the main content of a window.<br/>
    /// Inherits from <see cref="IMainContentCloser"/>.
    /// </summary>
    public interface IMainContentOwner : IMainContentCloser
    {
        void SetMainContent(object content);
    }
}