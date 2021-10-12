using System.Windows.Input;

namespace System.Windows.Controls
{
    public class DropableImage : Image
    {
        public DropableImage()
        {
            AllowDrop = true;
            Drop += OnDrop;
        }

        ~DropableImage()
        {
            Drop -= OnDrop;
        }

        #region Drag and Drop

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(AlbumCoverDropCommandProperty); }
            set { SetValue(AlbumCoverDropCommandProperty, value); }
        }

        public static readonly DependencyProperty AlbumCoverDropCommandProperty =
            DependencyProperty.Register(nameof(DropCommand),
                typeof(ICommand), typeof(DropableImage), new PropertyMetadata(null));

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (DropCommand?.CanExecute(filePath) ?? false)
                    DropCommand?.Execute(filePath);
            }
        }

        #endregion Drag and Drop
    }
}