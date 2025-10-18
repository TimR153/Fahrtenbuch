using MudBlazor;

namespace Fahrtenbuch.Shared.Layout
{
    public partial class MainLayout
    {
        private bool _isDarkMode = true;
        private MudThemeProvider? _mudThemeProvider;


        MudTheme _theme = new MudTheme()
        {

            PaletteLight = new PaletteLight()
            {
                Primary = MudBlazor.Colors.Blue.Darken2,
                Secondary = MudBlazor.Colors.Blue.Accent2,
                AppbarBackground = MudBlazor.Colors.Blue.Darken4,
                DrawerBackground = MudBlazor.Colors.Gray.Lighten1,
                Background = MudBlazor.Colors.Gray.Lighten2,
                DrawerText = MudBlazor.Colors.Gray.Darken4
            },
            PaletteDark = new PaletteDark()
            {
                Primary = MudBlazor.Colors.Blue.Darken2,
                Secondary = MudBlazor.Colors.Blue.Accent4,
                TextPrimary = MudBlazor.Colors.Gray.Lighten3,
                DrawerText = MudBlazor.Colors.Gray.Lighten2
            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px"
            }
        };

        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isDarkMode = await _mudThemeProvider.GetSystemDarkModeAsync();
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {

        }

        public string DarkLightModeButtonIcon => _isDarkMode switch
        {
            true => Icons.Material.Rounded.LightMode,
            false => Icons.Material.Outlined.DarkMode,
        };

        private async Task DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;
        }
    }
}