using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using KitaBantu.CustomElements;
using KitaBantu.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LinelessEntry), typeof(LinelessEntryRenderer))]
namespace KitaBantu.iOS
{
    public class LinelessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}