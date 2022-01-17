using Foundation;
using System;
using UIKit;

namespace OfflineSyncSample
{
    public partial class ViewController : UIViewController
    {

        private AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;

        public ViewController(IntPtr handle) : base(handle)
        {

            /*btnDownload.TouchUpInside += (sender, e) => {
                ititiateDownload();
            };

            btnUpload.TouchUpInside += (sender, e) => {
                ititiateUpload();
            };*/
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void onDownloadClick(Foundation.NSObject sender)
        {
            var result = appDelegate.SyncManager.NewDownloadTask();
        }

        partial void onUploadClick(Foundation.NSObject sender)
        {
            var result = appDelegate.SyncManager.NewUploadTask();
        }

        partial void onDataTask(Foundation.NSObject sender)
        {
            var result = appDelegate.SyncManager.NewDataTask();
        }

        partial void onHTTPClientTask(Foundation.NSObject sender)
        {
            var result = appDelegate.SyncManager.NewHTTPClientTask();
        }
    }
}
