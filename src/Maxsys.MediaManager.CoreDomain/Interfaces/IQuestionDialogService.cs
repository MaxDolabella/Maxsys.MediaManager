using System;

namespace Maxsys.MediaManager.CoreDomain.Interfaces;

public interface IQuestionDialogService : IDisposable
{
    /// <summary>
    /// Specifies which message box button that a user clicks. System.Windows.MessageBoxResult
    /// is returned by the Overload:System.Windows.MessageBox.Show method.
    /// </summary>
    public enum QuestionDialogResult : byte
    {
        /// <summary>
        /// The question dialog returns no result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The result value of the question dialog is OK.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The result value of the question dialog is Cancel.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The result value of the question dialog is Yes.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The result value of the question dialog is No.
        /// </summary>
        No = 7
    }

    object Owner { get; }

    QuestionDialogResult ShowQuestion(string question, string? title = null);
}