// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace OfflineSyncSample
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton btnDataUpload { get; set; }

		[Outlet]
		UIKit.UIButton btnDownload { get; set; }

		[Outlet]
		UIKit.UIButton btnHttpClient { get; set; }

		[Outlet]
		UIKit.UIButton btnUpload { get; set; }

		[Action ("onDataTask:")]
		partial void onDataTask (Foundation.NSObject sender);

		[Action ("onDownloadClick:")]
		partial void onDownloadClick (Foundation.NSObject sender);

		[Action ("onHTTPClientTask:")]
		partial void onHTTPClientTask (Foundation.NSObject sender);

		[Action ("onUploadClick:")]
		partial void onUploadClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnDownload != null) {
				btnDownload.Dispose ();
				btnDownload = null;
			}

			if (btnUpload != null) {
				btnUpload.Dispose ();
				btnUpload = null;
			}

			if (btnDataUpload != null) {
				btnDataUpload.Dispose ();
				btnDataUpload = null;
			}

			if (btnHttpClient != null) {
				btnHttpClient.Dispose ();
				btnHttpClient = null;
			}
		}
	}
}
