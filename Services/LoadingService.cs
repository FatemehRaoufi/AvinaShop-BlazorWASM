
namespace AvinaShop.Services
{
    // Services/LoadingService.cs
    public class LoadingService
    {
        public event Action? OnChange;
        public bool IsLoading { get; private set; }

        public void Show()
        {
            IsLoading = true;
            NotifyStateChanged();
        }

        public void Hide()
        {
            IsLoading = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
