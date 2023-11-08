// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace VisionAnalyzer
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField FileLabel { get; set; }

		[Outlet]
		AppKit.NSImageView ImageFrame { get; set; }

		[Outlet]
		AppKit.NSPopUpButton JointMenu { get; set; }

		[Outlet]
		AppKit.NSTextFieldCell PositionVectorLabel { get; set; }

		[Action ("OnGetCameraPosition:")]
		partial void OnGetCameraPosition (Foundation.NSObject sender);

		[Action ("UploadButton:")]
		partial void UploadButton (Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (FileLabel != null) {
				FileLabel.Dispose ();
				FileLabel = null;
			}

			if (ImageFrame != null) {
				ImageFrame.Dispose ();
				ImageFrame = null;
			}

			if (PositionVectorLabel != null) {
				PositionVectorLabel.Dispose ();
				PositionVectorLabel = null;
			}

			if (JointMenu != null) {
				JointMenu.Dispose ();
				JointMenu = null;
			}

		}
	}
}
