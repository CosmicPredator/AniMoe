using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AniMoe.App.Dialogs.ActivityEditorDialogControls
{
    public sealed partial class EditorPage : UserControl
    {
        public EditorPage()
        {
            this.InitializeComponent();
        }

        public string GetActivityMessage()
        {
            return ActivitytextBox.Text;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            switch (appBarButton.Label)
            {
                case "Bold":
                    ActivitytextBox.Text += "____";
                    break;
                case "Italic":
                    ActivitytextBox.Text += "__";
                    break;
                case "Strike":
                    ActivitytextBox.Text += "~~~~";
                    break;
                case "Spoiler":
                    ActivitytextBox.Text += "~!!~";
                    break;
                case "Link":
                    ActivitytextBox.Text += "[link]()";
                    break;
                case "Image":
                    ActivitytextBox.Text += "img220()";
                    break;
                case "YouTube":
                    ActivitytextBox.Text += "youtube()";
                    break;
                case "WEBM":
                    ActivitytextBox.Text += "webm()";
                    break;
                case "List":
                    ActivitytextBox.Text += "\n1.";
                    break;
                case "Bulletin List":
                    ActivitytextBox.Text += "\n-";
                    break;
                case "Heading":
                    ActivitytextBox.Text += "\n# ";
                    break;
                case "Center":
                    ActivitytextBox.Text += "\n~~~~~~";
                    break;
                case "Quote":
                    ActivitytextBox.Text += "\n>";
                    break;
                case "Code Block":
                    ActivitytextBox.Text += "``";
                    break;
            }
            ActivitytextBox.Select(ActivitytextBox.Text.Length, 0);
        }
    }
}
